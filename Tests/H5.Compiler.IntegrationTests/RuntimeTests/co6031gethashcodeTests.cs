using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class co6031gethashcodeTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task co6031gethashcode_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/CoreMangLib/system/delegate/miscellaneous/co6031gethashcode.cs
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

delegate int Int32_VoidDelegate();
public class Co6031GetHashCode
{
	// [OuterLoop]
	// [Fact]
	public static int TestEntryPoint()
	{
		int iTestCount= 0;
		int iErrorCount= 0;
		String LocStr = "loc_000";
		int iExitCode = 101; //101: fail; 100: pass
		try
		{
			//The return value of Delegate.GetHashcode must not be persisted for two reasons.
			//First, the hash function of a class might be altered to generate a better
			//distribution, rendering any values from the old hash function useless.
			//Second, the default implementation of this class does not guarantee that
			//the same value will be returned by different instances.


			{
				LocStr = "loc_001";
				iTestCount++;
				Console.WriteLine( "test1: GetHashCode of delegate pointing to static method" );
				Int32_VoidDelegate sdg1 = new Int32_VoidDelegate( staticMethInt32_Void );

				int ihc1 = sdg1.GetHashCode();
				int ihc2 = sdg1.GetHashCode();
				if( ihc2 != ihc1 )
				{
					Console.WriteLine( "Err_001 : should be equal, but one is :" +  ihc1 + " other one is " + ihc2);
					iErrorCount++;
				}
			}

			{
				LocStr = "loc_002";
				iTestCount++;
				Console.WriteLine( "test1: GetHashCode of delegate pointing to instance method " );
				Co6031GetHashCode obj = new Co6031GetHashCode();
				Int32_VoidDelegate sdg2 = new Int32_VoidDelegate( obj.instanceMethInt32_Void );

				int ihc1 = sdg2.GetHashCode();
				int ihc2 = sdg2.GetHashCode();
				if( ihc2 != ihc1 )
				{
					Console.WriteLine( "Err_002 : should be equal, but one is :" +  ihc1 + " other one is " + ihc2);
					iErrorCount++;
				}
			}
		}
		catch( Exception e )
		{
			Console.WriteLine( LocStr + " : unexpected " + e.ToString() );
			iErrorCount++;
		}

		if( iErrorCount > 0)
		{
			Console.WriteLine( "Total tests count" +  iTestCount + " . Failed tests count" + iErrorCount);
			iExitCode = 101;
		}
		else
		{
			Console.WriteLine( "Total tests count" +  iTestCount + " . All passed" );
			iExitCode = 100;
		}
		return iExitCode;
	}

	public static int staticMethInt32_Void()
	{
		Console.WriteLine( "Invoked staticMethVoid_Void method");
		return 77;
	}

	public int instanceMethInt32_Void()
	{
		Console.WriteLine( "Invoked staticMethVoid_Void1 method");
		return 66;
	}
}



public class Program
{
    public static int Main()
    {
        try {
            return Co6031GetHashCode.TestEntryPoint();
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
