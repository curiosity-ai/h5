using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp80Tests : IntegrationTestBase
    {
        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task ReadonlyMembers()
        {
            var code = """
using System;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public readonly double Distance => Math.Sqrt(X * X + Y * Y);

    public readonly void Print()
    {
        Console.WriteLine($"({X}, {Y})");
    }
}

public class Program
{
    public static void Main()
    {
        var p = new Point { X = 3, Y = 4 };
        Console.WriteLine(p.Distance);
        p.Print();
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task DefaultInterfaceMethods()
        {
            var code = """
using System;

public interface ILogger
{
    void Log(string message);
    void LogError(string error)
    {
        Log("Error: " + error);
    }
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}

public class Program
{
    public static void Main()
    {
        ILogger logger = new ConsoleLogger();
        logger.Log("Hello");
        logger.LogError("Something failed");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task SwitchExpressions()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetColorName(ConsoleColor.Red));
        Console.WriteLine(GetColorName(ConsoleColor.Blue));
    }

    static string GetColorName(ConsoleColor color) => color switch
    {
        ConsoleColor.Red => "Red",
        ConsoleColor.Green => "Green",
        ConsoleColor.Blue => "Blue",
        _ => "Unknown"
    };
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task PropertyPatterns()
        {
            var code = """
using System;

public class Address
{
    public string State { get; set; }
}

public class Person
{
    public string Name { get; set; }
    public Address Address { get; set; }
}

public class Program
{
    public static void Main()
    {
        var p = new Person { Name = "Alice", Address = new Address { State = "WA" } };

        if (p is { Address: { State: "WA" } })
        {
            Console.WriteLine("Lives in WA");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task TuplePatterns()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetQuadrant(1, 1));
        Console.WriteLine(GetQuadrant(-1, -1));
    }

    static string GetQuadrant(int x, int y) => (x, y) switch
    {
        ( > 0, > 0) => "Q1",
        ( < 0, > 0) => "Q2",
        ( < 0, < 0) => "Q3",
        ( > 0, < 0) => "Q4",
        _ => "Origin/Axis"
    };
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task PositionalPatterns()
        {
            var code = """
using System;

public class Point
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) => (X, Y) = (x, y);
    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}

public class Program
{
    public static void Main()
    {
        var p = new Point(0, 0);
        if (p is (0, 0))
        {
            Console.WriteLine("Origin");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task UsingDeclarations()
        {
            var code = """
using System;

public class Disposable : IDisposable
{
    public void Dispose() => Console.WriteLine("Disposed");
}

public class Program
{
    public static void Main()
    {
        DoWork();
        Console.WriteLine("Done");
    }

    static void DoWork()
    {
        using var d = new Disposable();
        Console.WriteLine("Working");
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
        int y = 5;
        int x = 5;
        Console.WriteLine(Add(x, y));

        static int Add(int a, int b) => a + b;
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
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
        numbers ??= new List<int>();
        numbers.Add(1);
        Console.WriteLine(numbers.Count);

        numbers ??= new List<int>(); // Should not reassign
        Console.WriteLine(numbers.Count);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
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
                    // index from start    index from end
            "The",  // 0                   ^9
            "quick",// 1                   ^8
            "brown",// 2                   ^7
            "fox",  // 3                   ^6
            "jumps",// 4                   ^5
            "over", // 5                   ^4
            "the",  // 6                   ^3
            "lazy", // 7                   ^2
            "dog"   // 8                   ^1
        };              // 9 (or words.Length) ^0

        Console.WriteLine($"The last word is {words[^1]}");

        var quickBrownFox = words[1..4];
        foreach (var word in quickBrownFox)
            Console.Write($"{word} ");
        Console.WriteLine();

        var lazyDog = words[^2..^0];
        foreach (var word in lazyDog)
            Console.Write($"{word} ");
        Console.WriteLine();
    }
}
""";
            await RunTest(code);
        }
    }
}
