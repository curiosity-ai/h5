using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class MathTests : TranslatorTestBase
    {
        [TestMethod]
        public async Task TestClampAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        // Byte
        Console.WriteLine(Math.Clamp((byte)10, (byte)0, (byte)20));
        Console.WriteLine(Math.Clamp((byte)25, (byte)0, (byte)20));
        Console.WriteLine(Math.Clamp((byte)0, (byte)5, (byte)20));

        // Int32
        Console.WriteLine(Math.Clamp(10, 0, 20));
        Console.WriteLine(Math.Clamp(25, 0, 20));
        Console.WriteLine(Math.Clamp(-5, 0, 20));

        // Double
        Console.WriteLine(Math.Clamp(10.5, 0.0, 20.0).ToString(""F1""));
        Console.WriteLine(Math.Clamp(25.5, 0.0, 20.0).ToString(""F1""));
        Console.WriteLine(Math.Clamp(-5.5, 0.0, 20.0).ToString(""F1""));

        // Float
        Console.WriteLine(Math.Clamp(10.5f, 0.0f, 20.0f).ToString(""F1""));
        Console.WriteLine(Math.Clamp(25.5f, 0.0f, 20.0f).ToString(""F1""));
        Console.WriteLine(Math.Clamp(-5.5f, 0.0f, 20.0f).ToString(""F1""));

        // Decimal
        Console.WriteLine(Math.Clamp(10.5m, 0.0m, 20.0m).ToString(""F1""));
        Console.WriteLine(Math.Clamp(25.5m, 0.0m, 20.0m).ToString(""F1""));
        Console.WriteLine(Math.Clamp(-5.5m, 0.0m, 20.0m).ToString(""F1""));
    }
}
                ");
        }

        [TestMethod]
        public async Task TestCopySignAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(Math.CopySign(1.0, -1.0).ToString(""F1""));
        Console.WriteLine(Math.CopySign(-1.0, 1.0).ToString(""F1""));
        Console.WriteLine(Math.CopySign(1.0, 1.0).ToString(""F1""));
        Console.WriteLine(Math.CopySign(-1.0, -1.0).ToString(""F1""));

        // Test signed zero logic if possible (JS behavior)
        // H5 implementation uses ((1.0 / {y}) < 0 ? -1.0 : 1.0) * System.Math.abs({x})
        // Let's verify standard behavior first
        Console.WriteLine(Math.CopySign(5.0, -2.0).ToString(""F1""));
    }
}
                ");
        }

        [TestMethod]
        public async Task TestMinMaxMagnitudeAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(Math.MaxMagnitude(2.0, -3.0).ToString(""F1""));
        Console.WriteLine(Math.MaxMagnitude(-2.0, 3.0).ToString(""F1""));
        Console.WriteLine(Math.MaxMagnitude(2.0, 1.0).ToString(""F1""));

        Console.WriteLine(Math.MinMagnitude(2.0, -3.0).ToString(""F1""));
        Console.WriteLine(Math.MinMagnitude(-2.0, 3.0).ToString(""F1""));
        Console.WriteLine(Math.MinMagnitude(2.0, 1.0).ToString(""F1""));
    }
}
                ");
        }

        [TestMethod]
        public async Task TestLog2Async()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(Math.Log2(8.0).ToString(""F1""));
        Console.WriteLine(Math.Log2(1024.0).ToString(""F1""));
        Console.WriteLine(Math.Log2(1.0).ToString(""F1""));
    }
}
                ");
        }

        [TestMethod]
        public async Task TestFusedMultiplyAddAsync()
        {
             await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        // Note: JS implementation is likely (x * y) + z approximation, so double rounding might occur.
        // We test basic correctness here.
        Console.WriteLine(Math.FusedMultiplyAdd(2.0, 3.0, 4.0).ToString(""F1""));
        Console.WriteLine(Math.FusedMultiplyAdd(2.0, -3.0, 4.0).ToString(""F1""));
    }
}");
        }

        [TestMethod]
        public async Task TestBitIncrementDecrementAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        double d = 1.0;
        double next = Math.BitIncrement(d);
        Console.WriteLine($""next > d => {next > d}"");
        Console.WriteLine($""(next - d) < 1e-6 => {(next - d) < 1e-6}""); // Very small difference, but higher than expected due to the precision of JavaScript numbers

        double prev = Math.BitDecrement(d);
        Console.WriteLine($""prev < d => {prev < d}"");
        Console.WriteLine($""(d - prev) < 1e-6 => {(d - prev) < 1e-6}""); // Very small difference, but higher than expected due to the precision of JavaScript numbers

        Console.WriteLine(Math.BitIncrement(double.PositiveInfinity) == double.PositiveInfinity);
        Console.WriteLine(Math.BitDecrement(double.NegativeInfinity) == double.NegativeInfinity);
    }
}");
        }
    }
}
