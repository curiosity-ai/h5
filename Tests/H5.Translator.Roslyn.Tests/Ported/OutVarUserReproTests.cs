using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class OutVarUserReproTests : TranslatorTestBase
    {
        [TestMethod]
        public async Task UserRepro_EnumInNamespace()
        {
            var code = """
using System;
using GraphDB.Schema;

namespace GraphDB.Schema
{
    public enum CommandIcon { A, B }
}

public class App
{
    public void Test()
    {
        if (Enum.TryParse<CommandIcon>("A", out var icon))
        {
            Console.WriteLine(icon);
        }
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }
    }
}
