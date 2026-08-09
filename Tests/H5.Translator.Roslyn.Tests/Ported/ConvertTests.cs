using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class ConvertTests : TranslatorTestBase
    {
        [TestMethod]
        public async Task TestToHexStringAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        byte[] bytes = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };
        Console.WriteLine(Convert.ToHexString(bytes));
        Console.WriteLine(Convert.ToHexString(bytes, 1, 2));

        Console.WriteLine(Convert.ToHexString(new byte[0]));
    }
}");
        }

        [TestMethod]
        public async Task TestFromHexStringAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        byte[] bytes = Convert.FromHexString(""DEADBEEF"");
        foreach (var b in bytes)
        {
            Console.WriteLine(b.ToString(""X2""));
        }
        Console.WriteLine();

        bytes = Convert.FromHexString(""adbe""); // Case insensitive check
        foreach (var b in bytes)
        {
            Console.WriteLine(b.ToString(""X2""));
        }
    }
}
                ");
        }
    }
}
