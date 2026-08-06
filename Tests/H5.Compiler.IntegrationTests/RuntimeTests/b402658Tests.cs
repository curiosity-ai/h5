using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b402658Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b402658_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/dev10/b402658/b402658.cs
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
/*
   IndexOutOfRange Exception When Using UShort or Short as an Input Array Type
*/


namespace b402658;

using System;
using System.Runtime.CompilerServices;
// using Xunit;

public class small_repro
{
    void bug(int num)
    {
        short[] src = GetArray();
        // The induction variable is i4, but the array indexes are i8
        // on x64.  OSR gets confused by the different sym keys for the
        // equivsyms and creates different symbols for the rewritten
        // IVs and ends up with a def with no use and a use with no def!
        for (int i = 0; i < num; i += src.Length)
        {
            this.dst[i] = src[0];
            this.dst[i + 1] = src[1];
            this.dst[i + 2] = src[2];
        }
    }

    short[] dst = new short[12];

    [MethodImpl(MethodImplOptions.NoInlining)]
    short[] GetArray()
    {
        return new short[] { 0x100, 0x101, 0x102 };
    }

    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        small_repro s = new small_repro();
        try
        {
            s.bug(12);
            Console.WriteLine("Pass");
            return 100;
        }
        catch
        {
            Console.WriteLine("Fail");
            return 110;
        }
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return small_repro.TestEntryPoint();
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
