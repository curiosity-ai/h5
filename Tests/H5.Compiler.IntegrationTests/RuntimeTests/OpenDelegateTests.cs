using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class OpenDelegateTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task OpenDelegate_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/CoreMangLib/system/delegate/VSD/OpenDelegate.cs
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
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Collections.Generic;
// using Xunit;

public class OpenDelegate
{
    public class ClassA
    {
        public virtual int PublicInstanceMethod() { return 17; }
    }

    public delegate int Delegate_TC_Int(ClassA tc);
    public static MethodInfo GetMethod([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] Type t, string method)
    {
        TypeInfo typeInfo = t.GetTypeInfo();
        IEnumerator<MethodInfo> enumerator = typeInfo.DeclaredMethods.GetEnumerator();
        MethodInfo result = null;
        while (enumerator.MoveNext())
        {
            bool flag = enumerator.Current.Name.Equals(method);
            if (flag)
            {
                result = enumerator.Current;
                break;
            }
        }
        return result;
    }
    // [OuterLoop]
    // [Fact]
    public static int TestEntryPoint()
    {
        Type typeTestClass = typeof(ClassA);
        ClassA TestClass = (ClassA)Activator.CreateInstance(typeTestClass);
        MethodInfo miPublicInstanceMethod = GetMethod(typeTestClass, "PublicInstanceMethod");
        Delegate dlgt = miPublicInstanceMethod.CreateDelegate(typeof(Delegate_TC_Int));
        Object retValue = ((Delegate_TC_Int)dlgt).DynamicInvoke(new Object[] { TestClass });

        if(retValue.Equals(TestClass.PublicInstanceMethod()))
        {
            return 100;
        }


        return -1;

    }

}


public class Program
{
    public static int Main()
    {
        try {
            return OpenDelegate.TestEntryPoint();
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
