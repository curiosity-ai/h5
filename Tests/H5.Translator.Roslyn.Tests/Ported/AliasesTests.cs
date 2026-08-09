using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class AliasesTests : TranslatorTestBase
    {
        [TestMethod]
        public async Task AliasDisambiguation()
        {
            var code = """
using System;
using A;
using B;

namespace A
{
  public class Test { public string Name => "A"; }
}

namespace B
{
  public class Test { public string Name => "B"; }
}

namespace C
{
   using A;
   using B;
   using Test = A.Test;

   public class Program
   {
       public static void Main()
       {
           Console.WriteLine(GetTestA().Name);
           Console.WriteLine(GetTestB().Name);
           Console.WriteLine(GetTest().Name);
       }

       public static A.Test GetTestA() => new A.Test();
       public static B.Test GetTestB() => new B.Test();
       public static Test GetTest() => new Test();

   }
}
""";
            // The output should be "B" because GetTest() returns Test which is aliased to B.Test
            var output = await RunTest(code, skipRoslyn: true);
            Assert.AreEqual("A\nB\nA", output.Trim());
        }

        [TestMethod]
        public async Task IntArrayAliasTest()
        {
            var code = """
using System;
using IntArray = int[];

public class Program
{
    public static void Main()
    {
        var realArr  = new int[] { 1, 2, 3 };
        IntArray arr = new int[] { 1, 2, 3 };
        Console.WriteLine(realArr.Length);
        Console.WriteLine(realArr[0]);
        Console.WriteLine(arr.Length);
        Console.WriteLine(arr[0]);
    }
}
""";
            var output = await RunTest(code, skipRoslyn: true);
            Assert.AreEqual("3\n1\n3\n1", output.Trim());
        }
    }
}
