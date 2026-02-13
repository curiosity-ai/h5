using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class CSharp60Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task StringInterpolation()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        int x = 10;
        string s = "world";
        Console.WriteLine($"Hello {s}, value: {x}");
        Console.WriteLine($"Calc: {x + 5}");

        DateTime d = new DateTime(2023, 10, 25);
        Console.WriteLine($"Date: {d:yyyy-MM-dd}");

        double price = 123.456;
        Console.WriteLine($"Price: {price:F2}");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NullConditionalOperator()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        string s = null;
        Console.WriteLine("Length: " + (s?.Length ?? -1));

        s = "hello";
        Console.WriteLine("Length: " + s?.Length);

        int[] arr = null;
        Console.WriteLine("Index: " + (arr?[0] ?? -1));

        arr = new int[] { 42 };
        Console.WriteLine("Index: " + arr?[0]);

        // Nested property access
        Person p = null;
        Console.WriteLine("Name: " + (p?.Name ?? "Unknown"));

        p = new Person();
        Console.WriteLine("Name: " + (p?.Name ?? "Unknown")); // Name is null

        p.Name = "Alice";
        Console.WriteLine("Name: " + p?.Name);
    }

    class Person { public string Name { get; set; } }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ExpressionBodiedMembers()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        var c = new Calc(10, 20);
        Console.WriteLine("Sum: " + c.Sum);
        Console.WriteLine("ToString: " + c.ToString());
        c.PrintHello();
    }

    class Calc
    {
        private int _a, _b;
        public Calc(int a, int b) { _a = a; _b = b; }

        public int Sum => _a + _b; // Expression-bodied property

        public override string ToString() => $"Calc({_a}, {_b})"; // Expression-bodied method returning value

        public void PrintHello() => Console.WriteLine("Hello"); // Expression-bodied void method
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ExceptionFilters_Passing()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        try
        {
            throw new Exception("Test");
        }
        catch (Exception ex) when (true)
        {
            Console.WriteLine("Caught with when(true)");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ExceptionFilters_Failing()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        try
        {
            throw new Exception("Error 1");
        }
        catch (Exception ex) when (ex.Message == "Error 1")
        {
            Console.WriteLine("Caught Error 1");
        }
        catch (Exception)
        {
            Console.WriteLine("Caught other error");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NameofOperator()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        string myVar = "test";
        Console.WriteLine(nameof(myVar));
        Console.WriteLine(nameof(Program));
        Console.WriteLine(nameof(Console.WriteLine));
        Console.WriteLine(nameof(Program.Main));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task AutoPropertyInitializers()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        var p = new Person();
        Console.WriteLine("Age: " + p.Age);
        Console.WriteLine("Name: " + p.Name);

        p.Age = 30;
        Console.WriteLine("New Age: " + p.Age);
    }

    class Person
    {
        public int Age { get; set; } = 25;
        public string Name { get; set; } = "John Doe";
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GetterOnlyAutoProperties()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        var p = new Person(42);
        Console.WriteLine("ID: " + p.Id);
    }

    class Person
    {
        public int Id { get; }

        public Person(int id)
        {
            Id = id;
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task IndexInitializers()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        var dict = new Dictionary<int, string>
        {
            [1] = "One",
            [2] = "Two",
            [3] = "Three"
        };

        foreach (var kvp in dict)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task StaticImports()
        {
            var code = """
using System;
using static System.Math;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Sqrt(16): " + Sqrt(16));
        Console.WriteLine("Abs(-10): " + Abs(-10));
        Console.WriteLine("Max(5, 10): " + Max(5, 10));
        Console.WriteLine("PI: " + Math.Round(PI, 2));
    }
}
""";
            await RunTest(code);
        }
    }
}
