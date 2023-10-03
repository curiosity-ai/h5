using System;
using H5;
using static H5.Core.dom;

namespace Placeholder
{
    internal static class App
    {
        private static void Main()
        {
            if (true)
            {
                void Test() { }

                if (true)
                {
                    void Test1() { }
                    Test1();

                    if (true)
                    {
                        void Test2() { }
                        Test2();
                        if (true)
                        {
                            void Test3() { }
                            Test3();
                            
                            if (true)
                            {
                                void Test4() { }
                                Test4();
                            }
                        }
                    }
                }
            }
            //var hello = "Hello";
            //var world = World();
            //document.body.innerHTML = $"{hello} {world}!";
            //string World() => "World";
        }
    }
}
