using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class DynamicAndNamedArgumentsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task NamedArguments_Basic()
        {
            var code = @"
using System;

public class Program
{
    public static void Method(int a, int b)
    {
        Console.WriteLine(""a: "" + a + "", b: "" + b);
    }

    public static void Main()
    {
        Method(b: 10, a: 5);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NamedArguments_Mixed()
        {
            var code = @"
using System;

public class Program
{
    public static void Method(int a, int b, int c)
    {
        Console.WriteLine(""a: "" + a + "", b: "" + b + "", c: "" + c);
    }

    public static void Main()
    {
        Method(1, c: 3, b: 2);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task OptionalArguments_Basic()
        {
            var code = @"
using System;

public class Program
{
    public static void Method(int a, int b = 10)
    {
        Console.WriteLine(""a: "" + a + "", b: "" + b);
    }

    public static void Main()
    {
        Method(1);
        Method(1, 20);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task OptionalArguments_Multiple()
        {
            var code = @"
using System;

public class Program
{
    public static void Method(int a, int b = 10, int c = 20)
    {
        Console.WriteLine(""a: "" + a + "", b: "" + b + "", c: "" + c);
    }

    public static void Main()
    {
        Method(1);
        Method(1, 2);
        Method(1, 2, 3);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task OptionalArguments_WithNamed()
        {
            var code = @"
using System;

public class Program
{
    public static void Method(int a, int b = 10, int c = 20)
    {
        Console.WriteLine(""a: "" + a + "", b: "" + b + "", c: "" + c);
    }

    public static void Main()
    {
        Method(1, c: 30);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Disabled for now as Roslyn is failing to run with error CS0656: Missing compiler required member 'Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember'")]
        public async Task Dynamic_Basic()
        {
            var code = @"
using System;

public class Program
{
    public static void Main()
    {
        dynamic d = 10;
        Console.WriteLine(d);

        d = ""Hello"";
        Console.WriteLine(d);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NamedArguments_OnDynamic_ShouldNotCrash()
        {
            var code = @"
public class Program
{
    public static void Main()
    {
        dynamic d = new System.Object();
        try {
            d.Foo(a: 1);
        } catch { }
    }
}";
            await RunTest(code, skipRoslyn: true);
        }
    }
}
