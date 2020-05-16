using H5.Contract;
using H5.Contract.Constants;
using Mono.Cecil;
using Object.Net.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H5.Translator.TypeScript
{
    public class ClassBlock : TypeScriptBlock
    {
        public ClassBlock(IEmitter emitter, ITypeInfo typeInfo)
            : base(emitter, typeInfo.TypeDeclaration)
        {
            TypeInfo = typeInfo;
        }

        public ClassBlock(IEmitter emitter, ITypeInfo typeInfo, IEnumerable<ITypeInfo> nestedTypes, IEnumerable<ITypeInfo> allTypes, string ns)
            : this(emitter, typeInfo)
        {
            NestedTypes = nestedTypes;
            AllTypes = allTypes;
            Namespace = ns;
        }

        public ITypeInfo TypeInfo { get; set; }

        public bool IsGeneric { get; set; }

        public string JsName { get; set; }

        public string Namespace { get; set; }

        public IEnumerable<ITypeInfo> NestedTypes { get; private set; }

        public IEnumerable<ITypeInfo> AllTypes { get; private set; }

        public int Position;

        protected override void DoEmit()
        {
            XmlToJsDoc.EmitComment(this, Emitter.Translator.EmitNode);

            if (TypeInfo.IsEnum && TypeInfo.ParentType == null)
            {
                new EnumBlock(Emitter, TypeInfo, Namespace).Emit();
            }
            else
            {
                EmitClassHeader();
                EmitBlock();
                EmitClassEnd();
            }
        }

        protected virtual void EmitClassHeader()
        {
            var typeDef = Emitter.GetTypeDefinition();
            string name = Emitter.Validator.GetCustomTypeName(typeDef, Emitter, true, false);
            IsGeneric = typeDef.GenericParameters.Count > 0;

            if (name.IsEmpty())
            {
                name = H5Types.ToTypeScriptName(TypeInfo.Type, Emitter, false, true);

                if (IsGeneric)
                {
                    DefName = H5Types.ToTypeScriptName(TypeInfo.Type, Emitter, true, true);
                }
            }
            else if (IsGeneric)
            {
                DefName = Emitter.Validator.GetCustomTypeName(typeDef, Emitter, true);
            }

            Write("interface ");

            JsName = name;
            Write(JsName);

            string extend = GetTypeHierarchy();

            if (extend.IsNotEmpty() && !TypeInfo.IsEnum)
            {
                Write(" extends ");
                Write(extend);
            }

            WriteSpace();
            BeginBlock();
            Position = Emitter.Output.Length;
        }

        public string DefName { get; set; }

        private string GetTypeHierarchy()
        {
            StringBuilder sb = new StringBuilder();

            var list = new List<string>();

            foreach (var t in TypeInfo.GetBaseTypes(Emitter))
            {
                var name = H5Types.ToTypeScriptName(t, Emitter);

                list.Add(name);
            }

            if (list.Count > 0 && list[0] == JS.Types.System.Object.NAME)
            {
                list.RemoveAt(0);
            }

            if (list.Count == 0)
            {
                return "";
            }

            bool needComma = false;

            foreach (var item in list)
            {
                if (needComma)
                {
                    sb.Append(",");
                }

                needComma = true;
                sb.Append(item);
            }

            return sb.ToString();
        }

        protected virtual void EmitBlock()
        {
            var typeDef = Emitter.GetTypeDefinition();

            new MemberBlock(Emitter, TypeInfo, false).Emit();
            if (Emitter.TypeInfo.TypeDeclaration.ClassType != ICSharpCode.NRefactory.CSharp.ClassType.Interface)
            {
                if (Position != Emitter.Output.Length && !Emitter.IsNewLine)
                {
                    WriteNewLine();
                }

                EndBlock();

                WriteNewLine();

                Write("interface ");

                Write(DefName ?? JsName);

                Write("Func extends Function ");

                if (IsGeneric)
                {
                    BeginBlock();
                    Write("<");
                    var comma = false;
                    foreach (var p in typeDef.GenericParameters)
                    {
                        if (comma)
                        {
                            WriteComma();
                        }
                        Write(p.Name);
                        comma = true;
                    }
                    Write(">");

                    WriteOpenParentheses();
                    comma = false;
                    foreach (var p in typeDef.GenericParameters)
                    {
                        if (comma)
                        {
                            WriteComma();
                        }
                        Write(JS.Vars.D + p.Name);
                        WriteColon();
                        Write(JS.Types.TypeRef);
                        Write("<");
                        Write(p.Name);
                        Write(">");
                        comma = true;
                    }

                    WriteCloseParentheses();
                    WriteColon();
                }

                BeginBlock();

                Write(JS.Fields.PROTOTYPE + ": ");
                Write(JsName);
                WriteSemiColon();
                WriteNewLine();
                WriteNestedDefs();
                Position = Emitter.Output.Length;

                if (Emitter.TypeInfo.TypeDeclaration.ClassType != ICSharpCode.NRefactory.CSharp.ClassType.Interface)
                {
                    if (!TypeInfo.IsEnum)
                    {
                        new ConstructorBlock(Emitter, TypeInfo).Emit();
                    }
                    new MemberBlock(Emitter, TypeInfo, true).Emit();
                }
            }
        }

        protected virtual void WriteNestedDefs()
        {
            if (NestedTypes != null)
            {
                foreach (var nestedType in NestedTypes)
                {

                    var typeDef = Emitter.GetTypeDefinition(nestedType.Type);

                    if (typeDef.IsInterface || Emitter.Validator.IsObjectLiteral(typeDef))
                    {
                        continue;
                    }

                    string customName = Emitter.Validator.GetCustomTypeName(typeDef, Emitter, true);
                    string defName = customName;

                    if (defName.IsEmpty())
                    {
                        defName = H5Types.ToTypeScriptName(nestedType.Type, Emitter, true);
                        Write(H5Types.ToTypeScriptName(nestedType.Type, Emitter, true, true));
                    }
                    else
                    {
                        Write(defName);
                    }

                    if (typeDef.IsEnum)
                    {
                        var parentTypeDef = Emitter.GetTypeDefinition();
                        string parentName = Emitter.Validator.GetCustomTypeName(parentTypeDef, Emitter, false, false);
                        if (parentName.IsEmpty())
                        {
                            parentName = TypeInfo.Type.Name;
                        }
                        defName = parentName + "." + H5Types.ToTypeScriptName(nestedType.Type, Emitter, false, true);
                    }

                    WriteColon();

                    Write(defName + "Func");
                    WriteSemiColon();
                    WriteNewLine();
                }
            }
        }

        protected virtual void EmitClassEnd()
        {
            if (Position != Emitter.Output.Length && !Emitter.IsNewLine)
            {
                WriteNewLine();
            }

            var isInterface = Emitter.TypeInfo.TypeDeclaration.ClassType == ICSharpCode.NRefactory.CSharp.ClassType.Interface;
            EndBlock();

            if (IsGeneric && !isInterface)
            {
                WriteNewLine();
                EndBlock();
            }

            if (TypeInfo.ParentType == null && !isInterface)
            {
                string name = H5Types.ToTypeScriptName(TypeInfo.Type, Emitter, true, true);
                WriteNewLine();

                if (Namespace == null)
                {
                    Write("declare ");
                }

                Write("var ");
                Write(name);
                WriteColon();

                Write(name + "Func");

                WriteSemiColon();
            }

            WriteNestedTypes();
        }

        protected virtual void WriteNestedTypes()
        {
            if (NestedTypes != null && NestedTypes.Any())
            {
                if (!Emitter.IsNewLine)
                {
                    WriteNewLine();
                }

                var typeDef = Emitter.GetTypeDefinition();
                string name = Emitter.Validator.GetCustomTypeName(typeDef, Emitter, true);
                if (name.IsEmpty())
                {
                    name = H5Types.ToJsName(TypeInfo.Type, Emitter, true, true, nomodule: true);
                }

                Write("module ");
                Write(name);
                WriteSpace();
                BeginBlock();

                var last = NestedTypes.LastOrDefault();
                foreach (var nestedType in NestedTypes)
                {
                    Emitter.Translator.EmitNode = nestedType.TypeDeclaration;

                    if (nestedType.IsObjectLiteral)
                    {
                        continue;
                    }

                    ITypeInfo typeInfo;

                    if (Emitter.TypeInfoDefinitions.ContainsKey(nestedType.Key))
                    {
                        typeInfo = Emitter.TypeInfoDefinitions[nestedType.Key];

                        nestedType.Module = typeInfo.Module;
                        nestedType.FileName = typeInfo.FileName;
                        nestedType.Dependencies = typeInfo.Dependencies;
                        typeInfo = nestedType;
                    }
                    else
                    {
                        typeInfo = nestedType;
                    }

                    Emitter.TypeInfo = nestedType;

                    var nestedTypes = AllTypes.Where(t => t.ParentType == nestedType);
                    new ClassBlock(Emitter, Emitter.TypeInfo, nestedTypes, AllTypes, Namespace).Emit();
                    WriteNewLine();
                    if (nestedType != last)
                    {
                        WriteNewLine();
                    }
                }

                EndBlock();
            }
        }
    }
}