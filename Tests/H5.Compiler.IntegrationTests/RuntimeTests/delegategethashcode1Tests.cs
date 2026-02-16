using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class delegategethashcode1Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task delegategethashcode1_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/CoreMangLib/system/delegate/delegate/delegategethashcode1.cs
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
using System.Globalization;
// using Xunit;
//test case for delegate GetHashCode method.
// namespace DelegateGetHashCodeTest
{
    delegate bool booldelegate();
    delegate void voiddelegate();
    delegate bool booldelegate1();
    delegate bool booldelegate2(string str);
    public class DelegateGetHashCode
    {


        // [OuterLoop]
        // [Fact]
        public static int TestEntryPoint()
        {
            DelegateGetHashCode DelegateGetHashCode = new DelegateGetHashCode();

            TestFramework.BeginTestCase("DelegateGetHashCode");

            if (DelegateGetHashCode.RunTests())
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
            retVal = PosTest7() && retVal;
            retVal = PosTest8() && retVal;
            return retVal;
        }

        // Returns true if the expected result is right
        // Returns false if the expected result is wrong
        // one delegate object  is booldelegate
        // the other is voiddelegate
        public bool PosTest1()
        {
            bool retVal = true;
            TestFramework.BeginScenario("PosTest1: hash code of two different delegate object is not equal,the two delegate callback different function. ");

            try
            {
                DelegateGetHashCode delctor = new DelegateGetHashCode();
                booldelegate workDelegate = new booldelegate(new DelegateGetHashCodeTestClass(1).StartWork_Bool );
                voiddelegate workDelegate1 = new voiddelegate(new DelegateGetHashCodeTestClass(1).StartWork_Void);
                if (workDelegate.GetHashCode() == workDelegate1.GetHashCode())
                {
                    TestFramework.LogError("001", "HashCode is not excepted ");
                    retVal = false;
                }

                workDelegate();
                workDelegate1();

            }
            catch (Exception e)
            {
                TestFramework.LogError("002", "Unexpected exception: " + e);
                retVal = false;
            }

            return retVal;
        }
        // Returns true if the expected result is right
        // Returns false if the expected result is wrong
        // one delegate object  is booldelegate
        // the other is booldelegate1
        public bool PosTest2()
        {
            bool retVal = true;
            //Type,target, method, and invocation list
            TestFramework.BeginScenario("PosTest2: hash code of two different delegate object even though  they invoke the same function  is not equal ");

            try
            {
                DelegateGetHashCode delctor = new DelegateGetHashCode();
                booldelegate workDelegate = new booldelegate(new DelegateGetHashCodeTestClass(1).StartWork_Bool);
                booldelegate1 workDelegate1 = new booldelegate1(new DelegateGetHashCodeTestClass(1).StartWork_Bool);
                if (workDelegate.GetHashCode() == workDelegate1.GetHashCode())
                {
                    TestFramework.LogError("003", "HashCode is not excepted ");
                    retVal = false;
                }

                workDelegate();
                workDelegate1();

            }
            catch (Exception e)
            {
                TestFramework.LogError("004", "Unexpected exception: " + e);
                retVal = false;
            }

            return retVal;
        }
        // Returns true if the expected result is right
        // Returns false if the expected result is wrong
        // the same delegate object  is booldelegate
        public bool PosTest3()
        {
            bool retVal = true;

            TestFramework.BeginScenario("PosTest3: Use the same type's same  method to create two delegate which delegate object is the same,their hashcode is equal");

            try
            {
                DelegateGetHashCode delctor = new DelegateGetHashCode();
                booldelegate workDelegate = new booldelegate(DelegateGetHashCodeTestClass.Working_Bool);
                booldelegate workDelegate1 = new booldelegate(DelegateGetHashCodeTestClass.Working_Bool);
                if (workDelegate.GetHashCode() != workDelegate1.GetHashCode())
                {
                    TestFramework.LogError("005", "HashCode is not excepted ");
                    retVal = false;
                }

                workDelegate();
                workDelegate1();

            }
            catch (Exception e)
            {
                TestFramework.LogError("006", "Unexpected exception: " + e);
                retVal = false;
            }

            return retVal;
        }
        // Returns true if the expected result is right
        // Returns false if the expected result is wrong
        // the same delegate object  is booldelegate
        public bool PosTest7()
        {
            bool retVal = true;

            TestFramework.BeginScenario("PosTest7:  Use the different instance's same instance method to create two delegate which delegate object is the same, their hashcode is different");

            try
            {
                DelegateGetHashCode delctor = new DelegateGetHashCode();
                booldelegate workDelegate = new booldelegate(new DelegateGetHashCodeTestClass(1).StartWork_Bool);
                booldelegate workDelegate1 = new booldelegate(new DelegateGetHashCodeTestClass1(2).StartWork_Bool );

                if (workDelegate.GetHashCode()==workDelegate1.GetHashCode())
                {
                    TestFramework.LogError("013", "HashCode is not excepted ");
                    retVal = false;
                }

                workDelegate();
                workDelegate1();

            }
            catch (Exception e)
            {
                TestFramework.LogError("014", "Unexpected exception: " + e);
                retVal = false;
            }

            return retVal;
        }
        // Returns true if the expected result is right
        // Returns false if the expected result is wrong
        // one delegate object  is booldelegate
        // the other is booldelegate2
        public bool PosTest8()
        {
            bool retVal = true;
            //Type,target, method, and invocation list
            TestFramework.BeginScenario("PosTest8: hash code of two delegate object is not equal,the two delegate callback different function. ");

            try
            {
                DelegateGetHashCode delctor = new DelegateGetHashCode();
                booldelegate workDelegate = new booldelegate(new DelegateGetHashCodeTestClass(1).StartWork_Bool);
                booldelegate2 workDelegate1 = new booldelegate2(new DelegateGetHashCodeTestClass(1).StartWork_Bool);
                if (workDelegate.GetHashCode() == workDelegate1.GetHashCode())
                {
                    TestFramework.LogError("015", "HashCode is not excepted ");
                    retVal = false;
                }

                workDelegate();
                workDelegate1("hello");

            }
            catch (Exception e)
            {
                TestFramework.LogError("016", "Unexpected exception: " + e);
                retVal = false;
            }

            return retVal;
        }

    }
    //create testclass for providing test method and test target.
    class DelegateGetHashCodeTestClass
    {
        private int id;
        public DelegateGetHashCodeTestClass(int id) { this.id = id; }
        public bool StartWork_Bool()
        {
            TestFramework.LogInformation("DelegateGetHashCodeTestClass's StartWork_Bool method  is running. id="+this.id);
            return true;
        }
        public bool StartWork_Bool(string str)
        {
            TestFramework.LogInformation("DelegateGetHashCodeTestClass's StartWork_Bool method  is running. id=" + this.id +" "+ "message=" + str);
            return true;
        }
        public static  bool Working_Bool()
        {
            TestFramework.LogInformation("DelegateGetHashCodeTestClass's Working_Bool method  is running .");
            return true;
        }
        public static bool Completed_Bool()
        {
            TestFramework.LogInformation("DelegateGetHashCodeTestClass's Completed_Bool method  is running .");
            return true;
        }
        public void StartWork_Void()
        {
            TestFramework.LogInformation("DelegateGetHashCodeTestClass1's StartWork_Bool method  is running. id=" + this.id);

        }
    }
    class DelegateGetHashCodeTestClass1
    {
        private int id;
        public DelegateGetHashCodeTestClass1(int id) { this.id = id; }
        public bool StartWork_Bool()
        {
            TestFramework.LogInformation("DelegateGetHashCodeTestClass1's StartWork_Bool method  is running. id="+ this.id  );
            return true;
        }

        public static bool Working_Bool()
        {
            TestFramework.LogInformation("DelegateGetHashCodeTestClass1's Working_Bool method  is running .");
            return true;
        }
        public static bool Completed_Bool()
        {
            TestFramework.LogInformation("DelegateGetHashCodeTestClass1's Completed_Bool method  is running .");
            return true;
        }
    }


// }


public class Program
{
    public static int Main()
    {
        try {
            return DelegateGetHashCode.TestEntryPoint();
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
