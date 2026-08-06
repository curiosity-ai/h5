using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class fiboTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task fibo_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/Boxing/functional/fibo.cs
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


// namespace BoxTest_fibo_cs
{
    public class Test
    {
        protected object Fibonacci(object num, object flag)
        {
            if (num.GetType() != typeof(int) ||
                flag.GetType() != typeof(bool))
                throw new Exception();

            return Fibonacci2(num, flag);
        }

        protected object Fibonacci2(object num, object flag)
        {
            object N;
            if ((int)num <= 1)
                N = num;
            else
                N = (int)Fibonacci((int)num - 2, false) + (int)Fibonacci((int)num - 1, flag);
            if ((bool)flag)
            {
                if (((int)num % 2) == 0)
                    Console.Write(N.ToString() + " ");
                else
                    Console.Write(N.ToString() + " ");
            }
            return N;
        }

        // [Fact]
        // [OuterLoop]
        public static void TestEntryPoint()
        {
            new Test().Fibonacci(20, true);
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
