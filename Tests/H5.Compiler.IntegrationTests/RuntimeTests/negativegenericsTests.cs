using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class negativegenericsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task negativegenerics_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/CoreMangLib/system/delegate/generics/negativegenerics.cs
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
//Covers various negative binding cases for delegates and generics...
using System;
// using Xunit;


//Define some classes and method for use in our scenarios...
class A<T>{
	public virtual void GMeth<U>(){}
}

class B<T> : A<int>{
	public override void GMeth<U>(){}
}

//Define our delegate types...
delegate void Closed();
delegate void Open(B<int> b);
delegate void GClosed<T>();

public class Test_negativegenerics{
	public static int retVal=100;

	// [OuterLoop]
	// [Fact]
	public static int TestEntryPoint(){
		//Try to create an open-instance delegate to a virtual generic method (@TODO - Need early bound case here too)
		//Try to create a generic delegate of a non-instantiated type
		//Try to create a delegate over a non-instantiated target type
		//Try to create a delegate over a non-instantiated target method
		//Try to create a delegate to a generic method by name

		//Does Closed() over GMeth<int> == Closed() over GMeth<double>??
		//Does GClosed<int>() over GMeth<int> == GClosed<double>() over GMeth<int>??

		Console.WriteLine("Done - {0}",retVal==100?"Passed":"Failed");
		return retVal;
	}
}


public class Program
{
    public static int Main()
    {
        try {
            return A.TestEntryPoint();
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
