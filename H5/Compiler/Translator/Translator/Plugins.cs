using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using Microsoft.Extensions.Logging;
using Mono.Cecil;
using Mosaik.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ZLogger;

namespace H5.Translator
{
    public class Plugins : IPlugins
    {
        private static ILogger Logger = ApplicationLogging.CreateLogger<Plugins>();

        private const string PLUGIN_RESOURCE_NAME_PREFIX = "H5.Plugins.";

        public static bool IsLoaded { get; set; }

        public static string GetPluginPath(ITranslator translator, IAssemblyInfo config)
        {
            string path = null;

            if (!string.IsNullOrWhiteSpace(config.PluginsPath))
            {
                path = Path.Combine(Path.GetDirectoryName(translator.Location), config.PluginsPath);
            }
            else
            {
                path = Path.Combine(Path.GetDirectoryName(translator.Location), "H5" + Path.DirectorySeparatorChar + "plugins");
            }

            return path;
        }

        static Dictionary<string, AssemblyName> assemblyBindings = new Dictionary<string, AssemblyName>
        {
            { "System.ValueTuple", new AssemblyName("System.ValueTuple, Version=4.0.2.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51") },
            { "System.Collections.Immutable", new AssemblyName("System.Collections.Immutable, Version=1.2.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a") },
            { "System.IO.Compression", new AssemblyName("System.IO.Compression, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089") },
            { "System.Security.Cryptography.Algorithms", new AssemblyName("System.Security.Cryptography.Algorithms, Version=4.3.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a") },
            { "System.IO.FileSystem", new AssemblyName("System.IO.FileSystem, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a") },
            { "System.IO.FileSystem.Primitives", new AssemblyName("System.IO.FileSystem.Primitives, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a") },
            { "System.Security.Cryptography.Primitives", new AssemblyName("System.Security.Cryptography.Primitives, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a") },
            { "System.Xml.XPath.XDocument", new AssemblyName("System.Xml.XPath.XDocument, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a") },
            { "System.Diagnostics.FileVersionInfo", new AssemblyName("System.Diagnostics.FileVersionInfo, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a") }
        };

        protected class AssemblyResolver
        {
            private static ILogger Logger = ApplicationLogging.CreateLogger<AssemblyResolver>();

            public void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
            {
                Logger.ZLogTrace("Loaded assembly: {0}",  (args.LoadedAssembly is object ? args.LoadedAssembly.FullName : "none"));
            }

            public Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                var domain = sender as AppDomain;

                Logger.ZLogTrace("Domain {0} resolving assembly {1} requested by {2} ...", domain.FriendlyName, args.Name, (args.RequestingAssembly is object ? args.RequestingAssembly.FullName : "none"));

                AssemblyName askedAssembly = new AssemblyName(args.Name);
                Assembly assemblyLoaded;

                if (assemblyBindings.ContainsKey(askedAssembly.Name))
                {
                    assemblyLoaded = CheckIfFullAssemblyLoaded(assemblyBindings[askedAssembly.Name], domain);
                }
                else
                {
                    assemblyLoaded = CheckIfAssemblyLoaded(askedAssembly.Name, domain);
                }

                if (assemblyLoaded is object)
                {
                    Logger.ZLogTrace("Resolved for {0} in the loaded domain assemblies", assemblyLoaded.FullName);
                    return assemblyLoaded;
                }

                Logger.ZLogTrace("Did not find the assembly {0} in the loaded domain assemblies", args.Name);

                if (args.RequestingAssembly is object)
                {
                    assemblyLoaded = LoadAssemblyFromResources(Logger, args.RequestingAssembly, askedAssembly);

                    if (assemblyLoaded is object)
                    {
                        Logger.ZLogTrace("Resolved for {0} in {1} resources", assemblyLoaded.FullName, args.RequestingAssembly.FullName);
                        return assemblyLoaded;
                    }

                    assemblyLoaded = CheckAssemblyBinding(askedAssembly.Name);

                    if (assemblyLoaded is object)
                    {
                        Logger.ZLogTrace("Resolved for {0}", assemblyLoaded.FullName);
                        return assemblyLoaded;
                    }

                    Logger.LogTrace("Did not resolve assembly " + args.Name + " in " + args.RequestingAssembly.FullName + " resources");
                }
                else
                {
                    assemblyLoaded = CheckAssemblyBinding(askedAssembly.Name);

                    if (assemblyLoaded is object)
                    {
                        Logger.ZLogTrace("Resolved for {0}", assemblyLoaded.FullName);
                        return assemblyLoaded;
                    }

                    Logger.ZLogTrace("Did not resolve assembly {0}. Requesting assembly is null. Will not try to load the asked assembly in resources", args.Name);
                }

                return null;
            }

            private Stack<string> loadedStack = new Stack<string>();
            private Assembly CheckAssemblyBinding(string fullAssemblyName)
            {
                if (loadedStack.Contains(fullAssemblyName))
                {
                    return null;
                }

                if (assemblyBindings.ContainsKey(fullAssemblyName))
                {
                    loadedStack.Push(fullAssemblyName);
                    var asm = Assembly.Load(assemblyBindings[fullAssemblyName]);
                    loadedStack.Pop();
                    return asm;
                }
                return null;
            }

            public static Assembly CheckIfAssemblyLoaded(string fullAssemblyName, AppDomain domain)
            {
                var assemblies = domain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    var assemblyName = new AssemblyName(assembly.FullName);
                    if (assemblyName.Name == fullAssemblyName)
                    {
                        return assembly;
                    }
                }

                return null;
            }

            public static Assembly CheckIfFullAssemblyLoaded(AssemblyName name, AppDomain domain)
            {
                var assemblies = domain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    var assemblyName = new AssemblyName(assembly.FullName);
                    if (assemblyName.FullName == name.FullName)
                    {
                        return assembly;
                    }
                }

                return null;
            }
        }

        public static IPlugins GetPlugins(ITranslator translator, IAssemblyInfo config)
        {
            Logger.LogInformation("Discovering plugins...");

            if (!IsLoaded)
            {
                var resolver = new AssemblyResolver();

                AppDomain.CurrentDomain.AssemblyResolve += resolver.CurrentDomain_AssemblyResolve;

                AppDomain.CurrentDomain.AssemblyLoad += resolver.CurrentDomain_AssemblyLoad;

                IsLoaded = true;

                Logger.ZLogTrace("Set assembly Resolve and Load events for domain {0}.", AppDomain.CurrentDomain.FriendlyName);
            }

            Logger.ZLogTrace("Current domain {0}",  AppDomain.CurrentDomain.FriendlyName);

            Logger.ZLogTrace("Loaded assemblies:");

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                var location = a.IsDynamic ? "dynamic" : a.Location;
                Logger.ZLogTrace("\t{0} {1} {2}", a.FullName, location, a.GlobalAssemblyCache);
            }

            var path = GetPluginPath(translator, config);
            Logger.ZLogInformation("Will use the following plugin path '{0}'", path);

            var catalogs = new List<ComposablePartCatalog>();

            if (Directory.Exists(path))
            {
                catalogs.Add(new DirectoryCatalog(path, "*.dll"));
                Logger.LogInformation("The plugin path exists. Will use it as DirectoryCatalog");
            }
            else
            {
                Logger.LogInformation("The plugin path does not exist. Skipping searching test framework plugins in the plugin folder.");
            }

            string[] skipPluginAssemblies = null;
            if (translator is Translator translatorInstance)
            {
                skipPluginAssemblies = translatorInstance.SkipPluginAssemblies;
            }

            Logger.ZLogTrace("Will search all translator references to find resource(s) with names starting from '{0}' ...", PLUGIN_RESOURCE_NAME_PREFIX);

            foreach (var reference in translator.References)
            {
                Logger.ZLogTrace("Searching plugins in reference {0} ...", reference.FullName);

                if (skipPluginAssemblies is object && skipPluginAssemblies.FirstOrDefault(x => reference.Name.FullName.Contains(x)) is object)
                {
                    Logger.ZLogTrace("Skipping the reference {0} as it is in skipPluginAssemblies", reference.Name.FullName);
                    continue;
                }
                else
                {
                    Logger.ZLogTrace("skipPluginAssemblies is not set");
                }

                var assemblies = reference.MainModule.Resources.Where(res => res.Name.StartsWith(PLUGIN_RESOURCE_NAME_PREFIX)).ToArray();

                Logger.ZLogTrace("The reference contains {0} resource(s) needed", assemblies.Length);

                if (assemblies.Any())
                {
                    foreach (var res_assembly in assemblies)
                    {
                        Logger.ZLogTrace("Searching plugins in resource {0} ...", res_assembly.Name);

                        try
                        {
                            using (var resourcesStream = ((EmbeddedResource)res_assembly).GetResourceStream())
                            {
                                var ba = new byte[(int)resourcesStream.Length];
                                resourcesStream.Read(ba, 0, (int)resourcesStream.Length);

                                Logger.ZLogTrace("Read the assembly resource stream of {0} bytes length", resourcesStream.Length);

                                var trimmedName = TrimResourceAssemblyName(res_assembly, PLUGIN_RESOURCE_NAME_PREFIX);

                                var assembly = CheckIfAssemblyLoaded(ba, null, trimmedName);

                                catalogs.Add(new AssemblyCatalog(assembly));
                                Logger.ZLogTrace("The assembly {0} was added to the catalogs", assembly.FullName);
                            }
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            LogAssemblyLoaderException("Could not load assembly from resources", ex);
                        }
                        catch (Exception ex)
                        {
                            Logger.ZLogError(ex, "Could not load assembly from resources");
                        }
                    }
                }
            }

            if (catalogs.Count == 0)
            {
                Logger.ZLogInformation("No AssemblyCatalogs found");
                return new Plugins() { plugins = new IPlugin[0] };
            }

            var catalog = new AggregateCatalog(catalogs);

            CompositionContainer container = new CompositionContainer(catalog);
            var plugins = new Plugins();

            Logger.ZLogInformation("ComposingParts to discover plugins...");

            try
            {
                container.ComposeParts(plugins);
            }
            catch (ReflectionTypeLoadException ex)
            {
                LogAssemblyLoaderException("Could not compose Plugin parts", ex);
            }
            catch (Exception ex)
            {
                Logger.ZLogError(ex, "Could not compose Plugin parts.");
            }

            if (plugins.Parts is object)
            {
                Logger.ZLogInformation("Discovered {0} plugin(s)", plugins.Parts.Count());
            }

            return plugins;
        }

        public static string TrimResourceAssemblyName(Resource resource, string resourceNamePrefix)
        {
            var trimmedName = resource.Name;

            var i = trimmedName.IndexOf(resourceNamePrefix);
            if (i >= 0)
            {
                trimmedName = trimmedName.Remove(i, resourceNamePrefix.Length);
            }

            i = trimmedName.LastIndexOf(".dll");
            if (i >= 0)
            {
                trimmedName = trimmedName.Remove(i, 4);
            }

            return trimmedName;
        }

        public static string TrimAssemblyName(string assemblyName)
        {
            var trimmedName = assemblyName;

            var i = trimmedName.LastIndexOf(".dll");
            if (i >= 0)
            {
                trimmedName = trimmedName.Remove(i, 4);
            }

            return trimmedName;
        }

        public static Assembly LoadAssemblyFromResources(ILogger logger, Assembly sourceAssembly, AssemblyName assemblyName)
        {
            if (assemblyName == null)
            {
                Logger.LogWarning("Cannot try to load assembly from resources as the assemblyName is null");
            }

            if (sourceAssembly == null)
            {
                Logger.LogWarning("Cannot try to load assembly " + assemblyName.FullName + " from resources as the source assembly is null");
            }

            Logger.LogTrace("Trying to resolve " + assemblyName.FullName + " in the resources of " + sourceAssembly.FullName + " ...");

            Logger.LogTrace("Will use assembly name " + assemblyName.Name);

            var resourceNames = sourceAssembly.GetManifestResourceNames();

            foreach (var resourceName in resourceNames)
            {
                var trimmedResourceName = TrimAssemblyName(resourceName);

                Logger.LogTrace("Scanning resource " + resourceName + ". Trimmed resource name " + trimmedResourceName + " ...");

                if (trimmedResourceName == assemblyName.Name)
                {
                    Logger.LogTrace("Found resource with name " + resourceName);

                    using (var resourcesStream = sourceAssembly.GetManifestResourceStream(resourceName))
                    {
                        var ba = new byte[(int)resourcesStream.Length];
                        resourcesStream.Read(ba, 0, (int)resourcesStream.Length);

                        Logger.LogTrace("Read the assembly resource stream of " + resourcesStream.Length + " bytes length");

                        return CheckIfAssemblyLoaded(ba, null, resourceName);
                    }
                }
            }

            return null;
        }

        public static Assembly CheckIfAssemblyLoaded(byte[] ba, AssemblyName assemblyName, string trimmedName)
        {
            Logger.ZLogTrace("Check if assembly {0} is already loaded", trimmedName);

            Assembly assembly = AssemblyResolver.CheckIfAssemblyLoaded(trimmedName, AppDomain.CurrentDomain);
            if (assembly is object)
            {
                Logger.ZLogTrace("The assembly {0} is already loaded", trimmedName);
            }
            else
            {
                Logger.ZLogTrace("Loading the assembly into domain {0} ...", AppDomain.CurrentDomain.FriendlyName);

                if (ba is object)
                {
                    assembly = Assembly.Load(ba);
                }
                else
                {
                    assembly = Assembly.Load(assemblyName);
                }

                Logger.ZLogTrace("Assembly {0} is loaded into domain {1}", assembly.FullName, AppDomain.CurrentDomain.FriendlyName);
            }

            return assembly;
        }

        private static void LogAssemblyLoaderException(string reason, ReflectionTypeLoadException ex)
        {
            var sb = new StringBuilder(reason + ": ");

            sb.AppendLine(ex.ToString());

            if (ex.LoaderExceptions is object)
            {
                sb.AppendFormat("LoaderExceptions ({0} items): ", ex.LoaderExceptions.Length);

                foreach (var loaderException in ex.LoaderExceptions)
                {
                    sb.AppendLine(loaderException.Message);
                }
            }

            var message = sb.ToString();

            Logger.LogError(message);
        }

        [ImportMany]
        private IEnumerable<IPlugin> plugins;

        public IEnumerable<IPlugin> Parts
        {
            get
            {
                if (plugins == null)
                {
                    plugins = new List<IPlugin>();
                }

                return plugins;
            }
        }

        public void OnConfigRead(IAssemblyInfo config)
        {
            foreach (var plugin in Parts)
            {
                plugin.OnConfigRead(config);
            }
        }

        public IEnumerable<string> GetConstructorInjectors(IConstructorBlock constructorBlock)
        {
            IEnumerable<string> result = new List<string>();
            foreach (var plugin in Parts)
            {
                var answer = plugin.GetConstructorInjectors(constructorBlock);
                if (answer is object)
                {
                    result = result.Concat(answer);
                }
            }

            return result;
        }

        private InvocationInterceptor defaultInvocationInterceptor;

        private InvocationInterceptor GetInvocationInterceptor()
        {
            if (defaultInvocationInterceptor == null)
            {
                defaultInvocationInterceptor = new InvocationInterceptor();
            }

            defaultInvocationInterceptor.Cancel = false;
            defaultInvocationInterceptor.Replacement = null;
            return defaultInvocationInterceptor;
        }

        public IInvocationInterceptor OnInvocation(IAbstractEmitterBlock block, InvocationExpression expression, InvocationResolveResult resolveResult)
        {
            InvocationInterceptor interceptor = GetInvocationInterceptor();

            interceptor.Block = block;
            interceptor.Expression = expression;
            interceptor.ResolveResult = resolveResult;

            foreach (var plugin in Parts)
            {
                plugin.OnInvocation(interceptor);
                if (interceptor.Cancel)
                {
                    return interceptor;
                }
            }

            return interceptor;
        }

        private ReferenceInterceptor defaultReferenceInterceptor;

        private ReferenceInterceptor GetReferenceInterceptor()
        {
            if (defaultReferenceInterceptor == null)
            {
                defaultReferenceInterceptor = new ReferenceInterceptor();
            }

            defaultReferenceInterceptor.Cancel = false;
            defaultReferenceInterceptor.Replacement = null;
            return defaultReferenceInterceptor;
        }

        public IReferenceInterceptor OnReference(IAbstractEmitterBlock block, MemberReferenceExpression expression, MemberResolveResult resolveResult)
        {
            ReferenceInterceptor interceptor = GetReferenceInterceptor();

            interceptor.Block = block;
            interceptor.Expression = expression;
            interceptor.ResolveResult = resolveResult;

            foreach (var plugin in Parts)
            {
                plugin.OnReference(interceptor);
                if (interceptor.Cancel)
                {
                    return interceptor;
                }
            }

            return interceptor;
        }

        public bool HasConstructorInjectors(IConstructorBlock constructorBlock)
        {
            foreach (var plugin in Parts)
            {
                if (plugin.HasConstructorInjectors(constructorBlock))
                {
                    return true;
                }
            }

            return false;
        }

        public void BeforeEmit(IEmitter emitter, ITranslator translator)
        {
            foreach (var plugin in Parts)
            {
                plugin.BeforeEmit(emitter, translator);
            }
        }

        public void AfterEmit(IEmitter emitter, ITranslator translator)
        {
            foreach (var plugin in Parts)
            {
                plugin.AfterEmit(emitter, translator);
            }
        }

        public void AfterOutput(ITranslator translator, string outputPath)
        {
            foreach (var plugin in Parts)
            {
                plugin.AfterOutput(translator, outputPath);
            }
        }

        public void BeforeTypesEmit(IEmitter emitter, IList<ITypeInfo> types)
        {
            foreach (var plugin in Parts)
            {
                plugin.BeforeTypesEmit(emitter, types);
            }
        }

        public void AfterTypesEmit(IEmitter emitter, IList<ITypeInfo> types)
        {
            foreach (var plugin in Parts)
            {
                plugin.AfterTypesEmit(emitter, types);
            }
        }

        public void BeforeTypeEmit(IEmitter emitter, ITypeInfo type)
        {
            foreach (var plugin in Parts)
            {
                plugin.BeforeTypeEmit(emitter, type);
            }
        }

        public void AfterTypeEmit(IEmitter emitter, ITypeInfo type)
        {
            foreach (var plugin in Parts)
            {
                plugin.AfterTypeEmit(emitter, type);
            }
        }
    }
}