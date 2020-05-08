using HighFive.Contract;
using HighFive.Translator.Logging;
using HighFive.Translator.Utils;

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HighFive.Translator
{
    public class TranslatorProcessor
    {
        public HighFiveOptions HighFiveOptions { get; private set; }

        public Logger Logger { get; private set; }

        public IAssemblyInfo TranslatorConfiguration { get; private set; }

        public Translator Translator { get; private set; }

        public TranslatorProcessor(HighFiveOptions highfiveOptions, Logger logger)
        {
            this.HighFiveOptions = highfiveOptions;
            this.Logger = logger;
        }

        public void PreProcess()
        {
            this.AdjustHighFiveOptions();

            this.TranslatorConfiguration = this.ReadConfiguration();

            this.Translator = this.SetTranslatorProperties();

            this.SetLoggerConfigurationParameters();
        }

        private void AdjustHighFiveOptions()
        {
            var pathHelper = new ConfigHelper();
            var highfiveOptions = this.HighFiveOptions;

            highfiveOptions.HighFiveLocation = pathHelper.ConvertPath(highfiveOptions.HighFiveLocation);
            highfiveOptions.DefaultFileName = pathHelper.ConvertPath(highfiveOptions.DefaultFileName);
            highfiveOptions.Folder = pathHelper.ConvertPath(highfiveOptions.Folder);
            highfiveOptions.Lib = pathHelper.ConvertPath(highfiveOptions.Lib);
            highfiveOptions.OutputLocation = pathHelper.ConvertPath(highfiveOptions.OutputLocation);
            highfiveOptions.ProjectLocation = pathHelper.ConvertPath(highfiveOptions.ProjectLocation);
            highfiveOptions.Sources = pathHelper.ConvertPath(highfiveOptions.Sources);

            highfiveOptions.ProjectProperties.OutputPath = pathHelper.ConvertPath(highfiveOptions.ProjectProperties.OutputPath);
            highfiveOptions.ProjectProperties.OutDir = pathHelper.ConvertPath(highfiveOptions.ProjectProperties.OutDir);
        }

        public void Process()
        {
            this.Translator.Translate();
        }

        public string PostProcess()
        {
            var logger = this.Logger;
            var highfiveOptions = this.HighFiveOptions;
            var translator = this.Translator;

            logger.Info("Post processing...");

            var outputPath = GetOutputFolder();

            logger.Info("outputPath is " + outputPath);

            translator.CleanOutputFolderIfRequired(outputPath);

            translator.PrepareResourcesConfig();

            var projectPath = Path.GetDirectoryName(translator.Location);
            logger.Info("projectPath is " + projectPath);

            if (highfiveOptions.ExtractCore)
            {
                translator.ExtractCore(outputPath, projectPath);
            }
            else
            {
                logger.Info("No extracting core scripts option enabled");
            }

            var fileName = GetDefaultFileName(highfiveOptions);

            translator.Minify();
            translator.Combine(fileName);
            translator.Save(outputPath, fileName);

            translator.InjectResources(outputPath, projectPath);

            translator.RunAfterBuild();

            logger.Info("Run plugins AfterOutput...");
            translator.Plugins.AfterOutput(translator, outputPath, !highfiveOptions.ExtractCore);
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

        private string GetDefaultFileName(HighFiveOptions highfiveOptions)
        {
            var defaultFileName = this.Translator.AssemblyInfo.FileName;

            if (string.IsNullOrEmpty(defaultFileName))
            {
                defaultFileName = highfiveOptions.DefaultFileName;
            }

            if (string.IsNullOrEmpty(defaultFileName))
            {
                return AssemblyInfo.DEFAULT_FILENAME;
            }

            return Path.GetFileNameWithoutExtension(defaultFileName);
        }

        private string GetOutputFolder(bool basePathOnly = false, bool strict = false)
        {
            var highfiveOptions = this.HighFiveOptions;
            string basePath = highfiveOptions.IsFolderMode ? highfiveOptions.Folder : Path.GetDirectoryName(highfiveOptions.ProjectLocation);

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
                    ? Path.Combine(basePath, Path.GetDirectoryName(highfiveOptions.OutputLocation))
                    : Path.Combine(basePath, assemblyOutput);
            }

            basePath = new ConfigHelper().ConvertPath(basePath);
            basePath = Path.GetFullPath(basePath);

            return basePath;
        }

        private IAssemblyInfo ReadConfiguration()
        {
            var highfiveOptions = this.HighFiveOptions;

            var logger = this.Logger;

            var location = highfiveOptions.IsFolderMode ? highfiveOptions.Folder : highfiveOptions.ProjectLocation;

            var configReader = new AssemblyConfigHelper(logger);

            return configReader.ReadConfig(highfiveOptions.IsFolderMode, location, highfiveOptions.ProjectProperties.Configuration);
        }

        private void SetLoggerConfigurationParameters()
        {
            var logger = this.Logger;
            var highfiveOptions = this.HighFiveOptions;
            var assemblyConfig = this.TranslatorConfiguration;

            if (highfiveOptions.NoLoggerSetUp)
            {
                return;
            }

            logger.Trace("Applying logger configuration parameters...");

            logger.Name = highfiveOptions.Name;

            if (!string.IsNullOrEmpty(logger.Name))
            {
                logger.Trace("Logger name: " + logger.Name);
            }

            var loggerLevel = assemblyConfig.Logging.Level ?? LoggerLevel.None;

            logger.Trace("Logger level: " + loggerLevel);

            if (loggerLevel <= LoggerLevel.None)
            {
                logger.Info("    To enable detailed logging, configure \"logging\" in highfive.json.");
                logger.Info("    https://github.com/curiosity-ai/h5/wiki/global-configuration#logging");
            }

            logger.LoggerLevel = loggerLevel;

            logger.Trace("Read config file: " + AssemblyConfigHelper.ConfigToString(assemblyConfig));

            logger.BufferedMode = false;

            if (highfiveOptions.NoTimeStamp.HasValue)
            {
                logger.UseTimeStamp = !highfiveOptions.NoTimeStamp.Value;
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
            var highfiveOptions = this.HighFiveOptions;
            var assemblyConfig = this.TranslatorConfiguration;

            logger.Trace("Setting translator properties...");

            HighFive.Translator.Translator translator = null;

            // FIXME: detect by extension whether first argument is a project or DLL
            if (!highfiveOptions.IsFolderMode)
            {
                translator = new HighFive.Translator.Translator(highfiveOptions.ProjectLocation, highfiveOptions.Sources, highfiveOptions.FromTask);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(highfiveOptions.Lib))
                {
                    throw new InvalidOperationException("Please define path to assembly using -lib option");
                }

                highfiveOptions.Lib = Path.Combine(highfiveOptions.Folder, highfiveOptions.Lib);
                translator = new HighFive.Translator.Translator(highfiveOptions.Folder, highfiveOptions.Sources, highfiveOptions.Recursive, highfiveOptions.Lib);
            }

            translator.ProjectProperties = highfiveOptions.ProjectProperties;

            translator.AssemblyInfo = assemblyConfig;

            if (this.HighFiveOptions.ReferencesPath != null)
            {
                translator.AssemblyInfo.ReferencesPath = this.HighFiveOptions.ReferencesPath;
            }

            translator.OverflowMode = highfiveOptions.ProjectProperties.CheckForOverflowUnderflow.HasValue ?
                (highfiveOptions.ProjectProperties.CheckForOverflowUnderflow.Value ? OverflowMode.Checked : OverflowMode.Unchecked) : (OverflowMode?)null;

            if (string.IsNullOrEmpty(highfiveOptions.HighFiveLocation))
            {
                highfiveOptions.HighFiveLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "HighFive.dll");
            }

            translator.HighFiveLocation = highfiveOptions.HighFiveLocation;
            translator.Rebuild = highfiveOptions.Rebuild;
            translator.Log = logger;

            if (highfiveOptions.ProjectProperties.DefineConstants != null)
            {
                translator.DefineConstants.AddRange(highfiveOptions.ProjectProperties.DefineConstants.Split(';').Select(s => s.Trim()).Where(s => s != ""));
                translator.DefineConstants = translator.DefineConstants.Distinct().ToList();
            }

            translator.Log.Trace("Translator properties:");
            translator.Log.Trace("\tHighFiveLocation:" + translator.HighFiveLocation);
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
                    translator.DefaultNamespace = highfiveOptions.DefaultFileName;
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