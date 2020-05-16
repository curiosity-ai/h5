using System;
using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Object.Net.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace H5.Translator
{
    public class InlineArgumentsBlock : AbstractEmitterBlock
    {
        public InlineArgumentsBlock(IEmitter emitter, ArgumentsInfo argsInfo, string inline, IMethod method = null, ResolveResult targetResolveResult = null)
            : base(emitter, argsInfo.Expression)
        {
            Emitter = emitter;
            ArgumentsInfo = argsInfo;
            InlineCode = inline;

            argsInfo.AddExtensionParam();
            Method = method;
            TargetResolveResult = targetResolveResult;

            if (argsInfo.Expression != null)
            {
                if (emitter.Resolver.ResolveNode(argsInfo.Expression, emitter) is MemberResolveResult rr)
                {
                    H5Type h5Type = emitter.H5Types.Get(rr.Member.DeclaringType, true);

                    if (h5Type != null)
                    {
                        bool isCustomName;
                        H5Types.AddModule(null, h5Type, false, false, out isCustomName);
                    }
                }
            }
        }

        public int[] IgnoreRange
        {
            get; set;
        }

        public IMethod Method
        {
            get; set;
        }

        public ResolveResult TargetResolveResult
        {
            get; set;
        }

        public ArgumentsInfo ArgumentsInfo { get; set; }

        public string InlineCode { get; set; }

        protected override void DoEmit()
        {
            EmitInlineExpressionList(ArgumentsInfo, InlineCode);
        }

        internal static Regex FormatArgRegex = new Regex(@"\{(\*?)(\w+)(\:(\w+))?\}");
        private static Regex _inlineMethod = new Regex(@"([$\w\.\{\}\(\)]+)\(\s*(.*)\)");

        protected virtual IList<Expression> GetExpressionsByKey(IEnumerable<NamedParamExpression> expressions, string key)
        {
            if (expressions == null)
            {
                return new List<Expression>();
            }

            if (Regex.IsMatch(key, "^\\d+$"))
            {
                var list = new List<Expression>();

                list.Add(expressions.Skip(int.Parse(key)).First().Expression);

                return list;
            }

            return expressions.Where(e => e.Name == key).Select(e => e.Expression).ToList();
        }

        protected virtual IList<ResolveResult> GetResolveResultByKey(string key)
        {
            if (ArgumentsInfo.Attribute.PositionalArguments.Count == 0)
            {
                return new List<ResolveResult>();
            }

            if (Regex.IsMatch(key, "^\\d+$"))
            {
                return ArgumentsInfo.Attribute.PositionalArguments.Skip(int.Parse(key)).ToList();
            }

            var p = ArgumentsInfo.Attribute.Constructor.Parameters.FirstOrDefault(cp => cp.Name == key);

            if (p != null)
            {
                var idx = ArgumentsInfo.Attribute.Constructor.Parameters.IndexOf(p);
                return p.IsParams ? ArgumentsInfo.Attribute.PositionalArguments.Skip(idx).ToList() : new List<ResolveResult> { ArgumentsInfo.Attribute.PositionalArguments[idx] };
            }

            return new List<ResolveResult>();
        }

        protected virtual IList<string> GetStringArgumentByKey(string key)
        {
            if (ArgumentsInfo.StringArguments.Length == 0)
            {
                return new List<string>();
            }

            if (Regex.IsMatch(key, "^\\d+$"))
            {
                var i = int.Parse(key);
                var p1 = Method.Parameters[i];

                return p1.IsParams ? ArgumentsInfo.StringArguments.Skip(i).ToList() : ArgumentsInfo.StringArguments.Skip(i).Take(1).ToList();
            }

            var p = Method.Parameters.FirstOrDefault(cp => cp.Name == key);

            if (p != null)
            {
                var idx = Method.Parameters.IndexOf(p);
                return p.IsParams ? ArgumentsInfo.StringArguments.Skip(idx).ToList() : new List<string> { ArgumentsInfo.StringArguments[idx] };
            }

            return new List<string>();
        }

        protected virtual AstType GetAstTypeByKey(IEnumerable<TypeParamExpression> types, string key)
        {
            return types.Where(e => e.Name == key && e.AstType != null).Select(e => e.AstType).FirstOrDefault();
        }

        protected virtual TypeParamExpression GetTypeByKey(IEnumerable<TypeParamExpression> types, string key)
        {
            return types.Where(e => e.Name == key && e.IType != null).FirstOrDefault();
        }

        public static string ReplaceInlineArgs(AbstractEmitterBlock block, string inline, Expression[] args)
        {
            if (args.Length > 0)
            {
                var emitter = block.Emitter;
                inline = InlineArgumentsBlock.FormatArgRegex.Replace(inline, delegate (Match m)
                {
                    int count = emitter.Writers.Count;
                    string key = m.Groups[2].Value;
                    string modifier = m.Groups[1].Success ? m.Groups[4].Value : null;

                    if (!string.IsNullOrWhiteSpace(modifier) && modifier != "raw")
                    {
                        return m.Value;
                    }

                    StringBuilder oldSb = emitter.Output;
                    emitter.Output = new StringBuilder();

                    Expression expr = null;

                    if (Regex.IsMatch(key, "^\\d+$"))
                    {
                        expr = args.Skip(int.Parse(key)).FirstOrDefault();
                    }
                    else
                    {
                        expr = args.FirstOrDefault(e => e.ToString() == key);
                    }

                    string s = "";
                    if (expr != null)
                    {
                        var writer = block.SaveWriter();
                        block.NewWriter();
                        expr.AcceptVisitor(emitter);
                        s = emitter.Output.ToString();
                        block.RestoreWriter(writer);

                        if (modifier == "raw")
                        {
                            s = block.RemoveTokens(s).Trim('"');
                        }
                    }

                    block.Write(block.WriteIndentToString(s));

                    if (emitter.Writers.Count != count)
                    {
                        block.PopWriter();
                    }

                    string replacement = emitter.Output.ToString();
                    emitter.Output = oldSb;

                    return replacement;
                });
            }

            return inline;
        }

        protected virtual void WriteParamName(string name)
        {
            if (Method.TypeParameters.Count > 0 && Method.TypeArguments.Count > 0)
            {
                var tp = Method.TypeParameters.FirstOrDefault(p => p.Name == name);
                if (tp != null)
                {
                    name = H5Types.ToJsName(Method.TypeArguments[tp.Index], Emitter);
                }
            }

            if (Helpers.IsReservedWord(Emitter, name))
            {
                name = Helpers.ChangeReservedWord(name);
            }

            Write(name);
        }

        private string[] allowedModifiers = new[] {"default", "defaultFn", "raw", "plain", "body", "gettmp", "version", "tmp", "type", "array", "module", "nobox", CS.Methods.GETHASHCODE, CS.Methods.TOSTRING };

        protected virtual void EmitInlineExpressionList(ArgumentsInfo argsInfo, string inline, bool asRef = false, bool isNull = false, bool? definition = null)
        {
            if (inline != null && inline.Trim().EndsWith(";"))
            {
                inline = inline.Trim().TrimEnd(';');
            }

            IMember member = Method ?? argsInfo.Method ?? argsInfo.ResolveResult?.Member;
            if (member == null && argsInfo.Expression != null && argsInfo.Expression.Parent != null)
            {
                if (Emitter.Resolver.ResolveNode(argsInfo.Expression, Emitter) is MemberResolveResult rre)
                {
                    member = rre.Member;
                }
            }

            if (member != null)
            {
                inline = Helpers.ConvertTokens(Emitter, inline, member);
            }

            IEnumerable<NamedParamExpression> expressions = argsInfo.NamedExpressions;
            IEnumerable<TypeParamExpression> typeParams = argsInfo.TypeArguments;
            bool addClose = false;
            Write("");

            if (asRef)
            {
                var withoutTypeParams = Method.TypeArguments.Count > 0 &&
                                     Method.TypeArguments.All(t => t.Kind != TypeKind.TypeParameter);

                if (definition.HasValue)
                {
                    withoutTypeParams = !definition.Value;
                }

                if (withoutTypeParams && (!Method.IsStatic || Method.IsExtensionMethod && TargetResolveResult is ThisResolveResult) /*&& (this.TargetResolveResult is ThisResolveResult || this.TargetResolveResult == null)*/ && (inline.Contains("{this}") || Method.IsStatic || Method.IsExtensionMethod && inline.Contains("{" + Method.Parameters.First().Name + "}")))
                {
                    Write(JS.Funcs.H5_BIND);
                    Write("(this, ");
                    addClose = true;
                }

                Write("function (");
                EmitMethodParameters(Method, Method.Parameters, withoutTypeParams ? null : Method.TypeParameters, isNull);
                Write(") { return ");
            }

            bool needExpand = false;
            bool expandParams = false;

            string paramsName = null;
            IType paramsType = null;
            int paramsIndex = 0;
            if (argsInfo.ResolveResult != null)
            {
                var paramsParam = argsInfo.ResolveResult.Member.Parameters.FirstOrDefault(p => p.IsParams);
                if (paramsParam != null)
                {
                    paramsIndex = argsInfo.ResolveResult.Member.Parameters.IndexOf(paramsParam);
                    paramsName = paramsParam.Name;
                    paramsType = paramsParam.Type;
                }
                expandParams = argsInfo.ResolveResult.Member.Attributes.Any(a => a.AttributeType.FullName == "H5.ExpandParamsAttribute");
            }
            else if (argsInfo.Method != null)
            {
                var paramsParam = argsInfo.Method.Parameters.FirstOrDefault(p => p.IsParams);
                if (paramsParam != null)
                {
                    paramsIndex = argsInfo.Method.Parameters.IndexOf(paramsParam);
                    paramsName = paramsParam.Name;
                    paramsType = paramsParam.Type;
                }
                expandParams = argsInfo.Method.Attributes.Any(a => a.AttributeType.FullName == "H5.ExpandParamsAttribute");
            }

            if (paramsName != null)
            {
                var matches = InlineArgumentsBlock.FormatArgRegex.Matches(inline);
                bool ignoreArray = false;
                foreach (Match m in matches)
                {
                    if (m.Groups[2].Value == paramsName)
                    {
                        bool isRaw = m.Groups[1].Success && m.Groups[1].Value == "*";
                        ignoreArray = isRaw || argsInfo.ParamsExpression == null;
                        string modifier = m.Groups[1].Success ? m.Groups[4].Value : null;

                        if (modifier == "array")
                        {
                            ignoreArray = false;
                        }

                        break;
                    }
                }

                if (expandParams)
                {
                    ignoreArray = true;
                }

                if (argsInfo.ResolveResult is CSharpInvocationResolveResult)
                {
                    needExpand = !((CSharpInvocationResolveResult)argsInfo.ResolveResult).IsExpandedForm;
                }

                if (needExpand && ignoreArray && !asRef)
                {
                    IList<Expression> exprs = GetExpressionsByKey(expressions, paramsName);

                    if (exprs.Count == 1 && exprs[0] != null && exprs[0].Parent != null)
                    {
                        var exprrr = Emitter.Resolver.ResolveNode(exprs[0], Emitter);
                        if (exprrr.Type.Kind == TypeKind.Array)
                        {
                            var match = _inlineMethod.Match(inline);

                            if (match.Success)
                            {
                                string target = null;
                                var methodName = match.Groups[1].Value;

                                if (methodName.Contains("."))
                                {
                                    target = methodName.LeftOfRightmostOf('.');
                                }

                                string args = match.Groups[2].Value;

                                StringBuilder sb = new StringBuilder();
                                sb.Append(methodName);
                                sb.Append(".");
                                sb.Append(JS.Funcs.APPLY);
                                sb.Append("(");
                                sb.Append(target ?? "null");

                                if (args.Contains(","))
                                {
                                    sb.Append(", [");
                                    sb.Append(args.LeftOfRightmostOf(',').Trim());
                                    sb.Append("].concat(");
                                    sb.Append(args.RightOfRightmostOf(',').Trim());
                                    sb.Append(")");
                                }
                                else
                                {
                                    sb.Append(",");
                                    sb.Append(args);
                                }

                                sb.Append(")");

                                inline = inline.Remove(match.Index, match.Length);
                                inline = inline.Insert(match.Index, sb.ToString());
                            }
                        }
                    }
                }
            }

            var r = InlineArgumentsBlock.FormatArgRegex.Matches(inline);
            List<Match> keyMatches = new List<Match>();
            foreach (Match keyMatch in r)
            {
                keyMatches.Add(keyMatch);
            }

            var tempVars = new Dictionary<string, string>();
            var tempMap = new Dictionary<string, string>();

            inline = InlineArgumentsBlock.FormatArgRegex.Replace(inline, delegate (Match m)
            {
                if (IgnoreRange != null && m.Index >= IgnoreRange[0] && m.Index <= IgnoreRange[1])
                {
                    return m.Value;
                }

                int count = Emitter.Writers.Count;
                string key = m.Groups[2].Value;
                bool isRaw = m.Groups[1].Success && m.Groups[1].Value == "*";
                bool ignoreArray = isRaw || argsInfo.ParamsExpression == null;
                string modifier = m.Groups[1].Success ? m.Groups[4].Value : null;
                Emitter.TemplateModifier = modifier;

                bool isSimple = false;

                var tempKey = key + ":" + modifier ?? "";
                if (tempMap.ContainsKey(tempKey))
                {
                    return tempMap[tempKey];
                }

                if (!string.IsNullOrWhiteSpace(modifier) && !allowedModifiers.Contains(modifier))
                {
                    return m.Value;
                }

                if (modifier == "array")
                {
                    ignoreArray = false;
                }

                StringBuilder oldSb = Emitter.Output;
                Emitter.Output = new StringBuilder();

                if (modifier == "module")
                {
                    IList<Expression> exprs = GetExpressionsByKey(expressions, key);

                    if (exprs.Count > 0)
                    {
                        var amd = new List<string>();
                        var cjs = new List<string>();
                        foreach (var expr in exprs)
                        {
                            if (!(Emitter.Resolver.ResolveNode(expr, Emitter) is TypeOfResolveResult rr))
                            {
                                throw new EmitterException(expr, "Module.Load supports typeof expression only");
                            }

                            var h5Type = Emitter.H5Types.Get(rr.ReferencedType, true);
                            Module module = null;

                            if (h5Type.TypeInfo == null)
                            {
                                H5Types.EnsureModule(h5Type);
                                module = h5Type.Module;
                            }
                            else
                            {
                                module = h5Type.TypeInfo.Module;
                            }

                            AddModuleByType(amd, cjs, module);
                        }

                        Write("{");

                        if (amd.Count > 0)
                        {
                            Write("amd: ");
                            Write(Emitter.ToJavaScript(amd.ToArray()));
                            if (cjs.Count > 0)
                            {
                                Write(", ");
                            }
                        }

                        if (cjs.Count > 0)
                        {
                            Write("cjs: ");
                            Write(Emitter.ToJavaScript(cjs.ToArray()));
                        }

                        if (!string.IsNullOrWhiteSpace(Emitter.AssemblyInfo.Loader.FunctionName))
                        {
                            Write(", ");
                            Write(Emitter.ToJavaScript(Emitter.AssemblyInfo.Loader.FunctionName));
                        }

                        Write("}, function () { ");

                        var idx = 0;
                        var list = amd.Concat(cjs);

                        foreach (var moduleName in list)
                        {
                            Write(moduleName);
                            Write(" = arguments[");
                            Write(idx++);
                            Write("];");
                        }

                        Write(" }");
                    }
                }
                else if (asRef)
                {
                    if (Regex.IsMatch(key, "^\\d+$"))
                    {
                        var index = int.Parse(key);
                        key = Method.Parameters[index].Name;
                    }

                    if (modifier == "type")
                    {
                        Write(JS.Funcs.H5_GET_TYPE + "(");
                    }

                    if (key == "this")
                    {
                        if (isNull)
                        {
                            isSimple = true;
                            Write(JS.Vars.T);
                        }
                        else if (Method.IsExtensionMethod && TargetResolveResult is TypeResolveResult)
                        {
                            isSimple = true;
                            WriteParamName(Method.Parameters.First().Name);
                        }
                        else if (argsInfo.Expression is MemberReferenceExpression)
                        {
                            var trg = ((MemberReferenceExpression)argsInfo.Expression).Target;

                            if (trg is BaseReferenceExpression)
                            {
                                isSimple = true;
                                Write("this");
                            }
                            else
                            {
                                isSimple = IsSimpleExpression(trg);
                                trg.AcceptVisitor(Emitter);
                            }
                        }
                        else
                        {
                            Write("this");
                        }
                    }
                    else if (Method.IsExtensionMethod && key == Method.Parameters.First().Name)
                    {
                        if (TargetResolveResult is TypeResolveResult)
                        {
                            isSimple = true;
                            WriteParamName(key);
                        }
                        else if (argsInfo.Expression is MemberReferenceExpression)
                        {
                            var trg = ((MemberReferenceExpression)argsInfo.Expression).Target;

                            if (trg is BaseReferenceExpression)
                            {
                                isSimple = true;
                                Write("this");
                            }
                            else
                            {
                                isSimple = IsSimpleExpression(trg);
                                trg.AcceptVisitor(Emitter);
                            }
                        }
                        else
                        {
                            isSimple = true;
                            WriteParamName(key);
                        }
                    }
                    else if (paramsName == key && !ignoreArray)
                    {
                        isSimple = true;
                        Write(JS.Types.ARRAY + "." + JS.Fields.PROTOTYPE + "." + JS.Funcs.SLICE);
                        WriteCall("(" + JS.Vars.ARGUMENTS + ", " + paramsIndex + ")");
                    }
                    else
                    {
                        isSimple = true;
                        WriteParamName(key);
                    }

                    if (modifier == "type")
                    {
                        Write(")");
                    }
                }
                else if (key == "this" || key == argsInfo.ThisName || (key == "0" && argsInfo.IsExtensionMethod))
                {
                    if (modifier == CS.Methods.GETHASHCODE || modifier == CS.Methods.TOSTRING)
                    {
                        AstNode node = null;
                        if (argsInfo.ThisArgument is AstNode)
                        {
                            node = (AstNode)argsInfo.ThisArgument;
                        }
                        else
                        {
                            node = argsInfo.Expression;
                        }

                        if (node != null)
                        {
                            var rr = Emitter.Resolver.ResolveNode(node, Emitter);
                            var type = rr.Type;
                            if (rr is MemberResolveResult mrr && mrr.Member.ReturnType.Kind != TypeKind.Enum && mrr.TargetResult != null)
                            {
                                type = mrr.TargetResult.Type;
                            }

                            var inlineMethod = ConversionBlock.GetInlineMethod(Emitter, modifier,
                                           modifier == CS.Methods.TOSTRING ? Emitter.Resolver.Compilation.FindType(KnownTypeCode.String) :
                                            Emitter.Resolver.Compilation.FindType(KnownTypeCode.Int32), type, argsInfo.Expression);
                            Write(inlineMethod);
                        }
                    }
                    else if (modifier == "type")
                    {
                        AstNode node = null;
                        if (argsInfo.ThisArgument is AstNode)
                        {
                            node = (AstNode)argsInfo.ThisArgument;
                        }
                        else
                        {
                            node = argsInfo.Expression;
                        }

                        IType type = null;
                        if (node != null)
                        {
                            var rr = Emitter.Resolver.ResolveNode(node, Emitter);
                            type = rr.Type;
                            if (rr is MemberResolveResult mrr && mrr.Member.ReturnType.Kind != TypeKind.Enum && mrr.TargetResult != null)
                            {
                                type = mrr.TargetResult.Type;
                            }
                        }
                        else
                        {
                            type = ArgumentsInfo.ThisType;
                        }

                        if (type != null)
                        {
                            bool needName = NeedName(type);
                            var argExpr = argsInfo.ArgumentsExpressions != null && argsInfo.ArgumentsExpressions.Length > 0 ? argsInfo.ArgumentsExpressions[0] : null;
                            if (argExpr == null)
                            {
                                if (argsInfo.Expression is InvocationExpression expr && expr.Target is MemberReferenceExpression)
                                {
                                    argExpr = ((MemberReferenceExpression)expr.Target).Target;
                                }
                            }

                            string thisValue = argsInfo.GetThisValue();
                            bool skipType = false;

                            var typeDef = Emitter.H5Types.Get(type, true)?.TypeDefinition;
                            if (typeDef == null || !typeDef.IsValueType || NullableType.IsNullable(type) && NullableType.GetUnderlyingType(type).Kind == TypeKind.Struct)
                            {
                                if (argExpr != null)
                                {
                                    var writer = SaveWriter();
                                    NewWriter();
                                    argExpr.AcceptVisitor(Emitter);
                                    thisValue = Emitter.Output.ToString();
                                    RestoreWriter(writer);
                                }

                                if (thisValue != null)
                                {
                                    if (type.Kind == TypeKind.TypeParameter && !Helpers.IsIgnoreGeneric(((ITypeParameter)type).Owner, Emitter))
                                    {
                                        thisValue = thisValue + ", " + type.Name;
                                        skipType = true;
                                    }

                                }

                                var s = JS.Funcs.H5_GET_TYPE + "(" + thisValue;

                                if (needName && !skipType)
                                {
                                    s += ", ";
                                    isSimple = true;
                                    s += GetTypeName(type);
                                }

                                s += ")";
                                Write(s);
                            }
                            else
                            {
                                isSimple = true;
                                Write(GetTypeName(type));
                            }
                        }
                    }
                    else
                    {
                        string thisValue = argsInfo.GetThisValue();

                        if (thisValue != null)
                        {
                            isSimple = true;
                            Write(thisValue);
                        }
                    }
                }
                else
                {
                    IList<Expression> exprs = GetExpressionsByKey(expressions, key);

                    if (exprs.Count > 0)
                    {
                        if (modifier == CS.Methods.GETHASHCODE || modifier == CS.Methods.TOSTRING)
                        {
                            IType type = null;
                            if (paramsName == key && paramsType != null)
                            {
                                type = paramsType;
                            }
                            else
                            {
                                var rr = Emitter.Resolver.ResolveNode(exprs[0], Emitter);
                                type = rr.Type;
                            }

                            var inlineMethod = ConversionBlock.GetInlineMethod(Emitter, modifier,
                                               modifier == CS.Methods.TOSTRING ? Emitter.Resolver.Compilation.FindType(KnownTypeCode.String) :
                                                Emitter.Resolver.Compilation.FindType(KnownTypeCode.Int32), type, exprs[0]);
                            Write(inlineMethod);
                        }
                        else if (modifier == "type")
                        {
                            IType type = null;
                            if (paramsName == key && paramsType != null)
                            {
                                type = paramsType;
                            }
                            else
                            {
                                var rr = Emitter.Resolver.ResolveNode(exprs[0], Emitter);
                                type = rr.Type;
                            }

                            bool needName = NeedName(type);
                            WriteGetType(needName, type, exprs[0], modifier);
                            isSimple = true;
                        }
                        else if (modifier == "tmp")
                        {
                            string tmpVarName = null;

                            if (!(exprs[0] is PrimitiveExpression nameExpr))
                            {
                                throw new EmitterException(exprs[0], "Primitive expression is required");
                            }

                            var keyExpr = string.IsNullOrWhiteSpace(nameExpr.LiteralValue) ? nameExpr.Value.ToString() : nameExpr.LiteralValue;

                            if (!Emitter.NamedTempVariables.ContainsKey(keyExpr))
                            {
                                tmpVarName = GetTempVarName();
                                Emitter.NamedTempVariables[keyExpr] = tmpVarName;
                            } else
                            {
                                tmpVarName = Emitter.NamedTempVariables[keyExpr];
                            }

                            Write(tmpVarName);
                            isSimple = true;
                        }
                        else if (modifier == "version")
                        {
                            var versionTypeExp = exprs != null && exprs.Any() ? exprs[0] : null;

                            var versionType = 0;
                            if (versionTypeExp != null)
                            {
                                if (versionTypeExp is PrimitiveExpression versionTypePrimitiveExp && versionTypePrimitiveExp.Value is int)
                                {
                                    versionType = (int)versionTypePrimitiveExp.Value;
                                }
                                else
                                {
                                    var rr = Emitter.Resolver.ResolveNode(versionTypeExp, Emitter);

                                    if (rr != null && rr.ConstantValue != null && rr.ConstantValue is int)
                                    {
                                        versionType = (int)rr.ConstantValue;
                                    }
                                }
                            }

                            string version;

                            if (versionType == 0)
                            {
                                version = Emitter.Translator.GetVersionContext().Assembly.Version;
                            }
                            else
                            {
                                version = Emitter.Translator.GetVersionContext().Compiler.Version;
                            }

                            Write("\"", version, "\"");

                            isSimple = true;
                        }
                        else if (modifier == "gettmp")
                        {
                            if (!(exprs[0] is PrimitiveExpression nameExpr))
                            {
                                throw new EmitterException(exprs[0], "Primitive expression is required");
                            }

                            var keyExpr = string.IsNullOrWhiteSpace(nameExpr.LiteralValue) ? nameExpr.Value.ToString() : nameExpr.LiteralValue;

                            if (!Emitter.NamedTempVariables.ContainsKey(keyExpr))
                            {
                                Emitter.NamedTempVariables[keyExpr] = GetTempVarName();
                            }

                            var tmpVarName = Emitter.NamedTempVariables[keyExpr];
                            Write(tmpVarName);
                            isSimple = true;
                        }
                        else if (modifier == "body")
                        {
                            if (!(exprs[0] is LambdaExpression lambdaExpr))
                            {
                                throw new EmitterException(exprs[0], "Lambda expression is required");
                            }

                            var writer = SaveWriter();
                            NewWriter();

                            lambdaExpr.Body.AcceptVisitor(Emitter);

                            var s = Emitter.Output.ToString();
                            RestoreWriter(writer);
                            Write(WriteIndentToString(s));
                        }
                        else if (exprs.Count > 1 || paramsName == key)
                        {
                            if (needExpand)
                            {
                                ignoreArray = true;
                            }

                            if (!ignoreArray)
                            {
                                Write("[");
                            }

                            if (exprs.Count == 1 && exprs[0] == null)
                            {
                                isSimple = true;
                                Write("null");
                            }
                            else
                            {
                                new ExpressionListBlock(Emitter, exprs, null, null, 0).Emit();
                            }

                            if (!ignoreArray)
                            {
                                Write("]");
                            }
                        }
                        else
                        {
                            string s;
                            if (exprs[0] != null)
                            {
                                var writer = SaveWriter();
                                NewWriter();

                                if (exprs[0] is DirectionExpression directExpr)
                                {
                                    if (Emitter.Resolver.ResolveNode(exprs[0], Emitter) is ByReferenceResolveResult rr && !(rr.ElementResult is LocalResolveResult))
                                    {
                                        Write(JS.Funcs.H5_REF + "(");

                                        Emitter.IsRefArg = true;
                                        exprs[0].AcceptVisitor(Emitter);
                                        Emitter.IsRefArg = false;

                                        if (Emitter.Writers.Count != count)
                                        {
                                            PopWriter();
                                            count = Emitter.Writers.Count;
                                        }

                                        Write(")");
                                    }
                                    else
                                    {
                                        exprs[0].AcceptVisitor(Emitter);
                                    }
                                }
                                else if (modifier == "plain")
                                {
                                    if (!(exprs[0] is AnonymousTypeCreateExpression an))
                                    {
                                        Write(JS.Funcs.H5_TOPLAIN);
                                        WriteOpenParentheses();
                                        exprs[0].AcceptVisitor(Emitter);
                                        Write(")");
                                    }
                                    else
                                    {
                                        new AnonymousTypeCreateBlock(Emitter, an, true).Emit();
                                    }
                                }
                                else
                                {
                                    isSimple = IsSimpleExpression(exprs[0]);
                                    exprs[0].AcceptVisitor(Emitter);
                                }

                                s = Emitter.Output.ToString();
                                RestoreWriter(writer);

                                if (modifier == "raw")
                                {
                                    s = RemoveTokens(s).Trim('"');
                                }
                            }
                            else
                            {
                                isSimple = true;
                                s = "null";
                            }

                            Write(WriteIndentToString(s));
                        }
                    }
                    else if (ArgumentsInfo.Attribute != null)
                    {
                        var results = GetResolveResultByKey(key);

                        if (results.Count > 1 || paramsName == key)
                        {
                            if (needExpand)
                            {
                                ignoreArray = true;
                            }

                            if (!ignoreArray)
                            {
                                Write("[");
                            }

                            if (exprs.Count == 1 && results[0].IsCompileTimeConstant && results[0].ConstantValue == null)
                            {
                                isSimple = true;
                                Write("null");
                            }
                            else
                            {
                                bool needComma = false;
                                foreach (ResolveResult item in results)
                                {
                                    if (needComma)
                                    {
                                        WriteComma();
                                    }

                                    needComma = true;

                                    isSimple = IsSimpleResolveResult(item);
                                    AttributeCreateBlock.WriteResolveResult(item, this);
                                }
                            }

                            if (!ignoreArray)
                            {
                                Write("]");
                            }
                        }
                        else
                        {
                            string s;
                            if (results[0] != null)
                            {
                                var writer = SaveWriter();
                                NewWriter();

                                isSimple = IsSimpleResolveResult(results[0]);
                                AttributeCreateBlock.WriteResolveResult(results[0], this);

                                s = Emitter.Output.ToString();
                                RestoreWriter(writer);

                                if (modifier == "raw")
                                {
                                    s = RemoveTokens(s).Trim('"');
                                }
                            }
                            else
                            {
                                s = "null";
                            }

                            Write(WriteIndentToString(s));
                        }
                    }
                    else if (ArgumentsInfo.StringArguments != null)
                    {
                        var results = GetStringArgumentByKey(key);

                        if (results.Count > 1 || paramsName == key)
                        {
                            if (needExpand)
                            {
                                ignoreArray = true;
                            }

                            if (!ignoreArray)
                            {
                                Write("[");
                            }

                            bool needComma = false;
                            foreach (string item in results)
                            {
                                if (needComma)
                                {
                                    WriteComma();
                                }

                                needComma = true;

                                Write(item);
                            }

                            if (!ignoreArray)
                            {
                                Write("]");
                            }
                        }
                        else
                        {
                            string s;
                            if (results[0] != null)
                            {
                                s = results[0];

                                if (modifier == "raw")
                                {
                                    s = RemoveTokens(s).Trim('"');
                                }
                            }
                            else
                            {
                                s = "null";
                            }

                            Write(s);
                        }
                    }
                    else if (typeParams != null)
                    {
                        if (modifier == CS.Methods.GETHASHCODE || modifier == CS.Methods.TOSTRING)
                        {
                            var iType = GetTypeByKey(typeParams, key);

                            if (iType != null)
                            {
                                var inlineMethod = ConversionBlock.GetInlineMethod(Emitter, modifier,
                                    modifier == CS.Methods.TOSTRING
                                        ? Emitter.Resolver.Compilation.FindType(KnownTypeCode.String)
                                        : Emitter.Resolver.Compilation.FindType(KnownTypeCode.Int32), iType.IType,
                                    null);
                                Write(inlineMethod ?? "null");
                            }
                        }
                        else
                        {
                            var type = GetAstTypeByKey(typeParams, key);

                            if (type != null)
                            {
                                if (modifier == "default" || modifier == "defaultFn")
                                {
                                    var def = Inspector.GetDefaultFieldValue(type, Emitter.Resolver);
                                    GetDefaultValue(def, modifier);
                                }
                                else
                                {
                                    type.AcceptVisitor(Emitter);
                                }
                            }
                            else
                            {
                                var iType = GetTypeByKey(typeParams, key);

                                if (iType != null)
                                {
                                    if (modifier == "default" || modifier == "defaultFn")
                                    {
                                        var def = Inspector.GetDefaultFieldValue(iType.IType, iType.AstType);
                                        GetDefaultValue(def, modifier);
                                    }
                                    else
                                    {
                                        new CastBlock(Emitter, iType.IType).Emit();
                                    }
                                }
                            }
                        }
                    }
                }

                if (Emitter.Writers.Count != count)
                {
                    PopWriter();
                }

                string replacement = Emitter.Output.ToString();
                Emitter.Output = oldSb;
                Emitter.TemplateModifier = null;

                if (!isSimple && keyMatches.Count(keyMatch =>
                {
                    string key1 = keyMatch.Groups[2].Value;
                    string modifier1 = keyMatch.Groups[1].Success ? keyMatch.Groups[4].Value : null;
                    return key == key1 && modifier1 == modifier;
                }) > 1)
                {
                    var t = GetTempVarName();
                    tempVars.Add(t, replacement);
                    tempMap[tempKey] = t;
                    return t;
                }

                return replacement;
            });

            if (tempVars.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("(");

                foreach (var tempVar in tempVars)
                {
                    sb.Append(tempVar.Key);
                    sb.Append(" = ");
                    sb.Append(tempVar.Value);
                    sb.Append(", ");
                }

                sb.Append(inline);
                sb.Append(")");

                inline = sb.ToString();
            }
            Write(inline);

            if (asRef)
            {
                Write("; }");
                if (addClose)
                {
                    Write(")");
                }
            }
        }

        private string GetTypeName(IType type)
        {
            var enumType = NullableType.GetUnderlyingType(type);
            if (type.Kind == TypeKind.Enum && Emitter.Validator.IsExternalType(type.GetDefinition()))
            {
                var enumMode = Helpers.EnumEmitMode(type);
                if (enumMode >= 3 && enumMode < 7)
                {
                    enumType = Emitter.Resolver.Compilation.FindType(KnownTypeCode.String);
                }
                else if (enumMode == 2)
                {
                    enumType = type.GetDefinition().EnumUnderlyingType;
                }
            }

            return H5Types.ToJsName(enumType, Emitter);
        }

        public void AddModuleByType(List<string> amd, List<string> cjs, Module module)
        {
            if (module != null)
            {
                if (!(module.Type == ModuleType.UMD &&
                     Emitter.AssemblyInfo.Loader.Type == ModuleLoaderType.Global))
                {
                    if (module.Type == ModuleType.AMD
                        || (module.Type == ModuleType.UMD && Emitter.AssemblyInfo.Loader.Type == ModuleLoaderType.AMD))
                    {
                        amd.Add(module.Name);
                    }
                    else
                    {
                        cjs.Add(module.Name);
                    }
                }
            }
        }

        private void WriteGetType(bool needName, IType type, AstNode node, string modifier)
        {
            string s;
            var typeDef = Emitter.H5Types.Get(type, true)?.TypeDefinition;
            if (node != null && (typeDef == null || !typeDef.IsValueType || NullableType.IsNullable(type) && NullableType.GetUnderlyingType(type).Kind == TypeKind.Struct))
            {
                var writer = SaveWriter();
                NewWriter();
                node.AcceptVisitor(Emitter);
                s = Emitter.Output.ToString();
                RestoreWriter(writer);

                if (modifier == "raw")
                {
                    s = RemoveTokens(s).Trim('"');
                }
            }
            else
            {
                Write(GetTypeName(type));
                return;
            }

            bool skipType = false;
            if (type.Kind == TypeKind.TypeParameter && !Helpers.IsIgnoreGeneric(((ITypeParameter)type).Owner, Emitter))
            {
                s = s + ", " + type.Name;
                skipType = true;
            }

            s = JS.Funcs.H5_GET_TYPE + "(" + s;

            if (needName && !skipType)
            {
                s += ", ";
                s += GetTypeName(type);
            }

            s = s + ")";

            Write(WriteIndentToString(s));
        }

        private bool NeedName(IType type)
        {
            var def = type.GetDefinition();
            return (def != null && def.IsSealed && !Helpers.IsKnownType(KnownTypeCode.Array, type, Emitter.Resolver))
                   || (type.Kind == TypeKind.Enum || Helpers.IsKnownType(KnownTypeCode.Enum, type, Emitter.Resolver)) && type.FullName != "System.Enum"
                   || Helpers.IsIntegerType(type, Emitter.Resolver)
                   || Helpers.IsFloatType(type, Emitter.Resolver)
                   || Helpers.IsKnownType(KnownTypeCode.Boolean, type, Emitter.Resolver)
                   || Helpers.IsKnownType(KnownTypeCode.Type, type, Emitter.Resolver)
                   || Helpers.IsKnownType(KnownTypeCode.Char, type, Emitter.Resolver)
                   || Helpers.IsKnownType(KnownTypeCode.DateTime, type, Emitter.Resolver)
                   || Helpers.IsKnownType(KnownTypeCode.Delegate, type, Emitter.Resolver)
                   || Helpers.IsKnownType(KnownTypeCode.String, type, Emitter.Resolver);
        }

        private void GetDefaultValue(object def, string modifier)
        {
            if (def is AstType)
            {
                if (modifier == "defaultFn")
                {
                    WriteFunction();
                    WriteOpenCloseParentheses(true);
                    BeginBlock();
                    WriteReturn(true);

                    Write(JS.Funcs.H5_GETDEFAULTVALUE + "(");
                    Write(H5Types.ToJsName((AstType)def, Emitter));
                    Write(")");

                    WriteSemiColon();
                    WriteNewLine();
                    EndBlock();
                }
                else
                {
                    Write(Inspector.GetStructDefaultValue((AstType)def, Emitter));
                }
            }
            else if (def is IType)
            {
                if (modifier == "defaultFn")
                {
                    WriteFunction();
                    WriteOpenCloseParentheses(true);
                    BeginBlock();
                    WriteReturn(true);

                    Write(JS.Funcs.H5_GETDEFAULTVALUE + "(");

                    Write(H5Types.ToJsName((IType)def, Emitter));

                    Write(")");

                    WriteSemiColon();
                    WriteNewLine();
                    EndBlock();
                }
                else
                {
                    Write(Inspector.GetStructDefaultValue((IType)def, Emitter));
                }
            }
            else if (def is RawValue)
            {
                WriteFunction();
                WriteOpenCloseParentheses(true);
                BeginBlock();
                WriteReturn(true);
                Write(def.ToString());

                WriteSemiColon();
                WriteNewLine();
                EndBlock();
            }
            else
            {
                WriteScript(def);
            }
        }

        protected virtual void EmitMethodParameters(IMethod method, IEnumerable<IParameter> parameters, IEnumerable<ITypeParameter> typeParameters, bool isNull)
        {
            bool needComma = false;

            if (typeParameters != null && typeParameters.Any())
            {
                foreach (var tp in typeParameters)
                {
                    Emitter.Validator.CheckIdentifier(tp.Name, ArgumentsInfo.Expression);

                    if (needComma)
                    {
                        WriteComma();
                    }

                    needComma = true;
                    Write(tp.Name);
                }
            }

            if (isNull)
            {
                Write(JS.Vars.T);
                needComma = true;
            }
            else if (Method.IsExtensionMethod && !(TargetResolveResult is TypeResolveResult) && TargetResolveResult != null)
            {
                parameters = parameters.Skip(1);
            }

            foreach (var p in parameters)
            {
                var name = p.Name;

                if (Helpers.IsReservedWord(Emitter, name))
                {
                    name = Helpers.ChangeReservedWord(name);
                }

                if (Emitter.LocalsNamesMap != null && Emitter.LocalsNamesMap.ContainsKey(name))
                {
                    name = Emitter.LocalsNamesMap[name];
                }

                if (needComma)
                {
                    WriteComma();
                }

                needComma = true;
                Write(name);
            }
        }

        public virtual void EmitFunctionReference(bool? definition = false)
        {
            EmitInlineExpressionList(ArgumentsInfo, InlineCode, true, false, definition);
        }

        public virtual void EmitNullableReference()
        {
            EmitInlineExpressionList(ArgumentsInfo, InlineCode, true, true);
        }

        public bool IsSimpleExpression(Expression expression)
        {
            if (expression is PrimitiveExpression || expression is ThisReferenceExpression)
            {
                return true;
            }

            var rr = Emitter.Resolver.ResolveNode(expression, Emitter);
            return IsSimpleResolveResult(rr);
        }

        private bool IsSimpleResolveResult(ResolveResult rr)
        {
            bool isField = rr is MemberResolveResult memberTargetrr && memberTargetrr.Member is IField &&
                           (memberTargetrr.TargetResult is ThisResolveResult ||
                            memberTargetrr.TargetResult is LocalResolveResult);

            return rr is ThisResolveResult || rr is ConstantResolveResult || rr is LocalResolveResult || isField;
        }
    }
}