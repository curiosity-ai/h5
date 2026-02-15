using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class OptionalParametersTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task EnumOptionalParameterDefaultValue()
        {
             var code = """
using System;
using System.Collections.Generic;

public interface IComponentWithID {}
public class ObservableList<T> {}

public enum Orientation
{
    Vertical,
    Horizontal,
    VerticalReverse,
    HorizontalReverse,
}

public class ObservableStack
{
    public ObservableStack(ObservableList<IComponentWithID> observableList, Orientation orientation = Orientation.Vertical, bool debounce = true)
    {
        Console.WriteLine("Is Vertical: " + (orientation == Orientation.Vertical));
        Console.WriteLine("Value: " + orientation);
        Console.WriteLine("Debounce: " + debounce);
    }
}

public class Program
{
    public static void Main()
    {
        new ObservableStack(new ObservableList<IComponentWithID>());
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task EnumOptionalParameterDefaultKeyword()
        {
             var code = """
using System;

public enum Orientation
{
    Vertical,
    Horizontal,
}

public class ObservableStack
{
    public ObservableStack(Orientation orientation = default)
    {
        Console.WriteLine("Is Vertical: " + (orientation == Orientation.Vertical));
        Console.WriteLine("Value: " + orientation);
    }
}

public class Program
{
    public static void Main()
    {
        new ObservableStack();
    }
}
""";
            await RunTest(code);
        }
    }
}
