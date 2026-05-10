using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class enumiconvertibletouint64Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task enumiconvertibletouint64_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/CoreMangLib/system/enum/enumiconvertibletouint64.cs
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
/// System.Enum.IConvertibleToUint64(System.Type,IFormatProvider )
/// </summary>
public class EnumIConvertibleToUint64
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

        TestFramework.LogInformation("[Negative]");
        retVal = NegTest1() && retVal;

        return retVal;
    }

    #region Positive Test Cases
    public bool PosTest1()
    {
        bool retVal = true;


        TestFramework.BeginScenario("PosTest1: Convert an enum of zero to Uint64");

        try
        {
            color c1 = color.blue;
            IConvertible i1 = c1 as IConvertible;
            UInt64 u1 = i1.ToUInt64(null);
            if (u1 != 0)
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
            Enum e2 = System.StringComparison.OrdinalIgnoreCase;
            UInt64 l2 = (e2 as IConvertible).ToUInt64(null);
            if (l2 != 5)
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

        TestFramework.BeginScenario("PosTest3: Convert an enum of Uint64.maxvalue to Uint64");

        try
        {
            color c1 = color.white;
            IConvertible i1 = c1 as IConvertible;
            UInt64 u1 = i1.ToUInt64(null);
            if (u1 != UInt64.MaxValue)
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

        TestFramework.BeginScenario("PosTest4: Convert an enum of negative zero to Uint64");

        try
        {
            color c1 = color.red;
            IConvertible i1 = c1 as IConvertible;
            UInt64 u1 = i1.ToUInt64(null);
            if (u1 != 0)
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
    #endregion

    #region Nagetive Test Cases
    public bool NegTest1()
    {
        bool retVal = true;

        TestFramework.BeginScenario("NegTest1: Convert an enum of negative value to Uint64");

        try
        {
            e_test e1 = e_test.itemA;
            IConvertible i1 = e1 as IConvertible;
            UInt64 u1 = i1.ToUInt64(null);
            TestFramework.LogError("101", "The OverflowException was not thrown as expected");
            retVal = false;
        }
        catch (OverflowException)
        {
        }
        catch (Exception e)
        {
            TestFramework.LogError("102", "Unexpected exception: " + e);
            retVal = false;
        }

        return retVal;
    }
    #endregion
    #endregion

    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        EnumIConvertibleToUint64 test = new EnumIConvertibleToUint64();

        TestFramework.BeginTestCase("EnumIConvertibleToUint64");

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

    enum color : ulong
    {
        blue = 0,
        white = UInt64.MaxValue,
        red = -0,
    }
    enum e_test : long
    {
        itemA = -123,
        itemB = Int64.MaxValue,
        itemC = Int64.MaxValue,
        itemD = -0,
    }
}


public class Program
{
    public static int Main()
    {
        try {
            return EnumIConvertibleToUint64.TestEntryPoint();
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
