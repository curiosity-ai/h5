using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b13466Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b13466_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b13466/b13466.cs
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
// namespace Default
{
    //@BEGINRENAME; Verify this renames
    //@ENDRENAME; Verify this renames
    using System;

    public class q
    {
        static
        int func(int i, int updateAddr, byte[] newBytes, int[] m_fixupPos)
        {
            while (i > 10)
            {
                if (i == 3)
                {
                    if (updateAddr < 0)
                        newBytes[m_fixupPos[i]] = (byte)(256 + updateAddr);
                    else
                        newBytes[m_fixupPos[i]] = (byte)updateAddr;
                }
                else
                    i--;
            }

            return i;
        }

        // [OuterLoop]
        // [Fact]
        public static void TestEntryPoint()
        {
            func(0, 0, null, null);
        }
    }
// }


public class Program
{
    public static int Main()
    {
        try {
            return q.TestEntryPoint();
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
