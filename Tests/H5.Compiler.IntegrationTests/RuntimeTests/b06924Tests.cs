using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b06924Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b06924_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b06924/b06924.cs
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
// namespace AAAA
{
    //@BEGINRENAME; Verify this renames
    //@ENDRENAME; Verify this renames
    using System;
    public class CtTest
    {
        private static int iTest = 5;
        // [OuterLoop]
        // [Fact]
        public static void TestEntryPoint()
        {
            iTest++;
            Console.WriteLine("iTest is " + iTest);
        }
    }
// }


public class Program
{
    public static int Main()
    {
        try {
            return CtTest.TestEntryPoint();
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
