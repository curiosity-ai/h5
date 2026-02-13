using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class GenericsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task GenericClass_Basic()
        {
            var code = """
using System;

public class Box<T>
{
    private T _value;

    public Box(T value)
    {
        _value = value;
    }

    public T GetValue()
    {
        return _value;
    }

    public void SetValue(T value)
    {
        _value = value;
    }
}

public class Program
{
    public static void Main()
    {
        var intBox = new Box<int>(123);
        Console.WriteLine(intBox.GetValue());
        intBox.SetValue(456);
        Console.WriteLine(intBox.GetValue());

        var stringBox = new Box<string>("Hello");
        Console.WriteLine(stringBox.GetValue());
        stringBox.SetValue("World");
        Console.WriteLine(stringBox.GetValue());
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericMethod_Basic()
        {
            var code = """
using System;

public class Program
{
    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp = lhs;
        lhs = rhs;
        rhs = temp;
    }

    public static void Main()
    {
        int a = 1;
        int b = 2;
        Swap<int>(ref a, ref b);
        Console.WriteLine(a);
        Console.WriteLine(b);

        string s1 = "First";
        string s2 = "Second";
        Swap(ref s1, ref s2); // Type inference
        Console.WriteLine(s1);
        Console.WriteLine(s2);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericInterface_Implementation()
        {
            var code = """
using System;

public interface IContainer<T>
{
    T Item { get; set; }
}

public class Container<T> : IContainer<T>
{
    public T Item { get; set; }
}

public class Program
{
    public static void Main()
    {
        IContainer<int> intContainer = new Container<int>();
        intContainer.Item = 42;
        Console.WriteLine(intContainer.Item);

        IContainer<string> stringContainer = new Container<string>();
        stringContainer.Item = "Test";
        Console.WriteLine(stringContainer.Item);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericConstraints_Class()
        {
            var code = """
using System;

public class ReferenceBox<T> where T : class
{
    public T Value { get; set; }
}

public class Program
{
    public static void Main()
    {
        var stringBox = new ReferenceBox<string>();
        stringBox.Value = "RefType";
        Console.WriteLine(stringBox.Value);

        // ReferenceBox<int> intBox = new ReferenceBox<int>(); // This would be a compile error
        Console.WriteLine("Done");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericConstraints_New()
        {
            var code = """
using System;

public class Factory<T> where T : new()
{
    public T CreateInstance()
    {
        return new T();
    }
}

public class Item
{
    public Item()
    {
        Console.WriteLine("Item Created");
    }
}

public class Program
{
    public static void Main()
    {
        var factory = new Factory<Item>();
        var item = factory.CreateInstance();
    }
}
""";
            await RunTest(code);
        }
    }
}
