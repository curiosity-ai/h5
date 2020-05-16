using System;
using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp.Resolver;

namespace H5.Translator
{
    public class FieldBlock : AbstractEmitterBlock
    {
        public FieldBlock(IEmitter emitter, ITypeInfo typeInfo, bool staticBlock, bool fieldsOnly, bool isObjectLiteral = false)
            : base(emitter, typeInfo.TypeDeclaration)
        {
            Emitter = emitter;
            TypeInfo = typeInfo;
            StaticBlock = staticBlock;
            FieldsOnly = fieldsOnly;
            Injectors = new List<string>();
            IsObjectLiteral = isObjectLiteral;
            ClearTempVariables = true;
        }

        public bool IsObjectLiteral { get; set; }

        public ITypeInfo TypeInfo { get; set; }

        public bool StaticBlock { get; set; }

        public bool FieldsOnly { get; set; }

        public List<string> Injectors { get; private set; }

        public int BeginCounter { get; private set; }

        public bool WasEmitted { get; private set; }

        public bool ClearTempVariables
        {
            get; set;
        }

        protected override void DoEmit()
        {
            if (Emitter.TempVariables != null && ClearTempVariables)
            {
                Emitter.TempVariables = new Dictionary<string, bool>();
            }
            EmitFields(StaticBlock ? TypeInfo.StaticConfig : TypeInfo.InstanceConfig);

            /*if (this.Injectors.Count > 0 && this.Emitter.TempVariables != null && this.Emitter.TempVariables.Count > 0)
            {
                var writer = this.SaveWriter();
                this.NewWriter();
                this.SimpleEmitTempVars(false);
                this.Injectors.Insert(0, this.Emitter.Output.ToString());
                this.Emitter.TempVariables.Clear();
                this.RestoreWriter(writer);
            }*/
        }

        protected virtual void EmitFields(TypeConfigInfo info)
        {
            if (FieldsOnly || IsObjectLiteral)
            {
                if (info.Fields.Count > 0)
                {
                    var hasFields = WriteObject(JS.Fields.FIELDS, info.Fields.Where(f => f.IsConst).Concat(info.Fields.Where(f => !f.IsConst)).ToList(), "this.{0} = {1};", "this[{0}] = {1};");
                    if (hasFields)
                    {
                        Emitter.Comma = true;
                        WasEmitted = true;
                    }
                }

                if (!IsObjectLiteral)
                {
                    return;
                }
            }

            if (info.Events.Count > 0 && !IsObjectLiteral)
            {
                var hasProperties = WriteObject(JS.Fields.EVENTS, info.Events, JS.Funcs.H5_EVENT + "(this, \"{0}\", {1});", JS.Funcs.H5_EVENT + "(this, {0}, {1});");
                if (hasProperties)
                {
                    Emitter.Comma = true;
                    WasEmitted = true;
                }
            }

            if (info.Properties.Count > 0)
            {
                var hasProperties = WriteObject(JS.Fields.PROPERTIES, info.Properties, "this.{0} = {1};", "this[{0}] = {1};");
                if (hasProperties)
                {
                    Emitter.Comma = true;
                    WasEmitted = true;
                }
            }

            if (info.Alias.Count > 0 && !IsObjectLiteral)
            {
                WriteAlias("alias", info.Alias);
                Emitter.Comma = true;
            }
        }

        protected virtual bool WriteObject(string objectName, List<TypeConfigItem> members, string format, string interfaceFormat)
        {
            bool hasProperties = HasProperties(objectName, members);
            int pos = 0;
            IWriterInfo writer = null;
            bool beginBlock = false;

            if (hasProperties && objectName != null && !IsObjectLiteral)
            {
                beginBlock = true;
                pos = Emitter.Output.Length;
                writer = SaveWriter();
                EnsureComma();
                Write(objectName);

                WriteColon();
                BeginBlock();
            }

            bool isProperty = JS.Fields.PROPERTIES == objectName;
            bool isField = JS.Fields.FIELDS == objectName;
            int count = 0;

            foreach (var member in members)
            {
                object constValue = null;
                bool isPrimitive = false;
                bool write = false;
                bool writeScript = false;

                if (member.Initializer is PrimitiveExpression primitiveExpr)
                {
                    //isPrimitive = true;
                    constValue = primitiveExpr.Value;

                    ResolveResult rr = null;
                    if (member.VarInitializer != null)
                    {
                        rr = Emitter.Resolver.ResolveNode(member.VarInitializer, Emitter);
                    }
                    else
                    {
                        rr = Emitter.Resolver.ResolveNode(member.Entity, Emitter);
                    }

                    if (rr != null && rr.Type.Kind == TypeKind.Enum)
                    {
                        constValue = Helpers.GetEnumValue(Emitter, rr.Type, constValue);
                        writeScript = true;
                    }
                }

                if (constValue is RawValue)
                {
                    constValue = constValue.ToString();
                    write = true;
                    writeScript = false;
                }

                var isNull = member.Initializer.IsNull || member.Initializer is NullReferenceExpression || member.Initializer.Parent == null;

                if (!isNull && !isPrimitive)
                {
                    var constrr = Emitter.Resolver.ResolveNode(member.Initializer, Emitter);
                    if (constrr != null && constrr.IsCompileTimeConstant)
                    {
                        //isPrimitive = true;
                        constValue = constrr.ConstantValue;

                        var expectedType = Emitter.Resolver.Resolver.GetExpectedType(member.Initializer);
                        if (!expectedType.Equals(constrr.Type) && expectedType.Kind != TypeKind.Dynamic)
                        {
                            try
                            {
                                constValue = Convert.ChangeType(constValue, ReflectionHelper.GetTypeCode(expectedType));
                            }
                            catch (Exception)
                            {
                                Emitter.Log.Warn($"FieldBlock: Convert.ChangeType is failed. Value type: {constrr.Type.FullName}, Target type: {expectedType.FullName}");
                            }
                        }

                        if (constrr.Type.Kind == TypeKind.Enum)
                        {
                            constValue = Helpers.GetEnumValue(Emitter, constrr.Type, constrr.ConstantValue);
                        }

                        writeScript = true;
                    }
                }

                var isNullable = false;

                if (isPrimitive && constValue is AstType)
                {
                    var itype = Emitter.Resolver.ResolveNode((AstType)constValue, Emitter);

                    if (NullableType.IsNullable(itype.Type))
                    {
                        isNullable = true;
                    }
                }

                string tpl = null;
                IMember templateMember = null;
                MemberResolveResult init_rr = null;
                if (isField && member.VarInitializer != null)
                {
                    init_rr = Emitter.Resolver.ResolveNode(member.VarInitializer, Emitter) as MemberResolveResult;
                    tpl = init_rr != null ? Emitter.GetInline(init_rr.Member) : null;

                    if (tpl != null)
                    {
                        templateMember = init_rr.Member;
                    }
                }

                bool isAutoProperty = false;

                if (isProperty)
                {
                    var member_rr = Emitter.Resolver.ResolveNode(member.Entity, Emitter) as MemberResolveResult;
                    var property = (IProperty)member_rr.Member;
                    isAutoProperty = Helpers.IsAutoProperty(property);
                }

                bool written = false;
                if (!isNull && (!isPrimitive || constValue is AstType || tpl != null) && !(isProperty && !IsObjectLiteral && !isAutoProperty))
                {
                    string value = null;
                    bool needContinue = false;
                    string defValue = "";
                    if (!isPrimitive)
                    {
                        var oldWriter = SaveWriter();
                        NewWriter();
                        member.Initializer.AcceptVisitor(Emitter);
                        value = Emitter.Output.ToString();
                        RestoreWriter(oldWriter);

                        ResolveResult rr = null;
                        AstType astType = null;
                        if (member.VarInitializer != null)
                        {
                            rr = Emitter.Resolver.ResolveNode(member.VarInitializer, Emitter);
                        }
                        else
                        {
                            astType = member.Entity.ReturnType;
                            rr = Emitter.Resolver.ResolveNode(member.Entity, Emitter);
                        }

                        constValue = Inspector.GetDefaultFieldValue(rr.Type, astType);
                        if (rr.Type.Kind == TypeKind.Enum)
                        {
                            constValue = Helpers.GetEnumValue(Emitter, rr.Type, constValue);
                        }
                        isNullable = NullableType.IsNullable(rr.Type);
                        needContinue = constValue is IType;
                        writeScript = true;

                        /*if (needContinue && !(member.Initializer is ObjectCreateExpression))
                        {
                            defValue = " || " + Inspector.GetStructDefaultValue((IType)constValue, this.Emitter);
                        }*/
                    }
                    else if (constValue is AstType)
                    {
                        value = isNullable
                            ? "null"
                            : Inspector.GetStructDefaultValue((AstType)constValue, Emitter);
                        constValue = value;
                        write = true;
                        needContinue = !isProperty && !isNullable;
                    }

                    var name = member.GetName(Emitter);

                    bool isValidIdentifier = Helpers.IsValidIdentifier(name);

                    if (isProperty && isPrimitive)
                    {
                        constValue = "null";

                        if (IsObjectLiteral)
                        {
                            written = true;
                            if (isValidIdentifier)
                            {
                                Write(string.Format("this.{0} = {1};", name, value));
                            }
                            else
                            {
                                Write(string.Format("this[{0}] = {1};", AbstractEmitterBlock.ToJavaScript(name, Emitter), value));
                            }

                            WriteNewLine();
                        }
                        else
                        {
                            Injectors.Add(string.Format(name.StartsWith("\"") || !isValidIdentifier ? "this[{0}] = {1};" : "this.{0} = {1};", isValidIdentifier ? name : AbstractEmitterBlock.ToJavaScript(name, Emitter), value));
                        }
                    }
                    else
                    {
                        if (IsObjectLiteral)
                        {
                            written = true;
                            if (isValidIdentifier)
                            {
                                Write(string.Format("this.{0} = {1};", name, value + defValue));
                            }
                            else
                            {
                                Write(string.Format("this[{0}] = {1};", AbstractEmitterBlock.ToJavaScript(name, Emitter), value + defValue));
                            }
                            WriteNewLine();
                        }
                        else if (tpl != null)
                        {
                            if (!tpl.Contains("{0}"))
                            {
                                tpl = tpl + " = {0};";
                            }

                            string v = null;
                            if (!isNull && (!isPrimitive || constValue is AstType))
                            {
                                v = value + defValue;
                            }
                            else
                            {
                                if (write)
                                {
                                    v = constValue != null ? constValue.ToString() : "";
                                }
                                else if (writeScript)
                                {
                                    v = AbstractEmitterBlock.ToJavaScript(constValue, Emitter);
                                }
                                else
                                {
                                    var oldWriter = SaveWriter();
                                    NewWriter();
                                    member.Initializer.AcceptVisitor(Emitter);
                                    v = Emitter.Output.ToString();
                                    RestoreWriter(oldWriter);
                                }
                            }

                            tpl = Helpers.ConvertTokens(Emitter, tpl, templateMember);
                            tpl = tpl.Replace("{this}", "this").Replace("{0}", v);

                            if (!tpl.EndsWith(";"))
                            {
                                tpl += ";";
                            }
                            Injectors.Add(tpl);
                        }
                        else
                        {
                            bool isDefaultInstance = Emitter.Resolver.ResolveNode(member.Initializer, Emitter) is CSharpInvocationResolveResult rr &&
                                                     rr.Member.SymbolKind == SymbolKind.Constructor &&
                                                     rr.Arguments.Count == 0 &&
                                                     rr.InitializerStatements.Count == 0 &&
                                                     rr.Type.Kind == TypeKind.Struct;

                            if (!isDefaultInstance)
                            {
                                if (isField && !isValidIdentifier)
                                {
                                    Injectors.Add(string.Format("this[{0}] = {1};", name.StartsWith("\"") ? name : AbstractEmitterBlock.ToJavaScript(name, Emitter), value + defValue));
                                }
                                else
                                {
                                    Injectors.Add(string.Format(name.StartsWith("\"") ? interfaceFormat : format, name, value + defValue));
                                }
                            }
                        }
                    }
                }

                count++;

                if (written)
                {
                    continue;
                }
                bool withoutTypeParams = true;
                MemberResolveResult m_rr = null;
                if (member.Entity != null)
                {
                    m_rr = Emitter.Resolver.ResolveNode(member.Entity, Emitter) as MemberResolveResult;
                    if (m_rr != null)
                    {
                        withoutTypeParams = OverloadsCollection.ExcludeTypeParameterForDefinition(m_rr);
                    }
                }

                var mname = member.GetName(Emitter, withoutTypeParams);

                if (TypeInfo.IsEnum && m_rr != null)
                {
                    mname = Emitter.GetEntityName(m_rr.Member);
                }

                bool isValid = Helpers.IsValidIdentifier(mname);
                if (!isValid)
                {
                    if (IsObjectLiteral)
                    {
                        mname = "[" + AbstractEmitterBlock.ToJavaScript(mname, Emitter) + "]";
                    }
                    else
                    {
                        mname = AbstractEmitterBlock.ToJavaScript(mname, Emitter);
                    }
                }

                if (IsObjectLiteral)
                {
                    WriteThis();
                    if (isValid)
                    {
                        WriteDot();
                    }
                    Write(mname);
                    Write(" = ");
                }
                else
                {
                    EnsureComma();
                    XmlToJsDoc.EmitComment(this, member.Entity, null, member.Entity is FieldDeclaration ? member.VarInitializer : null);
                    Write(mname);
                    WriteColon();
                }

                bool close = false;
                if (isProperty && !IsObjectLiteral && !isAutoProperty)
                {
                    var oldTempVars = Emitter.TempVariables;
                    BeginBlock();
                    new VisitorPropertyBlock(Emitter, (PropertyDeclaration)member.Entity).Emit();
                    WriteNewLine();
                    EndBlock();
                    Emitter.Comma = true;
                    Emitter.TempVariables = oldTempVars;
                    continue;
                }

                if (constValue is AstType || constValue is IType)
                {
                    Write("null");

                    if (!isNullable)
                    {
                        var name = member.GetName(Emitter);
                        bool isValidIdentifier = Helpers.IsValidIdentifier(name);
                        var value = constValue is AstType ? Inspector.GetStructDefaultValue((AstType)constValue, Emitter) : Inspector.GetStructDefaultValue((IType)constValue, Emitter);

                        if (!isValidIdentifier)
                        {
                            Injectors.Insert(BeginCounter++, string.Format("this[{0}] = {1};", name.StartsWith("\"") ? name : AbstractEmitterBlock.ToJavaScript(name, Emitter), value));
                        }
                        else
                        {
                            Injectors.Insert(BeginCounter++, string.Format(name.StartsWith("\"") ? interfaceFormat : format, name, value));
                        }
                    }
                }
                else if (write)
                {
                    Write(constValue);
                }
                else if (writeScript)
                {
                    WriteScript(constValue);
                }
                else
                {
                    member.Initializer.AcceptVisitor(Emitter);
                }

                if (close)
                {
                    Write(" }");
                }

                if (IsObjectLiteral)
                {
                    WriteSemiColon(true);
                }

                Emitter.Comma = true;
            }

            if (count > 0 && objectName != null && !IsObjectLiteral)
            {
                WriteNewLine();
                EndBlock();
            }
            else if (beginBlock)
            {
                Emitter.IsNewLine = writer.IsNewLine;
                Emitter.ResetLevel(writer.Level);
                Emitter.Comma = writer.Comma;

                Emitter.Output.Length = pos;
            }

            return count > 0;
        }

        protected virtual bool HasProperties(string objectName, List<TypeConfigItem> members)
        {
            foreach (var member in members)
            {
                object constValue = null;
                bool isPrimitive = false;
                if (member.Initializer is PrimitiveExpression primitiveExpr)
                {
                    isPrimitive = true;
                    constValue = primitiveExpr.Value;
                }

                var isNull = member.Initializer.IsNull || member.Initializer is NullReferenceExpression;

                if (!isNull && !isPrimitive)
                {
                    if (Emitter.Resolver.ResolveNode(member.Initializer, Emitter) is ConstantResolveResult constrr)
                    {
                        isPrimitive = true;
                        constValue = constrr.ConstantValue;
                    }
                }

                if (isNull)
                {
                    return true;
                }

                if (objectName != JS.Fields.PROPERTIES && objectName != JS.Fields.FIELDS && objectName != JS.Fields.EVENTS)
                {
                    if (!isPrimitive || constValue is AstType)
                    {
                        continue;
                    }
                }

                return true;
            }

            return false;
        }

        protected virtual void WriteAlias(string objectName, List<TypeConfigItem> members)
        {
            int pos = Emitter.Output.Length;
            bool oldComma = Emitter.Comma;
            bool oldNewLine = Emitter.IsNewLine;
            bool nonEmpty = false;
            var changedIndenting = false;
            bool newLine = members.Count > 1;

            if (objectName != null)
            {
                EnsureComma();
                Write(objectName);

                WriteColon();
                WriteOpenBracket();

                if (newLine)
                {
                    WriteNewLine();
                    Indent();
                    changedIndenting = true;
                }
            }

            foreach (var member in members)
            {
                if (member.DerivedMember != null)
                {
                    if (EmitMemberAlias(member.DerivedMember, member.InterfaceMember))
                    {
                        nonEmpty = true;
                    }

                    continue;
                }

                var rr = Emitter.Resolver.ResolveNode(member.Entity, Emitter) as MemberResolveResult;

                if (rr == null && member.VarInitializer != null)
                {
                    rr = Emitter.Resolver.ResolveNode(member.VarInitializer, Emitter) as MemberResolveResult;
                }

                if (rr != null)
                {
                    foreach (var interfaceMember in rr.Member.ImplementedInterfaceMembers)
                    {
                        if (EmitMemberAlias(rr.Member, interfaceMember))
                        {
                            nonEmpty = true;
                        }
                    }
                }
            }

            if (newLine)
            {
                WriteNewLine();
                if (changedIndenting)
                {
                    Outdent();
                }
            }

            WriteCloseBracket();

            if (!nonEmpty)
            {
                Emitter.Output.Length = pos;
                Emitter.Comma = oldComma;
                Emitter.IsNewLine = oldNewLine;
            }
        }

        protected bool EmitMemberAlias(IMember member, IMember interfaceMember)
        {
            bool nonEmpty = false;
            if (member.IsShadowing || !member.IsOverride)
            {
                var baseMember = InheritanceHelper.GetBaseMember(member);

                if (baseMember != null && baseMember.ImplementedInterfaceMembers.Contains(interfaceMember))
                {
                    return false;
                }
            }

            var excludeTypeParam = OverloadsCollection.ExcludeTypeParameterForDefinition(member);
            var excludeAliasTypeParam = member.IsExplicitInterfaceImplementation && !excludeTypeParam;
            var pair = false;
            var itypeDef = interfaceMember.DeclaringTypeDefinition;
            if (!member.IsExplicitInterfaceImplementation &&
                MetadataUtils.IsJsGeneric(itypeDef, Emitter) &&
                itypeDef.TypeParameters != null &&
                itypeDef.TypeParameters.Any(typeParameter => typeParameter.Variance != VarianceModifier.Invariant))
            {
                pair = true;
            }

            if (member is IProperty && ((IProperty)member).IsIndexer)
            {
                var property = (IProperty)member;
                if (property.CanGet)
                {
                    nonEmpty = true;
                    EnsureComma();
                    WriteScript(Helpers.GetPropertyRef(member, Emitter, false, false, false, excludeTypeParam));
                    WriteComma();
                    var alias = Helpers.GetPropertyRef(interfaceMember, Emitter, false, false, false, withoutTypeParams: excludeAliasTypeParam);

                    if (pair)
                    {
                        WriteOpenBracket();
                    }

                    if (alias.StartsWith("\""))
                    {
                        Write(alias);
                    }
                    else
                    {
                        WriteScript(alias);
                    }

                    if (pair)
                    {
                        WriteComma();
                        WriteScript(Helpers.GetPropertyRef(interfaceMember, Emitter, withoutTypeParams: true));
                        WriteCloseBracket();
                    }

                    Emitter.Comma = true;
                }

                if (property.CanSet)
                {
                    nonEmpty = true;
                    EnsureComma();
                    WriteScript(Helpers.GetPropertyRef(member, Emitter, true, false, false, excludeTypeParam));
                    WriteComma();
                    var alias = Helpers.GetPropertyRef(interfaceMember, Emitter, true, false, false, withoutTypeParams: excludeAliasTypeParam);

                    if (pair)
                    {
                        WriteOpenBracket();
                    }

                    if (alias.StartsWith("\""))
                    {
                        Write(alias);
                    }
                    else
                    {
                        WriteScript(alias);
                    }

                    if (pair)
                    {
                        WriteComma();
                        WriteScript(Helpers.GetPropertyRef(interfaceMember, Emitter, true, withoutTypeParams: true));
                        WriteCloseBracket();
                    }

                    Emitter.Comma = true;
                }
            }
            else if (member is IEvent ev)
            {
                if (ev.CanAdd)
                {
                    nonEmpty = true;
                    EnsureComma();
                    WriteScript(Helpers.GetEventRef(member, Emitter, false, false, false, excludeTypeParam));
                    WriteComma();
                    var alias = Helpers.GetEventRef(interfaceMember, Emitter, false, false, false, excludeAliasTypeParam);

                    if (pair)
                    {
                        WriteOpenBracket();
                    }

                    if (alias.StartsWith("\""))
                    {
                        Write(alias);
                    }
                    else
                    {
                        WriteScript(alias);
                    }

                    if (pair)
                    {
                        WriteComma();
                        WriteScript(Helpers.GetEventRef(interfaceMember, Emitter, withoutTypeParams: true));
                        WriteCloseBracket();
                    }

                    Emitter.Comma = true;
                }

                if (ev.CanRemove)
                {
                    nonEmpty = true;
                    EnsureComma();
                    WriteScript(Helpers.GetEventRef(member, Emitter, true, false, false, excludeTypeParam));
                    WriteComma();
                    var alias = Helpers.GetEventRef(interfaceMember, Emitter, true, false, false, excludeAliasTypeParam);

                    if (pair)
                    {
                        WriteOpenBracket();
                    }

                    if (alias.StartsWith("\""))
                    {
                        Write(alias);
                    }
                    else
                    {
                        WriteScript(alias);
                    }

                    if (pair)
                    {
                        WriteComma();
                        WriteScript(Helpers.GetEventRef(interfaceMember, Emitter, true, withoutTypeParams: true));
                        WriteCloseBracket();
                    }

                    Emitter.Comma = true;
                }
            }
            else
            {
                nonEmpty = true;
                EnsureComma();
                WriteScript(OverloadsCollection.Create(Emitter, member).GetOverloadName(false, null, excludeTypeOnly: excludeTypeParam));
                WriteComma();
                var alias = OverloadsCollection.Create(Emitter, interfaceMember).GetOverloadName(withoutTypeParams: excludeAliasTypeParam);

                if (pair)
                {
                    WriteOpenBracket();
                }

                if (alias.StartsWith("\""))
                {
                    Write(alias);
                }
                else
                {
                    WriteScript(alias);
                }

                if (pair)
                {
                    WriteComma();
                    WriteScript(OverloadsCollection.Create(Emitter, interfaceMember).GetOverloadName(withoutTypeParams: true));
                    WriteCloseBracket();
                }
            }

            Emitter.Comma = true;
            return nonEmpty;
        }
    }
}