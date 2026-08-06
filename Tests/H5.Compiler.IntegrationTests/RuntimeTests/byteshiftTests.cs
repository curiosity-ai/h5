using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class byteshiftTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task byteshift_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1.2-M01/b07211/byteshift.cs
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

using System;
// using Xunit;
// namespace b07211
{
    public class ShiftTest
    {
        public byte data = 0xF0;
    }

    public class Test
    {
        // [OuterLoop]
        // [Fact]
        public static int TestEntryPoint()
        {
            Console.WriteLine("Both results should be 15");
            // This works
            byte dataByte = 0xF0;
            dataByte >>= 4; // becomes 0x0F
            Console.WriteLine(dataByte);

            // This gives wrong result
            ShiftTest shiftTest = new ShiftTest();
            shiftTest.data >>= 4; // becomes 0xFF
            Console.WriteLine(shiftTest.data);

            if (shiftTest.data != 0xF)
                return 1;
            else
                return 100;
        }
    }

// }




public class Program
{
    public static int Main()
    {
        try {
            return ShiftTest.TestEntryPoint();
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
