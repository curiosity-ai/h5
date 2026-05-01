using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class boxTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task box_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/int64/misc/box.cs
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

// namespace JitTest_box_misc_cs
{
    public class Test
    {
        // [Fact]
        // [OuterLoop]
        public static int TestEntryPoint()
        {
            try
            {
                ulong L1 = 0x8000123480001234;
                if (L1 != (ulong)(object)(ulong)(object)(ulong)(object)L1)
                    goto fail;
                long L2 = unchecked((long)0x8000123480001234);
                if (L2 != (long)(object)(long)(object)(long)(object)L2)
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
