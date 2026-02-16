using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class CSharp14Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task ExtensionMembers()
        {
            var code = """
using System;
using System.Collections.Generic;

public static class EnumerableExtensions
{
    // C# 14 syntax for extension members
    extension<TSource>(IEnumerable<TSource> source)
    {
        public bool IsEmpty => !System.Linq.Enumerable.Any(source);
    }
}

public class Program
{
    public static void Main()
    {
        var list = new List<int>();
        Console.WriteLine(list.IsEmpty); // Property access on extension
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task FieldKeyword()
        {
            var code = """
using System;

public class Person
{
    public string Name
    {
        get;
        set => field = value.Trim(); // 'field' keyword
    }
}

public class Program
{
    public static void Main()
    {
        var p = new Person();
        p.Name = "  Alice  ";
        Console.WriteLine($"'{p.Name}'");
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task UnboundGenericNameof()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(nameof(List<>)); // C# 14
        Console.WriteLine(nameof(Dictionary<,>));
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task SimpleLambdaModifiers()
        {
            var code = """
using System;

public class Program
{
    public delegate void RefAction(ref int x);

    public static void Main()
    {
        RefAction action = (ref x) => x++; // 'ref' without type
        int val = 10;
        action(ref val);
        Console.WriteLine(val);
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task PartialProperties()
        {
            var code = """
using System;

public partial class Person
{
    public partial string Name { get; set; }
}

public partial class Person
{
    private string _name;
    public partial string Name
    {
        get => _name;
        set => _name = value;
    }
}

public class Program
{
    public static void Main()
    {
        var p = new Person();
        p.Name = "Bob";
        Console.WriteLine(p.Name);
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task NullConditionalAssignment()
        {
            var code = """
using System;

public class Person
{
    public string Name { get; set; }
}

public class Program
{
    public static void Main()
    {
        Person p = null;
        p?.Name = "Alice"; // Should do nothing

        p = new Person();
        p?.Name = "Bob"; // Should set name
        Console.WriteLine(p.Name);
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }
    }
}
