using H5.Contract;
using Microsoft.Extensions.Logging;
using H5.Translator.Utils;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mosaik.Core;
using ZLogger;
using System.Threading;

namespace H5.Translator
{
    public class TranslatorProcessor
    {
        private static ILogger Logger = ApplicationLogging.CreateLogger<TranslatorProcessor>();

        public CompilationOptions H5Options { get; private set; }

        public IAssemblyInfo TranslatorConfiguration { get; private set; }

        public Translator Translator { get; private set; }

        private readonly CancellationToken _cancellationToken;

        public TranslatorProcessor(CompilationOptions h5Options, CancellationToken cancellationToken)
        {
            H5Options = h5Options;
            _cancellationToken = cancellationToken;
        }

        public void PreProcess()
        {
            AdjustH5Options();

            _cancellationToken.ThrowIfCancellationRequested();
            TranslatorConfiguration = ReadConfiguration();

            _cancellationToken.ThrowIfCancellationRequested();
            Translator = SetTranslatorProperties();

            Logger.LogInformation($"Ready to build {H5Options.ProjectLocation}");
        }

        private void AdjustH5Options()
        {
            var pathHelper = new ConfigHelper();
            var h5Options = H5Options;

            h5Options.H5Location = pathHelper.ConvertPath(h5Options.H5Location);
            h5Options.DefaultFileName = pathHelper.ConvertPath(h5Options.DefaultFileName);
            h5Options.OutputLocation = pathHelper.ConvertPath(h5Options.OutputLocation);
            h5Options.ProjectLocation = pathHelper.ConvertPath(h5Options.ProjectLocation);
            h5Options.ProjectProperties.OutputPath = pathHelper.ConvertPath(h5Options.ProjectProperties.OutputPath);
            h5Options.ProjectProperties.OutDir = pathHelper.ConvertPath(h5Options.ProjectProperties.OutDir);
        }

        public void Process()
        {
            Translator.Translate(_cancellationToken);
        }

        public string PostProcess()
        {
            var h5Options = H5Options;
            var translator = Translator;
            var outputPath = GetOutputFolder();
            var projectPath = Path.GetDirectoryName(translator.Location);

            using (new Measure(Logger, $"Post-processing output on '{outputPath}' for project '{translator.Location}'"))
            {
                _cancellationToken.ThrowIfCancellationRequested();
                translator.CleanOutputFolderIfRequired(outputPath);

                _cancellationToken.ThrowIfCancellationRequested();
                translator.PrepareResourcesConfig();

                _cancellationToken.ThrowIfCancellationRequested();
                if (!translator.SkipResourcesExtraction)
                {
                    translator.ExtractCore(outputPath, projectPath);
                }
                else
                {
                    Logger.ZLogInformation("Skipping extracting resources from referenced projects & packages");
                }

                var fileName = GetDefaultFileName(h5Options);

                if (!translator.SkipEmbeddingResources)
                {
                    _cancellationToken.ThrowIfCancellationRequested();
                    translator.Minify();
                    
                    _cancellationToken.ThrowIfCancellationRequested();
                    translator.Combine(fileName);
                    
                    _cancellationToken.ThrowIfCancellationRequested();
                    translator.Save(outputPath, fileName);

                    _cancellationToken.ThrowIfCancellationRequested();
                    translator.InjectResources(outputPath, projectPath);
                }

                _cancellationToken.ThrowIfCancellationRequested();
                translator.RunAfterBuild();

                _cancellationToken.ThrowIfCancellationRequested();

                if (!translator.SkipHtmlGeneration)
                {
                    GenerateHtmlIfNeeded(outputPath);
                }

                return outputPath;
            }
        }

        private void GenerateHtmlIfNeeded(string outputPath)
        {
            if (Translator.AssemblyInfo.Html.Disabled)
            {
                Logger.LogTrace("GenerateHtml skipped as disabled in config.");
                return;
            }
            else
            {
                var htmlTitle = Translator.AssemblyInfo.Html.Title;

                if (string.IsNullOrEmpty(htmlTitle))
                {
                    htmlTitle = Translator.GetAssemblyTitle();
                }

                var htmlGenerator = new HtmlGenerator(Translator.AssemblyInfo, Translator.Outputs, htmlTitle, Translator.ProjectProperties.Configuration);

                htmlGenerator.GenerateHtml(outputPath);
            }
        }

        private string GetDefaultFileName(CompilationOptions h5Options)
        {
            var defaultFileName = Translator.AssemblyInfo.FileName;

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
            var h5Options = H5Options;
            string basePath = Path.GetDirectoryName(h5Options.ProjectLocation);

            if (!basePathOnly)
            {
                string assemblyOutput = string.Empty;

                if (Translator != null)
                {
                    assemblyOutput = Translator.AssemblyInfo.Output;
                }
                else if (TranslatorConfiguration != null)
                {
                    assemblyOutput = TranslatorConfiguration.Output;
                }
                else if (strict)
                {
                    throw new InvalidOperationException("Could not get output folder as assembly configuration is still null");
                }
                else
                {
                    Logger.LogWarning("Could not get assembly output folder");
                }

                basePath = string.IsNullOrWhiteSpace(assemblyOutput)
                    ? Path.Combine(basePath, Path.GetDirectoryName(h5Options.OutputLocation))
                    : Path.Combine(basePath, assemblyOutput);
            }

            basePath = new ConfigHelper().ConvertPath(basePath);
            basePath = Path.GetFullPath(basePath);

            if (!string.IsNullOrEmpty(basePath))
            {
                try
                {
                    Directory.CreateDirectory(basePath);
                }
                catch (Exception E)
                {
                    Logger.ZLogError(E, "Failed to create output directory {0}", basePath);
                }
            }

            return basePath;
        }

        private IAssemblyInfo ReadConfiguration()
        {
            var h5Options = H5Options;
            var location = h5Options.ProjectLocation;
            var configReader = new AssemblyConfigHelper();
            return configReader.ReadConfig(location, h5Options.ProjectProperties.Configuration);
        }


        private Translator SetTranslatorProperties()
        {
            var h5Options = H5Options;
            var assemblyConfig = TranslatorConfiguration;

            Logger.LogTrace("Setting translator properties...");

            Translator translator = null;

            // FIXME: detect by extension whether first argument is a project or DLL
            translator = new Translator(h5Options.ProjectLocation);

            translator.ProjectProperties = h5Options.ProjectProperties;

            translator.AssemblyInfo = assemblyConfig;

            if (H5Options.ReferencesPath != null)
            {
                translator.AssemblyInfo.ReferencesPath = H5Options.ReferencesPath;
            }

            translator.OverflowMode = h5Options.ProjectProperties.CheckForOverflowUnderflow.HasValue ?
                (h5Options.ProjectProperties.CheckForOverflowUnderflow.Value ? OverflowMode.Checked : OverflowMode.Unchecked) : (OverflowMode?)null;

            if (string.IsNullOrEmpty(h5Options.H5Location))
            {
                h5Options.H5Location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "H5.dll");
            }

            translator.H5Location = h5Options.H5Location;
            translator.Rebuild = h5Options.Rebuild;

            if (h5Options.ProjectProperties.DefineConstants != null)
            {
                translator.DefineConstants.AddRange(h5Options.ProjectProperties.DefineConstants.Split(';').Select(s => s.Trim()).Where(s => s != ""));
                translator.DefineConstants = translator.DefineConstants.Distinct().ToList();
            }

           Logger.LogTrace("Translator properties:");
           Logger.LogTrace("\tH5Location:" + translator.H5Location);
           Logger.LogTrace("\tBuildArguments:" + translator.BuildArguments);
           Logger.LogTrace("\tDefineConstants:" + (translator.DefineConstants != null ? string.Join(" ", translator.DefineConstants) : ""));
           Logger.LogTrace("\tRebuild:" + translator.Rebuild);
           Logger.LogTrace("\tProjectProperties:" + translator.ProjectProperties);

            translator.EnsureProjectProperties();

            translator.ApplyProjectPropertiesToConfig();

            Logger.LogTrace("Setting translator properties done");

            return translator;
        }
    }
}