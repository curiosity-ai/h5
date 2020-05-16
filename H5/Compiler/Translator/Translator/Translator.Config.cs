using System.Diagnostics;
using System.IO;
using ZLogger;
using H5.Translator.Utils;

namespace H5.Translator
{
    public partial class Translator
    {
        public virtual void RunEvent(string e)
        {
            var info = new ProcessStartInfo()
            {
                FileName = e
            };
            info.WindowStyle = ProcessWindowStyle.Hidden;

            if (!File.Exists(e))
            {
                throw new TranslatorException($"The specified file '{e}' couldn't be found.\nWarning: Running H5 translator from directory: {Directory.GetCurrentDirectory()}");
            }

            using (var p = Process.Start(info))
            {
                p.WaitForExit();

                if (p.ExitCode != 0)
                {
                    throw new TranslatorException($"Error: The command '{e}' returned with exit code: {p.ExitCode}");
                }
            }
        }

        internal virtual void ApplyProjectPropertiesToConfig()
        {
            Logger.ZLogTrace("ApplyProjectPropertiesToConfig...");

            var configReader = new AssemblyConfigHelper();
            configReader.ApplyTokens(AssemblyInfo, ProjectProperties);

            Logger.ZLogTrace("ApplyProjectPropertiesToConfig done");
        }
    }
}