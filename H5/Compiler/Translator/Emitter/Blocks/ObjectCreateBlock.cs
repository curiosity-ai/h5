using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace H5.Translator
{
    public class ObjectCreateBlock : ConversionBlock
    {
        public ObjectCreateBlock(IEmitter emitter, ObjectCreateExpression objectCreateExpression)
            : base(emitter, objectCreateExpression)
        {
            Emitter = emitter;
            ObjectCreateExpression = objectCreateExpression;
        }

        public ObjectCreateExpression ObjectCreateExpression { get; set; }

        protected override Expression GetExpression()
        {
            return ObjectCreateExpression;
        }

        protected override void EmitConversionExpression()
        {
            VisitObjectCreateExpression();
        }

        protected void VisitObjectCreateExpression()
        {
            ObjectCreateExpression objectCreateExpression = ObjectCreateExpression;

            var resolveResult = Emitter.Resolver.ResolveNode(objectCreateExpression.Type) as TypeResolveResult;

            if (resolveResult != null && resolveResult.Type.Kind == TypeKind.Enum)
            {
                Write("(0)");
                return;
            }

            bool isTypeParam = resolveResult != null && resolveResult.Type.Kind == TypeKind.TypeParameter;
            var invocationResolveResult = Emitter.Resolver.ResolveNode(objectCreateExpression) as InvocationResolveResult;
            var hasInitializer = !objectCreateExpression.Initializer.IsNull && objectCreateExpression.Initializer.Elements.Count > 0;

            if (isTypeParam && invocationResolveResult != null && invocationResolveResult.Member.Parameters.Count == 0 && !hasInitializer)
            {
                Write(JS.Funcs.H5_CREATEINSTANCE);
                WriteOpenParentheses();
                Write(resolveResult.Type.Name);
                WriteCloseParentheses();

                return;
            }

            var type = isTypeParam ? null : Emitter.GetTypeDefinition(objectCreateExpression.Type);
            var isObjectLiteral = type != null && Emitter.Validator.IsObjectLiteral(type);

            if (type != null && type.BaseType != null && type.BaseType.FullName == "System.MulticastDelegate")
            {
                bool wrap = false;
                if (objectCreateExpression.Parent is InvocationExpression parent && parent.Target == objectCreateExpression)
                {
                    wrap = true;
                }

                if (wrap)
                {
                    WriteOpenParentheses();
                }

                Write("H5.fn.$build([");
                objectCreateExpression.Arguments.First().AcceptVisitor(Emitter);
                Write("])");

                if (wrap)
                {
                    WriteCloseParentheses();
                }
                return;
            }

            var argsInfo = new ArgumentsInfo(Emitter, objectCreateExpression);
            var argsExpressions = argsInfo.ArgumentsExpressions;
            var paramsArg = argsInfo.ParamsExpression;

            string inlineCode = null;

            if (invocationResolveResult != null)
            {
                if (invocationResolveResult.Member.DeclaringType.Kind == TypeKind.Struct && objectCreateExpression.Arguments.Count == 0)
                {
                    var ctors = invocationResolveResult.Member.DeclaringType.GetConstructors(c => c.Parameters.Count == 1);
                    var defCtor = ctors.FirstOrDefault(c => c.Parameters.First().Type.FullName == "System.Runtime.CompilerServices.DummyTypeUsedToAddAttributeToDefaultValueTypeConstructor");

                    if (defCtor != null)
                    {
                        inlineCode = Emitter.GetInline(defCtor);
                    }
                }

                if (inlineCode == null)
                {
                    inlineCode = Emitter.GetInline(invocationResolveResult.Member);
                }
            }

            var customCtor = isTypeParam ? "" : (Emitter.Validator.GetCustomConstructor(type) ?? "");

            AstNodeCollection<Expression> elements = null;

            if (hasInitializer)
            {
                elements = objectCreateExpression.Initializer.Elements;
            }

            var isPlainObjectCtor = Regex.Match(customCtor, @"\s*\{\s*\}\s*").Success;
            var isPlainMode = type != null && Emitter.Validator.GetObjectCreateMode(type) == 0;

            if (inlineCode == null && isPlainObjectCtor && isPlainMode)
            {
                WriteOpenBrace();
                WriteSpace();
                var pos = Emitter.Output.Length;

                WriteObjectInitializer(objectCreateExpression.Initializer.Elements, type, invocationResolveResult, false);

                if (pos < Emitter.Output.Length)
                {
                    WriteSpace();
                }

                WriteCloseBrace();
            }
            else
            {
                string tempVar = null;
                if (hasInitializer)
                {
                    tempVar = GetTempVarName();
                    WriteOpenParentheses();
                    Write(tempVar);
                    Write(" = ");
                }

                if (inlineCode != null)
                {
                    new InlineArgumentsBlock(Emitter, argsInfo, inlineCode).Emit();
                }
                else
                {
                    var ctorMember = ((InvocationResolveResult)Emitter.Resolver.ResolveNode(objectCreateExpression)).Member;
                    var expandParams = ctorMember.Attributes.Any(a => a.AttributeType.FullName == "H5.ExpandParamsAttribute");
                    bool applyCtor = false;

                    if (expandParams)
                    {
                        var ctor_rr = Emitter.Resolver.ResolveNode(paramsArg);

                        if (ctor_rr.Type.Kind == TypeKind.Array && !(paramsArg is ArrayCreateExpression) && objectCreateExpression.Arguments.Last() == paramsArg)
                        {
                            Write(JS.Types.H5.Reflection.APPLYCONSTRUCTOR + "(");
                            applyCtor = true;
                        }
                    }

                    if (String.IsNullOrEmpty(customCtor) || (isObjectLiteral && isPlainObjectCtor))
                    {
                        if (!applyCtor && !isObjectLiteral)
                        {
                            WriteNew();
                        }

                        var typerr = Emitter.Resolver.ResolveNode(objectCreateExpression.Type).Type;
                        var td = typerr.GetDefinition();
                        var isGeneric = typerr.TypeArguments.Count > 0 && !Helpers.IsIgnoreGeneric(typerr, Emitter) || td != null && Validator.IsVirtualTypeStatic(td);

                        if (isGeneric && !applyCtor)
                        {
                            WriteOpenParentheses();
                        }

                        objectCreateExpression.Type.AcceptVisitor(Emitter);

                        if (isGeneric && !applyCtor)
                        {
                            WriteCloseParentheses();
                        }
                    }
                    else
                    {
                        Write(customCtor);
                    }

                    if (!isTypeParam && type.Methods.Count(m => m.IsConstructor && !m.IsStatic) > (type.IsValueType || isObjectLiteral ? 0 : 1))
                    {
                        var member = ((InvocationResolveResult)Emitter.Resolver.ResolveNode(objectCreateExpression)).Member;
                        if (!Emitter.Validator.IsExternalType(type) || member.Attributes.Any(a => a.AttributeType.FullName == "H5.NameAttribute"))
                        {
                            WriteDot();
                            var name = OverloadsCollection.Create(Emitter, member).GetOverloadName();
                            Write(name);
                        }
                    }

                    if (applyCtor)
                    {
                        Write(", ");
                    }
                    else
                    {
                        WriteOpenParentheses();
                    }

                    new ExpressionListBlock(Emitter, argsExpressions, paramsArg, objectCreateExpression, -1).Emit();
                    WriteCloseParentheses();
                }

                if (hasInitializer)
                {
                    if (isObjectLiteral && isPlainMode)
                    {
                        WriteObjectInitializer(objectCreateExpression.Initializer.Elements, type, invocationResolveResult, true);
                    }
                    else
                    {
                        foreach (Expression item in elements)
                        {
                            WriteInitializerExpression(item, tempVar);
                        }
                    }

                    WriteComma();
                    Write(tempVar);
                    WriteCloseParentheses();
                    RemoveTempVar(tempVar);
                }
            }

            //Helpers.CheckValueTypeClone(invocationResolveResult, this.ObjectCreateExpression, this, pos);
        }

        private void WriteInitializerExpression(Expression item, string tempVar)
        {
            var rr = Emitter.Resolver.ResolveNode(item) as MemberResolveResult;

            var inlineCode = ObjectCreateBlock.GetInlineInit(item, this, tempVar);

            if (inlineCode != null)
            {
                WriteComma();
                Write(inlineCode);
            }
            else if (item is NamedExpression)
            {
                WriteNamedExptession(((NamedExpression)item).Expression, tempVar, rr);
            }
            else if (item is NamedArgumentExpression)
            {
                WriteNamedExptession(((NamedArgumentExpression)item).Expression, tempVar, rr);
            }
            else if (item is ArrayInitializerExpression arrayInitializer)
            {
                foreach (var el in arrayInitializer.Elements)
                {
                    WriteInitializerExpression(el, tempVar + "." + Emitter.GetEntityName(rr.Member));
                }
            }
            else if (item is IdentifierExpression)
            {
                WriteComma();
                var identifierExpression = (IdentifierExpression)item;
                new IdentifierBlock(Emitter, identifierExpression).Emit();
            }
            else
            {
                WriteComma();
                item.AcceptVisitor(Emitter);
            }
        }

        private void WriteNamedExptession(Expression expression, string tempVar, MemberResolveResult rr)
        {
            if (expression is ArrayInitializerExpression arrayInitializer)
            {
                foreach (var el in arrayInitializer.Elements)
                {
                    WriteInitializerExpression(el, tempVar + "." + OverloadsCollection.Create(Emitter, rr.Member).GetOverloadName());
                }
            }
            else
            {
                WriteComma();
                Write(tempVar);
                WriteDot();
                WriteIdentifier(OverloadsCollection.Create(Emitter, rr.Member).GetOverloadName());
                Write(" = ");
                expression.AcceptVisitor(Emitter);
            }
        }

        public static string GetInlineInit(Expression item, AbstractEmitterBlock block, string thisScope)
        {
            Expression expr = null;
            if (item is NamedExpression namedExpression)
            {
                expr = namedExpression.Expression;
            }
            else if (item is NamedArgumentExpression namedArgumentExpression)
            {
                expr = namedArgumentExpression.Expression;
            }

            var rr = block.Emitter.Resolver.ResolveNode(item);
            string inlineCode = null;
            if (expr != null && rr is MemberResolveResult)
            {
                var member = ((MemberResolveResult)rr).Member;

                if (member is IProperty)
                {
                    var setter = ((IProperty)member).Setter;
                    if (setter != null)
                    {
                        inlineCode = block.Emitter.GetInline(setter);
                    }
                }
                else
                {
                    inlineCode = block.Emitter.GetInline(member);
                }

                if (inlineCode != null)
                {
                    bool oldIsAssignment = block.Emitter.IsAssignment;
                    bool oldUnary = block.Emitter.IsUnaryAccessor;
                    var oldWriter = block.SaveWriter();
                    block.NewWriter();
                    block.Emitter.IsAssignment = true;
                    block.Emitter.IsUnaryAccessor = false;

                    inlineCode = Helpers.ConvertTokens(block.Emitter, inlineCode, member);
                    bool hasThis = inlineCode.Contains("{this}");
                    if (inlineCode.StartsWith("<self>"))
                    {
                        hasThis = true;
                        inlineCode = inlineCode.Substring(6);
                    }

                    if (hasThis)
                    {
                        inlineCode = inlineCode.Replace("{this}", thisScope);

                        if (member is IProperty)
                        {
                            var argsInfo = new ArgumentsInfo(block.Emitter, item, rr);
                            argsInfo.ArgumentsExpressions = new Expression[] { expr };
                            argsInfo.ArgumentsNames = new string[] { "value" };
                            argsInfo.ThisArgument = thisScope;
                            argsInfo.NamedExpressions = argsInfo.CreateNamedExpressions(argsInfo.ArgumentsNames, argsInfo.ArgumentsExpressions);

                            inlineCode = inlineCode.Replace("{0}", "[[0]]");
                            new InlineArgumentsBlock(block.Emitter, argsInfo, inlineCode).Emit();
                            inlineCode = block.Emitter.Output.ToString();
                            inlineCode = inlineCode.Replace("[[0]]", "{0}");
                        }
                    }
                    else
                    {
                        if (member.SymbolKind == SymbolKind.Property)
                        {
                            var count = block.Emitter.Writers.Count;
                            block.PushWriter(thisScope + "." + inlineCode);

                            expr.AcceptVisitor(block.Emitter);

                            if (block.Emitter.Writers.Count > count)
                            {
                                inlineCode = block.PopWriter(true);
                            }
                        }
                        else
                        {
                            block.Write(thisScope + "." + inlineCode);
                        }
                    }

                    block.Emitter.IsAssignment = oldIsAssignment;
                    block.Emitter.IsUnaryAccessor = oldUnary;
                    block.RestoreWriter(oldWriter);
                }
            }

            if (inlineCode != null && inlineCode.Trim().EndsWith(";"))
            {
                inlineCode = inlineCode.Trim().TrimEnd(';');
            }

            return inlineCode;
        }

        protected virtual void WriteObjectInitializer(IEnumerable<Expression> expressions, TypeDefinition type, InvocationResolveResult rr, bool withCtor)
        {
            bool needComma = false;
            List<string> names = new List<string>();
            var isObjectLiteral = Emitter.Validator.IsObjectLiteral(type);

            if (!withCtor && rr != null && ObjectCreateExpression.Arguments.Count > 0)
            {
                var args = ObjectCreateExpression.Arguments.ToList();
                var arrIsOpen = false;
                for (int i = 0; i < args.Count; i++)
                {
                    Expression expression = args[i];
                    var p = rr.Member.Parameters[i < rr.Member.Parameters.Count ? i : (rr.Member.Parameters.Count - 1)];
                    var name = p.Name;

                    if (p.Type.FullName == "H5.ObjectInitializationMode" ||
                        p.Type.FullName == "H5.ObjectCreateMode")
                    {
                        continue;
                    }

                    if (needComma)
                    {
                        WriteComma();
                    }

                    needComma = true;

                    if (p.IsParams && !arrIsOpen)
                    {
                        arrIsOpen = true;
                        Write("[");
                    }

                    Write(name, ": ");
                    expression.AcceptVisitor(Emitter);

                    names.Add(name);
                }

                if (arrIsOpen)
                {
                    Write("]");
                }
            }

            if (expressions != null)
            {
                foreach (Expression item in expressions)
                {
                    NamedExpression namedExression = item as NamedExpression;
                    NamedArgumentExpression namedArgumentExpression = item as NamedArgumentExpression;
                    string name = namedExression != null ? namedExression.Name : namedArgumentExpression.Name;

                    if (Emitter.Resolver.ResolveNode(item) is MemberResolveResult itemrr)
                    {
                        var oc = OverloadsCollection.Create(Emitter, itemrr.Member);
                        bool forceObjectLiteral = itemrr.Member is IProperty && !itemrr.Member.Attributes.Any(attr => attr.AttributeType.FullName == "H5.NameAttribute") && !Emitter.Validator.IsObjectLiteral(itemrr.Member.DeclaringTypeDefinition);

                        name = oc.GetOverloadName(isObjectLiteral: forceObjectLiteral);
                    }

                    if (needComma)
                    {
                        WriteComma();
                    }

                    needComma = true;

                    Expression expression = namedExression != null ? namedExression.Expression : namedArgumentExpression.Expression;

                    WriteIdentifier(name, true, true);
                    Write(": ");
                    expression.AcceptVisitor(Emitter);

                    names.Add(name);
                }
            }

            if (isObjectLiteral)
            {
                var key = H5Types.GetTypeDefinitionKey(type);
                var tinfo = Emitter.Types.FirstOrDefault(t => t.Key == key);

                var mode = 0;
                if (rr != null)
                {
                    if (rr.Member.Parameters.Count > 0)
                    {
                        var prm = rr.Member.Parameters.FirstOrDefault(p => p.Type.FullName == "H5.ObjectInitializationMode");

                        if (prm != null)
                        {
                            var prmIndex = rr.Member.Parameters.IndexOf(prm);
                            var arg = rr.Arguments.FirstOrDefault(a =>
                            {
                                if (a is NamedArgumentResolveResult)
                                {
                                    return ((NamedArgumentResolveResult)a).ParameterName == prm.Name;
                                }

                                return prmIndex == rr.Arguments.IndexOf(a);
                            });

                            if (arg != null && arg.ConstantValue != null && arg.ConstantValue is int)
                            {
                                mode = (int)arg.ConstantValue;
                            }
                        }
                    }
                    else if (type != null)
                    {
                        mode = Emitter.Validator.GetObjectInitializationMode(type);
                    }
                }

                if (tinfo == null)
                {
                    if (mode == 2)
                    {
                        var properties = rr.Member.DeclaringTypeDefinition.GetProperties(null, GetMemberOptions.IgnoreInheritedMembers);
                        foreach (var prop in properties)
                        {
                            var name = OverloadsCollection.Create(Emitter, prop).GetOverloadName();

                            if (names.Contains(name))
                            {
                                continue;
                            }

                            if (needComma)
                            {
                                WriteComma();
                            }

                            needComma = true;

                            WriteIdentifier(name, true, true);
                            Write(": ");

                            var argType = prop.ReturnType;
                            var defValue = Inspector.GetDefaultFieldValue(argType, null);
                            if (defValue == argType)
                            {
                                Write(Inspector.GetStructDefaultValue(argType, Emitter));
                            }
                            else
                            {
                                Write(defValue);
                            }
                        }
                    }

                    return;
                }

                if (mode != 0)
                {
                    var members = tinfo.InstanceConfig.Fields.Concat(tinfo.InstanceConfig.Properties);

                    if (members.Any())
                    {
                        foreach (var member in members)
                        {
                            if (mode == 1 && (member.VarInitializer == null || member.VarInitializer.Initializer.IsNull) && !member.IsPropertyInitializer)
                            {
                                continue;
                            }

                            var name = member.GetName(Emitter);

                            if (names.Contains(name))
                            {
                                continue;
                            }

                            if (needComma)
                            {
                                WriteComma();
                            }

                            needComma = true;

                            WriteIdentifier(name, true, true);
                            Write(": ");

                            if (mode == 2 && (member.Initializer == null || member.Initializer.IsNull) && !(member.VarInitializer == null || member.VarInitializer.Initializer.IsNull))
                            {
                                var argType = Emitter.Resolver.ResolveNode(member.VarInitializer).Type;
                                var defValue = Inspector.GetDefaultFieldValue(argType, null);
                                if (defValue == argType)
                                {
                                    Write(Inspector.GetStructDefaultValue(argType, Emitter));
                                }
                                else
                                {
                                    Write(defValue);
                                }
                            }
                            else
                            {
                                if (member.Initializer is PrimitiveExpression primitiveExpr && primitiveExpr.Value is AstType)
                                {
                                    Write(Inspector.GetStructDefaultValue((AstType)primitiveExpr.Value, Emitter));
                                }
                                else if (member.Initializer != null)
                                {
                                    member.Initializer.AcceptVisitor(Emitter);
                                }
                                else
                                {
                                    Write("null");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}