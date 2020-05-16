using H5.Contract.Constants;
using Mono.Cecil;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Object.Net.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using H5.Contract;

namespace H5.Translator
{
    public partial class Emitter
    {
        protected virtual void WrapToModules()
        {
            this.Log.Trace("Wrapping to modules...");

            foreach (var outputPair in Outputs)
            {
                var output = outputPair.Value;

                foreach (var moduleOutputPair in output.ModuleOutput)
                {
                    var module = moduleOutputPair.Key;
                    var moduleOutput = moduleOutputPair.Value;

                    this.Log.Trace("Module " + module.Name + " ...");

                    AbstractEmitterBlock.RemovePenultimateEmptyLines(moduleOutput, true);

                    switch (module.Type)
                    {
                        case ModuleType.CommonJS:
                            WrapToCommonJS(moduleOutput, module, output);
                            break;
                        case ModuleType.UMD:
                            WrapToUMD(moduleOutput, module, output);
                            break;
                        case ModuleType.ES6:
                            WrapToES6(moduleOutput, module, output);
                            break;
                        case ModuleType.AMD:
                        default:
                            WrapToAMD(moduleOutput, module, output);
                            break;
                    }


                }
            }

            this.Log.Trace("Wrapping to modules done");
        }

        protected virtual void WrapToAMD(StringBuilder moduleOutput, Module module, IEmitterOutput output)
        {
            var str = moduleOutput.ToString();
            moduleOutput.Length = 0;

            WriteIndent(moduleOutput, InitialLevel);
            moduleOutput.Append(JS.Funcs.DEFINE + "(");

            if (!module.NoName)
            {
                moduleOutput.Append(ToJavaScript(module.OriginalName));
                moduleOutput.Append(", ");
            }

            var enabledDependecies = GetEnabledDependecies(module, output);

            if (enabledDependecies.Count > 0)
            {
                moduleOutput.Append("[");
                enabledDependecies.Each(md =>
                {
                    moduleOutput.Append(ToJavaScript(md.DependencyName));
                    moduleOutput.Append(", ");
                });
                moduleOutput.Remove(moduleOutput.Length - 2, 2); // remove trailing comma
                moduleOutput.Append("], ");
            }

            moduleOutput.Append("function (");

            if (enabledDependecies.Count > 0)
            {
                enabledDependecies.Each(md =>
                {
                    moduleOutput.Append(md.VariableName.IsNotEmpty() ? md.VariableName : md.DependencyName);
                    moduleOutput.Append(", ");
                });
                moduleOutput.Remove(moduleOutput.Length - 2, 2); // remove trailing comma
            }

            WriteNewLine(moduleOutput, ") {");

            WriteIndent(moduleOutput, InitialLevel);
            WriteNewLine(moduleOutput, Emitter.INDENT + "var " + module.Name + " = { };");
            moduleOutput.Append(str);

            if (!str.Trim().EndsWith(Emitter.NEW_LINE))
            {
                WriteNewLine(moduleOutput);
            }

            WriteIndent(moduleOutput, InitialLevel);
            WriteNewLine(moduleOutput, Emitter.INDENT + "H5.init();");

            WriteIndent(moduleOutput, InitialLevel);
            WriteNewLine(moduleOutput, Emitter.INDENT + "return " + module.Name + ";");
            WriteIndent(moduleOutput, InitialLevel);
            WriteNewLine(moduleOutput, "});");
        }

        private List<IPluginDependency> GetEnabledDependecies(Module module, IEmitterOutput output)
        {
            var dependencies = output.ModuleDependencies;
            var loader = AssemblyInfo.Loader;

            if (dependencies.ContainsKey(module.Name) && dependencies[module.Name].Count > 0)
            {
                return dependencies[module.Name].Where(d => !loader.IsManual(d.DependencyName)).ToList();
            }
            return new List<IPluginDependency>();
        }

        protected virtual void WrapToCommonJS(StringBuilder moduleOutput, Module module, IEmitterOutput output)
        {
            var str = moduleOutput.ToString();
            moduleOutput.Length = 0;

            moduleOutput.Append(Emitter.INDENT);
            moduleOutput.Append("(function (");

            var enabledDependecies = GetEnabledDependecies(module, output);

            if (enabledDependecies.Count > 0)
            {
                enabledDependecies.Each(md =>
                {
                    moduleOutput.Append(md.VariableName.IsNotEmpty() ? md.VariableName : md.DependencyName);
                    moduleOutput.Append(", ");
                });
                moduleOutput.Remove(moduleOutput.Length - 2, 2); // remove trailing comma
            }

            WriteNewLine(moduleOutput, ") {");
            moduleOutput.Append(Emitter.INDENT);
            WriteIndent(moduleOutput, InitialLevel);
            WriteNewLine(moduleOutput, "var " + module.Name + " = { };");
            moduleOutput.Append(str);

            if (!str.Trim().EndsWith(Emitter.NEW_LINE))
            {
                WriteNewLine(moduleOutput);
            }

            WriteIndent(moduleOutput, InitialLevel);
            WriteNewLine(moduleOutput, Emitter.INDENT + "module.exports." + module.Name + " = " + module.Name + ";");
            WriteIndent(moduleOutput, InitialLevel);
            moduleOutput.Append("}) (");

            if (enabledDependecies.Count > 0)
            {
                enabledDependecies.Each(md =>
                {
                    moduleOutput.Append("require(" + ToJavaScript(md.DependencyName) + "), ");
                });
                moduleOutput.Remove(moduleOutput.Length - 2, 2); // remove trailing comma
            }

            WriteNewLine(moduleOutput, ");");
        }

        protected virtual void WrapToUMD(StringBuilder moduleOutput, Module module, IEmitterOutput output)
        {
            var str = moduleOutput.ToString();
            moduleOutput.Length = 0;

            WriteIndent(moduleOutput, 1);
            WriteNewLine(moduleOutput, "(function (root, factory) {");
            WriteIndent(moduleOutput, 2);
            WriteNewLine(moduleOutput, "if (typeof define === 'function' && define.amd) {");
            WriteIndent(moduleOutput, 3);
            moduleOutput.Append(JS.Funcs.DEFINE + "(");
            if (!module.NoName)
            {
                moduleOutput.Append(ToJavaScript(module.OriginalName));
                moduleOutput.Append(", ");
            }

            var enabledDependecies = GetEnabledDependecies(module, output);

            if (enabledDependecies.Count > 0)
            {
                moduleOutput.Append("[");
                enabledDependecies.Each(md =>
                {
                    moduleOutput.Append(ToJavaScript(md.DependencyName));
                    moduleOutput.Append(", ");
                });
                moduleOutput.Remove(moduleOutput.Length - 2, 2); // remove trailing comma
                moduleOutput.Append("], ");
            }
            WriteNewLine(moduleOutput, "factory);");

            WriteIndent(moduleOutput, 2);
            WriteNewLine(moduleOutput, "} else if (typeof module === 'object' && module.exports) {");
            WriteIndent(moduleOutput, 3);
            moduleOutput.Append("module.exports = factory(");
            if (enabledDependecies.Count > 0)
            {
                enabledDependecies.Each(md =>
                {
                    moduleOutput.Append("require(" + ToJavaScript(md.DependencyName) + "), ");
                });
                moduleOutput.Remove(moduleOutput.Length - 2, 2);
            }

            WriteNewLine(moduleOutput, ");");

            WriteIndent(moduleOutput, 2);
            WriteNewLine(moduleOutput, "} else {");
            WriteIndent(moduleOutput, 3);
            moduleOutput.Append("root[" + ToJavaScript(module.OriginalName) + "] = factory(");

            if (enabledDependecies.Count > 0)
            {
                enabledDependecies.Each(md =>
                {
                    moduleOutput.Append("root[" + ToJavaScript(md.DependencyName) + "], ");
                });
                moduleOutput.Remove(moduleOutput.Length - 2, 2); // remove trailing comma
            }

            WriteNewLine(moduleOutput, ");");
            WriteIndent(moduleOutput, 2);
            WriteNewLine(moduleOutput, "}");

            WriteIndent(moduleOutput, 1);
            moduleOutput.Append("}(this, function (");

            if (enabledDependecies.Count > 0)
            {
                enabledDependecies.Each(md =>
                {
                    moduleOutput.Append(md.VariableName ?? md.DependencyName);
                    moduleOutput.Append(", ");
                });
                moduleOutput.Remove(moduleOutput.Length - 2, 2); // remove trailing comma
            }

            moduleOutput.Append(") {");
            WriteNewLine(moduleOutput);

            WriteIndent(moduleOutput, 2);
            WriteNewLine(moduleOutput, "var " + module.Name + " = { };");
            moduleOutput.Append(str);

            if (!str.Trim().EndsWith(Emitter.NEW_LINE))
            {
                WriteNewLine(moduleOutput);
            }

            WriteIndent(moduleOutput, 2);
            WriteNewLine(moduleOutput, "H5.init();");

            WriteIndent(moduleOutput, 2);
            WriteNewLine(moduleOutput, "return " + module.Name + ";");

            WriteIndent(moduleOutput, 1);
            WriteNewLine(moduleOutput, "}));");
        }

        protected virtual void WrapToES6(StringBuilder moduleOutput, Module module, IEmitterOutput output)
        {
            var str = moduleOutput.ToString();
            moduleOutput.Length = 0;

            moduleOutput.Append(Emitter.INDENT);
            WriteNewLine(moduleOutput, "(function () {");

            moduleOutput.Append(Emitter.INDENT);
            WriteIndent(moduleOutput, InitialLevel);
            WriteNewLine(moduleOutput, "var " + module.Name + " = { };");

            var enabledDependecies = GetEnabledDependecies(module, output);

            if (enabledDependecies.Count > 0)
            {
                enabledDependecies.Each(md =>
                {
                    moduleOutput.Append(Emitter.INDENT);
                    WriteIndent(moduleOutput, InitialLevel);
                    WriteNewLine(moduleOutput, "import " + (md.VariableName.IsNotEmpty() ? md.VariableName : md.DependencyName) + " from " + ToJavaScript(md.DependencyName) + ";");
                });
            }

            moduleOutput.Append(str);

            if (!str.Trim().EndsWith(Emitter.NEW_LINE))
            {
                WriteNewLine(moduleOutput);
            }

            WriteIndent(moduleOutput, InitialLevel);
            WriteNewLine(moduleOutput, Emitter.INDENT + "export {" + module.Name + "};");
            WriteIndent(moduleOutput, InitialLevel);
            moduleOutput.Append("}) (");

            WriteNewLine(moduleOutput, ");");
        }
    }
}