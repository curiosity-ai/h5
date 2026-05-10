using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b119294Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b119294_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1.1-M1-Beta1/b119294/b119294.cs
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


namespace b119294;

using System;
using System.Collections;
//using System.Windows.Forms;
using System.IO;
using System.Text;
// using Xunit;

public class Test_b119294
{
    public int[,] m_nSourceDestMap;
    public static int m_coSourceLength = 100;
    public static int m_coDestLength = 100;
    // [OuterLoop]
    // [Fact]
    static public void TestEntryPoint()
    {
        String testenv = Environment.GetEnvironmentVariable("URTBUILDENV");
        if ((testenv == null) || (testenv.ToUpper() != "FRE"))
        {
            Console.WriteLine("Skip the test since %URTBUILDENV% NEQ 'FRE'");
        }

        Test_b119294 t = new Test_b119294();

        t.EstablishIdentityTransform();
    }

    internal void EstablishIdentityTransform()
    {
        //MessageBox.Show("EstablishIdentityTransform() enter");
        int nSourceElements = m_coSourceLength;
        int nDestElements = m_coDestLength;
        int nElements = Math.Max(nSourceElements, nDestElements);
        m_nSourceDestMap = new int[nElements, 2];
        for (int nIndex = 0; nIndex < nElements; nIndex++)
        {
            m_nSourceDestMap[nIndex, 0] = (nIndex > nSourceElements) ? -1 : nIndex;
            m_nSourceDestMap[nIndex, 1] = (nIndex > nDestElements) ? -1 : nIndex;
        }
        //MessageBox.Show("EstablishIdentityTransform() leave");
    }

}


public class Program
{
    public static int Main()
    {
        try {
            return Test_b119294.TestEntryPoint();
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
