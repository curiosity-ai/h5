using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class TypeToStringTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task TypeToString()
        {
            var code = """
// Original: https://github.com/dotnet/runtime/blob/main/src/tests/CoreMangLib/system/type/typetostring.cs

using System;
using System.Collections.Generic;
// using TestHelper;

// Shim for Xunit
public class FactAttribute : Attribute {}
public class OuterLoopAttribute : Attribute {}

// namespace TestLibrary
// {
    public static class TestFramework
    {
        public static void BeginTestCase(string message) { Console.WriteLine("BeginTestCase: " + message); }
        public static void EndTestCase() { Console.WriteLine("EndTestCase"); }
        public static void LogInformation(string message) { Console.WriteLine(message); }
        public static void LogError(string id, string message) { Console.WriteLine("Error " + id + ": " + message); }
        public static void BeginScenario(string message) { Console.WriteLine("BeginScenario: " + message); }
    }
// }

public class Program
{
    public static void Main()
    {
        TypeToString.TestEntryPoint();
    }
}

/// <summary>
/// Type.ToString()
/// Returns a String representing the name of the current Type
/// </summary>
public class TypeToString
{
    [OuterLoop]
    [Fact]
    public static int TestEntryPoint()
    {
        TypeToString tts = new TypeToString();

        TestFramework.BeginTestCase("for method: Type.ToString()");

        if (tts.RunTests())
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

    public bool RunTests()
    {
        bool retVal = true;

        TestFramework.LogInformation("[Positive]");
        retVal = PosTest1() && retVal;
        retVal = PosTest2() && retVal;
        retVal = PosTest3() && retVal;
        retVal = PosTest4() && retVal;
        retVal = PosTest5() && retVal;
        retVal = PosTest6() && retVal;

        return retVal;
    }

    public bool PosTest1()
    {
        bool retVal = true;

        const string c_TEST_ID = "P001";
        const string c_TEST_DESC = "PosTest1: primitive type int ";

        bool expectedValue = true;
        bool actualValue = false;
        string errorDesc;

        Type testType = typeof(int);

        TestFramework.BeginScenario(c_TEST_DESC);
        try
        {
            string typeName = testType.ToString();
            actualValue = (typeName == "System.Int32");
            if (actualValue != expectedValue)
            {
                errorDesc = "Value is not " + expectedValue + " as expected: Actual(" + actualValue + ")";
                //errorDesc += GetDataString(strA, indexA, strB, indexB, length, comparisonType);
                TestFramework.LogError("001" + " TestId-" + c_TEST_ID, errorDesc);
                retVal = false;
            }
        }
        catch(Exception e)
        {
            errorDesc = "Unexpected exception: " + e;
            TestFramework.LogError("002" + " TestId-" + c_TEST_ID, errorDesc);
            retVal = false;
        }

        return retVal;
    }

    public bool PosTest2()
    {
        bool retVal = true;

        const string c_TEST_ID = "P002";
        const string c_TEST_DESC = "PosTest2: primitive type string ";

        bool expectedValue = true;
        bool actualValue = false;
        string errorDesc;

        Type testType = typeof(string);

        TestFramework.BeginScenario(c_TEST_DESC);
        try
        {
            string typeName = testType.ToString();
            actualValue = (typeName == "System.String");
            if (actualValue != expectedValue)
            {
                errorDesc = "Value is not " + expectedValue + " as expected: Actual(" + actualValue + ")";
                //errorDesc += GetDataString(strA, indexA, strB, indexB, length, comparisonType);
                TestFramework.LogError("003" + " TestId-" + c_TEST_ID, errorDesc);
                retVal = false;
            }
        }
        catch (Exception e)
        {
            errorDesc = "Unexpected exception: " + e;
            TestFramework.LogError("004" + " TestId-" + c_TEST_ID, errorDesc);
            retVal = false;
        }

        return retVal;
    }

    public bool PosTest3()
    {
        bool retVal = true;

        const string c_TEST_ID = "P003";
        const string c_TEST_DESC = "PosTest3: primitive type int[] ";

        bool expectedValue = true;
        bool actualValue = false;
        string errorDesc;

        Type testType = typeof(int[]);

        TestFramework.BeginScenario(c_TEST_DESC);
        try
        {
            string typeName = testType.ToString();
            actualValue = (typeName == "System.Int32[]");
            if (actualValue != expectedValue)
            {
                errorDesc = "Value is not " + expectedValue + " as expected: Actual(" + actualValue + ")";
                //errorDesc += GetDataString(strA, indexA, strB, indexB, length, comparisonType);
                TestFramework.LogError("005" + " TestId-" + c_TEST_ID, errorDesc);
                retVal = false;
            }
        }
        catch (Exception e)
        {
            errorDesc = "Unexpected exception: " + e;
            TestFramework.LogError("006" + " TestId-" + c_TEST_ID, errorDesc);
            retVal = false;
        }

        return retVal;
    }

    public bool PosTest4()
    {
        bool retVal = true;

        const string c_TEST_ID = "P004";
        const string c_TEST_DESC = "PosTest4: generic type List<string> ";

        bool expectedValue = true;
        bool actualValue = false;
        string errorDesc;

        Type testType = typeof(List<string>);

        TestFramework.BeginScenario(c_TEST_DESC);
        try
        {
            string typeName = testType.ToString();
            actualValue = (typeName == "System.Collections.Generic.List`1[System.String]");
            if (actualValue != expectedValue)
            {
                errorDesc = "Value is not " + expectedValue + " as expected: Actual(" + actualValue + ")";
                //errorDesc += GetDataString(strA, indexA, strB, indexB, length, comparisonType);
                TestFramework.LogError("007" + " TestId-" + c_TEST_ID, errorDesc);
                retVal = false;
            }
        }
        catch (Exception e)
        {
            errorDesc = "Unexpected exception: " + e;
            TestFramework.LogError("008" + " TestId-" + c_TEST_ID, errorDesc);
            retVal = false;
        }

        return retVal;
    }

    public bool PosTest5()
    {
        bool retVal = true;

        const string c_TEST_ID = "P005";
        const string c_TEST_DESC = "PosTest5: generic type with multiple type parameters ";

        bool expectedValue = true;
        bool actualValue = false;
        string errorDesc;

        Type testType = typeof(Foo<List<int>, string>);

        TestFramework.BeginScenario(c_TEST_DESC);
        try
        {
            string typeName = testType.ToString();
            actualValue = (typeName == "Foo`2[System.Collections.Generic.List`1[System.Int32],System.String]");
            if (actualValue != expectedValue)
            {
                errorDesc = "Value is not " + expectedValue + " as expected: Actual(" + actualValue + ")";
                //errorDesc += GetDataString(strA, indexA, strB, indexB, length, comparisonType);
                TestFramework.LogError("09" + " TestId-" + c_TEST_ID, errorDesc);
                retVal = false;
            }
        }
        catch (Exception e)
        {
            errorDesc = "Unexpected exception: " + e;
            TestFramework.LogError("010" + " TestId-" + c_TEST_ID, errorDesc);
            retVal = false;
        }

        return retVal;
    }

    public bool PosTest6() //maybe bug
    {
        bool retVal = true;

        const string c_TEST_ID = "P006";
        const string c_TEST_DESC = "PosTest6: type parameter of generic type ";

        bool expectedValue = true;
        bool actualValue = false;
        string errorDesc;

        TestFramework.BeginScenario(c_TEST_DESC);
        try
        {
            string typeName = MyClass<ClassA>.TypeParameterToString();
            actualValue = (typeName == "ClassA");
            if (actualValue != expectedValue)
            {
                errorDesc = "Value is not " + expectedValue + " as expected: Actual(" + actualValue + ")";
                //errorDesc += GetDataString(strA, indexA, strB, indexB, length, comparisonType);
                TestFramework.LogError("011" + " TestId-" + c_TEST_ID, errorDesc);
                retVal = false;
            }
        }
        catch (Exception e)
        {
            errorDesc = "Unexpected exception: " + e;
            TestFramework.LogError("012" + " TestId-" + c_TEST_ID, errorDesc);
            retVal = false;
        }

        return retVal;
    }
}

#region helper class
// namespace TestHelper
// {
    internal class Foo<T1, T2>
    {
    }

    internal class MyClass<T>
    {
        public static string TypeParameterToString()
        {
            Type type = typeof(T);
            string typeName = type.ToString();
            return typeName;
        }
    }

    internal class ClassA
    {
        public static string m_name = "ClassA";
    }
// }
#endregion
""";
            await RunTest(code);
        }
    }
}
