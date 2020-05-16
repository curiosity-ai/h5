using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Refactoring;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace H5.Translator
{
    public class AttributeCreateBlock : AbstractEmitterBlock
    {
        public AttributeCreateBlock(IEmitter emitter, IAttribute attribute)
            : base(emitter, null)
        {
            Emitter = emitter;
            Attribute = attribute;
        }

        public IAttribute Attribute { get; set; }

        protected override void DoEmit()
        {
            IAttribute attribute = Attribute;

            var type = Emitter.GetTypeDefinition(attribute.AttributeType);

            var argsInfo = new ArgumentsInfo(Emitter, attribute);

            string inlineCode = Emitter.GetInline(attribute.Constructor);

            var customCtor = Emitter.Validator.GetCustomConstructor(type) ?? "";
            var hasInitializer = attribute.NamedArguments.Count > 0;

            if (inlineCode == null && Regex.Match(customCtor, @"\s*\{\s*\}\s*").Success)
            {
                WriteOpenBrace();
                WriteSpace();

                if (hasInitializer)
                {
                    WriteObjectInitializer(attribute.NamedArguments, type, attribute);
                    WriteSpace();
                }
                else if (Emitter.Validator.IsObjectLiteral(type))
                {
                    WriteObjectInitializer(null, type, attribute);
                    WriteSpace();
                }

                WriteCloseBrace();
            }
            else
            {
                if (hasInitializer)
                {
                    Write(JS.Types.H5.APPLY);
                    WriteOpenParentheses();
                }

                if (inlineCode != null)
                {
                    new InlineArgumentsBlock(Emitter, argsInfo, inlineCode, attribute.Constructor).Emit();
                }
                else
                {
                    if (String.IsNullOrEmpty(customCtor))
                    {
                        WriteNew();
                        Write(H5Types.ToJsName(attribute.AttributeType, Emitter));
                    }
                    else
                    {
                        Write(customCtor);
                    }

                    if (!Emitter.Validator.IsExternalType(type) && type.Methods.Count(m => m.IsConstructor && !m.IsStatic) > (type.IsValueType ? 0 : 1))
                    {
                        WriteDot();
                        var name = OverloadsCollection.Create(Emitter, attribute.Constructor).GetOverloadName();
                        Write(name);
                    }

                    WriteOpenParentheses();

                    WritePositionalList(attribute.PositionalArguments, attribute);
                    WriteCloseParentheses();
                }

                if (hasInitializer)
                {
                    WriteComma();

                    BeginBlock();

                    var inlineInit = WriteObjectInitializer(attribute.NamedArguments, type, attribute);

                    WriteNewLine();

                    EndBlock();

                    if (inlineInit.Count > 0)
                    {
                        Write(", function () ");
                        BeginBlock();

                        foreach (var init in inlineInit)
                        {
                            Write(init);
                            WriteNewLine();
                        }

                        EndBlock();
                    }

                    WriteSpace();
                    WriteCloseParentheses();
                }
            }
        }

        protected virtual void WritePositionalList(IList<ResolveResult> expressions, IAttribute attr)
        {
            bool needComma = false;
            int count = Emitter.Writers.Count;
            bool expanded = false;
            int paramsIndex = -1;

            if (attr.Constructor.Parameters.Any(p => p.IsParams))
            {
                paramsIndex = attr.Constructor.Parameters.IndexOf(attr.Constructor.Parameters.FirstOrDefault(p => p.IsParams));
                var or = new OverloadResolution(Emitter.Resolver.Compilation, expressions.ToArray());
                or.AddCandidate(attr.Constructor);
                expanded = or.BestCandidateIsExpandedForm;
            }

            for (int i = 0; i < expressions.Count; i++)
            {
                var expr = expressions[i];

                if (needComma)
                {
                    WriteComma();
                }

                needComma = true;

                if (expanded && paramsIndex == i)
                {
                    WriteOpenBracket();
                }

                WriteResolveResult(expr, this);

                if (Emitter.Writers.Count != count)
                {
                    PopWriter();
                    count = Emitter.Writers.Count;
                }
            }

            if (expanded)
            {
                WriteCloseBracket();
            }
        }

        public static void WriteResolveResult(ResolveResult rr, AbstractEmitterBlock block)
        {
            if (rr is ConversionResolveResult)
            {
                rr = ((ConversionResolveResult)rr).Input;
            }

            if (rr is TypeOfResolveResult)
            {
                block.Write(H5Types.ToJsName(((TypeOfResolveResult)rr).ReferencedType, block.Emitter));
            }
            else if (rr is ArrayCreateResolveResult)
            {
                TypeSystemAstBuilder typeBuilder =
                    new TypeSystemAstBuilder(new CSharpResolver(block.Emitter.Resolver.Compilation));
                var expression = typeBuilder.ConvertConstantValue(rr) as ArrayCreateExpression;
                new ArrayCreateBlock(block.Emitter, expression, (ArrayCreateResolveResult)rr).Emit();
            }
            else if (rr is MemberResolveResult mrr)
            {
                if (mrr.IsCompileTimeConstant && mrr.Member.DeclaringType.Kind == TypeKind.Enum)
                {
                    if (mrr.Member.DeclaringType is DefaultResolvedTypeDefinition typeDef)
                    {
                        var enumMode = Helpers.EnumEmitMode(typeDef);

                        if ((block.Emitter.Validator.IsExternalType(typeDef) && enumMode == -1) || enumMode == 2)
                        {
                            block.WriteScript(mrr.ConstantValue);

                            return;
                        }

                        if (enumMode >= 3 && enumMode < 7)
                        {
                            string enumStringName = mrr.Member.Name;
                            var attr = Helpers.GetInheritedAttribute(mrr.Member,
                                Translator.H5_ASSEMBLY + ".NameAttribute");

                            if (attr != null)
                            {
                                enumStringName = block.Emitter.GetEntityName(mrr.Member);
                            }
                            else
                            {
                                switch (enumMode)
                                {
                                    case 3:
                                        enumStringName =
                                            Object.Net.Utilities.StringUtils.ToLowerCamelCase(mrr.Member.Name);
                                        break;

                                    case 4:
                                        break;

                                    case 5:
                                        enumStringName = enumStringName.ToLowerInvariant();
                                        break;

                                    case 6:
                                        enumStringName = enumStringName.ToUpperInvariant();
                                        break;
                                }
                            }

                            block.WriteScript(enumStringName);
                        }
                        else
                        {
                            block.WriteScript(rr.ConstantValue);
                        }
                    }
                }
                else
                {
                    block.WriteScript(rr.ConstantValue);
                }
            }
            else
            {
                block.WriteScript(rr.ConstantValue);
            }
        }

        public static string GetInlineInit(KeyValuePair<IMember, ResolveResult> item, AbstractEmitterBlock block)
        {
            string inlineCode = null;
            var member = item.Key;

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

                bool hasThis = Helpers.HasThis(inlineCode);
                inlineCode = Helpers.ConvertTokens(block.Emitter, inlineCode, member);
                if (inlineCode.StartsWith("<self>"))
                {
                    hasThis = true;
                    inlineCode = inlineCode.Substring(6);
                }

                if (hasThis)
                {
                    inlineCode = inlineCode.Replace("{this}", "this");

                    if (member is IProperty)
                    {
                        inlineCode = inlineCode.Replace("{0}", "[[0]]");

                        WriteResolveResult(item.Value, block);
                        var value = block.Emitter.Output.ToString();
                        inlineCode = inlineCode.Replace("{value}", value);
                        inlineCode = inlineCode.Replace("[[0]]", "{0}");
                    }
                }
                else
                {
                    if (member.SymbolKind == SymbolKind.Property)
                    {
                        var count = block.Emitter.Writers.Count;
                        block.PushWriter("this." + inlineCode);
                        WriteResolveResult(item.Value, block);

                        if (block.Emitter.Writers.Count > count)
                        {
                            inlineCode = block.PopWriter(true);
                        }
                    }
                    else
                    {
                        block.Write("this." + inlineCode);
                    }
                }

                block.Emitter.IsAssignment = oldIsAssignment;
                block.Emitter.IsUnaryAccessor = oldUnary;
                block.RestoreWriter(oldWriter);
            }

            if (inlineCode != null && !inlineCode.Trim().EndsWith(";"))
            {
                inlineCode += ";";
            }

            return inlineCode;
        }

        protected virtual List<string> WriteObjectInitializer(IList<KeyValuePair<IMember, ResolveResult>> expressions, TypeDefinition type, IAttribute attr)
        {
            bool needComma = false;
            List<string> names = new List<string>();
            List<string> inlineInit = new List<string>();

            if (expressions != null)
            {
                foreach (KeyValuePair<IMember, ResolveResult> item in expressions)
                {
                    var member = item.Key;
                    var name = Emitter.GetEntityName(member);

                    var inlineCode = GetInlineInit(item, this);

                    if (inlineCode != null)
                    {
                        inlineInit.Add(inlineCode);
                    }
                    else
                    {
                        if (member is IProperty)
                        {
                            name = Helpers.GetPropertyRef(member, Emitter, true);
                        }
                        else if (member is IEvent)
                        {
                            name = Helpers.GetEventRef(member, Emitter, false);
                        }

                        if (needComma)
                        {
                            WriteComma();
                        }

                        needComma = true;

                        Write(name, ": ");

                        WriteResolveResult(item.Value, this);

                        names.Add(name);
                    }
                }
            }

            if (Emitter.Validator.IsObjectLiteral(type))
            {
                var key = H5Types.GetTypeDefinitionKey(type);
                var tinfo = Emitter.Types.FirstOrDefault(t => t.Key == key);

                if (tinfo == null)
                {
                    return inlineInit;
                }

                var mode = 0;
                if (attr.Constructor != null)
                {
                    if (tinfo.Type is ITypeDefinition itype)
                    {
                        var oattr = Emitter.Validator.GetAttribute(itype.Attributes, Translator.H5_ASSEMBLY + ".ObjectLiteralAttribute");
                        if (oattr.PositionalArguments.Count > 0)
                        {
                            var value = oattr.PositionalArguments.First().ConstantValue;

                            if (value is int)
                            {
                                mode = (int)value;
                            }
                        }
                    }
                }

                if (mode != 0)
                {
                    var members = tinfo.InstanceConfig.Fields.Concat(tinfo.InstanceConfig.Properties);

                    if (members.Any())
                    {
                        foreach (var member in members)
                        {
                            if (mode == 1 && (member.VarInitializer == null || member.VarInitializer.Initializer.IsNull))
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

                            Write(name, ": ");


                            if (member.Initializer is PrimitiveExpression primitiveExpr && primitiveExpr.Value is AstType)
                            {
                                Write(Inspector.GetStructDefaultValue((AstType)primitiveExpr.Value, Emitter));
                            }
                            else
                            {
                                member.Initializer.AcceptVisitor(Emitter);
                            }
                        }
                    }
                }
            }

            return inlineInit;
        }
    }
}