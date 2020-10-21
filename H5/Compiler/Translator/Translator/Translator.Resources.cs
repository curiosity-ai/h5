using H5.Contract;
using Microsoft.Extensions.Logging;
using Mono.Cecil;
using Mosaik.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZLogger;

namespace H5.Translator
{
    public partial class Translator
    {
        private Dictionary<string, string> resourceHeaderInfo;
        private Dictionary<string, string> ResourceHeaderInfo
        {
            get
            {
                if (resourceHeaderInfo == null)
                {
                    resourceHeaderInfo = PrepareResourseHeaderInfo();
                }

                return resourceHeaderInfo;
            }
        }

        internal virtual string ReadEmbeddedResource(EmbeddedResource resource)
        {
            using (var resourcesStream = resource.GetResourceStream())
            {
                using (StreamReader reader = new StreamReader(resourcesStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public virtual void InjectResources(string outputPath, string projectPath)
        {
            using (new Measure(Logger, $"Injecting resources from project '{projectPath}' into output '{outputPath}'"))
            {
                var resourcesConfig = AssemblyInfo.Resources;

                if (resourcesConfig.Default != null && resourcesConfig.Default.Inject != true && !resourcesConfig.HasEmbedResources())
                {
                    Logger.ZLogInformation("Resource config option to inject resources `inject` is switched off. Skipping embedding resources");
                    return;
                }

                var outputs = Outputs;

                if (outputs.Main.Count <= 0 && !resourcesConfig.HasEmbedResources())
                {
                    Logger.ZLogInformation("No files nor resources to inject");
                    return;
                }

                var resourcesToEmbed = PrepareAndExtractResources(outputPath, projectPath);

                EmbedResources(PrepareResourcesForEmbedding(resourcesToEmbed));
            }
        }

        private IEnumerable<Tuple<H5ResourceInfo, byte[]>> PrepareAndExtractResources(string outputPath, string projectPath)
        {
            if (AssemblyInfo.Resources.HasEmbedResources())
            { 
                // There are resources defined in the config so let's grab files
                // Find all items and put in the order
                Logger.ZLogTrace("Preparing resources specified in config...");

                foreach (var resource in AssemblyInfo.Resources.EmbedItems)
                {
                    Logger.ZLogTrace("Preparing resource {0}", resource.Name);

                    if (resource.Inject != true && resource.Extract != true)
                    {
                        Logger.ZLogTrace("Skipping the resource as it has inject != true and extract != true");
                        continue;
                    }

                    using (var resourceBuffer = new MemoryStream(500 * 1024))
                    {
                        GenerateResourceHeader(resourceBuffer, resource, projectPath);

                        var needSourceMap = ReadResourceFiles(projectPath, resourceBuffer, resource);

                        if (resourceBuffer.Length > 0)
                        {
                            Logger.ZLogTrace("Prepared file items for resource {0}", resource.Name);

                            var resourcePath = GetFullPathForResource(outputPath, resource);

                            var code = resourceBuffer.ToArray();

                            if (needSourceMap)
                            {
                                TranslatorOutputItemContent content = code;

                                var fullPath = Path.GetFullPath(Path.Combine(resourcePath.Item1, resourcePath.Item2));

                                content = GenerateSourceMap(fullPath, content.GetContentAsString());

                                code = content.GetContentAsBytes();
                            }

                            ExtractResource(resourcePath.Item1, resourcePath.Item2, resource, code);

                            var info = new H5ResourceInfo
                            {
                                Name = resource.Name,
                                FileName = resource.Name,
                                Path = string.IsNullOrWhiteSpace(resource.Output) ? null : resource.Output
                            };

                            yield return Tuple.Create(info, code);
                        }
                        else
                        {
                            Logger.ZLogError("No files found for resource {0}",  resource.Name);
                        }
                    }

                    Logger.ZLogTrace("Done preparing resource {0}", resource.Name);
                }

                Logger.ZLogTrace("Done preparing resources specified in config...");
            }
            else
            {
                // There are no resources defined in the config so let's just grab files
                Logger.ZLogTrace("Preparing outputs for resources");

                var nonMinifiedCombinedPartsDone = false;
                var minifiedCombinedPartsDone = false;

                foreach (var outputItem in Outputs.GetOutputs(true))
                {
                    H5ResourceInfoPart[] parts = null;

                    Logger.ZLogTrace("Getting output full path: {0}", outputItem.FullPath.ToString());
                    Logger.ZLogTrace("Getting output local path: {0}", outputItem.FullPath.LocalPath);

                    var isCombined = outputItem.OutputKind.HasFlag(TranslatorOutputKind.Combined) && Outputs.Combined != null;
                    var isMinified = outputItem.OutputKind.HasFlag(TranslatorOutputKind.Minified);

                    if (isCombined)
                    {
                        Logger.ZLogTrace("The resource item is combined. Setting up its parts.");

                        if (!isMinified)
                        {
                            Logger.ZLogTrace("Choosing non-minified parts.");
                            parts = Outputs.CombinedResourcePartsNonMinified.Select(x => x.Key).ToArray();
                        }

                        if (isMinified)
                        {
                            Logger.ZLogTrace("Choosing minified parts.");
                            parts = Outputs.CombinedResourcePartsMinified.Select(x => x.Key).ToArray();
                        }
                    }

                    var info = new H5ResourceInfo
                    {
                        Name = outputItem.Name,
                        FileName = outputItem.Name,
                        Path = null,
                        Parts = parts
                    };

                    byte[] content;

                    if (!outputItem.HasGeneratedSourceMap)
                    {
                        Logger.ZLogTrace("The output item does not have HasGeneratedSourceMap so we use it right from the Outputs");
                        content = outputItem.Content.GetContentAsBytes();
                        Logger.ZLogTrace("The output is of content " + content.Length + " length");
                    }
                    else
                    {
                        Logger.ZLogTrace("Reading content file as the output has HasGeneratedSourceMap");
                        content = File.ReadAllBytes(outputItem.FullPath.LocalPath);
                        Logger.ZLogTrace("Read " + content.Length + " bytes for " + info.Name);
                    }

                    yield return Tuple.Create(info, content);

                    if (isCombined)
                    {
                        Dictionary<H5ResourceInfoPart, string> partSource = null;

                        if (!isMinified && !nonMinifiedCombinedPartsDone)
                        {
                            Logger.ZLogTrace("Preparing non-minified combined resource parts");

                            partSource = Outputs.CombinedResourcePartsNonMinified;
                        }

                        if (isMinified && !minifiedCombinedPartsDone)
                        {
                            Logger.ZLogTrace("Preparing minified combined resource parts");

                            partSource = Outputs.CombinedResourcePartsMinified;
                        }

                        if (partSource != null)
                        {
                            foreach (var part in partSource)
                            {
                                //if (part.Key.Assembly != null)
                                //{
                                //    Logger.ZLogTrace("Resource part " + part.Key.ResourceName + " is not embedded into resources as it is from another assembly " + part.Key.Assembly);
                                //    continue;
                                //}

                                if (part.Value == null)
                                {
                                    Logger.ZLogTrace("Resource part " + part.Key.ResourceName + " from " + part.Key.Assembly + " is not embedded into resources as it is empty");
                                    continue;
                                }

                                var partResource = new H5ResourceInfo
                                {
                                    Name = null,
                                    FileName = part.Key.ResourceName,
                                    Path = null,
                                    Parts = null
                                };

                                var partContent = new byte[0];

                                if (!string.IsNullOrEmpty(part.Value))
                                {
                                    if (CheckIfRequiresSourceMap(part.Key))
                                    {
                                        var partSourceMap = GenerateSourceMap(part.Key.Name, part.Value);
                                        partContent = OutputEncoding.GetBytes(partSourceMap);
                                    }
                                    else
                                    {
                                        partContent = OutputEncoding.GetBytes(part.Value);
                                    }
                                }

                                yield return Tuple.Create(partResource, partContent);
                            }

                            if (!isMinified)
                            {
                                nonMinifiedCombinedPartsDone = true;
                            }

                            if (isMinified)
                            {
                                minifiedCombinedPartsDone = true;
                            }
                        }
                    }
                }

                Logger.ZLogTrace("Done preparing output files for resources");
            }
        }

        private string ReadEmbeddedResourceList(AssemblyDefinition assemblyDefinition, string listName)
        {
            Logger.ZLogTrace("Checking if reference " + assemblyDefinition.FullName + " contains H5 Resources List " + listName);

            var listRes = assemblyDefinition.MainModule.Resources.FirstOrDefault(x => x.Name == listName);

            if (listRes == null)
            {
                Logger.ZLogTrace("Reference " + assemblyDefinition.FullName + " does not contain H5 Resources List");

                return null;
            }

            string resourcesStr = null;
            using (var resourcesStream = ((EmbeddedResource)listRes).GetResourceStream())
            {
                using (StreamReader reader = new StreamReader(resourcesStream))
                {
                    Logger.ZLogTrace("Reading H5 Resources List");
                    resourcesStr = reader.ReadToEnd();
                    Logger.ZLogTrace("Read H5 Resources List: " + resourcesStr);
                }
            }

            return resourcesStr;
        }

        private H5ResourceInfo[] GetEmbeddedResourceList(AssemblyDefinition assemblyDefinition)
        {
            // First read Json format list
            var resourcesStr = ReadEmbeddedResourceList(assemblyDefinition, H5ResourcesJsonFormatList);

            if (resourcesStr != null)
            {
                return ParseH5ResourceListInJsonFormat(resourcesStr);
            }

            // Then read old plus separated format list (by another file name)
            resourcesStr = ReadEmbeddedResourceList(assemblyDefinition, H5ResourcesPlusSeparatedFormatList);

            if (resourcesStr != null)
            {
                return ParseH5ResourceListInPlusSeparatedFormat(resourcesStr);
            }

            return new H5ResourceInfo[0];
        }

        private static H5ResourceInfo[] ParseH5ResourceListInJsonFormat(string json)
        {
            var r = Newtonsoft.Json.JsonConvert.DeserializeObject<H5ResourceInfo[]>(json);

            return r;
        }

        private static H5ResourceInfo[] ParseH5ResourceListInPlusSeparatedFormat(string resourcesStr)
        {
            var r = new List<H5ResourceInfo>();

            var resources = resourcesStr.Split('+');

            foreach (var resourcePair in resources)
            {
                var parts = resourcePair.Split(':');
                var fileName = parts[0].Trim();
                var resName = parts[1].Trim();

                r.Add(new H5ResourceInfo
                {
                    Name = resName,
                    FileName = fileName,
                    Path = null
                });
            }

            return r.ToArray();
        }

        private List<H5ResourceInfo> PrepareResourcesForEmbedding(IEnumerable<Tuple<H5ResourceInfo, byte[]>> resourcesToEmbed)
        {
            Logger.ZLogTrace("PrepareResourcesForEmbedding...");

            var assemblyDef = AssemblyDefinition;
            var resources = assemblyDef.MainModule.Resources;
            var resourceList = new List<H5ResourceInfo>();

            var configHelper = new ConfigHelper();

            var reportResources = new List<Tuple<string, string, string>>();

            foreach (var item in resourcesToEmbed)
            {
                var r = item.Item1;
                var name = r.Name;
                var fileName = r.FileName;

                Logger.ZLogTrace("Embedding resource " + name + " (fileName: " + fileName + ")");

                // Normalize resourse output path to always have '/' as a directory separator
                r.Path = configHelper.ConvertPath(r.Path, '/');

                var newResource = new EmbeddedResource(name ?? fileName, ManifestResourceAttributes.Private, item.Item2);

                var existingResource = resources.FirstOrDefault(x => x.Name == (name ?? fileName));

                if (existingResource != null)
                {
                    Logger.ZLogTrace("Removing already existed resource with the same name");
                    resources.Remove(existingResource);
                }

                resources.Add(newResource);

                if (name != null)
                {
                    resourceList.Add(r);
                }

                var sizeInBytes = Utils.ByteSizeHelper.ToSizeInBytes(item.Item2 != null ? item.Item2.Length : 0, "0.000 KB", true);

                var resourceLocation = name ?? fileName;
                if (!string.IsNullOrEmpty(r.Path) && !string.IsNullOrEmpty(resourceLocation))
                {
                    resourceLocation = Path.Combine(
                        configHelper.ConvertPath(r.Path),
                        configHelper.ConvertPath(resourceLocation));

                    resourceLocation = configHelper.ConvertPath(resourceLocation, '/');
                }

                reportResources.Add(Tuple.Create(name ?? fileName, resourceLocation, sizeInBytes));

                Logger.ZLogTrace("Added resource " + name + " (fileName: " + fileName + ")");
            }

            Logger.ZLogTrace("PrepareResourcesForEmbedding done");

            return resourceList;
        }

        private void EmbedResources(List<H5ResourceInfo> resourcesToEmbed)
        {
            Logger.ZLogTrace("Embedding resources...");

            var assemblyDef = AssemblyDefinition;
            var resources = assemblyDef.MainModule.Resources;
            var configHelper = new ConfigHelper();

            CheckIfResourceExistsAndRemove(resources, H5ResourcesPlusSeparatedFormatList);

            var resourceListName = H5ResourcesJsonFormatList;
            CheckIfResourceExistsAndRemove(resources, resourceListName);

            var listArray = resourcesToEmbed.ToArray();
            var listContent = Newtonsoft.Json.JsonConvert.SerializeObject(listArray, Newtonsoft.Json.Formatting.Indented);

            var listResources = new EmbeddedResource(resourceListName, ManifestResourceAttributes.Private, OutputEncoding.GetBytes(listContent));

            resources.Add(listResources);
            Logger.ZLogTrace("Added resource list " + resourceListName);
            Logger.ZLogTrace(listContent);

            // Checking if mscorlib reference added and removing if added
            var mscorlib = assemblyDef.MainModule.AssemblyReferences.FirstOrDefault(r => r.Name == SystemAssemblyName);
            if (mscorlib != null)
            {
                Logger.ZLogTrace("Removing mscorlib reference");
                assemblyDef.MainModule.AssemblyReferences.Remove(mscorlib);
            }

            var assemblyLocation = AssemblyLocation;

            Logger.ZLogTrace("Writing resources into " + assemblyLocation);

            using (var s = File.Open(assemblyLocation, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                assemblyDef.Write(s);
                s.Flush();
                s.Close();
            }
            Logger.ZLogTrace("Wrote resources into " + assemblyLocation);

            Logger.ZLogTrace("Done embedding resources");
        } 

        private void CheckIfResourceExistsAndRemove(Mono.Collections.Generic.Collection<Resource> resources, string resourceName)
        {
            var existingList = resources.FirstOrDefault(r => r.Name == resourceName);
            if (existingList != null)
            {
                Logger.ZLogTrace("Removing already existed resource " + resourceName);
                resources.Remove(existingList);
            }
        }

        private void ExtractResource(string path, string fileName, ResourceConfigItem resource, byte[] code)
        {
            Logger.ZLogTrace("Extracting resource {0}", resource.Name);

            if (!resource.Extract.HasValue || !resource.Extract.Value)
            {
                Logger.ZLogTrace("Skipping extracting resource as no extract option enabled for this resource");
                return;
            }

            try
            {
                var fullPath = Path.GetFullPath(Path.Combine(path, fileName));

                Logger.ZLogTrace("Writing resource {0}", resource.Name + " into " + fullPath);

                EnsureDirectoryExists(path);

                File.WriteAllBytes(fullPath, code);

                AddExtractedResourceOutput(resource, code);

                Logger.ZLogTrace("Done writing resource into file");
            }
            catch (Exception ex)
            {
                Logger.ZLogError(ex, "Error extracting resource {0}", resource.Name);
                throw;
            }
        }

        private Tuple<string, string> GetFullPathForResource(string outputPath, ResourceConfigItem resource)
        {
            string resourceOutputFileName = null;
            string resourceOutputDirName = null;

            if (resource.Output != null)
            {
                GetResourceOutputPath(outputPath, resource, ref resourceOutputFileName, ref resourceOutputDirName);
            }

            if (resourceOutputDirName == null)
            {
                resourceOutputDirName = outputPath;
                Logger.ZLogTrace("Using project output path " + resourceOutputDirName);
            }

            if (string.IsNullOrWhiteSpace(resourceOutputFileName))
            {
                resourceOutputFileName = resource.Name;
                Logger.ZLogTrace("Using resource name as file name " + resourceOutputFileName);
            }

            return Tuple.Create(resourceOutputDirName, resourceOutputFileName);
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Logger.ZLogTrace("The resource path does not exist. Creating...");
                Directory.CreateDirectory(path);
                Logger.ZLogTrace("Created directory for the resource path");
            }
        }

        public void GetResourceOutputPath(string basePath, string output, string name, bool? silent, ref string resourceOutputFileName, ref string resourceOutputDirName)
        {
            Logger.ZLogTrace("Checking output path setting " + output);

            try
            {
                var pathParts = FileHelper.GetDirectoryAndFilenamePathComponents(output);

                resourceOutputDirName = pathParts[0];
                Logger.ZLogTrace("Resource output setting directory relative to base path is " + resourceOutputDirName);

                resourceOutputFileName = pathParts[1];
                Logger.ZLogTrace("Resource output setting file name is " + resourceOutputFileName);

                if (resourceOutputDirName != null)
                {
                    Logger.ZLogTrace("Checking resource output directory on invalid characters");
                    resourceOutputDirName = CheckInvalidCharacters(name, silent, resourceOutputDirName, InvalidPathChars);
                }

                if (resourceOutputDirName != null)
                {
                    Logger.ZLogTrace("Getting absolute resource output directory");
                    resourceOutputDirName = Path.Combine(basePath, resourceOutputDirName);
                    Logger.ZLogTrace("Resource output directory is " + resourceOutputDirName);

                    if (resourceOutputFileName != null)
                    {
                        Logger.ZLogTrace("Checking resource output file name on invalid characters");
                        resourceOutputFileName = CheckInvalidCharacters(name, silent, resourceOutputFileName, Path.GetInvalidFileNameChars());

                        if (resourceOutputFileName == null)
                        {
                            Logger.ZLogTrace("Setting resource output directory to null as file name part contains invalid characters");
                            resourceOutputDirName = null;
                        }
                    }
                }
                else
                {
                    Logger.ZLogTrace("Setting resource output file name to null as directory part contains invalid characters");
                    resourceOutputFileName = null;
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is PathTooLongException)
            {
                Logger.ZLogError(ex, "Could not extract directory name from resource output setting");

                if (silent != true)
                {
                    throw;
                }

                resourceOutputDirName = null;
                resourceOutputFileName = null;
            }
        }

        public void GetResourceOutputPath(string basePath, ResourceConfigItem resource, ref string resourceOutputFileName, ref string resourceOutputDirName)
        {
            GetResourceOutputPath(basePath, resource.Output, resource.Name, resource.Silent, ref resourceOutputFileName, ref resourceOutputDirName);
        }

        private string CheckInvalidCharacters(string name, bool? silent, string s, ICollection<char> invalidChars)
        {
            if (s == null)
            {
                return null;
            }

            if (s.Select(x => invalidChars.Contains(x)).Where(x => x).Any())
            {
                var message = "There is invalid path character contained in resource.output setting = "
                        + s
                        + " for resource "
                        + name;

                if (silent != true)
                {
                    throw new ArgumentException(message);
                }
                else
                {
                    Logger.ZLogTrace(message);
                    s = null;
                }
            }

            return s;
        }

        private void GenerateResourceHeader(MemoryStream resourceBuffer, ResourceConfigItem resource, string basePath)
        {
            if (resource.Header == null)
            {
                Logger.ZLogTrace("Resource header is not specified.");
                return;
            }

            Logger.ZLogTrace("Writing header for resource config item {0}", resource.Name);

            var headerInfo = ResourceHeaderInfo;

            var headerContent = GetHeaderContent(resource, basePath);

            ApplyTokens(headerContent, headerInfo);

            if (headerContent.Length > 0)
            {
                var b = OutputEncoding.GetBytes(headerContent.ToString());
                resourceBuffer.Write(b, 0, b.Length);

                NewLine(resourceBuffer);

                Logger.ZLogTrace("Wrote " + headerContent.Length + " symbols as a resource header");
            }
            else
            {
                Logger.ZLogTrace("No header content written as it was empty");
            }

            Logger.ZLogTrace("Done writing header for resource config item {0}", resource.Name);
        }

        private Dictionary<string, string> PrepareResourseHeaderInfo()
        {
            var assemblyInfo = GetVersionContext().Assembly;

            var nowDate = DateTime.Now;

            string version = null;
            string author = null;
            string date = nowDate.ToString("yyyy-MM-dd");
            string year = nowDate.Year.ToString();
            string copyright = null;

            if (assemblyInfo == null)
            {
                Logger.ZLogError("Could not get assembly version to generate resource header info");
            }
            else
            {
                version = assemblyInfo.Version;
                author = assemblyInfo.CompanyName;
                copyright = assemblyInfo.Copyright;
            }

            var headerInfo = new Dictionary<string, string>
            {
                ["version"] = version,
                ["author"] = author,
                ["date"] = date,
                ["year"] = year,
                ["copyright"] = copyright
            };

            return headerInfo;
        }

        private StringBuilder ApplyTokens(string s, Dictionary<string, string> info)
        {
            var sb = new StringBuilder(s);

            ApplyTokens(sb, info);

            return sb;
        }

        private void ApplyTokens(StringBuilder sb, Dictionary<string, string> info)
        {
            Logger.ZLogTrace("Applying tokens...");

            if (sb == null)
            {
                throw new ArgumentNullException("sb", "Cannot apply tokens for null StringBuilder");
            }

            if (info == null)
            {
                throw new ArgumentNullException("info", "Cannot apply tokens of null data");
            }

            foreach (var item in info)
            {
                Logger.ZLogTrace(string.Format("Applying {{{0}}}: {1}", item.Key, item.Value));
                sb.Replace("{" + item.Key + "}", item.Value);
            }

            Logger.ZLogTrace("Applying tokens done");
        }

        private StringBuilder GetHeaderContent(ResourceConfigItem resource, string basePath)
        {
            Logger.ZLogTrace("Getting header content...");

            var isFileName = false;
            string convertedHeaderPath = null;

            string resourceHeader = resource.Header;

            try
            {
                Logger.ZLogTrace("Checking if resource header setting is a file path...");

                Logger.ZLogTrace("Converting slashes in resource header setting...");
                var configHelper = new ConfigHelper();
                convertedHeaderPath = configHelper.ConvertPath(resourceHeader);

                Logger.ZLogTrace("Checking if " + convertedHeaderPath + " contains file name...");
                var headerFileName = Path.GetFileName(convertedHeaderPath);
                isFileName = !string.IsNullOrEmpty(headerFileName);
            }
            catch (ArgumentException ex)
            {
                Logger.ZLogTrace(ex.ToString());
            }

            if (isFileName)
            {
                Logger.ZLogTrace("Checking if header content file exists");
                var fullHeaderPath = Path.Combine(basePath, convertedHeaderPath);

                if (File.Exists(fullHeaderPath))
                {
                    Logger.ZLogTrace("Reading header content file at " + fullHeaderPath);

                    using (var m = new StreamReader(fullHeaderPath, OutputEncoding, true))
                    {
                        var sb = new StringBuilder(m.ReadToEnd());

                        if (m.CurrentEncoding != OutputEncoding)
                        {
                            Logger.ZLogTrace("Converting resource header file {0} from encoding {1} to encoding {2}", fullHeaderPath, m.CurrentEncoding.EncodingName, OutputEncoding.EncodingName);
                        }

                        Logger.ZLogTrace("Read {0} symbols from the header file {1}", sb.Length , fullHeaderPath);

                        return sb;
                    }
                }

                Logger.ZLogWarning("Could not find header content file at {0} for resource {1}", fullHeaderPath, resource.Name);
            }

            Logger.ZLogTrace("Considered resource header setting to be a header content");

            return new StringBuilder(resourceHeader);
        }

        /// <summary>
        /// Reads files for the resource item and return a value indication whether it is a resource for SourceMap generation
        /// </summary>
        /// <param name="outputPath">Project output path</param>
        /// <param name="buffer"></param>
        /// <param name="item">Resource</param>
        /// <returns>Bool value indication whether it is a resource for SourceMap generation</returns>
        private bool ReadResourceFiles(string outputPath, MemoryStream buffer, ResourceConfigItem item)
        {
            Logger.ZLogTrace("Reading resource with " + item.Files.Length + " items");

            var needSourceMap = false;

            var useDefaultEncoding = item.Files.Length > 1;

            foreach (var fileName in item.Files)
            {
                Logger.ZLogTrace("Reading resource item(s) in location " + fileName);

                try
                {
                    string directoryPath;

                    directoryPath = outputPath;

                    var dirPathInFileName = FileHelper.GetDirectoryAndFilenamePathComponents(fileName)[0];

                    var filePathCleaned = fileName;
                    if (!string.IsNullOrEmpty(dirPathInFileName))
                    {
                        directoryPath = Path.Combine(directoryPath, dirPathInFileName);
                        Logger.ZLogTrace("Cleaned folder path part: " + dirPathInFileName + " from location: " + fileName + " and added to the directory path: " + directoryPath);

                        filePathCleaned = Path.GetFileName(filePathCleaned);
                    }

                    directoryPath = Path.GetFullPath(directoryPath);

                    var fullFileName = Path.Combine(directoryPath, filePathCleaned);

                    GenerateResourceFileRemark(buffer, item, filePathCleaned, dirPathInFileName);

                    var outputItem = FindTranslatorOutputItem(fullFileName);

                    if (outputItem != null)
                    {
                        Logger.ZLogTrace("Found required file for resources in Outputs " + fullFileName);

                        if (outputItem.HasGeneratedSourceMap)
                        {
                            Logger.ZLogTrace("The output item HasGeneratedSourceMap so that the resource requires SourceMap also");
                            needSourceMap = true;
                        }

                        var content = outputItem.Content.GetContentAsBytes();

                        buffer.Write(content, 0, content.Length);
                    }
                    else
                    {
                        var directory = new DirectoryInfo(directoryPath);

                        if (!directory.Exists)
                        {
                            throw new InvalidOperationException($"Missing resource from json config file, could not find folder: '{directory.FullName}' for resource '{item.Name}' with file name '{fileName}'");
                        }

                        Logger.ZLogTrace("Searching files for resources in folder: " + directoryPath);

                        var file = directory.GetFiles(filePathCleaned, SearchOption.TopDirectoryOnly).FirstOrDefault();

                        if (file == null)
                        {
                            throw new InvalidOperationException($"Missing resource from json config file, could not find in folder '{directory.FullName}' the required resource '{item.Name}' with file name '{fileName}'");
                        }

                        Logger.ZLogTrace("Reading resource item at " + file.FullName);

                        var resourceAsOneFile = item.Header == null
                                                && item.Remark == null
                                                && item.Files.Length <= 1;

                        var content = CheckResourceOnBomAndAddToBuffer(buffer, item, file, resourceAsOneFile);

                        Logger.ZLogTrace("Read " + content.Length + " bytes");
                    }
                }
                catch (Exception ex)
                {
                    Logger.ZLogError(ex, "Error reading resources files for item {0}", item);
                    throw;
                }
            }

            return needSourceMap;
        }

        private System.Reflection.AssemblyName GetAssemblyNameFromResource(string assemblyName)
        {
            System.Reflection.AssemblyName name = null;

            try
            {
                name = new System.Reflection.AssemblyName(assemblyName);
            }
            catch (Exception ex)
            {
                Logger.ZLogWarning(ex, "Could not get assembly name: {0}", assemblyName);
            }

            return name;
        }

        private string GetAssemblyNameForResource(AssemblyDefinition assembly)
        {
            return assembly.FullName;
        }

        private string GetExtractedResourceName(string assemblyName, string resourceName)
        {
            var assembly = GetAssemblyNameFromResource(assemblyName);

            if (assembly != null && !string.IsNullOrEmpty(assembly.Name))
            {
                return assembly.Name + " | " + resourceName;
            }

            return assemblyName + " | " + resourceName;
        }

        private byte[] CheckResourceOnBomAndAddToBuffer(MemoryStream buffer, ResourceConfigItem item, FileInfo file, bool oneFileResource)
        {
            byte[] content;

            if (oneFileResource)
            {
                Logger.ZLogTrace("Reading resource file {0} as one-file-resource", file.FullName);
                content = File.ReadAllBytes(file.FullName);
            }
            else
            {
                Logger.ZLogTrace("Reading resource file {0} via StreamReader with byte order mark detection option", file.FullName);

                using (var m = new StreamReader(file.FullName, OutputEncoding, true))
                {
                    content = OutputEncoding.GetBytes(m.ReadToEnd());

                    if (m.CurrentEncoding != OutputEncoding)
                    {
                        Logger.ZLogTrace("Converting resource file {0} from encoding {1} into default encoding {2}", file.FullName, m.CurrentEncoding.EncodingName, OutputEncoding.EncodingName);
                    }
                }
            }

            if (content.Length > 0)
            {
                var checkBom = (oneFileResource && item.RemoveBom == true) || (!oneFileResource && (!item.RemoveBom.HasValue || item.RemoveBom.Value));

                var bomLength = checkBom ? GetBomLength(content) : 0;

                if (bomLength > 0)
                {
                    Logger.ZLogTrace("Found BOM symbols ({0} byte length)", bomLength);
                }

                if (bomLength < content.Length)
                {
                    buffer.Write(content, bomLength, content.Length - bomLength);
                }
                else
                {
                    Logger.ZLogTrace("Skipped resource as it contains only BOM");
                }
            }

            return content;
        }

        private int GetBomLength(byte[] buffer)
        {
            if (buffer.Length >= 2)
            {
                if (buffer[0] == 0xff && buffer[1] == 0xfe)
                {
                    // UTF-16LE
                    return 2;
                }
                if (buffer[0] == 0xfe && buffer[1] == 0xff)
                {
                    // UTF-16BE
                    return 2;
                }

                if (buffer.Length >= 3)
                {
                    if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                    {
                        // UTF7;
                        return 3;
                    }

                    if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                    {
                        // UTF8
                        return 3;
                    }

                }

                if (buffer.Length >= 4)
                {
                    if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                    {
                        // UTF32;
                        return 4;
                    }
                }
            }

            return 0;
        }

        private void GenerateResourceFileRemark(MemoryStream resourceBuffer, ResourceConfigItem item, string fileName, string dirPathInFileName)
        {
            if (item.Remark != null)
            {
                Logger.ZLogTrace("Inserting resource file remark");

                var filePath = MakeStandardPath(Path.Combine(dirPathInFileName, fileName));

                var remarkInfo = new Dictionary<string, string>()
                {
                    ["name"] = fileName,
                    ["path"] = filePath
                };

                var remarkBuffer = ApplyTokens(item.Remark, remarkInfo);
                remarkBuffer.Replace(Emitter.CRLF, Emitter.NEW_LINE);

                var remarkLines = remarkBuffer.ToString().Split(new[] { Emitter.NEW_LINE }, StringSplitOptions.None);
                foreach (var remarkLine in remarkLines)
                {
                    var b = OutputEncoding.GetBytes(remarkLine);
                    resourceBuffer.Write(b, 0, b.Length);
                    NewLine(resourceBuffer);
                }
            }
        }

        public void PrepareResourcesConfig()
        {
            Logger.ZLogTrace("Preparing resources config...");

            var config = AssemblyInfo.Resources;

            var rawResources = config.Items;

            var resources = new List<ResourceConfigItem>();
            ResourceConfigItem defaultSetting = null;

            if (rawResources != null)
            {
                Array.ForEach(rawResources, (x) =>
                {
                    if (x.Name != null)
                    {
                        var parts = x.Name.Split(new[] { '#' }, StringSplitOptions.None);

                        if (parts.Length > 1)
                        {
                            x.Assembly = parts[0];
                            x.Name = parts[1];
                        }
                    }
                });

                var defaultResources = rawResources.Where(x => x.Name == null);

                if (defaultResources.Count() > 1)
                {
                    Logger.ZLogError("There are more than one default resource in the configuration setting file (resources section). Will use the first occurrence as a default resource settings");
                }

                defaultSetting = defaultResources.FirstOrDefault();

                if (defaultSetting != null)
                {
                    Logger.ZLogTrace("The resources config section has a default settings");

                    defaultSetting.SetDefaulValues();

                    var rawNonDefaultResources = rawResources.Where(x => x.Name != null);

                    ValidateResourceSettings(defaultSetting, rawNonDefaultResources);

                    foreach (var resource in rawNonDefaultResources)
                    {
                        ApplyDefaultResourceConfigSetting(defaultSetting, resource);

                        resources.Add(resource);
                    }
                }
                else
                {
                    Array.ForEach(rawResources, x => x.SetDefaulValues());
                    resources.AddRange(rawResources);
                }

                Logger.ZLogTrace("The resources config section has {0} non-default settings", resources.Count);
            }

            var toEmbed = resources.Where(x => x.Files != null && x.Files.Count() > 0).ToArray();
            var toExtract = resources.Where(x => x.Files == null || x.Files.Count() <= 0).ToArray();

            config.Prepare(defaultSetting, toEmbed, toExtract);
            Logger.ZLogTrace("Done preparing resources config");

            return;
        }

        private void ValidateResourceSettings(ResourceConfigItem defaultSetting, IEnumerable<ResourceConfigItem> rawNonDefaultResources)
        {
            var defaultExtract = defaultSetting.Extract ?? false;
            var rawNonDefaultResourcesWithExtractAndNoOutput = rawNonDefaultResources
                .Where(x => x.Output == null && (x.Extract == true || defaultExtract));

            if (defaultSetting.Output != null && rawNonDefaultResourcesWithExtractAndNoOutput.Count() > 1)
            {
                string defaultOutputFileName = null;

                try
                {
                    defaultOutputFileName = Path.GetFileName(defaultSetting.Output);
                }
                catch (Exception)
                {
                }

                if (!string.IsNullOrEmpty(defaultOutputFileName))
                {
                    Logger.ZLogError("The resource config setting has a default output setting {0} containing file part {1}.However, there are several resources with no output setting defined and active extract option. It means the resources will be overwritten by each other.", defaultSetting.Output, defaultOutputFileName);
                }
            }
        }

        private void ApplyDefaultResourceConfigSetting(ResourceConfigItem defaultSetting, ResourceConfigItem current)
        {
            if (!current.Extract.HasValue)
            {
                current.Extract = defaultSetting.Extract;
            }

            if (!current.Inject.HasValue)
            {
                current.Inject = defaultSetting.Inject;
            }

            if (current.Output == null)
            {
                current.Output = defaultSetting.Output;
            }

            if (current.Files == null && defaultSetting.Files != null)
            {
                current.Files = defaultSetting.Files.Select(x => x).ToArray();
            }

            if (current.Header == null)
            {
                current.Header = defaultSetting.Header;
            }

            if (current.Remark == null)
            {
                current.Remark = defaultSetting.Remark;
            }
        }

        /// <summary>
        /// Replaces platform specific dir separators to just /
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A path with replaced '\' -> '/'</returns>
        private string MakeStandardPath(string path)
        {
            if (path == null)
            {
                return path;
            }

            return path.Replace('\\', '/');
        }

        protected byte[] ReadStream(Stream stream)
        {
            if (stream is MemoryStream memoryStream)
                return memoryStream.ToArray();

            if (stream.Position != 0)
                stream.Seek(0, SeekOrigin.Begin);
            using (memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        //private string NormalizePath(string value)
        //{
        //    value = value.Replace(@"\", ".");
        //    string path = value.LeftOfRightmostOf('.').LeftOfRightmostOf('.');
        //    string name = value.Substring(path.Length);
        //    return path.Replace('-', '_') + name;
        //}
    }
}