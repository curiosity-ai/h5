using H5.Contract;
using H5.Contract.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace H5.Translator.TypeScript
{
    public class EmitBlock : TypeScriptBlock
    {
        private class OutputKey
        {
            public OutputKey(string key, Module module, string ns)
            {
                Key = key;
                Module = module;
                Namespace = ns;
            }

            public string Key { get; set; }

            public Module Module { get; set; }

            public string Namespace { get; set; }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Key != null ? Key.GetHashCode() : 0) * 397) ^ (Module != null ? Module.GetHashCode() : 0);
                }
            }

            public override bool Equals(object obj)
            {
                return !(obj is OutputKey other) ? false : Key == other.Key && (Module == null && other.Module == null || Module != null && Module.Equals(other.Module));
            }
        }

        // This ensures a constant line separator throughout the application
        private const char newLine = XmlToJSConstants.DEFAULT_LINE_SEPARATOR;

        private Dictionary<OutputKey, StringBuilder> Outputs { get; set; }

        private string ns = null;
        private OutputKey outputKey = null;

        public EmitBlock(IEmitter emitter)
            : base(emitter, null)
        {
            Emitter = emitter;
        }

        protected virtual StringBuilder GetOutputForType(ITypeInfo typeInfo)
        {
            var info = H5Types.GetNamespaceFilename(typeInfo, Emitter);
            var ns = info.Item1;
            var fileName = info.Item2;
            var module = info.Item3;

            StringBuilder output = null;
            OutputKey key = new OutputKey(fileName, module, ns);

            if (this.ns != null && (this.ns != ns || outputKey != null && !outputKey.Equals(key)))
            {
                EndBlock();
                WriteNewLine();
            }

            this.ns = ns;
            outputKey = key;

            if (Outputs.ContainsKey(key))
            {
                output = Outputs[key];
            }
            else
            {
                if (Emitter.Output != null)
                {
                    InsertDependencies(Emitter.Output);
                }

                output = new StringBuilder();
                Emitter.Output = output;

                if (ns != null)
                {
                    if (module == null || module.Type == ModuleType.UMD)
                    {
                        output.Append("declare ");
                    }

                    output.Append($"namespace {ns} ");
                    BeginBlock();
                }

                Outputs.Add(key, output);
                Emitter.CurrentDependencies = new List<IModuleDependency>();
            }

            return output;
        }

        protected virtual void InsertDependencies(StringBuilder sb)
        {
            if (Emitter.CurrentDependencies != null && Emitter.CurrentDependencies.Count > 0)
            {
                StringBuilder depSb = new StringBuilder();
                foreach (var d in Emitter.CurrentDependencies)
                {
                    depSb.Append($@"/// <reference path=""./{d.DependencyName}.d.ts"" />");
                    depSb.Append(newLine);
                }

                sb.Insert(0, $"{depSb}{newLine}");
                Emitter.CurrentDependencies.Clear();
            }
        }

        private void TransformOutputs()
        {
            if (Emitter.Outputs.Count == 0)
            {
                return;
            }

            var withoutModuleOutputs = Outputs.Where(o => o.Key.Module == null).ToList();
            var withModuleOutputs = Outputs.Where(o => o.Key.Module != null).ToList();

            var nonModuleOutputs = withoutModuleOutputs.GroupBy(o => o.Key.Key).ToDictionary(t => t.Key, t => string.Join(newLine.ToString(), t.Select(r => r.Value.ToString()).ToList()));
            var outputs = withModuleOutputs.GroupBy(o => o.Key.Module).ToDictionary(t => t.Key, t => string.Join(newLine.ToString(), t.Select(r => r.Value.ToString()).ToList()));

            if (Emitter.AssemblyInfo.OutputBy == OutputBy.Project)
            {
                var fileName = $"{Path.GetFileNameWithoutExtension(Emitter.Outputs.First().Key)}{Files.Extensions.DTS}";
                var e = new EmitterOutput(fileName);

                foreach (var item in nonModuleOutputs)
                {
                    e.NonModuletOutput.Append($"{item.Value}{newLine}");
                }

                foreach (var item in outputs)
                {
                    e.NonModuletOutput.Append($"{WrapModule(item)}{newLine}");
                }

                Emitter.Outputs.Add(fileName, e);
            }
            else
            {
                foreach (var item in nonModuleOutputs)
                {
                    var fileName = $"{item.Key}{Files.Extensions.DTS}";
                    var e = new EmitterOutput(fileName);
                    e.NonModuletOutput.Append(item.Value.ToString());
                    Emitter.Outputs.Add(fileName, e);
                }

                foreach (var item in outputs)
                {
                    var fileName = $"{item.Key.ExportAsNamespace}{Files.Extensions.DTS}";
                    var e = new EmitterOutput(fileName);
                    e.NonModuletOutput.Append(WrapModule(item));
                    Emitter.Outputs.Add(fileName, e);
                }
            }
        }

        private string WrapModule(KeyValuePair<Module, string> item)
        {
            StringBuilder sb = new StringBuilder();

            if (item.Key.Type == ModuleType.AMD || item.Key.Type == ModuleType.CommonJS)
            {
                sb.Append($"declare module \"{item.Key.ExportAsNamespace}\" {{");
                sb.Append(H5.Translator.Emitter.NEW_LINE);
                sb.Append($"    {WriteIndentToString(item.Value, 1)}");
                sb.Append(H5.Translator.Emitter.NEW_LINE);
                sb.Append("}");
            }
            else if (item.Key.Type == ModuleType.UMD)
            {
                sb.Append(item.Value);
                sb.Append(H5.Translator.Emitter.NEW_LINE);
                sb.Append($"declare module \"{item.Key.ExportAsNamespace}\" {{");
                sb.Append(H5.Translator.Emitter.NEW_LINE);
                sb.Append($"    export = {item.Key.ExportAsNamespace};");
                sb.Append(H5.Translator.Emitter.NEW_LINE);
                sb.Append("}");
            }
            else
            {
                sb.Append(item.Value);
            }

            sb.Append(H5.Translator.Emitter.NEW_LINE);

            return sb.ToString();
        }

        protected override void DoEmit()
        {
            Emitter.Tag = "TS";
            Emitter.Writers = new Stack<IWriter>();
            Outputs = new Dictionary<OutputKey, StringBuilder>();

            var types = Emitter.Types.ToArray();
            Array.Sort(types, (t1, t2) =>
            {
                var t1ns = H5Types.GetNamespaceFilename(t1, Emitter);
                var t2ns = H5Types.GetNamespaceFilename(t2, Emitter);

                if (t1ns.Item1 == null && t2ns.Item1 == null)
                {
                    return 0;
                }

                if (t1ns.Item1 == null)
                {
                    return -1;
                }

                if (t2ns.Item1 == null)
                {
                    return 1;
                }

                var key1 = t1ns.Item1 + (t1ns.Item3 != null ? t1ns.Item3.ExportAsNamespace : "");
                var key2 = t2ns.Item1 + (t2ns.Item3 != null ? t2ns.Item3.ExportAsNamespace : "");

                return t1ns.Item1.CompareTo(t2ns.Item1);
            });
            Emitter.InitEmitter();

            bool nsExists = false;
            var last = types.LastOrDefault();
            foreach (var type in types)
            {
                if (!nsExists)
                {
                    var tns = H5Types.GetNamespaceFilename(type, Emitter);

                    if (tns.Item1 != null)
                    {
                        nsExists = true;
                    }
                }

                if (type.ParentType != null)
                {
                    continue;
                }

                Emitter.Translator.EmitNode = type.TypeDeclaration;

                if (type.IsObjectLiteral)
                {
                    continue;
                }

                ITypeInfo typeInfo;

                if (Emitter.TypeInfoDefinitions.ContainsKey(type.Key))
                {
                    typeInfo = Emitter.TypeInfoDefinitions[type.Key];

                    type.Module = typeInfo.Module;
                    type.FileName = typeInfo.FileName;
                    type.Dependencies = typeInfo.Dependencies;
                    typeInfo = type;
                }
                else
                {
                    typeInfo = type;
                }

                Emitter.TypeInfo = type;
                type.JsName = H5Types.ToJsName(type.Type, Emitter, true);

                Emitter.Output = GetOutputForType(typeInfo);
                var nestedTypes = types.Where(t => t.ParentType == type);
                new ClassBlock(Emitter, Emitter.TypeInfo, nestedTypes, types, ns).Emit();
                WriteNewLine();

                if (type != last)
                {
                    WriteNewLine();
                }
            }

            InsertDependencies(Emitter.Output);
            if (outputKey != null && nsExists)
            {
                EndBlock();
            }

            TransformOutputs();
        }
    }
}