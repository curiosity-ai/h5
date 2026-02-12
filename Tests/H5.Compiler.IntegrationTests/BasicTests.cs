using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class BasicTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task HelloWorld()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello World");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task SimpleArithmetic()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        int a = 10;
        int b = 20;
        Console.WriteLine(a + b);
        Console.WriteLine(a * b);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ForLoop()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine("Iter: " + i);
        }
    }
}
""";
            await RunTest(code);
        }
    }
}
