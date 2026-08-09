using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class RandomTests : TranslatorTestBase
    {
        [TestMethod]
        public async Task TestNextInt64Async()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        var r = new Random(42);

        // Unbounded
        long l1 = r.NextInt64();
        Console.WriteLine(l1 >= 0);

        // Max value
        long l2 = r.NextInt64(100);
        Console.WriteLine(l2 >= 0 && l2 < 100);

        // Range
        long l3 = r.NextInt64(50, 60);
        Console.WriteLine(l3 >= 50 && l3 < 60);

        // Large Range
        long l4 = r.NextInt64(long.MaxValue - 100, long.MaxValue);
        Console.WriteLine(l4 >= (long.MaxValue - 100) && l4 < long.MaxValue);
    }
}");
        }

        [TestMethod]
        public async Task TestNextSingleAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {
        var r = new Random(42);
        float f = r.NextSingle();
        Console.WriteLine(f >= 0.0f && f < 1.0f);
    }
}");
        }

        [TestMethod]
        public async Task TestSharedAsync()
        {
            await RunTest(
                @"
using System;

public class Program
{
    public static void Main()
    {

                // Just check it exists and works
                var r = Random.Shared;
                Console.WriteLine(r != null);
                Console.WriteLine(r.Next() >= 0);
    }
}");
        }
    }
}
