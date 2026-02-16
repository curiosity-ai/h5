using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class testclassTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task testclass_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-QFE/b148815/testclass.cs
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

using System;
// using Xunit;

// namespace TestClass
{

    public class Test
    {
        // [OuterLoop]
        // [Fact]
        public static int TestEntryPoint()
        {
            double a = new TestClass().ApplyTime();
            if (a == 5000)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }
    }
    public struct ExpenseValues
    {
        public double AnnualPaidOutsideFunds;
    }

    public class TestClass
    {
        double mPeriodicExpense = 10000.0;

        public TestClass()
        {

        }

        public double ApplyTime()
        {
            ExpenseValues values = new ExpenseValues();
            values.AnnualPaidOutsideFunds = 0.0;
            double expense = mPeriodicExpense;
            double outside = 0.50 * expense;
            expense = expense - outside;

            // if you comment the next line the rutn value == 5000 (correct)
            values.AnnualPaidOutsideFunds += outside;

            return expense;
        }
    }
// }


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
