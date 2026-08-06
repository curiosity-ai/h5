using H5.Contract.Constants;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace H5.Contract
{
    public interface IH5DotJson_AssemblySettings
    {
        List<IModuleDependency> Dependencies{ get; set; }

        string FileName{ get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        OutputBy OutputBy{ get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        FileNameCaseConvert FileNameCasing{ get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        JavaScriptOutputType OutputFormatting{ get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        OutputModuleType OutputModuleType{ get; set; }

        Module Module{ get; set; }

        string Output{ get; set; }

        int StartIndexInName{ get; set; }

        string BeforeBuild{ get; set; }

        string AfterBuild{ get; set; }

        bool AutoPropertyToField{ get; set; }

        string PluginsPath{ get; set; }

        bool GenerateTypeScript{ get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        DocumentationMode GenerateDocumentation{ get; set; }

        string BuildArguments{ get; set; }

        /// <summary>
        /// Deletes files from output directory using pattern "*.js|*.d.ts|*.css" before build (before extracting scripts after translation).
        /// It is useful to replace BeforeBuild event if it just contain commands to clean the output folder.
        /// </summary>
        [JsonConverter(typeof(StringBoolJsonConverter), "*" + Files.Extensions.JS + "|*" + Files.Extensions.DTS + "|*" + Files.Extensions.CSS)]
        string CleanOutputFolderBeforeBuild{ get; set; }

        /// <summary>
        /// Sets search pattern for cleaning output directory.
        /// </summary>
        string CleanOutputFolderBeforeBuildPattern{ get; set; }

        string Locales{ get; set; }

        string LocalesOutput{ get; set; }

        string LocalesFileName{ get; set; }

        bool CombineLocales{ get; set; }

        bool CombineScripts{ get; set; }

        bool UseTypedArrays{ get; set; }

        bool IgnoreCast{ get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        OverflowMode? OverflowMode{ get; set; }

        bool StrictNullChecks{ get; set; }

        IReflectionConfig Reflection{ get; set; }

        AssemblyConfig Assembly{ get; set; }

        ResourceConfig Resources{ get; set; }

        IModuleLoader Loader{ get; set; }

        SourceMapConfig SourceMap{ get; set; }

        HtmlConfig Html{ get; set; }

        CompilerRule Rules{ get; set; }

        string ReferencesPath { get; set; }

        string[] References { get; set; }

        bool IgnoreDuplicateTypes { get; set; }

        bool EnableCache { get; set; }

        /// <summary>
        /// When true, after the compilation completes, all files left in the project's
        /// output directory (typically the bin/$(OutDir) folder) other than those inside
        /// the H5 javascript/html output sub-folder are packed into an .h5 bundle file
        /// placed inside the H5 output folder. The .h5 file is a zip archive that can
        /// later be fed back to the compiler via the dependency-bundle injection API as
        /// if its contents were a regular project reference.
        /// </summary>
        bool BundleDependencies { get; set; }

        /// <summary>
        /// Optional name for the generated dependency bundle. Defaults to
        /// "$(AssemblyName).h5" when <see cref="BundleDependencies"/> is enabled.
        /// </summary>
        string BundleFileName { get; set; }
    }
 }
