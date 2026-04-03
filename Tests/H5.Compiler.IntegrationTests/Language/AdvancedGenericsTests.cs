using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class AdvancedGenericsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task CovariantInterface()
        {
            var code = @"
using System;
using System.Collections.Generic;

public interface IProducer<out T>
{
    T Produce();
}

public class Producer<T> : IProducer<T>
{
    private T _value;
    public Producer(T value) { _value = value; }
    public T Produce() => _value;
}

public class Program
{
    public static void Main()
    {
        IProducer<string> stringProducer = new Producer<string>(""Covariant"");
        IProducer<object> objProducer = stringProducer; // Covariance
        Console.WriteLine(objProducer.Produce());
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ContravariantInterface()
        {
            var code = @"
using System;

public interface IConsumer<in T>
{
    void Consume(T item);
}

public class Consumer<T> : IConsumer<T>
{
    public void Consume(T item) => Console.WriteLine(item);
}

public class Program
{
    public static void Main()
    {
        IConsumer<object> objConsumer = new Consumer<object>();
        IConsumer<string> stringConsumer = objConsumer; // Contravariance
        stringConsumer.Consume(""Contravariant"");
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericInterfaceInheritanceChain()
        {
            var code = @"
using System;

public interface ILevel1<T> { void M1(T t); }
public interface ILevel2<T> : ILevel1<T> { void M2(T t); }
public interface ILevel3<T> : ILevel2<T> { void M3(T t); }

public class Impl<T> : ILevel3<T>
{
    public void M1(T t) => Console.WriteLine(""M1 "" + t);
    public void M2(T t) => Console.WriteLine(""M2 "" + t);
    public void M3(T t) => Console.WriteLine(""M3 "" + t);
}

public class Program
{
    public static void Main()
    {
        ILevel3<int> impl = new Impl<int>();
        impl.M1(1);
        impl.M2(2);
        impl.M3(3);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ExplicitGenericInterfaceImplementation()
        {
            var code = @"
using System;

public interface IGeneric<T> { void Method(T t); }

public class Explicit<T> : IGeneric<T>
{
    void IGeneric<T>.Method(T t) => Console.WriteLine(""Explicit "" + t);
    public void Method(T t) => Console.WriteLine(""Implicit "" + t);
}

public class Program
{
    public static void Main()
    {
        var e = new Explicit<int>();
        e.Method(1);
        ((IGeneric<int>)e).Method(2);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericMethodInGenericInterfaceImplementation()
        {
            var code = @"
using System;

public interface IProcessor<T>
{
    void Process<U>(T item, U other);
}

public class Processor<T> : IProcessor<T>
{
    public void Process<U>(T item, U other)
    {
        Console.WriteLine($""T:{item}, U:{other}"");
    }
}

public class Program
{
    public static void Main()
    {
        IProcessor<string> p = new Processor<string>();
        p.Process<int>(""Hello"", 123);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task MultipleGenericInterfacesSameMethod()
        {
            var code = @"
using System;

public interface IA<T> { void M(T t); }
public interface IB<T> { void M(T t); }

public class Multi<T> : IA<T>, IB<T>
{
    void IA<T>.M(T t) => Console.WriteLine(""IA "" + t);
    void IB<T>.M(T t) => Console.WriteLine(""IB "" + t);
}

public class Program
{
    public static void Main()
    {
        var m = new Multi<string>();
        ((IA<string>)m).M(""A"");
        ((IB<string>)m).M(""B"");
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task StaticGenericFields()
        {
            var code = @"
using System;

public class Gen<T>
{
    public static int Count;
    public Gen() { Count++; }
}

public class Program
{
    public static void Main()
    {
        new Gen<int>();
        new Gen<int>();
        new Gen<string>();
        Console.WriteLine(Gen<int>.Count); // 2
        Console.WriteLine(Gen<string>.Count); // 1
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task StaticGenericMethods()
        {
            var code = @"
using System;

public class Utils
{
    public static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }
}

public class Program
{
    public static void Main()
    {
        int x = 1, y = 2;
        Utils.Swap(ref x, ref y);
        Console.WriteLine($""{x}, {y}"");
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task AbstractGenericBase()
        {
            var code = @"
using System;

public abstract class Base<T>
{
    public abstract void AbstractMethod(T t);
    public virtual void VirtualMethod(T t) => Console.WriteLine(""Base Virtual "" + t);
}

public class Derived<T> : Base<T>
{
    public override void AbstractMethod(T t) => Console.WriteLine(""Derived Abstract "" + t);
    public override void VirtualMethod(T t)
    {
        base.VirtualMethod(t);
        Console.WriteLine(""Derived Virtual "" + t);
    }
}

public class Program
{
    public static void Main()
    {
        Base<int> d = new Derived<int>();
        d.AbstractMethod(10);
        d.VirtualMethod(20);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task VirtualGenericOverride()
        {
            var code = @"
using System;

public class Base
{
    public virtual void M<T>(T t) => Console.WriteLine(""Base M "" + t);
}

public class Derived : Base
{
    public override void M<T>(T t) => Console.WriteLine(""Derived M "" + t);
}

public class Program
{
    public static void Main()
    {
        Base b = new Derived();
        b.M(123);
        b.M(""abc"");
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task HidingGenericMember()
        {
            var code = @"
using System;

public class Base<T>
{
    public T Value = default(T);
}

public class Derived<T> : Base<T>
{
    public new string Value = ""Derived""; // Hiding
}

public class Program
{
    public static void Main()
    {
        Derived<int> d = new Derived<int>();
        Console.WriteLine(d.Value); // Derived
        Console.WriteLine(((Base<int>)d).Value); // 0
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task SealedGenericClass()
        {
            var code = @"
using System;

public sealed class SealedBox<T>
{
    public T Item;
}

public class Program
{
    public static void Main()
    {
        var box = new SealedBox<int> { Item = 99 };
        Console.WriteLine(box.Item);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ConstructorChainingGenerics()
        {
            var code = @"
using System;

public class Base<T>
{
    public T Val;
    public Base(T val) { Val = val; }
}

public class Derived<T> : Base<T>
{
    public Derived(T val) : base(val) { }
}

public class Program
{
    public static void Main()
    {
        var d = new Derived<string>(""Chain"");
        Console.WriteLine(d.Val);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericInheritingNonGeneric()
        {
            var code = @"
using System;

public class NonGenericBase
{
    public void Hello() => Console.WriteLine(""Hello"");
}

public class GenericDerived<T> : NonGenericBase
{
    public T Data;
}

public class Program
{
    public static void Main()
    {
        var g = new GenericDerived<int>();
        g.Hello();
        g.Data = 5;
        Console.WriteLine(g.Data);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NonGenericInheritingClosedGeneric()
        {
            var code = @"
using System;

public class GenericBase<T>
{
    public T Value;
}

public class IntDerived : GenericBase<int>
{
    public void Set(int v) => Value = v;
}

public class Program
{
    public static void Main()
    {
        var d = new IntDerived();
        d.Set(100);
        Console.WriteLine(d.Value);
    }
}
";
            await RunTest(code);
        }
    }
}
