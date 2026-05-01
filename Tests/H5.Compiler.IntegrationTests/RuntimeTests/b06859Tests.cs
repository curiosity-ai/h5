using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b06859Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b06859_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b06859/b06859.cs
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
// namespace b06859
{
    using System;
    using System.Collections;

    public class test
    {

        internal static void ccc(byte[] bytes)
        {
            int[] m_array;
            int m_length;

            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            m_array = new int[(bytes.Length + 3) / 4];
            m_length = bytes.Length * 8;

            int i = 0;
            int j = 0;
            while (bytes.Length - j >= 4)
            {
                m_array[i++] = (bytes[j] & 0xff) |
                              ((bytes[j + 1] & 0xff) << 8) |
                              ((bytes[j + 2] & 0xff) << 16) |
                              ((bytes[j + 3] & 0xff) << 24);
                j += 4;
            }
            if (bytes.Length - j >= 0)
            {
                Console.WriteLine("hhhh");
            }
        }

        // [OuterLoop]
        // [Fact]
        public static void TestEntryPoint()
        {
            byte[] ub = new byte[0];
            ccc(ub);
        }
    }
// }


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
