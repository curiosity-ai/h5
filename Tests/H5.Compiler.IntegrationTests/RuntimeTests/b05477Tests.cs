using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b05477Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b05477_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b05477/b05477.cs
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


namespace b05477;

using System;
// using Xunit;

public class TestClass
{
    public int IntI = 0;
}

public class mem035
{

    public static TestClass getTC
    {
        get
        {
            return null;
        }
    }

    // [Fact]
    public static int TestEntryPoint()
    {
        int RetInt = 1;

        try
        {
            int TempInt = getTC.IntI;
        }
        catch (NullReferenceException)
        {
            RetInt = 100;
        }
        return RetInt;
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return TestClass.TestEntryPoint();
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
