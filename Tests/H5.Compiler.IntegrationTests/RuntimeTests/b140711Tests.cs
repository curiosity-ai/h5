using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b140711Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b140711_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1.1-M1-Beta1/b140711/b140711.cs
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


namespace b140711;

using System;
// using Xunit;

public class BadMath
{
    public static double[,] Res = new double[2, 40];
    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {

        double t0 = 1.5;
        int i = 0;
        for (i = 0; i < 4; i++)
        {
            double dd = t0 / 3;
            Res[0, i] = t0;
            Res[1, i] = dd;
            t0 -= dd;
            if (dd > 2)
            {
                break;
            }
        }

        for (int j = 0; (j < i); j++)
            Console.WriteLine(Res[0, j] + " " + Res[1, j]);

        Console.WriteLine();

        if (Res[0, 0] != 1.5)
        {
            Console.WriteLine("Res[0,0] is {0}", Res[0, 0]);
            Console.WriteLine("FAILED");
            return 1;
        }
        if (Res[1, 0] != 0.5)
        {
            Console.WriteLine("Res[1,0] is {0}", Res[1, 0]);
            Console.WriteLine("FAILED");
            return 1;
        }
        if (Res[0, 1] != 1.0)
        {
            Console.WriteLine("Res[0,1] is {0}", Res[0, 1]);
            Console.WriteLine("FAILED");
            return 1;
        }
        if ((Res[1, 1] - 0.333333333333333) > 0.000001)
        {
            Console.WriteLine("Res[1,1] is {0}", Res[1, 1]);
            Console.WriteLine("FAILED");
            return 1;
        }
        if ((Res[0, 2] - 0.666666666666667) > 0.000001)
        {
            Console.WriteLine("Res[0,2] is {0}", Res[0, 2]);
            Console.WriteLine("FAILED");
            return 1;
        }
        if ((Res[1, 2] - 0.222222222222222) > 0.000001)
        {
            Console.WriteLine("Res[1,2] is {0}", Res[1, 2]);
            Console.WriteLine("FAILED");
            return 1;
        }
        if ((Res[0, 3] - 0.444444444444445) > 0.000001)
        {
            Console.WriteLine("Res[0,3] is {0}", Res[0, 3]);
            Console.WriteLine("FAILED");
            return 1;
        }
        if ((Res[1, 3] - 0.148148148148148) > 0.000001)
        {
            Console.WriteLine("Res[1,3] is {0}", Res[1, 3]);
            Console.WriteLine("FAILED");
            return 1;
        }

        Console.WriteLine("PASSED");
        return 100;
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return BadMath.TestEntryPoint();
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
