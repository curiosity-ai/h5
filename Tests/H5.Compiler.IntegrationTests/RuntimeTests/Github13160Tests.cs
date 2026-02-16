using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class Github13160Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task Github13160_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/CoreMangLib/system/delegate/regressions/Github_13160/Github_13160.cs
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

public class Github_13160
{
    public virtual void VirtualMethod()
    {
    }

    public void NonVirtualMethod()
    {
    }

    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        Github_13160 p = new Github_13160();

        Action d1 = p.VirtualMethod;
        Action d2 = p.VirtualMethod;

        if (!d1.Equals(d2))
        {
            Console.WriteLine("FAILED: d1.Equals(d2) is not true");
            return 200;
        }

        if (d1.GetHashCode() != d2.GetHashCode())
        {
            Console.WriteLine("FAILED: d1.GetHashCode() != d2.GetHashCode()");
            return 201;
        }

        Action d3 = p.NonVirtualMethod;
        Action d4 = p.NonVirtualMethod;

        if (!d3.Equals(d4))
        {
            Console.WriteLine("FAILED: d3.Equals(d4) is not true");
            return 202;
        }

        if (d3.GetHashCode() != d4.GetHashCode())
        {
            Console.WriteLine("FAILED: d3.GetHashCode() != d4.GetHashCode()");
            return 203;
        }

        return 100;
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return Github_13160.TestEntryPoint();
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
