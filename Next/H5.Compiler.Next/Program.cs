using System;
using System.IO;
using H5.Compiler.Service.Next;

namespace H5.Compiler.Next
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: h5-next <source-file.cs>");
                return;
            }

            var filePath = args[0];
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File '{filePath}' not found.");
                return;
            }

            var sourceCode = File.ReadAllText(filePath);
            var compiler = new H5Compiler();

            try
            {
                var javascript = compiler.Compile(sourceCode);

                var outPath = Path.ChangeExtension(filePath, ".js");
                File.WriteAllText(outPath, javascript);

                Console.WriteLine($"Successfully compiled to '{outPath}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Compilation failed: {ex.Message}");
            }
        }
    }
}
