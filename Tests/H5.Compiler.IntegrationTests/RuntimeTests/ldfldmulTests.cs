using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class ldfldmulTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task ldfldmul_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/int64/unsigned/ldfld_mul.cs
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

// namespace JitTest_ldfld_mul_unsigned_cs
{
    public class Test
    {
        private ulong _op1,_op2;

        private bool check(ulong product, bool overflow)
        {
            Console.Write("Multiplying {0} and {1}...", _op1, _op2);
            try
            {
                if (unchecked(_op1 * _op2) != product)
                    return false;
                Console.WriteLine();
                return !overflow;
            }
            catch (OverflowException)
            {
                Console.WriteLine("overflow.");
                return overflow;
            }
        }

        // [Fact]
        // [OuterLoop]
        public static int TestEntryPoint()
        {
            Test app = new Test();
            app._op1 = 0x00000000ffffffff;
            app._op2 = 0x00000000ffffffff;
            if (!app.check(0xfffffffe00000001, false))
                goto fail;
            app._op1 = 0x0000000100000000;
            app._op2 = 0x00000000ffffffff;
            if (!app.check(0xffffffff00000000, false))
                goto fail;
            app._op1 = 0x0000000100000000;
            app._op2 = 0x0000000100000000;
            if (!app.check(0x0000000000000000, false))
                goto fail;
            app._op1 = 0x7fffffffffffffff;
            app._op2 = 0x0000000000000002;
            if (!app.check(0xfffffffffffffffe, false))
                goto fail;
            app._op1 = 0x8000000000000000;
            app._op2 = 0x0000000000000002;
            if (!app.check(0x0000000000000000, false))
                goto fail;
            app._op1 = 0x0000000000100000;
            app._op2 = 0x0000001000000000;
            if (!app.check(0x0100000000000000, false))
                goto fail;

            Console.WriteLine("Test passed");
            return 100;
        fail:
            Console.WriteLine("Test failed");
            return 1;
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
