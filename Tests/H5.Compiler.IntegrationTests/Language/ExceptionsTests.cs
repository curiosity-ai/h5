using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class ExceptionsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task TryCatch()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        Console.WriteLine("Start");
        try
        {
            Console.WriteLine("Inside Try");
            throw new Exception("Test Exception");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Caught: " + ex.Message);
        }
        Console.WriteLine("End");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task TryCatchFinally()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        Console.WriteLine("Start");
        try
        {
            Console.WriteLine("Inside Try");
            throw new Exception("Error");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Caught: " + ex.Message);
        }
        finally
        {
            Console.WriteLine("Finally Block");
        }
        Console.WriteLine("End");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task CatchSpecificException()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        try
        {
            throw new InvalidOperationException("Invalid Op");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Caught ArgumentException");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Caught InvalidOperationException: " + ex.Message);
        }
        catch (Exception)
        {
            Console.WriteLine("Caught Generic Exception");
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Throw()
        {
             var code = """
using System;
public class Program
{
    public static void Main()
    {
        try
        {
            ThrowMethod();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void ThrowMethod()
    {
        throw new Exception("Throw Method");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Rethrow()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        try
        {
            Method1();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Main Caught: " + ex.Message);
        }
    }

    static void Method1()
    {
        try
        {
            throw new Exception("Original Error");
        }
        catch (Exception)
        {
            Console.WriteLine("Method1 Caught and Rethrowing");
            throw;
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task CustomException()
        {
            var code = """
using System;

public class MyCustomException : Exception
{
    public MyCustomException(string message) : base(message) { }
}

public class Program
{
    public static void Main()
    {
        try
        {
            throw new MyCustomException("My Error");
        }
        catch (MyCustomException ex)
        {
            Console.WriteLine("Caught Custom: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Caught Generic: " + ex.Message);
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ExceptionProperties()
        {
            var code = """
using System;

public class Program
{
    public static void Main()
    {
        try
        {
            try
            {
                throw new InvalidOperationException("Inner Error");
            }
            catch (Exception inner)
            {
                throw new Exception("Outer Error", inner);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Message: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner Message: " + ex.InnerException.Message);
            }

            // Checking StackTrace presence.
            // Note: Content will differ between Roslyn and H5, so we only check existence.
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                Console.WriteLine("Has StackTrace");
            }
            else
            {
                Console.WriteLine("No StackTrace");
            }
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task AsyncTryCatchFinally()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        try
        {
            await ThrowAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Caught: " + ex.Message);
        }
        finally
        {
            Console.WriteLine("Finally Block");
        }
        Console.WriteLine("End");
        Console.WriteLine("<<DONE>>");
    }

    public static async Task ThrowAsync()
    {
        await Task.Delay(10);
        throw new Exception("Async Error");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncNestedTryCatch()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        try
        {
            try
            {
                await ThrowAsync("Inner Error");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Inner Caught: " + ex.Message);
                throw new Exception("Outer Error", ex);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Outer Caught: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
            }
        }
        Console.WriteLine("End");
        Console.WriteLine("<<DONE>>");
    }

    public static async Task ThrowAsync(string msg)
    {
        await Task.Delay(10);
        throw new InvalidOperationException(msg);
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncLambdaException()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Func<Task> asyncLambda = async () =>
        {
            await Task.Delay(10);
            throw new Exception("Lambda Error");
        };

        try
        {
            await asyncLambda();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Caught: " + ex.Message);
        }
        Console.WriteLine("<<DONE>>");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task AsyncLocalFunctionException()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        async Task LocalAsync()
        {
            await Task.Delay(10);
            throw new Exception("Local Function Error");
        }

        try
        {
            await LocalAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Caught: " + ex.Message);
        }
        Console.WriteLine("<<DONE>>");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task TaskWhenAllException()
        {
            var code = """
using System;
using System.Threading.Tasks;
using System.Linq;

public class Program
{
    public static async Task Main()
    {
        var t1 = Task.Delay(50).ContinueWith(_ => Console.WriteLine("T1 Done"));
        var t2 = Task.Run(async () => {
            await Task.Delay(10);
            throw new Exception("T2 Error");
        });

        try
        {
            await Task.WhenAll(t1, t2);
        }
        catch (Exception ex)
        {
            // Note: When awaiting WhenAll, only the first exception is thrown directly,
            // unless we inspect the Task itself or catch AggregateException explicitly if not awaited directly?
            // Actually 'await' unwraps AggregateException and throws the first one.
            Console.WriteLine("Caught: " + ex.Message);
        }
        Console.WriteLine("<<DONE>>");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task TaskWhenAnyException()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        var t1 = Task.Delay(500);
        var t2 = Task.Run(async () => {
            await Task.Delay(10);
            throw new Exception("T2 Error");
        });

        Task finishedTask = await Task.WhenAny(t1, t2);

        if (finishedTask.IsFaulted)
        {
             Console.WriteLine("Task Faulted: " + finishedTask.Exception.InnerException.Message);
        }
        else
        {
             Console.WriteLine("Task Completed Successfully");
        }
        Console.WriteLine("<<DONE>>");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task RethrowInAsync()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        try
        {
            await Method1();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Main Caught: " + ex.Message);
        }
        Console.WriteLine("<<DONE>>");
    }

    static async Task Method1()
    {
        try
        {
            await Task.Delay(10);
            throw new Exception("Original Error");
        }
        catch (Exception)
        {
            Console.WriteLine("Method1 Caught and Rethrowing");
            throw;
        }
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task ComplexAsyncExceptionHandling()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start Complex");
        try
        {
            await Step1();
        }
        catch (Exception ex)
        {
             Console.WriteLine("Main Caught: " + ex.Message);
        }
        Console.WriteLine("End Complex");
        Console.WriteLine("<<DONE>>");
    }

    static async Task Step1()
    {
        Console.WriteLine("Step1 Start");
        try
        {
            await Step2();
        }
        finally
        {
            Console.WriteLine("Step1 Finally");
        }
        Console.WriteLine("Step1 End"); // Should not be reached
    }

    static async Task Step2()
    {
         Console.WriteLine("Step2 Start");

         var t1 = SuccessTask();
         var t2 = FailTask();

         try
         {
             await Task.WhenAll(t1, t2);
         }
         catch (Exception ex)
         {
             Console.WriteLine("Step2 Caught: " + ex.Message);
             throw new Exception("Step2 Rethrow", ex);
         }
    }

    static async Task SuccessTask()
    {
        await Task.Delay(10);
        Console.WriteLine("SuccessTask Done");
    }

    static async Task FailTask()
    {
        await Task.Delay(20);
        throw new InvalidOperationException("FailTask Error");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }
    }
}
