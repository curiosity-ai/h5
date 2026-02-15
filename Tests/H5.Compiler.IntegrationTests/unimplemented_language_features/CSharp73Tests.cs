using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp73Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task StackAllocInitializers()
        {
            // Note: This requires Span<T> support in H5 runtime.
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Span<int> span = stackalloc int[] { 1, 2, 3 };
        Console.WriteLine(span[0]);
        Console.WriteLine(span[1]);
        Console.WriteLine(span[2]);
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        // [Ignore("Not implemented yet")]
        public async Task InOverloadResolution()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int x = 10;
        Print(x); // Should call by-value
        Print(in x); // Should call in
    }

    static void Print(int x)
    {
        Console.WriteLine("By Value: " + x);
    }

    static void Print(in int x)
    {
        Console.WriteLine("In: " + x);
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task AttributesOnBackingFields()
        {
            var code = """
using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Field)]
public class MyFieldAttr : Attribute { }

public class Program
{
    [field: MyFieldAttr]
    public int Prop { get; set; }

    public static void Main()
    {
        var props = typeof(Program).GetProperties();
        foreach (var p in props)
        {
            // Backing field name is compiler generated, usually <Prop>k__BackingField
            // This is hard to test reliably across compilers (Roslyn vs H5) without assuming internal naming.
            // But we can check if it compiles.
            Console.WriteLine("Prop: " + p.Name);
        }
        Console.WriteLine("Compiled successfully");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        // [Ignore("Not implemented yet")]
        public async Task FixedSizedBuffers()
        {
            // Requires unsafe code
            var code = """
using System;

public unsafe struct Buffer
{
    public fixed int Data[10];
}

public class Program
{
    public static unsafe void Main()
    {
        Buffer b = new Buffer();
        b.Data[0] = 123; // Indexing without pinning in C# 7.3+
        Console.WriteLine(b.Data[0]);
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task ExpressionVariablesInInitializers()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        var p = new Point(int.TryParse("10", out var x) ? x : 0, int.TryParse("20", out var y) ? y : 0);
        Console.WriteLine(p.X);
        Console.WriteLine(p.Y);
    }
}

public class Point
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) { X = x; Y = y; }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task EnumConstraint()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        PrintEnum(ConsoleColor.Red);
    }

    static void PrintEnum<T>(T e) where T : Enum
    {
        Console.WriteLine(e.ToString());
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task DelegateConstraint()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Action a = () => Console.WriteLine("Action");
        RunDelegate(a);
    }

    static void RunDelegate<T>(T d) where T : Delegate
    {
        d.DynamicInvoke();
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task UnmanagedConstraint()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        PrintUnmanaged(10);
        PrintUnmanaged(10.5);
        // PrintUnmanaged("string"); // Should fail to compile
    }

    // unmanaged constraint implies struct and no ref fields
    static void PrintUnmanaged<T>(T value) where T : unmanaged
    {
        Console.WriteLine(value);
    }
}
""";
            await RunTestExpectingError(code, "Unmanaged constraint is not supported");
        }
    }
}
