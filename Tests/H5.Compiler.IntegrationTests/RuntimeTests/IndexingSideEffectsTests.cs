using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class IndexingSideEffectsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task IndexingSideEffects_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/Arrays/misc/IndexingSideEffects.cs
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
using System.Runtime.CompilerServices;
// using Xunit;

namespace IndexingSideEffects;

public class IndexingSideEffects
{
    // [Fact]
    public static int TestEntryPoint()
    {
        if (!Problem())
        {
            return 101;
        }

        return 100;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool Problem()
    {
        bool result = false;

        try
        {
            TryIndexing(Array.Empty<int>());
        }
        catch (IndexOutOfRangeException)
        {
            result = true;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void TryIndexing(int[] a)
    {
        // Make sure that early flowgraph simplification does not remove the side effect of indexing
        // when deleting the relop.
        if (a[int.MaxValue] == 0)
        {
            NopInlinedCall();
        }
    }

    private static void NopInlinedCall() { }
}


public class Program
{
    public static int Main()
    {
        try {
            return IndexingSideEffects.TestEntryPoint();
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
