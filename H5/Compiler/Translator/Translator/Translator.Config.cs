using System.Diagnostics;
using System.IO;

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
                throw new TranslatorException("The specified file '" + e + "' couldn't be found." +
                    "\nWarning: H5 translator working directory: " + Directory.GetCurrentDirectory());
            }

            using (var p = Process.Start(info))
            {
                p.WaitForExit();

                if (p.ExitCode != 0)
                {
                    throw new TranslatorException("Error: The command '" + e + "' returned with exit code: " + p.ExitCode);
                }
            }
        }

        internal virtual void ApplyProjectPropertiesToConfig()
        {
            this.Log.Trace("ApplyProjectPropertiesToConfig...");

            var configReader = new AssemblyConfigHelper(this.Log);
            configReader.ApplyTokens(AssemblyInfo, ProjectProperties);

            this.Log.Trace("ApplyProjectPropertiesToConfig done");
        }
    }
}