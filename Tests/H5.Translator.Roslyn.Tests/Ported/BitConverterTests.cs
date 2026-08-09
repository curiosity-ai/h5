using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class BitConverterTests : TranslatorTestBase
    {
        [TestMethod]
        public async Task TestSingleToInt32BitsAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        // 1.0f -> 0x3f800000 (1065353216)
        Console.WriteLine(BitConverter.SingleToInt32Bits(1.0f));

        // -1.0f -> 0xbf800000 (-1082130432)
        Console.WriteLine(BitConverter.SingleToInt32Bits(-1.0f));

        // 0.0f -> 0
        Console.WriteLine(BitConverter.SingleToInt32Bits(0.0f));

        // -0.0f -> 0x80000000 (-2147483648)
        Console.WriteLine(BitConverter.SingleToInt32Bits(-0.0f));

        // Infinity -> 0x7f800000 (2139095040)
        Console.WriteLine(BitConverter.SingleToInt32Bits(float.PositiveInfinity));
    }
}
                ");
        }

        [TestMethod]
        public async Task TestInt32BitsToSingleAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        // 0x3f800000 -> 1.0f
        Console.WriteLine(BitConverter.Int32BitsToSingle(1065353216).ToString(""F1""));

        // 0xbf800000 -> -1.0f
        Console.WriteLine(BitConverter.Int32BitsToSingle(-1082130432).ToString(""F1""));

        // 0 -> 0.0f
        Console.WriteLine(BitConverter.Int32BitsToSingle(0).ToString(""F1""));

        // Round trip
        float f = 123.456f;
        int bits = BitConverter.SingleToInt32Bits(f);
        float f2 = BitConverter.Int32BitsToSingle(bits);
        Console.WriteLine(f);
        Console.WriteLine(f2);
        Console.WriteLine(f.ToString() == f2.ToString()); //Round trip on JS is not precise, so compare in a way that will ignore small errors
    }
}
                ");
        }
    }
}
