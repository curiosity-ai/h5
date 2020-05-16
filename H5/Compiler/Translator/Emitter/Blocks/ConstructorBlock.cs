using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ICSharpCode.NRefactory.TypeSystem;
using Object.Net.Utilities;

namespace H5.Translator
{
    public partial class ConstructorBlock : AbstractMethodBlock, IConstructorBlock
    {
        public ConstructorBlock(IEmitter emitter, ITypeInfo typeInfo, bool staticBlock)
            : base(emitter, typeInfo.TypeDeclaration)
        {
            Emitter = emitter;
            TypeInfo = typeInfo;
            StaticBlock = staticBlock;
        }

        public ITypeInfo TypeInfo { get; set; }

        public bool StaticBlock { get; set; }

        public bool HasEntryPoint
        {
            get; set;
        }

        protected override void DoEmit()
        {
            if (StaticBlock)
            {
                EmitCtorForStaticClass();
            }
            else
            {
                EmitCtorForInstantiableClass();
            }
        }

        protected virtual IEnumerable<string> GetInjectors()
        {
            var handlers = GetEventsAndAutoStartupMethods();
            var injectors = Emitter.Plugins.GetConstructorInjectors(this);
            return injectors.Concat(handlers);
        }

        protected virtual void EmitCtorForStaticClass()
        {
            var injectors = GetInjectors();
            IEnumerable<string> fieldsInjectors = null;

            var fieldBlock = new FieldBlock(Emitter, TypeInfo, true, true);
            fieldBlock.Emit();

            fieldsInjectors = fieldBlock.Injectors;

            if (fieldBlock.WasEmitted)
            {
                Emitter.Comma = true;
            }

            bool ctorHeader = false;
            if (TypeInfo.StaticConfig.HasConfigMembers || injectors.Any() || fieldsInjectors.Any())
            {
                EnsureComma();

                if (TypeInfo.StaticConfig.HasConfigMembers)
                {
                    var configBlock = new FieldBlock(Emitter, TypeInfo, true, false);
                    configBlock.ClearTempVariables = false;
                    configBlock.Emit();

                    if (configBlock.Injectors.Count > 0)
                    {
                        injectors = configBlock.Injectors.Concat(injectors);
                    }

                    if (configBlock.WasEmitted)
                    {
                        Emitter.Comma = true;
                    }
                }

                if (fieldsInjectors.Any())
                {
                    injectors = fieldsInjectors.Concat(injectors);
                }

                if (injectors.Count() > 0)
                {
                    EnsureComma();

                    ctorHeader = true;
                    Write(JS.Fields.CTORS);
                    WriteColon();
                    BeginBlock();

                    Write(JS.Funcs.INIT);
                    WriteColon();
                    WriteFunction();
                    WriteOpenParentheses();
                    WriteCloseParentheses();
                    WriteSpace();
                    BeginBlock();

                    if (Emitter.TempVariables != null)
                    {
                        SimpleEmitTempVars();
                        Emitter.TempVariables = new Dictionary<string, bool>();
                    }

                    foreach (var fn in injectors)
                    {
                        Write(WriteIndentToString(fn, Level - 1));
                        WriteNewLine();
                    }
                    EndBlock();
                    Emitter.Comma = true;
                }
            }

            var ctor = TypeInfo.StaticCtor;

            if (ctor != null && ctor.Body.HasChildren)
            {
                EnsureComma();

                if (!ctorHeader)
                {
                    ctorHeader = true;
                    Write(JS.Fields.CTORS);
                    WriteColon();
                    BeginBlock();
                }

                ResetLocals();
                var prevNamesMap = BuildLocalsNamesMap();
                Write(JS.Funcs.CONSTRUCTOR);
                WriteColon();
                WriteFunction();
                WriteOpenCloseParentheses(true);

                BeginBlock();
                var beginPosition = Emitter.Output.Length;

                var oldRules = Emitter.Rules;
                if (Emitter.Resolver.ResolveNode(ctor, Emitter) is MemberResolveResult rr)
                {
                    Emitter.Rules = Rules.Get(Emitter, rr.Member);
                }

                ctor.Body.AcceptChildren(Emitter);

                if (!Emitter.IsAsync)
                {
                    var indent = Emitter.TempVariables.Count > 0;
                    EmitTempVars(beginPosition, true);

                    if (indent)
                    {
                        Indent();
                    }
                }

                Emitter.Rules = oldRules;
                EndBlock();
                ClearLocalsNamesMap(prevNamesMap);
                Emitter.Comma = true;
            }

            if (ctorHeader)
            {
                WriteNewLine();
                EndBlock();
            }
        }

        private bool ctorHeader = false;
        protected virtual IEnumerable<string> EmitInitMembers()
        {
            var injectors = GetInjectors();

            var constructorWrapperString = CS.Wrappers.CONSTRUCTORWRAPPER + ":";

            IEnumerable<string> ctorWrappers = injectors.Where(i => i.StartsWith(constructorWrapperString)).Select(i => i.Substring(constructorWrapperString.Length));
            injectors = injectors.Where(i => !i.StartsWith(constructorWrapperString));

            IEnumerable<string> fieldsInjectors = null;

            var fieldBlock = new FieldBlock(Emitter, TypeInfo, false, true);
            fieldBlock.Emit();

            fieldsInjectors = fieldBlock.Injectors;

            if (fieldBlock.WasEmitted)
            {
                Emitter.Comma = true;
            }

            if (!TypeInfo.InstanceConfig.HasConfigMembers && !injectors.Any() && !fieldsInjectors.Any())
            {
                return ctorWrappers;
            }

            if (TypeInfo.InstanceConfig.HasConfigMembers)
            {
                var configBlock = new FieldBlock(Emitter, TypeInfo, false, false);
                configBlock.ClearTempVariables = false;
                configBlock.Emit();

                if (configBlock.Injectors.Count > 0)
                {
                    injectors = configBlock.Injectors.Concat(injectors);
                }

                if (configBlock.WasEmitted)
                {
                    Emitter.Comma = true;
                }
            }

            if (fieldsInjectors.Any())
            {
                injectors = fieldsInjectors.Concat(injectors);
            }

            if (injectors.Count() > 0)
            {
                EnsureComma();
                ctorHeader = true;
                Write(JS.Fields.CTORS);
                WriteColon();
                BeginBlock();

                Write(JS.Funcs.INIT);
                WriteColon();
                WriteFunction();
                WriteOpenParentheses();
                WriteCloseParentheses();
                WriteSpace();
                BeginBlock();

                if (Emitter.TempVariables != null)
                {
                    SimpleEmitTempVars();
                    Emitter.TempVariables = new Dictionary<string, bool>();
                }

                foreach (var fn in injectors)
                {
                    Write(WriteIndentToString(fn, Level - 1));
                    WriteNewLine();
                }

                EndBlock();

                Emitter.Comma = true;
            }

            return ctorWrappers;
        }

        protected virtual void EmitCtorForInstantiableClass()
        {
            var baseType = Emitter.GetBaseTypeDefinition();
            var typeDef = Emitter.GetTypeDefinition();
            var isObjectLiteral = Emitter.Validator.IsObjectLiteral(typeDef);
            var isPlainMode = Emitter.Validator.GetObjectCreateMode(typeDef) == 0;

            var ctorWrappers = isObjectLiteral ? new string[0] : EmitInitMembers().ToArray();

            if (!TypeInfo.HasRealInstantiable(Emitter) && ctorWrappers.Length == 0 || isObjectLiteral && isPlainMode)
            {
                if (ctorHeader)
                {
                    WriteNewLine();
                    EndBlock();
                }
                return;
            }

            bool forceDefCtor = isObjectLiteral && Emitter.Validator.GetObjectCreateMode(typeDef) == 1 && TypeInfo.Ctors.Count == 0;

            if (typeDef.IsValueType || forceDefCtor || (TypeInfo.Ctors.Count == 0 && ctorWrappers.Length > 0))
            {
                TypeInfo.Ctors.Add(new ConstructorDeclaration
                {
                    Modifiers = Modifiers.Public,
                    Body = new BlockStatement()
                });
            }

            if (!ctorHeader && TypeInfo.Ctors.Count > 0)
            {
                EnsureComma();
                ctorHeader = true;
                Write(JS.Fields.CTORS);
                WriteColon();
                BeginBlock();
            }

            Emitter.InConstructor = true;
            foreach (var ctor in TypeInfo.Ctors)
            {
                var oldRules = Emitter.Rules;

                if (ctor.Body.HasChildren)
                {
                    if (Emitter.Resolver.ResolveNode(ctor, Emitter) is MemberResolveResult rr)
                    {
                        Emitter.Rules = Rules.Get(Emitter, rr.Member);
                    }
                }

                EnsureComma();
                ResetLocals();
                var prevMap = BuildLocalsMap();
                var prevNamesMap = BuildLocalsNamesMap();
                AddLocals(ctor.Parameters, ctor.Body);

                var ctorName = JS.Funcs.CONSTRUCTOR;

                if (TypeInfo.Ctors.Count > 1 && ctor.Parameters.Count > 0)
                {
                    var overloads = OverloadsCollection.Create(Emitter, ctor);
                    ctorName = overloads.GetOverloadName();
                }

                XmlToJsDoc.EmitComment(this, ctor);
                Write(ctorName);

                WriteColon();
                WriteFunction();

                int pos = Emitter.Output.Length;
                EmitMethodParameters(ctor.Parameters, null, ctor);
                var ctorParams = Emitter.Output.ToString().Substring(pos);

                WriteSpace();
                BeginBlock();
                var len = Emitter.Output.Length;
                var requireNewLine = false;

                var noThisInvocation = ctor.Initializer == null || ctor.Initializer.IsNull || ctor.Initializer.ConstructorInitializerType == ConstructorInitializerType.Base;
                IWriterInfo oldWriter = null;
                if (ctorWrappers.Length > 0 && noThisInvocation)
                {
                    oldWriter = SaveWriter();
                    NewWriter();
                }

                ConvertParamsToReferences(ctor.Parameters);

                if (len != Emitter.Output.Length)
                {
                    requireNewLine = true;
                }

                if (isObjectLiteral)
                {
                    if (requireNewLine)
                    {
                        WriteNewLine();
                    }

                    Write("var " + JS.Vars.D_THIS + " = ");

                    var isBaseObjectLiteral = baseType != null && Emitter.Validator.IsObjectLiteral(baseType);
                    if (isBaseObjectLiteral && baseType != null && (!Emitter.Validator.IsExternalType(baseType) || Emitter.Validator.IsH5Class(baseType)) ||
                    (ctor.Initializer != null && ctor.Initializer.ConstructorInitializerType == ConstructorInitializerType.This))
                    {
                        EmitBaseConstructor(ctor, ctorName, true);
                    }
                    else if (isBaseObjectLiteral && baseType != null && ctor.Initializer != null &&
                             ctor.Initializer.ConstructorInitializerType == ConstructorInitializerType.Base)
                    {
                        EmitExternalBaseCtor(ctor, ref requireNewLine);
                    }
                    else
                    {
                        Write("{ };");
                    }

                    WriteNewLine();

                    string name = Emitter.Validator.GetCustomTypeName(typeDef, Emitter, false);
                    if (name.IsEmpty())
                    {
                        name = H5Types.ToJsName(TypeInfo.Type, Emitter);
                    }

                    Write(JS.Vars.D_THIS + "." + JS.Funcs.GET_TYPE + " = function () { return " + name + "; };");

                    WriteNewLine();
                    Write("(function ()");
                    BeginBlock();
                    requireNewLine = false;
                }

                var beginPosition = Emitter.Output.Length;

                if (noThisInvocation)
                {
                    if (requireNewLine)
                    {
                        WriteNewLine();
                    }

                    if (isObjectLiteral)
                    {
                        var fieldBlock = new FieldBlock(Emitter, TypeInfo, false, false, true);
                        fieldBlock.Emit();

                        var properties = TypeInfo.InstanceProperties;

                        var names = new List<string>(properties.Keys);

                        foreach (var name in names)
                        {
                            var props = properties[name];

                            foreach (var prop in props)
                            {
                                if (prop is PropertyDeclaration p)
                                {
                                    if (p.Getter.Body.IsNull && p.Setter.Body.IsNull)
                                    {
                                        continue;
                                    }

                                    Write(JS.Types.Object.DEFINEPROPERTY);
                                    WriteOpenParentheses();
                                    Write("this, ");
                                    WriteScript(OverloadsCollection.Create(Emitter, p).GetOverloadName());
                                    WriteComma();
                                    Emitter.Comma = false;
                                    BeginBlock();
                                    var memberResult = Emitter.Resolver.ResolveNode(p, Emitter) as MemberResolveResult;
                                    var block = new VisitorPropertyBlock(Emitter, p);
                                    block.EmitPropertyMethod(p, p.Getter, ((IProperty)memberResult.Member).Getter, false, true);
                                    block.EmitPropertyMethod(p, p.Setter, ((IProperty)memberResult.Member).Setter, true, true);
                                    EnsureComma(true);
                                    Write(JS.Fields.ENUMERABLE + ": true");
                                    WriteNewLine();
                                    EndBlock();
                                    WriteCloseParentheses();
                                    Write(";");
                                    WriteNewLine();
                                }
                            }
                        }
                    }
                    else
                    {
                        Write("this." + JS.Funcs.INITIALIZE + "();");
                        requireNewLine = true;
                    }
                }

                if (!isObjectLiteral)
                {
                    if (baseType != null && (!Emitter.Validator.IsExternalType(baseType) || Emitter.Validator.IsH5Class(baseType)) ||
                    (ctor.Initializer != null && ctor.Initializer.ConstructorInitializerType == ConstructorInitializerType.This))
                    {
                        if (requireNewLine)
                        {
                            WriteNewLine();
                            requireNewLine = false;
                        }
                        EmitBaseConstructor(ctor, ctorName, false);
                    }
                    else if (baseType != null && (ctor.Initializer == null || ctor.Initializer.IsNull || ctor.Initializer.ConstructorInitializerType == ConstructorInitializerType.Base))
                    {
                        EmitExternalBaseCtor(ctor, ref requireNewLine);
                    }
                }

                var script = Emitter.GetScript(ctor);
                var hasAdditionalIndent = false;

                if (script == null)
                {
                    if (ctor.Body.HasChildren)
                    {
                        if (requireNewLine)
                        {
                            WriteNewLine();
                        }

                        ctor.Body.AcceptChildren(Emitter);

                        if (!Emitter.IsAsync)
                        {
                            hasAdditionalIndent = Emitter.TempVariables.Count > 0;
                            EmitTempVars(beginPosition, true);
                        }
                    }
                    else if (requireNewLine)
                    {
                        WriteNewLine();
                    }
                }
                else
                {
                    if (requireNewLine)
                    {
                        WriteNewLine();
                    }

                    WriteLines(script);
                }

                if (oldWriter != null)
                {
                    WrapBody(oldWriter, ctorWrappers, ctorParams);
                }

                if (isObjectLiteral)
                {
                    if (requireNewLine)
                    {
                        WriteNewLine();
                    }
                    EndBlock();
                    Write(")." + JS.Funcs.CALL + "(" + JS.Vars.D_THIS + ");");
                    WriteNewLine();
                    Write("return " + JS.Vars.D_THIS + ";");
                    WriteNewLine();
                }

                if (hasAdditionalIndent)
                {
                    Indent();
                }

                EndBlock();
                Emitter.Comma = true;
                ClearLocalsMap(prevMap);
                ClearLocalsNamesMap(prevNamesMap);

                Emitter.Rules = oldRules;
            }

            Emitter.InConstructor = false;

            if (ctorHeader)
            {
                WriteNewLine();
                EndBlock();
            }
        }

        private void EmitExternalBaseCtor(ConstructorDeclaration ctor, ref bool requireNewLine)
        {
            IMember member = null;
            var hasInitializer = ctor.Initializer != null && !ctor.Initializer.IsNull;
            var baseType = Emitter.GetBaseTypeDefinition();

            if (hasInitializer)
            {
                member = ((InvocationResolveResult)Emitter.Resolver.ResolveNode(ctor.Initializer, Emitter)).Member;
            }

            if (member != null)
            {
                var inlineCode = Emitter.GetInline(member);

                if (!string.IsNullOrEmpty(inlineCode))
                {
                    if (requireNewLine)
                    {
                        WriteNewLine();
                        requireNewLine = false;
                    }

                    Write(JS.Types.H5.APPLY);
                    WriteOpenParentheses();

                    Write("this, ");
                    var argsInfo = new ArgumentsInfo(Emitter, ctor.Initializer);
                    new InlineArgumentsBlock(Emitter, argsInfo, inlineCode).Emit();
                    WriteCloseParentheses();
                    WriteSemiColon();
                    WriteNewLine();

                    return;
                }
            }

            if (hasInitializer || (baseType.FullName != "System.Object" && baseType.FullName != "System.ValueType" && baseType.FullName != "System.Enum" && !baseType.CustomAttributes.Any(a => a.AttributeType.FullName == "H5.NonScriptableAttribute") && !baseType.IsInterface))
            {
                if (requireNewLine)
                {
                    WriteNewLine();
                    requireNewLine = false;
                }


                string name = null;
                if (TypeInfo.GetBaseTypes(Emitter).Any())
                {
                    name = H5Types.ToJsName(TypeInfo.GetBaseClass(Emitter), Emitter);
                }
                else
                {
                    name = H5Types.ToJsName(baseType, Emitter);
                }

                Write(name);
                WriteCall();
                int openPos = Emitter.Output.Length;
                WriteOpenParentheses();
                Write("this");

                if (hasInitializer && ctor.Initializer.Arguments.Count > 0)
                {
                    Write(", ");
                    var argsInfo = new ArgumentsInfo(Emitter, ctor.Initializer);
                    var argsExpressions = argsInfo.ArgumentsExpressions;
                    var paramsArg = argsInfo.ParamsExpression;

                    new ExpressionListBlock(Emitter, argsExpressions, paramsArg, ctor.Initializer, openPos).Emit();
                }

                WriteCloseParentheses();
                WriteSemiColon();
                WriteNewLine();
            }
        }

        protected virtual void WrapBody(IWriterInfo oldWriter, string[] ctorWrappers, string ctorParams)
        {
            var body = Emitter.Output.ToString();
            RestoreWriter(oldWriter);

            List<string> endParts = new List<string>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ctorWrappers.Length; i++)
            {
                var isLast = i == (ctorWrappers.Length - 1);
                var ctorWrapper = ctorWrappers[i];
                var parts = ctorWrapper.Split(new[] { CS.Wrappers.Params.BODY }, StringSplitOptions.RemoveEmptyEntries);
                endParts.Add(parts[1]);

                sb.Append(parts[0]);
                sb.Append("function ");
                sb.Append(ctorParams);
                sb.Append(" {");

                if (!isLast)
                {
                    sb.Append(H5.Translator.Emitter.NEW_LINE);
                }

                Indent();

                for (var j = 0; j < Emitter.Level; j++)
                {
                    sb.Append(H5.Translator.Emitter.INDENT);
                }

                if (isLast)
                {
                    sb.Append(WriteIndentToString(body));
                }
            }

            endParts.Reverse();

            var newLine = false;
            foreach (var endPart in endParts)
            {
                Outdent();

                if (newLine)
                {
                    sb.Append(H5.Translator.Emitter.NEW_LINE);
                    for (var j = 0; j < Emitter.Level; j++)
                    {
                        sb.Append(H5.Translator.Emitter.INDENT);
                    }
                }
                else if (sb.ToString().Substring(sb.Length - 4) == H5.Translator.Emitter.INDENT)
                {
                    sb.Length -= 4;
                }
                newLine = true;

                sb.Append("}");
                sb.Append(endPart);
            }

            Write(sb.ToString());
            WriteNewLine();
        }

        protected virtual void EmitBaseConstructor(ConstructorDeclaration ctor, string ctorName, bool isObjectLiteral)
        {
            var initializer = ctor.Initializer != null && !ctor.Initializer.IsNull ? ctor.Initializer : new ConstructorInitializer()
            {
                ConstructorInitializerType = ConstructorInitializerType.Base
            };

            bool appendScope = false;
            bool isBaseObjectLiteral = false;

            if (initializer.ConstructorInitializerType == ConstructorInitializerType.Base)
            {
                var baseType = Emitter.GetBaseTypeDefinition();
                //var baseName = JS.Funcs.CONSTRUCTOR;
                string baseName = null;
                isBaseObjectLiteral = Emitter.Validator.IsObjectLiteral(baseType);

                if (ctor.Initializer != null && !ctor.Initializer.IsNull)
                {
                    var member = ((InvocationResolveResult)Emitter.Resolver.ResolveNode(ctor.Initializer, Emitter)).Member;
                    var overloads = OverloadsCollection.Create(Emitter, member);
                    if (overloads.HasOverloads)
                    {
                        baseName = overloads.GetOverloadName();
                    }
                }

                string name = null;

                if (TypeInfo.GetBaseTypes(Emitter).Any())
                {
                    name = H5Types.ToJsName(TypeInfo.GetBaseClass(Emitter), Emitter);
                }
                else
                {
                    name = H5Types.ToJsName(baseType, Emitter);
                }

                if (!isObjectLiteral && isBaseObjectLiteral)
                {
                    Write(JS.Types.H5.COPY_PROPERTIES);
                    WriteOpenParentheses();

                    Write("this, ");
                }

                Write(name, ".");

                if (baseName == null)
                {
                    var baseIType = Emitter.H5Types.Get(baseType).Type;

                    var baseCtor = baseIType.GetConstructors().SingleOrDefault(c => c.Parameters.Count == 0);
                    if (baseCtor == null)
                    {
                        baseCtor = baseIType.GetConstructors().SingleOrDefault(c => c.Parameters.All(p => p.IsOptional));
                    }

                    if (baseCtor == null)
                    {
                        baseCtor = baseIType.GetConstructors().SingleOrDefault(c => c.Parameters.Count == 1 && c.Parameters.First().IsParams);
                    }

                    if (baseCtor != null)
                    {
                        baseName = OverloadsCollection.Create(Emitter, baseCtor).GetOverloadName();
                    }
                    else
                    {
                        baseName = JS.Funcs.CONSTRUCTOR;
                    }
                }

                Write(baseName);

                if (!isObjectLiteral)
                {
                    WriteCall();
                    appendScope = true;
                }
            }
            else
            {
                // this.WriteThis();
                string name = H5Types.ToJsName(TypeInfo.Type, Emitter);
                Write(name);
                WriteDot();

                var baseName = JS.Funcs.CONSTRUCTOR;
                var member = ((InvocationResolveResult)Emitter.Resolver.ResolveNode(ctor.Initializer, Emitter)).Member;
                var overloads = OverloadsCollection.Create(Emitter, member);
                if (overloads.HasOverloads)
                {
                    baseName = overloads.GetOverloadName();
                }

                Write(baseName);

                if (!isObjectLiteral)
                {
                    WriteCall();
                    appendScope = true;
                }
            }
            int openPos = Emitter.Output.Length;
            WriteOpenParentheses();

            if (appendScope)
            {
                WriteThis();

                if (initializer.Arguments.Count > 0)
                {
                    WriteComma();
                }
            }

            if (initializer.Arguments.Count > 0)
            {
                var argsInfo = new ArgumentsInfo(Emitter, ctor.Initializer);
                var argsExpressions = argsInfo.ArgumentsExpressions;
                var paramsArg = argsInfo.ParamsExpression;

                new ExpressionListBlock(Emitter, argsExpressions, paramsArg, ctor.Initializer, openPos).Emit();
            }

            if (!isObjectLiteral && isBaseObjectLiteral)
            {
                WriteCloseParentheses();
            }

            WriteCloseParentheses();
            WriteSemiColon();

            if (!isObjectLiteral)
            {
                WriteNewLine();
            }
        }

        protected virtual bool IsGenericType()
        {
            return TypeInfo.Type.TypeParameterCount > 0;
        }

        private bool IsGenericMethod(MethodDeclaration methodDeclaration)
        {
            return methodDeclaration.TypeParameters.Any();
        }
    }
}