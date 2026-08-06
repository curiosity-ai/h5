using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class enumiconvertibletoint64Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task enumiconvertibletoint64_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/CoreMangLib/system/enum/enumiconvertibletoint64.cs
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

/// <summary>
/// System.Enum.IConvertibleToInt64(System.IFormatProvider)
/// </summary>
public class EnumIConvertibleToInt64
{
    #region Public Methods
    public bool RunTests()
    {
        bool retVal = true;

        TestFramework.LogInformation("[Positive]");
        retVal = PosTest1() && retVal;
        retVal = PosTest2() && retVal;
        retVal = PosTest3() && retVal;
        retVal = PosTest4() && retVal;
        retVal = PosTest5() && retVal;

        return retVal;
    }

    #region Positive Test Cases
    public bool PosTest1()
    {
        bool retVal = true;


        TestFramework.BeginScenario("PosTest1: Test a customized enum type");

        try
        {
            color e1 = color.blue;
            IConvertible i1 = e1 as IConvertible;
            long l1 = i1.ToInt64(null);
            if (l1 != 100)
            {
                TestFramework.LogError("001", "The result is not the value as expected");
                retVal = false;
            }
        }
        catch (Exception e)
        {
            TestFramework.LogError("002", "Unexpected exception: " + e);
            retVal = false;
        }

        return retVal;
    }

    public bool PosTest2()
    {
        bool retVal = true;

        TestFramework.BeginScenario("PosTest2: Test a system defined enum type");

        try
        {
            Enum e2 = System.StringComparison.CurrentCultureIgnoreCase;
            long l2 = (e2 as IConvertible).ToInt64(null);
            if (l2 != 1)
            {
                TestFramework.LogError("003", "The result is not the value as expected");
                retVal = false;
            }
        }
        catch (Exception e)
        {
            TestFramework.LogError("004", "Unexpected exception: " + e);
            retVal = false;
        }

        return retVal;
    }

    public bool PosTest3()
    {
        bool retVal = true;

        TestFramework.BeginScenario("PosTest3: Convert a enum to int64, the value of which is Int32.Maximal ");

        try
        {
            e_test e3 = e_test.itemA;
            long l3 = (e3 as IConvertible).ToInt64(null);
            if (l3 != Int32.MaxValue)
            {
                TestFramework.LogError("005", "The result is not the value as expected");
                retVal = false;
            }
        }
        catch (Exception e)
        {
            TestFramework.LogError("006", "Unexpected exception: " + e);
            retVal = false;
        }

        return retVal;
    }

    public bool PosTest4()
    {
        bool retVal = true;


        TestFramework.BeginScenario("PosTest4: Convert a enum to Int64, the value of which is Int64.minvalue");

        try
        {
            e_test e4 = e_test.itemB;
            IConvertible i4 = e4 as IConvertible;
            long l4 = i4.ToInt64(null);
            if (l4 != Int64.MinValue)
            {
                TestFramework.LogError("007", "The result is not the value as expected");
                retVal = false;
            }
        }
        catch (Exception e)
        {
            TestFramework.LogError("008", "Unexpected exception: " + e);
            retVal = false;
        }

        return retVal;
    }

    public bool PosTest5()
    {
        bool retVal = true;


        TestFramework.BeginScenario("PosTest5: Convert a enum to Int64, the value of which is Int64.MaxValue");

        try
        {
            e_test? e5 = e_test.itemC;
            IConvertible i5 = e5 as IConvertible;
            long l5 = i5.ToInt64(null);
            if (l5 != Int64.MaxValue)
            {
                TestFramework.LogError("009", "The result is not the value as expected");
                retVal = false;
            }
        }
        catch (Exception e)
        {
            TestFramework.LogError("010", "Unexpected exception: " + e);
            retVal = false;
        }

        return retVal;
    }
    #endregion

    #region Nagetive Test Cases
    #endregion
    #endregion

    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        EnumIConvertibleToInt64 test = new EnumIConvertibleToInt64();

        TestFramework.BeginTestCase("EnumIConvertibleToInt64");

        if (test.RunTests())
        {
            TestFramework.EndTestCase();
            TestFramework.LogInformation("PASS");
            return 100;
        }
        else
        {
            TestFramework.EndTestCase();
            TestFramework.LogInformation("FAIL");
            return 0;
        }
    }
    enum color
    {
        blue = 100,
        white,
        red,
    }
    enum e_test : long
    {
        itemA = Int32.MaxValue,
        itemB = Int64.MinValue,
        itemC = Int64.MaxValue,
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return EnumIConvertibleToInt64.TestEntryPoint();
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
