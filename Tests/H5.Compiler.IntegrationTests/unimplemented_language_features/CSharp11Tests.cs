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
    }
}
