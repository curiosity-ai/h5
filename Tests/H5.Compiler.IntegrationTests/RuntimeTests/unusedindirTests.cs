using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class unusedindirTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task unusedindir_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/Boxing/misc/unusedindir.cs
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

// namespace Test_unusedindir_cs
{
public class Program
{
    struct NotPromoted
    {
        public int a, b, c, d, e, f;
    }

    class TypeWithStruct
    {
        public NotPromoted small;

        public TypeWithStruct() => small.c = 100;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void Escape(bool b)
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static NotPromoted Test(TypeWithStruct obj)
    {
        NotPromoted t = obj.small;
        // Try to create an OBJ(ADDR(LCL_VAR)) tree that gtTryRemoveBoxUpstreamEffects
        // does not remove due to a spurios exception side effect nor it parents it to
        // a COMMA like many other unused trees are.
        Escape(Unsafe.As<NotPromoted, NotPromoted>(ref t).GetType() == typeof(NotPromoted));
        return t;
    }

    // [Fact]
    public static int TestEntryPoint() => Test(new TypeWithStruct()).c;
}
// }


public class Program
{
    public static int Main()
    {
        try {
            return Program.TestEntryPoint();
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
