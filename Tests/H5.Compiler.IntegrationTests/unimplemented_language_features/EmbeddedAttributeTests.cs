using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class EmbeddedAttributeTests : IntegrationTestBase
    {
        [TestInitialize]
        public void Initialize()
        {
            H5Compiler.ClearRewriterCache();
        }

        [TestMethod]
        public async Task EmbeddedAttribute_ShouldNotBeEmitted()
        {
            var csharpCode = @"
using System;

namespace Microsoft.CodeAnalysis
{
    internal sealed class EmbeddedAttribute : Attribute
    {
    }
}

namespace Test
{
    public class MyClass { }
}
";
            var js = await H5Compiler.CompileToJs(csharpCode);

            Assert.IsFalse(js.Contains("EmbeddedAttribute"), "Output should not contain EmbeddedAttribute");
        }
    }
}
