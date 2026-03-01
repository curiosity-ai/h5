using System;

namespace TestNamespace
{
    public interface ITest
    {
        void DoWork();
    }

    public class Person : ITest
    {
        public static readonly string Species = "Human";
        public string Name { get; set; }
        public int Age;

        public Person(string name)
        {
            Name = name;
        }

        public void DoWork() {}
    }

    public struct Point<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
    }
}
