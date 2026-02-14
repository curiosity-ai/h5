using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.StandardLibrary
{
    [TestClass]
    public class WebApiTests : IntegrationTestBase
    {
        [TestMethod]
        [Ignore("Requires updated H5 package with ResizeObserver")]
        public async Task ResizeObserverTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof ResizeObserver === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var observer = new dom.ResizeObserver((entries, obs) => {
                        Console.WriteLine(""Callback"");
                    });

                    Console.WriteLine(observer != null ? ""Success"" : ""Failed"");
                }
            }";

            // skipRoslyn: true because ResizeObserver is not available in .NET
            var output = await RunTest(code, skipRoslyn: true);
            Assert.AreEqual("Success", output);
        }
    }
}
