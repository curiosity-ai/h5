using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class CSharp9Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task Records_1()
        {
            var code = """
using System;

public record Person(string FirstName, string LastName);

public class Program
{
    public static void Main()
    {
        var p1 = new Person("John", "Doe");
        var p2 = new Person("John", "Doe");
        var p3 = new Person("Jane", "Doe");

        Console.WriteLine(p1 == p2);
        Console.WriteLine(p1 == p3);
        Console.WriteLine(p1);

        var p4 = p1 with { FirstName = "Jane" };
        Console.WriteLine(p4);
        Console.WriteLine(p4 == p3);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Records_2()
        {
            var code = """
using System;

public record Person(string FirstName, string LastName);

public class Program
{
    public static void Main()
    {
        var p1 = new Person("John", "Doe");
        var p2 = new Person("John", "Doe");
        var p3 = p1 with { FirstName = "Jane" };

        Console.WriteLine(p1 == p2); // True
        Console.WriteLine(p1 == p3); // False
        Console.WriteLine(p3.FirstName); // Jane
        Console.WriteLine(p3.LastName); // Doe
        Console.WriteLine(p1.ToString()); // Person { FirstName = John, LastName = Doe }
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task InitOnlySetters()
        {
            var code = """
using System;

public class Point
{
    public int X { get; init; }
    public int Y { get; init; }
}

public class Program
{
    public static void Main()
    {
        var p = new Point { X = 10, Y = 20 };
        Console.WriteLine(p.X);
        Console.WriteLine(p.Y);
        // p.X = 30; // Error
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task PatternMatchingEnhancements()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(IsLetter('a'));
        Console.WriteLine(IsLetter('1'));
        Console.WriteLine(GetDescription(10));
        Console.WriteLine(GetDescription(null));
    }

    public static bool IsLetter(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';

    public static string GetDescription(object o) => o switch
    {
        int i and > 0 => "Positive integer",
        not null => "Object",
        null => "Null"
    };
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task PatternMatchingEnhancements_2()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(Classify(5));
        Console.WriteLine(Classify(15));
        Console.WriteLine(Classify(null));

        Console.WriteLine(IsLetter('a'));
        Console.WriteLine(IsLetter('1'));
    }

    static string Classify(int? i) => i switch
    {
        null => "Null",
        < 10 and > 0 => "Small Positive",
        > 10 => "Large",
        not 0 => "Non-zero",
        _ => "Zero"
    };

    static bool IsLetter(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task TargetTypedNew()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        List<int> list = new();
        list.Add(1);
        Console.WriteLine(list.Count);

        Point p = new(3, 4);
        Console.WriteLine(p.X);
    }
}

public class Point
{
    public int X, Y;
    public Point(int x, int y) { X = x; Y = y; }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task TargetTypedNew_2()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        Dictionary<string, int> dict = new() { { "A", 1 } };
        Console.WriteLine(dict["A"]);

        Point p = new(10, 20);
        Console.WriteLine(p.X);
    }
}

public class Point
{
    public int X, Y;
    public Point(int x, int y) { X = x; Y = y; }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task TargetTypedConditional()
        {
            var code = """
using System;

public class Base { }
public class Derived : Base { }

public class Program
{
    public static void Main()
    {
        var b = true ? new Base() : new Derived();
        Console.WriteLine(b.GetType().Name);

        int? x = true ? 1 : null;
        Console.WriteLine(x.HasValue);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task TargetTypedConditional_2()
        {
            var code = """
using System;

public class Base { }
public class Derived1 : Base { }
public class Derived2 : Base { }

public class Program
{
    public static void Main()
    {
        Base b = true ? new Derived1() : new Derived2();
        Console.WriteLine(b is Base);

        int? x = true ? 1 : null;
        Console.WriteLine(x.HasValue);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task StaticAnonymousFunctions()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Func<int, int> f = static x => x * x;
        Console.WriteLine(f(5));

        int y = 10;
        // Func<int> g = static () => y; // Error, can't capture a local variable with a static lambda
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task CovariantReturnTypes()
        {
            var code = """
using System;

public class Food { }
public class Meat : Food { }

public class Animal
{
    public virtual Food GetFood() => new Food();
}

public class Tiger : Animal
{
    public override Meat GetFood() => new Meat();
}

public class Program
{
    public static void Main()
    {
        Animal a = new Tiger();
        Console.WriteLine(a.GetFood().GetType().Name);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task CovariantReturnTypes_2()
        {
            var code = """
using System;

public class Base
{
    public virtual Base Clone() => new Base();
}

public class Derived : Base
{
    public override Derived Clone() => new Derived();
}

public class Program
{
    public static void Main()
    {
        Derived d = new Derived();
        Derived d2 = d.Clone();
        Console.WriteLine(d2 is Derived);
    }
}
""";
            await RunTest(code);
        }


        [TestMethod]
        public async Task TopLevelStatements()
        {
            var code = """
using System;
Console.WriteLine("Hello, Top-Level Statements!");
""";
            await RunTestExpectingError(code, "Top-level statements are not supported");
        }


        [TestMethod]
        public async Task ExtensionGetEnumerator()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Range
{
    public int From { get; }
    public int To { get; }
    public Range(int from, int to) { From = from; To = to; }
}

public static class Extensions
{
    public static IEnumerator<int> GetEnumerator(this Range range)
    {
        for (int i = range.From; i < range.To; i++)
            yield return i;
    }
}

public class Program
{
    public static void Main()
    {
        foreach (var i in new Range(0, 3))
        {
            Console.WriteLine(i);
        }
    }
}
""";
            // Skip Roslyn because extension methods in script wrapper classes are problematic
            var output = await RunTest(code, skipRoslyn: true);
            Assert.AreEqual("0\n1\n2", output);
        }

        [TestMethod]
        public async Task LambdaDiscardParameters()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Func<int, int, int> f = (_, _) => 42;
        Console.WriteLine(f(1, 2));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NativeSizedIntegers()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        nint x = 10;
        nuint y = 20;
        Console.WriteLine(x + (nint)y);
        Console.WriteLine(typeof(nint).Name);
    }
}
""";
            // Skip Roslyn as nint context might differ
            var output = await RunTest(code, skipRoslyn: true);
            Assert.IsTrue(output.Contains("30"));
            Assert.IsTrue(output.Contains("Int32") || output.Contains("IntPtr"));
        }


        [TestMethod]
        public async Task AttributesOnLocalFunctions()
        {
            var code = """
using System;
using System.Diagnostics;

public class Program
{
    public static void Main()
    {
        [Conditional("DEBUG")]
        static void LocalFunc()
        {
            Console.WriteLine("Debug");
        }
        LocalFunc();
    }
}
""";
            var output = await RunTest(code, skipRoslyn: true);
            Assert.AreEqual("Debug", output);
        }

        [TestMethod]
        public async Task ModuleInitializers()
        {
            var code = """
using System;
using System.Runtime.CompilerServices;

public class Program
{
    public static bool Initialized = false;

    [ModuleInitializer]
    public static void Init()
    {
        Initialized = true;
    }

    public static void Main()
    {
        Console.WriteLine(Initialized);
    }
}
""";
            await RunTest(code);
        }

    }
}
