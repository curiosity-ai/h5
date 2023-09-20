using H5.Contract;
using Microsoft.Extensions.Logging;
using Mosaik.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZLogger;

namespace H5.Translator.Utils
{
    public class AssemblyConfigHelper
    {
        private const string CONFIG_FILE_NAME = "h5.json";

        private static ILogger Logger = ApplicationLogging.CreateLogger<AssemblyConfigHelper>();

        private ConfigHelper<H5DotJson_AssemblySettings> helper { get; set; }

        public AssemblyConfigHelper()
        {
            helper = new ConfigHelper<H5DotJson_AssemblySettings>();
        }

        private IH5DotJson_AssemblySettings ReadConfig(string configFileName, string location, string configuration)
        {
            var config = helper.ReadConfig(configFileName, location, configuration);

            if (config == null)
            {
                config = new H5DotJson_AssemblySettings();
            }

            // Convert '/' and '\\' to platform-specific path separator.
            ConvertConfigPaths(config);

            return config;
        }

        public IH5DotJson_AssemblySettings ReadConfig(string location, string configuration)
        {
            return ReadConfig(CONFIG_FILE_NAME, location, configuration);
        }

        private string CreateConfig(IH5DotJson_AssemblySettings h5Config, string folder)
        {
            var path = Path.Combine(folder, CONFIG_FILE_NAME);

            using (var textFile = File.CreateText(path))
            {
                var jss = new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };

                var config = JsonConvert.SerializeObject(h5Config, jss);

                textFile.Write(config);
            }

            return path;
        }

        public static string ConfigToString(IH5DotJson_AssemblySettings config)
        {
            return JsonConvert.SerializeObject(config);
        }

        public void ApplyTokens(IH5DotJson_AssemblySettings config, ProjectProperties projectProperties)
        {
            Logger.LogTrace("ApplyTokens ...");

            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (projectProperties == null)
            {
                throw new ArgumentNullException("projectProperties");
            }

            Logger.LogTrace("Properties:" + projectProperties.ToString());

            var tokens = projectProperties.GetValues();

            if (tokens == null || tokens.Count <= 0)
            {
                Logger.LogTrace("No tokens applied as no values to apply");
                return;
            }

            config.FileName = helper.ApplyPathTokens(tokens, config.FileName);
            config.Output = helper.ApplyPathTokens(tokens, config.Output);
            config.BeforeBuild = helper.ApplyPathTokens(tokens, config.BeforeBuild);
            config.AfterBuild = helper.ApplyPathTokens(tokens, config.AfterBuild);
            config.PluginsPath = helper.ApplyPathTokens(tokens, config.PluginsPath);
            config.CleanOutputFolderBeforeBuild = helper.ApplyTokens(tokens, config.CleanOutputFolderBeforeBuild);
            config.CleanOutputFolderBeforeBuildPattern = helper.ApplyTokens(tokens, config.CleanOutputFolderBeforeBuildPattern);
            config.Locales = helper.ApplyTokens(tokens, config.Locales);
            config.LocalesOutput = helper.ApplyTokens(tokens, config.LocalesOutput);
            config.LocalesFileName = helper.ApplyPathTokens(tokens, config.LocalesFileName);

            if (config.Reflection != null)
            {
                config.Reflection.Filter = helper.ApplyTokens(tokens, config.Reflection.Filter);
                config.Reflection.Output = helper.ApplyTokens(tokens, config.Reflection.Output);
            }

            if (config.Resources != null)
            {
                foreach (var resourceItem in config.Resources.Items)
                {
                    resourceItem.Header = helper.ApplyTokens(tokens, resourceItem.Header);
                    resourceItem.Name   = helper.ApplyTokens(tokens, resourceItem.Name);
                    resourceItem.Remark = helper.ApplyTokens(tokens, resourceItem.Remark);

                    if (resourceItem.Files is object)
                    {
                        for (int i = 0; i < resourceItem.Files.Length; i++)
                        {
                            resourceItem.Files[i] = helper.ApplyPathTokens(tokens, resourceItem.Files[i]);
                        }
                    }
                }
            }

            if (config.Html != null)
            {
                config.Html.Name = helper.ApplyTokens(tokens, config.Html.Name);
            }

            Logger.LogTrace("ApplyTokens done");
        }

        public void ConvertConfigPaths(IH5DotJson_AssemblySettings assemblyInfo)
        {
            assemblyInfo.AfterBuild = helper.ConvertPath(assemblyInfo.AfterBuild);
            assemblyInfo.BeforeBuild = helper.ConvertPath(assemblyInfo.BeforeBuild);
            assemblyInfo.Output = helper.ConvertPath(assemblyInfo.Output);
            assemblyInfo.PluginsPath = helper.ConvertPath(assemblyInfo.PluginsPath);
            assemblyInfo.LocalesOutput = helper.ConvertPath(assemblyInfo.LocalesOutput);

            if (assemblyInfo.Html != null)
            {
                assemblyInfo.Html.Name = helper.ConvertPath(assemblyInfo.Html.Name);
            }

            if (assemblyInfo.Resources != null)
            {
                foreach (var resourceConfigItem in assemblyInfo.Resources.Items)
                {
                    if (resourceConfigItem.Files is object)
                    {
                        for (int i = 0; i < resourceConfigItem.Files.Length; i++)
                        {
                            var resourceItem = resourceConfigItem.Files[i];
                            resourceConfigItem.Files[i] = helper.ConvertPath(resourceItem);
                        }
                    }

                    resourceConfigItem.Output = helper.ConvertPath(resourceConfigItem.Output);
                }

                assemblyInfo.Resources.Items = ExpandGlobs(assemblyInfo.Resources.Items);
            }
        }

        public static ResourceConfigItem[] ExpandGlobs(ResourceConfigItem[] inputItems)
        {
            var final = new List<ResourceConfigItem>();

            foreach (var item in inputItems)
            {
                if(item.Files is object && item.Files.Any(f => f.Contains('*')))
                {
                    foreach (var f in item.Files)
                    {
                        if (f.Contains("*"))
                        {
                            var parts = f.Split(new char[] { '*' }, 2);
                            if (Directory.Exists(parts[0]))
                            {
                                var fullBasePath = Path.GetFullPath(parts[0]).TrimEnd(Path.DirectorySeparatorChar);
                                foreach (var file in Directory.EnumerateFiles(fullBasePath, "*" + parts[1], SearchOption.AllDirectories))
                                {
                                    if (!Path.GetFileName(file).Contains('.'))
                                    {
                                        throw new Exception($"Error parsing resource {item.Name}: Extensionless files not supported: {file}, add an extension and try again.");
                                    }

                                    if (file.EndsWith(".DS_Store", StringComparison.OrdinalIgnoreCase)) continue;

                                    var finalPath = Path.GetFullPath(Path.GetDirectoryName(file)).TrimEnd(Path.DirectorySeparatorChar);
                                    string outputPath;
                                    Logger.ZLogTrace("fullBasePath={0}   finalPath={1}", fullBasePath, finalPath);
                                    if(fullBasePath == finalPath)
                                    {
                                        outputPath = item.Output;
                                    }
                                    else
                                    {
                                        outputPath = item.Output + finalPath.Replace(fullBasePath, "").TrimStart(Path.DirectorySeparatorChar);
                                    }
                                    Logger.ZLogInformation("Found {0} when expanding {1}, to copy to {2}", file, f, outputPath);
                                    final.Add(item.CloneWithFile(file, outputPath));
                                }
                            }
                        }
                        else
                        {
                            throw new Exception($"Error parsing resource {item.Name}: Mixing globs and files is not supported.");
                        }
                    }
                }
                else
                {
                    final.Add(item);
                }
            }
            return final.ToArray();
        }

    }
}