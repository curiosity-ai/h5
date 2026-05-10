using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class 5w1d06Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task 5w1d06_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/fp/exgen/5w1d-06.cs
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
// namespace Test_5w1d_06
{
public class testout1
{
    private static double[,,] s_arr3d_0 = new double[5, 6, 4];
    private static double[,] s_arr2d_0 = new double[3, 6];


    public static int Func_0()
    {
        s_arr3d_0[4, 0, 3] = 1177305879;
        s_arr2d_0[2, 1] = 1177305779D;
        if ((s_arr2d_0[2, 1]) <= ((int)(s_arr3d_0[4, 0, 3]) % (int)s_arr2d_0[2, 1]))
            Console.WriteLine("Func_0: <= true");
        int retval_0 = (int)(s_arr3d_0[4, 0, 3]) % (int)s_arr2d_0[2, 1];
        return retval_0;
    }

    // [Fact]
    // [OuterLoop]
    public static int TestEntryPoint()
    {
        int retval;
        retval = Convert.ToInt32(Func_0());
        if ((retval >= 99) && (retval < 100))
            retval = 100;
        if ((retval > 100) && (retval <= 101))
            retval = 100;
        Console.WriteLine(retval);
        return retval;
    }
}
// }


public class Program
{
    public static int Main()
    {
        try {
            return testout1.TestEntryPoint();
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
