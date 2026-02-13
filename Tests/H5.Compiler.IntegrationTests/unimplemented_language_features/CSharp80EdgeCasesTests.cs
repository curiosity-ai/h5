using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp80EdgeCasesTests : IntegrationTestBase
    {
        [TestMethod]
        [Ignore("Known limitation: Side effects in left-hand side evaluated twice in H5 implementation")]
        public async Task NullCoalescingAssignment_SideEffects()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int i = 0;
        int[] arr = new int[5];

        // i starts at 0
        arr[i++] ??= 42; // i becomes 1. arr[0] is 0 (not null), so assignment happens (0 is not null? Wait, int is not nullable).
                         // Wait, for int?, ??= works. For int, ??= is invalid.
                         // Let's use int? or object.
    }
}
""";
            // Correct logic:
            code = """
using System;

public class Program
{
    public static void Main()
    {
        int i = 0;
        int?[] arr = new int?[5];

        // i starts at 0. arr[0] is null.
        // arr[i++] ??= 42;
        // 1. Evaluate arr[i++]. i becomes 1. Value is null.
        // 2. Assign 42 to arr[0] (since i was 0 when accessed).
        // 3. Result is 42.

        arr[i++] ??= 42;

        Console.WriteLine(i); // Should be 1
        Console.WriteLine(arr[0]); // Should be 42

        // i is 1. arr[1] is null.
        // arr[i++] ??= 100;
        // i becomes 2. arr[1] = 100.

        arr[i++] ??= 100;
        Console.WriteLine(i); // Should be 2
        Console.WriteLine(arr[1]); // Should be 100
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task UsingDeclarations_Nested()
        {
            var code = """
using System;

public class D : IDisposable
{
    public string Name;
    public D(string name) => Name = name;
    public void Dispose() => Console.WriteLine($"Disposed {Name}");
}

public class Program
{
    public static void Main()
    {
        {
            using var d1 = new D("1");
            Console.WriteLine("Inside 1");
            {
                using var d2 = new D("2");
                Console.WriteLine("Inside 2");
            }
            Console.WriteLine("After 2");
        }
        Console.WriteLine("After 1");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task UsingDeclarations_ControlFlow()
        {
            var code = """
using System;

public class D : IDisposable
{
    public string Name;
    public D(string name) => Name = name;
    public void Dispose() => Console.WriteLine($"Disposed {Name}");
}

public class Program
{
    public static void Main()
    {
        if (true)
        {
            using var d1 = new D("If");
            Console.WriteLine("Inside If");
        }

        for (int i = 0; i < 1; i++)
        {
            using var d2 = new D("For");
            Console.WriteLine("Inside For");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Missing IAsyncDisposable and ValueTask in H5 environment")]
        public async Task UsingDeclarations_Await()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class AD : IAsyncDisposable
{
    public string Name;
    public AD(string name) => Name = name;
    public async ValueTask DisposeAsync()
    {
        await Task.Delay(1);
        Console.WriteLine($"DisposedAsync {Name}");
    }
}

public class Program
{
    public static async Task Main()
    {
        {
            await using var d1 = new AD("1");
            Console.WriteLine("Inside 1");
        }
        Console.WriteLine("After 1");
    }
}
""";
            await RunTest(code);
        }
    }
}
