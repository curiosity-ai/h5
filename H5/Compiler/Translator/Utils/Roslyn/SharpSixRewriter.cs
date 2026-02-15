using H5.Contract;
using H5.Contract.Constants;
using MessagePack;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Mosaik.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UID;
using ZLogger;
using LanguageVersion = Microsoft.CodeAnalysis.CSharp.LanguageVersion;

namespace H5.Translator
{

    [MessagePackObject(keyAsPropertyName:true)]
    public class SharpSixRewriterCachedOutput
    {
        public ConcurrentDictionary<string, (UID128 hash, string code)> CachedCompilation { get; set; } = new ConcurrentDictionary<string, (UID128 hash, string code)>();
        
        public UID128 ConfigHash { get; set; }

        public void ClearIfConfigHashChanged(UID128 previousConfigHash, bool force = false)
        {
            if (ConfigHash != previousConfigHash || force)
            {
                ConfigHash = previousConfigHash;
                CachedCompilation.Clear();
            }
        }
    }

    public class SharpSixRewriter : CSharpSyntaxRewriter
    {
        private static ILogger Logger = ApplicationLogging.CreateLogger<SharpSixRewriter>();

        public const string AutoInitFieldPrefix = "__Property__Initializer__";
        private const string SYSTEM_IDENTIFIER = "System";
        private const string FUNC_IDENTIFIER = "Func";

        public readonly string envnl = Environment.NewLine;
        private readonly ITranslator translator;
        private CSharpCompilation compilation;
        private SemanticModel semanticModel;
        private List<MemberDeclarationSyntax> fields;
        private int tempKeyCounter = 0;
        private Stack<ITypeSymbol> currentType;
        private bool hasStaticUsingOrAliases;
        private bool hasChainingAssigment;
        private bool hasIsPattern;
        private bool hasCasePatternSwitchLabel;
        private bool hasLocalFunctions;
        internal List<string> usingStaticNames;

        private SharpSixRewriterCachedOutput _cachedRewrittenData;
        private bool isParent;

        public SharpSixRewriter(ITranslator translator) : base(visitIntoStructuredTrivia: true)
        {
            this.translator = translator;
            compilation = CreateCompilation();
            isParent = true;
            _cachedRewrittenData = LoadCache();

            //RFO: It's an assumption that only the config affects the end-result. Need to test what else could possibly affect it, and ignore if anything changed.
            //     THERE IS ALSO A POSSIBLE PROBLEMATIC ISSUE WITH THE ORDER THAT TYPES METHODS ARE EMITTED.
            //     Example: ctor vs. ctor$1, which will break this assumption and lead to bad-code being emitted when reusing cached code

            var configHash = JsonConvert.SerializeObject(translator.AssemblyInfo).Hash128();

            foreach (var reference in translator.References.OrderBy(r => r.MainModule.FileName))
            {
                var fi = new FileInfo(reference.MainModule.FileName);
                configHash = Hashes.Combine(configHash, reference.FullName.Hash128(), $"{fi.Length}/{fi.LastWriteTime}".Hash128());
            }

            var context = translator.GetVersionContext();

            configHash = Hashes.Combine(configHash, $"{context.Compiler.Version}/{context.H5.Version}".Hash128());

            _cachedRewrittenData.ClearIfConfigHashChanged(configHash, force: !translator.AssemblyInfo.EnableCache);
        }

        public SharpSixRewriter Clone()
        {
            return new SharpSixRewriter(this);
        }

        private SharpSixRewriter(SharpSixRewriter rewriter) : base(visitIntoStructuredTrivia: true)
        {
            translator = rewriter.translator;
            compilation = rewriter.compilation;
            _cachedRewrittenData = rewriter._cachedRewrittenData;
        }


        private string GetCacheFile()
        {
            return translator.AssemblyLocation.Replace(@"\bin\", @"\obj\").Replace(@"/bin/", @"/obj/") + ".h5.rewriter.cache";
        }

        public SharpSixRewriterCachedOutput LoadCache()
        {
            if (!isParent) throw new InvalidOperationException("Can only be called on parent Rewriter");

            var cf = GetCacheFile();
            
            Directory.CreateDirectory(Path.GetDirectoryName(cf));

            if (File.Exists(cf))
            {
                try
                {
                    using(var f = File.OpenRead(cf))
                    {
                        var cached =  MessagePackSerializer.Deserialize<SharpSixRewriterCachedOutput>(f);

                        foreach(var key in cached.CachedCompilation.Keys.Except(translator.SourceFiles).ToArray())
                        {
                            cached.CachedCompilation.TryRemove(key, out _);
                        }
                        return cached;
                    }
                }
                catch
                {
                    Logger.ZLogError("Error reading cache file '{0}', ignoring cache", cf);
                }
            }

            return new SharpSixRewriterCachedOutput();
        }

        public void CommitCache()
        {
            if (!isParent) throw new InvalidOperationException("Can only be called on parent Rewriter");
            using(var f = File.OpenWrite(GetCacheFile()))
            {
                f.SetLength(0);
                MessagePackSerializer.Serialize(f, _cachedRewrittenData);
                f.Flush();
                f.Close();
            }
        }

        private bool TryGetFromCache(int index, out string cached)
        {
#if DEBUG
            cached = null;
            return false;
#endif
            var fileName = translator.SourceFiles[index];
            var hashedSource = File.ReadAllText(fileName).Hash128();

            if(_cachedRewrittenData.CachedCompilation.TryGetValue(fileName, out var cachedData) && cachedData.hash == hashedSource)
            {
                cached = cachedData.code;
                return true;
            }
            else
            {
                cached = null;
                return false;
            }
        }

        private string AddToCache(int index, string rewritten)
        {
            //TODO: use https://www.nuget.org/packages/CSharpMinifier/
            var fileName = translator.SourceFiles[index];
            var hashedSource = File.ReadAllText(fileName).Hash128();

            _cachedRewrittenData.CachedCompilation[fileName] = (hashedSource, rewritten);

            return rewritten;
        }


        public string Rewrite(int index)
        {
            if(TryGetFromCache(index, out var cached))
            {
                return cached;
            }

            tempKeyCounter = 0;
            currentType = new Stack<ITypeSymbol>();
            usingStaticNames = new List<string>();

            var syntaxTree = compilation.SyntaxTrees[index];
            semanticModel = compilation.GetSemanticModel(syntaxTree, true);

            SyntaxTree newTree = null;

            Func<SyntaxNode, Tuple<SyntaxTree, SemanticModel>> modelUpdater = (root) => {
                newTree = SyntaxFactory.SyntaxTree(root, GetParseOptions());
                compilation = compilation.ReplaceSyntaxTree(syntaxTree, newTree);
                syntaxTree = newTree;
                semanticModel = compilation.GetSemanticModel(newTree, true);
                return new Tuple<SyntaxTree, SemanticModel>(newTree, semanticModel);
            };

            var result = new ExpressionBodyToStatementRewriter(semanticModel).Visit(syntaxTree.GetRoot());
            modelUpdater(result);

            result = new NameofReplacer(semanticModel).Visit(syntaxTree.GetRoot());
            modelUpdater(result);

            result = new DiscardReplacer().Replace(syntaxTree.GetRoot(), semanticModel, modelUpdater, this);
            modelUpdater(result);

            result = new DeconstructionReplacer().Replace(syntaxTree.GetRoot(), semanticModel, modelUpdater, this);
            modelUpdater(result);

            result = Visit(syntaxTree.GetRoot());

            var replacers = new List<ICSharpReplacer>();

            if (hasLocalFunctions)
            {
                replacers.Add(new LocalFunctionReplacer());
            }

            if (hasChainingAssigment)
            {
                replacers.Add(new ChainingAssigmentReplacer());
            }

            if (hasStaticUsingOrAliases)
            {
                replacers.Add(new UsingStaticReplacer());
            }

            if (hasIsPattern)
            {
                replacers.Add(new IsPatternReplacer());
            }

            if (hasCasePatternSwitchLabel)
            {
                replacers.Add(new SwitchPatternReplacer());
            }

            foreach (var replacer in replacers)
            {
                modelUpdater(result);

                try
                {
                    result = replacer.Replace(newTree.GetRoot(), semanticModel, this);
                }
                catch (Exception e)
                {
                    Logger.ZLogError("Error trying to rewrite syntax block while parsing source file.\n Replacer: {0}\nFile:{1}\nException: {2}", replacer.ToString(), translator.SourceFiles[index] + e.Message);
                    throw new TranslatorException("Error applying replacer '" + replacer.ToString() + "' on file '" + translator.SourceFiles[index] + "'. Inner exception: " + e.Message, e);
                }
            }

            modelUpdater(result);

            return AddToCache(index, newTree.GetRoot().ToFullString());
        }

        private string GetUniqueTempKey(string prefix)
        {
            var path = semanticModel.SyntaxTree.FilePath ?? "";
            var pathHash = path.Hash128().ToString();
            return $"{prefix}_{pathHash}_{++tempKeyCounter}";
        }

        // FIXME: Same call made by H5.Translator.BuildAssembly
        // (Translator\Translator.Build.cs). Shouldn't this also be called
        // from there (so this might become public/static).
        private CSharpParseOptions GetParseOptions()
        {
            LanguageVersion languageVersion = LanguageVersion.Latest;

            if (translator?.ProjectProperties?.LanguageVersion != null)
            {
                if (string.Equals(translator.ProjectProperties.LanguageVersion, "latest", StringComparison.OrdinalIgnoreCase))
                {
                    languageVersion = LanguageVersion.Latest;
                }
                else if (Enum.TryParse<LanguageVersion>(translator.ProjectProperties.LanguageVersion.Replace(".", ""), true, out var version))
                {
                    languageVersion = version;
                }
                else if (Enum.TryParse<LanguageVersion>("CSharp" + translator.ProjectProperties.LanguageVersion.Replace(".", "_"), true, out var version2))
                {
                    languageVersion = version2;
                }
            }

            return new CSharpParseOptions(languageVersion, Microsoft.CodeAnalysis.DocumentationMode.None, SourceCodeKind.Regular, translator.DefineConstants);
        }

        private CSharpCompilation CreateCompilation()
        {
            var compilationOptions = new CSharpCompilationOptions(OutputKind.ConsoleApplication);

            var parseOptions = GetParseOptions();

            var syntaxTrees = translator.SourceFiles.Select(s => ParseSourceFile(s, parseOptions)).Where(s => s != null).ToList();
            var references = new MetadataReference[translator.References.Count()];
            var i = 0;
            foreach (var r in translator.References)
            {
                references[i++] = MetadataReference.CreateFromFile(r.MainModule.FileName, new MetadataReferenceProperties(MetadataImageKind.Assembly, ImmutableArray.Create("global")));
            }

            return CSharpCompilation.Create(GetAssemblyName(), syntaxTrees, references, compilationOptions);
        }

        private string GetAssemblyName()
        {
            if (translator.AssemblyLocation != null)
            {
                return Path.GetFileNameWithoutExtension(translator.AssemblyLocation);
            }
            else if (translator.SourceFiles.Count > 0)
            {
                return Path.GetFileNameWithoutExtension(translator.SourceFiles[0]);
            }
            else
            {
                return null;
            }
        }

        private SyntaxTree ParseSourceFile(string path, CSharpParseOptions options)
        {
            if (!File.Exists(path))
            {
                Logger.ZLogError("Source file '{0}' could not be found", path);
            }

            try
            {
                using (var rdr = new StreamReader(path))
                {
                    return SyntaxFactory.ParseSyntaxTree(rdr.ReadToEnd(), options, path);
                }
            }
            catch (IOException ex)
            {
                Logger.ZLogError("Error reading source file `{0}': {1}", path, ex.Message);
                return null;
            }
        }

        private static bool IsExpandedForm(SemanticModel semanticModel, InvocationExpressionSyntax node, IMethodSymbol method)
        {
            var parameters = method.Parameters;
            var arguments = node.ArgumentList.Arguments;

            ExpressionSyntax target = null;
            if (method.ReducedFrom != null)
            {
                var mae = (MemberAccessExpressionSyntax)node.Expression;
                target = mae.Expression;
            }

            var isReducedExtensionMethod = target != null;

            if (parameters.Length == 0 || !parameters[parameters.Length - 1].IsParams)
                return false;   // Last parameter must be params

            int actualArgumentCount = arguments.Count + (isReducedExtensionMethod ? 1 : 0);

            if (actualArgumentCount < parameters.Length - 1)
                return false;   // No default arguments are allowed

            if (arguments.Any(a => a.NameColon != null))
                return false;   // No named arguments are allowed

            if (actualArgumentCount == parameters.Length - 1)
                return true;    // Empty param array

            var lastType = semanticModel.GetTypeInfo(arguments[arguments.Count - 1].Expression).ConvertedType;
            if (SymbolEqualityComparer.Default.Equals(((IArrayTypeSymbol)parameters[parameters.Length - 1].Type).ElementType, lastType))
                return true;    // A param array needs to be created

            return false;
        }

        public override SyntaxNode VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
        {
            node =  (CaseSwitchLabelSyntax)base.VisitCaseSwitchLabel(node);

            if (node.Value is CastExpressionSyntax ce && ce.Expression.Kind() == SyntaxKind.DefaultLiteralExpression)
            {
                hasCasePatternSwitchLabel = true;
            }

            return node;
        }

        public override SyntaxNode VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitBinaryExpression(node);
            }

            var symbol = semanticModel.GetSymbolInfo(node.Right).Symbol;
            var newNode = base.VisitBinaryExpression(node);
            node = newNode as BinaryExpressionSyntax;
            if (node != null && node.OperatorToken.IsKind(SyntaxKind.IsKeyword) && !(symbol is ITypeSymbol))
            {
                //node = node.WithOperatorToken(SyntaxFactory.Token(SyntaxKind.EqualsEqualsToken));
                newNode = SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    node.Left,
                                    SyntaxFactory.IdentifierName("Equals")), SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList(
                                        SyntaxFactory.Argument(
                                            node.Right)))).NormalizeWhitespace().WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }

            return newNode;
        }

        public override SyntaxNode VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
        {
            var oldMarkAsAsync = markAsAsync;
            markAsAsync = false;

            hasLocalFunctions = true;

            if (markAsAsync && node.Modifiers.IndexOf(SyntaxKind.AsyncKeyword) == -1)
            {
                node = node.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" ")).WithLeadingTrivia(SyntaxFactory.Whitespace(" ")));
            }

            markAsAsync = oldMarkAsAsync;

            return base.VisitLocalFunctionStatement(node);
        }

        private void ThrowRefNotSupported(SyntaxNode node)
        {
            var mapped = semanticModel.SyntaxTree.GetLineSpan(node.Span);
            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "{2} - {3}({0},{1}): {4}", mapped.StartLinePosition.Line + 1, mapped.StartLinePosition.Character + 1, "Ref returns and locals are not supported", semanticModel.SyntaxTree.FilePath, node.ToString()));
        }

        public override SyntaxNode VisitRefType(RefTypeSyntax node)
        {
            node = (RefTypeSyntax)base.VisitRefType(node);

            return SyntaxFactory.GenericName(SyntaxFactory.Identifier("H5.Ref"), SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new[] { node.Type }))).NormalizeWhitespace().WithTrailingTrivia(node.GetTrailingTrivia()).WithLeadingTrivia(node.GetLeadingTrivia());
        }

        public override SyntaxNode VisitRefExpression(RefExpressionSyntax node)
        {
            var symbol = semanticModel.GetSymbolInfo(node.Expression).Symbol;
            var typeInfo = semanticModel.GetTypeInfo(node.Expression);
            var type = typeInfo.Type ?? typeInfo.ConvertedType;
            var pos = node.Expression.GetLocation().SourceSpan.Start;

            node = (RefExpressionSyntax)base.VisitRefExpression(node);

            if (symbol is ILocalSymbol ls && ls.IsRef || symbol is IMethodSymbol ms && ms.ReturnsByRef)
            {
                return node.Expression.NormalizeWhitespace().WithTrailingTrivia(node.GetTrailingTrivia()).WithLeadingTrivia(node.GetLeadingTrivia());
            }

            var createExpression = SyntaxFactory.ObjectCreationExpression(SyntaxFactory.GenericName(SyntaxFactory.Identifier("H5.Ref"), SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new []{
                SyntaxHelper.GenerateTypeSyntax(type, semanticModel, pos, this)
            })))).WithArgumentList(SyntaxFactory.ArgumentList(
                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                    new SyntaxNodeOrToken[]{
                        SyntaxFactory.Argument(
                            SyntaxFactory.ParenthesizedLambdaExpression(
                                node.Expression)
                            .WithParameterList(
                                SyntaxFactory.ParameterList()
                                .WithOpenParenToken(
                                    SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                                .WithCloseParenToken(
                                    SyntaxFactory.Token(SyntaxKind.CloseParenToken)))
                            .WithArrowToken(
                                SyntaxFactory.Token(SyntaxKind.EqualsGreaterThanToken))),
                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                        SyntaxFactory.Argument(
                            SyntaxFactory.ParenthesizedLambdaExpression(
                                SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    node.Expression,
                                    SyntaxFactory.IdentifierName("_v_"))
                                .WithOperatorToken(
                                    SyntaxFactory.Token(SyntaxKind.EqualsToken)))
                            .WithParameterList(
                                SyntaxFactory.ParameterList(
                                    SyntaxFactory.SingletonSeparatedList(
                                        SyntaxFactory.Parameter(
                                            SyntaxFactory.Identifier("_v_"))))
                                .WithOpenParenToken(
                                    SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                                .WithCloseParenToken(
                                    SyntaxFactory.Token(SyntaxKind.CloseParenToken)))
                            .WithArrowToken(
                                SyntaxFactory.Token(SyntaxKind.EqualsGreaterThanToken)))})));


            return createExpression.NormalizeWhitespace().WithTrailingTrivia(node.GetTrailingTrivia()).WithLeadingTrivia(node.GetLeadingTrivia());
        }

        public override SyntaxNode VisitRefTypeExpression(RefTypeExpressionSyntax node)
        {
            ThrowRefNotSupported(node);
            return node;
        }

        public override SyntaxNode VisitRefValueExpression(RefValueExpressionSyntax node)
        {
            ThrowRefNotSupported(node);
            return node;
        }

        private static Regex binaryLiteral = new Regex(@"[_Bb]", RegexOptions.Compiled);
        private bool markAsAsync;

        public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            if (node.Token.IsKind(SyntaxKind.SingleLineRawStringLiteralToken) ||
                node.Token.IsKind(SyntaxKind.MultiLineRawStringLiteralToken))
            {
                return SyntaxFactory.LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    SyntaxFactory.Literal(node.Token.ValueText));
            }

            var spanStart = node.SpanStart;
            var pos = node.GetLocation().SourceSpan.Start;
            node =  (LiteralExpressionSyntax)base.VisitLiteralExpression(node);

            if (node.Kind() == SyntaxKind.NumericLiteralExpression)
            {
                var text = node.Token.Text;

                if (node.Token.ValueText != node.Token.Text && binaryLiteral.Match(text).Success)
                {
                    dynamic value = node.Token.Value;

                    if (node.Token.Value is long lv && lv == long.MinValue)
                    {
                        return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("long"), SyntaxFactory.IdentifierName("MinValue"));
                    }

                    node = node.WithToken(SyntaxFactory.Literal(value));
                }
            }
            else if (node.Kind() == SyntaxKind.DefaultLiteralExpression)
            {
                var typeInfo = semanticModel.GetTypeInfo(node);
                var type = typeInfo.Type ?? typeInfo.ConvertedType;

                if (type != null && type.TypeKind != TypeKind.Error)
                {
                    return SyntaxFactory.DefaultExpression(SyntaxFactory.ParseTypeName(type.ToMinimalDisplayString(semanticModel, pos)));
                }
            }

            return node;
        }

        public override SyntaxNode VisitTupleExpression(TupleExpressionSyntax node)
        {
            if (node.Parent is AssignmentExpressionSyntax ae && ae.Left == node)
            {
                return base.VisitTupleExpression(node);
            }

            var typeInfo = semanticModel.GetTypeInfo(node);
            var type = typeInfo.Type ?? typeInfo.ConvertedType;
            ImmutableArray<IFieldSymbol> elements;
            List<TypeSyntax> types = new List<TypeSyntax>();

            if (type.IsTupleType)
            {
                elements = ((INamedTypeSymbol)type).TupleElements;
                foreach (var el in elements)
                {
                    types.Add(SyntaxHelper.GenerateTypeSyntax(el.Type, semanticModel, node.GetLocation().SourceSpan.Start, this));
                }
            }
            node = (TupleExpressionSyntax)base.VisitTupleExpression(node);

            if (type.IsTupleType)
            {
                var createExpression = SyntaxFactory.ObjectCreationExpression(SyntaxFactory.GenericName(SyntaxFactory.Identifier("System.ValueTuple"), SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(types))));
                var argExpressions = new List<ArgumentSyntax>();

                foreach (var arg in node.Arguments)
                {
                    argExpressions.Add(arg.WithNameColon(null));
                }

                createExpression = createExpression.WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(argExpressions))).NormalizeWhitespace();
                return createExpression.WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }

            return node;
        }

        public override SyntaxNode VisitTupleType(TupleTypeSyntax node)
        {
            node = (TupleTypeSyntax)base.VisitTupleType(node);

            List<TypeSyntax> types = new List<TypeSyntax>();
            foreach (var el in node.Elements)
            {
                types.Add(el.Type);
            }

            var newType = SyntaxFactory.GenericName(SyntaxFactory.Identifier("System.ValueTuple"), SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(types)));

            return newType.WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia()); ;
        }

        public override SyntaxNode VisitCasePatternSwitchLabel(CasePatternSwitchLabelSyntax node)
        {
            hasCasePatternSwitchLabel = true;
            return base.VisitCasePatternSwitchLabel(node);
        }

        public override SyntaxNode VisitIsPatternExpression(IsPatternExpressionSyntax node)
        {
            hasIsPattern = true;
            if (node.SyntaxTree == null)
            {
                // Detached node
                return base.VisitIsPatternExpression(node);
            }
            return base.VisitIsPatternExpression(node);
        }

        public override SyntaxNode VisitForEachStatement(ForEachStatementSyntax node)
        {
            if (node.AwaitKeyword.IsKind(SyntaxKind.AwaitKeyword))
            {
                markAsAsync = true;

                var expression = (ExpressionSyntax)Visit(node.Expression);
                var loopBody = (StatementSyntax)Visit(node.Statement);
                var variableType = (TypeSyntax)Visit(node.Type);
                var variableName = node.Identifier;

                var enumeratorVarName = SyntaxFactory.Identifier(GetUniqueTempKey("async_enum"));

                var getEnumeratorCall = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, SyntaxFactory.IdentifierName("GetAsyncEnumerator"))
                );

                var enumeratorDecl = SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("var"))
                    .WithVariables(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(enumeratorVarName).WithInitializer(SyntaxFactory.EqualsValueClause(getEnumeratorCall))
                    ))
                );

                var moveNextCall = SyntaxFactory.AwaitExpression(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(enumeratorVarName), SyntaxFactory.IdentifierName("MoveNextAsync"))
                    )
                );

                var currentAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(enumeratorVarName), SyntaxFactory.IdentifierName("Current"));
                var varDecl = SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(variableType)
                    .WithVariables(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(variableName).WithInitializer(SyntaxFactory.EqualsValueClause(currentAccess))
                    ))
                );

                if (loopBody is BlockSyntax block)
                {
                    loopBody = block.WithStatements(block.Statements.Insert(0, varDecl));
                }
                else
                {
                    loopBody = SyntaxFactory.Block(varDecl, loopBody);
                }

                var whileLoop = SyntaxFactory.WhileStatement(moveNextCall, loopBody);

                var disposeCall = SyntaxFactory.AwaitExpression(
                    SyntaxFactory.InvocationExpression(
                         SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(enumeratorVarName), SyntaxFactory.IdentifierName("DisposeAsync"))
                    )
                );

                var finallyBlock = SyntaxFactory.Block(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, SyntaxFactory.IdentifierName(enumeratorVarName), SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.ExpressionStatement(disposeCall)
                    )
                );

                var tryFinally = SyntaxFactory.TryStatement(
                    SyntaxFactory.Block(whileLoop),
                    SyntaxFactory.List<CatchClauseSyntax>(),
                    SyntaxFactory.FinallyClause(finallyBlock)
                );

                return SyntaxFactory.Block(enumeratorDecl, tryFinally);
            }

            return base.VisitForEachStatement(node);
        }

        public override SyntaxNode VisitNullableDirectiveTrivia(NullableDirectiveTriviaSyntax node)
        {
            return null;
        }

        public override SyntaxNode VisitBlock(BlockSyntax node)
        {
            var newStatements = ProcessBlockStatements(node.Statements);
            return node.WithStatements(newStatements);
        }

        private SyntaxList<StatementSyntax> ProcessBlockStatements(IEnumerable<StatementSyntax> statementsEnumerable)
        {
            var statements = statementsEnumerable.ToList();
            var newStatements = new List<StatementSyntax>();
            for (int i = 0; i < statements.Count; i++)
            {
                var stmt = statements[i];
                var visitedStmt = Visit(stmt) as StatementSyntax;

                if (visitedStmt == null)
                {
                    continue;
                }

                if (visitedStmt is LocalDeclarationStatementSyntax localDecl && localDecl.UsingKeyword.IsKind(SyntaxKind.UsingKeyword))
                {
                    var remaining = new List<StatementSyntax>();
                    for (int j = i + 1; j < statements.Count; j++)
                    {
                        remaining.Add(statements[j]);
                    }

                    var processedRest = ProcessBlockStatements(remaining);
                    var newBody = SyntaxFactory.Block(processedRest);

                    var usingStmt = SyntaxFactory.UsingStatement(
                        SyntaxFactory.List<AttributeListSyntax>(),
                        localDecl.Declaration,
                        null,
                        newBody
                    ).WithLeadingTrivia(localDecl.GetLeadingTrivia());

                    if (localDecl.AwaitKeyword.IsKind(SyntaxKind.AwaitKeyword))
                    {
                        usingStmt = usingStmt.WithAwaitKeyword(localDecl.AwaitKeyword);
                    }

                    newStatements.Add(usingStmt);
                    return SyntaxFactory.List(newStatements);
                }

                newStatements.Add(visitedStmt);
            }
            return SyntaxFactory.List(newStatements);
        }

        public override SyntaxNode VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            if (node.Left is IdentifierNameSyntax identifier)
            {
                var local = node.GetParent<LocalDeclarationStatementSyntax>();
                var name = identifier.Identifier.ValueText;

                if (local != null && local.Declaration.Variables.Any(v => v.Identifier.ValueText == name))
                {
                    hasChainingAssigment = true;
                }
            }

            var newNode = base.VisitAssignmentExpression(node);

            if (node.IsKind(SyntaxKind.CoalesceAssignmentExpression) && newNode is AssignmentExpressionSyntax assignment)
            {
                return SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    assignment.Left,
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.CoalesceExpression,
                        assignment.Left,
                        assignment.Right
                    )
                ).NormalizeWhitespace().WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }

            return newNode;
        }

        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            // Handle Top-Level Statements
            if (node.Members.OfType<GlobalStatementSyntax>().Any())
            {
                 // Move all GlobalStatementSyntax to a Main method in a Program class
                 var globalStatements = node.Members.OfType<GlobalStatementSyntax>().ToList();
                 var otherMembers = node.Members.Where(m => !(m is GlobalStatementSyntax)).ToList();

                 var mainStatements = globalStatements.Select(g => g.Statement).ToList();

                 var mainMethod = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), "Main")
                     .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.StaticKeyword)))
                     .WithBody(SyntaxFactory.Block(mainStatements));

                 var programClass = SyntaxFactory.ClassDeclaration("Program")
                     .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword))) // Internal?
                     .WithMembers(SyntaxFactory.SingletonList<MemberDeclarationSyntax>(mainMethod));

                 otherMembers.Add(programClass);
                 node = node.WithMembers(SyntaxFactory.List(otherMembers));
            }

            var fileScopedNamespace = node.Members.OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
            if (fileScopedNamespace != null)
            {
                var members = new List<MemberDeclarationSyntax>();
                foreach (var member in node.Members)
                {
                    if (member == fileScopedNamespace) continue;
                    members.Add(member);
                }

                members.AddRange(fileScopedNamespace.Members);

                var newNamespace = SyntaxFactory.NamespaceDeclaration(
                    fileScopedNamespace.Name,
                    fileScopedNamespace.Externs,
                    fileScopedNamespace.Usings,
                    SyntaxFactory.List(members)
                ).WithLeadingTrivia(fileScopedNamespace.GetLeadingTrivia());

                var newCompilationUnitMembers = new List<MemberDeclarationSyntax>();
                // Add usings/externs from compilation unit if needed, but usually they are at top.
                // FileScopedNamespace usings are already in the new namespace.
                // Compilation unit usings should remain in compilation unit or be moved?
                // Standard behavior: Compilation unit usings apply to the whole file.
                // FileScopedNamespace usings apply to the namespace.
                // We just replace the namespace declaration.

                // However, the structure is flat in FileScopedNamespace.
                // node.Members contains the namespace AND potentially other things (though strictly only one namespace allowed).

                newCompilationUnitMembers.Add(newNamespace);

                node = node.WithMembers(SyntaxFactory.List(newCompilationUnitMembers));
            }

            return base.VisitCompilationUnit(node);
        }

        public override SyntaxNode VisitWithExpression(WithExpressionSyntax node)
        {
            // Rewrite 'record with { Prop = Val }' to:
            // 1. Clone the object (using H5.clone via H5.Script.Write)
            // 2. Execute initializer block on the clone using H5.Script.CallFor

            var expression = (ExpressionSyntax)Visit(node.Expression);
            var initializer = (InitializerExpressionSyntax)Visit(node.Initializer);

            // Get type of expression for generics
            var typeInfo = semanticModel.GetTypeInfo(node.Expression);
            var type = typeInfo.Type ?? typeInfo.ConvertedType;
            var typeName = type?.ToMinimalDisplayString(semanticModel, node.SpanStart) ?? "object";
            var parsedType = SyntaxFactory.ParseTypeName(typeName);

            // H5.Script.Write<T>("H5.clone({0})", expression)
            var writeMethod = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.ParseName("global::H5.Script"),
                SyntaxFactory.GenericName("Write")
                .WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(parsedType)))
            );

            var cloneCall = SyntaxFactory.InvocationExpression(writeMethod,
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] {
                    SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("H5.clone({0})"))),
                    SyntaxFactory.Argument(expression)
                })));

            var tempParamName = "_w";
            var statements = new List<StatementSyntax>();

            foreach (var expr in initializer.Expressions)
            {
                if (expr is AssignmentExpressionSyntax assign)
                {
                    var left = assign.Left;
                    ExpressionSyntax newLeft = null;

                    if (left is IdentifierNameSyntax id)
                    {
                        newLeft = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(tempParamName), id);
                    }
                    else
                    {
                         // Handle cases where left is already complex?
                         // Usually 'with' works on properties, so IdentifierName is most common.
                         // If it's `this.Prop`, rewriting might be tricky if we don't stripping `this`.
                         newLeft = left;
                    }

                    if (newLeft != null)
                    {
                         statements.Add(SyntaxFactory.ExpressionStatement(
                             SyntaxFactory.AssignmentExpression(
                                 assign.Kind(),
                                 newLeft,
                                 assign.Right
                             )
                         ));
                    }
                }
            }

            statements.Add(SyntaxFactory.ReturnStatement(SyntaxFactory.IdentifierName(tempParamName)));

            var lambda = SyntaxFactory.ParenthesizedLambdaExpression(
                SyntaxFactory.ParameterList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Parameter(SyntaxFactory.Identifier(tempParamName)))),
                SyntaxFactory.Block(statements)
            );

            // H5.Script.CallFor<T>(clone, lambda)
            var callForMethod = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.ParseName("global::H5.Script"),
                SyntaxFactory.GenericName("CallFor")
                .WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(parsedType)))
            );

            var invocation = SyntaxFactory.InvocationExpression(callForMethod,
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] {
                    SyntaxFactory.Argument(cloneCall),
                    SyntaxFactory.Argument(lambda)
                })));

            return invocation;
        }

        public override SyntaxNode VisitSwitchExpression(SwitchExpressionSyntax node)
        {
            // Rewrite Switch Expression to ternary chain
            // x switch { P1 => V1, ... } -> (x is P1) ? V1 : ...

            hasIsPattern = true; // Ensure IsPatternReplacer runs on the generated IsPatternExpressions

            var gov = (ExpressionSyntax)Visit(node.GoverningExpression);
            var arms = node.Arms;

            // Check if gov is complex
            // Use ORIGINAL node for semantic checks, but construct with visited gov
            var isComplex = IsExpressionComplexEnoughToGetATemporaryVariable.IsComplex(semanticModel, node.GoverningExpression);
            string tempKeyName = null;

            if (isComplex)
            {
                tempKeyName = GetUniqueTempKey("sw_expr");

                var keyArg = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(tempKeyName));
                var methodIdentifier = SyntaxFactory.IdentifierName("global::H5.Script.ToTemp");

                // ToTemp(key, gov)
                gov = SyntaxFactory.InvocationExpression(methodIdentifier,
                        SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(keyArg), SyntaxFactory.Argument(gov) })));
            }

            // Prepare default case: Throw SwitchExpressionException
            // We cannot use ThrowExpression here because it requires semantic model for rewriting,
            // and this synthetic node won't have it. We use H5.Script.Write<T> to emit a JS IIFE that throws.

            var resultType = semanticModel.GetTypeInfo(node).Type;
            var resultTypeName = resultType?.ToMinimalDisplayString(semanticModel, node.SpanStart) ?? "object";

            var writeMethod = SyntaxFactory.ParseName("global::H5.Script.Write");
            // We need Write<T>.
            var genericWrite = SyntaxFactory.GenericName("Write")
                .WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.ParseTypeName(resultTypeName))));

            // Combine global::H5.Script + Write<T>
            var writeAccess = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.ParseName("global::H5.Script"),
                genericWrite
            );

            // Emit JS: (function() { throw new System.Exception("Switch Expression failed"); })()
            // We use System.Exception to ensure H5 knows it.
            var jsCode = "(function() { throw new System.Exception(\"Switch Expression failed\"); })()";

            ExpressionSyntax result = SyntaxFactory.InvocationExpression(writeAccess,
                SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(jsCode)))
                )));

            // Reverse iterate to build inside-out
            for (int i = arms.Count - 1; i >= 0; i--)
            {
                var arm = arms[i];
                // Visit children before using them
                var pattern = (PatternSyntax)Visit(arm.Pattern);
                var whenClause = (WhenClauseSyntax)Visit(arm.WhenClause);
                var expression = (ExpressionSyntax)Visit(arm.Expression);

                ExpressionSyntax condition = null;

                ExpressionSyntax checkExpr = gov;
                if (isComplex)
                {
                     // For subsequent checks, use FromTemp
                     // First one (top most) uses ToTemp if we do top-down?
                     // But we are building bottom up.
                     // The executed structure is:
                     // (Cond1) ? Val1 : (Cond2) ? Val2 : ...
                     // Cond1 is evaluated first.
                     // So Cond1 should contain the ToTemp if complex?
                     // Or better: (ToTemp(x) is P1) ? ...
                     // Yes.

                     // But we iterate backwards. The last arm is the innermost 'False'.
                     // So we use FromTemp for all, EXCEPT the very first one we generate (which corresponds to arms[0]).

                     if (i == 0)
                     {
                         // Use 'gov' which has the ToTemp invocation
                         checkExpr = gov;
                     }
                     else
                     {
                         var keyArg = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(tempKeyName));
                         // We need the type. IsExpressionComplexEnough... doesn't give type.
                         // We can try to infer or use object? IsPatternExpression works on anything?
                         // FromTemp<T> needs T.
                         // Let's use semantic model to get type of gov.
                         var typeInfo = semanticModel.GetTypeInfo(node.GoverningExpression);
                         var type = typeInfo.Type ?? typeInfo.ConvertedType;
                         var typeName = type?.ToMinimalDisplayString(semanticModel, node.SpanStart) ?? "object";

                         checkExpr = SyntaxFactory.InvocationExpression(
                            SyntaxFactory.GenericName("global::H5.Script.FromTemp")
                                .WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.ParseTypeName(typeName)))),
                            SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(keyArg)))
                         );
                     }
                }

                // Generate Condition
                // pattern is Constant -> checkExpr == constant
                // pattern is Type -> checkExpr is Type
                // pattern is Discard -> true
                // But we can leverage IsPatternExpression!
                // checkExpr is Pattern

                if (pattern is DiscardPatternSyntax)
                {
                    condition = SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression);
                }
                else
                {
                    condition = SyntaxFactory.IsPatternExpression(checkExpr, pattern);
                }

                if (whenClause != null)
                {
                    condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, whenClause.Condition);
                }

                result = SyntaxFactory.ParenthesizedExpression(
                    SyntaxFactory.ConditionalExpression(condition, expression, result)
                );
            }

            // Recurse? The arms might contain switches.
            // But we are returning 'result' which is a new tree.
            // We should Visit(result) to process children?
            // Or just return result and let the rewriter continue?
            // Rewriter visits children automatically if we call base.
            // But we are replacing the node. We must rewrite the *new* node or ensure children are rewritten.
            // Since we constructed new nodes using 'expression' (from arm), we should probably visit 'expression' before putting it in?
            // Actually, usually we replace and then Visit the result? No, Visit returns the result.
            // If we return a new node, the rewriter stops there for this branch?
            // Yes, standard Rewriter behavior.
            // So we should visit the children we extracted.

            // However, IsPatternReplacer runs later. So emitting IsPatternExpression is fine.
            // But what about nested SwitchExpressions in 'expression'?
            // We should rewrite 'expression' (the value).

            // Let's manually visit children?
            // Or easier: construct the tree, then call Visit(result)?
            // Be careful of infinite recursion if result contains a SwitchExpression that looks like 'node'.
            // But here we replaced SwitchExpression with Conditional. So safe.

            return result;
        }

        public override SyntaxNode VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            if (node.IsKind(SyntaxKind.IndexExpression)) // ^ expression
            {
                // ^n -> Length - n
                // We need the parent to know what "Length" refers to.
                // Usually inside ElementAccessExpression: x[^1]
                // Or RangeExpression: 1..^1

                // If it's inside ElementAccess: x[... ^n ...]
                // We need access to 'x'.
                // But VisitPrefixUnaryExpression doesn't know about 'x'.
                // The ElementAccess rewriting should handle this?
                // Or we rewrite ElementAccess?

                // If we rewrite ElementAccess, we need to visit arguments.
                // But SharpSixRewriter visits recursively.

                // H5 doesn't support implicit indexer?
                // If I rewrite `^1` to `(new System.Index(1, true))`, does H5 support it?
                // Check System.Index support.
                // grep showed `H5/H5/System/Linq/Expressions/IndexExpression.cs`.
                // But standard `System.Index` struct?
                // If H5 has `System.Index` and the indexer takes `Index`, then we are good?
                // If the indexer takes `int`, then `^1` is syntax sugar for `Length - 1`.

                // Given "Low Effort", and I don't see `System.Index.cs` in the list (grep failed to find class definition easily),
                // Assuming we need to rewrite to `Length - n`.

                // To do this, we need context.
                // This suggests we should override `VisitElementAccessExpression` instead.
            }
            return base.VisitPrefixUnaryExpression(node);
        }

        public override SyntaxNode VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                // Detached node
                return base.VisitElementAccessExpression(node);
            }

            // Handle x[^1]
            // We need to check arguments.

            var expression = node.Expression;
            // Evaluated expression key if needed?
            // If we use 'expression' multiple times (e.g. for Length), we might need temp.

            bool hasIndex = false;
            foreach (var arg in node.ArgumentList.Arguments)
            {
                if (arg.Expression.IsKind(SyntaxKind.IndexExpression))
                {
                    hasIndex = true;
                    break;
                }
            }

            if (hasIndex)
            {
                var isComplex = IsExpressionComplexEnoughToGetATemporaryVariable.IsComplex(semanticModel, expression);
                string tempKeyName = null;
                if (isComplex)
                {
                    tempKeyName = GetUniqueTempKey("idx_expr");
                    var keyArg = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(tempKeyName));
                    var methodIdentifier = SyntaxFactory.IdentifierName("global::H5.Script.ToTemp");
                    expression = SyntaxFactory.InvocationExpression(methodIdentifier,
                        SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(keyArg), SyntaxFactory.Argument(expression) })));
                }

                var newArgs = new List<ArgumentSyntax>();
                foreach (var arg in node.ArgumentList.Arguments)
                {
                    if (arg.Expression.IsKind(SyntaxKind.IndexExpression) && arg.Expression is PrefixUnaryExpressionSyntax prefix && prefix.OperatorToken.IsKind(SyntaxKind.CaretToken))
                    {
                        // ^n -> expression.Length - n
                        // Use FromTemp if complex.

                        ExpressionSyntax lenAccess;
                        if (isComplex)
                        {
                             var keyArg = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(tempKeyName));
                             // Infer type? Or just dynamic access?
                             // MemberAccess "Length" on FromTemp<object> might fail static check if typed?
                             // But H5.Script.FromTemp<T> returns T.
                             // We need T.
                             var typeInfo = semanticModel.GetTypeInfo(node.Expression);
                             var type = typeInfo.Type ?? typeInfo.ConvertedType;
                             var typeName = type?.ToMinimalDisplayString(semanticModel, node.SpanStart) ?? "object";

                             var fromTemp = SyntaxFactory.InvocationExpression(
                                SyntaxFactory.GenericName("global::H5.Script.FromTemp")
                                    .WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.ParseTypeName(typeName)))),
                                SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(keyArg)))
                             );
                             lenAccess = fromTemp;
                        }
                        else
                        {
                            lenAccess = expression; // 'expression' variable holds the modified expression (if not complex) or original?
                            // If isComplex=false, expression is original.
                        }

                        // Check if Length or Count
                        // Default to Length (Array/String)
                        string lengthProp = "Length";
                        var typeInfo2 = semanticModel.GetTypeInfo(node.Expression);
                        var type2 = typeInfo2.Type ?? typeInfo2.ConvertedType;
                        if (type2 != null)
                        {
                             if (type2.GetMembers("Count").Any()) lengthProp = "Count";
                        }

                        var lengthAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            lenAccess, SyntaxFactory.IdentifierName(lengthProp));

                        var val = prefix.Operand;

                        var newIdx = SyntaxFactory.BinaryExpression(SyntaxKind.SubtractExpression, lengthAccess, val);
                        newArgs.Add(arg.WithExpression(newIdx));
                    }
                    else
                    {
                        newArgs.Add(arg);
                    }
                }

                var newNode = node.WithExpression(expression).WithArgumentList(SyntaxFactory.BracketedArgumentList(SyntaxFactory.SeparatedList(newArgs)));
                return Visit(newNode);
            }

            return base.VisitElementAccessExpression(node);
        }

        public override SyntaxNode VisitParameter(ParameterSyntax node)
        {
            node = (ParameterSyntax)base.VisitParameter(node);

            var idx = node.Modifiers.IndexOf(SyntaxKind.InKeyword);
            if (idx > -1)
            {
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[idx], SyntaxFactory.Token(SyntaxKind.RefKeyword).WithTrailingTrivia(SyntaxFactory.Space)));
            }

            return node;
        }

        public override SyntaxNode VisitArgument(ArgumentSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitArgument(node);
            }

            var ti = semanticModel.GetTypeInfo(node.Expression);

            ITypeSymbol type = null;
            IMethodSymbol method = null;
            IParameterSymbol parameter = null;
            bool nonTrailing = false;

            if (node.NameColon == null && node.Parent is ArgumentListSyntax argList)
            {
                foreach (var arg in argList.Arguments)
                {
                    if (arg == node)
                    {
                        break;
                    }

                    if (arg.NameColon != null)
                    {
                        nonTrailing = true;
                        break;
                    }
                }
            }

            if (ti.Type != null && (ti.Type.TypeKind == TypeKind.Delegate || nonTrailing))
            {
                type = ti.Type;
            }
            else if (ti.ConvertedType != null && (ti.ConvertedType.TypeKind == TypeKind.Delegate || nonTrailing))
            {
                type = ti.ConvertedType;
            }

            if (type != null)
            {
                if (node.Parent is ArgumentListSyntax list && node.Parent.Parent is InvocationExpressionSyntax invocation)
                {
                    method = semanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;

                    if (method != null)
                    {
                        if (node.NameColon != null)
                        {
                            if (node.NameColon.Name != null)
                            {
                                var nameText = node.NameColon.Name.Identifier.ValueText;
                                if (nameText != null)
                                {
                                    foreach (var p in method.Parameters)
                                    {
                                        if (string.Equals(p.Name, nameText, StringComparison.Ordinal))
                                        {
                                            parameter = p;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var index = list.Arguments.IndexOf(node);
                            if (index >= 0)
                            {
                                if (index < method.Parameters.Length)
                                {
                                    parameter = method.Parameters[index];
                                }
                                else if (index >= method.Parameters.Length && method.Parameters[method.Parameters.Length - 1].IsParams)
                                {
                                    parameter = method.Parameters[method.Parameters.Length - 1];
                                }
                            }
                        }
                    }
                }
            }
            var isParam = parameter != null && !SyntaxHelper.IsAnonymous(parameter.Type);
            var parent = isParam && parameter.IsParams ? (InvocationExpressionSyntax)node.Parent.Parent : null;
            var pos = node.Expression.GetLocation().SourceSpan.Start;
            node = (ArgumentSyntax)base.VisitArgument(node);

            if (isParam)
            {
                var pType = parameter.Type;
                if (parameter.IsParams && IsExpandedForm(semanticModel, parent, method))
                {
                    pType = ((IArrayTypeSymbol)parameter.Type).ElementType;
                }

                if (node.Expression is CastExpressionSyntax && SymbolEqualityComparer.Default.Equals(type, pType) || parameter.RefKind != RefKind.None)
                {
                    return node;
                }

                if (pType.TypeKind == TypeKind.Delegate || parameter.IsParams && ((IArrayTypeSymbol)parameter.Type).ElementType.TypeKind == TypeKind.Delegate)
                {
                    var expr = node.Expression;

                    if (expr is LambdaExpressionSyntax || expr is AnonymousMethodExpressionSyntax || expr is QueryExpressionSyntax)
                    {
                        expr = SyntaxFactory.ParenthesizedExpression(expr);
                    }

                    var cast = SyntaxFactory.CastExpression(
                        SyntaxHelper.GenerateTypeSyntax(
                            pType,
                            semanticModel,
                            pos,
                            this
                        ),
                        expr
                    );

                    node = node.WithExpression(cast);
                }
            }

            if (nonTrailing && parameter != null)
            {
                node = node.WithNameColon(SyntaxFactory.NameColon(SyntaxFactory.IdentifierName(parameter.Name)));
            }

            if (node.RefKindKeyword.IsKind(SyntaxKind.InKeyword))
            {
                node = node.WithRefKindKeyword(SyntaxFactory.Token(SyntaxKind.RefKeyword).WithTrailingTrivia(SyntaxFactory.Space));
            }
            else if (node.RefKindKeyword.IsKind(SyntaxKind.RefKeyword) && node.Expression is InvocationExpressionSyntax)
            {
                node = node.WithRefKindKeyword(SyntaxFactory.Token(SyntaxKind.None));
            }

            return node;
        }

        public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            node = (ConstructorDeclarationSyntax)base.VisitConstructorDeclaration(node);

            if (node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithAttributeLists(node.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            if (node.ExpressionBody != null)
            {
                return SyntaxHelper.ToStatementBody(node);
            }

            return node;
        }

        public override SyntaxNode VisitDestructorDeclaration(DestructorDeclarationSyntax node)
        {
            node = (DestructorDeclarationSyntax)base.VisitDestructorDeclaration(node);

            if (node.ExpressionBody != null)
            {
                return SyntaxHelper.ToStatementBody(node);
            }

            return node;
        }

        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                // This node is detached (e.g. from a rewrite), so semantic model won't work.
                return base.VisitInvocationExpression(node);
            }

            var method = semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;
            
            var isRef = false;
            var toAwait = false;

            if (method != null && method.GetAttributes().Any(a => a.AttributeClass.FullyQualifiedName() == "H5.ToAwaitAttribute"))
            {
                toAwait = true;
            }

            if (method != null && method.ReturnsByRef && (node.Parent is AssignmentExpressionSyntax aes && aes.Left == node ||
                node.Parent is MemberAccessExpressionSyntax ||
                node.Parent is ArgumentSyntax arg && !arg.RefKindKeyword.IsKind(SyntaxKind.RefKeyword) ||
                node.Parent is EqualsValueClauseSyntax && node.Parent.Parent is VariableDeclaratorSyntax && node.Parent.Parent.Parent is VariableDeclarationSyntax vs && vs.Type.IsVar))
            {
                isRef = true;
            }

            var spanStart = node.SpanStart;
            var si = node.ArgumentList.Arguments.Count > 0 ? semanticModel.GetSymbolInfo(node.ArgumentList.Arguments[0].Expression) : default(SymbolInfo);
            var costValue = (string)semanticModel.GetConstantValue(node).Value;
            var conditionalParent = node.GetParent<ConditionalAccessExpressionSyntax>();
            var pos = node.GetLocation().SourceSpan.Start;

            node = (InvocationExpressionSyntax)base.VisitInvocationExpression(node);
            if (node.Expression is IdentifierNameSyntax syntax && syntax.Identifier.Text == "nameof")
            {
                string name = SyntaxHelper.GetSymbolName(node, si, costValue, semanticModel);
                return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(name));
            }
            else
            {
                if (method != null && method.IsGenericMethod && !method.TypeArguments.Any(ta => SyntaxHelper.IsAnonymous(ta) || ta.Kind == SymbolKind.TypeParameter && SymbolEqualityComparer.Default.Equals((ta as ITypeParameterSymbol)?.ContainingSymbol, method)))
                {
                    var expr = node.Expression;

                    if (expr is IdentifierNameSyntax)
                    {
                        var name = (IdentifierNameSyntax)expr;

                        var genericName = SyntaxHelper.GenerateGenericName(name.Identifier, method.TypeArguments, semanticModel, pos, this);
                        genericName = genericName.WithLeadingTrivia(name.GetLeadingTrivia().ExcludeDirectivies()).WithTrailingTrivia(name.GetTrailingTrivia().ExcludeDirectivies());
                        node = node.WithExpression(genericName);
                    }
                    else if (expr is MemberAccessExpressionSyntax ma && ma.Name is IdentifierNameSyntax)
                    {
                        expr = ma.Name;
                        var name = (IdentifierNameSyntax)expr;
                        var genericName = SyntaxHelper.GenerateGenericName(name.Identifier, method.TypeArguments, semanticModel, pos, this);
                        genericName = genericName.WithLeadingTrivia(name.GetLeadingTrivia().ExcludeDirectivies()).WithTrailingTrivia(name.GetTrailingTrivia().ExcludeDirectivies());

                        if (method.MethodKind == MethodKind.ReducedExtension && conditionalParent == null)
                        {
                            var target = ma.Expression;
                            var clsName = method.ContainingType.GetFullyQualifiedNameAndValidate(semanticModel, spanStart);
                            ma = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(clsName), genericName);
                            node = node.WithArgumentList(node.ArgumentList.WithArguments(node.ArgumentList.Arguments.Insert(0, SyntaxFactory.Argument(target))));
                        }
                        else
                        {
                            ma = ma.WithName(genericName);
                        }

                        node = node.WithExpression(ma);
                    }
                }
            }

            if (isRef)
            {
                return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, node, SyntaxFactory.IdentifierName("Value")).NormalizeWhitespace().WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }

            if (toAwait)
            {
                markAsAsync = true;

                if (node.Expression is MemberAccessExpressionSyntax ma)
                {
                    node = node.WithExpression(ma.WithName(SyntaxFactory.IdentifierName("WaitTask")));
                }

                return SyntaxFactory.AwaitExpression(node).NormalizeWhitespace().WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }

            return node;
        }

        public override SyntaxNode VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
        {
            var isInterpolatedString = semanticModel.GetConversion(node).IsInterpolatedString;

            node = (InterpolatedStringExpressionSyntax)base.VisitInterpolatedStringExpression(node);
            string methodNameToCall;
            string classNameToCall;

            if (isInterpolatedString)
            {
                classNameToCall = "System.Runtime.CompilerServices.FormattableStringFactory";
                methodNameToCall = "Create";
            }
            else
            {
                classNameToCall = "string";
                methodNameToCall = "Format";
            }

            string str = "";
            int placeholder = 0;
            var expressions = new List<ExpressionSyntax>();
            var idx = -1;

            foreach (var content in node.Contents)
            {
                idx++;
                if (content is InterpolatedStringTextSyntax interpolatedStringTextSyntax)
                {
                    str += interpolatedStringTextSyntax.TextToken.ValueText;
                }
                else if (content is InterpolationSyntax interpolation)
                {
                    str += "{" + placeholder.ToString(CultureInfo.InvariantCulture);

                    if (interpolation.AlignmentClause != null)
                    {
                        object value = null;

                        if (interpolation.AlignmentClause.Value is LiteralExpressionSyntax syntax)
                        {
                            value = syntax.Token.Value;
                        }
                        else
                        {
                            value = semanticModel.GetConstantValue(interpolation.AlignmentClause.Value).Value;
                        }

                        if (value == null)
                        {
                            Logger.ZLogError("Non-constant alignment");
                            return null;
                        }

                        str += "," + Convert.ToInt32(value).ToString(CultureInfo.InvariantCulture);
                    }

                    if (interpolation.FormatClause != null)
                    {
                        str += ":" + interpolation.FormatClause.FormatStringToken.Text;
                    }

                    str += "}";
                    placeholder++;
                    expressions.Add(interpolation.Expression);
                }
                else
                {
                    Logger.ZLogError("Unknown content in interpolated string: {0}", content);
                    return null;
                }
            }

            expressions.Insert(0, SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(str)));

            var methodIdentifier = SyntaxFactory.IdentifierName(classNameToCall + "." + methodNameToCall);
            var invocation = SyntaxFactory.InvocationExpression(methodIdentifier, SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(expressions.Select(SyntaxFactory.Argument))));

            return invocation;
        }

        public override SyntaxNode VisitUsingDirective(UsingDirectiveSyntax node)
        {
            if (node.GlobalKeyword.IsKind(SyntaxKind.GlobalKeyword))
            {
                node = node.WithGlobalKeyword(SyntaxFactory.Token(SyntaxKind.None));
            }

            if (node.StaticKeyword.RawKind == (int)SyntaxKind.StaticKeyword)
            {
                hasStaticUsingOrAliases = true;
                usingStaticNames.Add(node.Name.ToString());
            }
            if (node.Alias != null)
            {
                hasStaticUsingOrAliases = true;
            }
            return base.VisitUsingDirective(node);
        }

        public override SyntaxNode VisitGenericName(GenericNameSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                // Detached node
                return base.VisitGenericName(node);
            }

            if (!hasStaticUsingOrAliases)
            {
                return base.VisitGenericName(node);
            }

            var symbol = semanticModel.GetSymbolInfo(node).Symbol;
            var nodeParent = node.Parent;

            ITypeSymbol thisType = currentType.Count == 0 ? null : currentType.Peek();

            bool needHandle = !node.IsVar &&
                              symbol is ITypeSymbol &&
                              symbol.ContainingType != null &&
                              thisType != null &&
                              (!thisType.InheritsFromOrEquals(symbol.ContainingType) || node.Parent != null && node.Parent.Parent is GenericNameSyntax) &&
                              !SymbolEqualityComparer.Default.Equals(thisType, symbol);

            if (nodeParent is QualifiedNameSyntax qns && needHandle)
            {
                SyntaxNode n = node;
                do
                {
                    if (!qns.Left.Equals(n))
                    {
                        needHandle = false;
                    }

                    n = qns;
                    qns = qns.Parent as QualifiedNameSyntax;
                } while (qns != null && needHandle);
            }

            var spanStart = node.SpanStart;
            node = (GenericNameSyntax)base.VisitGenericName(node);

            if (needHandle && !(nodeParent is MemberAccessExpressionSyntax))
            {
                if (symbol is INamedTypeSymbol namedType && namedType.IsGenericType && namedType.TypeArguments.Length > 0 && !namedType.TypeArguments.Any(SyntaxHelper.IsAnonymous))
                {
                    return SyntaxHelper.GenerateGenericName(SyntaxFactory.Identifier(node.GetLeadingTrivia(), symbol.GetFullyQualifiedNameAndValidate(semanticModel, spanStart, false), node.GetTrailingTrivia()), namedType.TypeArguments, semanticModel, spanStart, this);
                }

                return SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(node.GetLeadingTrivia(), symbol.GetFullyQualifiedNameAndValidate(semanticModel, spanStart), node.GetTrailingTrivia()));
            }

            IMethodSymbol methodSymbol = null;

            if (symbol != null && symbol.IsStatic && symbol.ContainingType != null
                && thisType != null && (!thisType.InheritsFromOrEquals(symbol.ContainingType) || node.Parent != null && node.Parent.Parent is GenericNameSyntax)
                && !(nodeParent is MemberAccessExpressionSyntax)
                && (
                    (methodSymbol = symbol as IMethodSymbol) != null
                    || symbol is IPropertySymbol
                    || symbol is IFieldSymbol
                    || symbol is IEventSymbol)
                )
            {
                if (methodSymbol != null && methodSymbol.IsGenericMethod && methodSymbol.TypeArguments.Length > 0 && !methodSymbol.TypeArguments.Any(SyntaxHelper.IsAnonymous))
                {
                    return SyntaxHelper.GenerateGenericName(SyntaxFactory.Identifier(node.GetLeadingTrivia(), symbol.GetFullyQualifiedNameAndValidate(semanticModel, spanStart, false), node.GetTrailingTrivia()), methodSymbol.TypeArguments, semanticModel, spanStart, this);
                }

                return SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(node.GetLeadingTrivia(), symbol.GetFullyQualifiedNameAndValidate(semanticModel, spanStart), node.GetTrailingTrivia()));
            }

            return node;
        }

        public override SyntaxNode VisitPredefinedType(PredefinedTypeSyntax node)
        {
            if (node.Keyword.Text == "nint")
            {
                return SyntaxFactory.ParseTypeName("System.IntPtr").WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }
            if (node.Keyword.Text == "nuint")
            {
                return SyntaxFactory.ParseTypeName("System.UIntPtr").WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }
            return base.VisitPredefinedType(node);
        }

        public override SyntaxNode VisitNullableType(NullableTypeSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitNullableType(node);
            }

            var typeInfo = semanticModel.GetTypeInfo(node.ElementType);
            var type = typeInfo.Type ?? typeInfo.ConvertedType;

            if (type != null && !type.IsValueType)
            {
                // Reference type T? -> T
                return Visit(node.ElementType);
            }

            return base.VisitNullableType(node);
        }

        public override SyntaxNode VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            var value = semanticModel.GetConstantValue(node.Value);
            var newNode = base.VisitEqualsValueClause(node);

            if (value.HasValue && value.Value != null && newNode is EqualsValueClauseSyntax evc && !(node.Value is CastExpressionSyntax) && !(node.Value is LiteralExpressionSyntax))
            {
                var parent = node.GetParent<MemberDeclarationSyntax>();
                if (parent != null && (parent is FieldDeclarationSyntax || parent is PropertyDeclarationSyntax pd && pd.Initializer != null && pd.Initializer.Equals(node)))
                {
                    ExpressionSyntax literal = null;
                    if (SyntaxHelper.IsNumeric(value.Value.GetType()))
                    {
                        if (value.Value is double d && (double.IsNaN(d) || double.IsInfinity(d)))
                        {
                            IdentifierNameSyntax name = null;
                            if (double.IsNaN(d))
                            {
                                name = SyntaxFactory.IdentifierName("NaN");
                            }
                            else if (double.IsPositiveInfinity(d))
                            {
                                name = SyntaxFactory.IdentifierName("PositiveInfinity");
                            }
                            else if (double.IsNegativeInfinity(d))
                            {
                                name = SyntaxFactory.IdentifierName("NegativeInfinity");
                            }

                            literal = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword)), name);
                        }
                        else if (value.Value is float f && (float.IsNaN(f) || float.IsInfinity(f)))
                        {
                            IdentifierNameSyntax name = null;
                            if (float.IsNaN(f))
                            {
                                name = SyntaxFactory.IdentifierName("NaN");
                            }
                            else if (float.IsPositiveInfinity(f))
                            {
                                name = SyntaxFactory.IdentifierName("PositiveInfinity");
                            }
                            else if (float.IsNegativeInfinity(f))
                            {
                                name = SyntaxFactory.IdentifierName("NegativeInfinity");
                            }

                            literal = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.FloatKeyword)), name);
                        }
                        else
                        {
                            var ti = semanticModel.GetTypeInfo(node.Value);
                            if (ti.Type != null && ti.Type.TypeKind == TypeKind.Enum || ti.ConvertedType != null && ti.ConvertedType.TypeKind == TypeKind.Enum)
                            {
                                return newNode;
                            }

                            if (value.Value is long lv && lv == long.MinValue)
                            {
                                return evc.WithValue(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("long"), SyntaxFactory.IdentifierName("MinValue")));
                            }

                            literal = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal((dynamic)value.Value));
                        }
                    }
                    else if (value.Value is bool boolean)
                    {
                        literal = SyntaxFactory.LiteralExpression(boolean ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);
                    }
                    else if (value.Value is string stringVal)
                    {
                        literal = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(stringVal));
                    }
                    else if (value.Value is char charVal)
                    {
                        literal = SyntaxFactory.LiteralExpression(SyntaxKind.CharacterLiteralExpression, SyntaxFactory.Literal(charVal));
                    }
                    else
                    {
                        return newNode;
                    }

                    return evc.WithValue(literal);
                }
            }

            return newNode;
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                // Detached node
                return base.VisitIdentifierName(node);
            }

            ISymbol symbol = null;
            try
            {
                symbol = semanticModel.GetSymbolInfo(node).Symbol;
            }
            catch (ArgumentException)
            {
                // Ignore mismatch tree errors
                return base.VisitIdentifierName(node);
            }

            bool isRef = false;
            if (symbol != null && symbol is ILocalSymbol ls && ls.IsRef && !(node.Parent is RefExpressionSyntax))
            {
                isRef = true;
            }

            if (!hasStaticUsingOrAliases)
            {
                var newNode = (ExpressionSyntax)base.VisitIdentifierName(node);

                if (isRef)
                {
                    return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, newNode, SyntaxFactory.IdentifierName("Value")).NormalizeWhitespace().WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
                }

                return newNode;
            }

            var isAlias = semanticModel.GetAliasInfo(node) != null;

            if (isAlias)
            {
                var aliasSymbol = semanticModel.GetAliasInfo(node);
                var target = aliasSymbol.Target;

                if (target is INamespaceSymbol ns)
                {
                    return SyntaxFactory.ParseName(ns.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)).WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
                }
                else if (target is ITypeSymbol ts)
                {
                    return SyntaxHelper.GenerateTypeSyntax(ts).WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
                }
            }

            ITypeSymbol thisType = currentType.Count == 0 ? null : currentType.Peek();

            bool needHandle = !isAlias &&
                              !node.IsVar &&
                              symbol is ITypeSymbol &&
                              symbol.ContainingType != null &&
                              thisType != null &&
                              (!thisType.InheritsFromOrEquals(symbol.ContainingType) || node.Parent != null && node.Parent.Parent is GenericNameSyntax) &&
                              !SymbolEqualityComparer.Default.Equals(thisType, symbol);

            if (node.Parent is QualifiedNameSyntax qns && needHandle)
            {
                SyntaxNode n = node;
                do
                {
                    if (!qns.Left.Equals(n))
                    {
                        needHandle = false;
                    }

                    n = qns;
                    qns = qns.Parent as QualifiedNameSyntax;
                } while (qns != null && needHandle);
            }

            var spanStart = node.SpanStart;
            node = (IdentifierNameSyntax)base.VisitIdentifierName(node);

            if (needHandle && !(node.Parent is MemberAccessExpressionSyntax))
            {
                if (symbol is INamedTypeSymbol namedType && namedType.IsGenericType && namedType.TypeArguments.Length > 0 && !namedType.TypeArguments.Any(SyntaxHelper.IsAnonymous))
                {
                    var genericName = SyntaxHelper.GenerateGenericName(node.Identifier, namedType.TypeArguments, semanticModel, spanStart, this);
                    return genericName.WithLeadingTrivia(node.GetLeadingTrivia().ExcludeDirectivies()).WithTrailingTrivia(node.GetTrailingTrivia().ExcludeDirectivies());
                }

                return SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(node.GetLeadingTrivia(), symbol.GetFullyQualifiedNameAndValidate(semanticModel, spanStart), node.GetTrailingTrivia()));
            }

            IMethodSymbol methodSymbol = null;

            if (symbol != null && symbol.IsStatic && symbol.ContainingType != null
                && thisType != null && (!thisType.InheritsFromOrEquals(symbol.ContainingType) || node.Parent != null && node.Parent.Parent is GenericNameSyntax)
                && !(node.Parent is MemberAccessExpressionSyntax)
                && !(node.Parent is QualifiedNameSyntax)
                && (
                    (methodSymbol = symbol as IMethodSymbol) != null
                    || symbol is IPropertySymbol
                    || symbol is IFieldSymbol
                    || symbol is IEventSymbol
                    || symbol is INamedTypeSymbol)
                )
            {
                if (methodSymbol != null && methodSymbol.IsGenericMethod && methodSymbol.TypeArguments.Length > 0 && !methodSymbol.TypeArguments.Any(SyntaxHelper.IsAnonymous))
                {
                    var genericName = SyntaxHelper.GenerateGenericName(SyntaxFactory.Identifier(symbol.GetFullyQualifiedNameAndValidate(semanticModel, spanStart, false)), methodSymbol.TypeArguments, semanticModel, spanStart, this);
                    return genericName.WithLeadingTrivia(node.GetLeadingTrivia().ExcludeDirectivies()).WithTrailingTrivia(node.GetTrailingTrivia().ExcludeDirectivies());
                }

                return SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(node.GetLeadingTrivia(), symbol.GetFullyQualifiedNameAndValidate(semanticModel, spanStart), node.GetTrailingTrivia()));
            }

            if (isRef)
            {
                return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, node, SyntaxFactory.IdentifierName("Value")).NormalizeWhitespace().WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }

            return node;
        }

        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                // Detached node
                return base.VisitMemberAccessExpression(node);
            }

            var oldNode = node;
            var symbol = new Lazy<ISymbol>(() => {
                try { return semanticModel.GetSymbolInfo(oldNode.Expression).Symbol; }
                catch (ArgumentException) { return null; }
            });
            var symbolNode = new Lazy<ISymbol>(() => {
                try { return semanticModel.GetSymbolInfo(oldNode).Symbol; }
                catch (ArgumentException) { return null; }
            });

            ITypeSymbol thisType = currentType.Count == 0 ? null : currentType.Peek();

            var spanStart = node.Expression.SpanStart;
            node = (MemberAccessExpressionSyntax)base.VisitMemberAccessExpression(node);
            var isIdentifier = node.Expression.Kind() == SyntaxKind.IdentifierName || node.Expression.Kind() == SyntaxKind.GenericName;

            if (isIdentifier && symbolNode.Value != null && symbolNode.Value is IPropertySymbol ps && ps.RefKind == RefKind.In)
            {
                return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, node, SyntaxFactory.IdentifierName("Value")).NormalizeWhitespace().WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }

            if (symbolNode.Value != null && symbolNode.Value is IFieldSymbol && symbolNode.Value.ContainingType.IsTupleType)
            {
                var field = symbolNode.Value as IFieldSymbol;
                var tupleField = field.CorrespondingTupleField;
                node = node.WithName(SyntaxFactory.IdentifierName(tupleField.Name));
            }

            if (isIdentifier
                && symbol.Value != null
                && (symbol.Value.IsStatic || symbol.Value.Kind == SymbolKind.NamedType)
                && symbol.Value.ContainingType != null
                && thisType != null
                && (!thisType.InheritsFromOrEquals(symbol.Value.ContainingType) || node.Parent != null && node.Parent.Parent is GenericNameSyntax)
                && (symbol.Value.Kind == SymbolKind.Method || symbol.Value.Kind == SymbolKind.Property || symbol.Value.Kind == SymbolKind.Field || symbol.Value.Kind == SymbolKind.Event || symbol.Value.Kind == SymbolKind.NamedType))
            {
                return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(node.GetLeadingTrivia(),
                        symbol.Value.GetFullyQualifiedNameAndValidate(semanticModel, spanStart),
                        node.GetTrailingTrivia())), node.OperatorToken, node.Name);
            }

            if (isIdentifier
                && symbol.Value != null
                && symbolNode.Value != null
                && symbol.Value.Kind == SymbolKind.NamedType
                && symbolNode.Value.IsStatic
                && symbol.Value.ContainingType != null
                && thisType != null && (!thisType.InheritsFromOrEquals((ITypeSymbol)symbol.Value) || node.Parent != null && node.Parent.Parent is GenericNameSyntax)
                && !((ITypeSymbol)symbol.Value).IsAccessibleIn(thisType)
                && (symbol.Value.Kind == SymbolKind.Method || symbol.Value.Kind == SymbolKind.Property || symbol.Value.Kind == SymbolKind.Field || symbol.Value.Kind == SymbolKind.Event || symbol.Value.Kind == SymbolKind.NamedType))
            {
                return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(node.GetLeadingTrivia(),
                        symbol.Value.GetFullyQualifiedNameAndValidate(semanticModel, spanStart),
                        node.GetTrailingTrivia())), node.OperatorToken, node.Name);
            }

            return node;
        }

        public override SyntaxNode VisitAccessorList(AccessorListSyntax node)
        {
            node = (AccessorListSyntax)base.VisitAccessorList(node);

            if (node.Accessors.Any(a => a.ExpressionBody != null))
            {
                var list = new List<AccessorDeclarationSyntax>();
                foreach (var accessor in node.Accessors)
                {
                    if (accessor != null && accessor.ExpressionBody != null)
                    {
                        list.Add(SyntaxHelper.ToStatementBody(accessor));
                    }
                    else
                    {
                        list.Add(accessor);
                    }
                }

                node = node.WithAccessors(SyntaxFactory.List(list));
            }

            return node;
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            node = (PropertyDeclarationSyntax)base.VisitPropertyDeclaration(node);
            var newNode = node;

            if (newNode.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword) > -1)
            {
                newNode = newNode.WithModifiers(newNode.Modifiers.RemoveAt(newNode.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword)));
            }

            if (newNode.ExpressionBody != null)
            {
                newNode = SyntaxHelper.ToStatementBody(newNode);
            }

            if (node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                newNode = newNode.WithModifiers(newNode.Modifiers.Replace(newNode.Modifiers[newNode.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                newNode = newNode.WithModifiers(newNode.Modifiers.Replace(newNode.Modifiers[newNode.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                newNode = newNode.WithAttributeLists(newNode.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            if (node.IsAutoProperty() && node.AccessorList != null)
            {
                var setter = node.AccessorList.Accessors.SingleOrDefault(a => a.Keyword.IsKind(SyntaxKind.SetKeyword));

                if (setter == null)
                {
                    var getter = node.AccessorList.Accessors.Single(a => a.Keyword.IsKind(SyntaxKind.GetKeyword));
                    setter = SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PrivateKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                            .WithBody(null)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                            .WithLeadingTrivia(getter.GetLeadingTrivia())
                            .WithTrailingTrivia(getter.GetTrailingTrivia());

                    newNode = newNode.AddAccessorListAccessors(setter);
                }

                if (newNode.Initializer != null)
                {
                    var modifiers = SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PrivateKeyword).WithTrailingTrivia(SyntaxFactory.Space));

                    if (node.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)))
                    {
                        modifiers = modifiers.Add(SyntaxFactory.Token(SyntaxKind.StaticKeyword).WithTrailingTrivia(SyntaxFactory.Space));
                    }

                    var evc = SyntaxFactory.EqualsValueClause(newNode.Initializer.Value);
                    var field = SyntaxFactory.FieldDeclaration(SyntaxFactory.List<AttributeListSyntax>(),
                        modifiers,
                        SyntaxFactory.VariableDeclaration(
                            node.Type,
                            SyntaxFactory.SeparatedList(new[] {
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(AutoInitFieldPrefix + node.Identifier.Text),
                                    null,
                                    evc
                                )
                            })
                        ),
                        SyntaxFactory.Token(SyntaxKind.SemicolonToken)
                    );


                    fields.Add(field);
                    newNode = newNode.ReplaceNode(newNode.Initializer, (SyntaxNode)null);
                    var trivias = node.Initializer.GetLeadingTrivia().AddRange(node.Initializer.GetTrailingTrivia());
                    newNode = newNode.WithTrailingTrivia(trivias.AddRange(node.GetTrailingTrivia()));
                    newNode = SyntaxHelper.RemoveSemicolon(newNode, newNode.SemicolonToken, t => newNode.WithSemicolonToken(t));
                }

                return newNode;
            }

            return newNode.Equals(node) ? node : newNode;
        }

        public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            node = base.VisitEnumDeclaration(node) as EnumDeclarationSyntax;

            if (node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithAttributeLists(node.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            return node;
        }

        public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            node = base.VisitFieldDeclaration(node) as FieldDeclarationSyntax;

            if (node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithAttributeLists(node.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            return node;
        }

        public override SyntaxNode VisitEventDeclaration(EventDeclarationSyntax node)
        {
            node = base.VisitEventDeclaration(node) as EventDeclarationSyntax;

            if (node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithAttributeLists(node.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            return node;
        }

        public override SyntaxNode VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            node = base.VisitEventFieldDeclaration(node) as EventFieldDeclarationSyntax;

            if (node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithAttributeLists(node.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            return node;
        }

        public override SyntaxNode VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            node = base.VisitDelegateDeclaration(node) as DelegateDeclarationSyntax;

            if (node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithAttributeLists(node.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            return node;
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            currentType.Push(semanticModel.GetDeclaredSymbol(node));

            var old = fields;
            fields = new List<MemberDeclarationSyntax>();
            var isReadOnly = node.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword) > -1;
            var isRef = node.Modifiers.IndexOf(SyntaxKind.RefKeyword) > -1;
            var c = base.VisitStructDeclaration(node) as StructDeclarationSyntax;

            if (c != null && fields.Count > 0)
            {
                var list = c.Members.ToList();
                var arr = fields.ToArray();
                var trivias = c.CloseBraceToken.LeadingTrivia;
                trivias = trivias.Insert(0, SyntaxFactory.Whitespace("\n")).Add(SyntaxFactory.Whitespace("\n"));
                arr[0] = arr[0].WithLeadingTrivia(trivias);
                c = c.WithCloseBraceToken(c.CloseBraceToken.WithLeadingTrivia(null));
                list.AddRange(arr);
                c = c.WithMembers(SyntaxFactory.List(list));
            }

            if (c != null && isReadOnly)
            {
                c = c.WithModifiers(c.Modifiers.RemoveAt(c.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword)));
                c = c.WithAttributeLists(c.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(new AttributeSyntax[1] { SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.Immutable")) })).WithTrailingTrivia(SyntaxFactory.Whitespace("\n"))));
            }

            if (c != null && isRef)
            {
                c = c.WithModifiers(c.Modifiers.RemoveAt(c.Modifiers.IndexOf(SyntaxKind.RefKeyword)));
            }

            if (c != null && c.Modifiers.Any(SyntaxKind.UnsafeKeyword))
            {
                c = c.WithModifiers(c.Modifiers.RemoveAt(c.Modifiers.IndexOf(SyntaxKind.UnsafeKeyword)));
            }

            if (c.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && c.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                c = c.WithModifiers(c.Modifiers.Replace(c.Modifiers[c.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                c = c.WithModifiers(c.Modifiers.Replace(c.Modifiers[c.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithAttributeLists(node.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            fields = old;
            currentType.Pop();

            return c;
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            currentType.Push(semanticModel.GetDeclaredSymbol(node));
            var oldIndex = IndexInstance;
            IndexInstance = 0;
            var old = fields;
            fields = new List<MemberDeclarationSyntax>();

            var c = base.VisitClassDeclaration(node) as ClassDeclarationSyntax;

            if (c != null && fields.Count > 0)
            {
                var list = c.Members.ToList();
                var arr = fields.ToArray();
                var trivias = c.CloseBraceToken.LeadingTrivia;
                trivias = trivias.Insert(0, SyntaxFactory.Whitespace("\n")).Add(SyntaxFactory.Whitespace("\n"));
                arr[0] = arr[0].WithLeadingTrivia(trivias);
                c = c.WithCloseBraceToken(c.CloseBraceToken.WithLeadingTrivia(null));
                list.AddRange(arr);
                c = c.WithMembers(SyntaxFactory.List(list));
            }

            if (c != null && c.Modifiers.Any(SyntaxKind.UnsafeKeyword))
            {
                c = c.WithModifiers(c.Modifiers.RemoveAt(c.Modifiers.IndexOf(SyntaxKind.UnsafeKeyword)));
            }

            if (c.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && c.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                c = c.WithModifiers(c.Modifiers.Replace(c.Modifiers[c.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                c = c.WithModifiers(c.Modifiers.Replace(c.Modifiers[c.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithAttributeLists(node.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            fields = old;
            IndexInstance = oldIndex;
            currentType.Pop();

            return c;
        }

        public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            currentType.Push(semanticModel.GetDeclaredSymbol(node));
            node = base.VisitInterfaceDeclaration(node) as InterfaceDeclarationSyntax;
            currentType.Pop();

            if (node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithAttributeLists(node.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            return node;
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var oldMarkAsAsync = markAsAsync;
            markAsAsync = false;

            var oldIndex = IndexInstance;
            IndexInstance = 0;

            node = base.VisitMethodDeclaration(node) as MethodDeclarationSyntax;

            if (node.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword) > -1)
            {
                node = node.WithModifiers(node.Modifiers.RemoveAt(node.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword)));
            }

            if (node.Modifiers.Any(SyntaxKind.UnsafeKeyword))
            {
                node = node.WithModifiers(node.Modifiers.RemoveAt(node.Modifiers.IndexOf(SyntaxKind.UnsafeKeyword)));
            }

            if (node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithModifiers(node.Modifiers.Replace(node.Modifiers[node.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                node = node.WithAttributeLists(node.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            if (markAsAsync && node.Modifiers.IndexOf(SyntaxKind.AsyncKeyword) == -1)
            {
                node = node.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" ")).WithLeadingTrivia(SyntaxFactory.Whitespace(" ")));
            }

            markAsAsync = oldMarkAsAsync;
            if (node.ExpressionBody != null)
            {
                return SyntaxHelper.ToStatementBody(node);
            }

            IndexInstance = oldIndex;

            return node;
        }

        public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            var oldIndex = IndexInstance;
            IndexInstance = 0;

            if (node.Keyword.IsKind(SyntaxKind.InitKeyword))
            {
                node = node.WithKeyword(SyntaxFactory.Token(SyntaxKind.SetKeyword).WithTrailingTrivia(node.Keyword.TrailingTrivia).WithLeadingTrivia(node.Keyword.LeadingTrivia));
            }

            var result = base.VisitAccessorDeclaration(node);

            if (result is AccessorDeclarationSyntax accessorResult)
            {
                if (accessorResult.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword) > -1)
                {
                    result = accessorResult.WithModifiers(accessorResult.Modifiers.RemoveAt(accessorResult.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword)));
                }
            }

            IndexInstance = oldIndex;
            return result;
        }

        public override SyntaxNode VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            currentType.Push(semanticModel.GetDeclaredSymbol(node));

            // Rewrite record to class or struct
            // Visit members first to ensure they are rewritten in context
            var visitedMembers = node.Members.Select(m => (MemberDeclarationSyntax)Visit(m)).ToList();

            TypeDeclarationSyntax typeDecl;

            if (node.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword))
            {
                typeDecl = SyntaxFactory.StructDeclaration(node.Identifier)
                    .WithKeyword(SyntaxFactory.Token(SyntaxKind.StructKeyword).WithTrailingTrivia(SyntaxFactory.Space));
            }
            else
            {
                typeDecl = SyntaxFactory.ClassDeclaration(node.Identifier)
                     .WithKeyword(SyntaxFactory.Token(SyntaxKind.ClassKeyword).WithTrailingTrivia(SyntaxFactory.Space));
            }

            typeDecl = typeDecl
                .WithModifiers(node.Modifiers)
                .WithTypeParameterList(node.TypeParameterList)
                .WithBaseList(node.BaseList)
                .WithConstraintClauses(node.ConstraintClauses)
                .WithAttributeLists(node.AttributeLists);

            if (node.OpenBraceToken.IsKind(SyntaxKind.None))
            {
                typeDecl = typeDecl
                    .WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken).WithLeadingTrivia(SyntaxFactory.Space).WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed))
                    .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken).WithLeadingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed));
            }
            else
            {
                typeDecl = typeDecl
                    .WithOpenBraceToken(node.OpenBraceToken)
                    .WithCloseBraceToken(node.CloseBraceToken);
            }

            typeDecl = typeDecl.WithMembers(SyntaxFactory.List(visitedMembers));

            if (node.ParameterList != null)
            {
                // Create properties and constructor
                var properties = new List<MemberDeclarationSyntax>();
                var ctorParams = new List<ParameterSyntax>();
                var ctorBody = new List<StatementSyntax>();

                foreach (var param in node.ParameterList.Parameters)
                {
                    var property = SyntaxFactory.PropertyDeclaration(param.Type.WithoutTrivia().WithTrailingTrivia(SyntaxFactory.Space), param.Identifier.WithoutTrivia())
                        .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                        .WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List(new[]
                        {
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithKeyword(SyntaxFactory.Token(SyntaxKind.SetKeyword)).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                        })));

                    properties.Add(property);

                    ctorParams.Add(param);
                }

                typeDecl = typeDecl.WithMembers(typeDecl.Members.InsertRange(0, properties));

                var ctor = SyntaxFactory.ConstructorDeclaration(node.Identifier)
                    .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                    .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(ctorParams)))
                    .WithBody(SyntaxFactory.Block(
                         ctorParams.Select(p =>
                             SyntaxFactory.ExpressionStatement(
                                 SyntaxFactory.AssignmentExpression(
                                     SyntaxKind.SimpleAssignmentExpression,
                                     SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ThisExpression(), SyntaxFactory.IdentifierName(p.Identifier)),
                                     SyntaxFactory.IdentifierName(p.Identifier)
                                 )
                             )
                         )
                    ));

                typeDecl = typeDecl.AddMembers(ctor);

                var deconstructParams = ctorParams.Select(p =>
                    SyntaxFactory.Parameter(p.Identifier).WithType(p.Type).AddModifiers(SyntaxFactory.Token(SyntaxKind.OutKeyword))
                );

                var deconstructBody = ctorParams.Select(p =>
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(p.Identifier),
                            SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ThisExpression(), SyntaxFactory.IdentifierName(p.Identifier))
                        )
                    )
                );

                var deconstruct = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword).WithTrailingTrivia(SyntaxFactory.Space)), "Deconstruct")
                    .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                    .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(deconstructParams)))
                    .WithBody(SyntaxFactory.Block(deconstructBody));

                typeDecl = typeDecl.AddMembers(deconstruct);
            }

            // Manually apply VisitClassDeclaration logic for unsafe/privateprotected
            if (typeDecl.Modifiers.Any(SyntaxKind.UnsafeKeyword))
            {
                typeDecl = typeDecl.WithModifiers(typeDecl.Modifiers.RemoveAt(typeDecl.Modifiers.IndexOf(SyntaxKind.UnsafeKeyword)));
            }

            if (typeDecl.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && typeDecl.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                typeDecl = typeDecl.WithModifiers(typeDecl.Modifiers.Replace(typeDecl.Modifiers[typeDecl.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                typeDecl = typeDecl.WithModifiers(typeDecl.Modifiers.Replace(typeDecl.Modifiers[typeDecl.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                typeDecl = typeDecl.WithAttributeLists(typeDecl.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            currentType.Pop();
            return typeDecl;
        }

        public override SyntaxNode VisitStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitStackAllocArrayCreationExpression(node);
            }

            node = (StackAllocArrayCreationExpressionSyntax)base.VisitStackAllocArrayCreationExpression(node);

            return SyntaxFactory.ArrayCreationExpression((ArrayTypeSyntax)node.Type, node.Initializer)
                .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                .WithLeadingTrivia(node.GetLeadingTrivia())
                .WithTrailingTrivia(node.GetTrailingTrivia());
        }

        public override SyntaxNode VisitImplicitStackAllocArrayCreationExpression(ImplicitStackAllocArrayCreationExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitImplicitStackAllocArrayCreationExpression(node);
            }

            node = (ImplicitStackAllocArrayCreationExpressionSyntax)base.VisitImplicitStackAllocArrayCreationExpression(node);

            return SyntaxFactory.ImplicitArrayCreationExpression(node.Initializer)
                .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                .WithLeadingTrivia(node.GetLeadingTrivia())
                .WithTrailingTrivia(node.GetTrailingTrivia());
        }

        public override SyntaxNode VisitImplicitObjectCreationExpression(ImplicitObjectCreationExpressionSyntax node)
        {
             if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitImplicitObjectCreationExpression(node);
            }

            // new() -> new Type()
            var typeInfo = semanticModel.GetTypeInfo(node);
            var type = typeInfo.Type ?? typeInfo.ConvertedType;

            if (type != null)
            {
                 var typeSyntax = SyntaxHelper.GenerateTypeSyntax(type, semanticModel, node.SpanStart, this);
                 var newObj = SyntaxFactory.ObjectCreationExpression(typeSyntax)
                     .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                     .WithArgumentList(node.ArgumentList)
                     .WithInitializer(node.Initializer);

                 if (node.Initializer != null)
                 {
                     bool needRewrite = false;
                     List<InitializerInfo> initializerInfos = new List<InitializerInfo>();
                     bool extensionMethodExists = false;
                     bool isImplicitElementAccessSyntax = false;

                     needRewrite = NeedRewriteInitializer(node.Initializer, initializerInfos, ref extensionMethodExists, ref isImplicitElementAccessSyntax);

                     if (needRewrite)
                     {
                         if (IsExpressionOfT)
                         {
                             // Simplified error handling
                             throw new Exception("Initializer in Expression<T> not supported for TargetTypedNew");
                         }

                         var initializers = node.Initializer.Expressions;
                         ExpressionSyntax[] args = new ExpressionSyntax[2];
                         var target = newObj.WithInitializer(null).WithoutTrivia();

                         if (target.ArgumentList == null)
                         {
                             target = target.WithArgumentList(SyntaxFactory.ArgumentList());
                         }

                         args[0] = target;

                         List<StatementSyntax> statements = new List<StatementSyntax>();

                         var parent = node.Parent;

                         while (parent != null && !(parent is MethodDeclarationSyntax) && !(parent is ClassDeclarationSyntax))
                         {
                             parent = parent.Parent;
                         }

                         string instance = "_o" + ++IndexInstance;
                         if (parent != null)
                         {
                             var info = LocalUsageGatherer.GatherInfo(semanticModel, parent);
                             while (info.DirectlyOrIndirectlyUsedLocals.Any(s => s.Name == instance) || info.Names.Contains(instance))
                             {
                                 instance = "_o" + ++IndexInstance;
                             }
                         }

                         ConvertInitializers(initializers, instance, statements, initializerInfos);

                         statements.Add(SyntaxFactory.ReturnStatement(SyntaxFactory.IdentifierName(instance).WithLeadingTrivia(SyntaxFactory.Space)));

                         var body = SyntaxFactory.Block(statements);
                         var lambda = SyntaxFactory.ParenthesizedLambdaExpression(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Parameter(SyntaxFactory.Identifier(instance)) })), body);
                         var isAsync = AwaitersCollector.HasAwaiters(semanticModel, node);
                         if (isAsync)
                         {
                             lambda = lambda.WithAsyncKeyword(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
                         }

                         args[1] = lambda;

                         var methodIdentifier = isAsync ? SyntaxFactory.IdentifierName("global::H5.Script.AsyncCallFor") : SyntaxFactory.IdentifierName("global::H5.Script.CallFor");
                         var invocation = SyntaxFactory.InvocationExpression(methodIdentifier, SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(args.Select(SyntaxFactory.Argument))));
                         invocation = invocation.WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());

                         if (isAsync)
                         {
                             return SyntaxFactory.AwaitExpression(invocation.WithLeadingTrivia(invocation.GetLeadingTrivia().Insert(0, SyntaxFactory.Whitespace(" "))));
                         }

                         return invocation;
                     }
                 }

                 return VisitObjectCreationExpression(newObj);
            }

            return base.VisitImplicitObjectCreationExpression(node);
        }

        public override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node)
        {
            node = (OperatorDeclarationSyntax)base.VisitOperatorDeclaration(node);
            if (node.ExpressionBody != null)
            {
                return SyntaxHelper.ToStatementBody(node);
            }

            return node;
        }

        public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            node = (ConversionOperatorDeclarationSyntax)base.VisitConversionOperatorDeclaration(node);
            if (node.ExpressionBody != null)
            {
                return SyntaxHelper.ToStatementBody(node);
            }

            return node;
        }

        public override SyntaxNode VisitIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            node = (IndexerDeclarationSyntax)base.VisitIndexerDeclaration(node);

            if (node.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword) > -1)
            {
                node = node.WithModifiers(node.Modifiers.RemoveAt(node.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword)));
            }

            if (node.ExpressionBody != null)
            {
                return SyntaxHelper.ToStatementBody(node);
            }

            return node;
        }

        public override SyntaxNode VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            var oldMarkAsAsync = markAsAsync;
            markAsAsync = false;
            var ti = semanticModel.GetTypeInfo(node);
            var oldValue = IsExpressionOfT;

            // Remove static
            if (node.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)))
            {
                var idx = node.Modifiers.IndexOf(SyntaxKind.StaticKeyword);
                node = node.WithModifiers(node.Modifiers.RemoveAt(idx));
            }

            // Handle discards
            if (node.ParameterList.Parameters.Any(p => p.Identifier.Text == "_"))
            {
                var newParams = new List<ParameterSyntax>();
                var discardCount = 0;
                foreach (var p in node.ParameterList.Parameters)
                {
                    if (p.Identifier.Text == "_")
                    {
                        discardCount++;
                        newParams.Add(p.WithIdentifier(SyntaxFactory.Identifier("__discard_" + discardCount)));
                    }
                    else
                    {
                        newParams.Add(p);
                    }
                }
                node = node.WithParameterList(node.ParameterList.WithParameters(SyntaxFactory.SeparatedList(newParams)));
            }

            if (ti.Type != null && ti.Type.IsExpressionOfT() ||
                ti.ConvertedType != null && ti.ConvertedType.IsExpressionOfT())
            {
                IsExpressionOfT = true;
            }

            var newNode = base.VisitParenthesizedLambdaExpression(node);

            IsExpressionOfT = oldValue;

            if (markAsAsync && newNode is ParenthesizedLambdaExpressionSyntax ple)
            {
                ple = ple.WithAsyncKeyword(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
                newNode = ple;
            }

            markAsAsync = oldMarkAsAsync;

            return newNode;
        }

        public override SyntaxNode VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            var oldMarkAsAsync = markAsAsync;
            markAsAsync = false;

            var ti = semanticModel.GetTypeInfo(node);
            var oldValue = IsExpressionOfT;

            // Remove static
            if (node.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)))
            {
                var idx = node.Modifiers.IndexOf(SyntaxKind.StaticKeyword);
                node = node.WithModifiers(node.Modifiers.RemoveAt(idx));
            }

            if (ti.Type != null && ti.Type.IsExpressionOfT() ||
                ti.ConvertedType != null && ti.ConvertedType.IsExpressionOfT())
            {
                IsExpressionOfT = true;
            }

            var newNode = base.VisitSimpleLambdaExpression(node);

            IsExpressionOfT = oldValue;

            if (markAsAsync && newNode is SimpleLambdaExpressionSyntax sle)
            {
                sle = sle.WithAsyncKeyword(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
                newNode = sle;
            }

            markAsAsync = oldMarkAsAsync;

            return newNode;
        }

        public bool IsExpressionOfT { get; set; }

        private int IndexInstance { get; set; }

        private class InitializerInfo
        {
            public IMethodSymbol method;
            public List<InitializerInfo> nested;
        }

        private bool NeedRewriteInitializer(InitializerExpressionSyntax initializer, List<InitializerInfo> infos, ref bool extensionMethodExists, ref bool isImplicitElementAccessSyntax)
        {
            bool need = false;
            foreach (var init in initializer.Expressions)
            {
                var info = new InitializerInfo();
                infos.Add(info);
                var ae = init as AssignmentExpressionSyntax;
                if (ae?.Right is InitializerExpressionSyntax)
                {
                    info.nested = new List<InitializerInfo>();
                    if (NeedRewriteInitializer((InitializerExpressionSyntax)ae.Right, info.nested, ref extensionMethodExists, ref isImplicitElementAccessSyntax))
                    {
                        need = true;
                    }
                }
                else
                {
                    var symbolInfo = semanticModel.GetCollectionInitializerSymbolInfo(init);
                    var collectionInitializer = symbolInfo.Symbol;

                    if (symbolInfo.Symbol == null && symbolInfo.CandidateSymbols.Length > 0)
                    {
                        collectionInitializer = symbolInfo.CandidateSymbols[0];
                    }

                    var mInfo = collectionInitializer != null ? collectionInitializer as IMethodSymbol : null;
                    if (mInfo != null)
                    {
                        info.method = mInfo;
                        if (mInfo.IsExtensionMethod)
                        {
                            extensionMethodExists = true;
                        }
                        need = true;
                    }

                    if (init.Kind() == SyntaxKind.SimpleAssignmentExpression)
                    {
                        var be = (AssignmentExpressionSyntax)init;
                        if (be.Left is ImplicitElementAccessSyntax)
                        {
                            isImplicitElementAccessSyntax = true;
                            need = true;
                        }
                    }
                }
            }

            return need;
        }

        public override SyntaxNode VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitObjectCreationExpression(node);
            }

            bool needRewrite = false;
            List<InitializerInfo> initializerInfos = null;
            bool extensionMethodExists = false;
            bool isImplicitElementAccessSyntax = false;

            if (node.Initializer != null)
            {
                initializerInfos = new List<InitializerInfo>();
                needRewrite = NeedRewriteInitializer(node.Initializer, initializerInfos, ref extensionMethodExists, ref isImplicitElementAccessSyntax);
            }

            node = (ObjectCreationExpressionSyntax)base.VisitObjectCreationExpression(node);
            if (needRewrite)
            {
                if (IsExpressionOfT)
                {
                    if (isImplicitElementAccessSyntax)
                    {
                        var mapped = semanticModel.SyntaxTree.GetLineSpan(node.Span);
                        throw new Exception(string.Format(CultureInfo.InvariantCulture, "{2} - {3}({0},{1}): {4}", mapped.StartLinePosition.Line + 1, mapped.StartLinePosition.Character + 1, "Index collection initializer is not supported inside Expression<T>", semanticModel.SyntaxTree.FilePath, node.ToString()));
                    }

                    if (extensionMethodExists)
                    {
                        var mapped = semanticModel.SyntaxTree.GetLineSpan(node.Span);
                        throw new Exception(string.Format(CultureInfo.InvariantCulture, "{2} - {3}({0},{1}): {4}", mapped.StartLinePosition.Line + 1, mapped.StartLinePosition.Character + 1, "Extension method for collection initializer is not supported inside Expression<T>", semanticModel.SyntaxTree.FilePath, node.ToString()));
                    }

                    return node;
                }

                var initializers = node.Initializer.Expressions;
                ExpressionSyntax[] args = new ExpressionSyntax[2];
                var target = node.WithInitializer(null).WithoutTrivia();

                if (target.ArgumentList == null)
                {
                    target = target.WithArgumentList(SyntaxFactory.ArgumentList());
                }

                args[0] = target;

                List<StatementSyntax> statements = new List<StatementSyntax>();

                var parent = node.Parent;

                while (parent != null && !(parent is MethodDeclarationSyntax) && !(parent is ClassDeclarationSyntax))
                {
                    parent = parent.Parent;
                }

                string instance = "_o" + ++IndexInstance;
                if (parent != null)
                {
                    var info = LocalUsageGatherer.GatherInfo(semanticModel, parent);
                    while (info.DirectlyOrIndirectlyUsedLocals.Any(s => s.Name == instance) || info.Names.Contains(instance))
                    {
                        instance = "_o" + ++IndexInstance;
                    }
                }

                ConvertInitializers(initializers, instance, statements, initializerInfos);

                statements.Add(SyntaxFactory.ReturnStatement(SyntaxFactory.IdentifierName(instance).WithLeadingTrivia(SyntaxFactory.Space)));

                var body = SyntaxFactory.Block(statements);
                var lambda = SyntaxFactory.ParenthesizedLambdaExpression(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Parameter(SyntaxFactory.Identifier(instance)) })), body);
                var isAsync = AwaitersCollector.HasAwaiters(semanticModel, node);
                if (isAsync)
                {
                    lambda = lambda.WithAsyncKeyword(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
                }

                args[1] = lambda;

                var methodIdentifier = isAsync ? SyntaxFactory.IdentifierName("global::H5.Script.AsyncCallFor") : SyntaxFactory.IdentifierName("global::H5.Script.CallFor");
                var invocation = SyntaxFactory.InvocationExpression(methodIdentifier, SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(args.Select(SyntaxFactory.Argument))));
                invocation = invocation.WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());

                if (isAsync)
                {
                    return SyntaxFactory.AwaitExpression(invocation.WithLeadingTrivia(invocation.GetLeadingTrivia().Insert(0, SyntaxFactory.Whitespace(" "))));
                }

                return invocation;
            }

            return node;
        }

        private static void ConvertInitializers(SeparatedSyntaxList<ExpressionSyntax> initializers, string instance, List<StatementSyntax> statements, List<InitializerInfo> infos)
        {
            var idx = 0;
            foreach (var init in initializers)
            {
                var info = infos[idx++];
                var mInfo = info != null && info.method != null ? info.method : null;
                if (mInfo != null)
                {
                    if (mInfo.IsStatic)
                    {
                        var ie = SyntaxHelper.GenerateStaticMethodCall(mInfo.Name,
                            mInfo.ContainingType.FullyQualifiedName(),
                            new[]
                            {
                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName(instance)),
                                SyntaxFactory.Argument(init.WithoutTrivia())
                            }, mInfo.TypeArguments.ToArray());
                        statements.Add(ie);
                    }
                    else
                    {
                        ArgumentSyntax[] arguments = null;
                        if (init.Kind() == SyntaxKind.ComplexElementInitializerExpression)
                        {
                            var complexInit = (InitializerExpressionSyntax)init;

                            arguments = new ArgumentSyntax[complexInit.Expressions.Count];
                            for (int i = 0; i < complexInit.Expressions.Count; i++)
                            {
                                arguments[i] = SyntaxFactory.Argument(complexInit.Expressions[i].WithoutTrivia());
                            }
                        }
                        else
                        {
                            arguments = new[]
                            {
                                SyntaxFactory.Argument(init.WithoutTrivia())
                            };
                        }

                        var ie = SyntaxHelper.GenerateMethodCall(mInfo.Name, instance, arguments, mInfo.TypeArguments.ToArray());
                        statements.Add(ie);
                    }
                }
                else
                {
                    var be = (AssignmentExpressionSyntax)init;

                    if (be.Right is InitializerExpressionSyntax syntax)
                    {
                        string name = null;
                        if (be.Left is IdentifierNameSyntax identifier)
                        {
                            name = instance + "." + identifier.Identifier.ValueText;
                        }
                        else if (be.Left is ImplicitElementAccessSyntax implicitSyntax)
                        {
                            name = SyntaxFactory.ElementAccessExpression(SyntaxFactory.IdentifierName(instance),
                                    implicitSyntax.ArgumentList.WithoutTrivia()).ToString();
                        }
                        else
                        {
                            name = instance;
                        }

                        ConvertInitializers(syntax.Expressions, name, statements, info.nested);
                    }
                    else
                    {
                        if (be.Left is ImplicitElementAccessSyntax indexerKeys)
                        {
                            be = be.WithLeft(SyntaxFactory.ElementAccessExpression(SyntaxFactory.IdentifierName(instance),
                                    indexerKeys.ArgumentList.WithoutTrivia()));
                        }
                        else
                        {
                            var identifier = (IdentifierNameSyntax)be.Left;
                            be =
                                be.WithLeft(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(instance),
                                    SyntaxFactory.IdentifierName(identifier.Identifier.ValueText)));
                        }

                        be = be.WithRight(be.Right.WithoutTrivia());
                        be = be.WithoutTrivia();

                        statements.Add(SyntaxFactory.ExpressionStatement(be, SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
                    }
                }
            }
        }

        public override SyntaxNode VisitThrowExpression(ThrowExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitThrowExpression(node);
            }

            if (node.Parent is ExpressionStatementSyntax es && es.Expression == node || node.Parent is ThrowStatementSyntax)
            {
                return base.VisitThrowExpression(node);
            }

            Microsoft.CodeAnalysis.TypeInfo typeInfo;
            try
            {
                typeInfo = semanticModel.GetTypeInfo(node);
            }
            catch (ArgumentException)
            {
                return base.VisitThrowExpression(node);
            }
            var pos = node.GetLocation().SourceSpan.Start;

            node = (ThrowExpressionSyntax)base.VisitThrowExpression(node);

            if ((typeInfo.ConvertedType ?? typeInfo.Type) != null)
            {
                var type = typeInfo.ConvertedType ?? typeInfo.Type;

                var invocation = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.ParenthesizedExpression(
                            SyntaxFactory.CastExpression(
                                SyntaxFactory.QualifiedName(
                                    SyntaxFactory.IdentifierName(SYSTEM_IDENTIFIER),
                                    SyntaxFactory.GenericName(
                                        SyntaxFactory.Identifier(FUNC_IDENTIFIER))
                                    .WithTypeArgumentList(
                                        SyntaxFactory.TypeArgumentList(
                                            SyntaxFactory.SingletonSeparatedList(
                                                SyntaxFactory.ParseTypeName(type.ToMinimalDisplayString(semanticModel, pos))
                                                ))
                                        .WithLessThanToken(
                                            SyntaxFactory.Token(SyntaxKind.LessThanToken))
                                        .WithGreaterThanToken(
                                            SyntaxFactory.Token(SyntaxKind.GreaterThanToken))))
                                .WithDotToken(
                                    SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.ParenthesizedExpression(
                                    SyntaxFactory.ParenthesizedLambdaExpression(
                                        SyntaxFactory.Block(
                                            SyntaxFactory.SingletonList<StatementSyntax>(
                                                SyntaxFactory.ThrowStatement(node.Expression.WithoutTrivia())
                                                    .WithThrowKeyword(SyntaxFactory.Token(SyntaxKind.ThrowKeyword).WithTrailingTrivia(SyntaxFactory.Space))))
                                        .WithOpenBraceToken(
                                            SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
                                        .WithCloseBraceToken(
                                            SyntaxFactory.Token(SyntaxKind.CloseBraceToken)))
                                    .WithParameterList(
                                        SyntaxFactory.ParameterList()
                                        .WithOpenParenToken(
                                            SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                                        .WithCloseParenToken(
                                            SyntaxFactory.Token(SyntaxKind.CloseParenToken)))
                                    .WithArrowToken(
                                        SyntaxFactory.Token(SyntaxKind.EqualsGreaterThanToken)))
                                .WithOpenParenToken(
                                    SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                                .WithCloseParenToken(
                                    SyntaxFactory.Token(SyntaxKind.CloseParenToken)))
                            .WithOpenParenToken(
                                SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                            .WithCloseParenToken(
                                SyntaxFactory.Token(SyntaxKind.CloseParenToken)))
                        .WithOpenParenToken(
                            SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                        .WithCloseParenToken(
                            SyntaxFactory.Token(SyntaxKind.CloseParenToken)))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList()
                        .WithOpenParenToken(
                            SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                        .WithCloseParenToken(
                            SyntaxFactory.Token(SyntaxKind.CloseParenToken)));

                return invocation;
            }

            return node;
        }

        public override SyntaxNode VisitTryStatement(TryStatementSyntax node)
        {
            var replace = node.Catches.Any(c => c.Filter != null);
            var parent = node.Parent;

            if (replace)
            {
                while (parent != null && !(parent is MethodDeclarationSyntax) && !(parent is ClassDeclarationSyntax))
                {
                    parent = parent.Parent;
                }
            }

            node = (TryStatementSyntax)base.VisitTryStatement(node);

            List<CatchClauseSyntax> catches = new List<CatchClauseSyntax>();

            if (replace)
            {
                string instance = "_e" + ++IndexInstance;
                if (parent != null)
                {
                    var info = LocalUsageGatherer.GatherInfo(semanticModel, parent);
                    while (info.DirectlyOrIndirectlyUsedLocals.Any(s => s.Name == instance) || info.Names.Contains(instance))
                    {
                        instance = "_e" + ++IndexInstance;
                    }
                }

                List<StatementSyntax> statements = new List<StatementSyntax>();
                statements.Add(CreateIfForCatch(node.Catches, 0, instance));

                var catchDeclaration = SyntaxFactory.CatchDeclaration(SyntaxFactory.ParseTypeName(CS.Types.System.Exception.NAME), SyntaxFactory.Identifier(instance));
                catches.Add(SyntaxFactory.CatchClause(catchDeclaration, null, SyntaxFactory.Block(statements)));
            }

            return replace ? node.WithCatches(SyntaxFactory.List(catches)).NormalizeWhitespace() : node;
        }

        private IfStatementSyntax CreateIfForCatch(SyntaxList<CatchClauseSyntax> catches, int index, string varName)
        {
            var catchItem = catches[index];
            ExpressionSyntax condition = SyntaxFactory.BinaryExpression(SyntaxKind.IsExpression,
                    SyntaxFactory.IdentifierName(varName), catchItem.Declaration.Type);

            if (catchItem.Filter != null)
            {
                var methodIdentifier = SyntaxFactory.IdentifierName("global::H5.Script.SafeFunc");
                var lambda = SyntaxFactory.ParenthesizedLambdaExpression(SyntaxFactory.ParameterList(), !catchItem.Declaration.Identifier.IsKind(SyntaxKind.None) ? new IdentifierReplacer(catchItem.Declaration.Identifier.Value.ToString(), SyntaxFactory.ParenthesizedExpression(SyntaxFactory.CastExpression(catchItem.Declaration.Type, SyntaxFactory.IdentifierName(varName)))).Replace(catchItem.Filter.FilterExpression) : catchItem.Filter.FilterExpression);
                var invocation = SyntaxFactory.InvocationExpression(methodIdentifier, SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(
                    lambda
                    ) })));

                condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, invocation);
            }

            BlockSyntax block = catchItem.Block.WithoutTrivia();

            if (!catchItem.Declaration.Identifier.IsKind(SyntaxKind.None))
            {
                var variableStatement = SyntaxFactory.LocalDeclarationStatement(SyntaxFactory.VariableDeclaration(catchItem.Declaration.Type,
                    SyntaxFactory.SeparatedList(new[] { SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(catchItem.Declaration.Identifier.Text)).WithInitializer(
                        SyntaxFactory.EqualsValueClause(SyntaxFactory.CastExpression(catchItem.Declaration.Type, SyntaxFactory.IdentifierName(varName)))
                    ) })));

                block = block.WithStatements(block.Statements.Insert(0, variableStatement));
            }

            var ifStatement = index < (catches.Count - 1) ?
                                SyntaxFactory.IfStatement(condition, block, SyntaxFactory.ElseClause(CreateIfForCatch(catches, index + 1, varName))) :
                                SyntaxFactory.IfStatement(condition, block, SyntaxFactory.ElseClause(SyntaxFactory.ThrowStatement()));

            return ifStatement;
        }

        private class ConditionalAccessInfo
        {
            public ConditionalAccessInfo(SemanticModel semanticModel, ConditionalAccessExpressionSyntax node)
            {
                Node = node;
                var trueSymbolNode = semanticModel.GetSymbolInfo(node.WhenNotNull).Symbol;

                var expressionType = semanticModel.GetTypeInfo(node.Expression).Type;
                ExpressionType = SyntaxFactory.ParseTypeName(expressionType.ToMinimalDisplayString(semanticModel, node.Expression.GetLocation().SourceSpan.Start));
                IsNullable = expressionType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;

                if (IsNullable)
                {
                    UnderlyingNullableType = ((INamedTypeSymbol)expressionType).TypeArguments[0];
                    ExpressionType = SyntaxFactory.ParseTypeName(UnderlyingNullableType.ToMinimalDisplayString(semanticModel, node.Expression.GetLocation().SourceSpan.Start));
                }

                var resultType = semanticModel.GetTypeInfo(node).Type;
                ResultType = SyntaxFactory.ParseTypeName(resultType.ToMinimalDisplayString(semanticModel, node.GetLocation().SourceSpan.Start));

                IsResultVoid = resultType.SpecialType == SpecialType.System_Void;
                IsComplex = IsExpressionComplexEnoughToGetATemporaryVariable.IsComplex(semanticModel, node.Expression);

                if (trueSymbolNode != null && trueSymbolNode is IFieldSymbol && trueSymbolNode.ContainingType.IsTupleType)
                {
                    var field = trueSymbolNode as IFieldSymbol;
                    var tupleField = field.CorrespondingTupleField;
                    TupleField = "." + tupleField.Name;
                }
            }

            public ConditionalAccessExpressionSyntax Node;
            public TypeSyntax ResultType;
            public TypeSyntax ExpressionType;
            public bool IsResultVoid;
            public bool IsComplex;
            public bool IsNullable;
            public ITypeSymbol UnderlyingNullableType;
            public string TupleField;
        }

        public override SyntaxNode VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
        {
            if (node.Parent is ConditionalAccessExpressionSyntax)
            {
                return base.VisitConditionalAccessExpression(node);
            }

            var infos = new List<ConditionalAccessInfo> { new ConditionalAccessInfo(semanticModel, node) };

            var conditionNode = node.WhenNotNull as ConditionalAccessExpressionSyntax;
            while (conditionNode != null)
            {
                infos.Add(new ConditionalAccessInfo(semanticModel, conditionNode));
                conditionNode = conditionNode.WhenNotNull as ConditionalAccessExpressionSyntax;
            }

            bool needParenthesized = node.Parent is BinaryExpressionSyntax
                                     || node.Parent is CastExpressionSyntax
                                     || node.Parent is AwaitExpressionSyntax
                                     || node.Parent is PostfixUnaryExpressionSyntax
                                     || node.Parent is PrefixUnaryExpressionSyntax;

            node = (ConditionalAccessExpressionSyntax)base.VisitConditionalAccessExpression(node);
            var idx = 0;
            infos[idx++].Node = node;
            conditionNode = node.WhenNotNull as ConditionalAccessExpressionSyntax;
            while (conditionNode != null)
            {
                infos[idx++].Node = conditionNode;
                conditionNode = conditionNode.WhenNotNull as ConditionalAccessExpressionSyntax;
            }

            ExpressionSyntax parentTarget = null;
            List<BinaryExpressionSyntax> conditions = new List<BinaryExpressionSyntax>();
            foreach (var info in infos)
            {
                ExpressionSyntax leftForCondition;
                if (info.IsComplex)
                {
                    var tempKeyName = GetUniqueTempKey("cond_access");
                    var keyArg = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(tempKeyName));
                    var methodIdentifier = SyntaxFactory.IdentifierName("global::H5.Script.ToTemp");
                    var arg = parentTarget != null
                        ? SyntaxFactory.ParseExpression(parentTarget.ToString() + info.Node.Expression.WithoutTrivia().ToString())
                        : info.Node.Expression.WithoutTrivia();

                    leftForCondition = SyntaxFactory.InvocationExpression(methodIdentifier,
                        SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(keyArg), SyntaxFactory.Argument(arg) })));

                    var parentMethodIdentifier = SyntaxFactory.GenericName(SyntaxFactory.Identifier("global::H5.Script.FromTemp"),
                                                                 SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new[] { info.ExpressionType })));
                    var invocation = SyntaxFactory.InvocationExpression(parentMethodIdentifier,
                        SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(keyArg) })));

                    parentTarget = SyntaxFactory.ParseExpression(invocation.ToString());
                }
                else
                {
                    leftForCondition = SyntaxFactory.ParseExpression(parentTarget != null ? (parentTarget.ToString() + info.Node.Expression.WithoutTrivia().ToString()) : (info.Node.Expression.WithoutTrivia().ToString()));
                    parentTarget = SyntaxFactory.ParseExpression(parentTarget != null ? (parentTarget.ToString() + info.Node.Expression.WithoutTrivia().ToString() + (info.IsNullable ? ".Value" : "")) : (info.Node.Expression.WithoutTrivia().ToString() + (info.IsNullable ? ".Value" : "")));
                }

                conditions.Add(SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, leftForCondition, SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)));
            }

            BinaryExpressionSyntax condition = null;

            if (conditions.Count == 1)
            {
                condition = conditions[0];
            }
            else
            {
                condition = conditions[0];

                for (int i = 1; i < conditions.Count; i++)
                {
                    condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, conditions[i]);
                }
            }

            ConditionalAccessInfo lastInfo = infos.Last();
            ExpressionSyntax whenTrue = SyntaxFactory.ParseExpression(parentTarget.ToString() + (lastInfo.TupleField ?? lastInfo.Node.WhenNotNull.WithoutTrivia().ToString()));

            if (lastInfo.IsResultVoid && lastInfo.Node.WhenNotNull is InvocationExpressionSyntax)
            {
                var methodIdentifier = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("global::H5.Script.FromLambda"));
                var invocation = SyntaxFactory.InvocationExpression(methodIdentifier,
                    SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(SyntaxFactory.ParenthesizedLambdaExpression(whenTrue)) })));
                whenTrue = invocation;
            }

            ExpressionSyntax whenFalse = lastInfo.IsResultVoid ? (ExpressionSyntax)SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression) : SyntaxFactory.CastExpression(lastInfo.ResultType, SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));

            var newNode = needParenthesized ? SyntaxFactory.ParenthesizedExpression(SyntaxFactory.ConditionalExpression(condition, whenTrue, whenFalse)) :
                                       (SyntaxNode)SyntaxFactory.ConditionalExpression(condition, whenTrue, whenFalse);

            return newNode.WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
        }
    }
}