using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class CSharp20Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task AnonymousMethods()
        {
            var code = """
using System;

public delegate void PrintDelegate(string msg);

public class Program
{
    public static void Main()
    {
        PrintDelegate print = delegate(string msg) {
            Console.WriteLine("Message: " + msg);
        };

        print("Hello Anonymous");

        // With captured variable
        int factor = 2;
        Action<int> multiply = delegate(int n) {
            Console.WriteLine(n * factor);
        };
        multiply(10);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task StaticClasses()
        {
            var code = """
using System;

public static class Utility
{
    public static int Double(int x)
    {
        return x * 2;
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine(Utility.Double(5));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task PartialTypes()
        {
            var code = """
using System;

public partial class MyClass
{
    public void Method1()
    {
        Console.WriteLine("Method1");
    }
}

public partial class MyClass
{
    public void Method2()
    {
        Console.WriteLine("Method2");
    }
}

public class Program
{
    public static void Main()
    {
        var obj = new MyClass();
        obj.Method1();
        obj.Method2();
    }
}
""";
            await RunTest(code);
        }
    }
}
