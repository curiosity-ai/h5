using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b08944aTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b08944a_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b08944/b08944a.cs
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
//extern("msvcrt.dll:printf") int printf(const char *fmt, ...);
//unsigned int _exception_code();

// namespace b08944a
{
    //@BEGINRENAME; Verify this renames
    //@ENDRENAME; Verify this renames
    using System;

    public class Y
    {
        /*
        int     filt(unsigned a)
        {
            Console.WriteLine("Exception code = " + a);
            return  1;
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
            UInt32 ec;
            ec = (UInt32)0;
            Console.WriteLine("Starting up.");
            try
            {
                bomb();
            }
            //except(filt(ec = _exception_code()))
            catch (NullReferenceException)
            {
                ec = (UInt32)1;
                Console.WriteLine("Caught the exception [code = " + ec + "]");
            }

            if (ec == 0)
            {
                Console.WriteLine("Failed.");
                return 1;
            }

            Console.WriteLine("Passed.");
            return 100;
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
