using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class CSharp11Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task RawStringLiterals()
        {
            var code = "\"\"\"\nLine 1\nLine 2\n\"\"\"";
            var program = $@"
using System;

public class Program
{{
    public static void Main()
    {{
        string s = {code};
        Console.WriteLine(s);
    }}
}}";
            await RunTest(program);
        }

        [TestMethod]
        public async Task ListPatterns()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int[] numbers = { 1, 2, 3 };

        Console.WriteLine(numbers is [1, 2, 3]);
        Console.WriteLine(numbers is [1, 2, ..]);
        Console.WriteLine(numbers is [.., 3]);
        Console.WriteLine(numbers is [1, _, 3]);

        if (numbers is [var first, .. var rest])
        {
            Console.WriteLine(first);
            Console.WriteLine(rest.Length);
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task RequiredMembers()
        {
            var code = """
using System;
using System.Diagnostics.CodeAnalysis;

public class Person
{
    public required string Name { get; init; }
}

public class Program
{
    public static void Main()
    {
        var p = new Person { Name = "Alice" };
        Console.WriteLine(p.Name);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task AutoDefaultStructs()
        {
            var code = """
using System;

public struct MyStruct
{
    public int X;
    public int Y;

    public MyStruct() // Fields auto-initialized to default if not set
    {
        X = 10;
        // Y is 0
    }
}

public class Program
{
    public static void Main()
    {
        var s = new MyStruct();
        Console.WriteLine(s.X);
        Console.WriteLine(s.Y);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task PatternMatchSpanOnConstantString()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        ReadOnlySpan<char> span = "Hello World".AsSpan();
        if (span is "Hello World")
        {
            Console.WriteLine("Matched");
        }
    }
}
""";
            // Need Span support in runtime
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task CheckedUserDefinedOperators()
        {
            var code = """
using System;

public struct IntWrapper
{
    public int Value;
    public IntWrapper(int v) => Value = v;

    public static IntWrapper operator +(IntWrapper a, IntWrapper b) => new IntWrapper(a.Value + b.Value);

    public static IntWrapper operator checked +(IntWrapper a, IntWrapper b)
    {
        checked
        {
            return new IntWrapper(a.Value + b.Value);
        }
    }
}

public class Program
{
    public static void Main()
    {
        var a = new IntWrapper(10);
        var b = new IntWrapper(20);
        var c = checked(a + b);
        Console.WriteLine(c.Value);
    }
}
""";
            await RunTest(code);
        }
    }
}
