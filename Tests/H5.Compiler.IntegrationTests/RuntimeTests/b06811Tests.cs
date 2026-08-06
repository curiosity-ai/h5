using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b06811Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b06811_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b06811/b06811.cs
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


namespace b06811;

using System;
using System.Collections;
// using Xunit;


public class test
{
    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        ArrayList objList = new ArrayList();
        objList.Add("hey");
        objList.Add(null);

        IEnumerator ienum = objList.GetEnumerator();
        int iCounter = 0;
        while (ienum.MoveNext())
        {
            iCounter++;
            Console.WriteLine(iCounter.ToString());
        }
        if (iCounter == 2)
        {
            Console.WriteLine("Passed");
            return 100;
        }
        else
        {
            Console.WriteLine("Failed");
            return 1;
        }
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return test.TestEntryPoint();
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
