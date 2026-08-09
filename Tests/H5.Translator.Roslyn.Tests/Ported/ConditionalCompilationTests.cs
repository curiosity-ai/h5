using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class ConditionalCompilationTests : TranslatorTestBase
    {
        [TestMethod]
        public async Task ComprehensiveFeatureTest()
        {
            var code = """
#define TRUE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Define conflicting types in #if FALSE to ensure they are ignored
#if FALSE
public class ConflictClass {
    This is syntax error garbage that should be ignored
}
public interface IFeature { void BadMethod(); }
#endif

// Define actual types in #if TRUE
#if TRUE
public interface IFeature { void Run(); }
public class Base { public virtual string Name => "Base"; }
public class Derived : Base { public override string Name => "Derived"; }

public record Person(string Name, int Age);

public struct Point { public int X, Y; }

public class GenericBox<T>
{
    public T Value { get; }
    public GenericBox(T value) { Value = value; }
}
#else
public class Base { public virtual string Name => "WrongBase"; }
#endif

public class Program
{
    public static async Task Main()
    {
        #if TRUE
        Console.WriteLine("Start Main");
        #else
        Console.WriteLine("Should not be executed");
        #endif

        #if FALSE
        Console.WriteLine("GARBAGE");
        int z = "string";
        #endif

        // 1. Basic Types & Control Flow
        #if TRUE
        TestBasics();
        #endif

        // 2. OOP Features
        #if TRUE
        TestOOP();
        #endif

        // 3. Generics
        #if TRUE
        TestGenerics();
        #endif

        // 4. Async/Await
        #if TRUE
        await TestAsync();
        #endif

        // 5. Collections & LINQ
        #if TRUE
        TestLinq();
        #endif

        // 6. Modern Features (Records, Pattern Matching, Tuples)
        #if TRUE
        TestModernFeatures();
        #endif

        // 7. Delegates & Lambdas
        #if TRUE
        TestDelegates();
        #endif

        // 8. Exceptions
        #if TRUE
        TestExceptions();
        #endif

        #if FALSE
        This should be a syntax error if compiled
        #endif
    }

    #if TRUE
    static void TestBasics()
    {
        #if FALSE
        garbage code
        #endif

        int x = 10;
        #if TRUE
        if (x == 10) Console.WriteLine("x is 10");
        else Console.WriteLine("x is not 10");
        #else
        Console.WriteLine("Wrong branch");
        #endif

        for (int i = 0; i < 2; i++)
        {
            #if FALSE
            syntax error
            #endif
            Console.WriteLine("For loop " + i);
        }

        int j = 0;
        while (j < 2) {
            #if TRUE
            Console.WriteLine("While loop " + j);
            j++;
            #endif
        }

        string[] arr = new[] { "A", "B" };
        foreach (var s in arr)
        {
            #if TRUE
            Console.WriteLine("Foreach " + s);
            #endif
        }
    }

    static void TestOOP()
    {
        #if FALSE
        ConflictClass c = new ConflictClass();
        #endif

        Base b = new Derived();
        Console.WriteLine("Polymorphism: " + b.Name);

        IFeature f = new FeatureImpl();
        f.Run();

        #if TRUE
        var p = new Point { X = 1, Y = 2 };
        Console.WriteLine($"Struct: {p.X},{p.Y}");
        #endif
    }

    static void TestGenerics()
    {
        #if FALSE
        GenericBox<int> b = "bad";
        #endif

        var box = new GenericBox<string>("Content");
        Console.WriteLine("Generic: " + box.Value);
    }

    static async Task TestAsync()
    {
        await Task.Delay(1);
        #if TRUE
        Console.WriteLine("Async: Done");
        #else
        Console.WriteLine("Async: Wrong");
        #endif
    }

    static void TestLinq()
    {
        #if FALSE
        var x = from y in z select w;
        #endif

        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var evenSquares = numbers.Where(n => n % 2 == 0).Select(n => n * n).ToList();
        Console.WriteLine("LINQ: " + string.Join(",", evenSquares));
    }

    static void TestModernFeatures()
    {
        // Records
        var p1 = new Person("Alice", 30);
        var p2 = p1 with { Age = 31 };
        Console.WriteLine($"Record: {p1.Name} is {p1.Age}");
        Console.WriteLine($"Record Copy: {p2.Name} is {p2.Age}");

        // Pattern Matching
        object obj = p1;
        #if TRUE
        if (obj is Person { Age: >= 30 })
        {
            Console.WriteLine($"Pattern: Matched {p1.Name}");
        }
        #endif

        var desc = p1 switch
        {
            { Age: < 20 } => "Young",
            #if FALSE
            garbage => syntax error,
            #endif
            { Age: >= 20 } => "Adult",
            _ => "Unknown"
        };
        Console.WriteLine($"Switch Expr: {desc}");

        // Tuples
        (int, string) t = (1, "One");
        Console.WriteLine($"Tuple: {t.Item1} - {t.Item2}");
        var (id, label) = t;
        Console.WriteLine($"Deconstruct: {id} - {label}");

        // Target-typed new
        Point pt = new() { X = 10, Y = 20 };
        Console.WriteLine($"Target-typed: {pt.X}");
    }

    static void TestDelegates()
    {
        Func<int, int> square = x => x * x;
        #if TRUE
        Console.WriteLine("Lambda: " + square(5));
        #endif

        Action<string> print = msg => Console.WriteLine("Action: " + msg);
        print("Hello");

        // Local function
        #if TRUE
        int LocalAdd(int a, int b) => a + b;
        Console.WriteLine("LocalFunc: " + LocalAdd(3, 4));
        #else
        int LocalAdd(int a, int b) => "error";
        #endif
    }

    static void TestExceptions()
    {
        try
        {
            #if TRUE
            throw new InvalidOperationException("Test Error");
            #endif
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Caught: " + ex.Message);
        }
        finally
        {
            #if TRUE
            Console.WriteLine("Finally executed");
            #else
            Console.WriteLine("Wrong finally");
            #endif
        }
    }
    #endif

    #if FALSE
    static void TestBasics() { This code is garbage }
    #endif
}

#if TRUE
public class FeatureImpl : IFeature
{
    public void Run() => Console.WriteLine("FeatureImpl.Run executed");
}
#endif
""";
            await RunTest(code, waitForOutput: "Finally executed", includeCorePackages: true);
        }
    }
}
