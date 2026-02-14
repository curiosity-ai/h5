using Microsoft.Build.Locator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace H5.Compiler.Service.Tests
{
    [TestClass]
    public static class TestInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            try
            {
                var instances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
                if (instances.Length > 0)
                {
                    var instance = instances.OrderByDescending(x => x.Version).First();
                    MSBuildLocator.RegisterInstance(instance);
                    Console.WriteLine($"Registered MSBuild instance: {instance.Name} {instance.Version} at {instance.MSBuildPath}");
                }
                else
                {
                    MSBuildLocator.RegisterDefaults();
                    Console.WriteLine("Registered default MSBuild instance.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSBuildLocator initialization warning: {ex.Message}");
            }
        }
    }
}
