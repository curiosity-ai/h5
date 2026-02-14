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
    public static string ReverseText(this string s)
    {
        Console.WriteLine($"Reversing text: {s}");
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
        Console.WriteLine(s.ReverseText());
    }
}
""";

            //For Rosyln we don't wrap it in a static class as the code already is compiled as a "static class"
            var rosylnCode = """
using System;

public static string ReverseText(this string s)
{
    Console.WriteLine($"Reversing text: {s}");
    char[] charArray = s.ToCharArray();
    Array.Reverse(charArray);
    return new string(charArray);
}

public class Program
{
    public static void Main()
    {
        string s = "Hello";
        Console.WriteLine(s.ReverseText());
    }
}
""";

            await RunTest(code, overrideRoslynCode: rosylnCode);
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

        [TestMethod]
        public async Task Linq_SetOperators()
        {
            var code = """
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var list1 = new[] { 1, 2, 2, 3 };
        var list2 = new[] { 3, 4, 5 };

        var distinct = list1.Distinct();
        Console.WriteLine(distinct.Count()); // 3 (1, 2, 3)

        var union = list1.Union(list2);
        Console.WriteLine(union.Count()); // 5 (1, 2, 3, 4, 5)

        var intersect = list1.Intersect(list2);
        Console.WriteLine(intersect.Single()); // 3

        var except = list1.Except(list2);
        Console.WriteLine(except.Contains(3)); // False
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Linq_Partitioning()
        {
            var code = """
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var list = new[] { 1, 2, 3, 4, 5 };

        var take = list.Take(2);
        Console.WriteLine(take.Count()); // 2
        Console.WriteLine(take.Last()); // 2

        var skip = list.Skip(3);
        Console.WriteLine(skip.First()); // 4

        var takeWhile = list.TakeWhile(x => x < 3);
        Console.WriteLine(takeWhile.Count()); // 2

        var skipWhile = list.SkipWhile(x => x < 3);
        Console.WriteLine(skipWhile.First()); // 3
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Linq_Grouping()
        {
            var code = """
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var list = new[] {
            new { K = "A", V = 1 },
            new { K = "A", V = 2 },
            new { K = "B", V = 3 }
        };

        var groups = list.GroupBy(x => x.K);
        Console.WriteLine(groups.Count()); // 2

        foreach(var g in groups) {
            Console.WriteLine(g.Key);
            Console.WriteLine(g.Count());
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Linq_Joins()
        {
            var code = """
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var outer = new[] {
            new { Id = 1, Name = "One" },
            new { Id = 2, Name = "Two" }
        };

        var inner = new[] {
            new { Id = 1, Val = "X" },
            new { Id = 1, Val = "Y" },
            new { Id = 3, Val = "Z" }
        };

        var join = outer.Join(inner, o => o.Id, i => i.Id, (o, i) => o.Name + "-" + i.Val);
        Console.WriteLine(join.Count()); // 2
        // Sorting needed for deterministic output order if platform varies
        foreach(var item in join.OrderBy(x => x)) Console.WriteLine(item);

        var groupJoin = outer.GroupJoin(inner, o => o.Id, i => i.Id, (o, matches) => new { o.Name, Count = matches.Count() });
        foreach(var item in groupJoin.OrderBy(x => x.Name)) Console.WriteLine(item.Name + ": " + item.Count);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Linq_Generation()
        {
            var code = """
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var range = Enumerable.Range(1, 5);
        Console.WriteLine(range.Count()); // 5
        Console.WriteLine(range.First()); // 1

        var repeat = Enumerable.Repeat("A", 3);
        Console.WriteLine(repeat.Count()); // 3
        Console.WriteLine(repeat.Last()); // A

        var empty = Enumerable.Empty<int>();
        Console.WriteLine(empty.Count()); // 0
    }
}
""";
            await RunTest(code);
        }
    }
}
