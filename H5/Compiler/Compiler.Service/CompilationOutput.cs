using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace H5.Compiler
{
    public class CompilationOutput
    {
        public Dictionary<string, MemoryStream> Output { get; private set; }

        public Dictionary<string, int> Stats { get; set; }

        internal static CompilationOutput FromOutputLocation(string outputLocation)
        {
            outputLocation = Path.GetFullPath(outputLocation);
            return new CompilationOutput()
            {
                Output = Directory.EnumerateFiles(outputLocation, "*", SearchOption.AllDirectories).Where(f => File.Exists(f)).ToDictionary(f => f.Replace(outputLocation, "").TrimStart(Path.DirectorySeparatorChar), f => GetMemoryStream(f))
            };
        }

        public static MemoryStream GetMemoryStream(string filePath)
        {
            using var file = File.OpenRead(filePath);
            var ms = new MemoryStream((int)file.Length);
            file.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public static string GetAsText(MemoryStream stream)
        {
            using (var reader = new StreamReader(stream, leaveOpen: true))
            {
                var text = reader.ReadToEnd();
                stream.Seek(0, SeekOrigin.Begin);
                return text;
            }
        }
    }
}