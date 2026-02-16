using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp90Tests : IntegrationTestBase
    {
        [TestMethod]
        [Ignore("Missing runtime support for records (System.Type.op_Equality, etc)")]
        public async Task Records()
        {
            var code = """
using System;

namespace System.Runtime.CompilerServices { internal static class IsExternalInit {} }

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
        // Console.WriteLine(p1); // Person { FirstName = John, LastName = Doe } -- ToString() might vary in H5 default implementation
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

namespace System.Runtime.CompilerServices { internal static class IsExternalInit {} }

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
            await RunTest(code, skipRoslyn: true);
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
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task TargetTypedConditional()
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
        [Ignore("NRefactory 5 limitation: Covariant return types are not supported by the parser.")]
        public async Task CovariantReturnTypes()
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
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        // [Ignore("Not implemented yet")]
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
            await RunTest(code, skipRoslyn: true);
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

namespace System
{
    public struct IntPtr
    {
        private int _value;
        public IntPtr(int value) { _value = value; }
        public IntPtr(long value) { _value = (int)value; }
        public static implicit operator IntPtr(int value) => new IntPtr(value);
        public static implicit operator int(IntPtr value) => value._value;
        public static IntPtr operator +(IntPtr a, IntPtr b) => new IntPtr(a._value + b._value);
        public override string ToString() => _value.ToString();
        public string Name => "IntPtr";
    }
    public struct UIntPtr
    {
        private uint _value;
        public UIntPtr(uint value) { _value = value; }
        public UIntPtr(ulong value) { _value = (uint)value; }
        public static implicit operator UIntPtr(uint value) => new UIntPtr(value);
        public static implicit operator uint(UIntPtr value) => value._value;
        public static UIntPtr operator +(UIntPtr a, UIntPtr b) => new UIntPtr(a._value + b._value);
        public override string ToString() => _value.ToString();
        public string Name => "UIntPtr";
    }
}

public class Program
{
    public static void Main()
    {
        nint x = 10;
        nuint y = 20;
        Console.WriteLine(x + (nint)y);
        // Console.WriteLine(typeof(nint).Name); // TypeOf might return runtime type name.
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        [Ignore("Requires OutputType=Exe which is not configurable in tests")]
        public async Task TopLevelStatements()
        {
            var code = """
using System;
Console.WriteLine("Hello, Top-Level Statements!");
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Environment configuration issue: CS8783")]
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
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        [Ignore("Environment configuration issue: ModuleInitializerAttribute not found")]
        public async Task ModuleInitializers()
        {
            var code = """
using System;
using System.Runtime.CompilerServices;

namespace System.Runtime.CompilerServices {
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ModuleInitializerAttribute : Attribute {}
}

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
            await RunTest(code, skipRoslyn: true);
        }
    }
}
