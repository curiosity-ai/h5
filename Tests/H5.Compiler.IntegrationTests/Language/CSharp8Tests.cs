using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class CSharp8Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task NullableReferenceTypes()
        {
            var code = """
#nullable enable
using System;

public class Program
{
    public static void Main()
    {
        string? s = null;
        Console.WriteLine(s == null);

        s = "Hello";
        Console.WriteLine(s.Length);

        string notNull = "World";
        Console.WriteLine(notNull.Length);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task SwitchExpressions()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetColor("Red"));
        Console.WriteLine(GetColor("Green"));
        Console.WriteLine(GetColor("Unknown"));
    }

    public static string GetColor(string color) => color switch
    {
        "Red" => "#FF0000",
        "Green" => "#00FF00",
        "Blue" => "#0000FF",
        _ => "#000000"
    };
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task UsingDeclarations()
        {
            var code = """
using System;

public class Disposable : IDisposable
{
    public void Dispose()
    {
        Console.WriteLine("Disposed");
    }
}

public class Program
{
    public static void Main()
    {
        Process();
        Console.WriteLine("After Process");
    }

    public static void Process()
    {
        using var d = new Disposable();
        Console.WriteLine("Processing");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task StaticLocalFunctions()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(Add(5, 10));

        static int Add(int x, int y)
        {
            return x + y;
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task DisposableRefStructs()
        {
            var code = """
using System;

public ref struct MyRefStruct
{
    public void Dispose()
    {
        Console.WriteLine("RefStruct Disposed");
    }
}

public class Program
{
    public static void Main()
    {
        Process();
    }

    public static void Process()
    {
        using var d = new MyRefStruct();
        Console.WriteLine("Using Ref Struct");
    }
}
""";
            await RunTest(code, skipRoslyn: true); // Roslyn script execution might struggle with ref structs in script context
        }

        [TestMethod]
        public async Task IndicesAndRanges()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        var words = new string[]
        {
            "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog"
        };

        Console.WriteLine(words[^1]);

        var quickBrownFox = words[1..4];
        foreach (var w in quickBrownFox) Console.WriteLine(w);

        var lazyDog = words[^2..^0];
        foreach (var w in lazyDog) Console.WriteLine(w);

        var all = words[..];
        Console.WriteLine(all.Length);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NullCoalescingAssignment()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        List<int> numbers = null;
        (numbers ??= new List<int>()).Add(5);
        Console.WriteLine(string.Join(", ", numbers));

        numbers.Add(10);
        (numbers ??= new List<int>()).Add(20); // Should not re-assign
        Console.WriteLine(string.Join(", ", numbers));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ReadonlyMembers()
        {
            var code = """
using System;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public readonly double Distance => Math.Sqrt(X * X + Y * Y);

    public readonly override string ToString() => $"({X}, {Y})";
}

public class Program
{
    public static void Main()
    {
        var p = new Point { X = 3, Y = 4 };
        Console.WriteLine(p.Distance);
        Console.WriteLine(p.ToString());
    }
}
""";
            await RunTest(code);
        }
    }
}
