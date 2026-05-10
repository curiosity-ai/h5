using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class reproTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task repro_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V2.0-Beta2/b321799/repro.cs
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


namespace b321799;

using System;
using System.Runtime.CompilerServices;
// using Xunit;

class Exception1 : Exception { }

class Exception2 : Exception { }

delegate void NoArg();

public class SmallRepro
{

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void Throws1()
    {
        throw new Exception1();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void Throws2()
    {
        throw new Exception2();
    }


    static void Rethrows1()
    {
        try
        {
            Console.WriteLine("In Rethrows1");
            Throws1();
        }
        catch (Exception e)
        {
            Console.WriteLine("Caught {0}, rethrowing", e);
            throw;
        }
    }

    static void CatchAll()
    {
        try
        {
            Console.WriteLine("In CatchAll");
            Throws2();
        }
        catch
        {
            Console.WriteLine("Caught something");
        }
    }

    static void Finally()
    {
        try
        {
            Console.WriteLine("In Finally");
            Rethrows1();
            Console.WriteLine("Unreached");
        }
        finally
        {
            Console.WriteLine("In Finally funclet (1), Exception1 should be in-flight");
            CatchAll();
            Console.WriteLine("In Finally funclet (2), Exception1 should be in-flight");
        }
    }

    // [Fact]
    public static int TestEntryPoint()
    {
        bool bPassed = true;
        // Works
        Console.WriteLine("!!!!!!!!!!!!!!!!! Start Direct Call case !!!!!!!!!!!!!!!!!!!!!!!");
        try
        {
            Finally();
        }
        catch (Exception e)
        {
            if (e is Exception1)
            {
                Console.WriteLine("Caught Exception1");
                Console.WriteLine("Pass direct call");
            }
            else
            {
                Console.WriteLine("!!!! Fail direct call !!!!");
                Console.WriteLine("Caught {0}", e);
                bPassed = false;
            }
        }
        Console.WriteLine();
        Console.WriteLine();

        // Doesn't work
        Console.WriteLine("!!!!!!!!!!!!!!! Start Dynamic Invoke case !!!!!!!!!!!!!!!!!!!!!!");
        try
        {
            new NoArg(Finally).DynamicInvoke(null);
        }
        catch (Exception e)
        {
            if (e.InnerException is Exception1)
            {
                Console.WriteLine("Caught Exception1");
                Console.WriteLine("Pass Dynamic Invoke");
            }
            else
            {
                Console.WriteLine("!!!! Fail Dynamic Invoke !!!!");
                Console.WriteLine("Caught {0}", e.InnerException);
                bPassed = false;
            }
        }
        if (bPassed) return 100;
        return 1;
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return Exception1.TestEntryPoint();
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
