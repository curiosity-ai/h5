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
        private Stack<Dictionary<ISymbol, string>> _primaryConstructorCaptures = new Stack<Dictionary<ISymbol, string>>();

        public SharpSixRewriter(ITranslator translator)
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

            _cachedRewrittenData.ClearIfConfigHashChanged(configHash, force: !translator.AssemblyInfo.EnableCache || translator.Rebuild);
        }

        public SharpSixRewriter Clone()
        {
            return new SharpSixRewriter(this);
        }

        private SharpSixRewriter(SharpSixRewriter rewriter)
        {
            translator = rewriter.translator;
            compilation = rewriter.compilation;
            _cachedRewrittenData = rewriter._cachedRewrittenData;
        }


        private string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        private void WriteDebugFile(int index, string rewrittenCode)
        {
            var fileName = translator.SourceFiles[index];
            var projectPath = Path.GetDirectoryName(translator.Location);
            var relativePath = GetRelativePath(fileName, projectPath);

            if (relativePath.StartsWith(".."))
            {
                relativePath = Path.GetFileName(fileName);
            }

            var tempFilePath = Path.Combine(Path.GetTempPath(), "h5", "rewritten", GetAssemblyName(), relativePath);
            var dir = Path.GetDirectoryName(tempFilePath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(tempFilePath, rewrittenCode);
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

        private bool TryGetFromCache(int index, UID128 sourceHash, out string cached)
        {
#if DEBUG
            cached = null;
            return false;
#endif
            var fileName = translator.SourceFiles[index];

            if(_cachedRewrittenData.CachedCompilation.TryGetValue(fileName, out var cachedData) && cachedData.hash == sourceHash)
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

        private string AddToCache(int index, string rewritten, UID128 sourceHash)
        {
            //TODO: use https://www.nuget.org/packages/CSharpMinifier/
            var fileName = translator.SourceFiles[index];

            _cachedRewrittenData.CachedCompilation[fileName] = (sourceHash, rewritten);

            return rewritten;
        }


        public string Rewrite(int index)
        {
            var sourceText = File.ReadAllText(translator.SourceFiles[index]);
            var sourceHash = sourceText.Hash128();
            var debugRewrite = sourceText.TrimStart().StartsWith("//DEBUG REWRITE");

            if (TryGetFromCache(index, sourceHash, out var cached))
            {
                if (debugRewrite)
                {
                    WriteDebugFile(index, cached);
                }
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
            replacers.Add(new MethodImplAttributeRewriter());

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

            var rewritten = newTree.GetRoot().ToFullString();
            AddToCache(index, rewritten, sourceHash);

            if (debugRewrite)
            {
                WriteDebugFile(index, rewritten);
            }

            return rewritten;
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
            LanguageVersion languageVersion = LanguageVersion.CSharp7_2;

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
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

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

        private static ITypeSymbol GetCollectionElementType(ITypeSymbol type)
        {
            if (type is IArrayTypeSymbol arrayType)
            {
                return arrayType.ElementType;
            }

            if (type.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T && type is INamedTypeSymbol named && named.TypeArguments.Length > 0)
            {
                return named.TypeArguments[0];
            }

            foreach (var iface in type.AllInterfaces)
            {
                if (iface.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T)
                {
                    return iface.TypeArguments[0];
                }
            }

            if (type is INamedTypeSymbol namedType && (namedType.Name == "Span" || namedType.Name == "ReadOnlySpan") &&
                namedType.ContainingNamespace?.ToDisplayString() == "System" && namedType.TypeArguments.Length == 1)
            {
                return namedType.TypeArguments[0];
            }

            return null;
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

            var lastArg = arguments[arguments.Count - 1];
            var lastType = semanticModel.GetTypeInfo(lastArg.Expression).ConvertedType;
            var paramType = parameters[parameters.Length - 1].Type;

            // Check if normal form is applicable
            var conversion = semanticModel.ClassifyConversion(lastArg.Expression, paramType);
            if (conversion.Exists && conversion.IsImplicit)
            {
                return false;
            }

            return true;
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

        public override SyntaxNode VisitCastExpression(CastExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitCastExpression(node);
            }

            var methodSymbol = semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;

            if (methodSymbol != null && (methodSymbol.MethodKind == MethodKind.Conversion) && methodSymbol.Name.StartsWith("op_Checked"))
            {
                var expression = (ExpressionSyntax)Visit(node.Expression);

                var typeSyntax = SyntaxHelper.GenerateTypeSyntax(methodSymbol.ContainingType, semanticModel, node.SpanStart, this);

                var memberAccess = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    typeSyntax,
                    SyntaxFactory.IdentifierName(methodSymbol.Name));

                return SyntaxFactory.InvocationExpression(memberAccess,
                    SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Argument(expression)
                    )))
                    .WithLeadingTrivia(node.GetLeadingTrivia())
                    .WithTrailingTrivia(node.GetTrailingTrivia());
            }

            return base.VisitCastExpression(node);
        }

        public override SyntaxNode VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitBinaryExpression(node);
            }

            var methodSymbol = semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;

            if (methodSymbol != null && methodSymbol.MethodKind == MethodKind.UserDefinedOperator && methodSymbol.Name.StartsWith("op_Checked"))
            {
                var left = (ExpressionSyntax)Visit(node.Left);
                var right = (ExpressionSyntax)Visit(node.Right);

                var typeSyntax = SyntaxHelper.GenerateTypeSyntax(methodSymbol.ContainingType, semanticModel, node.SpanStart, this);

                var memberAccess = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    typeSyntax,
                    SyntaxFactory.IdentifierName(methodSymbol.Name));

                return SyntaxFactory.InvocationExpression(memberAccess,
                    SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] {
                        SyntaxFactory.Argument(left),
                        SyntaxFactory.Argument(right)
                    })))
                    .WithLeadingTrivia(node.GetLeadingTrivia())
                    .WithTrailingTrivia(node.GetTrailingTrivia());
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

        public override SyntaxNode VisitForEachStatement(ForEachStatementSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitForEachStatement(node);
            }

            var info = semanticModel.GetForEachStatementInfo(node);
            if (info.GetEnumeratorMethod != null && info.GetEnumeratorMethod.IsExtensionMethod)
            {
                var collection = (ExpressionSyntax)Visit(node.Expression);
                var enumeratorMethod = info.GetEnumeratorMethod;

                var enumeratorVarName = GetUniqueTempKey("enumerator");
                var enumeratorType = enumeratorMethod.ReturnType;
                var enumeratorTypeSyntax = SyntaxHelper.GenerateTypeSyntax(enumeratorType, semanticModel, node.SpanStart, this);

                var typeName = enumeratorMethod.ContainingType.FullyQualifiedName();
                if (typeName.StartsWith("global::"))
                {
                    typeName = typeName.Substring(8);
                }

                var getEnumeratorCall = SyntaxHelper.GenerateStaticMethodCall(
                    enumeratorMethod.Name,
                    typeName,
                    new[] { SyntaxFactory.Argument(collection) },
                    enumeratorMethod.TypeArguments.ToArray()
                ).Expression;

                var enumeratorDecl = SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName("var"))
                    .WithVariables(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(enumeratorVarName)
                        .WithInitializer(SyntaxFactory.EqualsValueClause(getEnumeratorCall))
                    ))
                ).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)).NormalizeWhitespace();

                var moveNextCall = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(enumeratorVarName), SyntaxFactory.IdentifierName("MoveNext")),
                    SyntaxFactory.ArgumentList()
                );

                var loopBody = (StatementSyntax)Visit(node.Statement);
                if (!(loopBody is BlockSyntax))
                {
                    loopBody = SyntaxFactory.Block(loopBody);
                }

                var currentAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(enumeratorVarName), SyntaxFactory.IdentifierName("Current"));

                var iterationVarDecl = SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(node.Type)
                    .WithVariables(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(node.Identifier)
                        .WithInitializer(SyntaxFactory.EqualsValueClause(currentAccess))
                    ))
                ).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)).NormalizeWhitespace();

                var newLoopBody = SyntaxFactory.Block(iterationVarDecl).AddStatements(((BlockSyntax)loopBody).Statements.ToArray());

                var whileLoop = SyntaxFactory.WhileStatement(moveNextCall, newLoopBody);

                var script = "if (H5.is({0}, System.IDisposable)) H5.cast({0}, System.IDisposable).System$IDisposable$Dispose();";
                var writeCall = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ParseName("global::H5.Script"), SyntaxFactory.IdentifierName("Write")),
                    SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] {
                        SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(script))),
                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName(enumeratorVarName))
                    }))
                );

                var disposeCheck = SyntaxFactory.ExpressionStatement(writeCall).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                var tryFinally = SyntaxFactory.TryStatement(
                    SyntaxFactory.Block(whileLoop),
                    SyntaxFactory.List<CatchClauseSyntax>(),
                    SyntaxFactory.FinallyClause(SyntaxFactory.Block(disposeCheck))
                );

                return SyntaxFactory.Block(enumeratorDecl, tryFinally);
            }

            return base.VisitForEachStatement(node);
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

            if (symbol is ILocalSymbol ls && ls.IsRef ||
                symbol is IMethodSymbol ms && ms.ReturnsByRef ||
                symbol is IPropertySymbol ps && ps.RefKind != RefKind.None)
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
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitLiteralExpression(node);
            }

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
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitTupleExpression(node);
            }

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
                var createExpression = SyntaxFactory.ObjectCreationExpression(SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName("System"), SyntaxFactory.GenericName(SyntaxFactory.Identifier("ValueTuple"), SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(types)))));
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

            var newType = SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName("System"), SyntaxFactory.GenericName(SyntaxFactory.Identifier("ValueTuple"), SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(types))));

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

        public override SyntaxNode VisitBlock(BlockSyntax node)
        {
            var newStatements = ProcessBlockStatements(node.Statements);
            return node.WithStatements(newStatements);
        }

        public override SyntaxNode VisitLockStatement(LockStatementSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitLockStatement(node);
            }

            var typeInfo = semanticModel.GetTypeInfo(node.Expression);
            var type = typeInfo.Type ?? typeInfo.ConvertedType;

            if (type != null && type.Name == "Lock" && type.ContainingNamespace?.ToString() == "System.Threading")
            {
                var memberAccess = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    node.Expression,
                    SyntaxFactory.IdentifierName("EnterScope")
                );

                var invocation = SyntaxFactory.InvocationExpression(memberAccess);

                var usingStmt = SyntaxFactory.UsingStatement(
                    null,
                    invocation,
                    node.Statement
                ).WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());

                return Visit(usingStmt);
            }

            return base.VisitLockStatement(node);
        }

        private SyntaxList<StatementSyntax> ProcessBlockStatements(IEnumerable<StatementSyntax> statementsEnumerable)
        {
            var statements = statementsEnumerable.ToList();
            var newStatements = new List<StatementSyntax>();
            for (int i = 0; i < statements.Count; i++)
            {
                var stmt = statements[i];
                var fs = stmt.ToFullString();

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
                var (stabilizedLeft, accessLeft) = StabilizeLValue(node.Left, assignment.Left);

                return SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    stabilizedLeft,
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.CoalesceExpression,
                        accessLeft,
                        assignment.Right
                    )
                ).NormalizeWhitespace().WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }

            return newNode;
        }

        private (ExpressionSyntax stabilized, ExpressionSyntax access) StabilizeLValue(ExpressionSyntax original, ExpressionSyntax rewritten)
        {
            if (original is IdentifierNameSyntax || !IsExpressionComplexEnoughToGetATemporaryVariable.IsComplex(semanticModel, original))
            {
                return (rewritten, rewritten);
            }

            if (original is MemberAccessExpressionSyntax maeOriginal && rewritten is MemberAccessExpressionSyntax maeRewritten)
            {
                var (stabilizedExpr, accessExpr) = StabilizeLValue(maeOriginal.Expression, maeRewritten.Expression);
                return (maeRewritten.WithExpression(stabilizedExpr), maeRewritten.WithExpression(accessExpr));
            }

            if (original is ElementAccessExpressionSyntax eaeOriginal && rewritten is ElementAccessExpressionSyntax eaeRewritten)
            {
                var (stabilizedExpr, accessExpr) = StabilizeLValue(eaeOriginal.Expression, eaeRewritten.Expression);

                if (eaeOriginal.ArgumentList.Arguments.Count == eaeRewritten.ArgumentList.Arguments.Count)
                {
                    var stabilizedArgs = new List<SyntaxNodeOrToken>();
                    var accessArgs = new List<SyntaxNodeOrToken>();

                    for (int i = 0; i < eaeOriginal.ArgumentList.Arguments.Count; i++)
                    {
                        var argOriginal = eaeOriginal.ArgumentList.Arguments[i];
                        var argRewritten = eaeRewritten.ArgumentList.Arguments[i];

                        var (sArg, aArg) = StabilizeRValue(argOriginal.Expression, argRewritten.Expression);

                        if (i > 0)
                        {
                            stabilizedArgs.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                            accessArgs.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                        }

                        stabilizedArgs.Add(argRewritten.WithExpression(sArg));
                        accessArgs.Add(argRewritten.WithExpression(aArg));
                    }

                    return (eaeRewritten.WithExpression(stabilizedExpr).WithArgumentList(SyntaxFactory.BracketedArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(stabilizedArgs))),
                            eaeRewritten.WithExpression(accessExpr).WithArgumentList(SyntaxFactory.BracketedArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(accessArgs))));
                }
            }

            return StabilizeRValue(original, rewritten);
        }

        private (ExpressionSyntax stabilized, ExpressionSyntax access) StabilizeRValue(ExpressionSyntax original, ExpressionSyntax rewritten)
        {
            if (!IsExpressionComplexEnoughToGetATemporaryVariable.IsComplex(semanticModel, original))
            {
                return (rewritten, rewritten);
            }

            var tempKey = GetUniqueTempKey("coal");
            var typeInfo = semanticModel.GetTypeInfo(original);
            var type = typeInfo.Type ?? typeInfo.ConvertedType;
            var typeName = type?.ToMinimalDisplayString(semanticModel, original.SpanStart) ?? "object";

            // H5.Script.ToTemp(key, value)
            var toTempArgs = new List<SyntaxNodeOrToken>();
            toTempArgs.Add(SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(tempKey))));
            toTempArgs.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
            toTempArgs.Add(SyntaxFactory.Argument(rewritten));

            var toTemp = SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ParseName("global::H5.Script"), SyntaxFactory.IdentifierName("ToTemp")),
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(toTempArgs))
            );

            // H5.Script.FromTemp<T>(key)
            var fromTemp = SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ParseName("global::H5.Script"),
                    SyntaxFactory.GenericName("FromTemp").WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.ParseTypeName(typeName))))
                ),
                SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(tempKey)))))
            );

            return (toTemp, fromTemp);
        }

        private SyntaxList<UsingDirectiveSyntax> VisitUsings(SyntaxList<UsingDirectiveSyntax> usings, ref SyntaxTriviaList? pendingTrivia)
        {
            var newUsings = new List<UsingDirectiveSyntax>();
            foreach (var u in usings)
            {
                var visited = (UsingDirectiveSyntax)Visit(u);
                if (visited == null)
                {
                    var leading = u.GetLeadingTrivia();
                    var trailing = u.GetTrailingTrivia();

                    if (pendingTrivia == null)
                    {
                        pendingTrivia = leading;
                    }
                    else
                    {
                        pendingTrivia = pendingTrivia.Value.AddRange(leading);
                    }

                    pendingTrivia = pendingTrivia.Value.AddRange(trailing);
                }
                else
                {
                    if (pendingTrivia != null)
                    {
                        visited = visited.WithLeadingTrivia(pendingTrivia.Value.AddRange(visited.GetLeadingTrivia()));
                        pendingTrivia = null;
                    }
                    newUsings.Add(visited);
                }
            }
            return SyntaxFactory.List(newUsings);
        }

        public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            var externs = VisitList(node.Externs);

            SyntaxTriviaList? pendingTrivia = null;
            var usings = VisitUsings(node.Usings, ref pendingTrivia);

            var members = VisitList(node.Members);
            if (pendingTrivia != null && members.Count > 0)
            {
                var first = members[0];
                members = members.Replace(first, first.WithLeadingTrivia(pendingTrivia.Value.AddRange(first.GetLeadingTrivia())));
                pendingTrivia = null;
            }

            var result = node
                .WithExterns(externs)
                .WithUsings(usings)
                .WithMembers(members);

            if (pendingTrivia != null)
            {
                result = result.WithCloseBraceToken(result.CloseBraceToken.WithLeadingTrivia(pendingTrivia.Value.AddRange(result.CloseBraceToken.LeadingTrivia)));
            }

            return result;
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

            var externs = VisitList(node.Externs);

            SyntaxTriviaList? pendingTrivia = null;
            var usings = VisitUsings(node.Usings, ref pendingTrivia);

            var attributeLists = VisitList(node.AttributeLists);
            if (pendingTrivia != null && attributeLists.Count > 0)
            {
                var first = attributeLists[0];
                attributeLists = attributeLists.Replace(first, first.WithLeadingTrivia(pendingTrivia.Value.AddRange(first.GetLeadingTrivia())));
                pendingTrivia = null;
            }

            var members = VisitList(node.Members);
            if (pendingTrivia != null && members.Count > 0)
            {
                var first = members[0];
                members = members.Replace(first, first.WithLeadingTrivia(pendingTrivia.Value.AddRange(first.GetLeadingTrivia())));
                pendingTrivia = null;
            }

            var result = node
                .WithExterns(externs)
                .WithUsings(usings)
                .WithAttributeLists(attributeLists)
                .WithMembers(members);

            if (pendingTrivia != null)
            {
                result = result.WithEndOfFileToken(result.EndOfFileToken.WithLeadingTrivia(pendingTrivia.Value.AddRange(result.EndOfFileToken.LeadingTrivia)));
            }

            return result;
        }

        public override SyntaxNode VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
        {
            var externs = VisitList(node.Externs);

            SyntaxTriviaList? pendingTrivia = null;
            var usings = VisitUsings(node.Usings, ref pendingTrivia);

            var members = new List<MemberDeclarationSyntax>();
            bool firstMember = true;
            foreach (var member in node.Members)
            {
                var visited = Visit(member) as MemberDeclarationSyntax;
                if (visited != null)
                {
                    if (firstMember && pendingTrivia != null)
                    {
                        visited = visited.WithLeadingTrivia(pendingTrivia.Value.AddRange(visited.GetLeadingTrivia()));
                        pendingTrivia = null;
                    }
                    members.Add(visited);
                    firstMember = false;
                }
            }

            var nsDecl = SyntaxFactory.NamespaceDeclaration(
                node.Name,
                externs,
                usings,
                SyntaxFactory.List(members)
            )
            .WithNamespaceKeyword(SyntaxFactory.Token(SyntaxKind.NamespaceKeyword).WithTrailingTrivia(SyntaxFactory.Space))
            .WithLeadingTrivia(node.GetLeadingTrivia());

            if (pendingTrivia != null)
            {
                nsDecl = nsDecl.WithCloseBraceToken(nsDecl.CloseBraceToken.WithLeadingTrivia(pendingTrivia.Value.AddRange(nsDecl.CloseBraceToken.LeadingTrivia)));
            }

            return nsDecl;
        }

        public override SyntaxNode VisitWithExpression(WithExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitWithExpression(node);
            }

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

            statements.Add(SyntaxFactory.ReturnStatement(SyntaxFactory.IdentifierName(tempParamName).WithLeadingTrivia(SyntaxFactory.Space)));

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
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitSwitchExpression(node);
            }

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

        public override SyntaxNode VisitRangeExpression(RangeExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitRangeExpression(node);
            }

            ExpressionSyntax start = null;
            ExpressionSyntax end = null;

            if (node.LeftOperand != null)
            {
                start = (ExpressionSyntax)Visit(node.LeftOperand);
            }
            else
            {
                start = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.ParseTypeName("System.Index"),
                    SyntaxFactory.IdentifierName("Start"));
            }

            if (node.RightOperand != null)
            {
                end = (ExpressionSyntax)Visit(node.RightOperand);
            }
            else
            {
                end = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.ParseTypeName("System.Index"),
                    SyntaxFactory.IdentifierName("End"));
            }

            return SyntaxFactory.ObjectCreationExpression(SyntaxFactory.ParseTypeName("System.Range"))
                .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] {
                    SyntaxFactory.Argument(start),
                    SyntaxFactory.Argument(end)
                })))
                .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                .WithLeadingTrivia(node.GetLeadingTrivia())
                .WithTrailingTrivia(node.GetTrailingTrivia());
        }

        public override SyntaxNode VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitPrefixUnaryExpression(node);
            }

            var methodSymbol = semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;

            if (methodSymbol != null && methodSymbol.MethodKind == MethodKind.UserDefinedOperator && methodSymbol.Name.StartsWith("op_Checked"))
            {
                if (methodSymbol.Name == "op_CheckedIncrement" || methodSymbol.Name == "op_CheckedDecrement")
                {
                    throw new NotSupportedException("Checked increment/decrement operators are not supported.");
                }

                var operand = (ExpressionSyntax)Visit(node.Operand);

                var typeSyntax = SyntaxHelper.GenerateTypeSyntax(methodSymbol.ContainingType, semanticModel, node.SpanStart, this);

                var memberAccess = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    typeSyntax,
                    SyntaxFactory.IdentifierName(methodSymbol.Name));

                return SyntaxFactory.InvocationExpression(memberAccess,
                    SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Argument(operand)
                    )))
                    .WithLeadingTrivia(node.GetLeadingTrivia())
                    .WithTrailingTrivia(node.GetTrailingTrivia());
            }

            if (node.IsKind(SyntaxKind.IndexExpression)) // ^ expression
            {
                var operand = (ExpressionSyntax)Visit(node.Operand);

                return SyntaxFactory.ObjectCreationExpression(SyntaxFactory.ParseTypeName("System.Index"))
                    .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] {
                        SyntaxFactory.Argument(operand),
                        SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression))
                    })))
                    .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                    .WithLeadingTrivia(node.GetLeadingTrivia())
                    .WithTrailingTrivia(node.GetTrailingTrivia());
            }

            return base.VisitPrefixUnaryExpression(node);
        }

        public override SyntaxNode VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitElementAccessExpression(node);
            }

            bool hasIndexOrRange = false;
            foreach (var arg in node.ArgumentList.Arguments)
            {
                if (arg.Expression.IsKind(SyntaxKind.IndexExpression) || arg.Expression.IsKind(SyntaxKind.RangeExpression))
                {
                    hasIndexOrRange = true;
                    break;
                }

                var argTypeInfo = semanticModel.GetTypeInfo(arg.Expression);
                var argType = argTypeInfo.Type ?? argTypeInfo.ConvertedType;
                if (argType != null && argType.ContainingNamespace?.Name == "System" && (argType.Name == "Index" || argType.Name == "Range"))
                {
                    hasIndexOrRange = true;
                    break;
                }
            }

            if (!hasIndexOrRange)
            {
                return base.VisitElementAccessExpression(node);
            }

            var symbolInfo = semanticModel.GetSymbolInfo(node);
            var indexerSymbol = symbolInfo.Symbol as IPropertySymbol;

            if (indexerSymbol == null && symbolInfo.CandidateSymbols.Length > 0)
            {
                indexerSymbol = symbolInfo.CandidateSymbols[0] as IPropertySymbol;
            }

            if (indexerSymbol != null && indexerSymbol.Parameters.Length == node.ArgumentList.Arguments.Count)
            {
                // Check if target indexer expects Index or Range
                bool targetExpectsIndexOrRange = false;
                foreach (var param in indexerSymbol.Parameters)
                {
                     if ((param.Type.Name == "Index" || param.Type.Name == "Range") && param.Type.ContainingNamespace?.Name == "System")
                     {
                         targetExpectsIndexOrRange = true;
                         break;
                     }
                }

                if (targetExpectsIndexOrRange)
                {
                    // Do not rewrite to Length-n or Slice. Just visit arguments to rewrite syntax.
                    var newArgs = new List<ArgumentSyntax>();
                    foreach (var arg in node.ArgumentList.Arguments)
                    {
                        newArgs.Add((ArgumentSyntax)Visit(arg));
                    }
                    return node.WithArgumentList(SyntaxFactory.BracketedArgumentList(SyntaxFactory.SeparatedList(newArgs)));
                }
            }

            var expression = (ExpressionSyntax)Visit(node.Expression);
            var isComplex = IsExpressionComplexEnoughToGetATemporaryVariable.IsComplex(semanticModel, node.Expression);
            string tempKeyName = null;

            if (isComplex)
            {
                tempKeyName = GetUniqueTempKey("idx_expr");
                var keyArg = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(tempKeyName));
                var methodIdentifier = SyntaxFactory.IdentifierName("global::H5.Script.ToTemp");
                expression = SyntaxFactory.InvocationExpression(methodIdentifier,
                    SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(keyArg), SyntaxFactory.Argument(expression) })));
            }

            var newArgsList = new List<ArgumentSyntax>();
            var typeInfo = semanticModel.GetTypeInfo(node.Expression);
            var type = typeInfo.Type ?? typeInfo.ConvertedType;

            foreach (var arg in node.ArgumentList.Arguments)
            {
                if (arg.Expression.IsKind(SyntaxKind.IndexExpression) && arg.Expression is PrefixUnaryExpressionSyntax prefix && prefix.OperatorToken.IsKind(SyntaxKind.CaretToken))
                {
                    ExpressionSyntax lenAccess;
                    if (isComplex)
                    {
                        var keyArg = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(tempKeyName));
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
                        lenAccess = expression;
                    }

                    string lengthProp = "Length";
                    if (type != null && type.GetMembers("Count").Any()) lengthProp = "Count";

                    var lengthAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        lenAccess, SyntaxFactory.IdentifierName(lengthProp));

                    var val = (ExpressionSyntax)Visit(prefix.Operand);

                    var newIdx = SyntaxFactory.BinaryExpression(SyntaxKind.SubtractExpression, lengthAccess, val);
                    newArgsList.Add(arg.WithExpression(newIdx));
                }
                else if (arg.Expression.IsKind(SyntaxKind.RangeExpression) && arg.Expression is RangeExpressionSyntax range)
                {
                    bool isString = type != null && type.SpecialType == SpecialType.System_String;
                    bool isArray = type != null && type.TypeKind == TypeKind.Array;

                    if (isArray || isString)
                    {
                        ExpressionSyntax lenAccess;
                        if (isComplex)
                        {
                            var keyArg = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(tempKeyName));
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
                            lenAccess = expression;
                        }

                        string lengthProp = "Length";
                        var lengthAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            lenAccess, SyntaxFactory.IdentifierName(lengthProp));

                        ExpressionSyntax GetOffset(ExpressionSyntax e)
                        {
                            if (e == null) return null;
                            if (e.IsKind(SyntaxKind.IndexExpression) && e is PrefixUnaryExpressionSyntax p && p.OperatorToken.IsKind(SyntaxKind.CaretToken))
                            {
                                var op = (ExpressionSyntax)Visit(p.Operand);
                                return SyntaxFactory.BinaryExpression(SyntaxKind.SubtractExpression, lengthAccess, op);
                            }
                            return (ExpressionSyntax)Visit(e);
                        }

                        var startExpr = GetOffset(range.LeftOperand);
                        var endExpr = GetOffset(range.RightOperand);

                        if (startExpr == null) startExpr = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(0));

                        if (isString)
                        {
                             ExpressionSyntax lengthExpr;
                             if (endExpr == null)
                             {
                                 lengthExpr = SyntaxFactory.BinaryExpression(SyntaxKind.SubtractExpression, lengthAccess, startExpr);
                             }
                             else
                             {
                                 lengthExpr = SyntaxFactory.BinaryExpression(SyntaxKind.SubtractExpression, endExpr, startExpr);
                             }

                             var memberAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                 expression, SyntaxFactory.IdentifierName("Substring"));

                             var invocation = SyntaxFactory.InvocationExpression(memberAccess,
                                 SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] {
                                     SyntaxFactory.Argument(startExpr),
                                     SyntaxFactory.Argument(lengthExpr)
                                 })));

                             return invocation;
                        }
                        else
                        {
                             var memberAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                 expression, SyntaxFactory.IdentifierName("Slice"));

                             var args = new List<ArgumentSyntax>();
                             args.Add(SyntaxFactory.Argument(startExpr));
                             if (endExpr != null)
                             {
                                 args.Add(SyntaxFactory.Argument(endExpr));
                             }

                             var invocation = SyntaxFactory.InvocationExpression(memberAccess,
                                 SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(args)));

                             return invocation;
                        }
                    }
                    else
                    {
                        newArgsList.Add((ArgumentSyntax)Visit(arg));
                    }
                }
                else
                {
                    var argTypeInfo = semanticModel.GetTypeInfo(arg.Expression);
                    var argType = argTypeInfo.Type ?? argTypeInfo.ConvertedType;
                    bool isString = type != null && type.SpecialType == SpecialType.System_String;
                    bool isArray = type != null && type.TypeKind == TypeKind.Array;

                    if ((isArray || isString) && argType != null && argType.ContainingNamespace?.Name == "System")
                    {
                        if (argType.Name == "Index")
                        {
                            // Rewrite arr[idx] -> arr[idx.GetOffset(Length)]
                            ExpressionSyntax lenAccess;
                            if (isComplex)
                            {
                                var keyArg = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(tempKeyName));
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
                                lenAccess = expression;
                            }
                            string lengthProp = "Length";
                            var lengthAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                lenAccess, SyntaxFactory.IdentifierName(lengthProp));

                            var idxExpr = (ExpressionSyntax)Visit(arg.Expression);
                            var getOffsetCall = SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, idxExpr, SyntaxFactory.IdentifierName("GetOffset")),
                                SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(lengthAccess))));

                            newArgsList.Add(arg.WithExpression(getOffsetCall));
                            continue;
                        }
                        else if (argType.Name == "Range")
                        {
                            // Rewrite arr[range] -> arr.Slice(range.Start.GetOffset(Length), range.End.GetOffset(Length))
                            ExpressionSyntax lenAccess;
                            if (isComplex)
                            {
                                var keyArg = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(tempKeyName));
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
                                lenAccess = expression;
                            }
                            string lengthProp = "Length";
                            var lengthAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                lenAccess, SyntaxFactory.IdentifierName(lengthProp));

                            var rangeExpr = (ExpressionSyntax)Visit(arg.Expression); // Visiting rewrites .. to new Range() if needed

                            var startExpr = SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, rangeExpr, SyntaxFactory.IdentifierName("Start")),
                                    SyntaxFactory.IdentifierName("GetOffset")),
                                SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(lengthAccess))));

                            var endExpr = SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, rangeExpr, SyntaxFactory.IdentifierName("End")),
                                    SyntaxFactory.IdentifierName("GetOffset")),
                                SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(lengthAccess))));

                            if (isString)
                            {
                                 // Substring(start, end - start)
                                 var lengthExpr = SyntaxFactory.BinaryExpression(SyntaxKind.SubtractExpression, endExpr, startExpr);
                                 var memberAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                     expression, SyntaxFactory.IdentifierName("Substring"));

                                 var invocation = SyntaxFactory.InvocationExpression(memberAccess,
                                     SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] {
                                         SyntaxFactory.Argument(startExpr),
                                         SyntaxFactory.Argument(lengthExpr)
                                     })));
                                 return invocation;
                            }
                            else
                            {
                                 var memberAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                     expression, SyntaxFactory.IdentifierName("Slice"));

                                 var invocation = SyntaxFactory.InvocationExpression(memberAccess,
                                     SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new [] { SyntaxFactory.Argument(startExpr), SyntaxFactory.Argument(endExpr) })));

                                 return invocation;
                            }
                        }
                    }

                    newArgsList.Add((ArgumentSyntax)Visit(arg));
                }
            }

            return node.WithExpression(expression).WithArgumentList(SyntaxFactory.BracketedArgumentList(SyntaxFactory.SeparatedList(newArgsList)));
        }

        public override SyntaxNode VisitParameter(ParameterSyntax node)
        {
            var originalNode = node;
            node = (ParameterSyntax)base.VisitParameter(node);

            // Fix for C# 13/14 inferred type for ref/out parameters in lambdas: (ref x) => ...
            // NRefactory 5 requires explicit type: (ref int x) => ...
            if (node.Type == null && originalNode.Parent is ParameterListSyntax && originalNode.Parent.Parent is LambdaExpressionSyntax)
            {
                bool hasRef = node.Modifiers.Any(m => m.IsKind(SyntaxKind.RefKeyword));
                bool hasOut = node.Modifiers.Any(m => m.IsKind(SyntaxKind.OutKeyword));
                // 'in' is handled below, but we need the type for it too if it's missing
                bool hasIn = node.Modifiers.Any(m => m.IsKind(SyntaxKind.InKeyword));

                if (hasRef || hasOut || hasIn)
                {
                    var symbol = semanticModel.GetDeclaredSymbol(originalNode);
                    if (symbol != null)
                    {
                        var typeSyntax = SyntaxHelper.GenerateTypeSyntax(symbol.Type, semanticModel, originalNode.SpanStart, this);
                        node = node.WithType(typeSyntax).WithIdentifier(node.Identifier.WithLeadingTrivia(SyntaxFactory.Space));
                    }
                }
            }

            bool hasRefModifier = node.Modifiers.Any(m => m.IsKind(SyntaxKind.RefKeyword));
            bool hasReadOnlyModifier = node.Modifiers.Any(m => m.IsKind(SyntaxKind.ReadOnlyKeyword));

            if (hasRefModifier && hasReadOnlyModifier)
            {
                var newModifiers = node.Modifiers.Where(m => !m.IsKind(SyntaxKind.RefKeyword) && !m.IsKind(SyntaxKind.ReadOnlyKeyword));
                node = node.WithModifiers(SyntaxFactory.TokenList(newModifiers));

                var type = node.Type;
                var refType = SyntaxFactory.QualifiedName(
                   SyntaxFactory.ParseName("global::H5"),
                   SyntaxFactory.GenericName(
                       SyntaxFactory.Identifier("Ref"),
                       SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(type))));
                node = node.WithType(refType.NormalizeWhitespace());
            }

            var idx = node.Modifiers.IndexOf(SyntaxKind.InKeyword);
            if (idx > -1)
            {
                node = node.WithModifiers(node.Modifiers.RemoveAt(idx));

                var type = node.Type;
                var refType = SyntaxFactory.QualifiedName(
                    SyntaxFactory.ParseName("global::H5"),
                    SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier("Ref"),
                        SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(type))));
                node = node.WithType(refType.NormalizeWhitespace());
            }

            return node;
        }

        public override SyntaxNode VisitArgument(ArgumentSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitArgument(node);
            }

            var originalNode = node;
            var expressionSymbol = semanticModel.GetSymbolInfo(node.Expression).Symbol;

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
                    pType = GetCollectionElementType(parameter.Type);
                }

                if (!(node.Expression is CastExpressionSyntax && SymbolEqualityComparer.Default.Equals(type, pType) || parameter.RefKind != RefKind.None))
                {
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
            }

            if (nonTrailing && parameter != null)
            {
                node = node.WithNameColon(SyntaxFactory.NameColon(SyntaxFactory.IdentifierName(parameter.Name)));
            }

            bool isRefInvocation = node.RefKindKeyword.IsKind(SyntaxKind.RefKeyword) && node.Expression is InvocationExpressionSyntax;
            bool isIn = node.RefKindKeyword.IsKind(SyntaxKind.InKeyword);

            if (!isIn && parameter != null && (parameter.RefKind == RefKind.In || parameter.RefKind == RefKind.RefReadOnly))
            {
                isIn = true;
            }

            if (isIn)
            {
                if (expressionSymbol is IParameterSymbol ps && (ps.RefKind == RefKind.In || ps.RefKind == RefKind.RefReadOnly))
                {
                    node = node.WithRefKindKeyword(SyntaxFactory.Token(SyntaxKind.None));
                }
                else
                {
                    bool isLValue = false;
                    if (expressionSymbol is ILocalSymbol || expressionSymbol is IParameterSymbol || expressionSymbol is IFieldSymbol || originalNode.Expression is ElementAccessExpressionSyntax)
                    {
                        isLValue = true;
                    }

                    var exprType = ti.Type ?? ti.ConvertedType;
                    var typeSyntax = SyntaxHelper.GenerateTypeSyntax(exprType, semanticModel, originalNode.Expression.SpanStart, this);

                    if (isLValue)
                    {
                        var createExpression = SyntaxFactory.ObjectCreationExpression(
                             SyntaxFactory.QualifiedName(
                                SyntaxFactory.ParseName("global::H5"),
                                SyntaxFactory.GenericName(
                                    SyntaxFactory.Identifier("Ref"),
                                    SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(
                                        typeSyntax
                                    ))))).WithArgumentList(SyntaxFactory.ArgumentList(
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
                                            SyntaxFactory.Block())
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

                        node = node.WithExpression(createExpression.NormalizeWhitespace()).WithRefKindKeyword(SyntaxFactory.Token(SyntaxKind.None));
                    }
                    else
                    {
                        var createIn = SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                 SyntaxFactory.QualifiedName(
                                    SyntaxFactory.ParseName("global::H5"),
                                    SyntaxFactory.GenericName(
                                        SyntaxFactory.Identifier("Ref"),
                                        SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(
                                            typeSyntax
                                        )))),
                                SyntaxFactory.IdentifierName("CreateIn")
                            ),
                            SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(node.Expression)))
                        );

                        node = node.WithExpression(createIn.NormalizeWhitespace()).WithRefKindKeyword(SyntaxFactory.Token(SyntaxKind.None));
                    }
                }
            }
            else if (isRefInvocation)
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

            var originalNode = node;
            node = (InvocationExpressionSyntax)base.VisitInvocationExpression(node);

            // Process CallerArgumentExpression
            if (method != null)
            {
                var newArgs = ProcessCallerArgumentExpression(node.ArgumentList, originalNode, method, semanticModel);
                if (newArgs != node.ArgumentList)
                {
                    node = node.WithArgumentList(newArgs);
                }
            }

            // Handle params collection expansion (C# 13)
            if (method != null && method.Parameters.Length > 0)
            {
                var lastParam = method.Parameters[method.Parameters.Length - 1];
                if (lastParam.IsParams && !(lastParam.Type is IArrayTypeSymbol) && IsExpandedForm(semanticModel, originalNode, method))
                {
                    var elementType = GetCollectionElementType(lastParam.Type);
                    if (elementType != null)
                    {
                        var paramsIndex = method.Parameters.Length - 1;
                        var explicitArgsCount = node.ArgumentList.Arguments.Count;

                        if (paramsIndex <= explicitArgsCount)
                        {
                            var newArgs = new List<ArgumentSyntax>();
                            for (int i = 0; i < paramsIndex; i++)
                            {
                                newArgs.Add(node.ArgumentList.Arguments[i]);
                            }

                            var collectedArgs = new List<ExpressionSyntax>();
                            for (int i = paramsIndex; i < explicitArgsCount; i++)
                            {
                                collectedArgs.Add(node.ArgumentList.Arguments[i].Expression);
                            }

                            var arrayTypeSyntax = SyntaxHelper.GenerateTypeSyntax(elementType, semanticModel, originalNode.SpanStart, this);
                            var arrayType = SyntaxFactory.ArrayType(arrayTypeSyntax)
                                .WithRankSpecifiers(SyntaxFactory.SingletonList(SyntaxFactory.ArrayRankSpecifier(SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(SyntaxFactory.OmittedArraySizeExpression()))));

                            var arrayCreation = SyntaxFactory.ArrayCreationExpression(arrayType)
                               .WithInitializer(SyntaxFactory.InitializerExpression(SyntaxKind.ArrayInitializerExpression, SyntaxFactory.SeparatedList(collectedArgs)))
                               .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space));

                            ExpressionSyntax paramsArgument = arrayCreation;

                            // If the params parameter type is NOT an array, wrap the array in the collection constructor
                            if (lastParam.Type.TypeKind != TypeKind.Interface && lastParam.Type.TypeKind != TypeKind.Array)
                            {
                                var collectionTypeSyntax = SyntaxHelper.GenerateTypeSyntax(lastParam.Type, semanticModel, originalNode.SpanStart, this);
                                paramsArgument = SyntaxFactory.ObjectCreationExpression(collectionTypeSyntax)
                                    .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(arrayCreation))))
                                    .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space));
                            }

                            newArgs.Add(SyntaxFactory.Argument(paramsArgument));
                            node = node.WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(newArgs)));
                        }
                    }
                }
            }

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

                            if (method.IsGenericMethod && method.TypeArguments.Length == 1 && method.Parameters.Length > 0 && SymbolEqualityComparer.Default.Equals(method.Parameters[0].Type, method.TypeArguments[0]))
                            {
                                var targetExpression = (node.Expression as MemberAccessExpressionSyntax)?.Expression;
                                if (targetExpression != null)
                                {
                                    var targetType = semanticModel.GetTypeInfo(targetExpression).Type;
                                    if (targetType != null && targetType.Kind != SymbolKind.ErrorType && !SymbolEqualityComparer.Default.Equals(targetType, method.TypeArguments[0]))
                                    {
                                        genericName = SyntaxHelper.GenerateGenericName(name.Identifier, new[] { targetType }, semanticModel, pos, this);
                                        genericName = genericName.WithLeadingTrivia(name.GetLeadingTrivia().ExcludeDirectivies()).WithTrailingTrivia(name.GetTrailingTrivia().ExcludeDirectivies());
                                    }
                                }
                            }

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
            if (node.StaticKeyword.RawKind == (int)SyntaxKind.StaticKeyword)
            {
                hasStaticUsingOrAliases = true;
                usingStaticNames.Add(node.Name.ToString());
            }
            if (node.Alias != null)
            {
                hasStaticUsingOrAliases = true;
            }

            if (node.StaticKeyword.IsKind(SyntaxKind.StaticKeyword) || node.Alias != null)
            {
                return null;
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

        public override SyntaxNode VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitEqualsValueClause(node);
            }

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

            // Check if nint/nuint are used as identifiers (variables/members)
            if (node.Identifier.ValueText == "nint" || node.Identifier.ValueText == "nuint")
            {
                var sym = semanticModel.GetSymbolInfo(node).Symbol;
                if (sym == null || !(sym.Kind == SymbolKind.Local || sym.Kind == SymbolKind.Field || sym.Kind == SymbolKind.Parameter || sym.Kind == SymbolKind.Property || sym.Kind == SymbolKind.Method))
                {
                    if (node.Identifier.ValueText == "nint")
                    {
                        return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword))
                            .WithLeadingTrivia(node.GetLeadingTrivia())
                            .WithTrailingTrivia(node.GetTrailingTrivia());
                    }
                    else if (node.Identifier.ValueText == "nuint")
                    {
                        return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.UIntKeyword))
                            .WithLeadingTrivia(node.GetLeadingTrivia())
                            .WithTrailingTrivia(node.GetTrailingTrivia());
                    }
                }
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

            if (IsCapturedPrimaryConstructorParameter(symbol, out var fieldName) && ShouldUseFieldForCapturedParameter(node))
            {
                return SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.ThisExpression(),
                    SyntaxFactory.IdentifierName(fieldName))
                .NormalizeWhitespace()
                .WithLeadingTrivia(node.GetLeadingTrivia())
                .WithTrailingTrivia(node.GetTrailingTrivia());
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

                if (target.CanBeReferencedByName)
                {
                    if (target is INamespaceSymbol ns)
                    {
                        return SyntaxFactory.ParseName(ns.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)).WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
                    }
                    else if (target is ITypeSymbol ts)
                    {
                        return SyntaxHelper.GenerateTypeSyntax(ts, semanticModel, node.SpanStart, this).WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
                    }
                }
                else
                {
                    //Happens for global:: aliases, don't replace otherwise we'll end up with a "::" node
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
            if (node.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
            {
                var isDefining = true;

                if (node.ExpressionBody != null)
                {
                    isDefining = false;
                }
                else if (node.AccessorList != null)
                {
                    foreach (var accessor in node.AccessorList.Accessors)
                    {
                        if (accessor.Body != null || accessor.ExpressionBody != null)
                        {
                            isDefining = false;
                            break;
                        }
                    }
                }

                if (isDefining)
                {
                    return null;
                }

                var idx = node.Modifiers.IndexOf(SyntaxKind.PartialKeyword);
                if (idx > -1)
                {
                    node = node.WithModifiers(node.Modifiers.RemoveAt(idx));
                }
            }

            node = (PropertyDeclarationSyntax)base.VisitPropertyDeclaration(node);
            var newNode = node;

            if (newNode.Modifiers.IndexOf(SyntaxKind.RequiredKeyword) > -1)
            {
                newNode = newNode.WithModifiers(newNode.Modifiers.RemoveAt(newNode.Modifiers.IndexOf(SyntaxKind.RequiredKeyword)));
            }

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

            if (node.Modifiers.IndexOf(SyntaxKind.RequiredKeyword) > -1)
            {
                node = node.WithModifiers(node.Modifiers.RemoveAt(node.Modifiers.IndexOf(SyntaxKind.RequiredKeyword)));
            }

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
            ProcessPrimaryConstructor(node);

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

            if (node.ParameterList != null && c != null)
            {
                c = (StructDeclarationSyntax)SynthesizePrimaryConstructor(node, c);
            }
            _primaryConstructorCaptures.Pop();

            if (c != null && isReadOnly)
            {
                c = c.WithModifiers(c.Modifiers.RemoveAt(c.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword)));
                c = c.WithAttributeLists(c.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(new AttributeSyntax[1] { SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.Immutable")) })).WithTrailingTrivia(SyntaxFactory.Whitespace("\n"))));
            }

            if (c != null && isRef)
            {
                c = c.WithModifiers(c.Modifiers.RemoveAt(c.Modifiers.IndexOf(SyntaxKind.RefKeyword)));
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
            ProcessPrimaryConstructor(node);

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

            if (node.ParameterList != null && c != null)
            {
                c = (ClassDeclarationSyntax)SynthesizePrimaryConstructor(node, c);
            }
            _primaryConstructorCaptures.Pop();

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

            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitMethodDeclaration(node);
            }

            var methodSymbol = semanticModel.GetDeclaredSymbol(node);

            // Rewrite ValueTask -> Task in return type
            if (methodSymbol != null && methodSymbol.ReturnType.Name == "ValueTask" && methodSymbol.ReturnType.ContainingNamespace?.Name == "Tasks" && methodSymbol.ReturnType.ContainingNamespace?.ContainingNamespace?.Name == "Threading")
            {
                var returnType = node.ReturnType;
                if (methodSymbol.ReturnType is INamedTypeSymbol named && named.IsGenericType)
                {
                    // ValueTask<T> -> Task<T>
                    var typeArg = named.TypeArguments[0];
                    var typeArgSyntax = SyntaxHelper.GenerateTypeSyntax(typeArg, semanticModel, node.SpanStart, this);
                    var newTaskType = SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.QualifiedName(
                                SyntaxFactory.IdentifierName("System"),
                                SyntaxFactory.IdentifierName("Threading")
                            ),
                            SyntaxFactory.IdentifierName("Tasks")
                        ),
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("Task"),
                            SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(typeArgSyntax))
                        )
                    );
                    node = node.WithReturnType(newTaskType.WithLeadingTrivia(returnType.GetLeadingTrivia()).WithTrailingTrivia(returnType.GetTrailingTrivia()));
                }
                else
                {
                    // ValueTask -> Task
                    var newTaskType = SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.QualifiedName(
                                SyntaxFactory.IdentifierName("System"),
                                SyntaxFactory.IdentifierName("Threading")
                            ),
                            SyntaxFactory.IdentifierName("Tasks")
                        ),
                        SyntaxFactory.IdentifierName("Task")
                    );
                    node = node.WithReturnType(newTaskType.WithLeadingTrivia(returnType.GetLeadingTrivia()).WithTrailingTrivia(returnType.GetTrailingTrivia()));
                }
            }

            bool isModuleInitializer = false;

            if (methodSymbol != null)
            {
                var attrs = methodSymbol.GetAttributes();
                if (attrs.Any(a => a.AttributeClass != null && a.AttributeClass.Name == "ModuleInitializerAttribute" && a.AttributeClass.ContainingNamespace?.ToDisplayString() == "System.Runtime.CompilerServices"))
                {
                    isModuleInitializer = true;
                }
            }

            if (isModuleInitializer)
            {
                var newLists = new List<AttributeListSyntax>();
                foreach (var list in node.AttributeLists)
                {
                    var newAttrs = new List<AttributeSyntax>();
                    foreach (var attr in list.Attributes)
                    {
                        if (attr.Name.ToString().Contains("ModuleInitializer"))
                        {
                            continue;
                        }
                        newAttrs.Add(attr);
                    }
                    if (newAttrs.Count > 0)
                    {
                        newLists.Add(list.WithAttributes(SyntaxFactory.SeparatedList(newAttrs)));
                    }
                }

                var initAttr = SyntaxFactory.Attribute(SyntaxFactory.ParseName("global::H5.Init"))
                    .WithArgumentList(SyntaxFactory.AttributeArgumentList(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.AttributeArgument(
                            SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ParseTypeName("global::H5.InitPosition"),
                                SyntaxFactory.IdentifierName("After")
                            )
                        )
                    )));

                newLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(initAttr)));

                node = node.WithAttributeLists(SyntaxFactory.List(newLists));
            }

            node = base.VisitMethodDeclaration(node) as MethodDeclarationSyntax;

            if (node.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword) > -1)
            {
                node = node.WithModifiers(node.Modifiers.RemoveAt(node.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword)));
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
            // Rewrite record to class
            // 1. Convert to ClassDeclaration
            // 2. Add constructor for positional parameters
            // 3. Add Deconstruct method if positional
            // 4. Add Properties for positional parameters
            // 5. Synthesize PrintMembers, ToString, Equals, GetHashCode, op_Equality, op_Inequality if not present (simplified for H5)

            // For H5, we can just treat it as a class with auto-props for now to get basic support.
            // Positional records: record Person(string Name, int Age);
            // -> class Person { public string Name { get; init; } public int Age { get; init; } ... ctor ... }

            var openBrace = node.OpenBraceToken;
            if (openBrace.IsKind(SyntaxKind.None))
            {
                openBrace = SyntaxFactory.Token(SyntaxKind.OpenBraceToken).WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed);
            }

            var closeBrace = node.CloseBraceToken;
            if (closeBrace.IsKind(SyntaxKind.None))
            {
                closeBrace = SyntaxFactory.Token(SyntaxKind.CloseBraceToken).WithLeadingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed);
            }

            TypeDeclarationSyntax classDecl;

            if (node.Kind() == SyntaxKind.RecordStructDeclaration || node.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword))
            {
                // HACK: Always use ClassDeclaration even for structs to avoid H5 emitter issues (invalid JS '?.' generation)
                classDecl = SyntaxFactory.ClassDeclaration(node.Identifier)
                    .WithKeyword(SyntaxFactory.Token(SyntaxKind.ClassKeyword).WithTrailingTrivia(SyntaxFactory.Space));
            }
            else
            {
                classDecl = SyntaxFactory.ClassDeclaration(node.Identifier)
                    .WithKeyword(SyntaxFactory.Token(SyntaxKind.ClassKeyword).WithTrailingTrivia(SyntaxFactory.Space));
            }

            ArgumentListSyntax baseArgs = null;
            BaseListSyntax newBaseList = node.BaseList;

            if (node.BaseList != null)
            {
                var newTypes = new List<BaseTypeSyntax>();
                bool changed = false;
                foreach (var baseType in node.BaseList.Types)
                {
                    if (baseType is PrimaryConstructorBaseTypeSyntax pcbt)
                    {
                        baseArgs = pcbt.ArgumentList;
                        newTypes.Add(SyntaxFactory.SimpleBaseType(pcbt.Type).WithLeadingTrivia(pcbt.GetLeadingTrivia()).WithTrailingTrivia(pcbt.GetTrailingTrivia()));
                        changed = true;
                    }
                    else
                    {
                        newTypes.Add(baseType);
                    }
                }

                if (changed)
                {
                    newBaseList = node.BaseList.WithTypes(SyntaxFactory.SeparatedList(newTypes));
                }
            }

            classDecl = classDecl
                .WithModifiers(node.Modifiers)
                .WithTypeParameterList(node.TypeParameterList)
                .WithBaseList(newBaseList)
                .WithConstraintClauses(node.ConstraintClauses)
                .WithAttributeLists(node.AttributeLists)
                .WithOpenBraceToken(openBrace)
                .WithCloseBraceToken(closeBrace)
                .WithMembers(node.Members);

            if (node.ParameterList != null)
            {
                // Create properties and constructor
                var properties = new List<MemberDeclarationSyntax>();
                var ctorParams = new List<ParameterSyntax>();
                var ctorBody = new List<StatementSyntax>();

                foreach (var param in node.ParameterList.Parameters)
                {
                    var propName = param.Identifier.ValueText;
                    // UpperCamelCase for property? No, records keep case usually?
                    // Actually standard C# records use the parameter name exactly for the property, usually PascalCase is convention but if param is camelCase, property is PascalCase?
                    // No, record Person(string name) -> property Name?
                    // Verify C# behavior: record R(int x) -> public int X { get; init; } ?
                    // Actually yes, it uppercases the first letter if it's not.
                    // Wait, standard convention says positional params should be PascalCase.
                    // If I write record R(int x), the property is named 'x'. It does NOT auto-capitalize.

                    var property = SyntaxFactory.PropertyDeclaration(param.Type, param.Identifier)
                        .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                        .WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List(new[]
                        {
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithKeyword(SyntaxFactory.Token(SyntaxKind.SetKeyword)).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                        })));

                    properties.Add(property);

                    ctorParams.Add(param);
                    ctorBody.Add(SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(param.Identifier), // This might be ambiguous with param name, usually this.Name = Name
                            SyntaxFactory.IdentifierName(param.Identifier)
                        )
                    ));
                    // To avoid ambiguity: this.Prop = param
                    // But we used same name.
                    // So: this.x = x.
                }

                // Add properties to class members (at start)
                classDecl = classDecl.WithMembers(classDecl.Members.InsertRange(0, properties));

                // Add constructor
                var ctor = SyntaxFactory.ConstructorDeclaration(node.Identifier)
                    .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                    .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(ctorParams)));

                if (baseArgs != null)
                {
                    ctor = ctor.WithInitializer(SyntaxFactory.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer, baseArgs));
                }

                ctor = ctor.WithBody(SyntaxFactory.Block(
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

                classDecl = classDecl.AddMembers(ctor);

                // Add parameterless constructor to avoid H5 emitter issues (duplicate ctor key) if implicit one is generated
                var defaultCtor = SyntaxFactory.ConstructorDeclaration(node.Identifier)
                    .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                    .WithBody(SyntaxFactory.Block());

                classDecl = classDecl.AddMembers(defaultCtor);

                // Add Deconstruct
                 var deconstructParams = ctorParams.Select(p =>
                    SyntaxFactory.Parameter(p.Identifier).WithType(p.Type).AddModifiers(SyntaxFactory.Token(SyntaxKind.OutKeyword).WithTrailingTrivia(SyntaxFactory.Space))
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

                classDecl = classDecl.AddMembers(deconstruct);

                // Add PrintMembers and ToString
                // Always use protected virtual for PrintMembers since we map everything to class in H5 to avoid emitter issues
                var printMembersModifiers = SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Space));
                printMembersModifiers = printMembersModifiers.Add(SyntaxFactory.Token(SyntaxKind.VirtualKeyword).WithTrailingTrivia(SyntaxFactory.Space));

                var sbType = SyntaxFactory.ParseTypeName("System.Text.StringBuilder");

                bool synthesizePrintMembers = !node.Members.OfType<MethodDeclarationSyntax>().Any(m => m.Identifier.ValueText == "PrintMembers" && m.ParameterList.Parameters.Count == 1);

                if (synthesizePrintMembers)
                {
                    var printMembersBody = new List<StatementSyntax>();

                    // builder.Append("Prop = "); builder.Append(this.Prop); ...
                    bool first = true;
                    foreach (var param in node.ParameterList.Parameters)
                    {
                        if (!first)
                        {
                            printMembersBody.Add(SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("builder"), SyntaxFactory.IdentifierName("Append")),
                                SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(", "))))))));
                        }
                        first = false;

                        printMembersBody.Add(SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("builder"), SyntaxFactory.IdentifierName("Append")),
                            SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(param.Identifier.ValueText + " = "))))))));

                        printMembersBody.Add(SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("builder"), SyntaxFactory.IdentifierName("Append")),
                            SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ThisExpression(), SyntaxFactory.IdentifierName(param.Identifier))))))));
                    }
                    printMembersBody.Add(SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression).WithLeadingTrivia(SyntaxFactory.Space)));

                    var printMembers = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword).WithTrailingTrivia(SyntaxFactory.Space)), "PrintMembers")
                        .WithModifiers(printMembersModifiers)
                        .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Parameter(SyntaxFactory.Identifier("builder").WithLeadingTrivia(SyntaxFactory.Space)).WithType(sbType))))
                        .WithBody(SyntaxFactory.Block(printMembersBody));

                    classDecl = classDecl.AddMembers(printMembers);
                }

                bool synthesizeToString = !node.Members.OfType<MethodDeclarationSyntax>().Any(m => m.Identifier.ValueText == "ToString" && m.ParameterList.Parameters.Count == 0);

                if (synthesizeToString)
                {
                    var symbol = semanticModel.GetDeclaredSymbol(node);
                    if (symbol != null)
                    {
                        var currentBase = symbol.BaseType;
                        while (currentBase != null && currentBase.SpecialType != SpecialType.System_Object)
                        {
                            var toStringSym = currentBase.GetMembers("ToString").FirstOrDefault(m => m is IMethodSymbol ms && ms.Parameters.Length == 0) as IMethodSymbol;
                            if (toStringSym != null)
                            {
                                if (toStringSym.IsSealed)
                                {
                                    synthesizeToString = false;
                                }
                                break;
                            }
                            currentBase = currentBase.BaseType;
                        }
                    }
                }

                if (synthesizeToString)
                {
                    var toStringBody = new List<StatementSyntax>();
                    // var sb = new StringBuilder();
                    toStringBody.Add(SyntaxFactory.LocalDeclarationStatement(SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("var").WithTrailingTrivia(SyntaxFactory.Space))
                        .WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator("sb").WithInitializer(SyntaxFactory.EqualsValueClause(
                            SyntaxFactory.ObjectCreationExpression(sbType)
                            .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                            .WithArgumentList(SyntaxFactory.ArgumentList())))))));

                    // sb.Append("TypeName");
                    toStringBody.Add(SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("sb"), SyntaxFactory.IdentifierName("Append")),
                        SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(node.Identifier.ValueText))))))));

                    // sb.Append(" { ");
                    toStringBody.Add(SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("sb"), SyntaxFactory.IdentifierName("Append")),
                        SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(" { "))))))));

                    // if (PrintMembers(sb)) sb.Append(" ");
                    toStringBody.Add(SyntaxFactory.IfStatement(
                        SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName("PrintMembers"), SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.IdentifierName("sb"))))),
                        SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("sb"), SyntaxFactory.IdentifierName("Append")),
                            SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(" ")))))))));

                    // sb.Append("}");
                    toStringBody.Add(SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("sb"), SyntaxFactory.IdentifierName("Append")),
                        SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("}"))))))));

                    // return sb.ToString();
                    toStringBody.Add(SyntaxFactory.ReturnStatement(SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("sb"), SyntaxFactory.IdentifierName("ToString"))).WithLeadingTrivia(SyntaxFactory.Space)));

                    var toString = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword).WithTrailingTrivia(SyntaxFactory.Space)), "ToString")
                        .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)).Add(SyntaxFactory.Token(SyntaxKind.OverrideKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                        .WithBody(SyntaxFactory.Block(toStringBody));

                    classDecl = classDecl.AddMembers(toString);
                }
            }

            // Add Clone method for H5 support (required for 'with' expressions)
            var cloneMethod = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword).WithTrailingTrivia(SyntaxFactory.Space)), "Clone")
                .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                .WithBody(SyntaxFactory.Block(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                             SyntaxFactory.MemberAccessExpression(
                                 SyntaxKind.SimpleMemberAccessExpression,
                                 SyntaxFactory.ParseTypeName("global::H5.Script"),
                                 SyntaxFactory.GenericName("Write")
                                     .WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                         SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword))
                                     )))
                             ),
                             SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(
                                 SyntaxFactory.Argument(
                                     SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("H5.copyProperties(new (this.constructor)(), this)"))
                                 )
                             ))
                        ).WithLeadingTrivia(SyntaxFactory.Space)
                    )
                ));

            classDecl = classDecl.AddMembers(cloneMethod);

            // Add System.ICloneable interface
            var cloneableType = SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("global::System.ICloneable"));
            if (classDecl.BaseList == null)
            {
                classDecl = classDecl.WithBaseList(SyntaxFactory.BaseList(SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(cloneableType)));
            }
            else
            {
                classDecl = classDecl.WithBaseList(classDecl.BaseList.AddTypes(cloneableType));
            }

            // Synthesize Equality Members
            bool synthesizeEquals = !node.Members.OfType<MethodDeclarationSyntax>().Any(m => m.Identifier.ValueText == "Equals" && m.ParameterList.Parameters.Count == 1);
            if (synthesizeEquals)
            {
                TypeSyntax currentTypeSyntax;
                if (node.TypeParameterList != null && node.TypeParameterList.Parameters.Count > 0)
                {
                    currentTypeSyntax = SyntaxFactory.GenericName(
                        node.Identifier,
                        SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList<TypeSyntax>(
                            node.TypeParameterList.Parameters.Select(p => SyntaxFactory.IdentifierName(p.Identifier))
                        )));
                }
                else
                {
                    currentTypeSyntax = SyntaxFactory.IdentifierName(node.Identifier);
                }

                var typeWithSpace = currentTypeSyntax.WithTrailingTrivia(SyntaxFactory.Space);

                // Add System.IEquatable<T>
                var equatableType = SyntaxFactory.SimpleBaseType(
                    SyntaxFactory.QualifiedName(
                        SyntaxFactory.ParseName("global::System"),
                        SyntaxFactory.GenericName("IEquatable").WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(currentTypeSyntax)))
                    ));

                if (classDecl.BaseList == null)
                {
                    classDecl = classDecl.WithBaseList(SyntaxFactory.BaseList(SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(equatableType)));
                }
                else
                {
                    classDecl = classDecl.WithBaseList(classDecl.BaseList.AddTypes(equatableType));
                }

                // Equals(object obj)
                var equalsObjBody = SyntaxFactory.Block(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(typeWithSpace)
                        .WithVariables(SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.VariableDeclarator("other").WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.BinaryExpression(SyntaxKind.AsExpression, SyntaxFactory.IdentifierName("obj"), currentTypeSyntax)
                                    .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.AsKeyword).WithLeadingTrivia(SyntaxFactory.Space).WithTrailingTrivia(SyntaxFactory.Space))
                                )
                            )
                        ))
                    ),
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, SyntaxFactory.IdentifierName("other"), SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.IdentifierName("Equals"),
                                SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.IdentifierName("other"))))
                            )
                        )
                    )
                    .WithReturnKeyword(SyntaxFactory.Token(SyntaxKind.ReturnKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                );

                var equalsObj = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword).WithTrailingTrivia(SyntaxFactory.Space)), "Equals")
                    .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)).Add(SyntaxFactory.Token(SyntaxKind.OverrideKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                    .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Parameter(SyntaxFactory.Identifier("obj")).WithType(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword).WithTrailingTrivia(SyntaxFactory.Space))))))
                    .WithBody(equalsObjBody);

                classDecl = classDecl.AddMembers(equalsObj);

                // Equals(T other)
                var equalsTBodyStatements = new List<StatementSyntax>();
                // if (ReferenceEquals(null, other)) return false;
                equalsTBodyStatements.Add(SyntaxFactory.IfStatement(
                    SyntaxFactory.InvocationExpression(
                         SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ParseName("global::System.Object"), SyntaxFactory.IdentifierName("ReferenceEquals")),
                         SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(new SyntaxNodeOrToken[] {
                             SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)),
                             SyntaxFactory.Token(SyntaxKind.CommaToken),
                             SyntaxFactory.Argument(SyntaxFactory.IdentifierName("other"))
                         }))
                    ),
                    SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression)).WithReturnKeyword(SyntaxFactory.Token(SyntaxKind.ReturnKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                )
                .WithIfKeyword(SyntaxFactory.Token(SyntaxKind.IfKeyword).WithTrailingTrivia(SyntaxFactory.Space)));

                 // if (ReferenceEquals(this, other)) return true;
                equalsTBodyStatements.Add(SyntaxFactory.IfStatement(
                    SyntaxFactory.InvocationExpression(
                         SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ParseName("global::System.Object"), SyntaxFactory.IdentifierName("ReferenceEquals")),
                         SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(new SyntaxNodeOrToken[] {
                             SyntaxFactory.Argument(SyntaxFactory.ThisExpression()),
                             SyntaxFactory.Token(SyntaxKind.CommaToken),
                             SyntaxFactory.Argument(SyntaxFactory.IdentifierName("other"))
                         }))
                    ),
                    SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression)).WithReturnKeyword(SyntaxFactory.Token(SyntaxKind.ReturnKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                )
                .WithIfKeyword(SyntaxFactory.Token(SyntaxKind.IfKeyword).WithTrailingTrivia(SyntaxFactory.Space)));

                ExpressionSyntax expr = SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression);

                foreach (var param in node.ParameterList?.Parameters ?? SyntaxFactory.SeparatedList<ParameterSyntax>())
                {
                    var propName = param.Identifier.ValueText;
                    var propType = param.Type;

                    // EqualityComparer<PropType>.Default.Equals(this.Prop, other.Prop)
                    var comparer = SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.ParseName("global::System.Collections.Generic"),
                            SyntaxFactory.GenericName("EqualityComparer").WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(propType)))
                        ),
                        SyntaxFactory.IdentifierName("Default")
                    );

                    var equalsCall = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, comparer, SyntaxFactory.IdentifierName("Equals")),
                        SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(new SyntaxNodeOrToken[] {
                            SyntaxFactory.Argument(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ThisExpression(), SyntaxFactory.IdentifierName(propName))),
                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                            SyntaxFactory.Argument(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("other"), SyntaxFactory.IdentifierName(propName)))
                        }))
                    );

                    expr = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, expr, equalsCall);
                }

                equalsTBodyStatements.Add(SyntaxFactory.ReturnStatement(expr).WithReturnKeyword(SyntaxFactory.Token(SyntaxKind.ReturnKeyword).WithTrailingTrivia(SyntaxFactory.Space)));

                var equalsT = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword).WithTrailingTrivia(SyntaxFactory.Space)), "Equals")
                    .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                    .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Parameter(SyntaxFactory.Identifier("other")).WithType(typeWithSpace))))
                    .WithBody(SyntaxFactory.Block(equalsTBodyStatements));

                classDecl = classDecl.AddMembers(equalsT);

                // GetHashCode
                var hashCodeBodyStatements = new List<StatementSyntax>();
                // var hashCode = -987654321;

                hashCodeBodyStatements.Add(SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                    .WithVariables(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator("hashCode").WithInitializer(SyntaxFactory.EqualsValueClause(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(-987654321))))
                    ))
                ));

                foreach (var param in node.ParameterList?.Parameters ?? SyntaxFactory.SeparatedList<ParameterSyntax>())
                {
                    var propName = param.Identifier.ValueText;
                    var propType = param.Type;

                    var comparer = SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.ParseName("global::System.Collections.Generic"),
                            SyntaxFactory.GenericName("EqualityComparer").WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(propType)))
                        ),
                        SyntaxFactory.IdentifierName("Default")
                    );

                    var getHashCodeCall = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, comparer, SyntaxFactory.IdentifierName("GetHashCode")),
                        SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.Argument(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ThisExpression(), SyntaxFactory.IdentifierName(propName)))
                        ))
                    );

                    // hashCode = (hashCode * -1521134295) + getHashCodeCall;
                    var calc = SyntaxFactory.BinaryExpression(
                        SyntaxKind.AddExpression,
                        SyntaxFactory.ParenthesizedExpression(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.MultiplyExpression,
                                SyntaxFactory.IdentifierName("hashCode"),
                                SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(-1521134295))
                            )
                        ),
                        getHashCodeCall
                    );

                    hashCodeBodyStatements.Add(SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName("hashCode"), calc)
                    ));
                }

                hashCodeBodyStatements.Add(SyntaxFactory.ReturnStatement(SyntaxFactory.IdentifierName("hashCode")).WithReturnKeyword(SyntaxFactory.Token(SyntaxKind.ReturnKeyword).WithTrailingTrivia(SyntaxFactory.Space)));

                var getHashCode = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword).WithTrailingTrivia(SyntaxFactory.Space)), "GetHashCode")
                    .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)).Add(SyntaxFactory.Token(SyntaxKind.OverrideKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                    .WithBody(SyntaxFactory.Block(hashCodeBodyStatements));

                classDecl = classDecl.AddMembers(getHashCode);

                // Operators
                var opEq = SyntaxFactory.OperatorDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword).WithTrailingTrivia(SyntaxFactory.Space)), SyntaxFactory.Token(SyntaxKind.EqualsEqualsToken).WithTrailingTrivia(SyntaxFactory.Space))
                    .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)).Add(SyntaxFactory.Token(SyntaxKind.StaticKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                    .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(new SyntaxNodeOrToken[] {
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier("left")).WithType(typeWithSpace),
                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier("right")).WithType(typeWithSpace)
                    })))
                    .WithBody(SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.QualifiedName(
                                            SyntaxFactory.ParseName("global::System.Collections.Generic"),
                                            SyntaxFactory.GenericName("EqualityComparer").WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(currentTypeSyntax)))
                                        ),
                                        SyntaxFactory.IdentifierName("Default")),
                                    SyntaxFactory.IdentifierName("Equals")),
                                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(new SyntaxNodeOrToken[] {
                                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName("left")),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName("right"))
                                }))
                            )
                        )
                        .WithReturnKeyword(SyntaxFactory.Token(SyntaxKind.ReturnKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                    ));

                 var opNeq = SyntaxFactory.OperatorDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword).WithTrailingTrivia(SyntaxFactory.Space)), SyntaxFactory.Token(SyntaxKind.ExclamationEqualsToken).WithTrailingTrivia(SyntaxFactory.Space))
                    .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)).Add(SyntaxFactory.Token(SyntaxKind.StaticKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                    .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(new SyntaxNodeOrToken[] {
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier("left")).WithType(typeWithSpace),
                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier("right")).WithType(typeWithSpace)
                    })))
                    .WithBody(SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression,
                                SyntaxFactory.ParenthesizedExpression(
                                    SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, SyntaxFactory.IdentifierName("left"), SyntaxFactory.IdentifierName("right"))
                                )
                            )
                        )
                        .WithReturnKeyword(SyntaxFactory.Token(SyntaxKind.ReturnKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                    ));

                classDecl = classDecl.AddMembers(opEq, opNeq);
            }

            // return VisitClassDeclaration(classDecl);
            // We cannot call VisitClassDeclaration because classDecl is a synthesized node
            // and semanticModel.GetDeclaredSymbol(classDecl) would fail.
            // Instead, we inline the logic here, using 'node' (the record) for symbol resolution.

            currentType.Push(semanticModel.GetDeclaredSymbol(node));
            var oldIndex = IndexInstance;
            IndexInstance = 0;
            var old = fields;
            fields = new List<MemberDeclarationSyntax>();

            TypeDeclarationSyntax c;
            if (classDecl is StructDeclarationSyntax structDecl)
            {
                c = base.VisitStructDeclaration(structDecl) as StructDeclarationSyntax;
            }
            else
            {
                c = base.VisitClassDeclaration((ClassDeclarationSyntax)classDecl) as ClassDeclarationSyntax;
            }

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

            if (c.Modifiers.IndexOf(SyntaxKind.PrivateKeyword) > -1 && c.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword) > -1)
            {
                c = c.WithModifiers(c.Modifiers.Replace(c.Modifiers[c.Modifiers.IndexOf(SyntaxKind.ProtectedKeyword)], SyntaxFactory.Token(SyntaxKind.InternalKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                c = c.WithModifiers(c.Modifiers.Replace(c.Modifiers[c.Modifiers.IndexOf(SyntaxKind.PrivateKeyword)], SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Whitespace(" "))));
                c = c.WithAttributeLists(c.AttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("H5.PrivateProtectedAttribute"))))));
            }

            fields = old;
            IndexInstance = oldIndex;
            currentType.Pop();

            return c;
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

                         ConvertInitializers(initializers, instance, statements, initializerInfos, type);

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
            var originalNode = node;
            node = (OperatorDeclarationSyntax)base.VisitOperatorDeclaration(node);

            if (originalNode.CheckedKeyword.IsKind(SyntaxKind.CheckedKeyword))
            {
                var methodName = GetCheckedOperatorName(node.OperatorToken, node.ParameterList.Parameters.Count);
                var method = SyntaxFactory.MethodDeclaration(
                    node.ReturnType,
                    methodName)
                    .WithModifiers(node.Modifiers)
                    .WithParameterList(node.ParameterList)
                    .WithBody(node.Body)
                    .WithExpressionBody(node.ExpressionBody)
                    .WithAttributeLists(node.AttributeLists)
                    .WithTypeParameterList(null)
                    .WithConstraintClauses(default);

                if (method.ExpressionBody != null)
                {
                    return SyntaxHelper.ToStatementBody(method);
                }
                return method;
            }

            if (node.ExpressionBody != null)
            {
                return SyntaxHelper.ToStatementBody(node);
            }

            return node;
        }

        public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            var originalNode = node;
            node = (ConversionOperatorDeclarationSyntax)base.VisitConversionOperatorDeclaration(node);

            if (originalNode.CheckedKeyword.IsKind(SyntaxKind.CheckedKeyword))
            {
                var methodName = "op_CheckedExplicit";
                var method = SyntaxFactory.MethodDeclaration(
                    node.Type,
                    methodName)
                    .WithModifiers(node.Modifiers)
                    .WithParameterList(node.ParameterList)
                    .WithBody(node.Body)
                    .WithExpressionBody(node.ExpressionBody)
                    .WithAttributeLists(node.AttributeLists)
                    .WithTypeParameterList(null)
                    .WithConstraintClauses(default);

                if (method.ExpressionBody != null)
                {
                    return SyntaxHelper.ToStatementBody(method);
                }
                return method;
            }

            if (node.ExpressionBody != null)
            {
                return SyntaxHelper.ToStatementBody(node);
            }

            return node;
        }

        private string GetCheckedOperatorName(SyntaxToken operatorToken, int parameterCount)
        {
            switch (operatorToken.Kind())
            {
                case SyntaxKind.PlusToken: return "op_CheckedAddition";
                case SyntaxKind.MinusToken: return parameterCount == 2 ? "op_CheckedSubtraction" : "op_CheckedUnaryNegation";
                case SyntaxKind.AsteriskToken: return "op_CheckedMultiply";
                case SyntaxKind.SlashToken: return "op_CheckedDivision";
                // case SyntaxKind.PlusPlusToken: return "op_CheckedIncrement";
                // case SyntaxKind.MinusMinusToken: return "op_CheckedDecrement";
                default: throw new NotSupportedException("Checked operator not supported: " + operatorToken.Text);
            }
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
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitParenthesizedLambdaExpression(node);
            }

            var hasOptionalParameters = node.ParameterList.Parameters.Any(p => p.Default != null);

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

            var newNode = base.VisitParenthesizedLambdaExpression(node);

            if (newNode is ParenthesizedLambdaExpressionSyntax ple)
            {
                // Handle discards
                if (ple.ParameterList.Parameters.Any(p => p.Identifier.Text == "_"))
                {
                    var newParams = new List<ParameterSyntax>();
                    var discardCount = 0;
                    foreach (var p in ple.ParameterList.Parameters)
                    {
                        if (p.Identifier.Text == "_")
                        {
                            if (discardCount > 0)
                            {
                                newParams.Add(p.WithIdentifier(SyntaxFactory.Identifier($"_{discardCount}")));
                            }
                            else
                            {
                                newParams.Add(p);
                            }
                            discardCount++;
                        }
                        else
                        {
                            newParams.Add(p);
                        }
                    }
                    ple = ple.WithParameterList(ple.ParameterList.WithParameters(SyntaxFactory.SeparatedList(newParams)));
                    newNode = ple;
                }
            }

            IsExpressionOfT = oldValue;

            if (markAsAsync && newNode is ParenthesizedLambdaExpressionSyntax pleAsync)
            {
                pleAsync = pleAsync.WithAsyncKeyword(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
                newNode = pleAsync;
            }

            markAsAsync = oldMarkAsAsync;

            if (hasOptionalParameters && !IsExpressionOfT && newNode is ParenthesizedLambdaExpressionSyntax lambdaWithOptional)
            {
                var symbol = semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;

                if (symbol != null)
                {
                    var delegateName = GetUniqueTempKey("Delegate_L");
                    var typeParameters = new List<TypeParameterSyntax>();
                    var typeArguments = new List<TypeSyntax>();
                    var constraints = new List<TypeParameterConstraintClauseSyntax>();

                    var current = symbol.ContainingSymbol;
                    while (current is IMethodSymbol method)
                    {
                        if (method.IsGenericMethod)
                        {
                            foreach (var tp in method.TypeParameters.Reverse())
                            {
                                typeParameters.Insert(0, SyntaxFactory.TypeParameter(tp.Name));
                                typeArguments.Insert(0, SyntaxFactory.ParseTypeName(tp.Name));

                                var constraint = GetConstraint(tp, node.SpanStart);
                                if (constraint != null)
                                {
                                    constraints.Insert(0, constraint);
                                }
                            }
                        }
                        current = current.ContainingSymbol;
                    }

                    var returnType = SyntaxHelper.GenerateTypeSyntax(symbol.ReturnType, semanticModel, node.SpanStart, this);

                    var delegateDecl = SyntaxFactory.DelegateDeclaration(
                        SyntaxFactory.List<AttributeListSyntax>(),
                        SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PrivateKeyword).WithTrailingTrivia(SyntaxFactory.Space)),
                        SyntaxFactory.Token(SyntaxKind.DelegateKeyword).WithTrailingTrivia(SyntaxFactory.Space),
                        returnType,
                        SyntaxFactory.Identifier(delegateName).WithLeadingTrivia(SyntaxFactory.Space),
                        typeParameters.Count > 0 ? SyntaxFactory.TypeParameterList(SyntaxFactory.SeparatedList(typeParameters)) : null,
                        lambdaWithOptional.ParameterList,
                        SyntaxFactory.List(constraints),
                        SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                    fields.Add(delegateDecl);

                    var newParams = new List<ParameterSyntax>();
                    foreach (var p in lambdaWithOptional.ParameterList.Parameters)
                    {
                        newParams.Add(p.WithDefault(null));
                    }

                    newNode = SyntaxFactory.CastExpression(
                        typeArguments.Count > 0
                            ? SyntaxFactory.GenericName(SyntaxFactory.Identifier(delegateName), SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(typeArguments)))
                            : SyntaxFactory.ParseTypeName(delegateName),
                        SyntaxFactory.ParenthesizedExpression(lambdaWithOptional.WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(newParams))))
                    );
                }
            }

            if (newNode is ParenthesizedLambdaExpressionSyntax lambda && lambda.ReturnType != null)
            {
                var parameters = lambda.ParameterList.Parameters;
                var returnType = lambda.ReturnType;

                bool isVoid = false;
                if (returnType is PredefinedTypeSyntax pts && pts.Keyword.IsKind(SyntaxKind.VoidKeyword))
                {
                    isVoid = true;
                }

                var typeArguments = new List<TypeSyntax>();
                foreach (var p in parameters)
                {
                    if (p.Type == null)
                    {
                        return lambda.WithReturnType(null).WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
                    }
                    typeArguments.Add(p.Type);
                }

                if (!isVoid)
                {
                    typeArguments.Add(returnType);
                }

                var baseType = isVoid ? "Action" : "Func";

                TypeSyntax delegateType;

                if (isVoid && typeArguments.Count == 0)
                {
                    delegateType = SyntaxFactory.QualifiedName(
                       SyntaxFactory.IdentifierName(SYSTEM_IDENTIFIER),
                       SyntaxFactory.IdentifierName("Action"));
                }
                else
                {
                    delegateType = SyntaxFactory.QualifiedName(
                       SyntaxFactory.IdentifierName(SYSTEM_IDENTIFIER),
                       SyntaxFactory.GenericName(baseType)
                           .WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(typeArguments)))
                    );
                }

                var cast = SyntaxFactory.CastExpression(
                    delegateType,
                    SyntaxFactory.ParenthesizedExpression(lambda.WithReturnType(null))
                );

                return cast.WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }

            return newNode;
        }

        public override SyntaxNode VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitSimpleLambdaExpression(node);
            }

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
            public IPropertySymbol indexer;
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
                    if (ae.Left is ImplicitElementAccessSyntax implicitSyntax)
                    {
                        var symInfo = semanticModel.GetSymbolInfo(implicitSyntax);
                        info.indexer = symInfo.Symbol as IPropertySymbol ?? symInfo.CandidateSymbols.FirstOrDefault() as IPropertySymbol;
                    }

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
                        if (be.Left is ImplicitElementAccessSyntax implicitSyntax)
                        {
                            var symInfo = semanticModel.GetSymbolInfo(implicitSyntax);
                            info.indexer = symInfo.Symbol as IPropertySymbol ?? symInfo.CandidateSymbols.FirstOrDefault() as IPropertySymbol;

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

            var originalNode = node;
            node = (ObjectCreationExpressionSyntax)base.VisitObjectCreationExpression(node);

            var constructorSymbol = semanticModel.GetSymbolInfo(originalNode).Symbol as IMethodSymbol;
            if (constructorSymbol != null)
            {
                var newArgs = ProcessCallerArgumentExpression(node.ArgumentList, originalNode, constructorSymbol, semanticModel);
                if (newArgs != node.ArgumentList)
                {
                    node = node.WithArgumentList(newArgs);
                }
            }

            if (needRewrite)
            {
                if (IsExpressionOfT)
                {
                    if (isImplicitElementAccessSyntax)
                    {
                        var mapped = semanticModel.SyntaxTree.GetLineSpan(originalNode.Span);
                        throw new Exception(string.Format(CultureInfo.InvariantCulture, "{2} - {3}({0},{1}): {4}", mapped.StartLinePosition.Line + 1, mapped.StartLinePosition.Character + 1, "Index collection initializer is not supported inside Expression<T>", semanticModel.SyntaxTree.FilePath, originalNode.ToString()));
                    }

                    if (extensionMethodExists)
                    {
                        var mapped = semanticModel.SyntaxTree.GetLineSpan(originalNode.Span);
                        throw new Exception(string.Format(CultureInfo.InvariantCulture, "{2} - {3}({0},{1}): {4}", mapped.StartLinePosition.Line + 1, mapped.StartLinePosition.Character + 1, "Extension method for collection initializer is not supported inside Expression<T>", semanticModel.SyntaxTree.FilePath, originalNode.ToString()));
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

                var parent = originalNode.Parent;

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

                var typeInfo = semanticModel.GetTypeInfo(originalNode);
                var type = typeInfo.Type ?? typeInfo.ConvertedType;

                ConvertInitializers(initializers, instance, statements, initializerInfos, type);

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

        private void ConvertInitializers(SeparatedSyntaxList<ExpressionSyntax> initializers, string instance, List<StatementSyntax> statements, List<InitializerInfo> infos, ITypeSymbol type)
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
                            bool targetExpectsIndex = false;
                            if (info.indexer != null && info.indexer.Parameters.Length > 0)
                            {
                                var pType = info.indexer.Parameters[0].Type;
                                if ((pType.Name == "Index" || pType.Name == "Range") && pType.ContainingNamespace?.Name == "System") targetExpectsIndex = true;
                            }

                            var newArgs = new List<ArgumentSyntax>();
                            foreach (var arg in implicitSyntax.ArgumentList.Arguments)
                            {
                                if (!targetExpectsIndex && arg.Expression is PrefixUnaryExpressionSyntax prefix && prefix.OperatorToken.IsKind(SyntaxKind.CaretToken))
                                {
                                    string lengthProp = "Length";
                                    if (type != null)
                                    {
                                        if (type.GetMembers("Count").Any()) lengthProp = "Count";
                                    }

                                    var lengthAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ParseExpression(instance), SyntaxFactory.IdentifierName(lengthProp));

                                    var val = prefix.Operand;

                                    var newIdx = SyntaxFactory.BinaryExpression(SyntaxKind.SubtractExpression, lengthAccess, val);
                                    newArgs.Add(arg.WithExpression(newIdx));
                                }
                                else
                                {
                                    newArgs.Add((ArgumentSyntax)Visit(arg));
                                }
                            }

                            name = SyntaxFactory.ElementAccessExpression(SyntaxFactory.IdentifierName(instance),
                                    SyntaxFactory.BracketedArgumentList(SyntaxFactory.SeparatedList(newArgs)).WithoutTrivia()).ToString();
                        }
                        else
                        {
                            name = instance;
                        }

                        ITypeSymbol newType = null;
                        var symbol = semanticModel.GetSymbolInfo(be.Left).Symbol;

                        if (symbol is IPropertySymbol ps) newType = ps.Type;
                        else if (symbol is IFieldSymbol fs) newType = fs.Type;

                        ConvertInitializers(syntax.Expressions, name, statements, info.nested, newType);
                    }
                    else
                    {
                        if (be.Left is ImplicitElementAccessSyntax indexerKeys)
                        {
                            bool targetExpectsIndex = false;
                            if (info.indexer != null && info.indexer.Parameters.Length > 0)
                            {
                                var pType = info.indexer.Parameters[0].Type;
                                if ((pType.Name == "Index" || pType.Name == "Range") && pType.ContainingNamespace?.Name == "System") targetExpectsIndex = true;
                            }

                            var newArgs = new List<ArgumentSyntax>();
                            foreach (var arg in indexerKeys.ArgumentList.Arguments)
                            {
                                if (!targetExpectsIndex && arg.Expression is PrefixUnaryExpressionSyntax prefix && prefix.OperatorToken.IsKind(SyntaxKind.CaretToken))
                                {
                                    string lengthProp = "Length";
                                    if (type != null)
                                    {
                                        if (type.GetMembers("Count").Any()) lengthProp = "Count";
                                    }

                                    var lengthAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ParseExpression(instance), SyntaxFactory.IdentifierName(lengthProp));

                                    var val = prefix.Operand;

                                    var newIdx = SyntaxFactory.BinaryExpression(SyntaxKind.SubtractExpression, lengthAccess, val);
                                    newArgs.Add(arg.WithExpression(newIdx));
                                }
                                else
                                {
                                    newArgs.Add((ArgumentSyntax)Visit(arg));
                                }
                            }

                            be = be.WithLeft(SyntaxFactory.ElementAccessExpression(SyntaxFactory.IdentifierName(instance),
                                    SyntaxFactory.BracketedArgumentList(SyntaxFactory.SeparatedList(newArgs)).WithoutTrivia()));
                        }
                        else
                        {
                            var identifier = (IdentifierNameSyntax)be.Left;
                            be =
                                be.WithLeft(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(instance),
                                    SyntaxFactory.IdentifierName(identifier.Identifier.ValueText)));
                        }

                        var right = (ExpressionSyntax)Visit(be.Right);
                        be = be.WithRight(right.WithoutTrivia());
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

        public override SyntaxNode VisitCollectionExpression(CollectionExpressionSyntax node)
        {
            if (node.SyntaxTree == null || node.SyntaxTree != semanticModel.SyntaxTree)
            {
                return base.VisitCollectionExpression(node);
            }

            var typeInfo = semanticModel.GetTypeInfo(node);
            var targetType = typeInfo.ConvertedType;

            if (targetType == null || targetType.TypeKind == TypeKind.Error)
            {
                return base.VisitCollectionExpression(node);
            }

            // Check for spread elements
            if (node.Elements.Any(e => e is SpreadElementSyntax))
            {
                 return RewriteCollectionExpressionWithSpread(node, targetType);
            }

            var visitedElements = new List<SyntaxNodeOrToken>();
            for (int i = 0; i < node.Elements.Count; i++)
            {
                var element = node.Elements[i];
                if (element is ExpressionElementSyntax expr)
                {
                    visitedElements.Add((ExpressionSyntax)Visit(expr.Expression));
                    if (i < node.Elements.Count - 1)
                    {
                        visitedElements.Add(SyntaxFactory.Token(SyntaxKind.CommaToken).WithTrailingTrivia(SyntaxFactory.Space));
                    }
                }
            }

            var initializer = SyntaxFactory.InitializerExpression(
                SyntaxKind.ArrayInitializerExpression,
                SyntaxFactory.SeparatedList<ExpressionSyntax>(visitedElements));

            // Determine concrete type to instantiate
            // 1. Array
            if (targetType is IArrayTypeSymbol arrayType)
            {
                var elementTypeSyntax = SyntaxHelper.GenerateTypeSyntax(arrayType.ElementType, semanticModel, node.SpanStart, this);
                var newArray = SyntaxFactory.ArrayCreationExpression(
                    SyntaxFactory.ArrayType(elementTypeSyntax)
                        .WithRankSpecifiers(SyntaxFactory.SingletonList(SyntaxFactory.ArrayRankSpecifier(SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(SyntaxFactory.OmittedArraySizeExpression())))))
                    .WithInitializer(initializer)
                    .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space));

                return newArray.WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }

            // 2. Named Type (List, Span, IEnumerable, etc.)
            if (targetType is INamedTypeSymbol namedType)
            {
                // Check if it's Span/ReadOnlySpan -> Array
                 if ((namedType.Name == "Span" || namedType.Name == "ReadOnlySpan") &&
                    namedType.ContainingNamespace?.ToDisplayString() == "System" && namedType.TypeArguments.Length == 1)
                {
                    var elementTypeSyntax = SyntaxHelper.GenerateTypeSyntax(namedType.TypeArguments[0], semanticModel, node.SpanStart, this);
                    var newArray = SyntaxFactory.ArrayCreationExpression(
                        SyntaxFactory.ArrayType(elementTypeSyntax)
                            .WithRankSpecifiers(SyntaxFactory.SingletonList(SyntaxFactory.ArrayRankSpecifier(SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(SyntaxFactory.OmittedArraySizeExpression())))))
                        .WithInitializer(initializer)
                        .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space));

                    return newArray.WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
                }

                // Check for List<T> or interfaces implemented by List<T> or T[]
                bool useList = false;
                bool useArray = false;

                var specialType = namedType.OriginalDefinition.SpecialType;
                if (specialType == SpecialType.System_Collections_Generic_IEnumerable_T ||
                    specialType == SpecialType.System_Collections_Generic_IReadOnlyList_T ||
                    specialType == SpecialType.System_Collections_Generic_IReadOnlyCollection_T)
                {
                    useArray = true;
                }
                else if (specialType == SpecialType.System_Collections_Generic_IList_T ||
                         specialType == SpecialType.System_Collections_Generic_ICollection_T ||
                         (namedType.Name == "List" && namedType.ContainingNamespace?.ToDisplayString() == "System.Collections.Generic"))
                {
                    useList = true;
                }

                if (useArray)
                {
                     var elementTypeSyntax = SyntaxHelper.GenerateTypeSyntax(namedType.TypeArguments[0], semanticModel, node.SpanStart, this);
                    var newArray = SyntaxFactory.ArrayCreationExpression(
                        SyntaxFactory.ArrayType(elementTypeSyntax)
                            .WithRankSpecifiers(SyntaxFactory.SingletonList(SyntaxFactory.ArrayRankSpecifier(SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(SyntaxFactory.OmittedArraySizeExpression())))))
                        .WithInitializer(initializer)
                        .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space));

                    return newArray.WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
                }

                if (useList)
                {
                    var elementType = namedType.TypeArguments[0];
                    var elementTypeSyntax = SyntaxHelper.GenerateTypeSyntax(elementType, semanticModel, node.SpanStart, this);
                    TypeSyntax typeSyntax;

                    if (namedType.TypeKind == TypeKind.Interface)
                    {
                         typeSyntax = SyntaxFactory.QualifiedName(
                            SyntaxFactory.ParseName("System.Collections.Generic"),
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("List"),
                                SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(elementTypeSyntax))));
                    }
                    else
                    {
                        typeSyntax = SyntaxHelper.GenerateTypeSyntax(namedType, semanticModel, node.SpanStart, this);
                    }

                    var arrayInitializer = SyntaxFactory.InitializerExpression(
                        SyntaxKind.ArrayInitializerExpression,
                        SyntaxFactory.SeparatedList<ExpressionSyntax>(visitedElements));

                    var newArray = SyntaxFactory.ArrayCreationExpression(
                        SyntaxFactory.ArrayType(elementTypeSyntax)
                            .WithRankSpecifiers(SyntaxFactory.SingletonList(SyntaxFactory.ArrayRankSpecifier(SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(SyntaxFactory.OmittedArraySizeExpression())))))
                        .WithInitializer(arrayInitializer)
                        .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space));

                    var newList = SyntaxFactory.ObjectCreationExpression(typeSyntax)
                        .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                        .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(newArray))));

                    return newList.WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
                }

                // Fallback: Try `new Type { elements }`
                var fallbackTypeSyntax = SyntaxHelper.GenerateTypeSyntax(namedType, semanticModel, node.SpanStart, this);
                 var fallbackInitializer = SyntaxFactory.InitializerExpression(
                        SyntaxKind.CollectionInitializerExpression,
                        SyntaxFactory.SeparatedList<ExpressionSyntax>(visitedElements));

                 var fallbackObj = SyntaxFactory.ObjectCreationExpression(fallbackTypeSyntax)
                        .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                        .WithArgumentList(SyntaxFactory.ArgumentList())
                        .WithInitializer(fallbackInitializer);

                 return fallbackObj.WithLeadingTrivia(node.GetLeadingTrivia()).WithTrailingTrivia(node.GetTrailingTrivia());
            }

            return base.VisitCollectionExpression(node);

        }

        private SyntaxNode RewriteCollectionExpressionWithSpread(CollectionExpressionSyntax node, ITypeSymbol targetType)
        {
            var elementType = GetCollectionElementType(targetType);

            if (elementType == null)
            {
                var mapped = semanticModel.SyntaxTree.GetLineSpan(node.Span);
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "{2} - {3}({0},{1}): {4}", mapped.StartLinePosition.Line + 1, mapped.StartLinePosition.Character + 1, "Cannot determine element type for collection expression with spread elements", semanticModel.SyntaxTree.FilePath, node.ToString()));
            }

            var elementTypeSyntax = SyntaxHelper.GenerateTypeSyntax(elementType, semanticModel, node.SpanStart, this);
            var listTypeSyntax = SyntaxFactory.QualifiedName(
                SyntaxFactory.ParseName("global::System.Collections.Generic"),
                SyntaxFactory.GenericName(SyntaxFactory.Identifier("List"), SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(elementTypeSyntax))));

            var statements = new List<StatementSyntax>();
            var listVarName = GetUniqueTempKey("list");

            var creation = SyntaxFactory.ObjectCreationExpression(listTypeSyntax)
                .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(SyntaxFactory.Space))
                .WithArgumentList(SyntaxFactory.ArgumentList());

            statements.Add(SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("var"))
                .WithVariables(SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(listVarName))
                    .WithInitializer(SyntaxFactory.EqualsValueClause(creation)))))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

            foreach (var element in node.Elements)
            {
                if (element is ExpressionElementSyntax expr)
                {
                    var visitedExpr = (ExpressionSyntax)Visit(expr.Expression);
                    var addCall = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(listVarName), SyntaxFactory.IdentifierName("Add")),
                        SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(visitedExpr))));

                    statements.Add(SyntaxFactory.ExpressionStatement(addCall).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
                }
                else if (element is SpreadElementSyntax spread)
                {
                    var visitedExpr = (ExpressionSyntax)Visit(spread.Expression);
                    var tempVarName = GetUniqueTempKey("spread");

                    // Assign spread expression to temp variable
                    var typeInfo = semanticModel.GetTypeInfo(spread.Expression);
                    var spreadType = typeInfo.Type ?? typeInfo.ConvertedType;
                    var spreadTypeSyntax = SyntaxHelper.GenerateTypeSyntax(spreadType, semanticModel, spread.SpanStart, this);

                    statements.Add(SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(spreadTypeSyntax)
                        .WithVariables(SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(tempVarName))
                            .WithInitializer(SyntaxFactory.EqualsValueClause(visitedExpr)))))
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

                    // if (temp != null) list.AddRange(temp)
                    var addRangeCall = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(listVarName), SyntaxFactory.IdentifierName("AddRange")),
                        SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(tempVarName)))));

                    var ifStatement = SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, SyntaxFactory.IdentifierName(tempVarName), SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.ExpressionStatement(addRangeCall).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

                    statements.Add(ifStatement);
                }
            }

            ExpressionSyntax resultExpr = SyntaxFactory.IdentifierName(listVarName);

            bool targetIsList = false;
            if (targetType is INamedTypeSymbol named &&
                (named.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_IList_T ||
                 named.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_ICollection_T ||
                 (named.Name == "List" && named.ContainingNamespace?.ToDisplayString() == "System.Collections.Generic")))
            {
                targetIsList = true;
            }

            if (!targetIsList)
            {
                resultExpr = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, resultExpr, SyntaxFactory.IdentifierName("ToArray")),
                    SyntaxFactory.ArgumentList());
            }

            statements.Add(SyntaxFactory.ReturnStatement(resultExpr).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

            var block = SyntaxFactory.Block(statements);
            var lambda = SyntaxFactory.ParenthesizedLambdaExpression(SyntaxFactory.ParameterList(), block);

            var isAsync = AwaitersCollector.HasAwaiters(semanticModel, node);
            if (isAsync)
            {
                lambda = lambda.WithAsyncKeyword(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
            }

            TypeSyntax returnTypeSyntax;
            if (targetIsList)
            {
                returnTypeSyntax = listTypeSyntax;
            }
            else
            {
                returnTypeSyntax = SyntaxFactory.ArrayType(elementTypeSyntax)
                    .WithRankSpecifiers(SyntaxFactory.SingletonList(SyntaxFactory.ArrayRankSpecifier(SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(SyntaxFactory.OmittedArraySizeExpression()))));
            }

            TypeSyntax funcTypeSyntax;
            if (isAsync)
            {
                var taskType = SyntaxFactory.QualifiedName(
                    SyntaxFactory.ParseName("global::System.Threading.Tasks"),
                    SyntaxFactory.GenericName(SyntaxFactory.Identifier("Task"), SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(returnTypeSyntax))));

                funcTypeSyntax = SyntaxFactory.QualifiedName(
                    SyntaxFactory.IdentifierName("global::System"),
                    SyntaxFactory.GenericName(SyntaxFactory.Identifier("Func"), SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList<TypeSyntax>(taskType))));
            }
            else
            {
                funcTypeSyntax = SyntaxFactory.QualifiedName(
                    SyntaxFactory.IdentifierName("global::System"),
                    SyntaxFactory.GenericName(SyntaxFactory.Identifier("Func"), SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(returnTypeSyntax))));
            }

            var cast = SyntaxFactory.CastExpression(funcTypeSyntax, SyntaxFactory.ParenthesizedExpression(lambda));
            var invocation = SyntaxFactory.InvocationExpression(SyntaxFactory.ParenthesizedExpression(cast));

            if (isAsync)
            {
                return SyntaxFactory.AwaitExpression(invocation).NormalizeWhitespace();
            }

            return invocation.NormalizeWhitespace();
        }

        private TypeParameterConstraintClauseSyntax GetConstraint(ITypeParameterSymbol tp, int pos)
        {
            var constraints = new List<TypeParameterConstraintSyntax>();

            if (tp.HasReferenceTypeConstraint)
            {
                constraints.Add(SyntaxFactory.ClassOrStructConstraint(SyntaxKind.ClassConstraint));
            }
            else if (tp.HasValueTypeConstraint)
            {
                constraints.Add(SyntaxFactory.ClassOrStructConstraint(SyntaxKind.StructConstraint));
            }

            if (tp.HasConstructorConstraint)
            {
                constraints.Add(SyntaxFactory.ConstructorConstraint());
            }

            foreach (var type in tp.ConstraintTypes)
            {
                constraints.Add(SyntaxFactory.TypeConstraint(SyntaxHelper.GenerateTypeSyntax(type, semanticModel, pos, this)));
            }

            if (constraints.Count > 0)
            {
                return SyntaxFactory.TypeParameterConstraintClause(SyntaxFactory.IdentifierName(tp.Name), SyntaxFactory.SeparatedList(constraints))
                    .WithWhereKeyword(SyntaxFactory.Token(SyntaxKind.WhereKeyword).WithLeadingTrivia(SyntaxFactory.Space).WithTrailingTrivia(SyntaxFactory.Space));
            }

            return null;
        }

        private ArgumentListSyntax ProcessCallerArgumentExpression(ArgumentListSyntax argumentList, SyntaxNode originalNode, IMethodSymbol method, SemanticModel semanticModel)
        {
            if (method == null || argumentList == null)
            {
                return argumentList;
            }

            var newArgs = new List<ArgumentSyntax>(argumentList.Arguments);
            bool changed = false;

            // Get original arguments for text extraction
            // We need to map parameters to arguments in original node to get text
            // originalNode is InvocationExpressionSyntax or ObjectCreationExpressionSyntax
            ArgumentListSyntax originalArgsList = null;
            if (originalNode is InvocationExpressionSyntax inv) originalArgsList = inv.ArgumentList;
            else if (originalNode is ObjectCreationExpressionSyntax obj) originalArgsList = obj.ArgumentList;

            if (originalArgsList == null) return argumentList;

            for (int i = 0; i < method.Parameters.Length; i++)
            {
                var param = method.Parameters[i];
                var callerArgAttr = param.GetAttributes().FirstOrDefault(a =>
                    a.AttributeClass != null &&
                    a.AttributeClass.Name == "CallerArgumentExpressionAttribute" &&
                    (a.AttributeClass.ContainingNamespace?.ToDisplayString() == "System.Runtime.CompilerServices"));

                if (callerArgAttr != null)
                {
                    // Check if argument is explicitly provided
                    // Logic:
                    // 1. Check for named argument
                    bool isProvided = argumentList.Arguments.Any(a => a.NameColon != null && a.NameColon.Name.Identifier.ValueText == param.Name);

                    // 2. Check for positional argument
                    if (!isProvided)
                    {
                        // Count positional arguments up to this parameter
                        int positionalCount = 0;
                        foreach (var arg in argumentList.Arguments)
                        {
                            if (arg.NameColon == null) positionalCount++;
                        }

                        // If we have enough positional args, then this param is provided
                        if (positionalCount > i) isProvided = true;
                    }

                    if (!isProvided)
                    {
                        // Get target parameter name
                        string targetParamName = callerArgAttr.ConstructorArguments.FirstOrDefault().Value as string;
                        if (!string.IsNullOrEmpty(targetParamName))
                        {
                            // Find target parameter symbol
                            var targetParam = method.Parameters.FirstOrDefault(p => p.Name == targetParamName);
                            if (targetParam != null)
                            {
                                // Find argument for target parameter in ORIGINAL node
                                ArgumentSyntax targetArg = null;
                                int targetParamIndex = method.Parameters.IndexOf(targetParam);

                                // Check named args in original
                                targetArg = originalArgsList.Arguments.FirstOrDefault(a => a.NameColon != null && a.NameColon.Name.Identifier.ValueText == targetParamName);

                                if (targetArg == null)
                                {
                                    // Check positional args in original
                                    int originalPositionalCount = 0;
                                    foreach (var arg in originalArgsList.Arguments)
                                    {
                                        if (arg.NameColon == null)
                                        {
                                            if (originalPositionalCount == targetParamIndex)
                                            {
                                                targetArg = arg;
                                                break;
                                            }
                                            originalPositionalCount++;
                                        }
                                    }
                                }

                                if (targetArg != null)
                                {
                                    // Capture text
                                    var text = targetArg.Expression.ToString();

                                    // Create string literal
                                    var literal = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(text));

                                    // Add argument
                                    newArgs.Add(SyntaxFactory.Argument(SyntaxFactory.NameColon(param.Name), default, literal));
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            }

            if (changed)
            {
                return SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(newArgs));
            }

            return argumentList;
        }

        private bool IsCapturedPrimaryConstructorParameter(ISymbol symbol, out string fieldName)
        {
            fieldName = null;
            if (_primaryConstructorCaptures.Count == 0 || symbol == null)
            {
                return false;
            }

            var captures = _primaryConstructorCaptures.Peek();
            return captures != null && captures.TryGetValue(symbol, out fieldName);
        }

        private bool ShouldUseFieldForCapturedParameter(SyntaxNode node)
        {
            var parent = node.Parent;
            while (parent != null)
            {
                if (parent is MethodDeclarationSyntax ||
                    parent is AccessorDeclarationSyntax)
                {
                    return true;
                }

                // If in arrow expression body, check if it belongs to something that should use the field
                if (parent is ArrowExpressionClauseSyntax)
                {
                    var arrowParent = parent.Parent;
                    if (arrowParent is MethodDeclarationSyntax ||
                        arrowParent is LocalFunctionStatementSyntax ||
                        arrowParent is AccessorDeclarationSyntax ||
                        arrowParent is PropertyDeclarationSyntax ||
                        arrowParent is IndexerDeclarationSyntax)
                    {
                        // Expression-bodied members use captured fields
                        return true;
                    }
                    // What else uses arrow?
                }

                if (parent is VariableDeclaratorSyntax && parent.Parent is VariableDeclarationSyntax && parent.Parent.Parent is FieldDeclarationSyntax)
                {
                    // Inside field initializer - use parameter
                    return false;
                }

                if (parent is PropertyDeclarationSyntax && (parent as PropertyDeclarationSyntax).Initializer != null)
                {
                    // Inside property initializer - use parameter
                    return false;
                }

                if (parent is BaseListSyntax || parent is ConstructorInitializerSyntax)
                {
                    // Inside base call - use parameter
                    return false;
                }

                if (parent is TypeDeclarationSyntax)
                {
                    // Reached class/struct level without finding method/accessor
                    // This implies we are in some other member like attribute?
                    // Attributes on members?
                    // [Attr(param)] void M() -> attribute arguments must be constant, so param usage is invalid anyway.
                    // So return false (don't rewrite).
                    return false;
                }

                parent = parent.Parent;
            }

            return false;
        }

        private void ProcessPrimaryConstructor(TypeDeclarationSyntax node)
        {
            Dictionary<ISymbol, string> captures = new Dictionary<ISymbol, string>(SymbolEqualityComparer.Default);

            if (node.ParameterList != null)
            {
                // Identify captured parameters
                var parameterSymbols = new Dictionary<string, IParameterSymbol>();
                foreach (var param in node.ParameterList.Parameters)
                {
                    var symbol = semanticModel.GetDeclaredSymbol(param);
                    if (symbol != null)
                    {
                        parameterSymbols[param.Identifier.ValueText] = symbol;
                    }
                }

                if (parameterSymbols.Count > 0)
                {
                    // Scan usages in the class
                    // We need to look for IdentifierNameSyntax that resolves to one of our parameters
                    // And is inside a location that requires capturing (method, property body)

                    // Optimization: We can just scan all descendant identifiers.
                    // But we must exclude the ParameterList itself.

                    foreach (var member in node.Members)
                    {
                         // Visit all identifiers in members
                         foreach (var identifier in member.DescendantNodes().OfType<IdentifierNameSyntax>())
                         {
                             var symbolInfo = semanticModel.GetSymbolInfo(identifier);
                             var symbol = symbolInfo.Symbol;

                             if (symbol != null && symbol is IParameterSymbol ps && parameterSymbols.Values.Any(p => SymbolEqualityComparer.Default.Equals(p, ps)))
                             {
                                 // Found usage. Check if it's a capture scenario.
                                 if (ShouldUseFieldForCapturedParameter(identifier))
                                 {
                                     if (!captures.ContainsKey(ps))
                                     {
                                         // Create unique field name
                                         // Usually just the name, but to avoid collision with other members?
                                         // Primary constructor parameters share scope with class members.
                                         // If class has 'int x', and primary ctor has 'int x',
                                         // then 'x' inside method refers to 'this.x' (field) not param 'x' (shadowed).
                                         // C# rules say:
                                         // Simple names look up parameters first in scope of class?
                                         // No, parameters are in scope throughout class body.
                                         // But they can be shadowed by members.
                                         // If 'x' resolves to ParameterSymbol, it means it was NOT shadowed.

                                         // So we generate a backing field.
                                         // Use a name that won't conflict. "<p>Name" or similar.
                                         // Or just use the name if we assume H5 handles private fields okay?
                                         // Let's use a safe name.
                                         captures[ps] = "_ctor_param_" + ps.Name;
                                     }
                                 }
                             }
                         }
                    }
                }
            }

            _primaryConstructorCaptures.Push(captures);
        }

        private TypeDeclarationSyntax SynthesizePrimaryConstructor(TypeDeclarationSyntax originalNode, TypeDeclarationSyntax rewrittenNode)
        {
            var captures = _primaryConstructorCaptures.Peek();

            // 1. Remove ParameterList
            rewrittenNode = rewrittenNode.WithParameterList(null);

            // 2. Extract Base Constructor Arguments
            ArgumentListSyntax baseArgs = null;
            if (originalNode.BaseList != null)
            {
                var newTypes = new List<BaseTypeSyntax>();
                bool changed = false;
                foreach (var baseType in originalNode.BaseList.Types)
                {
                    if (baseType is PrimaryConstructorBaseTypeSyntax pcbt)
                    {
                        baseArgs = pcbt.ArgumentList;
                        // Keep the type, but convert to SimpleBaseType
                        var newBase = SyntaxFactory.SimpleBaseType(pcbt.Type)
                            .WithLeadingTrivia(pcbt.GetLeadingTrivia())
                            .WithTrailingTrivia(pcbt.GetTrailingTrivia());
                        newTypes.Add(newBase);
                        changed = true;
                    }
                    else
                    {
                        newTypes.Add(baseType);
                    }
                }

                if (changed)
                {
                    var rewrittenBaseList = rewrittenNode.BaseList;
                    var rewrittenTypes = new List<BaseTypeSyntax>();
                    int idx = 0;
                    foreach (var bt in rewrittenBaseList.Types)
                    {
                        if (idx < originalNode.BaseList.Types.Count && originalNode.BaseList.Types[idx] is PrimaryConstructorBaseTypeSyntax)
                        {
                            if (bt is PrimaryConstructorBaseTypeSyntax btpc)
                            {
                                rewrittenTypes.Add(SyntaxFactory.SimpleBaseType(btpc.Type).WithLeadingTrivia(bt.GetLeadingTrivia()).WithTrailingTrivia(bt.GetTrailingTrivia()));
                            }
                            else
                            {
                                rewrittenTypes.Add(bt);
                            }
                        }
                        else
                        {
                            rewrittenTypes.Add(bt);
                        }
                        idx++;
                    }
                    rewrittenNode = rewrittenNode.WithBaseList(rewrittenBaseList.WithTypes(SyntaxFactory.SeparatedList(rewrittenTypes)));
                }
            }

            // 3. Extract Field Initializers
            var newMembers = new List<MemberDeclarationSyntax>();
            var movedInitializers = new List<StatementSyntax>();

            foreach (var member in rewrittenNode.Members)
            {
                if (member is FieldDeclarationSyntax fd && !fd.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword) || m.IsKind(SyntaxKind.ConstKeyword)))
                {
                    // Instance field
                    var newVars = new List<VariableDeclaratorSyntax>();
                    bool changed = false;
                    foreach (var v in fd.Declaration.Variables)
                    {
                        if (v.Initializer != null)
                        {
                            string targetName = v.Identifier.ValueText;
                            if (targetName.StartsWith(AutoInitFieldPrefix))
                            {
                                // It's a property backing field generated by VisitPropertyDeclaration
                                // We remove the field and assign to the property instead
                                targetName = targetName.Substring(AutoInitFieldPrefix.Length);
                                var assign = SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ThisExpression(), SyntaxFactory.IdentifierName(targetName)),
                                    v.Initializer.Value
                                );
                                movedInitializers.Add(SyntaxFactory.ExpressionStatement(assign));
                                // Do not add to newVars (remove the field)
                                changed = true;
                            }
                            else
                            {
                                // Move initializer to statement: this.v = init;
                                var assign = SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ThisExpression(), SyntaxFactory.IdentifierName(targetName)),
                                    v.Initializer.Value
                                );
                                movedInitializers.Add(SyntaxFactory.ExpressionStatement(assign));
                                newVars.Add(v.WithInitializer(null));
                                changed = true;
                            }
                        }
                        else
                        {
                            newVars.Add(v);
                        }
                    }
                    if (changed)
                    {
                        if (newVars.Count > 0)
                        {
                            newMembers.Add(fd.WithDeclaration(fd.Declaration.WithVariables(SyntaxFactory.SeparatedList(newVars))));
                        }
                    }
                    else
                    {
                        newMembers.Add(fd);
                    }
                }
                else if (member is PropertyDeclarationSyntax pd && !pd.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)))
                {
                    if (pd.Initializer != null)
                    {
                        // Instance property with initializer
                        // Should have been handled by VisitPropertyDeclaration but if not:
                        var assign = SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ThisExpression(), SyntaxFactory.IdentifierName(pd.Identifier)),
                                pd.Initializer.Value
                            );
                        movedInitializers.Add(SyntaxFactory.ExpressionStatement(assign));
                        newMembers.Add(pd.WithInitializer(null).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.None)));
                    }
                    else
                    {
                        newMembers.Add(pd);
                    }
                }
                else
                {
                    newMembers.Add(member);
                }
            }

            // 4. Synthesize Fields for Captures
            if (captures != null)
            {
                foreach (var kvp in captures)
                {
                    var symbol = kvp.Key as IParameterSymbol;
                    var fieldName = kvp.Value;

                    var typeSyntax = SyntaxHelper.GenerateTypeSyntax(symbol.Type, semanticModel, originalNode.SpanStart, this);

                    // Use ParseMemberDeclaration to ensure valid syntax structure including trivia
                    var fieldCode = $"private {typeSyntax.ToString()} {fieldName};" + Environment.NewLine;
                    var fieldDecl = (FieldDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration(fieldCode);

                    newMembers.Insert(0, fieldDecl);
                }
            }

            // 5. Synthesize Constructor
            var ctorParams = originalNode.ParameterList.Parameters;

            var ctorBodyStatements = new List<StatementSyntax>();

            // Assign captures: this._field = param;
            if (captures != null)
            {
                foreach (var kvp in captures)
                {
                    var symbol = kvp.Key as IParameterSymbol;
                    var fieldName = kvp.Value;
                    var paramName = symbol.Name;

                    var assign = SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ThisExpression(), SyntaxFactory.IdentifierName(fieldName)),
                        SyntaxFactory.IdentifierName(paramName)
                    );
                    ctorBodyStatements.Add(SyntaxFactory.ExpressionStatement(assign));
                }
            }

            ctorBodyStatements.AddRange(movedInitializers);

            var ctor = SyntaxFactory.ConstructorDeclaration(originalNode.Identifier)
                .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)))
                .WithParameterList(originalNode.ParameterList)
                .WithBody(SyntaxFactory.Block(ctorBodyStatements));

            if (baseArgs != null)
            {
                ctor = ctor.WithInitializer(SyntaxFactory.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer, baseArgs));
            }

            newMembers.Add(ctor);

            return rewrittenNode.WithMembers(SyntaxFactory.List(newMembers));
        }
    }
}