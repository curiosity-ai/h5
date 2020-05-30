using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace H5.Compiler
{
    public class CompilationOutput
    {
        public Dictionary<string,string> Output { get; private set; }

        internal static CompilationOutput FromOutputLocation(string outputLocation)
        {
            outputLocation = Path.GetFullPath(outputLocation);
            return new CompilationOutput()
            {
                Output = Directory.EnumerateFiles(outputLocation, "*", SearchOption.AllDirectories).Where(f => File.Exists(f)).ToDictionary(f => f.Replace(outputLocation, "").TrimStart(Path.DirectorySeparatorChar), f => File.ReadAllText(f))
            };
        }
    }
}