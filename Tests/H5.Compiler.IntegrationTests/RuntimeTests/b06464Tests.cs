using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b06464Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b06464_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b06464/b06464.cs
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


namespace b06464;

using System;
// using Xunit;

public class Test_b06464
{
    static int[] a = new int[10];

    static int[] A()
    {
        Console.WriteLine("A");
        return a;
    }

    static int F()
    {
        Console.WriteLine("F");
        return 1;
    }

    static int G()
    {
        Console.WriteLine("G");
        return 1;
    }

    // [OuterLoop]
    // [Fact]
    public static void TestEntryPoint()
    {
        A()[F()] = G();
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return Test_b06464.TestEntryPoint();
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
