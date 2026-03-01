using System;
using Xunit;
using Xunit.Abstractions;
using H5.Compiler.Service.Next;

namespace H5.Compiler.Next.Tests
{
    public class HelloWorldTest
    {
        private readonly ITestOutputHelper _output;

        public HelloWorldTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CompilesHelloWorld()
        {
            var source = @"
using System;

namespace MyNamespace
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}";

            var compiler = new H5Compiler();
            var result = compiler.Compile(source);

            _output.WriteLine("RESULT:\n" + result);

            Assert.Contains("H5.define('MyNamespace.Program'", result);
            Assert.Contains("Main: function () {", result);
            Assert.Contains("console.log('Hello, World!');", result);
        }
    }
}
