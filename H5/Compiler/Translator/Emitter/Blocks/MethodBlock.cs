using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Semantics;
using Newtonsoft.Json;

namespace H5.Translator
{
    public class MethodBlock : AbstractEmitterBlock
    {
        public MethodBlock(IEmitter emitter, ITypeInfo typeInfo, bool staticBlock)
            : base(emitter, typeInfo.TypeDeclaration)
        {
            Emitter = emitter;
            TypeInfo = typeInfo;
            StaticBlock = staticBlock;
        }

        public ITypeInfo TypeInfo { get; set; }

        public bool StaticBlock { get; set; }

        protected override void DoEmit()
        {
            if (StaticBlock)
            {
                EmitMethods(TypeInfo.StaticMethods, TypeInfo.StaticProperties, TypeInfo.Operators);
            }
            else
            {
                EmitMethods(TypeInfo.InstanceMethods, TypeInfo.InstanceProperties, null);
            }
        }

        protected virtual void EmitMethods(Dictionary<string, List<MethodDeclaration>> methods, Dictionary<string, List<EntityDeclaration>> properties, Dictionary<OperatorType, List<OperatorDeclaration>> operators)
        {
            int pos = Emitter.Output.Length;
            var writerInfo = SaveWriter();

            string globalTarget = H5Types.GetGlobalTarget(TypeInfo.Type.GetDefinition(), TypeInfo.TypeDeclaration);

            if (globalTarget == null)
            {
                EnsureComma();
                Write(JS.Fields.METHODS);
                WriteColon();
                BeginBlock();
            }

            int checkPos = Emitter.Output.Length;

            var names = new List<string>(properties.Keys);

            foreach (var name in names)
            {
                var props = properties[name];

                foreach (var prop in props)
                {
                    if (prop is PropertyDeclaration)
                    {
                        Emitter.VisitPropertyDeclaration((PropertyDeclaration)prop);
                    }
                    else if (prop is CustomEventDeclaration)
                    {
                        Emitter.VisitCustomEventDeclaration((CustomEventDeclaration)prop);
                    }
                    else if (prop is IndexerDeclaration)
                    {
                        Emitter.VisitIndexerDeclaration((IndexerDeclaration)prop);
                    }
                }
            }

            names = new List<string>(methods.Keys);

            foreach (var name in names)
            {
                EmitMethodsGroup(methods[name]);
            }

            if (operators != null)
            {
                var ops = new List<OperatorType>(operators.Keys);

                foreach (var op in ops)
                {
                    EmitOperatorGroup(operators[op]);
                }
            }

            if (TypeInfo.ClassType == ClassType.Struct)
            {
                if (!StaticBlock)
                {
                    EmitStructMethods();
                }
                else
                {
                    string structName = H5Types.ToJsName(TypeInfo.Type, Emitter);
                    if (TypeInfo.Type.TypeArguments.Count > 0 &&
                        !Helpers.IsIgnoreGeneric(TypeInfo.Type, Emitter))
                    {
                        structName = "(" + structName + ")";
                    }

                    EnsureComma();
                    Write(JS.Funcs.GETDEFAULTVALUE + ": function () { return new " + structName + "(); }");
                    Emitter.Comma = true;
                }
            }
            else if (StaticBlock)
            {
                var ctor = TypeInfo.Type.GetConstructors().FirstOrDefault(c => c.Parameters.Count == 0 && Emitter.GetInline(c) != null);

                if (ctor != null)
                {
                    var code = Emitter.GetInline(ctor);
                    EnsureComma();
                    Write(JS.Funcs.GETDEFAULTVALUE + ": function () ");
                    BeginBlock();
                    Write("return ");
                    var argsInfo = new ArgumentsInfo(Emitter, ctor);
                    new InlineArgumentsBlock(Emitter, argsInfo, code).Emit();
                    Write(";");
                    WriteNewLine();
                    EndBlock();
                    Emitter.Comma = true;
                }
            }

            if (globalTarget == null)
            {
                if (checkPos == Emitter.Output.Length)
                {
                    Emitter.IsNewLine = writerInfo.IsNewLine;
                    Emitter.ResetLevel(writerInfo.Level);
                    Emitter.Comma = writerInfo.Comma;
                    Emitter.Output.Length = pos;
                }
                else
                {
                    WriteNewLine();
                    EndBlock();
                }
            }
        }

        protected virtual void EmitStructMethods()
        {
            var typeDef = Emitter.GetTypeDefinition();
            string structName = H5Types.ToJsName(TypeInfo.Type, Emitter);

            bool immutable = Emitter.Validator.IsImmutableType(typeDef);

            if (!immutable)
            {
                var mutableFields = TypeInfo.Type.GetFields(f => !f.IsConst, GetMemberOptions.IgnoreInheritedMembers);
                var autoProps = typeDef.Properties.Where(Helpers.IsAutoProperty);
                var autoEvents = TypeInfo.Type.GetEvents(null, GetMemberOptions.IgnoreInheritedMembers);
                immutable = !mutableFields.Any() && !autoProps.Any() && !autoEvents.Any();
            }

            var fields = TypeInfo.InstanceConfig.Fields;
            var props = TypeInfo.InstanceConfig.Properties.Where(ent =>
            {
                return ent.Entity is PropertyDeclaration p && p.Getter != null && p.Getter.Body.IsNull && p.Setter != null && p.Setter.Body.IsNull;
            });

            var list = fields.ToList();
            list.AddRange(props);

            if (list.Count == 0)
            {
                EnsureComma();
                Write(JS.Funcs.CLONE + ": function (to) { return this; }");
                Emitter.Comma = true;
                return;
            }

            if (!TypeInfo.InstanceMethods.ContainsKey(CS.Methods.GETHASHCODE))
            {
                EnsureComma();
                Write(JS.Funcs.GETHASHCODE + ": function () ");
                BeginBlock();
                Write("var h = " + JS.Funcs.H5_ADDHASH + "([");

                var nameHashValue = new HashHelper().GetDeterministicHash(TypeInfo.Name);
                Write(nameHashValue);

                foreach (var field in list)
                {
                    string fieldName = field.GetName(Emitter);
                    Write(", this." + fieldName);
                }

                Write("]);");

                WriteNewLine();
                Write("return h;");
                WriteNewLine();
                EndBlock();
                Emitter.Comma = true;
            }

            if (!TypeInfo.InstanceMethods.ContainsKey(CS.Methods.EQUALS))
            {
                EnsureComma();
                Write(JS.Funcs.EQUALS + ": function (o) ");
                BeginBlock();
                Write("if (!" + JS.Types.H5.IS + "(o, ");
                Write(structName);
                Write(")) ");
                BeginBlock();
                Write("return false;");
                WriteNewLine();
                EndBlock();
                WriteNewLine();
                Write("return ");

                bool and = false;

                foreach (var field in list)
                {
                    string fieldName = field.GetName(Emitter);

                    if (and)
                    {
                        Write(" && ");
                    }

                    and = true;

                    Write(JS.Funcs.H5_EQUALS + "(this.");
                    Write(fieldName);
                    Write(", o.");
                    Write(fieldName);
                    Write(")");
                }

                Write(";");
                WriteNewLine();
                EndBlock();
                Emitter.Comma = true;
            }

            EnsureComma();

            if (immutable)
            {
                Write(JS.Funcs.CLONE + ": function (to) { return this; }");
            }
            else
            {
                Write(JS.Funcs.CLONE + ": function (to) ");
                BeginBlock();
                Write("var s = to || new ");
                if (TypeInfo.Type.TypeArguments.Count > 0 && !Helpers.IsIgnoreGeneric(TypeInfo.Type, Emitter))
                {
                    structName = "(" + structName + ")";
                }
                Write(structName);
                Write("();");

                foreach (var field in list)
                {
                    WriteNewLine();
                    string fieldName = field.GetName(Emitter);

                    Write("s.");
                    Write(fieldName);
                    Write(" = ");

                    int insertPosition = Emitter.Output.Length;
                    Write("this.");
                    Write(fieldName);

                    var rr = Emitter.Resolver.ResolveNode(field.Entity, Emitter) as MemberResolveResult;

                    if (rr == null && field.VarInitializer != null)
                    {
                        rr = Emitter.Resolver.ResolveNode(field.VarInitializer, Emitter) as MemberResolveResult;
                    }

                    if (rr != null)
                    {
                        Helpers.CheckValueTypeClone(rr, null, this, insertPosition);
                    }

                    Write(";");
                }

                WriteNewLine();
                Write("return s;");
                WriteNewLine();
                EndBlock();
            }

            Emitter.Comma = true;
        }

        protected void EmitEventAccessor(EventDeclaration e, VariableInitializer evtVar, bool add)
        {
            string name = evtVar.Name;

            Write(Helpers.GetAddOrRemove(add), name, " : ");
            WriteFunction();
            WriteOpenParentheses();
            Write("value");
            WriteCloseParentheses();
            WriteSpace();
            BeginBlock();
            WriteThis();
            WriteDot();
            Write(Emitter.GetEntityName(e));
            Write(" = ");
            Write(add ? JS.Funcs.H5_COMBINE : JS.Funcs.H5_REMOVE);
            WriteOpenParentheses();
            WriteThis();
            WriteDot();
            Write(Emitter.GetEntityName(e));
            WriteComma();
            WriteSpace();
            Write("value");
            WriteCloseParentheses();
            WriteSemiColon();
            WriteNewLine();
            EndBlock();
        }

        protected virtual void EmitMethodsGroup(List<MethodDeclaration> group)
        {
            if (group.Count == 1)
            {
                if ((!group[0].Body.IsNull || Emitter.GetScript(group[0]) != null) && (!StaticBlock || !Helpers.IsEntryPointMethod(Emitter, group[0])))
                {
                    Emitter.VisitMethodDeclaration(group[0]);
                }
            }
            else
            {
                var typeDef = Emitter.GetTypeDefinition();
                var name = group[0].Name;
                var methodsDef = typeDef.Methods.Where(m => m.Name == name);
                Emitter.MethodsGroup = methodsDef;
                Emitter.MethodsGroupBuilder = new Dictionary<int, StringBuilder>();

                foreach (var method in group)
                {
                    if (!method.Body.IsNull && (!StaticBlock || !Helpers.IsEntryPointMethod(Emitter, group[0])))
                    {
                        Emitter.VisitMethodDeclaration(method);
                    }
                }

                Emitter.MethodsGroup = null;
                Emitter.MethodsGroupBuilder = null;
            }
        }

        protected virtual void EmitOperatorGroup(List<OperatorDeclaration> group)
        {
            if (group.Count == 1)
            {
                if (!group[0].Body.IsNull)
                {
                    Emitter.VisitOperatorDeclaration(group[0]);
                }
            }
            else
            {
                var name = group[0].Name;
                var methodsDef = Emitter.GetTypeDefinition().Methods.Where(m => m.Name == name);
                Emitter.MethodsGroup = methodsDef;
                Emitter.MethodsGroupBuilder = new Dictionary<int, StringBuilder>();

                foreach (var method in group)
                {
                    if (!method.Body.IsNull)
                    {
                        Emitter.VisitOperatorDeclaration(method);
                    }
                }

                Emitter.MethodsGroup = null;
                Emitter.MethodsGroupBuilder = null;
            }
        }
    }
}