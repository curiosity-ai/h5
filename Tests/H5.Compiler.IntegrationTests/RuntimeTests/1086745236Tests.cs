using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class 1086745236Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task 1086745236_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V2.0-Beta2/b320147/1086745236.cs
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


namespace b320147;

using System;
using System.Collections;
using System.Runtime.InteropServices;
// using Xunit;

public struct AA
{
    public void Method1()
    {
        bool local1 = true;
        for (; local1; )
        {
            if (local1)
                break;
        }
        do
        {
            if (local1)
                break;
        }
        while (local1);
        return;
    }

}

[StructLayout(LayoutKind.Sequential)]
public class App
{
    // [OuterLoop]
    // [Fact]
    public static void TestEntryPoint()
    {
        try
        {
            Console.WriteLine("Testing AA::Method1");
            new AA().Method1();
        }
        catch (Exception x)
        {
            Console.WriteLine("Exception handled: " + x.ToString());
        }

        // JIT Stress test... if jitted it passes
        Console.WriteLine("Passed.");
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return App.TestEntryPoint();
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
