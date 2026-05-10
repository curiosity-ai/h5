using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b08944bTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b08944b_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b08944/b08944b.cs
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

// using Xunit;
// namespace b08944b
{
    //@BEGINRENAME; Verify this renames
    //@ENDRENAME; Verify this renames
    using System;

    public class Y
    {

        //extern("msvcrt.dll:printf") int printf(const char *fmt, ...);
        //UInt32 int _exception_code();

        /*
        public static int     filt(UInt32 a)
        {
            Console.WriteLine("Exception code = " + a);
            return  1;
        }

        public static int     filt0(UInt32 a)
        {
            Console.WriteLine("Exception code = " + a);
            return  0;
        }
        */

        internal static void bomb()
        {
            char[] p = null;
            p[0] = (char)0;
        }

        // [Fact]
        public static int TestEntryPoint()
        {
            UInt32 ec, ec1;

            ec = (UInt32)0;
            ec1 = (UInt32)0;

            try
            {
                try
                {
                    bomb();
                }
                //except(filt(ec = _exception_code()))
                catch (NullReferenceException e)
                {
                    ec = (UInt32)1;
                    Console.WriteLine("Caught the exception once, now throwing again.");
                    throw e;
                }

            }
            //except(filt(ec1 = _exception_code()))
            catch (NullReferenceException /*e1*/)
            {
                ec1 = (UInt32)2;
                Console.WriteLine("'Outer' catch handler");
                Console.WriteLine("Caught the exception [code1 = " + ec + "] [code2 = " + ec1 + "]");
            }
            //    printf("Caught the exception [code1 = %08X] [code2 = %08X]\n", ec, ec1);
            if ((ec != 0) && (ec1 != 0))
            {
                Console.WriteLine("Passed.");
                return 100;
            }
            Console.WriteLine("Failed.");
            return 1;
        }
    }
// }


public class Program
{
    public static int Main()
    {
        try {
            return Y.TestEntryPoint();
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
