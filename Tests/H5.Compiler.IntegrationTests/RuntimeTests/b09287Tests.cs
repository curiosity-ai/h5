using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b09287Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b09287_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b09287/b09287.cs
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
// namespace Default
{
    //@BEGINRENAME; Verify this renames
    //@ENDRENAME; Verify this renames
    using System;
    //
    // X class
    //
    public class X
    {
        // [OuterLoop]
        // [Fact]
        public static void TestEntryPoint()
        {

            Console.WriteLine("Entering Hello world");


            Console.WriteLine("Done");

        } // main

    } // Spin

// }


public class Program
{
    public static int Main()
    {
        try {
            return X.TestEntryPoint();
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
