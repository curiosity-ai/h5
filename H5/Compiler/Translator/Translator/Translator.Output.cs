using H5.Contract;
using H5.Contract.Constants;
using Microsoft.Extensions.Logging;
using Microsoft.Ajax.Utilities;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZLogger;
using Mosaik.Core;
using System.Threading.Tasks;

namespace H5.Translator
{
    public partial class Translator
    {
        public virtual void Save(string projectOutputPath, string defaultFileName)
        {
            using (new Measure(Logger, $"Saving results to output folder '{projectOutputPath}'"))
            {
                var outputs = Outputs.GetOutputs().ToList();
                var dtsReferences = new List<string>();
                bool addNoLibReference = false;

                foreach (var item in outputs)
                {
                    if (item.OutputType == TranslatorOutputType.TypeScript && item.OutputKind == TranslatorOutputKind.Reference)
                    {
                        string filePath = DefineOutputItemFullPath(item, projectOutputPath, defaultFileName);
                        string path = "./" + FileHelper.GetRelativePath(filePath, projectOutputPath).Replace(Path.DirectorySeparatorChar, '/');

                        if (!dtsReferences.Contains(path))
                        {
                            dtsReferences.Add(path);
                        }

                        if (!addNoLibReference && H5.Translator.Validator.IsTypeFromH5Core(item.Assembly))
                        {
                            addNoLibReference = true;
                        }
                    }
                }

                Parallel.ForEach(outputs, item =>
                {
                    Logger.ZLogTrace("Output {0}", item.Name);

                    string filePath = DefineOutputItemFullPath(item, projectOutputPath, defaultFileName);

                    if (item.IsEmpty && (item.MinifiedVersion == null || item.MinifiedVersion.IsEmpty))
                    {
                        Logger.LogTrace("Output {0} is empty", filePath);
                        return;
                    }

                    var file = FileHelper.CreateFileDirectory(filePath);
                    Logger.LogTrace("Output full name {0}", file.FullName);

                    byte[] buffer = null;
                    string content;

                    if (item.OutputType == TranslatorOutputType.TypeScript && item.OutputKind == TranslatorOutputKind.ProjectOutput)
                    {
                        content = item.Content.GetContentAsString();
                        var sb = new StringBuilder();
                        var nl = Emitter.NEW_LINE;

                        if (addNoLibReference)
                        {
                            sb.Append("/// <reference no-default-lib=\"true\"/>" + nl);
                        }

                        foreach (var reference in dtsReferences)
                        {
                            sb.Append($"/// <reference path=\"{reference}\" />" + nl);
                        }

                        if (sb.Length > 0 && !content.StartsWith("/// <reference path="))
                        {
                            sb.Append(nl);
                        }

                        sb.Append(content);
                        SaveToFile(file.FullName, sb.ToString());
                    }
                    else if (CheckIfRequiresSourceMap(item))
                    {
                        content = item.Content.GetContentAsString();
                        content = GenerateSourceMap(file.FullName, content);

                        item.HasGeneratedSourceMap = true;

                        SaveToFile(file.FullName, content);
                    }
                    else
                    {
                        buffer = item.Content.Buffer;

                        if (buffer != null)
                        {
                            SaveToFile(file.FullName, null, buffer);
                        }
                        else
                        {
                            content = item.Content.GetContentAsString();
                            SaveToFile(file.FullName, content);
                        }
                    }
                });
            }
        }

        private string DefineOutputItemFullPath(TranslatorOutputItem item, string projectOutputPath, string defaultFileName)
        {
            var fileName = item.Name;
            
            if (fileName.Contains(H5.Translator.AssemblyInfo.DEFAULT_FILENAME))
            {
                fileName = fileName.Replace(H5.Translator.AssemblyInfo.DEFAULT_FILENAME, defaultFileName);
            }

            // Ensure filename contains no ":". It could be used like "c:/absolute/path"
            fileName = fileName.Replace(":", "_");

            // Trim heading slash/backslash off file names until it does not start with slash.
            var oldFNlen = fileName.Length;
            while (Path.IsPathRooted(fileName))
            {
                fileName = fileName.TrimStart(Path.DirectorySeparatorChar, '/', '\\');

                // Trimming didn't change the path. This way, it will just loop indefinitely.
                // Also, this means the absolute path specifies a fully-qualified DOS PathName with drive letter.
                if (fileName.Length == oldFNlen)
                {
                    break;
                }
                oldFNlen = fileName.Length;
            }

            if (fileName != item.Name)
            {
                Logger.ZLogTrace("Output file name changed to {0}", fileName);
                item.Name = fileName;
            }

            // If 'fileName' is an absolute path, Path.Combine will ignore the 'path' prefix.
            string filePath = fileName;

            if (item.Location != null)
            {
                filePath = Path.Combine(item.Location, fileName);

                if (fileName != filePath)
                {
                    Logger.ZLogTrace("Output file name changed to {0}", filePath);
                }
            }

            var filePath1 = Path.Combine(projectOutputPath, filePath);

            if (filePath1 != filePath)
            {
                filePath = filePath1;
                Logger.ZLogTrace("Output file name changed to {0}", filePath1);
            }

            filePath = Path.GetFullPath(filePath);

            item.FullPath = new Uri(filePath, UriKind.RelativeOrAbsolute);

            return filePath;
        }

        protected virtual void SaveToFile(string fileName, string content, byte[] binary = null)
        {
            if (content != null && binary != null)
            {
                Logger.ZLogError("Both content and binary are not null for {0}. Will use content.", fileName);
            }

            if (content != null)
            {
                File.WriteAllText(fileName, content, OutputEncoding);
                Logger.ZLogTrace("Saving content (string) into {0}", fileName);
            }
            else
            {
                File.WriteAllBytes(fileName, binary);
                Logger.ZLogTrace("Saving binary into {0}", fileName);
            }

            Logger.ZLogTrace("Saved file {0}", fileName);
        }

        public void CleanOutputFolderIfRequired(string outputPath)
        {
            if (AssemblyInfo != null && (!string.IsNullOrEmpty(AssemblyInfo.CleanOutputFolderBeforeBuild) || !string.IsNullOrEmpty(AssemblyInfo.CleanOutputFolderBeforeBuildPattern)))
            {
                var searchPattern = string.IsNullOrEmpty(AssemblyInfo.CleanOutputFolderBeforeBuildPattern)
                    ? AssemblyInfo.CleanOutputFolderBeforeBuild
                    : AssemblyInfo.CleanOutputFolderBeforeBuildPattern;
                
                CleanDirectory(outputPath, searchPattern);
            }
        }

        protected virtual void AddMainOutputs(List<TranslatorOutputItem> outputs)
        {
            Outputs.Main.AddRange(outputs);
        }

        protected virtual void AddLocaleOutput(EmbeddedResource resource, string outputPath)
        {
            var fileName = resource.Name.Substring(LocalesPrefix.Length);

            if (!string.IsNullOrWhiteSpace(AssemblyInfo.LocalesOutput))
            {
                outputPath = Path.Combine(outputPath, AssemblyInfo.LocalesOutput);
            }

            var content = ReadEmbeddedResource(resource);

            Emitter.AddOutputItem(Outputs.Locales, fileName, new StringBuilder(content), TranslatorOutputKind.Locale, location: outputPath);
        }

        protected virtual void AddLocaleOutputs(IEnumerable<EmbeddedResource> resources, string outputPath)
        {
            foreach (var resource in resources)
            {
                AddLocaleOutput(resource, outputPath);
            }
        }

        protected virtual void AddReferencedOutput(string outputPath, AssemblyDefinition assembly, H5ResourceInfo resource, string fileName, Func<string, string> preHandler = null)
        {
            Logger.ZLogTrace("Adding referenced output " + resource.Name);

            var currentAssembly = GetAssemblyNameForResource(assembly);

            var combinedResource = (resource.Parts != null && resource.Parts.Length > 0);

            TranslatorOutputItemContent content = null;

            if (combinedResource)
            {
                Logger.ZLogTrace("The resource contains parts");

                var contentBuffer = new StringBuilder();
                var needNewLine = false;
                var noConsole = AssemblyInfo.Console.Enabled != true;
                var fileHelper = new FileHelper();

                foreach (var part in resource.Parts)
                {
                    Logger.ZLogTrace("Handling part " + part.Assembly + " " + part.ResourceName);

                    bool needPart = true;

                    System.Reflection.AssemblyName partAssemblyName = null;

                    if (part.Assembly != null)
                    {
                        partAssemblyName = GetAssemblyNameFromResource(part.Assembly);

                        if (noConsole
                            && partAssemblyName.Name == H5_ASSEMBLY
                            && (string.Compare(part.Name, H5ConsoleName, true) == 0
                                || string.Compare(part.Name, fileHelper.GetMinifiedJSFileName(H5ConsoleName), true) == 0)
                            )
                        {
                            // Skip H5 console resource
                            needPart = false;

                            Logger.ZLogTrace("Skipping this part as it is H5 Console and the Console option disabled");
                        }
                        else
                        {
                            var partContentName = GetExtractedResourceName(part.Assembly, part.Name);
                            needPart = ExtractedScripts.Add(partContentName);

                            if (!needPart)
                            {
                                Logger.ZLogTrace("Skipping this part as it is already added");
                            }
                        }
                    }

                    if (needPart)
                    {
                        if (needNewLine)
                        {
                            NewLine(contentBuffer);
                        }

                        string partContent = null;
                        var resourcePartName = part.ResourceName;

                        if (partAssemblyName == null)
                        {
                            Logger.ZLogTrace("Using assembly " + assembly.FullName + " to search resource part " + resourcePartName);

                            partContent = ReadEmbeddedResource(assembly, resourcePartName, true, preHandler).Item2;
                        }
                        else
                        {
                            var partAssembly = References
                                .Where(x => x.Name.Name == partAssemblyName.Name)
                                .OrderByDescending(x => x.Name.Version)
                                .FirstOrDefault();

                            if (partAssembly == null)
                            {
                                Logger.ZLogWarning("Did not find assembly for resource part {0} by assembly name {1}. Skipping this item!", resourcePartName, partAssemblyName.Name);
                                continue;
                            }

                            if (partAssembly.Name.Version != partAssemblyName.Version)
                            {
                                Logger.ZLogInformation("Found different assembly version (higher) {0} for resource part {1} from {2}", partAssembly.FullName, resourcePartName, assembly.FullName);
                            }
                            else
                            {
                                Logger.ZLogTrace("Found exact assembly version {0} for resource part {1}.", partAssembly.FullName ,resourcePartName);
                            }

                            var resourcePartFound = false;

                            try
                            {
                                partContent = ReadEmbeddedResource(partAssembly, resourcePartName, true, preHandler).Item2;
                                resourcePartFound = true;
                            }
                            catch (InvalidOperationException)
                            {
                                Logger.ZLogTrace("Did not find resource part {0} in {1}. Will try to find it by short name {2}", resourcePartName , partAssembly.FullName, part.Name);
                            }

                            if (!resourcePartFound)
                            {
                                try
                                {
                                    partContent = ReadEmbeddedResource(partAssembly, part.Name, true, preHandler).Item2;
                                    resourcePartFound = true;
                                }
                                catch (InvalidOperationException)
                                {
                                    Logger.ZLogTrace("Did not find resource part {0} in {1}", part.Name, partAssembly.FullName);
                                }

                                if (!resourcePartFound)
                                {
                                    if (partAssembly.Name.Version != partAssemblyName.Version)
                                    {
                                        var partAssemblyExactVersion = References
                                            .Where(x => x.FullName == partAssemblyName.FullName)
                                            .FirstOrDefault();

                                        if (partAssemblyExactVersion != null)
                                        {
                                            Logger.ZLogTrace("Trying to find it in the part's assembly by long name {0}", part.Name);

                                            try
                                            {
                                                partContent = ReadEmbeddedResource(partAssemblyExactVersion, resourcePartName, true, preHandler).Item2;
                                                resourcePartFound = true;
                                            }
                                            catch (InvalidOperationException)
                                            {
                                                Logger.ZLogTrace("Did not find resource part {0} in {1}. Will try to find it by short name {2}", resourcePartName, partAssemblyExactVersion.FullName, part.Name);
                                            }

                                            if (!resourcePartFound)
                                            {
                                                try
                                                {
                                                    partContent = ReadEmbeddedResource(partAssemblyExactVersion, part.Name, true, preHandler).Item2;
                                                    resourcePartFound = true;
                                                }
                                                catch (InvalidOperationException)
                                                {
                                                    Logger.ZLogTrace("Did not find resource part {0} in {1}. Will try to find it by the resource's assembly by long name {2} ", part.Name,  partAssemblyExactVersion.FullName, resourcePartName);
                                                }
                                            }
                                        }
                                    }

                                    if (!resourcePartFound)
                                    {
                                        partContent = ReadEmbeddedResource(assembly, resourcePartName, true, preHandler).Item2;
                                        resourcePartFound = true;
                                    }
                                }
                            }
                        }

                        contentBuffer.Append(partContent);

                        needNewLine = true;
                    }
                }

                content = contentBuffer;
            }
            else
            {
                var readAsString = FileHelper.IsJS(fileName) || preHandler != null;

                var notCombinedResource = ReadEmbeddedResource(assembly, resource.Name, readAsString, preHandler);

                if (readAsString)
                {
                    content = notCombinedResource.Item2;
                }
                else
                {
                    content = notCombinedResource.Item1;
                }
            }

            ExtractedScripts.Add(GetExtractedResourceName(currentAssembly, resource.Name));

            Emitter.AddOutputItem(Outputs.References, fileName, content, TranslatorOutputKind.Reference, location: outputPath, assembly: currentAssembly);
        }

        private Tuple<byte[], string> ReadEmbeddedResource(AssemblyDefinition assembly, string resourceName, bool readAsString, Func<string, string> preHandler = null)
        {
            var res = assembly.MainModule.Resources.FirstOrDefault(r => r.Name == resourceName);

            if (res == null)
            {
                throw new InvalidOperationException("Could not read resource " + resourceName + " in " + assembly.FullName);
            }

            using (var resourcesStream = ((EmbeddedResource)res).GetResourceStream())
            {
                if (readAsString)
                {
                    using (var reader = new StreamReader(resourcesStream))
                    {
                        var content = reader.ReadToEnd();
                        return Tuple.Create((byte[])null, content);
                    }
                }
                else
                {
                    var binary = ReadStream(resourcesStream);
                    return Tuple.Create(binary, (string)null);
                }
            }
        }

        protected virtual void AddExtractedResourceOutput(ResourceConfigItem resource, byte[] code)
        {
            Emitter.AddOutputItem(Outputs.Resources, resource.Name, code, TranslatorOutputKind.Resource, location: resource.Output);
        }

        public void ExtractCore(string outputPath, string projectPath)
        {
            using (new Measure(Logger, "Extracting core scripts"))
            {
                ExtractResources(outputPath, projectPath);
                ExtractLocales(outputPath);
            }
        }

        private void ExtractResources(string outputPath, string projectPath)
        {
            using (new Measure(Logger, "Extracting resources"))
            {
                foreach (var reference in References)
                {
                    var resources = GetEmbeddedResourceList(reference);

                    if (!resources.Any())
                    {
                        continue;
                    }

                    var resourceOption = AssemblyInfo.Resources;

                    var noExtract = !resourceOption.HasEmbedResources()
                        && !resourceOption.HasExtractResources()
                        && resourceOption.Default != null
                        && resourceOption.Default.Extract != true;

                    if (noExtract)
                    {
                        Logger.ZLogInformation("No extract option enabled (resources config option contains only default setting with extract disabled)");
                        Logger.ZLogInformation("Skipping extracting all resources");

                        continue;
                    }

                    foreach (var resource in resources)
                    {
                        Logger.ZLogTrace("Extracting item " + resource.Name);

                        var fileName = resource.FileName;
                        var resName = resource.Name;

                        Logger.ZLogTrace("Resource name " + resName + " and file name: " + fileName);

                        string resourceOutputDirName = null;
                        string resourceOutputFileName = null;

                        var resourceExtractItems = resourceOption.ExtractItems
                            .Where(
                                x => string.Compare(x.Name, resName, StringComparison.InvariantCultureIgnoreCase) == 0
                                && (x.Assembly == null
                                    || string.Compare(x.Assembly, reference.Name.Name, StringComparison.InvariantCultureIgnoreCase) == 0))
                            .FirstOrDefault();

                        if (resourceExtractItems != null)
                        {
                            Logger.ZLogTrace("Found resource option for resource name " + resName + " and reference " + resourceExtractItems.Assembly);

                            if (resourceExtractItems.Extract != true)
                            {
                                if(resName != "h5.console.js")
                                {
                                    Logger.ZLogInformation("Skipping resource {0} as it has setting resources.extract != true", resName);
                                }
                                else
                                {
                                    Logger.ZLogTrace("Skipping resource {0} as it has setting resources.extract != true", resName);
                                }
                                continue;
                            }

                            if (resourceExtractItems.Output != null)
                            {
                                Logger.ZLogTrace("resources.output option " + resourceExtractItems.Output);

                                GetResourceOutputPath(outputPath, resourceExtractItems, ref resourceOutputFileName, ref resourceOutputDirName);

                                if (resourceOutputDirName != null)
                                {
                                    Logger.ZLogTrace("Changing output path according to output resource setting to " + resourceOutputDirName);
                                }

                                if (resourceOutputFileName != null)
                                {
                                    Logger.ZLogTrace("Changing output file name according to output resource setting to " + resourceOutputFileName);
                                }
                            }
                            else
                            {
                                Logger.ZLogTrace("No extract resource option affecting extraction for resource name " + resourceExtractItems.Name);
                            }
                        }
                        else
                        {
                            if (resourceOption.Default != null && resourceOption.Default.Extract != true)
                            {
                                Logger.ZLogInformation("Skipping resource " + resName + " as it has no setting resources.extract = true and default setting is resources.extract != true");
                                continue;
                            }

                            Logger.ZLogTrace("Did not find extract resource option for resource name " + resName + ". Will use default embed behavior");

                            if (resource.Path != null)
                            {
                                Logger.ZLogTrace("resource.Path option " + resource.Path);

                                GetResourceOutputPath(outputPath, resource.Path, resource.Name, true, ref resourceOutputFileName, ref resourceOutputDirName);

                                if (resourceOutputDirName != null)
                                {
                                    Logger.ZLogTrace("Changing output path according to embedded resource Path setting to " + resourceOutputDirName);
                                }

                                if (resourceOutputFileName != null)
                                {
                                    Logger.ZLogTrace("Changing output file name according to embedded resource Path setting to " + resourceOutputFileName);
                                }
                            }
                        }

                        if (resourceOutputDirName == null)
                        {
                            resourceOutputDirName = outputPath;
                        }

                        if (resourceOutputFileName == null)
                        {
                            resourceOutputFileName = fileName;
                        }

                        bool isTs = FileHelper.IsDTS(resName);

                        if (!isTs || AssemblyInfo.GenerateTypeScript)
                        {
                            AddReferencedOutput(resourceOutputDirName, reference, resource, resourceOutputFileName);
                        }
                    }
                }
            }
        }

        private void ExtractLocales(string outputPath)
        {
            if (string.IsNullOrWhiteSpace(AssemblyInfo.Locales))
            {
                Logger.ZLogInformation("Skipping extracting Locales");
                return;
            }

            using (new Measure(Logger, "Extracting Locales"))
            {

                var h5Assembly = References.FirstOrDefault(r => r.Name.Name == CS.NS.H5);
                var localesResources = h5Assembly.MainModule.Resources.Where(r => r.Name.StartsWith(LocalesPrefix)).Cast<EmbeddedResource>();
                var locales = AssemblyInfo.Locales.Split(';');

                if (locales.Any(x => x == "all"))
                {
                    AddLocaleOutputs(localesResources, outputPath);
                }
                else
                {
                    foreach (var locale in locales)
                    {
                        if (locale.Contains("*"))
                        {
                            var name = LocalesPrefix + locale.SubstringUpToFirst('*');
                            var maskedResources = localesResources.Where(r => r.Name.StartsWith(name));

                            AddLocaleOutputs(maskedResources, outputPath);
                        }
                        else
                        {
                            var name = LocalesPrefix + locale + Files.Extensions.JS;
                            var maskedResource = localesResources.First(r => r.Name == name);

                            AddLocaleOutput(maskedResource, outputPath);
                        }
                    }
                }

                //if ((bufferjs != null && bufferjs.Length > 0) || (bufferjsmin != null && bufferjsmin.Length > 0))
                //{
                //    if (!string.IsNullOrWhiteSpace(this.AssemblyInfo.LocalesOutput))
                //    {
                //        outputPath = Path.Combine(outputPath, this.AssemblyInfo.LocalesOutput);
                //    }

                //    var defaultFileName = this.AssemblyInfo.LocalesFileName ?? "H5.Locales.js";
                //    var fileName = defaultFileName.Replace(":", "_");
                //    var oldFNlen = fileName.Length;
                //    while (Path.IsPathRooted(fileName))
                //    {
                //        fileName = fileName.TrimStart(Path.DirectorySeparatorChar, '/', '\\');
                //        if (fileName.Length == oldFNlen)
                //        {
                //            break;
                //        }
                //        oldFNlen = fileName.Length;
                //    }

                //    var file = CreateFileDirectory(outputPath, fileName);

                //    if (bufferjs != null && bufferjs.Length > 0)
                //    {
                //        File.WriteAllText(file.FullName, bufferjs.ToString(), OutputEncoding);
                //        this.AddOutputItem(this.OutputItems.Locales, file.Name, Path.GetDirectoryName(file.FullName));
                //    }

                //    if (bufferjsmin != null && bufferjsmin.Length > 0)
                //    {
                //        var minifiedName = FileHelper.GetMinifiedJSFileName(file.FullName);
                //        File.WriteAllText(minifiedName, bufferjsmin.ToString(), OutputEncoding);
                //        this.AddOutputItem(this.OutputItems.Locales, minifiedName, Path.GetDirectoryName(file.FullName));
                //    }
                //}
            }
        }

        internal void Combine(string fileName)
        {
            CombineLocales();
            CombineProjectOutput(fileName);
        }

        private void CombineLocales()
        {
            Logger.ZLogTrace("Combining locales...");

            if (!AssemblyInfo.CombineLocales && !AssemblyInfo.CombineScripts)
            {
                Logger.ZLogTrace("Skipping combining locales as CombineLocales and CombineScripts config oiptions are both switched off.");
                return;
            }

            var fileName = AssemblyInfo.LocalesFileName ?? DefaultLocalesOutputName;

            var combinedLocales = Combine(null, Outputs.Locales, fileName, "locales", TranslatorOutputKind.Locale);

            if (combinedLocales != null && !string.IsNullOrWhiteSpace(AssemblyInfo.LocalesOutput))
            {
                combinedLocales.Location = AssemblyInfo.LocalesOutput;
            }

            Outputs.CombinedLocales = combinedLocales;

            Outputs.Locales.Clear();

            Logger.ZLogTrace("Combining locales done");
        }

        private void CombineProjectOutput(string fileName)
        {
            Logger.ZLogTrace("Combining project outputs...");

            if (!AssemblyInfo.CombineScripts)
            {
                Logger.ZLogTrace("Skipping project outputs as CombineScripts config option switched off.");
                return;
            }

            var needNewLine = false;
            StringBuilder buffer = null;
            int bufferLength = 0;

            var combinedResourcePartsMinified = new Dictionary<H5ResourceInfoPart, string>();
            var combinedResourcePartsNonMinified = new Dictionary<H5ResourceInfoPart, string>();

            foreach (var referenceOutput in Outputs.References)
            {
                ConvertOutputItemIntoResourceInfoPart(combinedResourcePartsNonMinified, combinedResourcePartsMinified, referenceOutput);
            }

            var combinedOutput = Combine(null, Outputs.References, fileName, "project references", TranslatorOutputKind.ProjectOutput);

            if (combinedOutput?.Content?.Builder != null)
            {
                buffer = combinedOutput.Content.Builder;

                bufferLength = buffer.Length;

                if (bufferLength > 0)
                {
                    needNewLine = true;
                }
            }

            if (Outputs.CombinedLocales != null)
            {
                if (needNewLine)
                {
                    NewLine(buffer);
                    needNewLine = false;
                }

                ConvertOutputItemIntoResourceInfoPart(combinedResourcePartsNonMinified, combinedResourcePartsMinified, Outputs.CombinedLocales);

                combinedOutput = Combine(combinedOutput, new List<TranslatorOutputItem> { Outputs.CombinedLocales }, fileName, "combined locales output", TranslatorOutputKind.ProjectOutput);

                Outputs.CombinedLocales = null;

                if (combinedOutput?.Content?.Builder != null)
                {
                    buffer = combinedOutput.Content.Builder;

                    if (buffer.Length > bufferLength)
                    {
                        needNewLine = true;
                    }

                    bufferLength = buffer.Length;
                }
            }

            if (needNewLine)
            {
                NewLine(buffer);
                needNewLine = false;
            }

            var combinedMainOutput = Combine(null, Outputs.Main, fileName, "project main output", TranslatorOutputKind.ProjectOutput);

            ConvertOutputItemIntoResourceInfoPart(combinedResourcePartsNonMinified, combinedResourcePartsMinified, combinedMainOutput);

            if (combinedOutput == null)
            {
                combinedOutput = combinedMainOutput;
            }
            else
            {
                if (combinedMainOutput?.Content?.Builder != null)
                {
                    if (combinedOutput?.Content?.Builder != null)
                    {
                        combinedOutput.Content.Builder.Append(combinedMainOutput.Content.GetContentAsString());
                    }
                    else
                    {
                        combinedOutput.Content = combinedMainOutput.Content;
                    }
                }

                if (combinedMainOutput?.MinifiedVersion?.Content?.Builder != null)
                {
                    if (combinedOutput?.MinifiedVersion?.Content?.Builder != null)
                    {
                        combinedOutput.MinifiedVersion.Content.Builder.Append(combinedMainOutput.MinifiedVersion.Content.GetContentAsString());
                    }
                    else
                    {
                        combinedOutput.MinifiedVersion.Content = combinedMainOutput.MinifiedVersion.Content;
                    }
                }
            }

            Outputs.Combined = combinedOutput;

            Outputs.CombinedResourcePartsNonMinified = combinedResourcePartsNonMinified;
            Outputs.CombinedResourcePartsMinified = combinedResourcePartsMinified;

            Logger.ZLogTrace("Combining project outputs done");
        }

        private void ConvertOutputItemIntoResourceInfoPart(Dictionary<H5ResourceInfoPart, string> resourcePartsNonMinified, Dictionary<H5ResourceInfoPart, string> resourcePartsMinified, TranslatorOutputItem outputItem)
        {
            if (outputItem == null || outputItem.OutputType != TranslatorOutputType.JavaScript)
            {
                return;
            }

            if (!outputItem.IsEmpty)
            {
                var part = new H5ResourceInfoPart()
                {
                    Assembly = outputItem.Assembly,
                    Name = outputItem.Name,
                    ResourceName = H5ResourcesCombinedPrefix + outputItem.Name
                };

                resourcePartsNonMinified[part] = outputItem.Content.GetContentAsString();
            }

            if (outputItem.MinifiedVersion != null && !outputItem.MinifiedVersion.IsEmpty)
            {
                var part = new H5ResourceInfoPart()
                {
                    Assembly = outputItem.MinifiedVersion.Assembly,
                    Name = outputItem.MinifiedVersion.Name,
                    ResourceName = H5ResourcesCombinedPrefix + outputItem.MinifiedVersion.Name
                };

                resourcePartsMinified[part] = outputItem.MinifiedVersion.Content.GetContentAsString();
            }
        }

        private TranslatorOutputItem Combine(TranslatorOutputItem target, List<TranslatorOutputItem> outputs, string fileName, string message, TranslatorOutputKind outputKind, TranslatorOutputType[] filter = null)
        {
            Logger.ZLogTrace("There are " + outputs.Count + " " + message);

            if (outputs.Count <= 0)
            {
                Logger.ZLogTrace("Skipping combining " + message + " as empty.");
                return null;
            }

            if (filter == null)
            {
                filter = new[] { TranslatorOutputType.JavaScript };
            }

            if (target != null)
            {
                Logger.ZLogTrace("Using exisiting target " + target.Name);
            }
            else
            {
                Logger.ZLogTrace("Using " + fileName + " as a fileName for combined " + message);
            }

            StringBuilder buffer = null;
            StringBuilder minifiedBuffer = null;

            if (AssemblyInfo.OutputFormatting != JavaScriptOutputType.Minified)
            {
                buffer = target != null
                            ? target.Content.Builder
                            : new StringBuilder();
            }

            if (AssemblyInfo.OutputFormatting != JavaScriptOutputType.Formatted)
            {
                minifiedBuffer = (target != null && target.MinifiedVersion != null)
                                    ? target.MinifiedVersion.Content.Builder
                                    : new StringBuilder();
            }

            bool firstLine = true;

            foreach (var output in outputs)
            {
                if (filter != null && !filter.Contains(output.OutputType))
                {
                    continue;
                }

                string formattedContent = null;
                string minifiedContent = null;

                if (!output.IsEmpty)
                {
                    formattedContent = output.Content.GetContentAsString();
                }

                if (output.MinifiedVersion != null && !output.MinifiedVersion.IsEmpty)
                {
                    minifiedContent = output.MinifiedVersion.Content.GetContentAsString();
                }

                if (formattedContent == null && minifiedContent == null)
                {
                    output.IsEmpty = true;
                    if (output.MinifiedVersion != null)
                    {
                        output.MinifiedVersion.IsEmpty = true;
                    }

                    Logger.ZLogTrace("Skipping " + output.Name + " as it does not have formatted content nor minified.");
                    continue;
                }

                if (buffer != null)
                {
                    if (!firstLine)
                    {
                        firstLine = false;
                        NewLine(buffer);
                    }

                    firstLine = false;

                    if (formattedContent != null)
                    {
                        buffer.Append(formattedContent);
                    }
                    else if (minifiedContent != null)
                    {
                        buffer.Append(minifiedContent);
                    }
                }

                if (minifiedBuffer != null)
                {
                    if (minifiedContent != null)
                    {
                        minifiedBuffer.Append(minifiedContent);
                    }
                    else
                    {
                        Logger.ZLogWarning("Output " + output.Name + " does not contain minified version");
                    }
                }

                output.IsEmpty = true;

                if (output.MinifiedVersion != null)
                {
                    output.MinifiedVersion.IsEmpty = true;
                }
            }

            if (target != null)
            {
                return target;
            }

            var adjustedFileName = fileName.Replace(":", "_");
            var fileNameLenth = fileName.Length;

            while (Path.IsPathRooted(adjustedFileName))
            {
                adjustedFileName = adjustedFileName.TrimStart(Path.DirectorySeparatorChar, '/', '\\');

                if (adjustedFileName.Length == fileNameLenth)
                {
                    break;
                }

                fileNameLenth = adjustedFileName.Length;
            }

            if (adjustedFileName != fileName)
            {
                fileName = adjustedFileName;
                Logger.ZLogTrace("Adjusted fileName: " + fileName);
            }

            var checkExtentionFileName = FileHelper.CheckFileNameAndOutputType(fileName, TranslatorOutputType.JavaScript);

            if (checkExtentionFileName != null)
            {
                fileName = checkExtentionFileName;
                Logger.ZLogTrace("Extention checked fileName: " + fileName);
            }

            var r = new TranslatorOutputItem
            {
                Content = buffer,
                OutputType = TranslatorOutputType.JavaScript,
                OutputKind = outputKind | TranslatorOutputKind.Combined,
                Name = fileName
            };

            if (minifiedBuffer != null)
            {
                var minifiedName = FileHelper.GetMinifiedJSFileName(r.Name);

                r.MinifiedVersion = new TranslatorOutputItem
                {
                    IsMinified = true,
                    Name = minifiedName,
                    OutputType = r.OutputType,
                    OutputKind = r.OutputKind | TranslatorOutputKind.Minified,
                    Location = r.Location,
                    Content = minifiedBuffer
                };
            }

            return r;
        }

        internal void Minify()
        {
            Logger.ZLogTrace("Minification...");

            if (AssemblyInfo.OutputFormatting == JavaScriptOutputType.Formatted)
            {
                Logger.ZLogTrace("No minification required as OutputFormatting = Formatted");
                return;
            }

            Minify(Outputs.References, GetMinifierSettings);
            Minify(Outputs.Locales, (s) => MinifierCodeSettingsLocales);
            Minify(Outputs.Main, GetMinifierSettings);

            Logger.ZLogTrace("Minification done");
        }

        private void Minify(IEnumerable<TranslatorOutputItem> outputs, Func<string, CodeSettings> minifierSettingsResolver = null)
        {
            if (outputs == null)
            {
                return;
            }

            foreach (var output in outputs)
            {
                CodeSettings settings;

                if (minifierSettingsResolver != null)
                {
                    settings = minifierSettingsResolver(output.Name);
                }
                else
                {
                    settings = MinifierCodeSettingsSafe;
                }

                Minify(output, settings);
            }
        }

        private void Minify(TranslatorOutputItem output, CodeSettings minifierSettings)
        {
            if (output.OutputType != TranslatorOutputType.JavaScript)
            {
                return;
            }

            if (output.MinifiedVersion != null)
            {
                Logger.ZLogTrace(output.Name + " has already a minified version " + output.MinifiedVersion.Name);
                return;
            }

            var formatted = output.IsEmpty ? null : output.Content.GetContentAsString();

            if (formatted == null)
            {
                Logger.ZLogTrace("Content of " + output.Name + " is empty - skipping it a nothing to minifiy");
                return;
            }

            var minifiedName = FileHelper.GetMinifiedJSFileName(output.Name);

            var minifiedContent = Minify(new Minifier(), formatted, minifierSettings);

            output.MinifiedVersion = new TranslatorOutputItem
            {
                Assembly = output.Assembly,
                IsMinified = true,
                Name = minifiedName,
                OutputType = output.OutputType,
                OutputKind = output.OutputKind | TranslatorOutputKind.Minified,
                Location = output.Location,
                Content = minifiedContent
            };

            if (AssemblyInfo.OutputFormatting == JavaScriptOutputType.Minified)
            {
                output.IsEmpty = true;
            }
        }

        private string Minify(Minifier minifier, string source, CodeSettings settings)
        {
            Logger.ZLogTrace("Minification...");

            if (string.IsNullOrEmpty(source))
            {
                Logger.ZLogTrace("Skip minification as input script is empty");
                return source;
            }

            Logger.ZLogTrace("Input script length is " + source.Length + " symbols...");

            var contentMinified = minifier.MinifyJavaScript(source, settings);

            Logger.ZLogTrace("Output script length is " + contentMinified.Length + " symbols. Done.");

            return contentMinified;
        }

        private CodeSettings GetMinifierSettings(string fileName)
        {
            //Different settings depending on whether a file is an internal H5 (like h5.js) or user project's file
            if (MinifierCodeSettingsInternalFileNames.Contains(fileName.ToLower()))
            {
                Logger.ZLogTrace("Will use MinifierCodeSettingsInternal for " + fileName);
                return MinifierCodeSettingsInternal;
            }

            var settings = MinifierCodeSettingsSafe;
            if (NoStrictMode)
            {
                settings = settings.Clone();
                settings.StrictMode = false;

                Logger.ZLogTrace("Will use MinifierCodeSettingsSafe with no StrictMode");
            }
            else
            {
                Logger.ZLogTrace("Will use MinifierCodeSettingsSafe");
            }

            return settings;
        }

        private void CleanDirectory(string outputPath, string searchPattern, params string[] systemFilesToIgnore)
        {
            using (new Measure(Logger, $"Cleaning output folder {(outputPath ?? "")} with search pattern ({(searchPattern ?? "")})", logOnlyDuration: true))
            {

                if (string.IsNullOrWhiteSpace(outputPath))
                {
                    Logger.ZLogWarning("Output directory is not specified. No files deleted.");
                    return;
                }

                try
                {
                    var outputDirectory = new DirectoryInfo(outputPath);
                    if (!outputDirectory.Exists)
                    {
                        Logger.ZLogInformation("Output directory {0} does not exist. No files to delete.", outputPath);
                        return;
                    }

                    var patterns = searchPattern.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    if (patterns.Length == 0)
                    {
                        Logger.ZLogWarning("Invalid empty search pattern. No files will be deleted from output folder.");
                        return;
                    }

                    var filesToDelete = new List<FileInfo>();
                    var filesToSkip = new List<Tuple<string, FileInfo>>();
                    foreach (var pattern in patterns)
                    {
                        if (string.IsNullOrEmpty(pattern)) { continue; }

                        if (pattern.StartsWith("!"))
                        {
                            if (pattern.Length > 1)
                            {
                                filesToSkip.AddRange(outputDirectory.GetFiles(pattern.Substring(1), SearchOption.AllDirectories).Select(x => Tuple.Create(pattern, x)));
                            }
                        }
                        else
                        {
                            filesToDelete.AddRange(outputDirectory.GetFiles(pattern, SearchOption.AllDirectories));
                        }
                    }

                    if (systemFilesToIgnore != null && systemFilesToIgnore.All(x => string.IsNullOrEmpty(x)))
                    {
                        systemFilesToIgnore = null;
                    }

                    foreach (var file in filesToDelete)
                    {
                        if (systemFilesToIgnore != null && systemFilesToIgnore.Any(x => x == file.FullName))
                        {
                            Logger.ZLogTrace("skip cleaning {0} as it is a system file", file.FullName);
                            continue;
                        }

                        var skipPattern = filesToSkip.FirstOrDefault(x => x.Item2.FullName == file.FullName);

                        if (skipPattern != null)
                        {
                            Logger.ZLogTrace("skip cleaning {0} as it has skip pattern {1}", file.FullName, skipPattern.Item1);
                            continue;
                        }

                        Logger.ZLogTrace("cleaning {0}", file.FullName);
                        file.Delete();
                    }
                }
                catch (Exception ex)
                {
                    Logger.ZLogError(ex.ToString());
                }
            }
        }
    }
}