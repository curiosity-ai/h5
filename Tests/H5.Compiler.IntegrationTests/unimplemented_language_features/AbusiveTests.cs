using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class AbusiveTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task DeeplyNestedPatternMatching()
        {
            var code = """
using System;

namespace System.Runtime.CompilerServices { internal static class IsExternalInit {} }

public record Node(int Value, Node Next);

public class Program
{
    public static void Main()
    {
        var n3 = new Node(3, null);
        var n2 = new Node(2, n3);
        var n1 = new Node(1, n2);

        Console.WriteLine(IsMatch(n1)); // True
        Console.WriteLine(IsMatch(n2)); // False
    }

    static bool IsMatch(Node n) => n is { Value: 1, Next: { Value: 2, Next: { Value: > 2 } } };
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task ComplexTupleDeconstruction()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(Evaluate((1, 2, "add")));
        Console.WriteLine(Evaluate((10, 2, "sub")));
        Console.WriteLine(Evaluate((5, 0, "div")));
        Console.WriteLine(Evaluate((5, 5, "unknown")));
    }

    static string Evaluate((int, int, string) input) => input switch
    {
        (var a, var b, "add") => $"{a + b}",
        (var a, var b, "sub") when a > b => $"{a - b}",
        (var a, var b, "sub") => $"{b - a}", // flipped
        (_, 0, "div") => "Division by zero",
        (var a, var b, "div") => $"{a / b}",
        _ => "Unknown operation"
    };
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task RefStructMutationInLocalFunction()
        {
            var code = """
using System;

public struct MutableStruct
{
    public int Value;
}

public class Program
{
    public static void Main()
    {
        var s = new MutableStruct { Value = 10 };
        Mutate(ref s);
        Console.WriteLine(s.Value); // Should be 20
    }

    static void Mutate(ref MutableStruct s)
    {
        void LocalMutate(ref MutableStruct ms)
        {
            ms.Value += 5;
        }

        LocalMutate(ref s);
        LocalMutate(ref s);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task TargetTypedNewInComplexObjectInitializer()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        var data = new Dictionary<string, List<int>>
        {
            ["evens"] = new() { 2, 4, 6 },
            ["odds"] = new() { 1, 3, 5 }
        };

        foreach (var kvp in data)
        {
            Console.WriteLine($"{kvp.Key}: {string.Join(",", kvp.Value)}");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericPatternMatching()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(Check<int>(10)); // True
        Console.WriteLine(Check<string>("hello")); // True
        Console.WriteLine(Check<int?>(null)); // False
    }

    static bool Check<T>(T item)
    {
        return item is T t && t is not null;
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task LambdaCaptureComplexScope()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int outer = 10;
        Func<int, int> f1 = (x) =>
        {
            int inner = 20;
            Func<int, int> f2 = (y) =>
            {
                outer += 1;
                return x + y + inner + outer;
            };
            return f2(5);
        };

        Console.WriteLine(f1(3)); // 3 + 5 + 20 + 11 = 39
        Console.WriteLine(outer); // 11
    }
}
""";
            await RunTest(code);
        }
    }
}
