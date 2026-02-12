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
    }
}
