using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b00735Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b00735_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1.2-M01/b00735/b00735.cs
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


namespace b00735;

using System;
// using Xunit;
public struct AA
{
    static void f()
    {
        bool flag = false;
        if (flag)
        {
            while (flag)
            {
                while (flag) { }
            }
        }
        do { } while (flag);
    }
    // [OuterLoop]
    // [Fact]
    public static void TestEntryPoint()
    {
        f();
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return b00735.TestEntryPoint();
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
