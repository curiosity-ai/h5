using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp13Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task LockObject()
        {
            var code = """
using System;
using System.Threading;

public class Program
{
    // Requires System.Threading.Lock type availability
    private readonly Lock _lock = new();

    public void DoWork()
    {
        lock (_lock)
        {
            Console.WriteLine("Locked");
        }
    }

    public static void Main()
    {
        var p = new Program();
        p.DoWork();
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ImplicitIndexAccess()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Buffer
{
    public int[] Data { get; } = new int[10];
}

public class Program
{
    public static void Main()
    {
        var b = new Buffer
        {
            Data =
            {
                [^1] = 42
            }
        };
        Console.WriteLine(b.Data[9]);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task EscapeSequence()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine((int)"\e"[0]); // 27
        Console.WriteLine((int)"\a"[0]); // 7
        Console.WriteLine((int)"\b"[0]); // 8
        Console.WriteLine((int)"\f"[0]); // 12
        Console.WriteLine((int)"\n"[0]); // 10
        Console.WriteLine((int)"\r"[0]); // 13
        Console.WriteLine((int)"\t"[0]); // 9
        Console.WriteLine((int)"\v"[0]); // 11
        Console.WriteLine((int)"\'"[0]); // 39
        Console.WriteLine((int)"\""[0]); // 34
        Console.WriteLine((int)"\\"[0]); // 92
        Console.WriteLine((int)"\0"[0]); // 0
    }
}
""";
            await RunTest(code);
        }
    }
}
