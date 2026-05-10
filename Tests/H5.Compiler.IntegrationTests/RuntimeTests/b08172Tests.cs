using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b08172Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b08172_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b08172/b08172.cs
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

// Factorial


namespace b08172;

using System;
using System.Runtime.CompilerServices;
// using Xunit;

public class Test
{
    // [OuterLoop]
    // [Fact]
    public static void TestEntryPoint() {
        Test app = new Test();
        app.Run(17);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public int Run(long i)
    {
        Console.Out.WriteLine("Factorial of " + i.ToString() + " is " + Fact(i).ToString());
        return (0);
    }

    private long Fact(long i)
    {
        if (i <= 1L)
            return (i);
        return (i * Fact(i - 1L));
    }
}


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
