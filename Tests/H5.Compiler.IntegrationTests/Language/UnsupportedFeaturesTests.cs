using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class UnsupportedFeaturesTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task UnsafePointers()
        {
            var code = """
using System;

public class Program
{
    public static unsafe void Main()
    {
        int x = 10;
        int* p = &x;
        Console.WriteLine(*p);
    }
}
""";
            // Expected to fail in H5 with "Feature not supported" eventually.
            // Currently might crash or fail compilation.
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task FixedSizeBuffers()
        {
            var code = """
using System;

public unsafe struct MyBuffer
{
    public fixed int Data[10];
}

public class Program
{
    public static void Main()
    {
        var b = new MyBuffer();
        unsafe
        {
            b.Data[0] = 5;
            Console.WriteLine(b.Data[0]);
        }
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task MakeRef()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int x = 10;
        TypedReference tr = __makeref(x);
        Console.WriteLine(__refvalue(tr, int));
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task ArgIterator() // varargs not fully supported like in C/C++ style
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Print(__arglist(1, 2, 3));
    }

    public static void Print(__arglist)
    {
        ArgIterator iterator = new ArgIterator(__arglist);
        while (iterator.GetRemainingCount() > 0)
        {
             TypedReference tr = iterator.GetNextArg();
             Console.WriteLine(__refvalue(tr, int));
        }
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }
    }
}
