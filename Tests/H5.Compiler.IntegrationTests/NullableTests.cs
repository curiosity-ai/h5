using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class NullableTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task Nullable_Basic()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int? a = 5;
        int? b = null;
        Console.WriteLine(a.HasValue);
        Console.WriteLine(b.HasValue);
        Console.WriteLine(a.Value);
        try
        {
            Console.WriteLine(b.Value);
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("InvalidOperationException");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Nullable_Coalescing()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int? a = null;
        int b = a ?? 10;
        Console.WriteLine(b);

        int? c = 5;
        int d = c ?? 20;
        Console.WriteLine(d);

        string s1 = null;
        string s2 = s1 ?? "Default";
        Console.WriteLine(s2);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Nullable_LiftedOperators()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int? a = 5;
        int? b = null;
        int? c = 10;

        Console.WriteLine((a + c).HasValue); // True
        Console.WriteLine((a + c).Value);    // 15

        Console.WriteLine((a + b).HasValue); // False

        Console.WriteLine(a == 5); // True
        Console.WriteLine(b == null); // True
        Console.WriteLine(a != b); // True
        Console.WriteLine(a > b); // False
        Console.WriteLine(a < b); // False
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Nullable_Boxing()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int? a = 5;
        object obj = a;
        Console.WriteLine(obj.GetType().FullName);
        Console.WriteLine(obj);

        int? b = null;
        object obj2 = b;
        Console.WriteLine(obj2 == null);
    }
}
""";
            await RunTest(code);
        }
    }
}
