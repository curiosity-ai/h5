using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class SecondLevelIndirectionTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task SecondLevelIndirection_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/CoreMangLib/system/delegate/miscellaneous/SecondLevelIndirection.cs
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
using System;
// using Xunit;

public class SecondLevelIndirection
{
    public int value = 23;

    public void Update(int num)
    {
        value += num;
    }

    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        SecondLevelIndirection prog = new SecondLevelIndirection();

        Action<int> action = prog.Update;
        Action<int> secondLevel = action.Invoke;

        secondLevel(77);

        // Update should be invoked exactly once
        return prog.value;
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return SecondLevelIndirection.TestEntryPoint();
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
