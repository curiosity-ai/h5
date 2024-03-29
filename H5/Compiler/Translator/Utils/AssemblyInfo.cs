using H5.Contract;
using System.Collections.Generic;

namespace H5.Translator
{
    public class H5DotJson_AssemblySettings : IH5DotJson_AssemblySettings
    {
        public const string DEFAULT_FILENAME = "---";
        public const string DEFAULT_OUTPUT = "$(OutDir)/h5/";

        public H5DotJson_AssemblySettings()
        {
            Dependencies = new List<IModuleDependency>();
            Reflection = new ReflectionConfig();
            ReflectionInternal = new ReflectionConfig();
            Assembly = new AssemblyConfig();
            Resources = new ResourceConfig();
            Loader = new ModuleLoader();
            Output = DEFAULT_OUTPUT;
            SourceMap = new SourceMapConfig();
            Html = new HtmlConfig();
            Rules = new CompilerRule();
            IgnoreDuplicateTypes = false;
        }

        /// <summary>
        /// A file name where JavaScript is generated to. If omitted, it is [Namespace_Name].js by default.
        /// Example: "MyH5NetLibrary.js"
        /// Tip. You can decorate a class with a [FileName('MyClass.js')] attribute. A class script will be generated to the defined file. It supersedes a global h5.json fileName.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The output folder path for generated JavaScript. A non-absolute path is concatenated with a project's root.
        /// Examples: "H5\\output\\", "..\\H5\\output\\", "c:\\H5\\output\\"
        /// </summary>
        public string Output { get; set; }

        private OutputBy outputBy = OutputBy.Project;

        /// <summary>
        /// The option to manage JavaScript output folders and files.
        /// See the OutputBy enum for more details.
        /// </summary>
        public OutputBy OutputBy
        {
            get
            {
                if (CombineScripts || !string.IsNullOrEmpty(FileName))
                {
                    return OutputBy.Project;
                }

                return outputBy;
            }
            set
            {
                outputBy = value;
            }
        }

        private FileNameCaseConvert jsFileCasing = FileNameCaseConvert.None;

        /// <summary>
        /// The option to manage JavaScript file name case converting for class grouping.
        /// See the FileNameCaseConvert enum for more details.
        /// </summary>
        public FileNameCaseConvert FileNameCasing
        {
            get
            {
                return jsFileCasing;
            }
            set
            {
                jsFileCasing = value;
            }
        }

        private JavaScriptOutputType jsOutType = JavaScriptOutputType.Both;

        /// <summary>
        /// The option to select JavaScript file output for only beautified, only minified or both versions.
        /// See the JavaScriptOutputType enum for more details.
        /// </summary>
        public JavaScriptOutputType OutputFormatting
        {
            get
            {
                return jsOutType;
            }
            set
            {
                jsOutType = value;
            }
        }

        /// <summary>
        /// Substrings the file name starting with the defined index.
        /// For example, it might be useful to get rid of the first namespace in the chain if use ByFullName or ByNamespace FilesHierarchy.
        /// </summary>
        public int StartIndexInName { get; set; }

        /// <summary>
        /// The global Module setting. The entire project is considered as one Module.
        /// Though, you are still able to define a Module attribute on the class level.
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// The list of module dependencies.
        /// </summary>
        public List<IModuleDependency> Dependencies { get; set; }

        /// <summary>
        /// The executable file to be launched before building. The path will be concatenated with the project's root.
        /// For example, it might be used for cleaning up the output directory - "H5\\builder\\clean.bat".
        /// </summary>
        public string BeforeBuild { get; set; }

        /// <summary>
        /// The executable file to be launched after building. The path will be concatenated with the project's root.
        /// For example, it might be used for copying the generated JavaScript files to a Web application - "H5\\builder\\copy.bat"
        /// </summary>
        public string AfterBuild { get; set; }

        public bool AutoPropertyToField { get; set; }

        public string PluginsPath { get; set; }

        public bool GenerateTypeScript { get; set; }

        private DocumentationMode generateDocumentation = DocumentationMode.Basic;

        public DocumentationMode GenerateDocumentation
        {
            get
            {
                return generateDocumentation;
            }
            set
            {
                generateDocumentation = value;
            }
        }

        /// <summary>
        /// The BuildArguments will be added to the command line that build project files. It is useful for debugging, logging etc.
        /// For example, "/fileLogger /fileLoggerParameters:Append;"
        /// </summary>
        public string BuildArguments { get; set; }

        /// <summary>
        /// Deletes files from output directory using pattern "*.js|*.d.ts" before build (before extracting scripts after translation).
        /// It is useful to replace BeforeBuild event if it just contain commands to clean the output folder.
        /// Default value is null. It can be used either as string or bool value. True means "*.js|*.d.ts"
        /// </summary>
        public string CleanOutputFolderBeforeBuild { get; set; }

        /// <summary>
        /// Sets pattern for cleaning output directory.
        /// </summary>
        public string CleanOutputFolderBeforeBuildPattern { get; set; }

        public string Locales { get; set; }

        public string LocalesOutput { get; set; }

        public string LocalesFileName { get; set; }

        public bool CombineLocales { get; set; }

        public bool CombineScripts { get; set; }

        public bool UseTypedArrays { get; set; }

        public bool IgnoreCast { get; set; }

        public OverflowMode? OverflowMode { get; set; }

        public bool? NoLoggerTimeStamps { get; set; }

        public bool StrictNullChecks { get; set; }

        public IReflectionConfig Reflection { get; set; }

        internal IReflectionConfig ReflectionInternal { get; set; }

        public AssemblyConfig Assembly { get; set; }

        public ResourceConfig Resources { get; set; }

        public IModuleLoader Loader { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(SourceMapConfigConverter))]
        public SourceMapConfig SourceMap { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(HtmlConfigConverter))]
        public HtmlConfig Html { get; set; }

        public CompilerRule Rules { get; set; }

        public string ReferencesPath { get; set; }

        public string[] References { get; set; }

        /// <summary>
        /// Skips loading types off assemblies when they have already been loaded.
        /// If false, throws an exception when a same type comes from more than
        /// one assembly.
        /// </summary>
        public bool IgnoreDuplicateTypes { get; set; }
    }
}