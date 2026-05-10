using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class AdvancedConstraintsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task ConstraintStruct()
        {
            var code = @"
using System;

public class StructCheck<T> where T : struct
{
    public T Value;
    public StructCheck(T v) { Value = v; }
}

public class Program
{
    public static void Main()
    {
        var s = new StructCheck<int>(10);
        Console.WriteLine(s.Value);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ConstraintClass()
        {
            var code = @"
using System;

public class ClassCheck<T> where T : class
{
    public T Value;
    public ClassCheck(T v) { Value = v; }
}

public class Program
{
    public static void Main()
    {
        var c = new ClassCheck<string>(""Class"");
        Console.WriteLine(c.Value);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ConstraintNew()
        {
            var code = @"
using System;

public class Factory<T> where T : new()
{
    public T Create() => new T();
}

public class Item { public int Id = 5; }

public class Program
{
    public static void Main()
    {
        var f = new Factory<Item>();
        Console.WriteLine(f.Create().Id);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ConstraintBaseClass()
        {
            var code = @"
using System;

public class Base { public virtual void M() => Console.WriteLine(""Base""); }
public class Derived : Base { public override void M() => Console.WriteLine(""Derived""); }

public class Wrapper<T> where T : Base
{
    public void Call(T item) => item.M();
}

public class Program
{
    public static void Main()
    {
        var w = new Wrapper<Derived>();
        w.Call(new Derived());
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ConstraintInterface()
        {
            var code = @"
using System;

public interface I { void M(); }
public class C : I { public void M() => Console.WriteLine(""Impl""); }

public class Caller<T> where T : I
{
    public void Run(T t) => t.M();
}

public class Program
{
    public static void Main()
    {
        new Caller<C>().Run(new C());
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ConstraintNakedType()
        {
            var code = @"
using System;
using System.Collections.Generic;

public class Copier<T, U> where T : U
{
    public U Copy(T t) => t; // Implicit conversion T -> U
}

public class Program
{
    public static void Main()
    {
        var c = new Copier<string, object>();
        Console.WriteLine(c.Copy(""Naked""));
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task MultipleConstraints()
        {
            var code = @"
using System;

public interface I { }
public class Base { }
public class Target : Base, I { public Target() { } }

public class Multi<T> where T : Base, I, new()
{
    public T Create() => new T();
}

public class Program
{
    public static void Main()
    {
        var m = new Multi<Target>();
        Console.WriteLine(m.Create().GetType().Name); // Target (or similar depending on JS minification, we check logic)
    }
}
";
            // GetType().Name might vary in H5 minified code but usually preserves name in simple cases.
            // Let's rely on standard output or skipRoslyn if flaky.
            // But logic is sound.
            var output = await RunTest(code);
            Assert.IsTrue(output.Contains("Target"));
        }

        [TestMethod]
        public async Task GenericEvent()
        {
            var code = @"
using System;

public class EventPublisher<T>
{
    public event Action<T> OnEvent;
    public void Raise(T t) => OnEvent?.Invoke(t);
}

public class Program
{
    public static void Main()
    {
        var p = new EventPublisher<string>();
        p.OnEvent += s => Console.WriteLine(""Event: "" + s);
        p.Raise(""Payload"");
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericDelegate()
        {
            var code = @"
using System;

public delegate T Transformer<T>(T input);

public class Program
{
    public static void Main()
    {
        Transformer<int> square = x => x * x;
        Console.WriteLine(square(5));
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericIndexer()
        {
            var code = @"
using System;
using System.Collections.Generic;

public class Table<T>
{
    private Dictionary<string, T> _data = new Dictionary<string, T>();
    public T this[string key]
    {
        get => _data.ContainsKey(key) ? _data[key] : default(T);
        set => _data[key] = value;
    }
}

public class Program
{
    public static void Main()
    {
        var t = new Table<int>();
        t[""A""] = 100;
        Console.WriteLine(t[""A""]);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericOperatorOverloading()
        {
            var code = @"
using System;

public class Wrapper<T>
{
    public int Val;
    public Wrapper(int v) { Val = v; }
    public static Wrapper<T> operator +(Wrapper<T> a, Wrapper<T> b) => new Wrapper<T>(a.Val + b.Val);
}

public class Program
{
    public static void Main()
    {
        var a = new Wrapper<string>(1);
        var b = new Wrapper<string>(2);
        var c = a + b;
        Console.WriteLine(c.Val);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericConversionOperator()
        {
            var code = @"
using System;

public class Box<T>
{
    public T Value;
    public Box(T v) { Value = v; }
    public static implicit operator T(Box<T> b) => b.Value;
    public static explicit operator Box<T>(T v) => new Box<T>(v);
}

public class Program
{
    public static void Main()
    {
        Box<int> b = (Box<int>)123;
        int i = b;
        Console.WriteLine(i);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task TypeOfGeneric()
        {
            var code = @"
using System;

public class G<T> { }

public class Program
{
    public static void Main()
    {
        Console.WriteLine(typeof(G<int>) == typeof(G<int>));
        Console.WriteLine(typeof(G<int>) != typeof(G<string>));
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task DefaultOfGeneric()
        {
            var code = @"
using System;

public class Utils
{
    public static T GetDefault<T>() => default(T);
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine(Utils.GetDefault<int>()); // 0
        Console.WriteLine(Utils.GetDefault<string>() == null); // True
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task IsAsGenericPattern()
        {
            var code = @"
using System;

public class Program
{
    public static void Check<T>(object o)
    {
        if (o is T t)
        {
            Console.WriteLine(""Is T: "" + t);
        }
        else
        {
            Console.WriteLine(""Not T"");
        }
    }

    public static void Main()
    {
        Check<string>(""Hello"");
        Check<int>(""Hello"");
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NullableGenericStruct()
        {
            var code = @"
using System;

public class Program
{
    public static void Check<T>(T? val) where T : struct
    {
        if (val.HasValue) Console.WriteLine(val.Value);
        else Console.WriteLine(""Null"");
    }

    public static void Main()
    {
        Check<int>(10);
        Check<int>(null);
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task GenericMethodConstraints()
        {
            var code = @"
using System;

public class Program
{
    public static void Check<T>(T item) where T : IComparable<T>
    {
        Console.WriteLine(item.CompareTo(item) == 0);
    }

    public static void Main()
    {
        Check(10);
        Check(""Test"");
    }
}
";
            await RunTest(code);
        }
    }
}
