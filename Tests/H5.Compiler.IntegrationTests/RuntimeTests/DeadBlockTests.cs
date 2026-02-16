using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class DeadBlockTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task DeadBlock_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V2.0-Beta2/b102533/DeadBlock.cs
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


namespace b102533;

using System;
// using Xunit;
public struct AA
{
    public static void f()
    {
        while (App.flag)
        {
            bool a = true;
            while (a)
            {
                if (a)
                    break;
                else
                {
                    if (a)
                    {
                    }
                }
            }
            a = false;
            do
            {
            }
            while (a);

            // stop the loop
            App.flag = false;
        }
    }

}

public class App
{
    // [OuterLoop]
    // [Fact]
    public static void TestEntryPoint()
    {
        try
        {
            AA.f();
        }
        catch (Exception x)
        {
            Console.WriteLine("Exception handled: " + x.ToString());
        }
        Console.WriteLine("Passed.");
    }
    public static bool flag = true;
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
