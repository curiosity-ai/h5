using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class ComplexInheritanceGenericsInterfacesTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task SimpleInheritanceAndGenerics()
        {
            var code = @"
using System;

public interface IProcessor<T>
{
    void Process(T item);
}

public class BaseProcessor<T> : IProcessor<T>
{
    public virtual void Process(T item)
    {
        Console.WriteLine(""Base: "" + item);
    }
}

public class StringProcessor : BaseProcessor<string>
{
    public override void Process(string item)
    {
        Console.WriteLine(""String: "" + item);
        base.Process(item);
    }
}

public class Program
{
    public static void Main()
    {
        IProcessor<string> processor = new StringProcessor();
        processor.Process(""test"");
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task NestedInheritanceAndGenerics()
        {
             var code = @"
using System;

public interface IRepository<T>
{
    T Get(int id);
}

public abstract class BaseRepository<T> : IRepository<T>
{
    public abstract T Get(int id);
}

public class GenericRepository<T> : BaseRepository<T>
{
    public override T Get(int id)
    {
        Console.WriteLine(""Generic Get "" + id);
        return default(T);
    }
}

public class UserRepository : GenericRepository<string>
{
    public override string Get(int id)
    {
        Console.WriteLine(""User Get "" + id);
        return ""User"" + id;
    }
}

public class Program
{
    public static void Main()
    {
        IRepository<string> repo = new UserRepository();
        Console.WriteLine(repo.Get(1));
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task ComplexExtensionMethodsWithGenerics()
        {
            var code = @"
using System;
using System.Collections.Generic;
using System.Linq;

public interface IData<T>
{
    T Value { get; }
}

public class Data<T> : IData<T>
{
    public T Value { get; }
    public Data(T value) { Value = value; }
}

public static class Extensions
{
    public static void Print<T>(this IData<T> data)
    {
        Console.WriteLine(""Data: "" + data.Value);
    }

    public static void TransformAndPrint<T, U>(this IData<T> data, Func<T, U> transformer)
    {
        var result = transformer(data.Value);
        Console.WriteLine(""Transformed: "" + result);
    }

    public static void DeepProcess<T>(this IEnumerable<IData<T>> list)
    {
        foreach(var item in list)
        {
            Console.WriteLine(""Processing item..."");
            item.Print();
        }
    }
}

public class Program
{
    public static void Main()
    {
        var d = new Data<int>(42);
        d.Print();
        d.TransformAndPrint(x => x * 2);

        var list = new List<IData<string>>
        {
            new Data<string>(""A""),
            new Data<string>(""B"")
        };
        list.DeepProcess();
    }
}
";
            var output = await RunTest(code, skipRoslyn: true);
            var expectedOutput = @"Data: 42
Transformed: 84
Processing item...
Data: A
Processing item...
Data: B";
            Assert.AreEqual(expectedOutput, output.Trim());
        }

        [TestMethod]
        public async Task DeeplyNestedNamespacesAndGenerics()
        {
            var code = @"
using System;
using Level1.Level2.Level3;

namespace Level1
{
    namespace Level2
    {
        namespace Level3
        {
            public interface IDeep<T>
            {
                T Value { get; }
            }

            public class DeepContainer<T> : IDeep<T>
            {
                public T Value { get; private set; }
                public DeepContainer(T value)
                {
                    Value = value;
                }
            }

            public class DeepProcessor<T>
            {
                public void Process(IDeep<T> item)
                {
                    Console.WriteLine(""Processed: "" + item.Value);
                }
            }
        }
    }
}

public class Program
{
    public static void Main()
    {
        var container = new DeepContainer<string>(""Deep Value"");
        var processor = new DeepProcessor<string>();
        processor.Process(container);
    }
}
";
            var output = await RunTest(code, skipRoslyn: true);
            Assert.AreEqual("Processed: Deep Value", output.Trim());
        }

        [TestMethod]
        public async Task ComplexGenericConstraintsInInheritanceChain()
        {
            var code = @"
using System;

public interface IEntity
{
    int Id { get; set; }
}

public class Entity : IEntity
{
    public int Id { get; set; }
    public Entity() { Id = 0; }
}

public class BaseService<T> where T : IEntity
{
    public virtual void Handle(T entity)
    {
        Console.WriteLine(""Base Handle: "" + entity.Id);
    }
}

public class SpecializedService<T> : BaseService<T> where T : IEntity, new()
{
    public T CreateAndHandle()
    {
        T entity = new T();
        entity.Id = 100;
        Handle(entity);
        return entity;
    }

    public override void Handle(T entity)
    {
        Console.WriteLine(""Specialized Handle: "" + entity.Id);
        base.Handle(entity);
    }
}

public class Program
{
    public static void Main()
    {
        var service = new SpecializedService<Entity>();
        service.CreateAndHandle();
    }
}
";
            await RunTest(code);
        }

        [TestMethod]
        public async Task CrossNamespaceInheritanceAndExtensions()
        {
            var code = @"
using System;
using LibA;
using LibB;
using LibExtensions;

namespace LibA
{
    public class BaseComponent<T>
    {
        public T Data { get; set; }
        public BaseComponent(T data) { Data = data; }
        public virtual void Display() => Console.WriteLine(""Base: "" + Data);
    }
}

namespace LibB
{
    public class DerivedComponent : BaseComponent<int>
    {
        public DerivedComponent(int data) : base(data) { }
        public override void Display()
        {
            Console.WriteLine(""Derived: "" + Data);
            base.Display();
        }
    }
}

namespace LibExtensions
{
    public static class ComponentExtensions
    {
        public static void ExtendedDisplay<T>(this BaseComponent<T> component)
        {
            Console.WriteLine(""Extended Start"");
            component.Display();
            Console.WriteLine(""Extended End"");
        }
    }
}

public class Program
{
    public static void Main()
    {
        var comp = new DerivedComponent(99);
        comp.ExtendedDisplay();
    }
}
";
            var output = await RunTest(code, skipRoslyn: true);
            var expected = @"Extended Start
Derived: 99
Base: 99
Extended End";
            Assert.AreEqual(expected, output.Trim());
        }
    }
}
