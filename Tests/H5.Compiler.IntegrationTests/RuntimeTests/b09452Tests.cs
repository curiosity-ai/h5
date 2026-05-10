using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class b09452Tests : IntegrationTestBase
    {
        [TestMethod]
        public async Task b09452_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Regression/CLR-x86-JIT/V1-M10/b09452/b09452.cs
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

// using Xunit;
// namespace b09452
{
    //@BEGINRENAME; Verify this renames
    //@ENDRENAME; Verify this renames
    using System;

    public class X
    {
        // [OuterLoop]
        // [Fact]
        public static void TestEntryPoint()
        {
            Object[,] obj = new Object[1, 1];
            //			IL_0000:  ldc.i4.1
            //    		IL_0001:  ldc.i4.1
            // 	 		IL_0002:  newobj instance void class System.Object[,]::.ctor(int32,int32)
            // 			IL_0007:  stloc.0

            obj[0, 0] = new Object();
            //        	IL_0008:  ldloc.0
            //    		IL_0009:  ldc.i4.0
            //    		IL_000a:  ldc.i4.0
            //    		IL_000b:  newobj instance void System.Object::.ctor()
            //   	 	IL_0010:  call instance void class System.Object[,]::Set(int32,int32,class System.Object)

            //    		IL_0015:  ret

        } // main

    } // X

// }


public class Program
{
    public static int Main()
    {
        try {
            return X.TestEntryPoint();
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
