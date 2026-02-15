using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class AliasAnyTypeTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task TupleAlias()
        {
            var code = """
using System;
using Point = (int X, int Y);

public class Program
{
    public static void Main()
    {
        Point p = (10, 20);
        Console.WriteLine(p.X);
        Console.WriteLine(p.Y);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ArrayAlias()
        {
            var code = """
using System;
using IntArray = int[];

public class Program
{
    public static void Main()
    {
        IntArray arr = new int[] { 1, 2, 3 };
        Console.WriteLine(arr.Length);
        Console.WriteLine(arr[0]);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NullableAlias()
        {
            var code = """
using System;
using NullableInt = int?;

public class Program
{
    public static void Main()
    {
        NullableInt n = 42;
        Console.WriteLine(n.HasValue);
        Console.WriteLine(n.Value);

        NullableInt n2 = null;
        Console.WriteLine(n2.HasValue);
    }
}
""";
            await RunTest(code);
        }
/*
        [TestMethod]
        public async Task PointerAlias()
        {
            // Pointers might not be fully supported in H5/JS environment unless using specific interop or unsafe blocks that compile differently.
            // Skipping for now unless requested specifically.
            var code = """
using System;
using IntArgs = delegate*<int, void>;

public class Program
{
    public static void Main()
    {
        // usage
    }
}
""";
            await RunTest(code);
        }
*/
    }
}
