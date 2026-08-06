using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using H5;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class BugReportTest : IntegrationTestBase
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
        // We pass a structure that matches the user's usage
        var result = NodeTypeToggle(""A"", new Dictionary<string, Dictionary<string, int>> { { ""A"", dict } });
        Console.WriteLine(result);
    }

    private static object NodeTypeToggle(string nodeType, Dictionary<string, Dictionary<string, int>> neighbourSummary)
    {
        var renderer = new { Icon = ""icon"", Color = ""color"", DisplayName = ""name"" };
        var Theme = new { Disabled = new { Foreground = ""disabled"" } };

        var count = neighbourSummary is object && neighbourSummary.TryGetValue(nodeType, out var neighbors) ? neighbors.Values.Sum() : 0;

        return Toggle(
            onText: HStack().Children(
                Icon(renderer.Icon, color: renderer.Color).PR(8),
                TextBlock(renderer.DisplayName).Foreground(renderer.Color).PR(16),
                neighbourSummary is object ? Badge(count).AlignEnd() : Empty()
            ).AlignItemsCenter(),
            offText: HStack().Children(
                Icon(renderer.Icon, color: Theme.Disabled.Foreground).PR(8),
                TextBlock(renderer.DisplayName).Foreground(Theme.Disabled.Foreground).PR(16),
                neighbourSummary is object ? Badge(count).AlignEnd() : Empty()
            ).AlignItemsCenter());
    }

    public static object Toggle(object onText, object offText) => ""Toggle"";
    public static HStackClass HStack() => new HStackClass();
    public static object Icon(object icon, object color) => new Element();
    public static Element TextBlock(string text) => new Element();
    public static Element Badge(int n) => new Element();
    public static object Empty() => ""Empty"";

    public class Element {}
    public class HStackClass
    {
        public HStackClass Children(params object[] args) => this;
        public HStackClass AlignItemsCenter() => this;
    }
}

public static class Extensions
{
    public static object PR(this object o, int n) => o;
    public static object Foreground(this object o, object color) => o;
    public static object AlignEnd(this object o) => o;
}
";
            await RunTest(code, skipRoslyn: true);
        }
    }
}
