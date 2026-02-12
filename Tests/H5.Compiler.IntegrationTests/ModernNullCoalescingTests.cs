using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class ModernNullCoalescingTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task NullCoalescing_String()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        string s = null;
        Console.WriteLine("null ?? default: " + (s ?? "default"));

        s = "value";
        Console.WriteLine("value ?? default: " + (s ?? "default"));

        s = "";
        Console.WriteLine("empty ?? default: " + (s ?? "default"));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NullCoalescing_NullableInt()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        int? i = null;
        Console.WriteLine("null ?? -1: " + (i ?? -1));

        i = 10;
        Console.WriteLine("10 ?? -1: " + (i ?? -1));

        i = 0;
        Console.WriteLine("0 ?? -1: " + (i ?? -1));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NullCoalescing_Object()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        object o = null;
        object def = new object();
        Console.WriteLine("null ?? new: " + (o ?? "new object") is string); // simplified check

        o = "existing";
        Console.WriteLine("existing ?? new: " + (o ?? "new object"));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NullCoalescing_SideEffects()
        {
            var code = """
using System;
public class Program
{
    static int counter = 0;
    static string GetString()
    {
        counter++;
        return "value";
    }

    static string GetNull()
    {
        counter++;
        return null;
    }

    public static void Main()
    {
        counter = 0;
        var res = GetString() ?? "default";
        Console.WriteLine("Result 1: " + res);
        Console.WriteLine("Counter 1: " + counter); // Should be 1

        counter = 0;
        res = GetNull() ?? "default";
        Console.WriteLine("Result 2: " + res);
        Console.WriteLine("Counter 2: " + counter); // Should be 1
    }
}
""";
            await RunTest(code);
        }
    }
}
