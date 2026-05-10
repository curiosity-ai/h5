using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class pStateTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task pState_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1.2-M01/b16570/pState.cs
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


namespace b16570;

using System;
// using Xunit;
public class test
{
    public struct VT
    {
        public double a1;
        public float a4;
        public VT(int i)
        {
            a1 = 1;
            a4 = 1;
        }
    }

    public class CL
    {
        public double[,,] arr3d = new double[5, 11, 4];
        public float a5 = -0.001953125F;
    }

    public static VT vtstatic = new VT(1);
    public static CL clstatic = new CL();

    public static double Func()
    {
        VT vt = new VT(1);
        vt.a1 = -2.0386427882503781E-07;
        vt.a4 = 0.5F;
        CL cl = new CL();

        vtstatic.a1 = -2.0386427882503781E-07;
        vtstatic.a4 = 0.5F;
        cl.arr3d[4, 0, 3] = 1.1920928955078125E-07;
        float asgop0 = vtstatic.a4;
        asgop0 -= (0.484375F);
        if (((vtstatic.a4 * clstatic.a5)) <= (vtstatic.a4))
        {
            return Convert.ToDouble((((vtstatic.a4 * clstatic.a5) + (asgop0 - (0.25F - 0.235290527F))) / (cl.arr3d[4, 0, 3] - (vt.a1))));
        }
        else
        {
            if ((cl.arr3d[4, 0, 3]) < ((((vtstatic.a4 * clstatic.a5) + (asgop0 - (0.25F - 0.235290527F))) / (cl.arr3d[4, 0, 3] - (vt.a1)))))
            {
                double if0_1retval = Convert.ToDouble((((vtstatic.a4 * clstatic.a5) + (asgop0 - (0.25F - 0.235290527F))) / (cl.arr3d[4, 0, 3] - (vt.a1))));
                return if0_1retval;
            }
        }
        return Convert.ToDouble((((vtstatic.a4 * clstatic.a5) + (asgop0 - (0.25F - 0.235290527F))) / (cl.arr3d[4, 0, 3] - (vt.a1))));
    }

    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        double retval = Func();
        if ((retval > -191) && (retval < -188))
        {
            Console.WriteLine("PASSED");
            return 100;
        }
        else
        {
            Console.WriteLine("FAILED");
            Console.WriteLine(retval);
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
