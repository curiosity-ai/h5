using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class EnumTests : TranslatorTestBase
    {
        [TestMethod]
        public async Task TestGenericMethodsAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    enum Colors { Red = 1, Green = 2, Blue = 4 };
    public static void Main()
    {
        // Parse
        Colors c0 = (Colors)(Enum.Parse(typeof(Colors), ""Red""));
        Console.WriteLine(c0.ToString());

        // Parse
        Colors c1 = Enum.Parse<Colors>(""Red"");
        Console.WriteLine(c1.ToString());

        Colors c2 = Enum.Parse<Colors>(""green"", true);
        Console.WriteLine(c2.ToString());

        try {
            Enum.Parse<Colors>(""Yellow"");
        } catch (ArgumentException) {
            Console.WriteLine(""Caught ArgumentException"");
        }

        // GetValues
        var values = Enum.GetValues<Colors>();
        foreach (var v in values)
        {
            Console.WriteLine(v.ToString());
        }

        // GetNames
        var names = Enum.GetNames<Colors>();
        foreach (var n in names)
        {
            Console.WriteLine(n.ToString());
        }

        // IsDefined
        Console.WriteLine(Enum.IsDefined<Colors>(Colors.Red));
        Console.WriteLine(Enum.IsDefined<Colors>((Colors)5)); // Not defined
    }
}
                ");
        }
    }
}
