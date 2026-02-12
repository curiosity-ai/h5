using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class ControlFlowTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task Conditionals()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        int x = 10;

        if (x > 5)
            Console.WriteLine("x > 5");

        if (x < 5)
            Console.WriteLine("x < 5");
        else
            Console.WriteLine("x >= 5");

        if (x == 1)
            Console.WriteLine("x is 1");
        else if (x == 10)
            Console.WriteLine("x is 10");
        else
            Console.WriteLine("x is unknown");

        if (x > 0)
        {
            if (x % 2 == 0)
                Console.WriteLine("Positive Even");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task SwitchStatement()
        {
            var code = """
using System;

public enum Mode { A, B, C }

public class Program
{
    public static void Main()
    {
        // Switch on Integer
        int i = 2;
        switch (i)
        {
            case 1: Console.WriteLine("One"); break;
            case 2: Console.WriteLine("Two"); break;
            default: Console.WriteLine("Other"); break;
        }

        // Switch on String
        string s = "Hello";
        switch (s)
        {
            case "Hi": Console.WriteLine("Hi"); break;
            case "Hello": Console.WriteLine("Greeting"); break;
            default: Console.WriteLine("Unknown"); break;
        }

        // Switch on Enum
        Mode m = Mode.B;
        switch (m)
        {
            case Mode.A: Console.WriteLine("Mode A"); break;
            case Mode.B: Console.WriteLine("Mode B"); break;
            case Mode.C: Console.WriteLine("Mode C"); break;
        }

        // Switch Fallthrough (Empty case)
        int k = 1;
        switch (k)
        {
            case 1:
            case 2:
                Console.WriteLine("1 or 2");
                break;
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Loops()
        {
            var code = """
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        // For
        for (int i = 0; i < 3; i++)
            Console.WriteLine("For: " + i);

        // While
        int w = 0;
        while (w < 3)
        {
            Console.WriteLine("While: " + w);
            w++;
        }

        // Do-While
        int d = 0;
        do
        {
            Console.WriteLine("DoWhile: " + d);
            d++;
        } while (d < 3);

        // Foreach Array
        int[] arr = { 10, 20 };
        foreach (var item in arr)
            Console.WriteLine("Array: " + item);

        // Foreach List
        List<string> list = new List<string> { "A", "B" };
        foreach (var item in list)
            Console.WriteLine("List: " + item);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task JumpStatements()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        // Break
        for (int i = 0; i < 10; i++)
        {
            if (i == 3) break;
            Console.WriteLine("Break: " + i);
        }

        // Continue
        for (int i = 0; i < 5; i++)
        {
            if (i % 2 == 0) continue;
            Console.WriteLine("Continue: " + i);
        }

        // Return
        ReturnTest();

        // Goto
        int counter = 0;
    Start:
        Console.WriteLine("Goto: " + counter);
        counter++;
        if (counter < 3) goto Start;
    }

    static void ReturnTest()
    {
        Console.WriteLine("Before Return");
        return;
        Console.WriteLine("After Return"); // Unreachable but valid syntax in some contexts or warnings
    }
}
""";
            await RunTest(code);
        }
    }
}
