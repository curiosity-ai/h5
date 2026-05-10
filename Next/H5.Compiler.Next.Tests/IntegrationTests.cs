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

        [Fact]
        public void Test5_LocalVariables()
        {
            var source = @"
            namespace TestNs {
                public class MathHelper {
                    public int Add(int a, int b) {
                        int c = a + b;
                        int d = c * 2;
                        return d;
                    }
                }
            }";

            var js = @"
            var helper = new TestNs.MathHelper();
            helper.Add(5, 3);
            ";

            _runner.AssertExecution(source, js, 16);
        }

        [Fact]
        public void Test6_IfStatement()
        {
            var source = @"
            namespace TestNs {
                public class Conditions {
                    public string Check(int a) {
                        if (a > 10) {
                            return ""Big"";
                        } else if (a == 10) {
                            return ""Exact"";
                        } else {
                            return ""Small"";
                        }
                    }
                }
            }";

            var js = @"
            var c = new TestNs.Conditions();
            [c.Check(15), c.Check(10), c.Check(5)].join(',');
            ";

            _runner.AssertExecution(source, js, "Big,Exact,Small");
        }

        [Fact]
        public void Test7_ForLoop()
        {
            var source = @"
            namespace TestNs {
                public class Looper {
                    public int Sum(int max) {
                        int total = 0;
                        for (int i = 0; i < max; i = i + 1) {
                            total = total + i;
                        }
                        return total;
                    }
                }
            }";

            var js = @"
            var l = new TestNs.Looper();
            l.Sum(5);
            ";

            _runner.AssertExecution(source, js, 10);
        }

        [Fact]
        public void Test8_WhileLoop()
        {
            var source = @"
            namespace TestNs {
                public class WhileLooper {
                    public int Factorial(int n) {
                        int result = 1;
                        int i = n;
                        while (i > 1) {
                            result = result * i;
                            i = i - 1;
                        }
                        return result;
                    }
                }
            }";

            var js = @"
            var l = new TestNs.WhileLooper();
            l.Factorial(4);
            ";

            _runner.AssertExecution(source, js, 24);
        }
    }
}
