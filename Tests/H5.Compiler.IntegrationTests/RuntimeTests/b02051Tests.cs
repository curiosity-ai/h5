using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b02051Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b02051_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b02051/b02051.cs
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
//

// using Xunit;
// namespace b02051
{
    using System;
    public class JITcrash
    {
        // [OuterLoop]
        // [Fact]
        public static void TestEntryPoint()
        {
            UInt32 x = (0xFFFFFFFF);
            Int64 y = x;

            //	just added few cases of WriteLine
            Console.WriteLine("Running test");
            Console.WriteLine("x = " + x);
            Console.WriteLine("x = " + x + ".");
            Console.WriteLine("x = " + x + " y = " + y + ".");
            Console.WriteLine("Test passed.");
        }
    }
// }


public class Program
{
    public static int Main()
    {
        try {
            return JITcrash.TestEntryPoint();
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
