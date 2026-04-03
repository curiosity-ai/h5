using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class AdvancedExtensionMethodsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task ExtensionOnGenericArray()
        {
            var code = @"
using System;
using System.Linq;

public static class Extensions
{
    public static void PrintArray<T>(this T[] array)
    {
        Console.WriteLine(string.Join("","", array));
    }
}

public class Program
{
    public static void Main()
    {
        int[] numbers = { 1, 2, 3 };
        numbers.PrintArray();
    }
}
";
            var output = await RunTest(code, skipRoslyn: true);
            Assert.AreEqual("1,2,3", output.Trim());
        }

        [TestMethod]
        public async Task ExtensionOnGenericList()
        {
            var code = @"
using System;
using System.Collections.Generic;

public static class Extensions
{
    public static void AddRangeCustom<T>(this List<T> list, params T[] items)
    {
        foreach(var item in items) list.Add(item);
    }
}

public class Program
{
    public static void Main()
    {
        var list = new List<string>();
        list.AddRangeCustom(""A"", ""B"");
        Console.WriteLine(string.Join("","", list));
    }
}
";
            var output = await RunTest(code, skipRoslyn: true);
            Assert.AreEqual("A,B", output.Trim());
        }

        [TestMethod]
        public async Task ExtensionOnGenericDictionary()
        {
            var code = @"
using System;
using System.Collections.Generic;

public static class Extensions
{
    public static void PrintKeys<K,V>(this Dictionary<K,V> dict)
    {
        foreach(var key in dict.Keys) Console.WriteLine(key);
    }
}

public class Program
{
    public static void Main()
    {
        var dict = new Dictionary<int, string> { { 1, ""One"" }, { 2, ""Two"" } };
        dict.PrintKeys();
    }
}
";
            var output = await RunTest(code, skipRoslyn: true);
            var expected = "1\n2"; // Order might vary in dictionaries but for small int keys usually insertion/numeric order
            // Assuming 1 then 2. If flaky, we'll fix.
            Assert.AreEqual(expected, output.Trim());
        }

        [TestMethod]
        public async Task ExtensionOnNestedGenericType()
        {
            var code = @"
using System;

public class Outer<T>
{
    public class Inner<U>
    {
        public U Val;
        public Inner(U val) { Val = val; }
    }
}

public static class Extensions
{
    public static void Dump<T, U>(this Outer<T>.Inner<U> inner)
    {
        Console.WriteLine(inner.Val);
    }
}

public class Program
{
    public static void Main()
    {
        var inner = new Outer<int>.Inner<string>(""Nested"");
        inner.Dump();
    }
}
";
            var output = await RunTest(code, skipRoslyn: true);
            Assert.AreEqual("Nested", output.Trim());
        }

        [TestMethod]
        public async Task GenericExtensionWithConstraints()
        {
            var code = @"
using System;

public interface IHaveId { int Id { get; } }
public class Item : IHaveId { public int Id => 99; }

public static class Extensions
{
    public static void PrintId<T>(this T item) where T : IHaveId
    {
        Console.WriteLine(item.Id);
    }
}

public class Program
{
    public static void Main()
    {
        var item = new Item();
        item.PrintId();
    }
}
";
            var output = await RunTest(code, skipRoslyn: true);
            Assert.AreEqual("99", output.Trim());
        }

        [TestMethod]
        public async Task ExtensionResolutionPriority()
        {
            // Instance method vs Extension method: Instance wins
            var code = @"
using System;

public class Box
{
    public void Method() => Console.WriteLine(""Instance"");
}

public static class Extensions
{
    public static void Method(this Box b) => Console.WriteLine(""Extension"");
}

public class Program
{
    public static void Main()
    {
        new Box().Method();
    }
}
";
            var output = await RunTest(code, skipRoslyn: true);
            Assert.AreEqual("Instance", output.Trim());
        }

        [TestMethod]
        public async Task GenericClassNestedGenericClass()
        {
            var code = @"
using System;

public class Outer<T>
{
    public T OuterVal;
    public Outer(T val) { OuterVal = val; }

    public class Inner<U>
    {
        public U InnerVal;
        public Inner(U val) { InnerVal = val; }
        public void Print(Outer<T> outer) => Console.WriteLine(outer.OuterVal + ""-"" + InnerVal);
    }
}

public class Program
{
    public static void Main()
    {
        var outer = new Outer<string>(""Out"");
        var inner = new Outer<string>.Inner<int>(123);
        inner.Print(outer);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericClassNestedNonGenericClass()
        {
            var code = @"
using System;

public class Outer<T>
{
    public class Inner
    {
        public void M(T t) => Console.WriteLine(""Inner "" + t);
    }
}

public class Program
{
    public static void Main()
    {
        var inner = new Outer<string>.Inner();
        inner.M(""Hello"");
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NonGenericClassNestedGenericClass()
        {
            var code = @"
using System;

public class Outer
{
    public class Inner<T>
    {
        public T Val;
        public Inner(T val) { Val = val; }
    }
}

public class Program
{
    public static void Main()
    {
        var i = new Outer.Inner<double>(3.14);
        Console.WriteLine(i.Val);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NestedClassInheritingOuter()
        {
            var code = @"
using System;

public class Outer<T>
{
    public T Val;

    public class Inner : Outer<T>
    {
        public void Set(T val) { Val = val; }
    }
}

public class Program
{
    public static void Main()
    {
        var i = new Outer<int>.Inner();
        i.Set(55);
        Console.WriteLine(i.Val);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NestedClassImplementingOuterInterface()
        {
            var code = @"
using System;

public interface IBase<T> { T Get(); }

public class Outer<T>
{
    public class Inner : IBase<T>
    {
        public T _val;
        public Inner(T val) { _val = val; }
        public T Get() => _val;
    }
}

public class Program
{
    public static void Main()
    {
        var i = new Outer<string>.Inner(""NestedInterface"");
        Console.WriteLine(i.Get());
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task DeepNestedGenerics()
        {
            var code = @"
using System;

public class A<T>
{
    public class B<U>
    {
        public class C<V>
        {
            public void Print(T t, U u, V v) => Console.WriteLine($""{t}-{u}-{v}"");
        }
    }
}

public class Program
{
    public static void Main()
    {
        var c = new A<int>.B<string>.C<bool>();
        c.Print(1, ""Deep"", true);
    }
}
";
            var output = await RunTest(code, skipRoslyn: true);
            Assert.AreEqual("1-Deep-true", output.Trim());
        }
    }
}
