using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class LinqAndFunctionalTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task AutoImplementedProperties()
        {
            var code = """
using System;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class Program
{
    public static void Main()
    {
        var p = new Person();
        p.Name = "Alice";
        p.Age = 30;
        Console.WriteLine(p.Name);
        Console.WriteLine(p.Age);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ObjectInitializers()
        {
            var code = """
using System;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class Program
{
    public static void Main()
    {
        var p = new Person { Name = "Bob", Age = 25 };
        Console.WriteLine(p.Name);
        Console.WriteLine(p.Age);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task CollectionInitializers()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        var list = new List<int> { 1, 2, 3 };
        foreach (var item in list)
        {
            Console.WriteLine(item);
        }

        var dict = new Dictionary<int, string> { { 1, "One" }, { 2, "Two" } };
        foreach (var key in dict.Keys)
        {
             Console.WriteLine(key + ": " + dict[key]);
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task AnonymousTypes()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        var p = new { Name = "Charlie", Age = 40 };
        Console.WriteLine(p.Name);
        Console.WriteLine(p.Age);
        // Console.WriteLine(p.ToString()); // ToString() format differs between runtimes
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ImplicitlyTypedLocals()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        var i = 10;
        var s = "Hello";
        var list = new List<string>();
        list.Add("World");

        Console.WriteLine(i);
        Console.WriteLine(s);
        foreach(var item in list) {
            Console.WriteLine(item);
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ExtensionMethods()
        {
            var code = """
using System;

public static class StringExtensions
{
    public static string Reverse(this string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}

public class Program
{
    public static void Main()
    {
        string s = "Hello";
        Console.WriteLine(s.Reverse());
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task LambdaExpressions()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        Func<int, int> square = x => x * x;
        Console.WriteLine(square(5));

        Func<int, int, int> add = (x, y) => { return x + y; };
        Console.WriteLine(add(3, 4));

        Action<string> print = s => Console.WriteLine(s);
        print("Hello Lambda");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task LinqToObjects()
        {
            var code = """
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        var evens = list.Where(x => x % 2 == 0);
        foreach (var x in evens) Console.WriteLine("Even: " + x);

        var squares = list.Select(x => x * x);
        foreach (var x in squares) Console.WriteLine("Square: " + x);

        var ordered = list.OrderByDescending(x => x);
        foreach (var x in ordered) Console.WriteLine("Ordered: " + x);

        var first = list.First();
        Console.WriteLine("First: " + first);

        var any = list.Any(x => x > 100);
        Console.WriteLine("Any > 100: " + any);

        var all = list.All(x => x > 0);
        Console.WriteLine("All > 0: " + all);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task QueryExpressions()
        {
            var code = """
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        var query = from x in list
                    where x > 5
                    orderby x descending
                    select x * 2;

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }
}
""";
            await RunTest(code);
        }
    }
}
