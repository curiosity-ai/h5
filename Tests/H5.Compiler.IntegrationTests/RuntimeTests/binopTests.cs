using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class binopTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task binop_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/int64/misc/binop.cs
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

// namespace JitTest_binop_misc_cs
{
    public class Test
    {
        // [Fact]
        // [OuterLoop]
        public static int TestEntryPoint()
        {
            long L1, L2;
            ulong U1, U2;
            try
            {
                L1 = 0x7000123480001234;
                L2 = 0x7123400081234000;
                if ((L1 & L2) != 0x7000000080000000)
                    goto fail;

                L1 = 0x7000123480001234;
                L2 = 0x7123400081234000;
                if ((L1 | L2) != 0x7123523481235234)
                    goto fail;

                U1 = 0x8000123480001234;
                U2 = 0x8123400081234000;
                if (~(U1 & U2) != 0x7fffffff7fffffff)
                    goto fail;

                U1 = 0x8000123480001234;
                U2 = 0x8123400081234000;
                if ((U1 | U2) != 0x8123523481235234)
                    goto fail;
            }
            catch (Exception)
            {
                Console.WriteLine("Exception handled!");
                goto fail;
            }
            Console.WriteLine("Passed");
            return 100;
        fail:
            Console.WriteLine("Failed");
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
