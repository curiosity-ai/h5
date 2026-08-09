using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class OutVarEnumTests : TranslatorTestBase
    {
        [TestMethod]
        public async Task OutVarWithEnumInNamespace()
        {
            var code = """
using System;

namespace GraphDB.Schema
{
    public enum CommandIcon { A, B }
}

public class Program
{
    public static void Main()
    {
        if (Enum.TryParse<GraphDB.Schema.CommandIcon>("A", out var icon))
        {
            Console.WriteLine(icon.ToString());
        }
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task OutVarWithEnumInNamespaceUsingAlias()
        {
            var code = """
using System;
using CI = GraphDB.Schema.CommandIcon;

namespace GraphDB.Schema
{
    public enum CommandIcon { A, B }
}

public class Program
{
    public static void Main()
    {
        if (Enum.TryParse<CI>("B", out var icon))
        {
            Console.WriteLine(icon.ToString());
        }
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task OutVarWithEnumAndNamespaceConflict()
        {
            var code = """
using System;
using Test;

namespace GraphDB.Schema
{
    public enum CommandIcon { A, B }
}

namespace Test
{
    public class GraphDB {}
}

public class Program
{
    public static void Main()
    {
        // GraphDB refers to Test.GraphDB
        // We must use global::GraphDB.Schema.CommandIcon to access the enum
        if (Enum.TryParse<global::GraphDB.Schema.CommandIcon>("A", out var icon))
        {
            Console.WriteLine(icon.ToString());
        }
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }

        [TestMethod]
        public async Task OutVarWithGenericMethod()
        {
            var code = """
using System;

namespace GraphDB.Schema
{
    public enum CommandIcon { A, B }
}

public class Program
{
    public static void Main()
    {
        Generic<GraphDB.Schema.CommandIcon>(out var icon);
        Console.WriteLine(icon.ToString());
    }

    public static void Generic<T>(out T value)
    {
        value = default(T);
        if (typeof(T).IsEnum)
        {
             // For test purpose, set to first value
             var values = Enum.GetValues(typeof(T));
             if (values.Length > 0) value = (T)values.GetValue(0);
        }
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }
    }
}
