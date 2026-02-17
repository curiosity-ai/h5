using H5.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class LoaderTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task TestLoaderOptions_Global()
        {
            await RunTestLoader(ModuleLoaderType.Global);
        }

        [TestMethod]
        public async Task TestLoaderOptions_AMD()
        {
            await RunTestLoader(ModuleLoaderType.AMD);
        }

        [TestMethod]
        public async Task TestLoaderOptions_CommonJS()
        {
            await RunTestLoader(ModuleLoaderType.CommonJS);
        }

        private async Task RunTestLoader(ModuleLoaderType loaderType)
        {
            // C# code for H5 compilation
            var csharpCode = @"
using System;
using System.Threading.Tasks;
using H5;

[External]
[ModuleDependency(""my-lib"", ""MyLib"")]
public class MyLib
{
    public static extern void DoWork();
}

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine(""Start"");
        // await Task.Delay(1); // Removed to isolate issue
        MyLib.DoWork();
        Console.WriteLine(""Done"");
    }
}
";

            // C# code for Roslyn execution (Standard .NET)
            var roslynCode = @"
using System;
using System.Threading.Tasks;

public class MyLib
{
    public static void DoWork() { Console.WriteLine(""MyLib.DoWork""); }
}

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine(""Start"");
        await Task.Delay(1);
        MyLib.DoWork();
        Console.WriteLine(""Done"");
    }
}
";

            await RunTest(csharpCode, waitForOutput: "Done", overrideRoslynCode: roslynCode, loaderType: loaderType);
        }
    }
}
