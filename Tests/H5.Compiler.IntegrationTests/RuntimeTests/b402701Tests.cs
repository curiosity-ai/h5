using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b402701Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b402701_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/dev10/b402701/b402701.cs
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


namespace b402701;

using System;
using System.Security;
// using Xunit;


public class Foo
{
    internal virtual void callee()
    {
        Console.WriteLine("callee");
    }

    internal static void caller(object o)
    {
        if (o == null)
            return;
        if (o.GetType() == typeof(Foo))
        {
            ((Foo)o).callee();
        }
    }

    // [OuterLoop]
    // [Fact]
    public static void TestEntryPoint()
    {
        Foo f = new Foo();
        caller(f);

        Console.WriteLine("test passed");
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return Foo.TestEntryPoint();
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
