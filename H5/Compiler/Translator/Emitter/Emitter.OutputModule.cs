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
using Mosaik.Core;
using Microsoft.Extensions.Logging;

namespace H5.Translator
{
    public partial class Emitter
    {
        protected void WrapToModules()
        {
            using (var m = new Measure(Logger, "Wrapping to modules", logLevel: LogLevel.Trace))
            {
                var isEsm = AssemblyInfo.OutputModuleType == OutputModuleType.ESM;

                foreach (var outputPair in Outputs)
                {
                    CancellationToken.ThrowIfCancellationRequested();

                    var output = outputPair.Value;
                    int k = 0;
                    foreach (var moduleOutputPair in output.ModuleOutput)
                    {
                        var module = moduleOutputPair.Key;
                        var moduleOutput = moduleOutputPair.Value;

                        m.SetOperations(++k).EmitPartial($"Processing Module '{module.Name}");

                        AbstractEmitterBlock.RemovePenultimateEmptyLines(moduleOutput, true);

                        if (isEsm)
                        {
                            WrapToESM(moduleOutput, module, output);
                        }
                        else
                        {
                            WrapToH5Define(moduleOutput, module, output);
                        }
                    }
                }
            }
        }

        private List<IModuleDependency> GetEnabledDependecies(Module module, IEmitterOutput output)
        {
            var dependencies = output.ModuleDependencies;
            var loader = AssemblyInfo.Loader;

            if (dependencies.ContainsKey(module.Name) && dependencies[module.Name].Count > 0)
            {
                return dependencies[module.Name].Where(d => !loader.IsManual(d.DependencyName)).ToList();
            }
            return new List<IModuleDependency>();
        }

        /// <summary>
        /// Legacy <c>H5.define</c> module wrap. Wraps the module content in an IIFE that
        /// builds the named module object and exposes it through the existing global
        /// H5 namespace. Used when <see cref="OutputModuleType.Default"/> is selected.
        /// </summary>
        protected void WrapToH5Define(StringBuilder moduleOutput, Module module, IEmitterOutput output)
        {
            var str = moduleOutput.ToString();
            moduleOutput.Length = 0;

            moduleOutput.Append(INDENT);
            WriteNewLine(moduleOutput, "(function () {");

            WriteIndent(moduleOutput, InitialLevel);
            WriteNewLine(moduleOutput, INDENT + "var " + module.Name + " = { };");
            moduleOutput.Append(str);

            if (!str.Trim().EndsWith(NEW_LINE))
            {
                WriteNewLine(moduleOutput);
            }

            WriteIndent(moduleOutput, InitialLevel);
            WriteNewLine(moduleOutput, INDENT + JS.Types.H5.INIT + "();");

            WriteIndent(moduleOutput, InitialLevel);
            moduleOutput.Append("}) ();");
            WriteNewLine(moduleOutput);
        }

        /// <summary>
        /// Modern ES module wrap. Emits top-level <c>import</c> statements for the module's
        /// dependencies and an <c>export</c> for the module object. Used when
        /// <see cref="OutputModuleType.ESM"/> is selected.
        /// </summary>
        protected void WrapToESM(StringBuilder moduleOutput, Module module, IEmitterOutput output)
        {
            var str = moduleOutput.ToString();
            moduleOutput.Length = 0;

            var enabledDependecies = GetEnabledDependecies(module, output);

            foreach (var md in enabledDependecies)
            {
                WriteIndent(moduleOutput, InitialLevel);
                WriteNewLine(moduleOutput, "import " + (md.VariableName.IsNotEmpty() ? md.VariableName : md.DependencyName) + " from " + ToJavaScript(md.DependencyName) + ";");
            }

            if (enabledDependecies.Count > 0)
            {
                WriteNewLine(moduleOutput);
            }

            WriteIndent(moduleOutput, InitialLevel);
            WriteNewLine(moduleOutput, "var " + module.Name + " = { };");

            moduleOutput.Append(str);

            if (!str.Trim().EndsWith(NEW_LINE))
            {
                WriteNewLine(moduleOutput);
            }

            WriteIndent(moduleOutput, InitialLevel);
            WriteNewLine(moduleOutput, "export { " + module.Name + " };");
        }
    }
}
