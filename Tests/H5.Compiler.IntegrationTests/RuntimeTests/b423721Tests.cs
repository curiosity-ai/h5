using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b423721Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b423721_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V2.0-Beta2/b423721/b423721.cs
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
using System.Reflection;
// using Xunit;

// namespace b423721
{

    public class C2
    {
        // [OuterLoop]
        // [Fact]
        public static int TestEntryPoint()
        {
            int ret = 100;

            if (C1Helper.IsFoo(C1<string>.GetString()))
            {
                Console.WriteLine("PASS: C1<string> handles intra-assembly interning");
            }
            else
            {
                Console.WriteLine("FAIL: C1<string> does NOT handles intra-assembly interning");
                ret = 101;
            }

            if (C1Helper.IsFoo(C1<int>.GetString()))
            {
                Console.WriteLine("PASS: C1<int> handles intra-assembly interning");
            }
            else
            {
                Console.WriteLine("FAIL: C1<int> does NOT handles intra-assembly interning");
                ret = 101;
            }

            Type t = Type.GetType("b423721.C1`1[[System.Int64, mscorlib, Version=0.0.0.0, Culture=neutral ]], c1, Version=0.0.0.0, Culture=neutral");
            if (t == null)
            {
                Console.WriteLine("FAIL: Could not get Type C1`1[[System.Int64, mscorlib, Version=0.0.0.0, Culture=neutral ]], c1, Version=0.0.0.0, Culture=neutral");
                return 101;
            }

            Console.WriteLine("Test SUCCESS");

            return ret;
        }
    }

// }


public class Program
{
    public static int Main()
    {
        try {
            return C2.TestEntryPoint();
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
