using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class NativeAsyncTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task AsyncReturnTask()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        int res = await GetValue();
        Console.WriteLine(res);
        Console.WriteLine("End");
        Console.WriteLine("<<DONE>>");
    }

    public static async Task<int> GetValue()
    {
        await Task.Delay(10);
        return 42;
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncVoid()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        DoVoid();
        await Task.Delay(50);
        Console.WriteLine("End");
        Console.WriteLine("<<DONE>>");
    }

    public static async void DoVoid()
    {
        await Task.Delay(10);
        Console.WriteLine("Void Done");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncWhileLoop()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        int i = 0;
        while (i < 3)
        {
            await Task.Delay(10);
            Console.WriteLine("i = " + i);
            i++;
        }
        Console.WriteLine("End");
        Console.WriteLine("<<DONE>>");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncException()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        try
        {
            await ThrowAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Caught: " + ex.Message);
        }
        Console.WriteLine("End");
        Console.WriteLine("<<DONE>>");
    }

    public static async Task ThrowAsync()
    {
        await Task.Delay(10);
        throw new Exception("Boom");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncRecursion()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        int res = await Fact(5);
        Console.WriteLine(res);
        Console.WriteLine("<<DONE>>");
    }

    public static async Task<int> Fact(int n)
    {
        if (n <= 1) return 1;
        await Task.Delay(1);
        return n * await Fact(n - 1);
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncWithGoto()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        await RunWithGoto();
        Console.WriteLine("End");
        Console.WriteLine("<<DONE>>");
    }

    public static async Task RunWithGoto()
    {
        Console.WriteLine("Step 1");
        await Task.Delay(10);
        goto JumpHere;

        Console.WriteLine("Skipped");

        JumpHere:
        Console.WriteLine("Step 2");
        await Task.Delay(10);
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncMultipleCatch()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        try
        {
            await Task.Delay(1);
            throw new InvalidOperationException("Test");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Caught InvalidOperationException: " + ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Caught Exception: " + e.Message);
        }
        Console.WriteLine("End");
        Console.WriteLine("<<DONE>>");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncLocals()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        var t1 = Process(10);
        var t2 = Process(20);

        int r1 = await t1;
        int r2 = await t2;

        Console.WriteLine(r1);
        Console.WriteLine(r2);
        Console.WriteLine("<<DONE>>");
    }

    public static async Task<int> Process(int val)
    {
        int x = val;
        await Task.Delay(10);
        return x;
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }
    }
}
