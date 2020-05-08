using HighFive.Contract;
using HighFive.Translator;
using HighFive.Translator.Logging;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Linq;

namespace HighFive.Build
{
    public class HighFiveCompilerTask : Task
    {
        [Required]
        public ITaskItem Assembly
        {
            get;
            set;
        }

        public string OutputPath
        {
            get;
            set;
        }

        public string OutDir
        {
            get;
            set;
        }

        [Required]
        public string ProjectPath
        {
            get;
            set;
        }

        [Required]
        public string AssembliesPath
        {
            get;
            set;
        }

        [Required]
        public string AssemblyName
        {
            get;
            set;
        }

        [Required]
        public ITaskItem[] Sources
        {
            get;
            set;
        }

        public string CheckForOverflowUnderflow
        {
            get;
            set;
        }

        public bool NoCore
        {
            get;
            set;
        }

        public string Platform
        {
            get;
            set;
        }

        [Required]
        public string Configuration
        {
            get;
            set;
        }

        [Required]
        public string OutputType
        {
            get;
            set;
        }

        public string DefineConstants
        {
            get;
            set;
        }

        [Required]
        public string RootNamespace
        {
            get;
            set;
        }

#if DEBUG

        /// <summary>
        /// Attaches the process to a debugger once a build event is triggered. If false/absent, does nothing.
        /// This option is similar to Builder.exe's '-d'
        /// </summary>
        public bool AttachDebugger
        {
            get;
            set;
        }

#endif

        public override bool Execute()
        {
            var success = true;

#if DEBUG
            if (AttachDebugger)
            {
                System.Diagnostics.Debugger.Launch();
            };
#endif
            var logger = new Translator.Logging.Logger(null, false, LoggerLevel.Info, true, new VSLoggerWriter(this.Log), new FileLoggerWriter());

            logger.Trace("Executing HighFive.Build.Task...");

            var highfiveOptions = this.GetHighFiveOptions();

            var processor = new TranslatorProcessor(highfiveOptions, logger);

            try
            {
                processor.PreProcess();

                processor.Process();

                processor.PostProcess();
            }
            catch (EmitterException ex)
            {
                var errMsg = $"{ex.Message} {ex.StackTrace}";

                logger.Error(errMsg, ex.FileName, ex.StartLine + 1, ex.StartColumn + 1, ex.EndLine + 1, ex.EndColumn + 1);

                success = false;
            }
            catch (Exception ex)
            {
                var errMsg = $"{ex.Message} {ex.StackTrace}";

                var ee = processor.Translator != null ? processor.Translator.CreateExceptionFromLastNode() : null;

                if (ee != null)
                {
                    logger.Error(errMsg, ee.FileName, ee.StartLine + 1, ee.StartColumn + 1, ee.EndLine + 1, ee.EndColumn + 1);
                }
                else
                {
                    logger.Error(errMsg);
                }

                success = false;
            }

            processor = null;

            return success;
        }

        private HighFive.Translator.HighFiveOptions GetHighFiveOptions()
        {
            var highfiveOptions = new HighFive.Translator.HighFiveOptions()
            {
                ProjectLocation = this.ProjectPath,
                OutputLocation = this.OutputPath,
                DefaultFileName = Path.GetFileName(this.Assembly.ItemSpec),
                HighFiveLocation = Path.Combine(this.AssembliesPath, "HighFive.dll"),
                Rebuild = false,
                ExtractCore = !NoCore,
                Folder = null,
                Recursive = false,
                Lib = null,
                NoCompilation = false,
                NoTimeStamp = null,
                FromTask = true,
                Name = "",
                Sources = GetSources()
            };

            highfiveOptions.ProjectProperties = new ProjectProperties()
            {
                AssemblyName = this.AssemblyName,
                OutputPath = this.OutputPath,
                OutDir = this.OutDir,
                RootNamespace = this.RootNamespace,
                Configuration = this.Configuration,
                Platform = this.Platform,
                DefineConstants = this.DefineConstants,
                CheckForOverflowUnderflow = GetCheckForOverflowUnderflow(),
                OutputType = this.OutputType
            };

            return highfiveOptions;
        }

        private string GetSources()
        {
            if (this.Sources != null && this.Sources.Length > 0)
            {
                var result = string.Join(";", this.Sources.Select(x => x.ItemSpec));

                return result;
            }

            return null;
        }

        private bool? GetCheckForOverflowUnderflow()
        {
            if (this.CheckForOverflowUnderflow == null)
            {
                return null;
            }

            bool b;

            if (bool.TryParse(this.CheckForOverflowUnderflow, out b))
            {
                return b;
            }

            return null;
        }
    }
}