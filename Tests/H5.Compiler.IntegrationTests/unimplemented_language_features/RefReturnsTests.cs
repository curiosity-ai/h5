using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class RefReturnsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task RefField_Works()
        {
            var code = """
using System;

public class C
{
    public int F;
}

public class Program
{
    public static void Main()
    {
        var c = new C();
        c.F = 1;
        ref int r = ref c.F;
        r = 2;
        Console.WriteLine(c.F); // 2
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task RefLocal_PassToMethod_Works()
        {
            var code = """
using System;

public class Program
{
    public static void Modify(ref int x)
    {
        x = 5;
    }

    public static void Main()
    {
        int x = 1;
        ref int r = ref x;
        Modify(ref r);
        Console.WriteLine(x); // 5
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task RefReturn_Method_Works()
        {
            var code = """
using System;

public class Program
{
    static int val = 1;

    public static ref int GetRef()
    {
        return ref val;
    }

    public static void Main()
    {
        GetRef() = 10;
        Console.WriteLine(val); // 10

        ref int r = ref GetRef();
        r = 20;
        Console.WriteLine(val); // 20
    }
}
""";
            await RunTest(code);
        }
    }
}
