using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class ExtensionMethodsNamedArgsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task NamedArgumentsWithExtensionMethodAndOptionalParams()
        {
            var code = @"
            using System;

            public class Program
            {
                public static void Main()
                {
                    var c = new Component();
                    // Using named arguments for extension method with optional parameters
                    c.ShowAt(fromTop: 10, fromLeft: 20);
                    c.ShowAt(fromRight: 5, fromBottom: 5);

                    // Mixed
                    c.ShowAt(1, fromLeft: 2);

                    Console.WriteLine(""Done"");
                }
            }

            public class Component { }

            public static class Extensions
            {
                public static void ShowAt(this Component c, int fromTop = 0, int fromLeft = 0, int fromRight = 0, int fromBottom = 0)
                {
                    Console.Write(""ShowAt: "");
                    if (fromTop != 0) Console.Write(""Top="" + fromTop + "" "");
                    if (fromLeft != 0) Console.Write(""Left="" + fromLeft + "" "");
                    if (fromRight != 0) Console.Write(""Right="" + fromRight + "" "");
                    if (fromBottom != 0) Console.Write(""Bottom="" + fromBottom + "" "");
                    Console.WriteLine("""");
                }
            }";

            await RunTest(code, skipRoslyn: true);
        }
    }
}
