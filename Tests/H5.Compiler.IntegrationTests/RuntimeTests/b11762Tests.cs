using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b11762Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b11762_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1.2-M01/b11762/b11762.cs
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


namespace b11762;

using System;
// using Xunit;

public class test1
{

    public static double f1()
    {
        return 1.0;
    }

    internal static void foo()
    {
        Console.Write(".");
    }

    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        double c = 100.0;
        double a = f1();
        double b = f1();
        int x = 0;
        while (c > 0.0)
        {
            c = c * a;
            c = c - b;
            x++;
        }
        Console.WriteLine("\nx=" + x);
        return x;
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return test1.TestEntryPoint();
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
