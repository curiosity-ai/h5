using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class ddb113347Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task ddb113347_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/CoreMangLib/system/delegate/regressions/devdivbugs/113347/ddb113347.cs
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
using System.Security;
// using Xunit;

[SecuritySafeCritical]
public class DDB113347 {
    // [OuterLoop]
    [ConditionalFact(typeof(Utilities), nameof(Utilities.IsNotNativeAot))]
    public static int TestEntryPoint() {
        Console.WriteLine("Attempting delegate construction with null method pointer.");
        Console.WriteLine("Expecting: ArgumentNullException wrapped in TargetInvocationException.");
        try {
            Activator.CreateInstance(typeof(Action<object>), null, IntPtr.Zero);
            Console.WriteLine("FAIL: Creation succeeded");
            return 200;
        }
        catch (TargetInvocationException ex) {
            Console.WriteLine("Caught expected TargetInvocationException");
            if (ex.InnerException == null) {
                Console.WriteLine("No inner exception was provided");
                Console.WriteLine("FAILED");
                return 201;
            }
            else if (ex.InnerException is ArgumentNullException) {
                Console.WriteLine("Inner exception is ArgumentNullException as expected");
                Console.WriteLine("PASSED");
                return 100;
            }
            else {
                Console.WriteLine("Unexpected inner exception: {0}", ex.InnerException);
                Console.WriteLine("FAILED");
                return 202;
            }
        }
        catch (Exception ex) {
            Console.WriteLine("Caught unexpected exception: {0}", ex);
            Console.WriteLine("FAILED");
            return 203;
        }
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return DDB113347.TestEntryPoint();
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
