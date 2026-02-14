using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace H5.Compiler.Service.Tests
{
    [TestClass]
    public class CacheEdgeCasesTests
    {
        [TestMethod]
        public void TestCacheReuse_PartialClass_SameFile()
        {
            // Simple case: partial in one file (not really partial but using partial keyword)
            using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "App.cs", "public partial class App { public static void Main() { System.Console.WriteLine(\"Hello\"); } }" }
                };

                var result1 = compiler.Compile(sources, rebuild: true);
                Assert.IsTrue(result1.Output.Count > 0);

                // Modify App
                sources["App.cs"] = "public partial class App { public static void Main() { System.Console.WriteLine(\"Changed\"); } }";

                var result2 = compiler.Compile(sources, rebuild: false);
                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;
                Assert.AreEqual(0, stats["ReusedFiles"]);
            }
        }

        [TestMethod]
        public void TestCacheReuse_PartialClass_MultipleFiles()
        {
            using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "App.Part1.cs", "public partial class App { public static void Main() { Part2(); } }" },
                    { "App.Part2.cs", "public partial class App { static void Part2() { System.Console.WriteLine(\"Part2\"); } }" }
                };

                compiler.Compile(sources, rebuild: true);

                // Modify Part2
                sources["App.Part2.cs"] = "public partial class App { static void Part2() { System.Console.WriteLine(\"Changed\"); } }";

                var result2 = compiler.Compile(sources, rebuild: false);
                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;

                // Both files contribute to App. Modifying one should invalidate App.
                // Since App is emitted as one unit (likely), both source files should be considered involved.
                // Or at least ReusedFiles should NOT contain App.Part1.cs if App is re-emitted using it.
                // If ReusedFiles is 1 (Part1 reused), but App is re-emitted, then it's fine?
                // Wait, if Part1 is reused, it means we didn't re-read/re-parse it?
                // But to emit App, we need both parts.
                // If the compiler re-emits App, it MUST read both files.
                // So ReusedFiles should be 0.
                // However, ReusedFiles is calculated as SourceFiles.Count - emittedFiles.Count.
                // emittedFiles only tracks the primary file for the type.
                // Since App spans 2 files but is emitted as 1 type, emittedFiles.Count is 1.
                // So ReusedFiles = 2 - 1 = 1.
                Assert.AreEqual(1, stats["ReusedFiles"], "Should invalidate App. (2 source files - 1 emitted type = 1 'reused' count artifact)");

                // Verify content to be sure
                var js = result2.Output.Values.FirstOrDefault(v => v.Contains("Changed"));
                Assert.IsNotNull(js, "Output should contain the changed string");
            }
        }

        [TestMethod]
        public void TestCacheReuse_ChainedDependencies()
        {
            using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "App.cs", "public class App { public void Run() { new ClassB().Run(); } }" },
                    { "ClassB.cs", "public class ClassB { public void Run() { new ClassA().Run(); } }" },
                    { "ClassA.cs", "public class ClassA { public void Run() { System.Console.WriteLine(\"A\"); } }" }
                };

                compiler.Compile(sources, rebuild: true);

                // Modify ClassA
                sources["ClassA.cs"] = "public class ClassA { public void Run() { System.Console.WriteLine(\"Changed\"); } }";

                var result2 = compiler.Compile(sources, rebuild: false);
                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;

                // A changed -> B depends on A -> App depends on B.
                // All 3 should be invalidated.
                Assert.AreEqual(0, stats["ReusedFiles"], "Should invalidate all files due to chain.");
            }
        }

        [TestMethod]
        public void TestCacheReuse_CircularDependencies()
        {
            using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "ClassA.cs", "public class ClassA { public void CallB() { new ClassB().CallA(); } }" },
                    { "ClassB.cs", "public class ClassB { public void CallA() { new ClassA().CallB(); } }" }
                };

                compiler.Compile(sources, rebuild: true);

                // Modify ClassA
                sources["ClassA.cs"] = "public class ClassA { public void CallB() { System.Console.WriteLine(\"Changed\"); new ClassB().CallA(); } }";

                var result2 = compiler.Compile(sources, rebuild: false);
                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;

                // A changed -> B depends on A (invalidated) -> A depends on B (already invalidated).
                Assert.AreEqual(0, stats["ReusedFiles"], "Should invalidate both files.");
            }
        }

        [TestMethod]
        public void TestCacheReuse_NestedClass_ModifyNested()
        {
            using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "Container.cs", "public class Container { public class Nested { public void Run() {} } }" },
                    { "App.cs", "public class App { public void Main() { new Container.Nested().Run(); } }" }
                };

                compiler.Compile(sources, rebuild: true);

                // Modify Nested class in Container.cs
                sources["Container.cs"] = "public class Container { public class Nested { public void Run() { System.Console.WriteLine(\"Changed\"); } } }";

                var result2 = compiler.Compile(sources, rebuild: false);
                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;

                // Container changed. App depends on Nested (which is in Container).
                // So App should be invalidated.
                Assert.AreEqual(0, stats["ReusedFiles"], "Should invalidate both files.");
            }
        }

        [TestMethod]
        public void TestCacheReuse_GenericClass_ModifyGeneric()
        {
             using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "Generic.cs", "public class G<T> { public T Value; }" },
                    { "App.cs", "public class App { public void Run() { var g = new G<int>(); g.Value = 1; } }" }
                };

                compiler.Compile(sources, rebuild: true);

                // Modify Generic
                sources["Generic.cs"] = "public class G<T> { public T Value; public void NewMethod() {} }";

                var result2 = compiler.Compile(sources, rebuild: false);
                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;

                // Generic changed. App uses it. App should be invalidated.
                Assert.AreEqual(0, stats["ReusedFiles"]);
            }
        }

        [TestMethod]
        public void TestCacheReuse_ConstField_Propagate()
        {
            using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "Lib.cs", "public class Lib { public const int Val = 1; }" },
                    { "App.cs", "public class App { public void Run() { System.Console.WriteLine(Lib.Val); } }" }
                };

                compiler.Compile(sources, rebuild: true);

                // Modify Lib constant
                sources["Lib.cs"] = "public class Lib { public const int Val = 2; }";

                var result2 = compiler.Compile(sources, rebuild: false);
                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;

                // Lib changed. App uses Lib.Val (inlined). App must be invalidated.
                Assert.AreEqual(0, stats["ReusedFiles"], "App should be invalidated because it uses a changed const.");

                var js = string.Join(Environment.NewLine, result2.Output.Values);
                Assert.IsTrue(js.Contains("2"), "Output should contain the new const value '2'.");
            }
        }

        [TestMethod]
        public void TestCacheReuse_Enum_Propagate()
        {
            using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "Lib.cs", "public enum E { V1 = 1 }" },
                    { "App.cs", "public class App { public void Run() { System.Console.WriteLine((int)E.V1); } }" }
                };

                compiler.Compile(sources, rebuild: true);

                // Modify Enum value
                sources["Lib.cs"] = "public enum E { V1 = 2 }";

                var result2 = compiler.Compile(sources, rebuild: false);
                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;

                // Enum changed. App uses E.V1. App must be invalidated.
                Assert.AreEqual(0, stats["ReusedFiles"], "App should be invalidated because it uses a changed enum value.");

                var js = string.Join(Environment.NewLine, result2.Output.Values);
                Assert.IsTrue(js.Contains("2"), "Output should contain the new enum value '2'.");
            }
        }

        [TestMethod]
        public void TestCacheReuse_InlineMethod_Template_Change()
        {
            using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "Lib.cs", "using H5; public class Lib { [Template(\"console.log('Old')\")] public static void Foo() {} }" },
                    { "App.cs", "public class App { public void Run() { Lib.Foo(); } }" }
                };

                compiler.Compile(sources, rebuild: true);

                // Modify Template
                sources["Lib.cs"] = "using H5; public class Lib { [Template(\"console.log('New')\")] public static void Foo() {} }";

                var result2 = compiler.Compile(sources, rebuild: false);
                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;

                // Template changed. App uses Lib.Foo (inlined). App must be invalidated.
                Assert.AreEqual(0, stats["ReusedFiles"], "App should be invalidated because it uses a method with changed [Template].");

                var js = string.Join(Environment.NewLine, result2.Output.Values);
                Assert.IsTrue(js.Contains("New"), "Output should contain the new template content 'New'.");
            }
        }

        [TestMethod]
        public void TestCacheReuse_Struct_Change()
        {
            using (var compiler = new TestCompiler())
            {
                var sources = new Dictionary<string, string>
                {
                    { "Struct.cs", "public struct S { public int X; }" },
                    { "App.cs", "public class App { public void Run() { var s = new S(); s.X = 1; } }" }
                };

                compiler.Compile(sources, rebuild: true);

                // Modify Struct
                sources["Struct.cs"] = "public struct S { public int X; public int Y; }";

                var result2 = compiler.Compile(sources, rebuild: false);
                var stats = result2.GetType().GetProperty("Stats")?.GetValue(result2) as Dictionary<string, int>;

                // Struct changed. App depends on it. Should invalidate.
                Assert.AreEqual(0, stats["ReusedFiles"], "App should be invalidated because struct changed.");
            }
        }
    }
}
