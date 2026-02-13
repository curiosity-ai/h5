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
        [Ignore("Not implemented yet")]
        public async Task FileScopedNamespace()
        {
            var code = """
using System;

namespace MyNamespace;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(typeof(Program).Namespace);
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
