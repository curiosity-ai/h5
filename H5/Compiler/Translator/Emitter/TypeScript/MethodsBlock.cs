using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using Object.Net.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace H5.Translator.TypeScript
{
    public class MethodsBlock : TypeScriptBlock
    {
        public MethodsBlock(IEmitter emitter, ITypeInfo typeInfo, bool staticBlock)
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
            var names = new List<string>(properties.Keys);
            var fields = StaticBlock ? TypeInfo.StaticConfig.Fields : TypeInfo.InstanceConfig.Fields;

            foreach (var name in names)
            {
                if (fields.Any(f => f.Name == name))
                {
                    continue;
                }

                var props = properties[name];

                foreach (var prop in props)
                {
                    if (prop is PropertyDeclaration)
                    {
                        new PropertyBlock(Emitter, (PropertyDeclaration)prop).Emit();
                    }
                    else if (prop is CustomEventDeclaration)
                    {
                        new CustomEventBlock(Emitter, (CustomEventDeclaration)prop).Emit();
                    }
                    else if (prop is IndexerDeclaration)
                    {
                        new IndexerBlock(Emitter, (IndexerDeclaration)prop).Emit();
                    }
                }
            }

            names = new List<string>(methods.Keys);

            foreach (var name in names)
            {
                var group = methods[name];

                foreach (var method in group)
                {
                    if ((!method.Body.IsNull || Emitter.GetScript(method) != null) || Emitter.TypeInfo.TypeDeclaration.ClassType == ClassType.Interface)
                    {
                        new MethodBlock(Emitter, method).Emit();
                    }
                }
            }

            var abstractMethods = TypeInfo.TypeDeclaration.Members.Where(m =>
            {
                return m is MethodDeclaration && m.HasModifier(Modifiers.Abstract) && !(StaticBlock ^ m.HasModifier(Modifiers.Static));
            }).Cast<MethodDeclaration>();

            foreach (var method in abstractMethods)
            {
                new MethodBlock(Emitter, method).Emit();
            }

            if (operators != null)
            {
                var ops = new List<OperatorType>(operators.Keys);

                foreach (var op in ops)
                {
                    var group = operators[op];

                    foreach (var o in group)
                    {
                        if (!o.Body.IsNull && Emitter.TypeInfo.TypeDeclaration.ClassType != ClassType.Interface)
                        {
                            new OperatorBlock(Emitter, o).Emit();
                        }
                    }
                }
            }

            if (TypeInfo.ClassType == ClassType.Struct && !StaticBlock)
            {
                EmitStructMethods();
            }
        }

        protected virtual void EmitStructMethods()
        {
            var typeDef = Emitter.GetTypeDefinition();
            string structName = H5Types.ToTypeScriptName(TypeInfo.Type, Emitter);

            if (TypeInfo.InstanceConfig.Fields.Count == 0)
            {
                Write(JS.Funcs.CLONE + "(to");
                WriteColon();
                Write(structName);
                WriteCloseParentheses();
                WriteColon();
                Write(structName);
                WriteSemiColon();
                WriteNewLine();
                return;
            }

            if (!TypeInfo.InstanceMethods.ContainsKey(CS.Methods.GETHASHCODE))
            {
                Write(JS.Funcs.GETHASHCODE + "()");
                WriteColon();
                Write("number");
                WriteSemiColon();
                WriteNewLine();
            }

            if (!TypeInfo.InstanceMethods.ContainsKey(CS.Methods.EQUALS))
            {
                Write(JS.Funcs.EQUALS + "(o");
                WriteColon();
                Write(structName);
                WriteCloseParentheses();
                WriteColon();
                Write("boolean");
                WriteSemiColon();
                WriteNewLine();
            }

            Write(JS.Funcs.CLONE + "(to");
            WriteColon();
            Write(structName);
            WriteCloseParentheses();
            WriteColon();
            Write(structName);
            WriteSemiColon();
            WriteNewLine();
        }
    }
}