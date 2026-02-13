using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class ArraysTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task SingleDimensionalArray()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        int[] arr = new int[3];
        arr[0] = 10;
        arr[1] = 20;
        arr[2] = 30;

        Console.WriteLine(arr.Length);
        Console.WriteLine(arr[1]);

        int[] arr2 = { 1, 2, 3, 4, 5 };
        Console.WriteLine(arr2.Length);

        foreach (var x in arr2)
        {
            Console.WriteLine(x);
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task MultiDimensionalArray()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        int[,] grid = new int[2, 3];
        grid[0, 0] = 1;
        grid[0, 1] = 2;
        grid[0, 2] = 3;
        grid[1, 0] = 4;
        grid[1, 1] = 5;
        grid[1, 2] = 6;

        Console.WriteLine(grid.Length);
        Console.WriteLine(grid.GetLength(0));
        Console.WriteLine(grid.GetLength(1));

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Console.WriteLine(grid[i, j]);
            }
        }

        int[,] grid2 = { { 10, 20 }, { 30, 40 } };
        Console.WriteLine(grid2[1, 0]);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task JaggedArray()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        int[][] jagged = new int[2][];
        jagged[0] = new int[] { 1, 2 };
        jagged[1] = new int[] { 3, 4, 5 };

        Console.WriteLine(jagged.Length);
        Console.WriteLine(jagged[0].Length);
        Console.WriteLine(jagged[1].Length);

        Console.WriteLine(jagged[0][1]);
        Console.WriteLine(jagged[1][2]);

        foreach (var subArr in jagged)
        {
            foreach (var item in subArr)
            {
                Console.WriteLine(item);
            }
        }
    }
}
""";
            await RunTest(code);
        }
    }
}
