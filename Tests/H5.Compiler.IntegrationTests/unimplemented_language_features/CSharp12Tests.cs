using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp12Tests : IntegrationTestBase
    {
        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task PrimaryConstructors()
        {
            var code = """
using System;

public class Person(string name, int age)
{
    public string Name => name;
    public int Age => age;

    public void Print() => Console.WriteLine($"{name} is {age}");
}

public class Program
{
    public static void Main()
    {
        var p = new Person("Alice", 30);
        p.Print();
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
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

        Console.WriteLine(a.Length);
        Console.WriteLine(b.Count);
        Console.WriteLine(c.Length);

        // Spread operator
        int[] d = [..a, ..b];
        Console.WriteLine(d.Length); // 6
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task InlineArrays()
        {
            var code = """
using System;
using System.Runtime.CompilerServices;

// InlineArrayAttribute needs to be available
/*
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
    internal sealed class InlineArrayAttribute : Attribute
    {
        public InlineArrayAttribute(int length) { Length = length; }
        public int Length { get; }
    }
}
*/

[InlineArray(10)]
public struct Buffer10
{
    private int _element0;
}

public class Program
{
    public static void Main()
    {
        var buffer = new Buffer10();
        buffer[0] = 42;
        Console.WriteLine(buffer[0]);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
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
        public async Task AliasAnyType()
        {
            var code = """
using System;
using IntList = System.Collections.Generic.List<int>;
using PointTuple = (int X, int Y);

public class Program
{
    public static void Main()
    {
        var list = new IntList();
        list.Add(1);
        Console.WriteLine(list[0]);

        PointTuple p = (10, 20);
        Console.WriteLine(p.X);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
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
        Print2(ref x);
    }

    static void Print(in int x) => Console.WriteLine(x);
    static void Print2(ref readonly int x) => Console.WriteLine(x);
}
""";
            await RunTest(code);
        }
    }
}
