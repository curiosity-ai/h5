using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class IteratorsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task Iterators_YieldReturn()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static IEnumerable<int> GetNumbers()
    {
        yield return 1;
        yield return 2;
        yield return 3;
    }

    public static void Main()
    {
        foreach (var n in GetNumbers())
        {
            Console.WriteLine(n);
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Iterators_YieldBreak()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static IEnumerable<int> GetNumbers(int limit)
    {
        int i = 0;
        while (true)
        {
            if (i >= limit)
                yield break;
            yield return i;
            i++;
        }
    }

    public static void Main()
    {
        foreach (var n in GetNumbers(3))
        {
            Console.WriteLine(n);
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Iterators_StateMaintenance()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static IEnumerable<string> GetSteps()
    {
        Console.WriteLine("Start");
        yield return "Step 1";
        Console.WriteLine("After 1");
        yield return "Step 2";
        Console.WriteLine("End");
    }

    public static void Main()
    {
        foreach (var s in GetSteps())
        {
            Console.WriteLine("Received: " + s);
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Iterators_Nested()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static IEnumerable<int> Range(int start, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return start + i;
        }
    }

    public static void Main()
    {
        foreach (var n in Range(10, 3))
        {
            Console.WriteLine(n);
        }
    }
}
""";
            await RunTest(code);
        }
    }
}
