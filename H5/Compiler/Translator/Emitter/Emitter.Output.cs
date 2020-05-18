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
        public const string INDENT = "    ";
        public const string NEW_LINE = "\n";
        public const char NEW_LINE_CHAR = '\n';
        public const string CRLF = "\r\n";

        protected StringBuilder Write(StringBuilder dest, string s, int? position = null)
        {
            if (!position.HasValue)
            {
                dest.Append(s);
            }
            else
            {
                dest.Insert(position.Value, s);
            }

            return dest;
        }

        protected virtual void WriteNewLine(StringBuilder sb)
        {
            sb.Append(NEW_LINE);
        }

        protected virtual void WriteNewLine(StringBuilder sb, string text)
        {
            sb.Append(text);
            sb.Append(NEW_LINE);
        }

        public virtual void WriteIndented(string s, int? position = null)
        {
            var level = position.HasValue && position.Value == 0 ? InitialLevel : Level;

            var indented = new StringBuilder(AbstractEmitterBlock.WriteIndentToString(s, level));

            WriteIndent(indented, level, 0);

            Write(Output, indented.ToString(), position);
        }

        protected virtual void WriteIndent(StringBuilder sb, int level, int? position = null)
        {
            for (var i = 0; i < level; i++)
            {
                Write(sb, INDENT, position);
            }
        }

        protected virtual List<TranslatorOutputItem> TransformOutputs()
        {
            using(new Measure(Logger, "Transforming outputs", logLevel: LogLevel.Trace))
            {
                WrapToModules();
                var outputs = CombineOutputs();
                return outputs;
            }
        }

        protected virtual List<TranslatorOutputItem> CombineOutputs()
        {
            using (var m = new Measure(Logger, "Combining outputs", logLevel: LogLevel.Trace))
            {
                var result = new List<TranslatorOutputItem>();

                var disableAsm = AssemblyInfo.Assembly.DisableInitAssembly;

                AssemblyJsDocWritten = false;

                var fileHelper = new FileHelper();
                int k = 0;
                foreach (var outputPair in Outputs)
                {
                    CancellationToken.ThrowIfCancellationRequested();

                    var fileName = outputPair.Key;
                    var output = outputPair.Value;

                    m.SetOperations(++k).EmitPartial($"Processing file '{(fileName ?? "")}'");

                    bool isJs = fileHelper.IsJS(fileName);

                    var tmp = new StringBuilder(output.TopOutput.Length + output.BottomOutput.Length + output.NonModuletOutput.Length + 1000);

                    OutputTop(output, tmp);

                    OutputNonModule(disableAsm, fileName, output, isJs, tmp);

                    OutputBottom(output, tmp);

                    var outputKind = TranslatorOutputKind.ProjectOutput;

                    if (output.IsMetadata)
                    {
                        outputKind = outputKind | TranslatorOutputKind.Metadata;
                    }

                    AddOutputItem(result, fileName, tmp, outputKind, location: null);
                }

                return result;
            }
        }

        private void OutputAssemblyComment(StringBuilder tmp)
        {
            // /**
            //  * [AssemblyDescription]
            //  * @version 1.2.3.4
            //  * @author [AssemblyCompany]
            //  * @copyright [AssemblyCopyright]
            //  * @compiler H5 0.0.0
            //  */

            if (AssemblyJsDocWritten)
            {
                return;
            }

            var versionContext = Translator.GetVersionContext();

            string description = versionContext.Assembly.Description;
            string version = versionContext.Assembly.Version != null && versionContext.Assembly.Version != JS.Types.System.Reflection.Assembly.Config.DEFAULT_VERSION
                                ? versionContext.Assembly.Version
                                : null;
            string author = versionContext.Assembly.CompanyName;
            string copyright = versionContext.Assembly.Copyright;
            string compiler = "H5 " + versionContext.Compiler.Version;

            WriteNewLine(tmp, "/**");

            if (!string.IsNullOrWhiteSpace(description))
            {
                WriteNewLine(tmp, " * " + description);
            }

            if (!string.IsNullOrWhiteSpace(version))
            {
                WriteNewLine(tmp, " * @version " + version);
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                WriteNewLine(tmp, " * @author " + author);
            }

            if (!string.IsNullOrWhiteSpace(copyright))
            {
                WriteNewLine(tmp, " * @copyright " + copyright);
            }

            WriteNewLine(tmp, " * @compiler " + compiler);

            WriteNewLine(tmp, " */");

            AssemblyJsDocWritten = true;
        }

        private void OutputTop(IEmitterOutput output, StringBuilder tmp)
        {
            if (output.TopOutput.Length > 0)
            {
                tmp.Append(output.TopOutput.ToString());
                WriteNewLine(tmp);
            }
        }

        private bool OutputNonModule(bool disableAsm, string fileName, IEmitterOutput output, bool isJs, StringBuilder tmp)
        {
            bool metaDataWritten = false;

            var level = InitialLevel;
            StringBuilder endOutput = new StringBuilder();

            if (output.NonModuletOutput.Length > 0 || output.ModuleOutput.Count > 0)
            {
                if (isJs)
                {
                    if (!disableAsm)
                    {
                        string asmName = AssemblyInfo.Assembly.FullName ??
                                         Translator.ProjectProperties.AssemblyName;

                        OutputAssemblyComment(tmp);

                        tmp.Append(JS.Types.H5.ASSEMBLY + "(");

                        tmp.AppendFormat("\"{0}\"", asmName);

                        tmp.Append(",");

                        if (!metaDataWritten && (MetaDataOutputName == null || fileName == MetaDataOutputName))
                        {
                            var res = GetIncludedResources();

                            if (res != null)
                            {
                                tmp.Append(" ");
                                tmp.Append(res);
                                tmp.Append(",");
                            }

                            metaDataWritten = true;
                        }

                        tmp.Append(" ");
                        tmp.Append("function ($asm, globals) {");
                        WriteNewLine(tmp);
                        WriteIndent(tmp, level);
                        tmp.Append(GetOutputHeader());
                        WriteNewLine(tmp);
                        WriteNewLine(tmp);
                    }

                    level = AddDependencies(level, output, tmp, endOutput);
                }

                var code = output.NonModuletOutput.ToString();

                if (code.Length > 0)
                {
                    tmp.Append(level > InitialLevel ? INDENT + code.Replace(NEW_LINE, NEW_LINE + INDENT) : code);
                }

                if (endOutput.Length > 0)
                {
                    tmp.Append(endOutput.ToString());
                }

                if (output.ModuleOutput.Any())
                {
                    if (code.Length > 0)
                    {
                        WriteNewLine(tmp);
                    }

                    foreach (var moduleOutput in output.ModuleOutput)
                    {
                        WriteNewLine(tmp, moduleOutput.Value.ToString());
                    }
                }

                if (isJs && !disableAsm)
                {
                    tmp.Append("});");
                    WriteNewLine(tmp);
                }
            }

            return metaDataWritten;
        }

        private int AddDependencies(int level, IEmitterOutput output, StringBuilder tmp, StringBuilder endOutput)
        {
            var loader = AssemblyInfo.Loader;
            var dependencies = output.NonModuleDependencies;
            if (dependencies != null && dependencies.Count > 0)
            {
                var disabledDependecies = dependencies.Where(d => loader.IsManual(d.DependencyName)).ToList();
                dependencies = dependencies.Where(d => !loader.IsManual(d.DependencyName)).ToList();

                if (disabledDependecies.Count > 0 && !loader.SkipManualVariables)
                {
                    WriteIndent(tmp, level);
                    Write(tmp, "var ");
                    for (int i = 0; i < disabledDependecies.Count; i++)
                    {
                        var d = disabledDependecies[i];
                        if (i != 0)
                        {
                            WriteIndent(tmp, level + 1);
                        }

                        Write(tmp, d.VariableName.IsNotEmpty() ? d.VariableName : d.DependencyName);
                        Write(tmp, i == (disabledDependecies.Count - 1) ? ";" : ",");
                        WriteNewLine(tmp);
                    }

                    WriteNewLine(tmp);
                }

                var type = loader.Type;
                var amd = dependencies.Where(d => d.Type == ModuleType.AMD || ((d.Type == null || d.Type == ModuleType.UMD) && type == ModuleLoaderType.AMD)).ToList();
                var cjs = dependencies.Where(d => d.Type == ModuleType.CommonJS || ((d.Type == null || d.Type == ModuleType.UMD) && type == ModuleLoaderType.CommonJS)).ToList();
                var es6 = dependencies.Where(d => d.Type == ModuleType.ES6 || (d.Type == null && type == ModuleLoaderType.ES6)).ToList();

                if (amd.Count > 0)
                {
                    WriteIndent(tmp, level);
                    tmp.Append(loader.FunctionName ?? "require");
                    tmp.Append("([");

                    amd.Each(md =>
                    {
                        tmp.Append(ToJavaScript(md.DependencyName));
                        tmp.Append(", ");
                    });
                    tmp.Remove(tmp.Length - 2, 2); // remove trailing comma
                    tmp.Append("], function (");

                    amd.Each(md =>
                    {
                        tmp.Append(md.VariableName.IsNotEmpty() ? md.VariableName : md.DependencyName);
                        tmp.Append(", ");
                    });
                    tmp.Remove(tmp.Length - 2, 2); // remove trailing comma

                    WriteNewLine(tmp, ") {");

                    WriteIndent(endOutput, level);
                    WriteNewLine(endOutput, JS.Types.H5.INIT + "();");
                    WriteIndent(endOutput, level);
                    WriteNewLine(endOutput, "});");
                    level++;
                }

                if (cjs.Count > 0)
                {
                    cjs.Each(md =>
                    {
                        WriteIndent(tmp, level);
                        tmp.AppendFormat("var {0} = require(\"{1}\");", md.VariableName.IsNotEmpty() ? md.VariableName : md.DependencyName, md.DependencyName);
                        WriteNewLine(tmp);
                    });

                    if (es6.Count == 0)
                    {
                        WriteNewLine(tmp);
                    }
                }

                if (es6.Count > 0)
                {
                    es6.Each(md =>
                    {
                        WriteIndent(tmp, level);
                        WriteNewLine(tmp, "import " + (md.VariableName.IsNotEmpty() ? md.VariableName : md.DependencyName) + " from " + ToJavaScript(md.DependencyName) + ";");
                    });

                    WriteNewLine(tmp);
                }
            }

            return level;
        }

        private void OutputBottom(IEmitterOutput output, StringBuilder tmp)
        {
            if (output.BottomOutput.Length > 0)
            {
                WriteNewLine(tmp);
                tmp.Append(output.BottomOutput.ToString());
            }
        }

        private string GetIncludedResources()
        {
            var resources = Translator.AssemblyDefinition.MainModule.Resources.Where(r => r.ResourceType == ResourceType.Embedded && r.IsPublic && !r.Name.EndsWith(".dll")).Cast<EmbeddedResource>().ToArray();
            JObject obj = new JObject();

            foreach (var embeddedResource in resources)
            {
                obj.Add(embeddedResource.Name, Convert.ToBase64String(ReadResource(embeddedResource)));
            }

            return obj.Count > 0 ? obj.ToString(Formatting.None) : null;
        }

        private static byte[] ReadResource(EmbeddedResource r)
        {
            using (var ms = new MemoryStream())
            using (var s = r.GetResourceStream())
            {
                s.CopyTo(ms);
                return ms.ToArray();
            }
        }

        protected string GetOutputHeader()
        {
            if (Translator.NoStrictMode)
            {
                return string.Empty;
            }

            return "\"use strict\";";
        }
    }
}