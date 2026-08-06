using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b491215Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b491215_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V2.0-RTM/b491215/b491215.cs
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


namespace b491215;

using System;
using System.Collections;
using System.Collections.Generic;
// using Xunit;

public class Test
{
    internal static void IsType<T>(object o, bool expectedValue)
    {
        bool isType = o is T;
        Console.WriteLine("{0} is {1} (expected {2}): {3}", o.GetType(), typeof(T), expectedValue, isType);
        if (expectedValue != isType)
            throw new Exception("Casting failed");
    }

    // [Fact]
    public static int TestEntryPoint()
    {
        Object o = null;

        try
        {
            o = new ArgumentException();
            IsType<Exception>(o, true);
            IsType<IEnumerable>(o, false);
            IsType<IEnumerable<int>>(o, false);

            o = new Dictionary<string, bool>();
            IsType<Exception>(o, false);
            IsType<IEnumerable>(o, true);
            IsType<IEnumerable<KeyValuePair<string, bool>>>(o, true);

            o = new List<int>();
            IsType<Exception>(o, false);
            IsType<IEnumerable>(o, true);
            IsType<IEnumerable<int>>(o, true);

            Console.WriteLine("Test SUCCESS");
            return 100;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine("Test FAILED");
            return 101;
        }
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return Test.TestEntryPoint();
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
