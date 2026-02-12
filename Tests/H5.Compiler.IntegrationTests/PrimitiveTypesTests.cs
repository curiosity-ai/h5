using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class PrimitiveTypesTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task Integers_ArithmeticAndBitwise_Int32()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        int a = 10;
        int b = 3;
        Console.WriteLine(a + b);
        Console.WriteLine(a - b);
        Console.WriteLine(a * b);
        Console.WriteLine(a / b);
        Console.WriteLine(a % b);
        Console.WriteLine(a & b);
        Console.WriteLine(a | b);
        Console.WriteLine(a ^ b);
        Console.WriteLine(~a);
        Console.WriteLine(a << 1);
        Console.WriteLine(a >> 1);

        byte by = 255;
        Console.WriteLine(by + 1); // Implicit conversion to int
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Integers_ArithmeticAndBitwise_Int64()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        long l1 = 1234567890123L;
        long l2 = 2L;
        Console.WriteLine(l1 * l2);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Integers_Overflow()
        {
             var code = """
using System;
public class Program
{
    public static void Main()
    {
        int max = 2147483647; // int.MaxValue
        try {
            checked {
                Console.WriteLine(max + 1);
            }
        } catch (OverflowException) {
            Console.WriteLine("Overflow caught");
        } catch (Exception ex) {
            Console.WriteLine("Other exception: " + ex.GetType().Name);
        }

        unchecked {
             Console.WriteLine(max + 1);
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task FloatingPoint_Operations()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        double d1 = 1.5;
        double d2 = 2.0;
        Console.WriteLine(d1 + d2);
        Console.WriteLine(d1 / 0.0);
        Console.WriteLine(d1 / -0.0);
        Console.WriteLine(0.0 / 0.0); // NaN

        float f1 = 1.5f;
        Console.WriteLine(f1 * 2);

        decimal dec1 = 10.5m;
        decimal dec2 = 20.1m;
        Console.WriteLine(dec1 + dec2);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Booleans_Logic()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        bool t = true;
        bool f = false;
        Console.WriteLine(t && f);
        Console.WriteLine(t || f);
        Console.WriteLine(!t);
        Console.WriteLine(t ^ f);

        // Short circuiting
        if (t || SideEffect())
            Console.WriteLine("Short circuit ||");

        if (f && SideEffect())
            Console.WriteLine("Should not happen");
        else
            Console.WriteLine("Short circuit &&");
    }

    static bool SideEffect()
    {
        Console.WriteLine("Side effect executed");
        return true;
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task CharsAndStrings()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        char c = 'A';
        Console.WriteLine((int)c);
        Console.WriteLine(char.IsDigit('1'));

        string s = "Hello";
        string s2 = "World";
        Console.WriteLine(s + " " + s2);
        Console.WriteLine(s[0]);
        Console.WriteLine(s.Length);
        Console.WriteLine(s.Substring(1, 2));
        Console.WriteLine(s == "Hello");
        Console.WriteLine("Escape\tSequence\nNew Line");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Enums_CastToInt()
        {
            var code = """
using System;

public enum Colors { Red = 1, Green = 2, Blue = 4 }

[Flags]
public enum Permissions { None = 0, Read = 1, Write = 2, Execute = 4 }

public class Program
{
    public static void Main()
    {
        Colors c = Colors.Green;
        Console.WriteLine((int)c);

        Permissions p = Permissions.Read | Permissions.Write;
        Console.WriteLine(p.HasFlag(Permissions.Read));
        Console.WriteLine(p.HasFlag(Permissions.Execute));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Enums_ToString()
        {
            var code = """
using System;

public enum Colors { Red = 1, Green = 2, Blue = 4 }

public class Program
{
    public static void Main()
    {
        Colors c = Colors.Green;
        Console.WriteLine(c);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Enums_Flags()
        {
            var code = """
using System;

[Flags]
public enum Permissions { None = 0, Read = 1, Write = 2, Execute = 4 }

public class Program
{
    public static void Main()
    {
        Permissions p = Permissions.Read | Permissions.Write;
        Console.WriteLine(p);
    }
}
""";
            await RunTest(code);
        }
    }
}
