using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b405223Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b405223_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V2.0-Beta2/b405223/b405223.cs
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


namespace b405223;

using System;
// using Xunit;

public class Class1
{

    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        Console.WriteLine("Note that this is a test to verify that the implementation stays buggy");
        object o = new short[3];
        if (o is char[])
        {
            Console.WriteLine("Whidbey behavior");
            Console.WriteLine("Test FAILED");
            return 101;
        }
        else
        {
            Console.WriteLine("Everett behavior");
            Console.WriteLine("Test SUCCESS");
            return 100;
        }
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return Class1.TestEntryPoint();
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
