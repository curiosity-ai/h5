using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class vsw338014Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task vsw338014_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V2.0-Beta2/b338014/vsw338014.cs
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

// This is a coverage test...
//  The "fat" gc encoding was assumed to be dead code, but this test hits it
//
//  We want to hit PendingArgsStack::pasEnumGCoffs
//                 PendingArgsStack::pasEnumGCoffsCount


namespace b338014;

using System;
using System.Runtime.CompilerServices;
// using Xunit;

public class My
{

    [MethodImplAttribute(MethodImplOptions.NoInlining)]
    static string foo(
        Object o0, Object o1, Object o2, Object o3, Object o4, Object o5, Object o6, Object o7, Object o8, Object o9,
        Object o10, Object o11, Object o12, Object o13, Object o14, Object o15, Object o16, Object o17, Object o18, Object o19,
        Object o20, Object o21, Object o22, Object o23, Object o24, Object o25, Object o26, Object o27, Object o28, Object o29,
        Object o30, Object o31, Object o32, Object o33, Object o34, Object o35, Object o36, Object o37, Object o38, Object o39)
    {
        return null;
    }

    [MethodImplAttribute(MethodImplOptions.NoInlining)]
    static Object bar(Object o)
    {
        return null;
    }

    // [OuterLoop]
    // [Fact]
    public static void TestEntryPoint()
    {
        Object o = new Object();
        foo(o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, bar(o), o);
    }
}



public class Program
{
    public static int Main()
    {
        try {
            return My.TestEntryPoint();
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
