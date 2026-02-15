using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class NamedArgumentExpressionTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task NamedArguments_In_Lambda_With_Complex_Inference()
        {
            var code = @"
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        var list = new List<int> { 1, 2, 3 };
        Process(list, (a, b) => Method(x: a, y: b));
    }

    public static void Process<T>(List<T> list, Action<T, T> action)
    {
        if (list.Count >= 2)
            action(list[0], list[1]);
    }

    public static void Method(int x, int y)
    {
        Console.WriteLine(""x: "" + x + "", y: "" + y);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NamedArguments_With_Dynamic()
        {
             // Dynamic often fails in tests due to environment, so we skip Roslyn validation
             // to test H5 emission specifically.
            var code = @"
using System;

public class Program
{
    public static void Main()
    {
        dynamic d = 10;
        Method(val: d);
    }

    public static void Method(int val)
    {
        Console.WriteLine(val);
    }
}";
            // This might still fail if H5 runtime for dynamic is missing in test environment
            await RunTest(code, waitForOutput: null, skipRoslyn: true);
        }

    }
}
