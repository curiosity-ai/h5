using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b09246Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b09246_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b09246/b09246.cs
using System;

public static class TestFramework
{
    public static void BeginTestCase(string message) { Console.WriteLine("BeginTestCase: " + message); }
    public static void EndTestCase() { Console.WriteLine("EndTestCase"); }
    public static void LogInformation(string message) { Console.WriteLine(message); }
    public static void LogError(string id, string message) { Console.WriteLine("Error " + id + ": " + message); }
    public static void BeginScenario(string message) { Console.WriteLine("BeginScenario: " + message); }
}


// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace b09246;

using System;
// using Xunit;
public class MyClass
{
    //extern modifier
    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        bool b = true;
        int exitcode = 0;
        b &= false;
        Console.WriteLine(b);
        b = b & false;
        Console.WriteLine(b);
        exitcode = b ? 1 : 100;
        b = false;
        Console.WriteLine(b);
        if (exitcode == 100)
            Console.WriteLine("Test passed.");
        else
            Console.WriteLine("Test failed.");
        return (exitcode);
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return MyClass.TestEntryPoint();
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
