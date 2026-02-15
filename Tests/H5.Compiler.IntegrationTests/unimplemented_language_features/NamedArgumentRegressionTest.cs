using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class NamedArgumentRegressionTest : IntegrationTestBase
    {
        [TestMethod]
        public async Task DynamicNamedArgumentCrash()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        dynamic d = new Program();
        d.Foo(value: 10);
    }

    public void Foo(int value)
    {
        Console.WriteLine(value);
    }
}
""";
            await RunTest(code, "10", skipRoslyn: true);
        }
    }
}
