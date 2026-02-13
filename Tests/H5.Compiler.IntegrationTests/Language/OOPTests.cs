using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class OOPTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task Classes_Fields()
        {
            var code = @"
using System;

public class MyClass
{
    public int InstanceField;
    public static int StaticField;
}

public class Program
{
    public static void Main()
    {
        var obj = new MyClass();
        obj.InstanceField = 10;
        MyClass.StaticField = 20;

        Console.WriteLine(obj.InstanceField);
        Console.WriteLine(MyClass.StaticField);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Classes_Methods()
        {
            var code = @"
using System;

public class MyClass
{
    public void InstanceMethod()
    {
        Console.WriteLine(""InstanceMethod"");
    }

    public static void StaticMethod()
    {
        Console.WriteLine(""StaticMethod"");
    }
}

public class Program
{
    public static void Main()
    {
        var obj = new MyClass();
        obj.InstanceMethod();
        MyClass.StaticMethod();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Classes_Constructors()
        {
            var code = @"
using System;

public class MyClass
{
    public int Value;

    public MyClass()
    {
        Value = 1;
        Console.WriteLine(""Default Constructor"");
    }

    public MyClass(int v)
    {
        Value = v;
        Console.WriteLine(""Parameterized Constructor: "" + v);
    }
}

public class Program
{
    public static void Main()
    {
        var obj1 = new MyClass();
        Console.WriteLine(obj1.Value);

        var obj2 = new MyClass(100);
        Console.WriteLine(obj2.Value);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Classes_This()
        {
            var code = @"
using System;

public class MyClass
{
    private int value;

    public MyClass(int value)
    {
        this.value = value;
    }

    public void PrintValue()
    {
        Console.WriteLine(this.value);
    }
}

public class Program
{
    public static void Main()
    {
        var obj = new MyClass(42);
        obj.PrintValue();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Inheritance_Basic()
        {
            var code = @"
using System;

public class BaseClass
{
    public void BaseMethod()
    {
        Console.WriteLine(""BaseMethod"");
    }
}

public class DerivedClass : BaseClass
{
    public void DerivedMethod()
    {
        Console.WriteLine(""DerivedMethod"");
    }
}

public class Program
{
    public static void Main()
    {
        var obj = new DerivedClass();
        obj.BaseMethod();
        obj.DerivedMethod();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Inheritance_VirtualOverride()
        {
            var code = @"
using System;

public class BaseClass
{
    public virtual void Method()
    {
        Console.WriteLine(""BaseClass.Method"");
    }
}

public class DerivedClass : BaseClass
{
    public override void Method()
    {
        Console.WriteLine(""DerivedClass.Method"");
    }
}

public class Program
{
    public static void Main()
    {
        BaseClass obj = new DerivedClass();
        obj.Method();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Inheritance_Abstract()
        {
            var code = @"
using System;

public abstract class BaseClass
{
    public abstract void Method();
}

public class DerivedClass : BaseClass
{
    public override void Method()
    {
        Console.WriteLine(""DerivedClass.Method"");
    }
}

public class Program
{
    public static void Main()
    {
        BaseClass obj = new DerivedClass();
        obj.Method();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Inheritance_Polymorphism()
        {
            var code = @"
using System;

public class BaseClass
{
    public virtual void Identify()
    {
        Console.WriteLine(""Base"");
    }
}

public class DerivedA : BaseClass
{
    public override void Identify()
    {
        Console.WriteLine(""DerivedA"");
    }
}

public class DerivedB : BaseClass
{
    public override void Identify()
    {
        Console.WriteLine(""DerivedB"");
    }
}

public class Program
{
    public static void Main()
    {
        BaseClass[] objects = new BaseClass[] { new BaseClass(), new DerivedA(), new DerivedB() };
        foreach (var obj in objects)
        {
            obj.Identify();
        }
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Inheritance_Base()
        {
            var code = @"
using System;

public class BaseClass
{
    public virtual void Method()
    {
        Console.WriteLine(""BaseClass.Method"");
    }
}

public class DerivedClass : BaseClass
{
    public override void Method()
    {
        base.Method();
        Console.WriteLine(""DerivedClass.Method"");
    }
}

public class Program
{
    public static void Main()
    {
        var obj = new DerivedClass();
        obj.Method();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Interfaces_Implementation()
        {
            var code = @"
using System;

public interface IMyInterface
{
    void Method();
}

public class MyClass : IMyInterface
{
    public void Method()
    {
        Console.WriteLine(""MyClass.Method"");
    }
}

public class Program
{
    public static void Main()
    {
        IMyInterface obj = new MyClass();
        obj.Method();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Interfaces_ExplicitImplementation()
        {
            var code = @"
using System;

public interface IMyInterface
{
    void Method();
}

public class MyClass : IMyInterface
{
    void IMyInterface.Method()
    {
        Console.WriteLine(""MyClass.IMyInterface.Method"");
    }

    public void Method()
    {
        Console.WriteLine(""MyClass.Method"");
    }
}

public class Program
{
    public static void Main()
    {
        MyClass obj = new MyClass();
        obj.Method();
        ((IMyInterface)obj).Method();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Interfaces_Inheritance()
        {
            var code = @"
using System;

public interface IBase
{
    void BaseMethod();
}

public interface IDerived : IBase
{
    void DerivedMethod();
}

public class MyClass : IDerived
{
    public void BaseMethod()
    {
        Console.WriteLine(""BaseMethod"");
    }

    public void DerivedMethod()
    {
        Console.WriteLine(""DerivedMethod"");
    }
}

public class Program
{
    public static void Main()
    {
        IDerived obj = new MyClass();
        obj.BaseMethod();
        obj.DerivedMethod();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Structs_Basic()
        {
            var code = @"
using System;

public struct MyStruct
{
    public int X;
    public int Y;

    public void Print()
    {
        Console.WriteLine(X + "", "" + Y);
    }
}

public class Program
{
    public static void Main()
    {
        var s = new MyStruct();
        s.X = 10;
        s.Y = 20;
        s.Print();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Structs_Constructors()
        {
            var code = @"
using System;

public struct MyStruct
{
    public int X;
    public int Y;

    public MyStruct(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Program
{
    public static void Main()
    {
        var s = new MyStruct(1, 2);
        Console.WriteLine(s.X);
        Console.WriteLine(s.Y);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Structs_ValueSemantics()
        {
            var code = @"
using System;

public struct MyStruct
{
    public int Value;
}

public class Program
{
    public static void Main()
    {
        var s1 = new MyStruct();
        s1.Value = 10;

        var s2 = s1;
        s2.Value = 20;

        Console.WriteLine(s1.Value);
        Console.WriteLine(s2.Value);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Properties_Basic()
        {
            var code = @"
using System;

public class MyClass
{
    private int _value;

    public int Value
    {
        get { return _value; }
        set { _value = value; }
    }
}

public class Program
{
    public static void Main()
    {
        var obj = new MyClass();
        obj.Value = 10;
        Console.WriteLine(obj.Value);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Properties_Computed()
        {
            var code = @"
using System;

public class Rectangle
{
    public int Width;
    public int Height;

    public int Area
    {
        get { return Width * Height; }
    }
}

public class Program
{
    public static void Main()
    {
        var rect = new Rectangle();
        rect.Width = 10;
        rect.Height = 5;
        Console.WriteLine(rect.Area);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Properties_Auto()
        {
            var code = @"
using System;

public class MyClass
{
    public int Value { get; set; }
}

public class Program
{
    public static void Main()
    {
        var obj = new MyClass();
        obj.Value = 123;
        Console.WriteLine(obj.Value);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Indexers_Basic()
        {
            var code = @"
using System;

public class MyCollection
{
    private string[] _data = new string[10];

    public string this[int index]
    {
        get { return _data[index]; }
        set { _data[index] = value; }
    }
}

public class Program
{
    public static void Main()
    {
        var collection = new MyCollection();
        collection[0] = ""Hello"";
        collection[1] = ""World"";

        Console.WriteLine(collection[0]);
        Console.WriteLine(collection[1]);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Events_Basic()
        {
            var code = @"
using System;

public class Publisher
{
    public event EventHandler MyEvent;

    public void RaiseEvent()
    {
        MyEvent?.Invoke(this, EventArgs.Empty);
    }
}

public class Program
{
    public static void Main()
    {
        var pub = new Publisher();
        pub.MyEvent += (sender, e) => Console.WriteLine(""Event Received"");
        pub.RaiseEvent();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Delegates_Basic()
        {
            var code = @"
using System;

public delegate void MyDelegate(string message);

public class Program
{
    public static void Method(string message)
    {
        Console.WriteLine(""Method: "" + message);
    }

    public static void Main()
    {
        MyDelegate del = Method;
        del(""Hello"");
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Operators_Overloading()
        {
            var code = @"
using System;

public class Complex
{
    public int Real;
    public int Imaginary;

    public Complex(int real, int imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }

    public static Complex operator +(Complex c1, Complex c2)
    {
        return new Complex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
    }
}

public class Program
{
    public static void Main()
    {
        var c1 = new Complex(1, 2);
        var c2 = new Complex(3, 4);
        var c3 = c1 + c2;

        Console.WriteLine(c3.Real);
        Console.WriteLine(c3.Imaginary);
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Inheritance_Sealed()
        {
            var code = @"
using System;

public class BaseClass
{
    public virtual void Method()
    {
        Console.WriteLine(""Base"");
    }
}

public sealed class SealedClass : BaseClass
{
    public override void Method()
    {
        Console.WriteLine(""Sealed"");
    }
}

public class Program
{
    public static void Main()
    {
        BaseClass obj = new SealedClass();
        obj.Method();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Events_Unsubscribe()
        {
            var code = @"
using System;

public class Publisher
{
    public event EventHandler MyEvent;

    public void RaiseEvent()
    {
        MyEvent?.Invoke(this, EventArgs.Empty);
    }
}

public class Program
{
    public static void Handler(object sender, EventArgs e)
    {
        Console.WriteLine(""Handled"");
    }

    public static void Main()
    {
        var pub = new Publisher();
        pub.MyEvent += Handler;
        pub.RaiseEvent();
        pub.MyEvent -= Handler;
        pub.RaiseEvent(); // Should not print anything
        Console.WriteLine(""Done"");
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Delegates_Multicast()
        {
            var code = @"
using System;

public delegate void MyDelegate();

public class Program
{
    public static void Method1() => Console.WriteLine(""1"");
    public static void Method2() => Console.WriteLine(""2"");

    public static void Main()
    {
        MyDelegate del = Method1;
        del += Method2;
        del();
    }
}";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Operators_Conversion()
        {
            var code = @"
using System;

public class MyInt
{
    public int Value;

    public MyInt(int value)
    {
        Value = value;
    }

    public static implicit operator MyInt(int value)
    {
        return new MyInt(value);
    }

    public static explicit operator int(MyInt myInt)
    {
        return myInt.Value;
    }
}

public class Program
{
    public static void Main()
    {
        MyInt obj = 10; // Implicit
        int val = (int)obj; // Explicit
        Console.WriteLine(val);
    }
}";
            await RunTest(code);
        }
    }
}
