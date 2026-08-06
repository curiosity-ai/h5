using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b02076Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b02076_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b02076/b02076.cs
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


namespace b02076;

using System;
using System.Runtime.InteropServices;
// using Xunit;

[StructLayout(LayoutKind.Sequential)]
class RECT
{
    public int left;
};

class MyInt
{
    public int i;
};

class CSwarm
{
    public CSwarm()
    {

        i = new MyInt();
        m_rScreen = new RECT();

        i.i = 99;
        m_rScreen.left = 99;
        Console.WriteLine(m_rScreen.left);
        Console.WriteLine(i.i);

        Console.WriteLine("---");

        Console.WriteLine(m_rScreen.left.ToString());
        Console.WriteLine(i.i.ToString());
    }
    RECT m_rScreen;
    MyInt i;
};


public class MainClass
{
    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        CSwarm swarm = new CSwarm();
        return (100);
    }
};




public class Program
{
    public static int Main()
    {
        try {
            return RECT.TestEntryPoint();
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
