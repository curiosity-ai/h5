using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using H5.Compiler.IntegrationTests;

namespace Tests.H5.Compiler.IntegrationTests.Language
{
    [TestClass]
    public class TestGlobalEnum
    {
        [TestMethod]
        public async Task TestEnumReference()
        {
            var code = @"
using System;

namespace Test
{
    public static class MyClass { public enum TestEnum {A,B,C}}
}

public class App
{
    public static void Main()
    {
        // Implicit reference to TestEnum (via using or fully qualified without global::)
        // Here we test Fully Qualified without global::
        var x = Test.MyClass.TestEnum.A;
        System.Console.WriteLine(x);
    }
}";
            var js = await H5Compiler.CompileToJs(code, false);

            // We expect output NOT to have H5.global prefix for Test
            if (js.Contains("H5.global.Test.MyClass.TestEnum.A"))
            {
                Assert.Fail("Found H5.global.Test.MyClass.TestEnum.A which is incorrect.");
            }

            if (!js.Contains("Test.MyClass.TestEnum.A"))
            {
                 Assert.Fail("Did not find Test.MyClass.TestEnum.A.");
            }
        }
    }
}
