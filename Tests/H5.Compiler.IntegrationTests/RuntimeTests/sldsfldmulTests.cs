using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class sldsfldmulTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task sldsfldmul_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/int64/signed/s_ldsfld_mul.cs
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

// namespace JitTest_s_ldsfld_mul_signed_cs
{
    public class Test
    {
        private static long s_op1,s_op2;
        private static bool check(long product, bool overflow)
        {
            Console.Write("Multiplying {0} and {1}...", s_op1, s_op2);
            try
            {
                if (unchecked(s_op1 * s_op2) != product)
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
            s_op1 = 0x000000007fffffff;
            s_op2 = 0x000000007fffffff;
            if (!check(0x3fffffff00000001, false))
                goto fail;
            s_op1 = 0x0000000100000000;
            s_op2 = 0x000000007fffffff;
            if (!check(0x7fffffff00000000, false))
                goto fail;
            s_op1 = 0x0000000100000000;
            s_op2 = 0x0000000100000000;
            if (!check(0x0000000000000000, false))
                goto fail;
            s_op1 = 0x3fffffffffffffff;
            s_op2 = 0x0000000000000002;
            if (!check(0x7ffffffffffffffe, false))
                goto fail;
            s_op1 = unchecked((long)0xffffffffffffffff);
            s_op2 = unchecked((long)0xfffffffffffffffe);
            if (!check(2, false))
                goto fail;
            s_op1 = 0x0000000000100000;
            s_op2 = 0x0000001000000000;
            if (!check(0x0100000000000000, false))
                goto fail;
            s_op1 = unchecked((long)0xffffffffffffffff);
            s_op2 = unchecked((long)0x8000000000000001);
            if (!check(0x7fffffffffffffff, false))
                goto fail;
            s_op1 = unchecked((long)0xfffffffffffffffe);
            s_op2 = unchecked((long)0x8000000000000001);
            if (!check(unchecked((long)0xfffffffffffffffe), false))
                goto fail;
            s_op1 = 2;
            s_op2 = unchecked((long)0x8000000000000001);
            if (!check(2, false))
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
