using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp11Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task RawStringLiterals()
        {
            var code = """"
using System;

public class Program
{
    public static void Main()
    {
        var json = """
        {
            "name": "John",
            "age": 30
        }
        """;
        Console.WriteLine(json.Contains("name"));
    }
}
"""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task GenericAttributes()
        {
            var code = """
using System;

[AttributeUsage(AttributeTargets.Class)]
public class GenericAttribute<T> : Attribute
{
    public string TypeName => typeof(T).Name;
}

[GenericAttribute<int>]
public class MyClass { }

public class Program
{
    public static void Main()
    {
        var attr = (GenericAttribute<int>)Attribute.GetCustomAttribute(typeof(MyClass), typeof(GenericAttribute<int>));
        Console.WriteLine(attr.TypeName);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task ListPatterns()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int[] numbers = { 1, 2, 3, 4, 5 };

        if (numbers is [1, 2, .. var rest])
        {
            Console.WriteLine(rest.Length); // 3
        }

        if (numbers is [.., 5])
        {
            Console.WriteLine("Ends with 5");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task RequiredMembers()
        {
            var code = """
using System;
using System.Runtime.CompilerServices;

// Polyfills might be needed if not in reference assemblies
/*
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal sealed class RequiredMemberAttribute : Attribute {}
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    internal sealed class SetsRequiredMembersAttribute : Attribute {}
}
*/

public class Person
{
    public required string Name { get; set; }
}

public class Program
{
    public static void Main()
    {
        var p = new Person { Name = "Alice" };
        Console.WriteLine(p.Name);

        // new Person(); // Error
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

public struct S
{
    public int X;
    public int Y;

    public S(int x)
    {
        X = x;
        // Y is auto-defaulted to 0
    }
}

public class Program
{
    public static void Main()
    {
        var s = new S(10);
        Console.WriteLine(s.X);
        Console.WriteLine(s.Y);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NewlinesInInterpolations()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        var s = $"Hello {
            "World"
        }!";
        Console.WriteLine(s);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task FileLocalTypes()
        {
            var code = """
using System;

file class LocalHidden
{
    public string Message = "Hidden";
}

public class Program
{
    public static void Main()
    {
        var h = new LocalHidden();
        Console.WriteLine(h.Message);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task GenericMath()
        {
            var code = """
using System;

public interface IAdd<T> where T : IAdd<T>
{
    static abstract T operator +(T left, T right);
}

public struct MyInt : IAdd<MyInt>
{
    public int Value;
    public MyInt(int v) => Value = v;
    public static MyInt operator +(MyInt left, MyInt right) => new MyInt(left.Value + right.Value);
}

public class Program
{
    public static T Add<T>(T a, T b) where T : IAdd<T> => a + b;

    public static void Main()
    {
        var res = Add(new MyInt(1), new MyInt(2));
        Console.WriteLine(res.Value);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task Utf8StringLiterals()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        ReadOnlySpan<byte> utf8 = "Hello"u8;
        Console.WriteLine(utf8.Length);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task PatternMatchSpanChar()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        ReadOnlySpan<char> s = "Hello";
        if (s is "Hello")
        {
            Console.WriteLine("Matched");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ExtendedNameof()
        {
            var code = """
using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Parameter)]
public class ParamInfoAttribute : Attribute
{
    public ParamInfoAttribute(string name) { Name = name; }
    public string Name { get; }
}

public class Program
{
    public static void Main()
    {
        var method = typeof(Program).GetMethod("Print", BindingFlags.Static | BindingFlags.NonPublic);
        var param = method.GetParameters()[0];
        var attr = (ParamInfoAttribute)param.GetCustomAttributes(typeof(ParamInfoAttribute), false)[0];
        Console.WriteLine(attr.Name);
    }

    static void Print([ParamInfo(nameof(x))] int x)
    {
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ExtendedNameof_MethodAttribute()
        {
            var code = """
using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class MethodInfoAttribute : Attribute
{
    public MethodInfoAttribute(string paramName) { ParamName = paramName; }
    public string ParamName { get; }
}

public class Program
{
    public static void Main()
    {
        var method = typeof(Program).GetMethod("Test", BindingFlags.Static | BindingFlags.Public);
        var attr = (MethodInfoAttribute)method.GetCustomAttributes(typeof(MethodInfoAttribute), false)[0];
        Console.WriteLine(attr.ParamName);
    }

    [MethodInfo(nameof(p))]
    public static void Test(int p)
    {
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ExtendedNameof_Qualified()
        {
            var code = """
using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class MyAttr : Attribute
{
    public MyAttr(string name) { Name = name; }
    public string Name { get; }
}

public class Program
{
    public static void Main()
    {
        var method = typeof(Program).GetMethod("Test", BindingFlags.Instance | BindingFlags.Public);
        var attr = (MyAttr)method.GetCustomAttributes(typeof(MyAttr), false)[0];
        Console.WriteLine(attr.Name);
    }

    [MyAttr(nameof(System.Int32))]
    public void Test()
    {
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task NumericIntPtr()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        IntPtr x = 10; // Implicit conversion from int now standard? Or just via nint aliasing.
        nint y = 20;
        Console.WriteLine(x + y);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task RefFields()
        {
            var code = """
using System;

public ref struct RefContainer
{
    public ref int Field;
}

public class Program
{
    public static void Main()
    {
        int x = 10;
        var c = new RefContainer { Field = ref x };
        c.Field = 20;
        Console.WriteLine(x);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task ScopedRef()
        {
            var code = """
using System;

public ref struct S
{
    public void Method(scoped ref int x) { }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Scoped ref compiled");
    }
}
""";
            await RunTest(code);
        }
    }
}
