using System;
using Xunit;

namespace H5.Compiler.Next.Tests
{
    public class IntegrationTests
    {
        private readonly IntegrationTestRunner _runner = new IntegrationTestRunner();

        [Fact]
        public void Test1_SimpleReturnValue()
        {
            var source = @"
            namespace TestNs {
                public class Calc {
                    public static int GetValue() {
                        return 42;
                    }
                }
            }";

            _runner.AssertExecution(source, "TestNs.Calc.GetValue();", 42);
        }

        [Fact]
        public void Test2_FieldAccess()
        {
            var source = @"
            namespace TestNs {
                public class Counter {
                    public int Count;

                    public Counter() {
                        Count = 10;
                    }

                    public void Increment() {
                        Count = Count + 1;
                    }
                }
            }";

            var js = @"
            var c = new TestNs.Counter();
            c.Increment();
            c.Count;
            ";

            _runner.AssertExecution(source, js, 11);
        }

        [Fact]
        public void Test3_PropertyAccess()
        {
            var source = @"
            namespace TestNs {
                public class Box {
                    public int Value { get; set; }
                }
            }";

            var js = @"
            var b = new TestNs.Box();
            b.Value = 99;
            b.Value;
            ";

            _runner.AssertExecution(source, js, 99);
        }

        [Fact]
        public void Test4_GenericClass()
        {
            var source = @"
            namespace TestNs {
                public class Container<T> {
                    public T Item;
                }
            }";

            var js = @"
            var containerType = TestNs.Container(String);
            var c = new containerType();
            c.Item = 'Hello';
            c.Item;
            ";

            _runner.AssertExecution(source, js, "Hello");
        }
    }
}
