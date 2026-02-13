using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp10Tests : IntegrationTestBase
    {
        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task RecordStructs()
        {
            var code = """
using System;

public record struct Point(int X, int Y);

public class Program
{
    public static void Main()
    {
        var p = new Point(1, 2);
        var p2 = p with { X = 3 };
        Console.WriteLine(p);
        Console.WriteLine(p2);
        Console.WriteLine(p.X);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task StructParameterlessConstructor()
        {
            var code = """
using System;

public struct S
{
    public int X;
    public S() { X = 10; }
}

public class Program
{
    public static void Main()
    {
        S s = new S();
        Console.WriteLine(s.X);

        S[] arr = new S[1]; // Should be default(S) which is zeroed, constructor NOT called for arrays
        Console.WriteLine(arr[0].X);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        // [Ignore("Not implemented yet")]
        public async Task FileScopedNamespace()
        {
            // RoslynCompiler (CSharpScript) does not support file-scoped namespaces in scripts.
            // But H5 compiler should support it if we compile to JS.
            // We need to bypass Roslyn checks if possible or accept that Roslyn test runner might fail.
            // However, RunTest runs both.
            // Let's modify the test to only run H5 compilation if Roslyn fails due to script limitations?
            // Or better, just test the H5 output.

            // For now, let's keep it but mark it as H5 only test if we had that capability.
            // Wait, we can wrap the code in a standard namespace for Roslyn if we want to share logic? No, the point is to test syntax.

            // The error "Cannot declare namespace in script code" confirms Roslyn limitation.
            // We can't change RoslynCompiler easily.
            // We should expect this test to fail on Roslyn side but pass on H5.

            // But RunTest assertions check output match.
            // Let's try to verify if H5 compiles it.

            var code = """
using System;

namespace MyNamespace;

public class Program
{
    public static void Main()
    {
        System.Console.WriteLine(typeof(Program).Namespace);
    }
}
""";
             await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
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
            Console.WriteLine("Lives in Seattle");
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
    const string Message = $"{Greeting}, World!";

    public static void Main()
    {
        Console.WriteLine(Message);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task LambdaImprovements()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        // Explicit return type
        var f = int (bool b) => b ? 1 : 0;
        Console.WriteLine(f(true));

        // Attributes on lambda (requires defining an attribute)
        // var g = [Obsolete] (int x) => x;
        // Attributes on lambdas are tricky to test execution-wise, mostly compilation check.
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task SealedToStringInRecords()
        {
            var code = """
using System;

public record Person(string Name)
{
    public sealed override string ToString() => Name;
}

public record Student(string Name, int Grade) : Person(Name);

public class Program
{
    public static void Main()
    {
        var s = new Student("Alice", 10);
        Console.WriteLine(s.ToString()); // Should print "Alice", not include Grade
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task MixedDeconstruction()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        int x;
        (x, var y) = (1, 2);
        Console.WriteLine(x + y);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task CallerArgumentExpression()
        {
            var code = """
using System;
using System.Runtime.CompilerServices;

public class Program
{
    public static void Main()
    {
        Validate(1 > 0);
    }

    static void Validate(bool condition, [CallerArgumentExpression("condition")] string message = null)
    {
        Console.WriteLine($"Expression: {message}");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task GlobalUsings()
        {
            var code = """
global using System;
global using static System.Math;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(Abs(-10));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        [Ignore("Not implemented yet")]
        public async Task AsyncMethodBuilderAttribute()
        {
            var code = """
using System;
using System.Runtime.CompilerServices;

[AsyncMethodBuilder(typeof(MyAsyncMethodBuilder))]
public class MyTask
{
    public MyAsyncMethodBuilder GetAwaiter() => new MyAsyncMethodBuilder();
}

public class MyAsyncMethodBuilder
{
    public static MyAsyncMethodBuilder Create() => new MyAsyncMethodBuilder();
    public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();
    public void SetStateMachine(IAsyncStateMachine stateMachine) { }
    public void SetResult() { }
    public void SetException(Exception exception) { }
    public MyTask Task => new MyTask();
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine { }
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine { }
    public bool IsCompleted => true;
    public void GetResult() { }
}


public class Program
{
    public static async MyTask Method()
    {
        await System.Threading.Tasks.Task.Delay(1);
    }

    public static void Main()
    {
        Method();
        Console.WriteLine("Custom Builder");
    }
}
""";
            await RunTest(code);
        }
    }
}
