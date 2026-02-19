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
    }
}
