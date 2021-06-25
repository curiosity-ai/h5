using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Microsoft.Extensions.Logging;
using Mono.Cecil;
using Mosaik.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopologicalSorting;
using ZLogger;

namespace H5.Translator
{
    public partial class Emitter
    {
        public virtual int CompareTypeInfosByName(ITypeInfo x, ITypeInfo y)
        {
            if (x == y)
            {
                return 0;
            }

            if (x.Key == CS.NS.H5)
            {
                return -1;
            }

            if (y.Key == CS.NS.H5)
            {
                return 1;
            }

            if (!TypeDefinitions.ContainsKey(x.Key))
            {
                throw new TranslatorException("Class with name '" + x.Key + "' is not found in the assembly, probably rebuild is required");
            }

            if (!TypeDefinitions.ContainsKey(y.Key))
            {
                throw new TranslatorException("Class with name '" + y.Key + "' is not found in the assembly, probably rebuild is required");
            }

            var xTypeDefinition = TypeDefinitions[x.Key];
            var yTypeDefinition = TypeDefinitions[y.Key];

            return xTypeDefinition.FullName.CompareTo(yTypeDefinition.FullName);
        }

        public virtual int CompareTypeInfosByPriority(ITypeInfo x, ITypeInfo y)
        {
            if (x == y)
            {
                return 0;
            }

            if (x.Key == CS.NS.H5)
            {
                return -1;
            }

            if (y.Key == CS.NS.H5)
            {
                return 1;
            }

            var xZero = x.Key == "0";
            var yZero = y.Key == "0";
            var xTypeDefinition = xZero ? null : TypeDefinitions[x.Key];
            var yTypeDefinition = yZero ? null : TypeDefinitions[y.Key];

            var xPriority = xZero ? 0 : GetPriority(xTypeDefinition);
            var yPriority = yZero ? 0 : GetPriority(yTypeDefinition);

            return -xPriority.CompareTo(yPriority);
        }

        public virtual bool IsInheritedFrom(ITypeInfo x, ITypeInfo y)
        {
            if (x == y)
            {
                return false;
            }

            var inherits = false;
            var xTypeDefinition = TypeDefinitions[x.Key];
            var yTypeDefinition = TypeDefinitions[y.Key];

            if (Helpers.IsSubclassOf(xTypeDefinition, yTypeDefinition, this) ||
                (yTypeDefinition.IsInterface && Helpers.IsImplementationOf(xTypeDefinition, yTypeDefinition, this)) ||
                Helpers.IsTypeArgInSubclass(xTypeDefinition, yTypeDefinition, this))
            {
                inherits = true;
            }

            return inherits;
        }

        private List<ITypeInfo> SortByPriority(IList<ITypeInfo> list)
        {
            List<ITypeInfo> sortable = new List<ITypeInfo>();
            List<ITypeInfo> nonSortable = new List<ITypeInfo>();
            for (int i = 0; i < list.Count; i++)
            {
                if (GetPriority(TypeDefinitions[list[i].Key]) == 0)
                {
                    nonSortable.Add(list[i]);
                }
                else
                {
                    sortable.Add(list[i]);
                }
            }

            var zeroPlaceholder = new TypeInfo() { Key = "0" };
            sortable.Add(zeroPlaceholder);
            sortable.Sort(CompareTypeInfosByPriority);

            var idx = sortable.FindIndex(t => t.Key == "0");
            sortable.RemoveAt(idx);
            sortable.InsertRange(idx, nonSortable);

            return sortable;
        }

        public virtual void SortTypesByInheritance()
        {
            using (new Measure(Logger, "Sorting types by inheritance", logLevel: LogLevel.Trace))
            {
                if (Types.Count > 0)
                {
                    TopologicalSort();

                    //this.Types.Sort has strange effects for items with 0 priority

                    Logger.ZLogTrace("Priority sorting...");

                    Types = SortByPriority(Types);

                    Logger.ZLogTrace("Priority sorting done");
                }
                else
                {
                    Logger.ZLogTrace("No types to sort");
                }
            }
        }

        private Stack<IType> activeTypes;
        private Dictionary<IType, IList<ITypeInfo>> cacheParents = new Dictionary<IType, IList<ITypeInfo>>();

        public IList<ITypeInfo> GetParents(IType type, List<ITypeInfo> list = null)
        {
            IList<ITypeInfo> result;

            if (cacheParents.TryGetValue(type, out result))
            {
                list?.AddRange(result);
                return result;
            }

            bool endPoint = list == null;
            if (endPoint)
            {
                activeTypes = new Stack<IType>();
                list = new List<ITypeInfo>();
            }

            var typeDef = type.GetDefinition() ?? type;

            if (activeTypes.Contains(typeDef))
            {
                return list;
            }

            activeTypes.Push(typeDef);

            var types = type.GetAllBaseTypes();
            var thisTypelist = new List<ITypeInfo>();
            foreach (var t in types)
            {
                var bType = H5Types.Get(t, true);

                if (bType?.TypeInfo != null && !bType.Type.Equals(typeDef))
                {
                    thisTypelist.Add(bType.TypeInfo);
                }

                if (t.TypeArguments.Count > 0)
                {
                    foreach (var typeArgument in t.TypeArguments)
                    {
                        bType = H5Types.Get(typeArgument, true);
                        if (bType?.TypeInfo != null && !bType.Type.Equals(typeDef))
                        {
                            thisTypelist.Add(bType.TypeInfo);
                        }

                        GetParents(typeArgument, thisTypelist);
                    }
                }
            }
            list.AddRange(thisTypelist);
            activeTypes.Pop();
            list = list.Distinct().ToList();
            cacheParents[type] = list;

            return list;
        }

        public string GetReflectionName(IType type)
        {
            string name = null;
            if (nameCache.TryGetValue(type, out name))
            {
                return name;
            }

            name = type.ReflectionName;
            nameCache[type] = name;

            return name;
        }

        private Dictionary<IType, string> nameCache = new Dictionary<IType, string>();
        public virtual void TopologicalSort()
        {
            using (new Measure(Logger, "Topological Sorting", logLevel: LogLevel.Trace))
            {

                var graph = new DependencyGraph();

                Logger.ZLogTrace("\tTopological sorting first iteration...");

                var hitCounters = new long[7];

                foreach (var t in Types)
                {
                    hitCounters[0]++;
                    var parents = GetParents(t.Type);
                    var reflectionName = GetReflectionName(t.Type);
                    var tProcess = graph.Processes.FirstOrDefault(p => p.Name == reflectionName);
                    if (tProcess == null)
                    {
                        hitCounters[1]++;
                        tProcess = new OrderedProcess(graph, reflectionName);
                    }

                    for (int i = parents.Count - 1; i > -1; i--)
                    {
                        hitCounters[2]++;
                        var x = parents[i];
                        reflectionName = GetReflectionName(x.Type);
                        if (tProcess.Predecessors.All(p => p.Name != reflectionName))
                        {
                            hitCounters[3]++;

                            var dProcess = graph.Processes.FirstOrDefault(p => p.Name == reflectionName);
                            if (dProcess == null)
                            {
                                hitCounters[4]++;
                                dProcess = new OrderedProcess(graph, reflectionName);
                            }

                            if (tProcess != dProcess && dProcess.Predecessors.All(p => p.Name != tProcess.Name))
                            {
                                hitCounters[4]++;
                                tProcess.After(dProcess);
                            }
                        }
                    }
                }

                for (int i = 0; i < hitCounters.Length; i++)
                {
                    Logger.ZLogTrace("\t\tHitCounter{0} = {1}", i, hitCounters[i]);
                }

                Logger.ZLogTrace("\tTopological sorting first iteration done");

                if (graph.ProcessCount > 0)
                {
                    ITypeInfo tInfo = null;
                    OrderedProcess handlingProcess = null;
                    try
                    {
                        Logger.ZLogTrace("\tTopological sorting third iteration...");

                        System.Array.Clear(hitCounters, 0, hitCounters.Length);

                        Logger.ZLogTrace("\t\tCalculate sorting...");
                        TopologicalSort sorted = graph.CalculateSort();
                        Logger.ZLogTrace("\t\tCalculate sorting done");

                        Logger.ZLogTrace("\t\tGetting Reflection names for {0} types...", Types.Count);

                        var list = new List<ITypeInfo>(Types.Count);
                        // The fix required for Mono 5.0.0.94
                        // It does not "understand" TopologicalSort's Enumerator in foreach
                        // foreach (var processes in sorted)
                        // The code is modified to get it "directly" and "typed"
                        var sortedISetEnumerable = sorted as IEnumerable<ISet<OrderedProcess>>;
                        Logger.ZLogTrace("\t\tGot Enumerable<ISet<OrderedProcess>>");

                        var sortedISetEnumerator = sortedISetEnumerable.GetEnumerator();
                        Logger.ZLogTrace("\t\tGot Enumerator<ISet<OrderedProcess>>");

                        while (sortedISetEnumerator.MoveNext())
                        {
                            var processes = sortedISetEnumerator.Current;

                            hitCounters[0]++;

                            foreach (var process in processes)
                            {
                                handlingProcess = process;
                                hitCounters[1]++;

                                tInfo = Types.First(ti => GetReflectionName(ti.Type) == process.Name);

                                var reflectionName = GetReflectionName(tInfo.Type);

                                if (list.All(t => GetReflectionName(t.Type) != reflectionName))
                                {
                                    hitCounters[2]++;
                                    list.Add(tInfo);
                                }
                            }
                        }

                        Logger.ZLogTrace("\t\tGetting Reflection names done");

                        Types.Clear();
                        Types.AddRange(list);

                        for (int i = 0; i < hitCounters.Length; i++)
                        {
                            Logger.ZLogTrace("\t\tHitCounter{0} = {1}", i, hitCounters[i]);
                        }

                        Logger.ZLogTrace("\tTopological sorting third iteration done");
                    }
                    catch (System.Exception ex)
                    {
                        Logger.LogWarning($"Topological sort failed {(tInfo != null || handlingProcess != null ? "at type " + (tInfo != null ? tInfo.Type.ReflectionName : handlingProcess.Name) : string.Empty)} with error {ex}");
                    }
                }
                cacheParents = null;
                activeTypes = null;
            }
        }

        public virtual TypeDefinition GetTypeDefinition()
        {
            return TypeDefinitions[TypeInfo.Key];
        }

        public virtual TypeDefinition GetTypeDefinition(IType type)
        {
            return H5Types.Get(type).TypeDefinition;
        }

        public virtual TypeDefinition GetTypeDefinition(AstType reference, bool safe = false)
        {
            var resolveResult = Resolver.ResolveNode(reference) as TypeResolveResult;
            var type = H5Types.Get(resolveResult.Type, safe);
            return type?.TypeDefinition;
        }

        public virtual TypeDefinition GetBaseTypeDefinition()
        {
            return GetBaseTypeDefinition(GetTypeDefinition());
        }

        public virtual TypeDefinition GetBaseTypeDefinition(TypeDefinition type)
        {
            var reference = type.BaseType;

            if (reference == null)
            {
                return null;
            }

            return H5Types.Get(reference).TypeDefinition;
        }

        public virtual TypeDefinition GetBaseMethodOwnerTypeDefinition(string methodName, int genericParamCount)
        {
            TypeDefinition type = GetBaseTypeDefinition();

            while (true)
            {
                var methods = type.Methods.Where(m => m.Name == methodName);

                foreach (var method in methods)
                {
                    if (genericParamCount < 1 || method.GenericParameters.Count == genericParamCount)
                    {
                        return type;
                    }
                }

                type = GetBaseTypeDefinition(type);
            }
        }

        public virtual string GetTypeHierarchy()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            var list = new List<string>();

            foreach (var t in TypeInfo.GetBaseTypes(this))
            {
                var name = H5Types.ToJsName(t, this);

                list.Add(name);
            }

            if (list.Count > 0 && list[0] == JS.Types.System.Object.NAME)
            {
                list.RemoveAt(0);
            }

            if (list.Count == 0)
            {
                return "";
            }

            bool needComma = false;

            foreach (var item in list)
            {
                if (needComma)
                {
                    sb.Append(",");
                }

                needComma = true;
                sb.Append(item);
            }

            sb.Append("]");

            return sb.ToString();
        }

        private Dictionary<TypeDefinition, int> priorityMap = new Dictionary<TypeDefinition, int>();
        public virtual int GetPriority(TypeDefinition type)
        {
            if (priorityMap.ContainsKey(type))
            {
                return priorityMap[type];
            }

            var attr = type.CustomAttributes.FirstOrDefault(a =>
            {
                return a.AttributeType.FullName == "H5.PriorityAttribute";
            });

            if (attr != null)
            {
                var attrp = System.Convert.ToInt32(attr.ConstructorArguments[0].Value);
                priorityMap[type] = attrp;
                return attrp;
            }

            var baseType = GetBaseTypeDefinition(type);
            var p = 0;
            if (baseType != null)
            {
                p = GetPriority(baseType);
            }

            priorityMap[type] = p;
            return p;
        }
    }
}