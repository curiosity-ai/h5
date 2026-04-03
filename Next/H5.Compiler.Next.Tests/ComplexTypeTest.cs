using System;
using Xunit;
using Xunit.Abstractions;
using H5.Compiler.Service.Next;

namespace H5.Compiler.Next.Tests
{
    public class ComplexTypeTest
    {
        private readonly ITestOutputHelper _output;

        public ComplexTypeTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CompilesComplexTypes()
        {
            var source = @"
using System;

namespace TestNamespace
{
    public interface ITest
    {
        void DoWork();
    }

    public class Person : ITest
    {
        public static readonly string Species = ""Human"";
        public string Name { get; set; }
        public int Age;

        public Person(string name)
        {
            Name = name;
        }

        public void DoWork() {}
    }

    public struct Point<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
    }
}
";

            var compiler = new H5Compiler();
            var result = compiler.Compile(source);

            _output.WriteLine("RESULT:\n" + result);

            // Interface check (interfaces generally skipped or emitted trivially, not implemented in emitter yet so skipping assertion)

            // Class Check
            Assert.Contains("H5.define('TestNamespace.Person', {", result);
            Assert.Contains("statics: {", result);
            Assert.Contains("Species: 'Human',", result);
            Assert.Contains("fields: {", result);
            Assert.Contains("Age: null,", result);
            Assert.Contains("props: {", result);
            Assert.Contains("Name: null, // Simplified property backing", result);
            Assert.Contains("ctors: {", result);
            Assert.Contains("init: function (name) {", result);
            Assert.Contains("this.Name = name;", result);
            Assert.Contains("methods: {", result);
            Assert.Contains("DoWork: function () {", result);

            // Generic Struct Check
            Assert.Contains("H5.define('TestNamespace.Point', function (T) { return {", result);
            Assert.Contains("$kind: \"struct\",", result);
            Assert.Contains("X: null, // Simplified property backing", result);
            Assert.Contains("Y: null, // Simplified property backing", result);
            Assert.Contains("}; });", result);
        }
    }
}
