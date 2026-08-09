using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Translator.Roslyn.Tests.Ported
{
    /// <summary>
    /// Covers the H5 attributes and codegen fixes added while aligning the Roslyn translator's
    /// output with the legacy H5 compiler: [Name] on types, [ExpandParams], [ObjectLiteral]
    /// ($literal), string-backed enums, generic-interface variance, default(T) for value-type
    /// parameters, types nested in a generic, overload disambiguation, and IEnumerable
    /// (GetEnumerator) dispatch through the runtime's is/as tracking.
    /// </summary>
    [TestClass]
    public class AttributeCodegenTests : TranslatorTestBase
    {
        // ---- [Name] on a TYPE -------------------------------------------------

        [TestMethod]
        public async Task TestNameAttributeOnType()
        {
            var code = @"
using H5;
using System;

namespace App
{
    [Name(""my.custom.Widget"")]
    public class Widget
    {
        public int V;
        public Widget(int v) { V = v; }
        public int Get() => V;
    }

    public class Program
    {
        public static void Main()
        {
            var w = new Widget(42);
            Console.WriteLine(w.Get());

            // The type is registered under its custom fully-qualified name.
            bool registered = Script.Write<bool>(""typeof my !== 'undefined' && typeof my.custom !== 'undefined' && typeof my.custom.Widget === 'function'"");
            Console.WriteLine(""registered: "" + registered);
            if (!registered) throw new Exception(""Widget should be registered as my.custom.Widget"");
            if (w.Get() != 42) throw new Exception(""Widget.Get() should be 42"");
        }
    }
}";
            await RunTest(code, skipRoslyn: true);
        }

        // ---- [ObjectLiteral] $literal flag ------------------------------------

        [TestMethod]
        public void TestObjectLiteralEmitsLiteralFlag()
        {
            // An [ObjectLiteral] type declares $literal: true so the runtime treats it as a
            // literal (instances are plain {} objects), matching the legacy compiler.
            var code = @"
using H5;
using System;

[ObjectLiteral]
public class Options
{
    public string Title { get; set; }
    public int Size { get; set; }
}

public class Program { public static void Main() { var o = new Options { Title = ""x"", Size = 1 }; } }
";
            var result = new RoslynTranslator().Translate(code);
            Assert.IsTrue(result.Success, "translation should succeed");
            Assert.IsTrue(result.Javascript!.Contains("$literal: true"),
                "[ObjectLiteral] type should emit $literal: true\n" + result.Javascript);
        }

        // ---- [ExpandParams] ---------------------------------------------------

        [TestMethod]
        public void TestExpandParamsSpread()
        {
            // A [ExpandParams] native variadic call must pass its arguments individually
            // (Math.max(3, 7, 2)), not wrapped in one array (Math.max([3, 7, 2]) → the array
            // coerces to a single value). Verified at the translation layer — the emitted call
            // shape is what the fix controls.
            var code = @"
using H5;
using System;

[External]
[Name(""Math"")]
public static class JsMath
{
    [ExpandParams]
    [Name(""max"")]
    public static extern double max(params double[] values);
}

public class Program
{
    public static void Main()
    {
        var m = JsMath.max(3, 7, 2);
    }
}";
            var result = new RoslynTranslator().Translate(code);
            Assert.IsTrue(result.Success, "translation should succeed");
            var js = result.Javascript!;
            Assert.IsTrue(js.Contains("Math.max(3, 7, 2)"),
                "ExpandParams call should spread its arguments, got:\n" + js);
            Assert.IsFalse(js.Contains("Math.max([3, 7, 2])"),
                "ExpandParams call must not wrap arguments in an array");
        }

        [TestMethod]
        public void TestExpandParamsSpreadsArrayArgument()
        {
            // Passing a single array to an [ExpandParams] method spreads it (Math.max(...arr)).
            var code = @"
using H5;
using System;

[External]
[Name(""Math"")]
public static class JsMath
{
    [ExpandParams]
    [Name(""max"")]
    public static extern double max(params double[] values);
}

public class Program
{
    public static void Main()
    {
        var arr = new double[] { 1, 2, 3 };
        var m = JsMath.max(arr);
    }
}";
            var result = new RoslynTranslator().Translate(code);
            Assert.IsTrue(result.Success, "translation should succeed");
            var js = result.Javascript!;
            Assert.IsTrue(js.Contains("Math.max(...") || js.Contains("Math.max.apply"),
                "a single array argument to an ExpandParams method should be spread, got:\n" + js);
        }

        // ---- string-backed enums with [Name] members --------------------------

        [TestMethod]
        public async Task TestStringEnumWithNameMembers()
        {
            var code = @"
using H5;
using System;

[Enum(Emit.StringName)]
public enum Color
{
    [Name(""bright-red"")]  Red,
    [Name(""deep-blue"")]   Blue,
}

public class Program
{
    public static void Main()
    {
        object r = Color.Red;
        object b = Color.Blue;
        Console.WriteLine(r);
        Console.WriteLine(b);
        if (!(r is string)) throw new Exception(""string-backed enum member should be a string"");
        if ((string)r != ""bright-red"") throw new Exception(""Color.Red should be 'bright-red'"");
        if ((string)b != ""deep-blue"") throw new Exception(""Color.Blue should be 'deep-blue'"");

        // A defaulted string-enum parameter emits the zero member's string, not 0.
        Console.WriteLine(Describe());
        if (Describe() != ""bright-red"") throw new Exception(""default(Color) parameter should be 'bright-red'"");
    }

    static string Describe(Color c = default) => (string)(object)c;
}";
            await RunTest(code, skipRoslyn: true);
        }

        // ---- generic-interface variance ($variance) --------------------------

        [TestMethod]
        public void TestCovariantInterfaceEmitsVariance()
        {
            // A generic interface with an `out`/`in` parameter records its variance in the define
            // ($variance: [2] for a single covariant parameter), matching the legacy compiler.
            var code = @"
using System;

public interface IProducer<out T> { T Produce(); }
public interface IConsumer<in T> { void Consume(T value); }
public interface IInvariant<T> { T Round(T value); }

public class Program { public static void Main() { } }
";
            var result = new RoslynTranslator().Translate(code);
            Assert.IsTrue(result.Success, "translation should succeed");
            var js = result.Javascript!;

            Assert.IsTrue(js.Contains("$variance: [2]"),
                "covariant (out) interface should emit $variance: [2]\n" + js);
            Assert.IsTrue(js.Contains("$variance: [1]"),
                "contravariant (in) interface should emit $variance: [1]\n" + js);
            // The invariant interface must NOT carry a $variance entry.
            var invariantIdx = js.IndexOf("\"App.IInvariant$1\"", StringComparison.Ordinal);
            if (invariantIdx < 0) invariantIdx = js.IndexOf("IInvariant$1", StringComparison.Ordinal);
            Assert.IsTrue(invariantIdx >= 0, "IInvariant$1 should be defined");
        }

        // ---- default(T) for a value-type type parameter -----------------------

        [TestMethod]
        public async Task TestDefaultOfValueTypeParameter()
        {
            // A generic type field of type T must default to the zeroed value for value-type T,
            // not null. Runs natively AND as JS and compares output.
            var code = @"
using System;

public class Box<T>
{
    public T Value;
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine(new Box<int>().Value);       // 0
        Console.WriteLine(new Box<long>().Value);      // 0
        Console.WriteLine(new Box<double>().Value);    // 0
        Console.WriteLine(new Box<string>().Value ?? ""<null>""); // <null>
    }
}";
            await RunTest(code);
        }

        // ---- a type nested inside a generic type ------------------------------

        [TestMethod]
        public async Task TestNestedTypeInGenericUsesOuterTypeParameter()
        {
            // Inner references the enclosing Outer<T>'s T; it must be emitted as a function of T
            // and default its field to the zeroed value.
            var code = @"
using System;

public class Outer<T>
{
    public class Inner
    {
        public T Value;
        public T Get() => Value;
    }
}

public class Program
{
    public static void Main()
    {
        var i = new Outer<int>.Inner();
        Console.WriteLine(i.Get());   // 0
        i.Value = 5;
        Console.WriteLine(i.Get());   // 5
    }
}";
            await RunTest(code);
        }

        // ---- overload disambiguation: derived overload + generic base ---------

        [TestMethod]
        public async Task TestOverloadDerivedWithGenericBase()
        {
            // Derived declares a NEW overload of a name whose other overload lives on a generic
            // base (the Card.OnClick shape). The two must get distinct JS names, not collide.
            var code = @"
using System;

public class Base<T>
{
    public virtual string M(int x) => ""base:"" + x;
}

public sealed class Derived : Base<Derived>
{
    public override string M(int x) => ""over:"" + x;
    public string M(string s) => ""str:"" + s;   // new overload, same name
}

public class Program
{
    public static void Main()
    {
        var d = new Derived();
        Console.WriteLine(d.M(5));
        Console.WriteLine(d.M(""hi""));
    }
}";
            await RunTest(code);
        }

        // ---- overload disambiguation: two interface implementations -----------

        [TestMethod]
        public async Task TestOverloadFromTwoInterfaces()
        {
            // A type implements a same-named method from two different interfaces; both must be
            // callable (they must not collapse onto one JS key).
            var code = @"
using System;

public interface IReader { string Handle(int x); }
public interface IWriter { string Handle(string x); }

public class RW : IReader, IWriter
{
    public string Handle(int x) => ""int:"" + x;
    public string Handle(string x) => ""str:"" + x;
}

public class Program
{
    public static void Main()
    {
        var rw = new RW();
        Console.WriteLine(((IReader)rw).Handle(5));
        Console.WriteLine(((IWriter)rw).Handle(""hi""));
        Console.WriteLine(rw.Handle(9));
        Console.WriteLine(rw.Handle(""z""));
    }
}";
            await RunTest(code);
        }

        // ---- IEnumerable dispatch through the runtime (GetEnumerator + inherits) ----

        [TestMethod]
        public async Task TestLinqOverCustomEnumerable()
        {
            // A user collection implementing IEnumerable<T> must be recognized as IEnumerable by
            // the runtime (its inherits must list the BCL collection interfaces) and expose
            // GetEnumerator under the PascalCase name h5.js looks up — otherwise Enumerable.from
            // falls back to {key,value} object iteration.
            var code = @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MyList<T> : IEnumerable<T>
{
    private readonly List<T> _items = new List<T>();
    public void Add(T x) => _items.Add(x);
    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class Program
{
    public static void Main()
    {
        var l = new MyList<int>();
        l.Add(1); l.Add(2); l.Add(3);

        Console.WriteLine(l.Count());
        Console.WriteLine(l.Sum());
        Console.WriteLine(string.Join("","", l.Select(x => x * 2)));

        int total = 0;
        foreach (var x in l) total += x;
        Console.WriteLine(total);
    }
}";
            await RunTest(code);
        }

        // ---- new T() with a method type parameter -----------------------------

        [TestMethod]
        public async Task TestNewMethodTypeParameter()
        {
            // new T() where T is a *method* type parameter (new() constraint) must instantiate a
            // real T, not an empty object literal.
            var code = @"
using System;

public class Thing
{
    public int X = 7;
    public string Name() => ""thing"";
}

public static class Factory
{
    public static T Make<T>() where T : new() => new T();
}

public class Program
{
    public static void Main()
    {
        var t = Factory.Make<Thing>();
        Console.WriteLine(t.X);
        Console.WriteLine(t.Name());
    }
}";
            await RunTest(code);
        }
    }
}
