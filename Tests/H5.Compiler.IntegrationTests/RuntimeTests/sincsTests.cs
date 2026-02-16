using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class sincsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task sincs_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/Boxing/xlang/sin_cs.cs
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
extern alias sinlib_cs;
// using Xunit;

// namespace SinCalc_against_sinlib_cs
{
    using System;
    using sinlib_cs::SinCalcLib;

    public class SinCalc
    {
        // [Fact]
        // [OuterLoop]
        public static int TestEntryPoint()
        {
            object i;
            object Angle;
            object Result1, Result2;
            object[] testresults = new object[10];
            testresults[0] = 0.000000000d;
            testresults[1] = 0.309016994d;
            testresults[2] = 0.587785252d;
            testresults[3] = 0.809016994d;
            testresults[4] = 0.951056516d;
            testresults[5] = 1.000000000d;
            testresults[6] = 0.951056516d;
            testresults[7] = 0.809016994d;
            testresults[8] = 0.587785252d;
            testresults[9] = 0.309016994d;

            object mistake = 1e-9d;
            for (i = 0; (int)i < 10; i = (int)i + 1)
            {
                Angle = ((PiVal)SinCalcLib.PI).Value * ((int)i / 10.0);
                Console.Write("Classlib Sin(");
                Console.Write(Angle);
                Console.Write(")=");
                Console.WriteLine(Result1 = Math.Sin((double)Angle));
                Console.Write("This Version(");
                Console.Write(Angle);
                Console.Write(")=");
                Console.WriteLine(Result2 = (double)SinCalcLib.mySin(Angle));
                Console.Write("Error is:");
                Console.WriteLine((double)Result1 - (double)Result2);
                Console.WriteLine();
                if (Math.Abs((double)Result1 - (double)Result2) > (double)mistake) // reasonable considering double
                {
                    Console.WriteLine("ERROR, Versions too far apart!");
                    return 1;
                }
                if (Math.Abs((double)testresults[(int)i] - (double)Result1) > (double)mistake) // reasonable considering double
                {
                    Console.WriteLine("ERROR, Classlib version isnt right!");
                    return 1;
                }
                if (Math.Abs((double)testresults[(int)i] - (double)Result2) > (double)mistake) // reasonable considering double
                {
                    Console.WriteLine("ERROR, our version isnt right!");
                    return 1;
                }

            }
            Console.WriteLine("Yippie, all correct");
            return 100;
        }
    }
// }


public class Program
{
    public static int Main()
    {
        try {
            return SinCalc.TestEntryPoint();
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
