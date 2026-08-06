using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class instanceTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task instance_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/Boxing/callconv/instance.cs
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


// namespace BoxTest_instance_cs
{
    public abstract class BaseTest
    {
        protected abstract object Fibonacci2(object num, object flag);
    }

    public class Test : BaseTest
    {
        private object _num;

        protected object Fibonacci(object num, object flag)
        {
            if (num.GetType() != typeof(float) ||
                flag.GetType() != typeof(bool))
                throw new Exception();

            return Fibonacci2(num, flag);
        }

        protected override object Fibonacci2(object num, object flag)
        {
            object N;
            if ((float)num < 1.1)
                N = num;
            else
                N = (float)Fibonacci((float)num - 2.0f, false) + (float)Fibonacci((float)num - 1.0f, flag);
            if ((bool)flag)
                Console.Write(N.ToString() + " ");
            return N;
        }

        private Test(object num)
        {
            _num = (float)(double)num;
        }

        public object Print()
        {
            Fibonacci(_num, true);
            Console.WriteLine();
            return _num;
        }

        // [Fact]
        // [OuterLoop]
        public static void TestEntryPoint()
        {
            Test test = new Test(20.0d);
            test.Print();
            Console.WriteLine("*** PASSED ***");
        }
    }
// }


public class Program
{
    public static int Main()
    {
        try {
            return BaseTest.TestEntryPoint();
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
