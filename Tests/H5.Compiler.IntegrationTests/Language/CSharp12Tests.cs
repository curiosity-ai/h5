using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class CSharp12Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task PrimaryConstructors()
        {
            var code = """
using System;

public class Person(string name, int age)
{
    public void Print() => Console.WriteLine($"{name} is {age}");
}

public class Program
{
    public static void Main()
    {
        var p = new Person("Bob", 30);
        p.Print();
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task CollectionExpressions()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        int[] a = [1, 2, 3];
        List<int> b = [4, 5, 6];
        Span<int> c = [7, 8, 9];

        int[] d = [..a, ..b];

        Console.WriteLine(string.Join(",", d));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task InlineArrays()
        {
            var code = """
using System;
using System.Runtime.CompilerServices;

[InlineArray(10)]
public struct MyBuffer
{
    private int _element0;
}

public class Program
{
    public static void Main()
    {
        var buffer = new MyBuffer();
        buffer[0] = 42;
        buffer[9] = 100;

        Console.WriteLine(buffer[0]);
        Console.WriteLine(buffer[9]);

        foreach(var item in buffer)
        {
            // iterate
        }
    }
}
""";
            // Need InlineArray support in runtime
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task OptionalParamsInLambdas()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        var f = (int x = 10) => x * 2;
        Console.WriteLine(f()); // 20
        Console.WriteLine(f(5)); // 10
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task RefReadonlyParameters()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int x = 10;
        Print(in x);
        Print(ref readonly x); // C# 12 syntax
    }

    static void Print(ref readonly int val)
    {
        Console.WriteLine(val);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task AliasAnyType()
        {
            var code = """
using System;
using MyInt = int;
using MyTuple = (int X, int Y);
using MyArray = int[];

public class Program
{
    public static void Main()
    {
        MyInt i = 10;
        MyTuple t = (1, 2);
        MyArray a = new[] { 1, 2, 3 };

        Console.WriteLine(i);
        Console.WriteLine(t.X);
        Console.WriteLine(a.Length);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task PrimaryConstructors_EdgeCases()
        {
            var code = """
using System;

public class Base(int x)
{
    public int X = x;
}

public class Derived(int x, int y) : Base(x + 1)
{
    public int Y = y;
    public int Z { get; } = x + y; // Uses params in initializer
    public int W => x * y; // Uses params in expression body (capture)

    public void Print()
    {
        Console.WriteLine($"{X}, {Y}, {Z}, {W}");
    }
}

public struct Point(int x, int y)
{
    public int X = x;
    public int Y = y;
    public void Print() => Console.WriteLine($"Point({X}, {Y})");
}

public class Program
{
    public static void Main()
    {
        var d = new Derived(10, 20);
        d.Print(); // Should print: 11, 20, 30, 200

        var pt = new Point(5, 6);
        pt.Print(); // Should print: Point(5, 6)
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericOptionalParamsInLambdas()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Run<MyClass>(new MyClass());
    }

    public static void Run<T>(T val) where T : class, new()
    {
        var f = (T x = null) => x ?? new T();

        var v1 = f(); // null -> new T()
        Console.WriteLine(v1 != null);

        var v2 = f(val); // val
        Console.WriteLine(v2 == val);
    }
}

public class MyClass { }
""";
            await RunTest(code);
        }
    }
}
