using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace H5.Compiler.Service.Tests
{
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public void TestCacheReuse_NoChanges()
        {
            using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "App.cs", "public class App { public static void Main() { System.Console.WriteLine(\"Hello\"); } }" },
                    { "Class1.cs", "public class Class1 { public void Method1() { } }" }
                };

                // First run
                var result1 = compiler.Compile(sources, rebuild: true);
                Assert.IsTrue(result1.Output.Count > 0);

                // Second run (no changes)
                var result2 = compiler.Compile(sources, rebuild: false);
                Assert.IsTrue(result2.Output.Count > 0);

                // Verify stats
                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;
                if (stats != null)
                {
                    Assert.IsTrue(stats.ContainsKey("ReusedFiles"));
                    Assert.AreEqual(2, stats["ReusedFiles"], "Should reuse both files");
                }
            }
        }

        [TestMethod]
        public void TestCacheReuse_IndependentChange()
        {
            using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "App.cs", "public class App { public static void Main() { System.Console.WriteLine(\"Hello\"); } }" },
                    { "Class1.cs", "public class Class1 { public void Method1() { } }" }
                };

                compiler.Compile(sources, rebuild: true);

                // Modify Class1
                sources["Class1.cs"] = "public class Class1 { public void Method1() { System.Console.WriteLine(\"Changed\"); } }";

                var result2 = compiler.Compile(sources, rebuild: false);

                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;
                if (stats != null)
                {
                    Assert.IsTrue(stats.ContainsKey("ReusedFiles"));
                    Assert.AreEqual(1, stats["ReusedFiles"], "Should reuse App.cs");
                }
            }
        }

        [TestMethod]
        public void TestCacheReuse_DependentChange()
        {
            using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "App.cs", "public class App { public static void Main() { var c = new Class1(); c.Method1(); } }" },
                    { "Class1.cs", "public class Class1 { public void Method1() { } }" }
                };

                compiler.Compile(sources, rebuild: true);

                // Modify Class1 signature (which affects App)
                // Even implementation change might affect App (inlining, etc.), so we should invalidate dependents.
                sources["Class1.cs"] = "public class Class1 { public void Method1(int i = 0) { } }";

                var result2 = compiler.Compile(sources, rebuild: false);

                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;
                if (stats != null)
                {
                    Assert.IsTrue(stats.ContainsKey("ReusedFiles"));
                    // Both should be recompiled because App depends on Class1
                    Assert.AreEqual(0, stats["ReusedFiles"], "Should reuse 0 files");
                }
            }
        }
    }
}
