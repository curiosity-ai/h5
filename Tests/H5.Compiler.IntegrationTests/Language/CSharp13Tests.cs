using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class CSharp13Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task ParamsCollections()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        Print(1, 2, 3);
        Print(new List<int> { 4, 5 });
        Print([6, 7]); // Collection expression
    }

    public static void Print(params IEnumerable<int> numbers)
    {
        Console.WriteLine(string.Join(" ", numbers));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ParamsCollections2()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        PrintList(1, 2, 3);
        PrintSpan(4, 5, 6);
    }

    static void PrintList(params List<int> list)
    {
        Console.WriteLine(list.Count);
    }

    static void PrintSpan(params ReadOnlySpan<int> span)
    {
        Console.WriteLine(span.Length);
    }
}
""";
            await RunTest(code);
        }


        [TestMethod]
        public async Task LockObject()
        {
            var code = """
using System;
using System.Threading;

public class Program
{
    private static readonly Lock _lock = new Lock();

    public static void Main()
    {
        using (_lock.EnterScope())
        {
            Console.WriteLine("Inside lock");
        }

        lock (_lock)
        {
            Console.WriteLine("Inside lock keyword");
        }
    }
}
""";
            // Lock is a new type in .NET 9.
            // H5 might support it via System.Threading.Lock.
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task EscapeSequence()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        string s = "\e"; // Escape character
        Console.WriteLine((int)s[0]); // Should be 27
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ImplicitIndexAccessInObjectInitializer()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        var numbers = new int[]
        {
            1, 2, 3, 4, 5
        };

        var wrapper = new Wrapper(numbers)
        {
            [^1] = 100, // Should update last element
            [^2] = 200
        };

        Console.WriteLine(string.Join(",", wrapper.Numbers));
    }
}

public class Wrapper
{
    public int[] Numbers { get; }
    public Wrapper(int[] numbers) => Numbers = numbers;

    public int this[Index index]
    {
        get => Numbers[index];
        set => Numbers[index] = value;
    }
}
""";
            await RunTest(code);
        }
    }
}
