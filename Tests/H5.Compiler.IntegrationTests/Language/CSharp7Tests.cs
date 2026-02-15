using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class CSharp7Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task OutVariables()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        if (int.TryParse("123", out int result))
        {
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine("Failed");
        }

        PrintOut(out var msg);
        Console.WriteLine(msg);
    }

    static void PrintOut(out string message)
    {
        message = "Hello Out Variable";
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Tuples()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        (int, string) t = (1, "A");
        Console.WriteLine(t.Item1);
        Console.WriteLine(t.Item2);

        var t2 = (Val: 2, Name: "B");
        Console.WriteLine(t2.Val);
        Console.WriteLine(t2.Name);

        (int Id, string Text) t3 = (3, "C");
        Console.WriteLine(t3.Id);
        Console.WriteLine(t3.Text);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Deconstruction()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        var t = (1, "A");
        (int x, string y) = t;
        Console.WriteLine(x);
        Console.WriteLine(y);

        var p = new Point(10, 20);
        (var px, var py) = p;
        Console.WriteLine(px);
        Console.WriteLine(py);
    }
}

public class Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task PatternMatching()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        object obj = 123;
        if (obj is int i)
        {
            Console.WriteLine("Integer: " + i);
        }

        PrintType("hello");
        PrintType(456);
        PrintType(null);
    }

    static void PrintType(object o)
    {
        switch (o)
        {
            case int n:
                Console.WriteLine("Int: " + n);
                break;
            case string s:
                Console.WriteLine("String: " + s);
                break;
            case null:
                Console.WriteLine("Null");
                break;
            default:
                Console.WriteLine("Unknown");
                break;
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task LocalFunctions()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(Add(5, 10));

        int Add(int x, int y)
        {
            return x + y;
        }

        void PrintLocal(string msg)
        {
            Console.WriteLine("Local: " + msg);
        }

        PrintLocal("Test");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Discards()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        _ = GetValue();

        (int _, string s) = (1, "Test");
        Console.WriteLine(s);

        if (int.TryParse("123", out _))
        {
            Console.WriteLine("Parsed");
        }
    }

    static int GetValue()
    {
        Console.WriteLine("GetValue called");
        return 42;
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task LiteralImprovements()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int bin = 0b101;
        Console.WriteLine(bin);

        int sep = 1_000_000;
        Console.WriteLine(sep);

        double d = 1_2.3_4;
        Console.WriteLine(d);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ThrowExpressions()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        string s = null;
        try
        {
            string v = s ?? throw new Exception("Null string");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        try
        {
             GetValue(true);
        }
        catch (Exception ex)
        {
             Console.WriteLine(ex.Message);
        }
    }

    static int GetValue(bool fail) => fail ? throw new Exception("Failed") : 0;
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task DefaultLiteral()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int i = default;
        Console.WriteLine(i);

        string s = default;
        Console.WriteLine(s == null);

        Print(default);
    }

    static void Print(int x = default)
    {
        Console.WriteLine(x);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task PrivateProtected()
        {
             var code = """
using System;

public class Base
{
    private protected int Value = 10;
}

public class Derived : Base
{
    public void Print()
    {
        Console.WriteLine(Value);
    }
}

public class Program
{
    public static void Main()
    {
        var d = new Derived();
        d.Print();
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NonTrailingNamedArguments()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Print(1, c: 3, b: 2);
        Print(1, 2, c: 3);
    }

    static void Print(int a, int b, int c)
    {
        Console.WriteLine(a);
        Console.WriteLine(b);
        Console.WriteLine(c);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task InParameters()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int x = 10;
        Print(in x);
        Print(20);
    }

    static void Print(in int val)
    {
        Console.WriteLine(val);
        // val = 20; // Error if uncommented
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ReadOnlyStructs()
        {
            var code = """
using System;

public readonly struct Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"({X}, {Y})";
}

public class Program
{
    public static void Main()
    {
        var p = new Point(10, 20);
        Console.WriteLine(p.X);
        Console.WriteLine(p.Y);
        Console.WriteLine(p.ToString());
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task LambdaDiscards()
        {
            var code = """
using System;

public class CommandBarItem { }

public class Program
{
    public static void Main()
    {
        var p = new Program();
        bool called = false;
        p.OnClick(() => { called = true; });
        if (called)
        {
            Console.WriteLine("Success");
        }
    }

    public CommandBarItem OnClick(Action action)
    {
        return OnClick((_, __) => action?.Invoke());
    }

    public CommandBarItem OnClick(Action<object, object> action)
    {
        action(null, null);
        return new CommandBarItem();
    }
}
""";
            await RunTest(code);
        }
    }
}
