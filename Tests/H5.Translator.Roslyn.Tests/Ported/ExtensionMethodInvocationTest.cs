using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using H5;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class BugReportTest : TranslatorTestBase
    {
        [TestMethod]
        public async Task TestMissingArgumentInExtensionMethod()
        {
            var code = @"
using System;
using System.Collections.Generic;
using System.Linq;
using H5;

public class Program
{
    public static void Main()
    {
        var dict = new Dictionary<string, int> { { ""A"", 1 } };
        var result = NodeTypeToggle(""A"", new Dictionary<string, Dictionary<string, int>> { { ""A"", dict } });
        Console.WriteLine(result);
    }

    private static string NodeTypeToggle(string nodeType, Dictionary<string, Dictionary<string, int>> neighbourSummary)
    {
        // Emulate out var usage
        var count = neighbourSummary is object && neighbourSummary.TryGetValue(nodeType, out var neighbors) ? neighbors.Values.Sum() : 0;

        // Emulate return structure with ternary
        return Container(
            neighbourSummary is object ? Badge(count).AlignEnd() : Empty()
        );
    }

    public static string Container(string s) => ""Container("" + s + "")"";
    public static string Badge(int n) => ""Badge("" + n + "")"";
    public static string Empty() => ""Empty"";
}

public static class Extensions
{
    public static string AlignEnd(this string s) => s + "".AlignEnd"";
}
";
            await RunTest(code, skipRoslyn: true);
        }
    }
}
