namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class EmbeddedAttributeTests : IntegrationTestBase
    {
        [TestInitialize]
        public void Initialize()
        {
            H5Compiler.ClearRewriterAndEmitterCache();
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

            Assert.DoesNotContain("EmbeddedAttribute", js, "Output should not contain EmbeddedAttribute");
        }
    }

}
