using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp80Tests : IntegrationTestBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
            H5Compiler.ClearRewriterAndEmitterCache();
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
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        [Ignore("Blocked by CS8701: Target runtime doesn't support default interface implementation")]
        public async Task DefaultInterfaceMethods()
        {
            var code = """
using System;
namespace System.Runtime.CompilerServices {
    public static class RuntimeFeature {
        public const string DefaultImplementationsOfInterfaces = "DefaultImplementationsOfInterfaces";
    }
}

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
            await RunTest(code, skipRoslyn: true);
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

public enum ConsoleColor
{
    Red,
    Green,
    Blue
}
""";
            await RunTest(code);
        }

        [TestMethod]
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
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
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

        Console.WriteLine($"The last word is {words[^1]}");

        // Range syntax not supported by parser
        // var quickBrownFox = words[1..4];
        // foreach (var word in quickBrownFox)
        //    Console.Write($"{word} ");
        // Console.WriteLine();
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

public ref struct RefStruct
{
    public void Dispose() => Console.WriteLine("Disposed RefStruct");
}

public class Program
{
    public static void Main()
    {
        using (var rs = new RefStruct())
        {
            Console.WriteLine("Inside");
        }
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task NullableReferenceTypes()
        {
            var code = """
using System;

#nullable enable

public class Program
{
    public static void Main()
    {
        string? name = null;
        if (name == null)
        {
            Console.WriteLine("Name is null");
        }
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task AsyncStreams()
        {
            // Requires IAsyncEnumerable<T>
            var code = """
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    public interface INotifyCompletion { void OnCompleted(Action continuation); }
}

public struct MyDelay
{
    public MyDelayAwaiter GetAwaiter() => new MyDelayAwaiter();
}
public struct MyDelayAwaiter : System.Runtime.CompilerServices.INotifyCompletion {
    public bool IsCompleted => true;
    public void GetResult() {}
    public void OnCompleted(Action c) => c();
}

namespace System.Threading.Tasks
{
    public struct CancellationToken {
        public static CancellationToken None => default(CancellationToken);
    }

    public struct ValueTask<TResult>
    {
        private readonly TResult _result;
        public ValueTask(TResult result) { _result = result; }
        public ValueTaskAwaiter<TResult> GetAwaiter() => new ValueTaskAwaiter<TResult>(_result);
    }

    public struct ValueTaskAwaiter<T> : System.Runtime.CompilerServices.INotifyCompletion
    {
        private readonly T _result;
        public ValueTaskAwaiter(T result) { _result = result; }
        public bool IsCompleted => true;
        public T GetResult() => _result;
        public void OnCompleted(Action continuation) { continuation(); }
    }

    public struct ValueTask
    {
        public ValueTaskAwaiter GetAwaiter() => new ValueTaskAwaiter();
    }

    public struct ValueTaskAwaiter : System.Runtime.CompilerServices.INotifyCompletion
    {
        public bool IsCompleted => true;
        public void GetResult() {}
        public void OnCompleted(Action continuation) { continuation(); }
    }
}

namespace System.Collections.Generic
{
    public interface IAsyncEnumerable<out T>
    {
        IAsyncEnumerator<T> GetAsyncEnumerator(System.Threading.CancellationToken cancellationToken = default);
    }

    public interface IAsyncEnumerator<out T> : IAsyncDisposable
    {
        T Current { get; }
        System.Threading.Tasks.ValueTask<bool> MoveNextAsync();
    }
}

namespace System
{
    public interface IAsyncDisposable
    {
        System.Threading.Tasks.ValueTask DisposeAsync();
    }
}

public class Program
{
    public static async Task Main()
    {
        await foreach (var number in GenerateSequence())
        {
            Console.WriteLine(number);
        }
    }

    public static IAsyncEnumerable<int> GenerateSequence()
    {
        return new ManualAsyncEnumerable();
    }

    class ManualAsyncEnumerable : IAsyncEnumerable<int>
    {
        public IAsyncEnumerator<int> GetAsyncEnumerator(System.Threading.CancellationToken cancellationToken = default) => new ManualAsyncEnumerator();
    }

    class ManualAsyncEnumerator : IAsyncEnumerator<int>
    {
        private int _current = -1;
        public int Current => _current;
        public System.Threading.Tasks.ValueTask<bool> MoveNextAsync()
        {
            _current++;
            return new System.Threading.Tasks.ValueTask<bool>(_current < 3);
        }
        public System.Threading.Tasks.ValueTask DisposeAsync() => new System.Threading.Tasks.ValueTask();
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task UnmanagedConstructedTypes()
        {
            var code = """
using System;

namespace System.Runtime.InteropServices {
    public enum UnmanagedType { }
}

public struct Coordinates<T> where T : unmanaged
{
    public T X;
    public T Y;
}

public class Program
{
    public static void Main()
    {
        var c = new Coordinates<int> { X = 10, Y = 20 };
        Console.WriteLine(c.X);
    }
}
""";
            await RunTestExpectingError(code, "Unmanaged constraint is not supported");
        }

        [TestMethod]
        public async Task StackAllocInNestedExpressions()
        {
            // Requires Span support
            var code = """
using System;

namespace System
{
    public struct Span<T>
    {
        private T[] _array;
        public Span(T[] array)
        {
            _array = array;
        }
        public int Length => _array != null ? _array.Length : 0;
        public static implicit operator Span<T>(T[] array) => new Span<T>(array);
    }
}

public class Program
{
    public static void Main()
    {
        Span<int> span = stackalloc[] { 1, 2, 3 };
        Console.WriteLine(span.Length);
    }
}
""";
            await RunTest(code, waitForOutput: "3", skipRoslyn: true);
        }

        [TestMethod]
        public async Task ExplicitStackAlloc()
        {
            var code = """
using System;

namespace System
{
    public struct Span<T>
    {
        private T[] _array;
        public Span(T[] array)
        {
            _array = array;
        }
        public int Length => _array != null ? _array.Length : 0;
        public static implicit operator Span<T>(T[] array) => new Span<T>(array);
    }
}

public class Program
{
    public static void Main()
    {
        Span<int> span = stackalloc int[] { 1, 2, 3 };
        Console.WriteLine(span.Length);

        Span<int> span2 = stackalloc int[4];
        Console.WriteLine(span2.Length);
    }
}
""";
            await RunTest(code, waitForOutput: "3\n4", skipRoslyn: true);
        }

        [TestMethod]
        public async Task StackAllocWithCustomSpan()
        {
            var code = """
using System;

public class CustomSpan<T>
{
    private T[] _array;
    public CustomSpan(T[] array)
    {
        _array = array;
    }

    public int Length => _array.Length;

    public static implicit operator CustomSpan<T>(T[] array) => new CustomSpan<T>(array);
}

public class Program
{
    public static void Main()
    {
        CustomSpan<int> span = stackalloc int[] { 1, 2, 3 };
        Console.WriteLine(span.Length);

        CustomSpan<int> span2 = stackalloc[] { 4, 5 };
        Console.WriteLine(span2.Length);

        CustomSpan<int> span3 = stackalloc int[10];
        Console.WriteLine(span3.Length);
    }
}
""";
            await RunTest(code, waitForOutput: "3\n2\n10", skipRoslyn: true);
        }
    }
}
