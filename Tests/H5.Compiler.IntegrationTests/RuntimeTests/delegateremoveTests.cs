using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class delegateremoveTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task delegateremove_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/CoreMangLib/system/delegate/delegate/delegateremove.cs
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
//test case for delegate Remove(System.Delegate,System.Delegate) method.
// namespace DelegateRemoveTest
{
    delegate bool booldelegate();
    delegate void voiddelegate();
    public class DelegateRemove
    {

        booldelegate starkWork;

        // [OuterLoop]
        // [Fact]
        public static int TestEntryPoint()
        {
            DelegateRemove delegateRemoveImpl = new DelegateRemove();

            TestFramework.BeginTestCase("DelegateRemove");

            if (delegateRemoveImpl.RunTests())
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
            retVal = PosTest5() && retVal;//static method
            TestFramework.LogInformation("[Positive]");
            retVal = NegTest1() && retVal;
            return retVal;
        }

        // Returns true if the expected result is right
        // Returns false if the expected result is wrong
        public bool PosTest1()
        {
            bool retVal = true;

            TestFramework.BeginScenario("PosTest1: Remove a function from the delegate which contains only 1 callback function");
            try
            {
                DelegateRemove delctor = new DelegateRemove();
                DelegateRemoveTestClass tcInstance = new DelegateRemoveTestClass();
                delctor.starkWork = new booldelegate(tcInstance.StartWork_Bool);
                delctor.starkWork=(booldelegate)Delegate.Remove(delctor.starkWork, new booldelegate(tcInstance.StartWork_Bool));
                if (null != delctor.starkWork)
                {
                    TestFramework.LogError("001", "remove failure  " );
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
        // Returns true if the expected result is right
        // Returns false if the expected result is wrong
        public bool PosTest2()
        {
            bool retVal = true;
            TestFramework.BeginScenario("PosTest2: Remove a function which is in the InvocationList");
            try
            {
                DelegateRemove delctor = new DelegateRemove();
                DelegateRemoveTestClass tcInstance = new DelegateRemoveTestClass();
		booldelegate bStartWork_Bool = new booldelegate(tcInstance.StartWork_Bool);
		booldelegate bWorking_Bool   = new booldelegate(tcInstance.Working_Bool);
		booldelegate bCompleted_Bool = new booldelegate(tcInstance.Completed_Bool);

                delctor.starkWork += bStartWork_Bool;
                delctor.starkWork += bWorking_Bool;
                delctor.starkWork += bCompleted_Bool;
                delctor.starkWork = (booldelegate)Delegate.Remove(delctor.starkWork, new booldelegate(tcInstance.Working_Bool));
                Delegate[] invocationList = delctor.starkWork.GetInvocationList();
                if (invocationList.Length != 2)
                {
                    TestFramework.LogError("003", "remove failure or remove method is not in the InvocationList");
                    retVal = false;
                }
                if (!delctor.starkWork.GetInvocationList()[0].Equals(bStartWork_Bool)
                    || !delctor.starkWork.GetInvocationList()[1].Equals(bCompleted_Bool))
                {
                    TestFramework.LogError("004", " remove failure ");
                    retVal = false;
                }
                delctor.starkWork();
            }
            catch (Exception e)
            {
                TestFramework.LogError("005", "Unexpected exception: " + e);
                retVal = false;
            }

            return retVal;
        }
        // Returns true if the expected result is right
        // Returns false if the expected result is wrong
        public bool PosTest3()
        {
            bool retVal = true;

            TestFramework.BeginScenario("PosTest3: Remove a function which is not in the InvocationList");
            try
            {
                DelegateRemove delctor = new DelegateRemove();
		booldelegate bStartWork_Bool = new booldelegate(new DelegateRemoveTestClass().StartWork_Bool);
		booldelegate bWorking_Bool   = new booldelegate(new DelegateRemoveTestClass().Working_Bool);
		booldelegate bCompleted_Bool = new booldelegate(new DelegateRemoveTestClass().Completed_Bool);

                delctor.starkWork += bStartWork_Bool;
                delctor.starkWork += null;
                delctor.starkWork += bWorking_Bool;
                delctor.starkWork += bCompleted_Bool;
                delctor.starkWork = (booldelegate)Delegate.Remove(delctor.starkWork, new booldelegate(new DelegateRemoveTestClass().Completed_Bool));
                Delegate[] invocationList = delctor.starkWork.GetInvocationList();
                if (invocationList.Length != 3)
                {
                    TestFramework.LogError("006", "Call GetInvocationList against a delegate with one function returns wrong result: " + invocationList.Length);
                    retVal = false;
                }
                if (!delctor.starkWork.GetInvocationList()[0].Equals(bStartWork_Bool)
                    || !delctor.starkWork.GetInvocationList()[1].Equals(bWorking_Bool)
                    || !delctor.starkWork.GetInvocationList()[2].Equals(bCompleted_Bool))
                {
                    TestFramework.LogError("007", " remove failure ");
                    retVal = false;
                }
                delctor.starkWork();
            }
            catch (Exception e)
            {
                TestFramework.LogError("008", "Unexpected exception: " + e);
                retVal = false;
            }

            return retVal;
        }

        // Returns true if the expected result is right
        // Returns false if the expected result is wrong
        public bool PosTest4()
        {
            bool retVal = true;

            TestFramework.BeginScenario("PosTest4: Remove a function which is in the InvocationList and not only one method");
            try
            {
                DelegateRemove delctor = new DelegateRemove();
                DelegateRemoveTestClass tcInstance = new DelegateRemoveTestClass();
		booldelegate bStartWork_Bool = new booldelegate(tcInstance.StartWork_Bool);
		booldelegate bWorking_Bool   = new booldelegate(tcInstance.Working_Bool);
		booldelegate bCompleted_Bool = new booldelegate(tcInstance.Completed_Bool);

                delctor.starkWork += bStartWork_Bool;
                delctor.starkWork += bStartWork_Bool;
                delctor.starkWork += bWorking_Bool;
                delctor.starkWork += bCompleted_Bool;
                delctor.starkWork = (booldelegate)Delegate.Remove(delctor.starkWork, new booldelegate(tcInstance.StartWork_Bool));
                Delegate[] invocationList = delctor.starkWork.GetInvocationList();
                if (invocationList.Length !=3)
                {
                    TestFramework.LogError("009", "remove failure: " + invocationList.Length);
                    retVal = false;
                }
                if (!delctor.starkWork.GetInvocationList()[0].Equals(bStartWork_Bool)
                    || !delctor.starkWork.GetInvocationList()[1].Equals(bWorking_Bool)
                    || !delctor.starkWork.GetInvocationList()[2].Equals(bCompleted_Bool))
                {
                    TestFramework.LogError("010", " remove failure ");
                    retVal = false;
                }
                delctor.starkWork();
            }
            catch (Exception e)
            {
                TestFramework.LogError("011", "Unexpected exception: " + e);
                retVal = false;
            }

            return retVal;
        }
        // Returns true if the expected result is right
        // Returns false if the expected result is wrong
        public bool PosTest5()
        {
            bool retVal = true;

            TestFramework.BeginScenario("PosTest5: Remove a function which is in the InvocationList and not only one method ,method is static method");
            try
            {
                DelegateRemove delctor = new DelegateRemove();
		booldelegate bStartWork_Bool = new booldelegate(DelegateRemoveTestClass1.StartWork_Bool);
		booldelegate bWorking_Bool   = new booldelegate(DelegateRemoveTestClass1.Working_Bool);
		booldelegate bCompleted_Bool = new booldelegate(DelegateRemoveTestClass1.Completed_Bool);

                delctor.starkWork += bStartWork_Bool;
                delctor.starkWork += bStartWork_Bool;
                delctor.starkWork += bWorking_Bool;
                delctor.starkWork += bCompleted_Bool;
                delctor.starkWork = (booldelegate)Delegate.Remove(delctor.starkWork, new booldelegate(DelegateRemoveTestClass1.StartWork_Bool));
                Delegate[] invocationList = delctor.starkWork.GetInvocationList();
                if (invocationList.Length != 3)
                {
                    TestFramework.LogError("012", "remove failure: " + invocationList.Length);
                    retVal = false;
                }
                if (!delctor.starkWork.GetInvocationList()[0].Equals(bStartWork_Bool)
                    || !delctor.starkWork.GetInvocationList()[1].Equals(bWorking_Bool)
                    || !delctor.starkWork.GetInvocationList()[2].Equals(bCompleted_Bool))
                {
                    TestFramework.LogError("013", " remove failure ");
                    retVal = false;
                }
                delctor.starkWork();
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
        public bool NegTest1()
        {
            bool retVal = true;
            TestFramework.BeginScenario("NegTest1: The delegate types do not match. ");

            try
            {
                DelegateRemove delctor = new DelegateRemove();
                DelegateRemoveTestClass tcInstance = new DelegateRemoveTestClass();
		booldelegate bStartWork_Bool = new booldelegate(tcInstance.StartWork_Bool);
		booldelegate bWorking_Bool   = new booldelegate(tcInstance.Working_Bool);
		booldelegate bCompleted_Bool = new booldelegate(tcInstance.Completed_Bool);

                delctor.starkWork += bStartWork_Bool;
                delctor.starkWork += bWorking_Bool;
                delctor.starkWork += bCompleted_Bool;
                delctor.starkWork = (booldelegate)Delegate.Remove(delctor.starkWork, new voiddelegate(tcInstance.StartWork_Void));

                TestFramework.LogError("015", "delegate remove error ");
                retVal = false;
            }
            catch (ArgumentException)
            {

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
    class DelegateRemoveTestClass
    {
        public bool StartWork_Bool()
        {
            TestFramework.LogInformation("StartWork_Bool method  is running .");
            return true;
        }
        public void StartWork_Void()
        {
            TestFramework.LogInformation("StartWork_Void method  is running .");

        }
        public bool Working_Bool()
        {
            TestFramework.LogInformation("Working_Bool method  is running .");
            return true;
        }
        public bool Completed_Bool()
        {
            TestFramework.LogInformation("Completed_Bool method  is running .");
            return true;
        }
    }
    class DelegateRemoveTestClass1
    {
        public static bool StartWork_Bool()
        {
            TestFramework.LogInformation("StartWork_Bool method  is running .");
            return true;
        }
        public static bool Working_Bool()
        {
            TestFramework.LogInformation("Working_Bool method  is running .");
            return true;
        }
        public static bool Completed_Bool()
        {
            TestFramework.LogInformation("Completed_Bool method  is running .");
            return true;
        }
    }
// }


public class Program
{
    public static int Main()
    {
        try {
            return DelegateRemove.TestEntryPoint();
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
