using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class ZzRefTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task Ref1()
        {
            var code = """
using System;
using System.Collections.Generic;
public abstract class Animal
{
    public string Name { get; set; }
    protected int legs;
    private static int Count;
    public Animal(string name, int legs) { Name = name; this.legs = legs; Count++; }
    public abstract string Speak();
    public virtual string Describe() => Name + ":" + Speak();
    public const int MaxLegs = 8;
}
public class Dog : Animal
{
    public Dog(string name) : base(name, 4) { }
    public override string Speak() => "Woof";
}
public interface IShape { double Area(); }
public struct Vec { public int X, Y; public Vec(int x,int y){X=x;Y=y;} }
public enum Color { Red, Green = 5 }
public class Program
{
    static int Add(int a, int b) => a + b;
    public static void Main()
    {
        Animal a = new Dog("Rex");
        Console.WriteLine(a.Describe());
        var list = new List<int> { 1, 2, 3 };
        Console.WriteLine(list.Count);
        Vec v = new Vec(1,2);
        Console.WriteLine(v.X + v.Y);
        Console.WriteLine((int)Color.Green);
        Console.WriteLine(Add(2,3));
    }
}
""";
            await RunTest(code, skipRoslyn: true);
        }
    }
}
