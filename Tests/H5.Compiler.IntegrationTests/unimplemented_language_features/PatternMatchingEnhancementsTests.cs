using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.PatternMatching
{
    [TestClass]
    public class PatternMatchingEnhancementsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task TestPatternMatchingEnhancements()
        {
            var csharpCode = @"
using System;

public class Program
{
    public static void Main()
    {
        object x = 5;

        // Not Pattern
        if (x is not null)
        {
            Console.WriteLine(""Not null"");
        }

        // Type Pattern
        if (x is int)
        {
             Console.WriteLine(""Is int"");
        }

        // Relational Pattern
        if (x is > 0)
        {
            Console.WriteLine(""Greater than 0"");
        }

        // Conjunctive Pattern (and)
        if (x is int and > 0)
        {
            Console.WriteLine(""Int and > 0"");
        }

        // Disjunctive Pattern (or)
        if (x is < 0 or > 2)
        {
             Console.WriteLine(""Less than 0 or greater than 2"");
        }

        // Parenthesized Pattern
        if (x is (int))
        {
             Console.WriteLine(""Parenthesized int"");
        }
    }
}";
            await RunTest(csharpCode);
        }
    }
}
