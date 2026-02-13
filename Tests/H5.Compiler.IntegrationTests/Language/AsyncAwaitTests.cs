using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class AsyncAwaitTests : IntegrationTestBase
    {
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
        DoAsync();
        Console.WriteLine("End");
        await Task.Delay(200); // Give time for async void to complete
        Console.WriteLine("<<DONE>>");
    }

    public static async void DoAsync()
    {
        await Task.Delay(10);
        Console.WriteLine("Async Void Completed");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncTask()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        await Run();
        Console.WriteLine("End");
        Console.WriteLine("<<DONE>>");
    }

    public static async Task Run()
    {
        await DoAsync();
    }

    public static async Task DoAsync()
    {
        await Task.Delay(10);
        Console.WriteLine("Async Task Completed");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AwaitTaskDelay()
        {
             var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        await Run();
        Console.WriteLine("<<DONE>>");
    }

    public static async Task Run()
    {
        Console.WriteLine("Before Delay");
        await Task.Delay(100);
        Console.WriteLine("After Delay");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AwaitTaskWhenAll()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        await Run();
        Console.WriteLine("<<DONE>>");
    }

    public static async Task Run()
    {
        var t1 = Task.Delay(50).ContinueWith(_ => Console.WriteLine("T1 Done"));
        var t2 = Task.Delay(10).ContinueWith(_ => Console.WriteLine("T2 Done"));

        await Task.WhenAll(t1, t2);
        Console.WriteLine("All Done");
    }
}
""";
            // Note: T2 (10ms) should finish before T1 (50ms).
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncTaskResult()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        await Run();
        Console.WriteLine("<<DONE>>");
    }

    public static async Task Run()
    {
        int result = await GetValueAsync();
        Console.WriteLine(result);
    }

    public static async Task<int> GetValueAsync()
    {
        await Task.Delay(10);
        return 42;
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncTaskException()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        try
        {
           await Run();
        }
        catch (Exception E)
        {
            Console.WriteLine(E.Message);    
        }
        Console.WriteLine("<<DONE>>");
    }

    public static async Task Run()
    {
        throw new Exception("<<THROW>>");
        Console.WriteLine("This should not happen");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }
    }
}
