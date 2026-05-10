using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class refanyvalTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task refanyval_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/xxobj/operand/refanyval.cs
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

// namespace JitTest_refanyval_operand_cs
{
    public struct Test
    {
        private int _m_v;

        private static int refanyval_ldobj()
        {
            Test T = new Test();
            T._m_v = 1;
            TypedReference R = __makeref(T);
            return __refvalue(R, Test)._m_v - 1;
        }

        private static int refanyval_initobj()
        {
            Test T = new Test();
            T._m_v = 1;
            TypedReference R = __makeref(T);
            __refvalue(R, Test) = new Test();
            return T._m_v;
        }

        private static int refanyval_cpobj()
        {
            Test T = new Test();
            T._m_v = 1;
            Test T1 = new Test();
            TypedReference R = __makeref(T);    //replace with cpobj in IL
            __refvalue(R, Test) = T1;
            return T._m_v;
        }

        private static int refanyval_stobj()
        {
            Test T = new Test();
            T._m_v = 1;
            Test T1 = new Test();
            TypedReference R = __makeref(T);
            __refvalue(R, Test) = T1;
            return T._m_v;
        }

        // [Fact]
        // [OuterLoop]
        public static int TestEntryPoint()
        {
            if (refanyval_ldobj() != 0)
            {
                return 101;
            }
            if (refanyval_initobj() != 0)
            {
                return 102;
            }
            if (refanyval_stobj() != 0)
            {
                return 103;
            }
            if (refanyval_cpobj() != 0)
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
            return refanyval.TestEntryPoint();
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
