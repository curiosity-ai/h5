using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class UnsupportedFeaturesTests : TranslatorTestBase
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
            await RunTestExpectingError(code, "Pointers are not supported");
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
            await RunTestExpectingError(code, "Unsafe code is not supported");
        }
    }
}
