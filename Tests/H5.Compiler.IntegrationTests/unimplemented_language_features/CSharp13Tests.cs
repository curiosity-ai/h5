using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp13Tests : IntegrationTestBase
    {
        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task ParamsCollections()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        PrintList(1, 2, 3);
        PrintSpan(4, 5, 6);
    }

    static void PrintList(params List<int> list)
    {
        Console.WriteLine(list.Count);
    }

    static void PrintSpan(params ReadOnlySpan<int> span)
    {
        Console.WriteLine(span.Length);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
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
        [Ignore("Not implemented yet")]
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
        [Ignore("Not implemented yet")]
        public async Task EscapeSequence()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        string s = "\e";
        Console.WriteLine((int)s[0]); // 27
    }
}
""";
            await RunTest(code);
        }
    }
}
