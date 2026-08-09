using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class MathFTests : TranslatorTestBase
    {
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
        float d = 1.0f;
        double next = MathF.BitIncrement(d);
        Console.WriteLine($""next > d => {next > d}"");
        Console.WriteLine($""(next - d) < 1e-6f => {(next - d) < 1e-6f}""); // Very small difference, but higher than expected due to the precision of JavaScript numbers

        float prev = MathF.BitDecrement(d);
        Console.WriteLine($""prev < d => {prev < d}"");
        Console.WriteLine($""(d - prev) < 1e-6f => {(d - prev) < 1e-6f}""); // Very small difference, but higher than expected due to the precision of JavaScript numbers

        Console.WriteLine(MathF.BitIncrement(float.PositiveInfinity) == float.PositiveInfinity);
        Console.WriteLine(MathF.BitDecrement(float.NegativeInfinity) == float.NegativeInfinity);
    }
}");
        }

        [TestMethod]
        public async Task TestConstantsAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
                Console.WriteLine(System.Math.Abs(MathF.E - 2.71828183f) < 1e-6f);
                Console.WriteLine(System.Math.Abs(MathF.PI - 3.14159265f) < 1e-6f);
    }
}
                ");
        }

        [TestMethod]
        public async Task TestBasicFunctionsAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
                Console.WriteLine(MathF.Abs(-10.5f).ToString(""F1""));
                Console.WriteLine(MathF.Ceiling(10.1f).ToString(""F1""));
                Console.WriteLine(MathF.Floor(10.9f).ToString(""F1""));
                Console.WriteLine(MathF.Round(10.5f).ToString(""F1"")); // Round half to even => 10
                Console.WriteLine(MathF.Round(11.5f).ToString(""F1"")); // Round half to even => 12

                Console.WriteLine(MathF.Min(5.0f, 10.0f).ToString(""F1""));
                Console.WriteLine(MathF.Max(5.0f, 10.0f).ToString(""F1""));

                Console.WriteLine(MathF.Sign(-10.0f));
                Console.WriteLine(MathF.Truncate(10.9f).ToString(""F1""));
    }
}
                ");
        }

        [TestMethod]
        public async Task TestTrigonometryAsync()
        {
             await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
                // Just check they execute and return somewhat correct values (JavaScript precision)
                Console.WriteLine(System.Math.Abs(MathF.Sin(0) - 0) < 1e-6f);
                Console.WriteLine(System.Math.Abs(MathF.Cos(0) - 1) < 1e-6f);
                Console.WriteLine(System.Math.Abs(MathF.Tan(0) - 0) < 1e-6f);
    }
}
                ");
        }

        [TestMethod]
        public async Task TestExponentialAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(System.Math.Abs(MathF.Exp(0) - 1) < 1e-6f);
        Console.WriteLine(System.Math.Abs(MathF.Log(MathF.E) - 1) < 1e-6f);
        Console.WriteLine(System.Math.Abs(MathF.Log10(100) - 2) < 1e-6f);
        Console.WriteLine(System.Math.Abs(MathF.Log2(8) - 3) < 1e-6f);
        Console.WriteLine(System.Math.Abs(MathF.Pow(2, 3) - 8) < 1e-6f);
        Console.WriteLine(System.Math.Abs(MathF.Sqrt(4) - 2) < 1e-6f);
    }
}
                ");
        }

        [TestMethod]
        public async Task TestHyperbolicAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
                Console.WriteLine(MathF.Sinh(0).ToString(""F1""));
                Console.WriteLine(MathF.Cosh(0).ToString(""F1""));
                Console.WriteLine(MathF.Tanh(0).ToString(""F1""));
    }
}
                ");
        }

        [TestMethod]
        public async Task TestAdvancedAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
                Console.WriteLine(MathF.CopySign(1.0f, -1.0f).ToString(""F1""));
                Console.WriteLine(MathF.MaxMagnitude(-10.0f, 5.0f).ToString(""F1""));
                Console.WriteLine(MathF.FusedMultiplyAdd(2.0f, 3.0f, 4.0f).ToString(""F1""));
    }
}
                ");
        }
    }
}
