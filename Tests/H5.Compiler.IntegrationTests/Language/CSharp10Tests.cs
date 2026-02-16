using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class CSharp10Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task RecordStructs()
        {
            var code = """
using System;

public record struct Point(int X, int Y);

public class Program
{
    public static void Main()
    {
        var p1 = new Point(1, 2);
        var p2 = new Point(1, 2);
        Console.WriteLine(p1 == p2);
        Console.WriteLine(p1.ToString());

        var p3 = p1 with { X = 3 };
        Console.WriteLine(p3.ToString());
    }
}
""";
            await RunTest(code);
        }



        [TestMethod]
        public async Task FileScopedNamespaces()
        {
            var code = """
using System;

namespace MyNamespace;

public class MyClass
{
    public void Hello() => Console.WriteLine("Hello File Scoped");
}

public class Program
{
    public static void Main()
    {
        new MyClass().Hello();
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task ExtendedPropertyPatterns()
        {
            var code = """
using System;

public class Address { public string City { get; set; } }
public class Person { public Address Address { get; set; } }

public class Program
{
    public static void Main()
    {
        var p = new Person { Address = new Address { City = "Seattle" } };

        if (p is { Address.City: "Seattle" })
        {
            Console.WriteLine("Matched Seattle");
        }

        if (p is { Address.City: "New York" })
        {
            Console.WriteLine("Matched New York");
        }
        else
        {
            Console.WriteLine("Not New York");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ConstantInterpolatedStrings()
        {
            var code = """
using System;

public class Program
{
    const string Greeting = "Hello";
    const string Name = "World";
    const string Message = $"{Greeting}, {Name}!";

    public static void Main()
    {
        Console.WriteLine(Message);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task AssignmentAndDeclarationInDeconstruction()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int x = 0;
        (x, int y) = (1, 2);
        Console.WriteLine(x);
        Console.WriteLine(y);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task LambdaImprovements()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        var f = object (bool b) => b ? 1 : "two"; // Explicit return type
        Console.WriteLine(f(true));
        Console.WriteLine(f(false));

        var a = (ref string s) => s = s.ToUpper();
        string val = "hello";
        a(ref val);
        Console.WriteLine(val);
    }
}
""";
            await RunTest(code, skipRoslyn: true); // Roslyn script issues with ref structs/lambdas sometimes
        }

        [TestMethod]
        public async Task GlobalUsings()
        {
            var code = """
global using System;
global using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        List<int> l = new List<int>();
        Console.WriteLine(l.Count);
    }
}
""";
            // Global usings are usually in a separate file or top of file.
            // This might fail if the compiler expects them in a specific way or if Roslyn script doesn't handle them well inside a submission.
            await RunTestExpectingError(code, "Global usings are not supported");
        }
    }
}
