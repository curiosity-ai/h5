using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class unboxTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task unbox_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/xxobj/operand/unbox.cs
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

// namespace JitTest_unbox_operand_cs
{
    public struct Test
    {
        private int _m_v;

        private static int unbox_ldobj()
        {
            Test T = new Test();
            T._m_v = 1;
            object R = T;
            return ((Test)R)._m_v - 1;
        }

        private static int unbox_initobj()
        {
            Test T = new Test();
            T._m_v = 1;
            object R = T;
            R = new Test();     //change to unbox<R> = new Test() in IL
            return ((Test)R)._m_v;
        }

        private static int unbox_cpobj()
        {
            Test T = new Test();
            T._m_v = 1;
            Test T1 = new Test();
            object R = T;
            R = T1;     //change to unbox<R> = T1 in IL
            return ((Test)R)._m_v;
        }

        private static int unbox_stobj()
        {
            Test T = new Test();
            T._m_v = 1;
            Test T1 = new Test();
            object R = T;
            R = T1;     //change to unbox<R> = T1 in IL
            return ((Test)R)._m_v;
        }

        // [Fact]
        // [OuterLoop]
        public static int TestEntryPoint()
        {
            if (unbox_ldobj() != 0)
            {
                return 101;
            }
            if (unbox_initobj() != 0)
            {
                return 102;
            }
            if (unbox_stobj() != 0)
            {
                return 103;
            }
            if (unbox_cpobj() != 0)
            {
                return 104;
            }
            return 100;
        }
    }
// }


public class Program
{
    public static int Main()
    {
        try {
            return unbox.TestEntryPoint();
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
