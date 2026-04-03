using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b449827Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b449827_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V2.0-Beta2/b449827/b449827.cs
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


namespace b449827;

using System;
// using Xunit;

public class MainApp
{
    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        int a, prev;

        Console.WriteLine("\n========== Case 1 (wrong result) ==========");

        a = 2044617152;
        Console.WriteLine("a1={0}", a);

        a += 0x12345678;

        if (a < 0) a = -a;
        Console.WriteLine("a2={0}", a);

        prev = a;
        Console.WriteLine("prev={0}, a2={1}", prev, a);

        Console.WriteLine("\n========== Case 2 (right result) ==========");

        a = 2044617152;
        Console.WriteLine("a1={0}", a);

        a += 0x12345678;
        a.ToString();

        if (a < 0) a = -a;
        Console.WriteLine("a2={0}", a);

        Console.WriteLine("prev={0}, a3={1}", prev, a);

        Console.WriteLine("\n========== Test Summary ==========");
        if (a == prev)
        {
            Console.WriteLine("Test SUCCESS");
            return 100;
        }
        else
        {
            Console.WriteLine("Test FAILED");
            return 101;
        }
    }
}



public class Program
{
    public static int Main()
    {
        try {
            return MainApp.TestEntryPoint();
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
