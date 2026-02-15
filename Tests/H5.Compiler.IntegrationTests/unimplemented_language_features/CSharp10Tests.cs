using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests.UnimplementedLanguageFeatures
{
    [TestClass]
    public class CSharp10Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task RecordStructs()
        {
            var code = """
using System;
namespace System.Runtime.CompilerServices { internal static class IsExternalInit {} }

public record struct Point(int X, int Y);

public class Program
{
    public static void Main()
    {
        var p = new Point(1, 2);
        // var p2 = p with { X = 3 }; // With expression on structs might be tricky without full support
        Console.WriteLine(p.X);
        Console.WriteLine(p.Y);
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        [Ignore("Struct parameterless constructor not supported in NRefactory/Emitter")]
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
        [Ignore("Rewriting failed or NRefactory parse error")]
        public async Task FileScopedNamespace()
        {
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
        [Ignore("Explicit return type on lambda not rewritten")]
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
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        [Ignore("Requires System.Type.op_Equality which is missing in test environment")]
        public async Task SealedToStringInRecords()
        {
            var code = """
using System;
namespace System.Runtime.CompilerServices { internal static class IsExternalInit {} }

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
            await RunTest(code, skipRoslyn: true);
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
        public async Task CallerArgumentExpression()
        {
            var code = """
using System;
using System.Runtime.CompilerServices;

namespace System.Runtime.CompilerServices {
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class CallerArgumentExpressionAttribute : Attribute {
        public CallerArgumentExpressionAttribute(string parameterName) {}
    }
}

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
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
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
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task AsyncMethodBuilderAttribute()
        {
            var code = """
using System;
using System.Runtime.CompilerServices;

namespace System.Runtime.CompilerServices {
    public sealed class AsyncMethodBuilderAttribute : Attribute
    {
        public AsyncMethodBuilderAttribute(Type builderType) {}
    }
}

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
            await RunTestExpectingError(code, "AsyncMethodBuilderAttribute is not supported");
        }
    }
}
