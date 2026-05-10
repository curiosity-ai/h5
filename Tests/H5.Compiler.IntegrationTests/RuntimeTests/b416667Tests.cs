using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b416667Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b416667_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V2.0-Beta2/b416667/b416667.cs
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


namespace b416667;

using System;
// using Xunit;
public class CMain
{
    public static int Count = 0;
    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        String s;
        s = Gen<String>.x;
        // we expect the Gen<T>.cctor to fire only once!
        if (1 == Count)
        {
            Console.WriteLine("Test SUCCESS");
            return 100;
        }
        else
        {
            Console.WriteLine("Test FAILED");
            return 101;
        }
    }
}

public class Gen<T>
{

    public static T x;
    static Gen()
    {
        CMain.Count++;
        Console.WriteLine("cctor.  Type: {0}", typeof(T).ToString());
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return CMain.TestEntryPoint();
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
