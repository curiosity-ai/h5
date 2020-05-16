using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using H5.Contract.Constants;
using H5.Translator.Utils;
using ZLogger;

namespace H5.Translator
{
    public partial class Translator
    {
        // ensure Microsoft.Build.Utilities.Core is copied
        private static readonly Type UtilityType = typeof(Microsoft.Build.Utilities.ToolLocationHelper);

        public static class ProjectPropertyNames
        {
            public const string OUTPUT_TYPE_PROP = "OutputType";
            public const string ASSEMBLY_NAME_PROP = "AssemblyName";
            public const string DEFINE_CONSTANTS_PROP = "DefineConstants";
            public const string ROOT_NAMESPACE_PROP = "RootNamespace";
            public const string OUTPUT_PATH_PROP = "OutputPath";
            public const string OUT_DIR_PROP = "OutDir";
            public const string CONFIGURATION_PROP = "Configuration";
            public const string PLATFORM_PROP = "Platform";
        }

        private bool ShouldReadProjectFile { get; set; }

        internal static Project OpenProject(string location, IDictionary<string,string> globalProperties)
        {
            return new Project(location, globalProperties, null, new ProjectCollection());
        }

        internal static void CloseProject(Project project)
        {
            project.ProjectCollection.UnloadProject(project);
        }

        internal virtual void EnsureProjectProperties()
        {
            Logger.ZLogTrace("EnsureProjectProperties at " + (Location ?? "") + " ...");

            ShouldReadProjectFile = !FromTask;

            var project = OpenProject(Location, GetEvaluationConditions());

            ValidateProject(project);

            EnsureOverflowMode(project);

            EnsureDefaultNamespace(project);

            EnsureAssemblyName(project);

            EnsureAssemblyLocation(project);

            SourceFiles = GetSourceFiles(project);
            ParsedSourceFiles = new ParsedSourceFile[SourceFiles.Count];

            EnsureDefineConstants(project);

            CloseProject(project);

            Logger.ZLogTrace("EnsureProjectProperties done");
        }

        /// <summary>
        /// Validates project and namespace names against conflicts with H5 namespaces.
        /// </summary>
        /// <param name="project">XDocument reference of the .csproj file.</param>
        private void ValidateProject(Project project)
        {
            var valid = true;
            var failList = new HashSet<string>();
            var failNodeList = new List<ProjectProperty>();
            var combined_tags = from x in project.AllEvaluatedProperties
                                where x.Name == ProjectPropertyNames.ROOT_NAMESPACE_PROP || x.Name == ProjectPropertyNames.ASSEMBLY_NAME_PROP
                                select x;

            if (!AssemblyInfo.Assembly.EnableReservedNamespaces)
            {
                foreach (var tag in combined_tags)
                {
                    if (tag.EvaluatedValue == CS.NS.H5)
                    {
                        valid = false;
                        if (!failList.Contains(tag.EvaluatedValue))
                        {
                            failList.Add(tag.EvaluatedValue);
                            failNodeList.Add(tag);
                        }
                    }
                }
            }

            if (!valid)
            {
                var offendingSettings = "";
                foreach (var tag in failNodeList)
                {
                    offendingSettings += "Line " + tag.Xml.Location.File + " (" + tag.Xml.Location.Line + "): <" + tag.Xml.Name + ">" +
                                         tag.UnevaluatedValue + "</" + tag.Xml.Name + ">\n";
                }

                throw new TranslatorException("'H5' name is reserved and may not " +
                    "be used as project names or root namespaces.\n" +
                    "Please verify your project settings and rename where it applies.\n" +
                    "Project file: " + Location + "\n" +
                    "Offending settings:\n" + offendingSettings
                );
            }

            var outputType = ProjectProperties.OutputType;

            if (outputType == null && ShouldReadProjectFile)
            {
                var projectType = (from n in project.AllEvaluatedProperties
                                   where n.Name == ProjectPropertyNames.OUTPUT_TYPE_PROP
                                   select n).LastOrDefault();

                if (projectType != null)
                {
                    outputType = projectType.EvaluatedValue;
                }
            }

            if (outputType != null && string.Compare(outputType, SupportedProjectType, true) != 0)
            {
                TranslatorException.Throw("Project type ({0}) is not supported, please use Library instead of {0}", outputType);
            }
        }

        private void EnsureOverflowMode(Project project)
        {
            if (!ShouldReadProjectFile)
            {
                return;
            }

            if (OverflowMode.HasValue)
            {
                return;
            }

            var property = (from n in project.AllEvaluatedProperties
                        where n.Name == "CheckForOverflowUnderflow"
                        select n).LastOrDefault();

            if (property != null)
            {
                var value = property.EvaluatedValue;
                bool boolValue;
                if (bool.TryParse(value, out boolValue))
                {
                    OverflowMode = boolValue ? Contract.OverflowMode.Checked : Contract.OverflowMode.Unchecked;
                }
            }
        }

        protected virtual void EnsureAssemblyLocation(Project project)
        {
            Logger.ZLogTrace("BuildAssemblyLocation...");

            if (string.IsNullOrEmpty(AssemblyLocation))
            {
                var fullOutputPath = GetOutputPaths(project);

                Logger.ZLogTrace("    FullOutputPath: {0}", fullOutputPath);

                AssemblyLocation = Path.Combine(fullOutputPath, ProjectProperties.AssemblyName + ".dll");
            }

            Logger.ZLogTrace("    OutDir: {0}", ProjectProperties.OutDir);
            Logger.ZLogTrace("    OutputPath: {0}", ProjectProperties.OutputPath);
            Logger.ZLogTrace("    AssemblyLocation: {0}", AssemblyLocation);

            Logger.ZLogTrace("BuildAssemblyLocation done");
        }

        protected virtual string GetOutputPaths(Project project)
        {
            var configHelper = new Contract.ConfigHelper();

            var outputPath = ProjectProperties.OutputPath;

            if (outputPath == null && ShouldReadProjectFile)
            {
                // Read OutputPath if not defined already
                // Throw exception if not found
                outputPath = ReadProperty(project, ProjectPropertyNames.OUTPUT_PATH_PROP, false, configHelper);
            }

            outputPath = outputPath  ?? "";

            ProjectProperties.OutputPath = outputPath;

            var outDir = ProjectProperties.OutDir;

            if (outDir is null && ShouldReadProjectFile)
            {
                // Read OutDir if not defined already
                outDir = ReadProperty(project, ProjectPropertyNames.OUT_DIR_PROP, true, configHelper);
            }

            // If OutDir value is not found then use OutputPath value
            ProjectProperties.OutDir = outDir ?? outputPath;

            var fullPath = ProjectProperties.OutDir;

            if (!Path.IsPathRooted(fullPath))
            {
                fullPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Location), fullPath));
            }

            fullPath = configHelper.ConvertPath(fullPath);

            return fullPath;
        }

        private string ReadProperty(Project doc, string name, bool safe, Contract.ConfigHelper configHelper)
        {
            var node = (from n in doc.AllEvaluatedProperties
                        where string.Compare(n.Name, name, true) == 0
                        select n).LastOrDefault();

            if (node is null)
            {
                if (safe) { return null; }

                TranslatorException.Throw("Unable to determine " + name + " in the project file with conditions " + EvaluationConditionsAsString());
            }

            var value = node.EvaluatedValue;
            value = configHelper.ConvertPath(value);

            return value;
        }

        private Dictionary<string, string> GetEvaluationConditions()
        {
            var properties = new Dictionary<string, string>();

            if (ProjectProperties.Configuration != null)
            {
                properties.Add(ProjectPropertyNames.CONFIGURATION_PROP, ProjectProperties.Configuration);
            }

            properties.Add(ProjectPropertyNames.PLATFORM_PROP, "AnyCPU");

            return properties;
        }

        private string EvaluationConditionsAsString()
        {
            var conditions = string.Join(", ", GetEvaluationConditions().Select(x => x.Key + ": " + x.Value));

            return conditions;
        }

        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        protected virtual IList<string> GetSourceFiles(Project project)
        {
            Logger.ZLogTrace("Getting source files by xml...");
            var configHelper = new Contract.ConfigHelper();

            IList<string> sourceFiles = new List<string>();

            foreach (var projectItem in project.AllEvaluatedItems.Where(i=>i.ItemType == "Compile"))
            {
                sourceFiles.Add(configHelper.ConvertPath(projectItem.EvaluatedInclude));
            }

            if (!sourceFiles.Any())
            {
                throw new TranslatorException("Unable to get source file list from project file '" + Location + "'. In order to use h5, you have to have at least one source code file " + "with the 'compile' property set (usually .cs files have it by default in C# projects).");
            };

            Logger.ZLogTrace("Getting source files by xml done");

            return sourceFiles;
        }

        protected virtual void EnsureDefineConstants(Project project)
        {
            Logger.ZLogTrace("EnsureDefineConstants...");

            if (DefineConstants == null)
            {
                DefineConstants = new List<string>();
            }

            if (ProjectProperties.DefineConstants == null && ShouldReadProjectFile)
            {
                Logger.ZLogTrace("Reading define constants...");

                var constantList = project.AllEvaluatedProperties
                    .Where(p => p.Name == ProjectPropertyNames.DEFINE_CONSTANTS_PROP)
                    .Select(p => p.EvaluatedValue);

                if (constantList == null || constantList.Count() < 1)
                {
                    ProjectProperties.DefineConstants = "";
                }
                else
                {
                    ProjectProperties.DefineConstants = string.Join(";", constantList);
                }
            }

            if (!string.IsNullOrWhiteSpace(ProjectProperties.DefineConstants))
            {
                DefineConstants.AddRange(ProjectProperties.DefineConstants.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()));
            }

            DefineConstants = DefineConstants.Distinct().ToList();

            Logger.ZLogTrace("EnsureDefineConstants done");
        }

        protected virtual void EnsureAssemblyName(Project project)
        {
            if (ProjectProperties.AssemblyName == null && ShouldReadProjectFile)
            {
                var property = (from n in project.AllEvaluatedProperties
                            where n.Name == ProjectPropertyNames.ASSEMBLY_NAME_PROP
                            select n).LastOrDefault();

                if (property != null)
                {
                    ProjectProperties.AssemblyName = property.EvaluatedValue;
                }
            }

            if (string.IsNullOrWhiteSpace(ProjectProperties.AssemblyName))
            {
                TranslatorException.Throw("Unable to determine assembly name");
            }
        }

        protected virtual void EnsureDefaultNamespace(Project project)
        {
            if (ProjectProperties.RootNamespace == null && ShouldReadProjectFile)
            {
                var property = (from n in project.AllEvaluatedProperties
                            where n.Name == ProjectPropertyNames.ROOT_NAMESPACE_PROP
                            select n).LastOrDefault();

                if (property != null)
                {
                    ProjectProperties.RootNamespace = property.EvaluatedValue;
                }
            }

            DefaultNamespace = ProjectProperties.RootNamespace;

            if (string.IsNullOrWhiteSpace(DefaultNamespace))
            {
                DefaultNamespace = DefaultRootNamespace;
            }

            Logger.ZLogTrace("DefaultNamespace:" + DefaultNamespace);
        }

        protected virtual IList<string> GetSourceFiles(string location)
        {
            Logger.ZLogTrace("Getting source files by location...");
            string[] allfiles = Directory.GetFiles(location, "*.cs", SearchOption.TopDirectoryOnly);
            Logger.ZLogTrace("Getting source files by location done (found {0} items)", allfiles.Length);
            return allfiles;
        }
    }
}