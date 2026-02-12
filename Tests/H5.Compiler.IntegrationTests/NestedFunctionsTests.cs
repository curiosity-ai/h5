using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class NestedFunctionsTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task LocalFunctionInsideLambda()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Func<Task> lambda = async () =>
        {
            Console.WriteLine("Lambda");
            async Task Local()
            {
                Console.WriteLine("Local Function");
            }
            await Local();
        };
        await lambda();
        Console.WriteLine("<<DONE>>");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task MinimumFailing_LocalLambdaLocal()
        {
             var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        async Task L1()
        {
             Console.WriteLine("L1");
             Func<Task> L2 = async () =>
             {
                 Console.WriteLine("L2");
                 async Task L3()
                 {
                     Console.WriteLine("L3");
                 }
                 await L3();
             };
             await L2();
        }
        await L1();
        Console.WriteLine("<<DONE>>");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task MinimumFailing_DeeplyNestedLocals()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        async Task L1()
        {
            Console.WriteLine("L1");
            async Task L2()
            {
                Console.WriteLine("L2");
                async Task L3()
                {
                    Console.WriteLine("L3");
                }
                await L3();
            }
            await L2();
        }
        await L1();
        Console.WriteLine("<<DONE>>");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task MinimumPassing_DeeplyNestedLambdas()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start");
        Func<Task> L1 = async () =>
        {
            Console.WriteLine("L1");
            Func<Task> L2 = async () =>
            {
                Console.WriteLine("L2");
                Func<Task> L3 = async () =>
                {
                    Console.WriteLine("L3");
                };
                await L3();
            };
            await L2();
        };
        await L1();
        Console.WriteLine("<<DONE>>");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task DeeplyNestedSyncLocals()
        {
             var code = """
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Start");
        void L1()
        {
            Console.WriteLine("L1");
            void L2()
            {
                Console.WriteLine("L2");
                void L3()
                {
                    Console.WriteLine("L3");
                }
                L3();
            }
            L2();
        }
        L1();
        Console.WriteLine("<<DONE>>");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task DeeplyNestedLambdas20()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("L1");
        Func<Task> l2 = async () => { Console.WriteLine("L2");
        Func<Task> l3 = async () => { Console.WriteLine("L3");
        Func<Task> l4 = async () => { Console.WriteLine("L4");
        Func<Task> l5 = async () => { Console.WriteLine("L5");
        Func<Task> l6 = async () => { Console.WriteLine("L6");
        Func<Task> l7 = async () => { Console.WriteLine("L7");
        Func<Task> l8 = async () => { Console.WriteLine("L8");
        Func<Task> l9 = async () => { Console.WriteLine("L9");
        Func<Task> l10 = async () => { Console.WriteLine("L10");
        Func<Task> l11 = async () => { Console.WriteLine("L11");
        Func<Task> l12 = async () => { Console.WriteLine("L12");
        Func<Task> l13 = async () => { Console.WriteLine("L13");
        Func<Task> l14 = async () => { Console.WriteLine("L14");
        Func<Task> l15 = async () => { Console.WriteLine("L15");
        Func<Task> l16 = async () => { Console.WriteLine("L16");
        Func<Task> l17 = async () => { Console.WriteLine("L17");
        Func<Task> l18 = async () => { Console.WriteLine("L18");
        Func<Task> l19 = async () => { Console.WriteLine("L19");
        Func<Task> l20 = async () => { Console.WriteLine("L20"); };
        await l20(); };
        await l19(); };
        await l18(); };
        await l17(); };
        await l16(); };
        await l15(); };
        await l14(); };
        await l13(); };
        await l12(); };
        await l11(); };
        await l10(); };
        await l9(); };
        await l8(); };
        await l7(); };
        await l6(); };
        await l5(); };
        await l4(); };
        await l3(); };
        await l2();
        Console.WriteLine("<<DONE>>");
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }

        [TestMethod]
        public async Task DeeplyNestedFunctions()
        {
            var code = """
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("L1: Main");
        await Level2();
        Console.WriteLine("<<DONE>>");
    }

    static async Task Level2()
    {
        Console.WriteLine("L2: Local Function Wrapper");

        async Task Level3()
        {
            Console.WriteLine("L3: Local Function");

            Func<Task> Level4 = async () =>
            {
                Console.WriteLine("L4: Lambda");

                async Task Level5()
                {
                    Console.WriteLine("L5: Local Function");
                    await NestedL5.Level6();
                }
                await Level5();
            };
            await Level4();
        }
        await Level3();
    }

    class NestedL5
    {
        public static async Task Level6()
        {
            Console.WriteLine("L6: Static Method");
            var l7 = new NestedL7();
            await l7.Level7();
        }
    }

    class NestedL7
    {
        public async Task Level7()
        {
            Console.WriteLine("L7: Instance Method");

            async Task Level8()
            {
                Console.WriteLine("L8: Local Function");

                Func<Task> Level9 = async () =>
                {
                    Console.WriteLine("L9: Lambda");

                    async Task Level10()
                    {
                         Console.WriteLine("L10: Local Function");
                         await NestedL11.Level11();
                    }
                    await Level10();
                };
                await Level9();
            }
            await Level8();
        }
    }

    class NestedL11
    {
        public static async Task Level11()
        {
             Console.WriteLine("L11: Static Method");

             async Task Level12()
             {
                 Console.WriteLine("L12: Local Function");

                 Func<Task> Level13 = async () =>
                 {
                     Console.WriteLine("L13: Lambda");

                     async Task Level14()
                     {
                         Console.WriteLine("L14: Local Function");
                         await new NestedL15().Level15();
                     }
                     await Level14();
                 };
                 await Level13();
             }
             await Level12();
        }
    }

    class NestedL15
    {
        public async Task Level15()
        {
            Console.WriteLine("L15: Instance Method");

            async Task Level16()
            {
                Console.WriteLine("L16: Local Function");

                Func<Task> Level17 = async () =>
                {
                    Console.WriteLine("L17: Lambda");

                    async Task Level18()
                    {
                        Console.WriteLine("L18: Local Function");

                        Func<Task> Level19 = async () =>
                        {
                            Console.WriteLine("L19: Lambda");

                            void Level20()
                            {
                                Console.WriteLine("L20: Sync Local Function");
                                NestedL21.Level21();
                            }
                            Level20();
                            await Task.Delay(1);
                        };
                        await Level19();
                    }
                    await Level18();
                };
                await Level17();
            }
            await Level16();
        }
    }

    class NestedL21
    {
        public static void Level21()
        {
            Console.WriteLine("L21: Static Sync Method (Deepest)");
        }
    }
}
""";
            await RunTest(code, "<<DONE>>");
        }
    }
}
