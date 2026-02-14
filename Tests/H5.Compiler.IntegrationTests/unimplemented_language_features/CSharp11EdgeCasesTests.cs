using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp11EdgeCasesTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task RawStringLiterals_Complex()
        {
            var code = """"
using System;

public class Program
{
    public static void Main()
    {
        var json = """
        {
            "name": "John",
            "age": 30,
            "nested": {
                "key": "value"
            }
        }
        """;

        var xml = """
        <root>
            <child attr="val">Text</child>
        </root>
        """;

        var withQuotes = """
        She said "Hello" to him.
        """;

        Console.WriteLine(json.Contains("\"name\""));
        Console.WriteLine(xml.Contains("<child"));
        Console.WriteLine(withQuotes.Contains("\"Hello\""));
    }
}
"""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task RawStringLiterals_Interpolation()
        {
            var code = """"
using System;

public class Program
{
    public static void Main()
    {
        var name = "World";
        // Interpolated raw string
        var s = $"""
        Hello {name}!
        """;

        Console.WriteLine(s);
    }
}
"""";
            await RunTest(code);
        }
    }
}
