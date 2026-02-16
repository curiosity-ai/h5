using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class enumTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task enum_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/Boxing/misc/enum.cs
using System;

public static class TestFramework
{
    public static void BeginTestCase(string message) { Console.WriteLine("BeginTestCase: " + message); }
    public static void EndTestCase() { Console.WriteLine("EndTestCase"); }
    public static void LogInformation(string message) { Console.WriteLine(message); }
    public static void LogError(string id, string message) { Console.WriteLine("Error " + id + ": " + message); }
    public static void BeginScenario(string message) { Console.WriteLine("BeginScenario: " + message); }
}


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
// using Xunit;

// namespace BoxTest_enum_cs
{
    internal enum ToPrintOrNotToPrint
    {
        Print,
        DoNotPrint
    }

    public class Test
    {
        protected object Fibonacci(object num, object flag)
        {
            if ((ToPrintOrNotToPrint)flag == ToPrintOrNotToPrint.DoNotPrint)
                return Fibonacci2(num, flag);
            if (((int)num % 2) == 0)
                return Fibonacci2(num, flag);
            return Fibonacci2(num, flag);
        }

        protected object Fibonacci2(object num, object flag)
        {
            int N;
            if ((int)num <= 1)
                N = (int)num;
            else
                N = (int)Fibonacci((int)num - 2,
                        ToPrintOrNotToPrint.DoNotPrint) + (int)Fibonacci((int)num - 1, flag);
            if ((ToPrintOrNotToPrint)flag == ToPrintOrNotToPrint.Print)
                Console.Write(N.ToString() + " ");
            return N;
        }

        // [Fact]
        // [OuterLoop]
        public static void TestEntryPoint()
        {
            new Test().Fibonacci(20, ToPrintOrNotToPrint.Print);
            Console.WriteLine();
            Console.WriteLine("*** PASSED ***");
        }
    }
// }


public class Program
{
    public static int Main()
    {
        try {
            return Test.TestEntryPoint();
        } catch(Exception e) {
            Console.WriteLine(e.ToString());
            return 0;
        }
        return 100;
    }
}
""";
            await RunTest(code);
        }
    }
}
