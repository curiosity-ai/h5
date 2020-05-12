using H5.Contract;
using H5.Translator.Logging;
using H5.Translator.Utils;

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace H5.Translator
{
    public class TranslatorProcessor
    {
        public H5Options H5Options { get; private set; }

        public Logger Logger { get; private set; }

        public IAssemblyInfo TranslatorConfiguration { get; private set; }

        public Translator Translator { get; private set; }

        public TranslatorProcessor(H5Options h5Options, Logger logger)
        {
            this.H5Options = h5Options;
            this.Logger = logger;
        }

        public void PreProcess()
        {
            this.AdjustH5Options();

            this.TranslatorConfiguration = this.ReadConfiguration();

            this.Translator = this.SetTranslatorProperties();

            this.SetLoggerConfigurationParameters();
        }

        private void AdjustH5Options()
        {
            var pathHelper = new ConfigHelper();
            var h5Options = this.H5Options;

            h5Options.H5Location = pathHelper.ConvertPath(h5Options.H5Location);
            h5Options.DefaultFileName = pathHelper.ConvertPath(h5Options.DefaultFileName);
            h5Options.Folder = pathHelper.ConvertPath(h5Options.Folder);
            h5Options.Lib = pathHelper.ConvertPath(h5Options.Lib);
            h5Options.OutputLocation = pathHelper.ConvertPath(h5Options.OutputLocation);
            h5Options.ProjectLocation = pathHelper.ConvertPath(h5Options.ProjectLocation);
            h5Options.Sources = pathHelper.ConvertPath(h5Options.Sources);

            h5Options.ProjectProperties.OutputPath = pathHelper.ConvertPath(h5Options.ProjectProperties.OutputPath);
            h5Options.ProjectProperties.OutDir = pathHelper.ConvertPath(h5Options.ProjectProperties.OutDir);
        }

        public void Process()
        {
            this.Translator.Translate();
        }

        public string PostProcess()
        {
            var logger = this.Logger;
            var h5Options = this.H5Options;
            var translator = this.Translator;

            logger.Info("Post processing...");

            var outputPath = GetOutputFolder();

            logger.Info("outputPath is " + outputPath);

            translator.CleanOutputFolderIfRequired(outputPath);

            translator.PrepareResourcesConfig();

            var projectPath = Path.GetDirectoryName(translator.Location);
            logger.Info("projectPath is " + projectPath);

            if (h5Options.ExtractCore)
            {
                translator.ExtractCore(outputPath, projectPath);
            }
            else
            {
                logger.Info("No extracting core scripts option enabled");
            }

            var fileName = GetDefaultFileName(h5Options);

            translator.Minify();
            translator.Combine(fileName);
            translator.Save(outputPath, fileName);

            translator.InjectResources(outputPath, projectPath);

            translator.RunAfterBuild();

            logger.Info("Run plugins AfterOutput...");
            translator.Plugins.AfterOutput(translator, outputPath, !h5Options.ExtractCore);
            logger.Info("Done plugins AfterOutput");

            this.GenerateHtml(outputPath);

            translator.Report(outputPath);

            logger.Info("Done post processing");

            return outputPath;
        }

        private void GenerateHtml(string outputPath)
        {
            var htmlTitle = Translator.AssemblyInfo.Html.Title;

            if (string.IsNullOrEmpty(htmlTitle))
            {
                htmlTitle = Translator.GetAssemblyTitle();
            }

            var htmlGenerator = new HtmlGenerator(
                Translator.Log,
                Translator.AssemblyInfo,
                Translator.Outputs,
                htmlTitle
                );

            htmlGenerator.GenerateHtml(outputPath);
        }

        private string GetDefaultFileName(H5Options h5Options)
        {
            var defaultFileName = this.Translator.AssemblyInfo.FileName;

            if (string.IsNullOrEmpty(defaultFileName))
            {
                defaultFileName = h5Options.DefaultFileName;
            }

            if (string.IsNullOrEmpty(defaultFileName))
            {
                return AssemblyInfo.DEFAULT_FILENAME;
            }

            return Path.GetFileNameWithoutExtension(defaultFileName);
        }

        private string GetOutputFolder(bool basePathOnly = false, bool strict = false)
        {
            var h5Options = this.H5Options;
            string basePath = h5Options.IsFolderMode ? h5Options.Folder : Path.GetDirectoryName(h5Options.ProjectLocation);

            if (!basePathOnly)
            {
                string assemblyOutput = string.Empty;

                if (this.Translator != null)
                {
                    assemblyOutput = this.Translator.AssemblyInfo.Output;
                }
                else if (this.TranslatorConfiguration != null)
                {
                    assemblyOutput = this.TranslatorConfiguration.Output;
                }
                else if (strict)
                {
                    throw new InvalidOperationException("Could not get output folder as assembly configuration is still null");
                }
                else
                {
                    this.Logger.Warn("Could not get assembly output folder");
                }

                basePath = string.IsNullOrWhiteSpace(assemblyOutput)
                    ? Path.Combine(basePath, Path.GetDirectoryName(h5Options.OutputLocation))
                    : Path.Combine(basePath, assemblyOutput);
            }

            basePath = new ConfigHelper().ConvertPath(basePath);
            basePath = Path.GetFullPath(basePath);

            return basePath;
        }

        private IAssemblyInfo ReadConfiguration()
        {
            var h5Options = this.H5Options;

            var logger = this.Logger;

            var location = h5Options.IsFolderMode ? h5Options.Folder : h5Options.ProjectLocation;

            var configReader = new AssemblyConfigHelper(logger);

            return configReader.ReadConfig(h5Options.IsFolderMode, location, h5Options.ProjectProperties.Configuration);
        }

        private void SetLoggerConfigurationParameters()
        {
            var logger = this.Logger;
            var h5Options = this.H5Options;
            var assemblyConfig = this.TranslatorConfiguration;

            if (h5Options.NoLoggerSetUp)    
            {
                return;
            }

            logger.Trace("Applying logger configuration parameters...");

            logger.Name = h5Options.Name;

            if (!string.IsNullOrEmpty(logger.Name))
            {
                logger.Trace("Logger name: " + logger.Name);
            }

            var loggerLevel = assemblyConfig.Logging.Level ?? LoggerLevel.None;

            logger.Trace("Logger level: " + loggerLevel);

            if (loggerLevel <= LoggerLevel.None)
            {
                logger.Info("    To enable detailed logging, configure \"logging\" in h5.json.");
                logger.Info("    https://github.com/bridgedotnet/bridge/wiki/global-configuration#logging");
            }

            logger.LoggerLevel = loggerLevel;

            logger.Trace("Read config file: " + AssemblyConfigHelper.ConfigToString(assemblyConfig));

            logger.BufferedMode = false;

            if (h5Options.NoTimeStamp.HasValue)
            {
                logger.UseTimeStamp = !h5Options.NoTimeStamp.Value;
            }
            else if (assemblyConfig.Logging.TimeStamps.HasValue)
            {
                logger.UseTimeStamp = assemblyConfig.Logging.TimeStamps.Value;
            }
            else
            {
                logger.UseTimeStamp = true;
            }

            var fileLoggerWriter = logger.GetFileLogger();

            if (fileLoggerWriter != null)
            {
                string logFileFolder = GetLoggerFolder(assemblyConfig);

                fileLoggerWriter.SetParameters(logFileFolder, assemblyConfig.Logging.FileName, assemblyConfig.Logging.MaxSize);
            }

            logger.Flush();

            logger.Trace("Setting logger configuration parameters done");
        }

        private string GetLoggerFolder(IAssemblyInfo assemblyConfig)
        {
            var logFileFolder = assemblyConfig.Logging.Folder;

            if (string.IsNullOrWhiteSpace(logFileFolder))
            {
                logFileFolder = this.GetOutputFolder(false, false);
            }
            else if (!Path.IsPathRooted(logFileFolder))
            {
                logFileFolder = Path.Combine(this.GetOutputFolder(true, false), logFileFolder);
            }

            return logFileFolder;
        }

        private Translator SetTranslatorProperties()
        {
            var logger = this.Logger;
            var h5Options = this.H5Options;
            var assemblyConfig = this.TranslatorConfiguration;

            logger.Trace("Setting translator properties...");

            H5.Translator.Translator translator = null;

            // FIXME: detect by extension whether first argument is a project or DLL
            if (!h5Options.IsFolderMode)
            {
                translator = new H5.Translator.Translator(h5Options.ProjectLocation, h5Options.Sources, h5Options.FromTask);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(h5Options.Lib))
                {
                    throw new InvalidOperationException("Please define path to assembly using -lib option");
                }

                h5Options.Lib = Path.Combine(h5Options.Folder, h5Options.Lib);
                translator = new H5.Translator.Translator(h5Options.Folder, h5Options.Sources, h5Options.Recursive, h5Options.Lib);
            }

            translator.ProjectProperties = h5Options.ProjectProperties;

            translator.AssemblyInfo = assemblyConfig;

            if (this.H5Options.ReferencesPath != null)
            {
                translator.AssemblyInfo.ReferencesPath = this.H5Options.ReferencesPath;
            }

            translator.OverflowMode = h5Options.ProjectProperties.CheckForOverflowUnderflow.HasValue ?
                (h5Options.ProjectProperties.CheckForOverflowUnderflow.Value ? OverflowMode.Checked : OverflowMode.Unchecked) : (OverflowMode?)null;

            if (string.IsNullOrEmpty(h5Options.H5Location))
            {
                h5Options.H5Location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "H5.dll");
            }

            translator.H5Location = h5Options.H5Location;
            translator.Rebuild = h5Options.Rebuild;
            translator.Log = logger;

            if (h5Options.ProjectProperties.DefineConstants != null)
            {
                translator.DefineConstants.AddRange(h5Options.ProjectProperties.DefineConstants.Split(';').Select(s => s.Trim()).Where(s => s != ""));
                translator.DefineConstants = translator.DefineConstants.Distinct().ToList();
            }

            translator.Log.Trace("Translator properties:");
            translator.Log.Trace("\tH5Location:" + translator.H5Location);
            translator.Log.Trace("\tBuildArguments:" + translator.BuildArguments);
            translator.Log.Trace("\tDefineConstants:" + (translator.DefineConstants != null ? string.Join(" ", translator.DefineConstants) : ""));
            translator.Log.Trace("\tRebuild:" + translator.Rebuild);
            translator.Log.Trace("\tProjectProperties:" + translator.ProjectProperties);

            if (translator.FolderMode)
            {
                translator.ReadFolderFiles();

                if (!string.IsNullOrEmpty(assemblyConfig.FileName))
                {
                    translator.DefaultNamespace = Path.GetFileNameWithoutExtension(assemblyConfig.FileName);
                }
                else
                {
                    translator.DefaultNamespace = h5Options.DefaultFileName;
                }
            }
            else
            {
                translator.EnsureProjectProperties();
            }

            translator.ApplyProjectPropertiesToConfig();

            logger.Trace("Setting translator properties done");

            return translator;
        }
    }
}