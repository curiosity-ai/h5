using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class UsingStaticTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task GenericMethod_Inferred_UsingStatic()
        {
            var code = """
using System;
using static Helper;

public static class Helper
{
    public static string GenericMethod<T>(T value)
    {
        return "Generic: " + value.ToString();
    }
}

public class Program
{
    public static void Main()
    {
        // Call GenericMethod with type inference
        var result = GenericMethod(123);
        Console.WriteLine(result);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericMethod_Explicit_UsingStatic()
        {
            var code = """
using System;
using static Helper;

public static class Helper
{
    public static string GenericMethod<T>(T value)
    {
        return "Generic: " + value.ToString();
    }
}

public class Program
{
    public static void Main()
    {
        // Call GenericMethod with explicit type
        var result = GenericMethod<int>(123);
        Console.WriteLine(result);
    }
}
""";
            await RunTest(code);
        }
    }
}
