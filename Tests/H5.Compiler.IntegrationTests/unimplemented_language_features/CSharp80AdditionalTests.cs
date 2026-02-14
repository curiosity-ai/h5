using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp80AdditionalTests : IntegrationTestBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
            H5Compiler.ClearRewriterCache();
        }

        [TestMethod]
        public async Task IndexType_Works()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Index i1 = new Index(1, fromEnd: true);
        Console.WriteLine(i1.Value);
        Console.WriteLine(i1.IsFromEnd);
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task Span_Ref_Modifies_Original()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        var array = new int[] { 1, 2, 3 };
        Span<int> span = new Span<int>(array);
        ref int first = ref span[0];
        first = 10;
        Console.WriteLine(array[0]); // Should be 10
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ReadOnlySpan_Ref_Read()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        var array = new int[] { 1, 2, 3 };
        ReadOnlySpan<int> span = new ReadOnlySpan<int>(array);
        ref readonly int first = ref span[0];
        Console.WriteLine(first); // 1
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task RangeType_Works()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Index start = new Index(1, false);
        Index end = new Index(1, true);
        Range r1 = new Range(start, end);
        Console.WriteLine(r1.Start.Value);
        Console.WriteLine(r1.End.IsFromEnd);

        var (offset, length) = r1.GetOffsetAndLength(10);
        Console.WriteLine(offset); // 1
        Console.WriteLine(length); // 10 - 1 - 1 = 8
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task RuntimeHelpers_GetSubArray_Works()
        {
            var code = """
using System;
using System.Runtime.CompilerServices;

public class Program
{
    public static void Main()
    {
        var array = new int[] { 0, 1, 2, 3, 4, 5 };
        var sub = RuntimeHelpers.GetSubArray(array, new Range(new Index(1, false), new Index(2, true)));
        // 1..^2 => indices 1, 2, 3. (length 6. start 1. end 6-2=4. 1 to 4. length 3)
        // items: 1, 2, 3

        Console.WriteLine(sub.Length); // 3
        foreach(var item in sub) Console.Write(item + " "); // 1 2 3
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }
    }
}
