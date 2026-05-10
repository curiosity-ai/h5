using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class bbHndIndexTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task bbHndIndex_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1.2-M01/b08020/bbHndIndex.cs
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


namespace b08020;

using System;
// using Xunit;
public class AA
{
    static void f(ref Array param)
    {
        try
        {

        }
        finally
        {
            for (int i = 0; i < 3; i++)
            {
            }
#pragma warning disable 1718
            while ((param != param))
#pragma warning restore 1718
            {
            }
        }
    }

    // [OuterLoop]
    // [Fact]
    public static void TestEntryPoint()
    {
        f(ref m_arr);
        Console.WriteLine("Passed.");
    }

    static Array m_arr;

}


public class Program
{
    public static int Main()
    {
        try {
            return AA.TestEntryPoint();
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
