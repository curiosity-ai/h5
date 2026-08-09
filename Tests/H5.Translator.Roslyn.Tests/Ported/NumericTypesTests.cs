using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class NumericTypesTests : TranslatorTestBase
    {
        [TestMethod]
        public async Task Byte_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        byte b = 10;
        byte b2 = 20;

        Console.WriteLine(b + b2);
        Console.WriteLine(b2 - b);
        Console.WriteLine(b * b2);
        Console.WriteLine(b2 / b);
        Console.WriteLine(b2 % 3);

        Console.WriteLine(b < b2);
        Console.WriteLine(b > b2);
        Console.WriteLine(b == 10);
        Console.WriteLine(b != 20);

        Console.WriteLine(byte.MinValue);
        Console.WriteLine(byte.MaxValue);

        Console.WriteLine(b.ToString());
        Console.WriteLine(b.ToString("D3"));
        Console.WriteLine(b.ToString("X"));

        Console.WriteLine(byte.Parse("255"));

        byte res;
        if (byte.TryParse("123", out res)) Console.WriteLine(res);
        if (!byte.TryParse("300", out res)) Console.WriteLine("Overflow handled");
        if (!byte.TryParse("abc", out res)) Console.WriteLine("Format handled");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task SByte_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        sbyte sb = -10;
        sbyte sb2 = 20;

        Console.WriteLine(sb + sb2);
        Console.WriteLine(sb2 - sb);
        Console.WriteLine(sb * sb2);
        Console.WriteLine(sb2 / 2);
        Console.WriteLine(sb % 3);

        Console.WriteLine(sb < sb2);
        Console.WriteLine(sb > sb2);
        Console.WriteLine(sb == -10);

        Console.WriteLine(sbyte.MinValue);
        Console.WriteLine(sbyte.MaxValue);

        Console.WriteLine(sb.ToString());

        Console.WriteLine(sbyte.Parse("-128"));

        sbyte res;
        if (sbyte.TryParse("127", out res)) Console.WriteLine(res);
        if (!sbyte.TryParse("128", out res)) Console.WriteLine("Overflow handled");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Int16_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        short s = -1000;
        short s2 = 2000;

        Console.WriteLine(s + s2);
        Console.WriteLine(s2 - s);
        Console.WriteLine(s * 2);
        Console.WriteLine(s2 / 2);
        Console.WriteLine(s % 3);

        Console.WriteLine(s < s2);
        Console.WriteLine(short.MinValue);
        Console.WriteLine(short.MaxValue);

        Console.WriteLine(s.ToString());

        Console.WriteLine(short.Parse("-32000"));

        short res;
        if (short.TryParse("32000", out res)) Console.WriteLine(res);
        if (!short.TryParse("40000", out res)) Console.WriteLine("Overflow handled");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task UInt16_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        ushort us = 1000;
        ushort us2 = 60000;

        Console.WriteLine(us + us2);
        Console.WriteLine(us2 - us);
        Console.WriteLine(us * 2);
        Console.WriteLine(us2 / 2);
        Console.WriteLine(us % 3);

        Console.WriteLine(us < us2);
        Console.WriteLine(ushort.MinValue);
        Console.WriteLine(ushort.MaxValue);

        Console.WriteLine(us.ToString());

        Console.WriteLine(ushort.Parse("65000"));

        ushort res;
        if (ushort.TryParse("65000", out res)) Console.WriteLine(res);
        if (!ushort.TryParse("-1", out res)) Console.WriteLine("Overflow/Format handled");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Int32_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        int i = -100000;
        int i2 = 200000;

        Console.WriteLine(i + i2);
        Console.WriteLine(i2 - i);
        Console.WriteLine(i * i2);
        Console.WriteLine(i2 / 2);
        Console.WriteLine(i % 3);

        Console.WriteLine(i < i2);
        Console.WriteLine(int.MinValue);
        Console.WriteLine(int.MaxValue);

        Console.WriteLine(i.ToString());
        Console.WriteLine(i.ToString("D10"));

        Console.WriteLine(int.Parse("-123456"));

        int res;
        if (int.TryParse("123456", out res)) Console.WriteLine(res);
        if (!int.TryParse("2147483648", out res)) Console.WriteLine("Overflow handled");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task UInt32_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        uint ui = 100000;
        uint ui2 = 3000000000;

        Console.WriteLine(ui + ui2);
        Console.WriteLine(ui + 100);
        Console.WriteLine(ui2 - ui);
        Console.WriteLine(ui * 2);
        Console.WriteLine(ui2 / 2);
        Console.WriteLine(ui % 3);

        Console.WriteLine(ui < ui2);
        Console.WriteLine(uint.MinValue);
        Console.WriteLine(uint.MaxValue);

        Console.WriteLine(ui.ToString());

        Console.WriteLine(uint.Parse("3000000000"));

        uint res;
        if (uint.TryParse("3000000000", out res)) Console.WriteLine(res);
        if (!uint.TryParse("-1", out res)) Console.WriteLine("Format handled");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Int64_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        long l = -10000000000L;
        long l2 = 20000000000L;

        Console.WriteLine(l + l2);
        Console.WriteLine(l2 - l);
        Console.WriteLine(l * 2);
        Console.WriteLine(l2 / 2);
        Console.WriteLine(l % 3);

        Console.WriteLine(l < l2);
        Console.WriteLine(long.MinValue);
        Console.WriteLine(long.MaxValue);

        Console.WriteLine(l.ToString());

        Console.WriteLine(long.Parse("-1234567890123"));

        long res;
        if (long.TryParse("1234567890123", out res)) Console.WriteLine(res);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task UInt64_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        ulong ul = 10000000000UL;
        ulong ul2 = 10000000000000000000UL;

        Console.WriteLine(ul + 100);
        Console.WriteLine(ul2 / 2);

        Console.WriteLine(ul < ul2);
        Console.WriteLine(ulong.MinValue);
        Console.WriteLine(ulong.MaxValue);

        Console.WriteLine(ul.ToString());

        Console.WriteLine(ulong.Parse("10000000000"));

        ulong res;
        if (ulong.TryParse("18446744073709551615", out res)) Console.WriteLine(res == ulong.MaxValue);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Single_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        float f = 1.5f;
        float f2 = 2.5f;

        Console.WriteLine(f + f2);
        Console.WriteLine(f2 - f);
        Console.WriteLine(f * f2);
        Console.WriteLine((f2 / f).ToString("F4"));
        Console.WriteLine(f2 % 1.0f); // 0.5

        Console.WriteLine(f < f2);
        // Formatting of MinValue/MaxValue differs between .NET and JS engines
        Console.WriteLine(float.MinValue < -3E+38f);
        Console.WriteLine(float.MaxValue > 3E+38f);
        Console.WriteLine(float.Epsilon > 0);

        Console.WriteLine(float.NaN);
        Console.WriteLine(float.PositiveInfinity);
        Console.WriteLine(float.NegativeInfinity);

        Console.WriteLine(float.IsNaN(float.NaN));
        Console.WriteLine(float.IsInfinity(float.PositiveInfinity));

        Console.WriteLine(f.ToString());
        Console.WriteLine(f.ToString("F2"));

        Console.WriteLine(float.Parse("1.5"));

        float res;
        if (float.TryParse("1.5", out res)) Console.WriteLine(res);
        if (!float.TryParse("abc", out res)) Console.WriteLine("Format handled");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Double_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        double d = 1.5;
        double d2 = 2.5;

        Console.WriteLine(d + d2);
        Console.WriteLine(d2 - d);
        Console.WriteLine(d * d2);
        Console.WriteLine((d2 / d).ToString("F4"));
        Console.WriteLine(d2 % 1.0); // 0.5

        Console.WriteLine(d < d2);
        // Formatting of MinValue/MaxValue differs between .NET and JS engines
        Console.WriteLine(double.MinValue < -1.7E+308);
        Console.WriteLine(double.MaxValue > 1.7E+308);
        Console.WriteLine(double.Epsilon > 0);

        Console.WriteLine(double.NaN);
        Console.WriteLine(double.PositiveInfinity);
        Console.WriteLine(double.NegativeInfinity);

        Console.WriteLine(double.IsNaN(double.NaN));
        Console.WriteLine(double.IsInfinity(double.PositiveInfinity));

        Console.WriteLine(d.ToString());
        Console.WriteLine(d.ToString("F2"));

        Console.WriteLine(double.Parse("1.5"));

        double res;
        if (double.TryParse("1.5", out res)) Console.WriteLine(res);
        if (!double.TryParse("abc", out res)) Console.WriteLine("Format handled");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Decimal_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        decimal d = 1.5m;
        decimal d2 = 2.5m;

        Console.WriteLine(d + d2);
        Console.WriteLine(d2 - d);
        Console.WriteLine(d * d2);
        Console.WriteLine(d2 / d);
        Console.WriteLine(d2 % 1.0m); // 0.5

        Console.WriteLine(d < d2);
        Console.WriteLine(decimal.MinValue);
        Console.WriteLine(decimal.MaxValue);

        Console.WriteLine(decimal.One);
        Console.WriteLine(decimal.Zero);
        Console.WriteLine(decimal.MinusOne);

        Console.WriteLine(d.ToString());
        Console.WriteLine(d.ToString("F2"));

        Console.WriteLine(decimal.Parse("1.5"));

        decimal res;
        if (decimal.TryParse("1.5", out res)) Console.WriteLine(res);
        if (!decimal.TryParse("abc", out res)) Console.WriteLine("Format handled");
    }
}
""";
            await RunTest(code);
        }
    }
}
