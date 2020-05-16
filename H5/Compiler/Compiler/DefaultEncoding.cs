using System;
using System.Text;
using System.Threading;

namespace H5.Builder
{
    public static class DefaultEncoding
    {
        public static void ForceInvariantCultureAndUTF8Output()
        {
            if (Environment.UserInteractive)
            {
                // A console is opened
                try
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.InputEncoding = Encoding.UTF8;
                }
                catch
                {
                    //This might throw if not running on a console, ignore as we don't care in that case
                }
            }
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        }
    }
}