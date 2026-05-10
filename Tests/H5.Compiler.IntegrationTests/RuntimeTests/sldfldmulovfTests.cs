using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class sldfldmulovfTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task sldfldmulovf_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/int64/signed/s_ldfld_mulovf.cs
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

// namespace JitTest_s_ldfld_mulovf_signed_cs
{
    public class Test
    {
        private long _op1,_op2;
        private bool check(long product, bool overflow)
        {
            Console.Write("Multiplying {0} and {1}...", _op1, _op2);
            try
            {
                if (checked(_op1 * _op2) != product)
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
        public static int TestEntryPoint()
        {
            Test app = new Test();
            app._op1 = 0x000000007fffffff;
            app._op2 = 0x000000007fffffff;
            if (!app.check(0x3fffffff00000001, false))
                goto fail;
            app._op1 = 0x0000000100000000;
            app._op2 = 0x000000007fffffff;
            if (!app.check(0x7fffffff00000000, false))
                goto fail;
            app._op1 = 0x0000000100000000;
            app._op2 = 0x0000000100000000;
            if (!app.check(0x0000000000000000, true))
                goto fail;
            app._op1 = 0x3fffffffffffffff;
            app._op2 = 0x0000000000000002;
            if (!app.check(0x7ffffffffffffffe, false))
                goto fail;
            app._op1 = unchecked((long)0xffffffffffffffff);
            app._op2 = unchecked((long)0xfffffffffffffffe);
            if (!app.check(2, false))
                goto fail;
            app._op1 = 0x0000000000100000;
            app._op2 = 0x0000001000000000;
            if (!app.check(0x0100000000000000, false))
                goto fail;
            app._op1 = unchecked((long)0xffffffffffffffff);
            app._op2 = unchecked((long)0x8000000000000001);
            if (!app.check(0x7fffffffffffffff, false))
                goto fail;
            app._op1 = unchecked((long)0xfffffffffffffffe);
            app._op2 = unchecked((long)0x8000000000000001);
            if (!app.check(0, true))
                goto fail;
            app._op1 = 2;
            app._op2 = unchecked((long)0x8000000000000001);
            if (!app.check(0, true))
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
