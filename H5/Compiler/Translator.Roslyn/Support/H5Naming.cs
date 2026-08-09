using System.Linq;
using Microsoft.CodeAnalysis;

namespace H5.Translator.Roslyn;

/// <summary>
/// Reads the H5 code-generation attributes ([Template], [Name], [External], [Script])
/// from symbols in the referenced H5 assembly, and derives JavaScript names using
/// H5's conventions. This is what lets emitted code interoperate with the h5.js runtime.
/// </summary>
internal static class H5Naming
{
    public const string TemplateAttr = "H5.TemplateAttribute";
    public const string NameAttr = "H5.NameAttribute";
    public const string ExternalAttr = "H5.ExternalAttribute";
    public const string ScriptAttr = "H5.ScriptAttribute";
    public const string EnumAttr = "H5.EnumAttribute";
    public const string ScopeAttr = "H5.ScopeAttribute";
    public const string GlobalMethodsAttr = "H5.GlobalMethodsAttribute";

    /// <summary>
    /// The JS scope prefix for a type marked <c>[Scope]</c>/<c>[GlobalMethods]</c> — the H5
    /// bindings (e.g. <c>H5.Core.dom</c>) that project onto ambient JS globals. Returns the
    /// scope's name argument, <c>""</c> for the global scope (no argument), or null when the
    /// type is not scoped. A scoped type's static members and nested types drop the C#
    /// type/namespace path and live under this prefix (so <c>dom.window</c> → <c>window</c>).
    /// </summary>
    public static string? ScopePrefix(ITypeSymbol? type)
    {
        if (type is null) return null;
        var scope = type.GetAttributes().FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == ScopeAttr);
        var global = type.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == GlobalMethodsAttr);
        if (scope is null && !global) return null;
        return (scope?.ConstructorArguments.FirstOrDefault().Value as string) ?? "";
    }

    /// <summary>
    /// The <c>[Enum(Emit.X)]</c> mode of an enum type (H5's <c>Emit</c> values:
    /// 1 Name, 2 Value, 3 StringName, 4 StringNamePreserveCase, 5 StringNameLowerCase,
    /// 6 StringNameUpperCase, 7 NamePreserveCase, 8 NameLowerCase, 9 NameUpperCase).
    /// Defaults to 7 (NamePreserveCase) when the attribute is absent, matching H5.
    /// </summary>
    public static int EnumEmitMode(ITypeSymbol enumType)
    {
        var a = enumType.GetAttributes().FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == EnumAttr);
        if (a is null || a.ConstructorArguments.Length == 0) return 7;
        return a.ConstructorArguments[0].Value is int m ? m : 7;
    }

    /// <summary>
    /// The string an enum member emits under a StringName mode (3–6): the member name
    /// with H5's per-mode casing (3 camelCases the first letter, 5 lowercases, 6 uppercases,
    /// 4 preserves), unless an explicit <c>[Name]</c> overrides it.
    /// </summary>
    public static string EnumStringName(IFieldSymbol member, int mode)
    {
        if (GetName(member) is { } named) return named;
        var name = member.Name;
        return mode switch
        {
            3 => char.ToLowerInvariant(name[0]) + name.Substring(1),
            5 => name.ToLowerInvariant(),
            6 => name.ToUpperInvariant(),
            _ => name,
        };
    }

    /// <summary>The [Template] JS string for a member, or null.</summary>
    public static string? GetTemplate(ISymbol symbol)
        => GetStringAttr(symbol, TemplateAttr);

    /// <summary>The explicit [Name] for a member/type, or null.</summary>
    public static string? GetName(ISymbol symbol)
        => GetStringAttr(symbol, NameAttr);

    public const string AccessorsIndexerAttr = "H5.AccessorsIndexerAttribute";

    /// <summary>
    /// True if an indexer maps to native JS bracket access (<c>obj[key]</c>) rather than
    /// getItem/setItem accessors — the case for an [External] type's plain indexer (e.g. a DOM
    /// element's <c>this[string]</c>), unless it opts into accessors via [AccessorsIndexer]/[Name]
    /// or a [Template].
    /// </summary>
    public static bool IsNativeIndexer(IPropertySymbol indexer)
    {
        // Native bracket access (obj[key]) is used by an [External] non-interface type — the
        // native JS objects like a DOM element's this[string]. The [External] BCL *collection
        // interfaces* (IReadOnlyList/IReadOnlyDictionary) still route through getItem/setItem.
        if (!indexer.IsIndexer) return false;
        // External (or scope-bound, e.g. a DOM NodeList under H5.Core.dom's [Scope]) non-interface
        // types are native JS objects, so their indexer is bracket access. Real H5 runtime
        // collection classes (List<T>, Dictionary<,>, …) are not external and keep getItem/setItem.
        if (indexer.ContainingType is not { TypeKind: TypeKind.Class } ct || !IsExternalType(ct)) return false;
        if (indexer.ContainingType.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == AccessorsIndexerAttr)) return false;
        if (GetName(indexer) is not null) return false;
        if (GetTemplate(indexer.GetMethod?.OriginalDefinition) is not null) return false;
        if (GetTemplate(indexer.SetMethod?.OriginalDefinition) is not null) return false;
        return true;
    }

    /// <summary>
    /// Accessor-method name for an indexer element access. An indexer with a [Name]
    /// (e.g. StringBuilder's [Name("Char")]) maps to get&lt;Name&gt;/set&lt;Name&gt;
    /// (getChar/setChar); the default is getItem/setItem.
    /// </summary>
    public static string IndexerAccessorName(IPropertySymbol indexer, bool isGet)
    {
        var name = GetName(indexer);
        var suffix = name ?? "Item";
        return (isGet ? "get" : "set") + suffix;
    }

    /// <summary>
    /// True if the type behaves like an external JS type for naming (no overload suffixes,
    /// camelCase members): it carries [External], or it is projected onto ambient JS globals
    /// via a [Scope]/[GlobalMethods] binding (e.g. the DOM types under H5.Core.dom).
    /// </summary>
    public static bool IsExternalType(ITypeSymbol? type)
    {
        if (HasExternalAttribute(type)) return true;
        for (var t = type; t is not null; t = t.ContainingType)
            if (ScopePrefix(t) is not null) return true;
        return false;
    }

    /// <summary>
    /// True if a type is emitted by an H5 compiler with source naming conventions — either it is
    /// in this compilation's source, or it lives in a referenced *user library* assembly (one
    /// compiled with --emit-package). It is false for external/DOM types and for the H5 runtime
    /// assemblies (H5, H5.Core, …) whose BCL types are baked into h5.js with fixed names. This is
    /// the discriminator for names that must agree between a library and the projects that
    /// reference it — preserving existing source/BCL behaviour while treating a referenced library
    /// the same as source.
    /// </summary>
    public static bool IsH5CompiledSource(ITypeSymbol? type)
    {
        if (type is null) return false;
        if (type.Locations.Any(l => l.IsInSource)) return true;
        return !IsExternalType(type) && !IsH5RuntimeAssembly(type.ContainingAssembly);
    }

    /// <summary>An H5 runtime/BCL package (H5.dll, H5.Core.dll, H5.Newtonsoft.Json.dll, …) whose
    /// types are provided pre-compiled by the runtime, as opposed to a user library.</summary>
    private static bool IsH5RuntimeAssembly(IAssemblySymbol? asm)
    {
        var n = asm?.Name;
        return n == "H5" || (n is not null && n.StartsWith("H5.", System.StringComparison.Ordinal));
    }

    /// <summary>
    /// True if an interface should be listed in a type's <c>inherits</c> so the runtime tracks it
    /// for <c>is</c>/<c>as</c> and interface dispatch. This is every implemented interface that the
    /// runtime registers: source/referenced-library interfaces AND the H5 BCL interfaces
    /// (System.Collections.Generic.IList/ICollection/IEnumerable, IComparable, …) which are real
    /// H5.define'd types. Only ambient DOM/scoped interfaces are excluded (they are native JS, not
    /// H5-registered). Omitting the BCL collection interfaces breaks e.g. LINQ over a user
    /// collection: <c>Enumerable.from(x)</c> tests <c>x is IEnumerable</c> before enumerating it.
    /// </summary>
    public static bool IsInheritableInterface(ITypeSymbol? i)
    {
        if (i is not { TypeKind: TypeKind.Interface }) return false;
        if (IsScopedType(i)) return false;                       // DOM / ambient JS — not registered
        if (IsH5CompiledSource(i)) return true;                  // source or referenced user library
        return IsH5RuntimeAssembly(i.ContainingAssembly);        // H5 BCL interface (IList, IEnumerable, …)
    }

    /// <summary>True if the type (or an enclosing type) is a [Scope]/[GlobalMethods] binding
    /// projected onto ambient JS (e.g. the DOM types under H5.Core.dom).</summary>
    public static bool IsScopedType(ITypeSymbol? type)
    {
        for (var t = type; t is not null; t = t.ContainingType)
            if (ScopePrefix(t) is not null) return true;
        return false;
    }

    /// <summary>True if the type (or an enclosing type) carries the [External] attribute.</summary>
    public static bool HasExternalAttribute(ITypeSymbol? type)
    {
        for (var t = type; t is not null; t = t.ContainingType)
        {
            if (t.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == ExternalAttr))
                return true;
        }
        return false;
    }

    public static bool IsExternal(ISymbol symbol)
    {
        for (var s = symbol; s is not null; s = s.ContainingType)
        {
            if (s.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == ExternalAttr))
                return true;
        }
        // Types defined in the H5 assembly (not in user source) are external BCL.
        return !symbol.Locations.Any(l => l.IsInSource);
    }

    private static string? GetStringAttr(ISymbol? symbol, string attrName)
    {
        if (symbol is null) return null;
        var attr = symbol.GetAttributes()
            .FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == attrName);
        if (attr is null || attr.ConstructorArguments.Length == 0) return null;
        return attr.ConstructorArguments[0].Value as string;
    }

    /// <summary>
    /// JavaScript member name for a member with no [Template], honouring [Name]
    /// and H5's convention (camelCase methods for library members; source members
    /// keep their C# name, with the entry point mapped to "main").
    /// </summary>
    public static string MemberJsName(ISymbol symbol)
    {
        // An explicit interface implementation is named by the interface-qualified mangled
        // name H5 uses (e.g. IMyInterface.Method → Namespace$IMyInterface$method), so it
        // stays a valid JS identifier and matches the runtime's interface-member slot.
        if (ExplicitInterfaceMangledName(symbol) is { } explicitName) return explicitName;

        // A member accessed through a *source* interface type resolves to the interface's own
        // member; H5 stores it under a mangled slot (Namespace$IFace$member) that every
        // implementer aliases (see InterfaceAliasPairs), so route the access there — this is
        // what reaches explicit implementations. BCL interfaces keep the plain name: their
        // implementers (h5.js types) expose it directly, so plain access already resolves.
        if (symbol.ContainingType is { TypeKind: TypeKind.Interface } iface && IsSourceInterface(iface))
            return MangledTypeName(iface) + "$" + LeafJsName(symbol);

        return LeafJsName(symbol);
    }

    /// <summary>The un-mangled JS name of a member (ignoring interface qualification).</summary>
    private static string LeafJsName(ISymbol symbol)
    {
        if (symbol is IMethodSymbol m) return MethodJsName(m);

        var raw = RawMemberName(symbol);

        // A property/field/event that hides a same-named base member (C# `new`) takes its own
        // slot via a $N suffix (as H5 does), so the hiding member and the base member don't
        // collide — and base access (base.X, emitted as this.X) still reaches the base slot.
        // Only H5-compiled members (source or referenced library) get a slot suffix: an
        // external/native member (e.g. a DOM NodeListOf<T>.length hiding NodeList.length) maps to
        // a single fixed JS property, so suffixing it would reference a nonexistent property.
        if (GetName(symbol) is null && IsH5CompiledSource(symbol.ContainingType)
            && symbol is IPropertySymbol { IsIndexer: false } or IFieldSymbol or IEventSymbol)
        {
            var idx = HidingIndex(symbol, raw);
            if (idx > 0) return raw + "$" + idx;
        }
        return raw;
    }

    /// <summary>The base JS name of a property/field/event (before any hiding suffix).</summary>
    private static string RawMemberName(ISymbol symbol)
    {
        if (GetName(symbol) is { } name) return name;
        if (symbol.Locations.Any(l => l.IsInSource)) return symbol.Name;

        // Property / field / event: camelCase under an [External] type or a
        // [Convention] covering that member kind; otherwise preserve.
        var kindFlag = symbol.Kind switch
        {
            SymbolKind.Property => ConvProperty,
            SymbolKind.Field => ConvField,
            SymbolKind.Event => ConvEvent,
            _ => ConvAll,
        };
        var notation = MemberConventionNotation(symbol)
                       ?? ResolveNotation(symbol.ContainingType, kindFlag)
                       ?? (IsExternalType(symbol.ContainingType) ? Notation.CamelCase : Notation.None);
        return Apply(notation, symbol.Name);
    }

    /// <summary>How many base-type members this member hides (share its slot name). Zero for an
    /// override (which shares the base slot) or a member that introduces a fresh name.</summary>
    private static int HidingIndex(ISymbol symbol, string raw)
    {
        if (symbol is IPropertySymbol { IsOverride: true } or IEventSymbol { IsOverride: true }) return 0;
        var count = 0;
        for (var t = symbol.ContainingType?.BaseType; t is not null; t = t.BaseType)
        {
            foreach (var bm in t.GetMembers())
            {
                if (bm.IsStatic || bm.Kind != symbol.Kind) continue;
                if (bm is IPropertySymbol { IsIndexer: true }) continue;
                if (RawMemberName(bm) == raw) { count++; break; }
            }
        }
        return count;
    }

    /// <summary>
    /// The mangled JS name of an explicit interface implementation
    /// (<c>Namespace$IFace$member</c>), or null when the member isn't an explicit impl.
    /// </summary>
    private static string? ExplicitInterfaceMangledName(ISymbol symbol)
    {
        var im = ExplicitlyImplementedMember(symbol);
        if (im?.ContainingType is not { } iface) return null;
        return MangledTypeName(iface) + "$" + LeafJsName(im.OriginalDefinition);
    }

    /// <summary>The interface member a symbol explicitly implements, or null.</summary>
    private static ISymbol? ExplicitlyImplementedMember(ISymbol symbol) => symbol switch
    {
        IMethodSymbol m when m.ExplicitInterfaceImplementations.Length > 0 => m.ExplicitInterfaceImplementations[0],
        IPropertySymbol p when p.ExplicitInterfaceImplementations.Length > 0 => p.ExplicitInterfaceImplementations[0],
        IEventSymbol e when e.ExplicitInterfaceImplementations.Length > 0 => e.ExplicitInterfaceImplementations[0],
        _ => null,
    };

    /// <summary>The mangled interface-member slot name (<c>Namespace$IFace$member</c>).</summary>
    public static string InterfaceMemberName(ISymbol interfaceMember)
        => MangledTypeName(interfaceMember.ContainingType) + "$" + LeafJsName(interfaceMember.OriginalDefinition);

    /// <summary>The un-mangled JS name of a member (its slot on the declaring class).</summary>
    public static string LeafMemberName(ISymbol symbol) => LeafJsName(symbol);

    /// <summary>
    /// The (plainName, mangledInterfaceSlot) aliases a type must publish so that a member
    /// accessed through one of its interfaces resolves to the implementing member. Covers
    /// members this type implements *implicitly* — explicit implementations are already
    /// emitted under the mangled slot, and inherited implementations are aliased by the base
    /// type. H5's `alias` config installs these on the prototype.
    /// </summary>
    public static System.Collections.Generic.List<(string plain, string mangled)> InterfaceAliasPairs(INamedTypeSymbol type)
    {
        var pairs = new System.Collections.Generic.List<(string, string)>();
        var seen = new System.Collections.Generic.HashSet<string>();

        foreach (var iface in type.AllInterfaces)
        {
            if (!IsSourceInterface(iface)) continue; // BCL interfaces use plain names (see MemberJsName)
            foreach (var member in iface.GetMembers())
            {
                if (member.IsStatic) continue;
                if (member is IMethodSymbol { MethodKind: not MethodKind.Ordinary }) continue; // skip accessors
                if (member is not (IMethodSymbol or IPropertySymbol or IEventSymbol)) continue;
                if (GetTemplate(member) is not null) continue;   // templated members apply at the call site

                if (type.FindImplementationForInterfaceMember(member) is not { } impl) continue;
                if (!SymbolEqualityComparer.Default.Equals(impl.ContainingType, type)) continue; // declared here
                if (ExplicitlyImplementedMember(impl) is not null) continue;                    // already mangled

                var plain = LeafJsName(impl);
                var mangled = InterfaceMemberName(member);
                if (plain == mangled) continue;
                if (seen.Add(plain + "\0" + mangled)) pairs.Add((plain, mangled));
            }
        }
        return pairs;
    }

    /// <summary>A user-defined (source) interface — its members are dispatched via mangled
    /// interface slots; BCL interfaces resolve through their implementers' plain names.</summary>
    private static bool IsSourceInterface(INamedTypeSymbol iface)
        // A user-defined interface — whether from this compilation's source or a referenced
        // H5-compiled assembly (both mangle their members the same way). Only truly external
        // (BCL/DOM) interfaces resolve through their implementers' plain names.
        => iface.TypeKind == TypeKind.Interface && IsH5CompiledSource(iface);

    /// <summary>Full type name as a single JS identifier: dotted segments joined by <c>$</c>,
    /// with a generic arity suffix (e.g. <c>System$Collections$Generic$IComparer$1</c>). Honours a
    /// type-level <c>[H5.Name]</c> (its dotted value becomes the mangled prefix, e.g.
    /// <c>[Name("tss.IC")]</c> → <c>tss$IC</c>), so interface-member slots stay consistent with the
    /// type's registered name.</summary>
    private static string MangledTypeName(INamedTypeSymbol type)
    {
        if (MangledSelf(type) is { } named) return named;
        var parts = new System.Collections.Generic.List<string>();
        for (INamedTypeSymbol? t = type; t is not null; t = t.ContainingType)
        {
            if (!SymbolEqualityComparer.Default.Equals(t, type) && MangledSelf(t) is { } enclosing)
            {
                var tail = string.Join("$", parts);
                return tail.Length == 0 ? enclosing : enclosing + "$" + tail;
            }
            parts.Insert(0, t.Arity > 0 ? t.Name + "$" + t.Arity : t.Name);
        }
        for (var ns = type.ContainingNamespace; ns is { IsGlobalNamespace: false }; ns = ns.ContainingNamespace)
            parts.Insert(0, ns.Name);
        return string.Join("$", parts);
    }

    /// <summary>The mangled form of a type's own <c>[Name]</c> (dotted → <c>$</c>, arity appended),
    /// or null when the type has no <c>[Name]</c>.</summary>
    private static string? MangledSelf(INamedTypeSymbol type)
    {
        if (GetName(type) is not { } n) return null;
        var mangled = n.Replace('.', '$');
        return type.Arity > 0 ? mangled + "$" + type.Arity : mangled;
    }

    private static readonly System.Collections.Generic.Dictionary<ISymbol, string> _methodCache = new(SymbolEqualityComparer.Default);

    private static string MethodJsName(IMethodSymbol method)
    {
        method = method.OriginalDefinition;
        if (_methodCache.TryGetValue(method, out var cached)) return cached;

        var baseName = JsBaseName(method);

        // A canonical name (object-override, explicit/inherited [Name], or an implemented
        // interface member's name) is used verbatim — no overload suffix (H5 behaviour).
        // External non-interface types also skip suffixes.
        var declType = method.ContainingType;
        var external = declType is not null && IsExternalType(declType) && declType.TypeKind != TypeKind.Interface;

        // Extern (no IL body) methods on a non-external type are hand-written-JS backed:
        // H5 excludes them from the overload set, so they carry no suffix.
        if (external || HasCanonicalName(method) || HasNoBody(method)) return Cache(method, baseName);

        // Overload index via H5's overload-collection ordering (override → base member),
        // grouped by the FINAL JS base name so differently-named overloads don't collide.
        var resolved = ResolveOverrideBase(method);
        var group = OverloadGroup(resolved);
        var index = group.FindIndex(x => SymbolEqualityComparer.Default.Equals(x, resolved));

        for (var i = 0; i < group.Count; i++)
        {
            if (_methodCache.ContainsKey(group[i])) continue;
            var gBase = JsBaseName(group[i]);
            _methodCache[group[i]] = i == 0 ? gBase : $"{gBase}${i}";
        }

        var result = index < 0 ? baseName : (index == 0 ? baseName : $"{baseName}${index}");
        return Cache(method, result);
    }

    // The object-method override signatures (H5 gives these canonical runtime names).
    private static bool IsObjectToString(IMethodSymbol m) => m is { Name: "ToString", Parameters.Length: 0 };
    private static bool IsObjectGetHashCode(IMethodSymbol m) => m is { Name: "GetHashCode", Parameters.Length: 0 };
    private static bool IsObjectEquals(IMethodSymbol m)
        => m is { Name: "Equals", Parameters.Length: 1 } && m.Parameters[0].Type.SpecialType == SpecialType.System_Object;

    /// <summary>
    /// The un-suffixed JS name for a method, following H5's resolution order:
    /// object-override runtime names, an explicit [Name], an inherited [Name] (from an
    /// overridden base or implemented interface member), the implemented interface member's
    /// own name, then the convention-derived name.
    /// </summary>
    private static string JsBaseName(IMethodSymbol method)
    {
        if (IsObjectToString(method)) return "toString";
        if (IsObjectGetHashCode(method)) return "getHashCode";
        if (IsObjectEquals(method)) return "equals";
        if (GetName(method) is { } explicitName) return explicitName;
        if (InheritedName(method) is { } inherited) return inherited;
        if (ImplementedInterfaceMember(method) is { } im) return JsBaseName(im);
        return Apply(MethodNotation(method), method.Name);
    }

    /// <summary>A resolved name that must be used verbatim (never overload-suffixed).</summary>
    private static bool HasCanonicalName(IMethodSymbol m)
        => IsObjectToString(m) || IsObjectGetHashCode(m) || IsObjectEquals(m)
           || GetName(m) is not null || InheritedName(m) is not null;
    // Note: a plain interface implementation is NOT canonical — H5 still numbers it through the
    // overload collection (so a type implementing e.g. both IDictionary.Add(k,v) and
    // ICollection.Add(KeyValuePair) gets add / add$1, not two colliding `add` keys). A lone
    // interface implementation still lands at index 0 and keeps its bare name.

    /// <summary>
    /// True if this is a library (H5.dll) method with no IL body and not abstract — a C#
    /// <c>extern</c> member backed by hand-written runtime JS (e.g. <c>Regex.Replace</c>).
    /// H5 leaves such members out of a non-external type's overload set, so they take the
    /// bare convention name with no <c>$N</c> suffix.
    /// </summary>
    public static bool HasNoBody(IMethodSymbol method)
    {
        if (method.Locations.Any(l => l.IsInSource)) return false;
        if (method.ContainingAssembly?.Name != "H5") return false;
        return H5Assemblies.NoBodyMethodTokens.Contains(method.OriginalDefinition.MetadataToken);
    }

    /// <summary>A [Name] inherited from an overridden base method or an implemented interface member.</summary>
    private static string? InheritedName(IMethodSymbol method)
    {
        for (var b = method.OverriddenMethod; b is not null; b = b.OverriddenMethod)
            if (GetName(b.OriginalDefinition) is { } n) return n;
        if (ImplementedInterfaceMember(method) is { } im && GetName(im) is { } inm) return inm;
        return null;
    }

    /// <summary>The interface member this method implements (implicitly), or null.</summary>
    private static IMethodSymbol? ImplementedInterfaceMember(IMethodSymbol method)
    {
        var type = method.ContainingType;
        if (type is null) return null;
        foreach (var iface in type.AllInterfaces)
        {
            foreach (var member in iface.GetMembers().OfType<IMethodSymbol>())
            {
                if (type.FindImplementationForInterfaceMember(member) is IMethodSymbol impl
                    && SymbolEqualityComparer.Default.Equals(impl.OriginalDefinition, method.OriginalDefinition))
                    return member.OriginalDefinition;
            }
        }
        return null;
    }

    private static string Cache(IMethodSymbol m, string name) { _methodCache[m.OriginalDefinition] = name; return name; }

    /// <summary>
    /// JS name of a constructor using H5's OverloadsCollection ordering: the first
    /// constructor is "ctor", the rest "$ctor1", "$ctor2"… This matches the names h5.js
    /// was generated with, so BCL constructions (e.g. new Guid(string) → $ctor4) resolve.
    /// </summary>
    public static string ConstructorName(IMethodSymbol ctor)
    {
        ctor = ctor.OriginalDefinition;
        var group = OverloadGroup(ctor);
        var index = group.FindIndex(x => SymbolEqualityComparer.Default.Equals(x, ctor));
        return index <= 0 ? "ctor" : "$ctor" + index;
    }

    private static IMethodSymbol ResolveOverrideBase(IMethodSymbol method)
    {
        var m = method;
        while (m.IsOverride && m.OverriddenMethod is not null && GetTemplate(m) is null)
            m = m.OverriddenMethod.OriginalDefinition;
        return m;
    }

    /// <summary>Notation for a library method: member [Convention], else type [Convention], else interface-inherited camelCase, else external, else preserve.</summary>
    private static Notation MethodNotation(IMethodSymbol method)
    {
        // H5-compiled methods (source or referenced library) keep their verbatim name; only the
        // external-type conventions below apply to BCL/DOM methods.
        if (IsH5CompiledSource(method.ContainingType)) return Notation.None;
        // A [Convention] applied directly to the method (e.g. IComparer<T>.Compare) wins.
        if (MemberConventionNotation(method) is { } mc) return mc;
        var conv = ResolveNotation(method.ContainingType, ConvMethod);
        if (conv is { } c) return c;
        // A templated member with no [Convention] keeps its raw name — H5 does not camelCase it.
        // The [Template] drives every call site, so the name is used only for an implementer's
        // method slot: e.g. IEnumerable.GetEnumerator (templated, no [Convention]) stays
        // "GetEnumerator", the PascalCase name h5.js's H5.getEnumerator looks up. (Collection
        // interfaces whose members SHOULD camelCase — ICollection.Add etc. — carry an explicit
        // type-level [Convention], handled above before this point.)
        if (GetTemplate(method) is not null) return Notation.None;
        if (ImplementsInterfaceMember(method)) return Notation.CamelCase;
        if (IsExternalType(method.ContainingType)) return Notation.CamelCase;
        return Notation.None;
    }

    private static bool ImplementsInterfaceMember(IMethodSymbol method)
    {
        var type = method.ContainingType;
        if (type is null) return false;
        foreach (var iface in type.AllInterfaces)
        {
            foreach (var member in iface.GetMembers().OfType<IMethodSymbol>())
            {
                if (type.FindImplementationForInterfaceMember(member) is IMethodSymbol im
                    && SymbolEqualityComparer.Default.Equals(im.OriginalDefinition, method.OriginalDefinition))
                    return true;
            }
        }
        return false;
    }

    // ---- overload collection (ported from H5's OverloadsCollection) --------

    private static System.Collections.Generic.List<IMethodSymbol> OverloadGroup(IMethodSymbol method)
    {
        var jsBase = JsBaseName(method);
        var isStatic = method.IsStatic;
        var kind = method.MethodKind;

        // Constructors are not inherited, so their overload set is the declaring type's own
        // ctors; ordinary methods fold in base-type members (for override-based numbering).
        var members = new System.Collections.Generic.List<IMethodSymbol>();
        var seen = new System.Collections.Generic.HashSet<IMethodSymbol>(SymbolEqualityComparer.Default);
        for (var t = method.ContainingType; t is not null; t = t.BaseType)
        {
            foreach (var candidate in t.GetMembers().OfType<IMethodSymbol>())
            {
                var c = candidate.OriginalDefinition;
                if (c.MethodKind != kind || c.IsStatic != isStatic) continue;
                if (c.ExplicitInterfaceImplementations.Length > 0) continue;
                if (JsBaseName(c) != jsBase) continue;                 // group by final JS name
                if (c.IsOverride) continue;                            // overrides fold into their base

                // The parameterless object ToString occupies a slot (so a same-named
                // overload like Version.ToString(int) numbers from $1), even though it is
                // inline/body-less — H5 keeps only ToString here, not Equals/GetHashCode.
                var isToStringSlot = IsObjectToString(c);
                if (!isToStringSlot && GetTemplate(c) is not null) continue;   // inline methods are excluded
                if (!isToStringSlot && HasNoBody(c)) continue;                 // extern (hand-written JS) methods aren't numbered
                if (seen.Add(c)) members.Add(c);
            }
            if (kind == MethodKind.Constructor) break;                 // ctors aren't inherited
        }

        members.Sort(CompareOverload);
        return members;
    }

    private static int CompareOverload(IMethodSymbol m1, IMethodSymbol m2)
    {
        if (!SymbolEqualityComparer.Default.Equals(m1.ContainingType, m2.ContainingType))
            return IsDerivedFrom(m1.ContainingType, m2.ContainingType) ? 1 : -1;

        var i1 = ImplementsInterfaceMember(m1);
        var i2 = ImplementsInterfaceMember(m2);
        if (i1 && !i2) return -1;
        if (i2 && !i1) return 1;

        var c1 = m1.MethodKind == MethodKind.Constructor;
        var c2 = m2.MethodKind == MethodKind.Constructor;
        if (c1 && !c2) return -1;
        if (c2 && !c1) return 1;
        // Two constructors compare by signature directly (H5 returns here, before accessibility).
        if (c1 && c2) return string.Compare(MethodToString(m1), MethodToString(m2), System.StringComparison.CurrentCulture);

        var a1 = AccessibilityWeight(m1.DeclaredAccessibility);
        var a2 = AccessibilityWeight(m2.DeclaredAccessibility);
        if (a1 != a2) return a1.CompareTo(a2);

        return string.Compare(MethodToString(m1), MethodToString(m2), System.StringComparison.CurrentCulture);
    }

    private static bool IsDerivedFrom(ITypeSymbol? derived, ITypeSymbol? base_)
    {
        // Compare on original definitions: an overload group mixes a derived type's own methods
        // with a *generic* base's methods, where the base appears constructed on the derived
        // (e.g. Card : ComponentBase<Card,…>) but as its open definition in the group. Comparing
        // the raw symbols would miss that relationship and mis-order the overloads — which made a
        // derived new overload collide with the base's slot (duplicate JS keys).
        var target = base_?.OriginalDefinition ?? base_;
        for (var t = derived?.BaseType; t is not null; t = t.BaseType)
            if (SymbolEqualityComparer.Default.Equals(t.OriginalDefinition, target)) return true;
        return false;
    }

    private static int AccessibilityWeight(Accessibility a) => a switch
    {
        Accessibility.Public => 1,
        Accessibility.Internal or Accessibility.ProtectedOrInternal => 2,
        Accessibility.Protected or Accessibility.ProtectedAndInternal => 3,
        _ => 4,
    };

    private static readonly SymbolDisplayFormat FullName = new(
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);

    private static string MethodToString(IMethodSymbol m)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append(m.ReturnType.ToDisplayString(FullName)).Append(' ');
        sb.Append(m.Name).Append(' ');
        sb.Append(m.TypeParameters.Length).Append(' ');
        foreach (var p in m.Parameters) sb.Append(p.Type.ToDisplayString(FullName)).Append(' ');
        return sb.ToString();
    }

    // ---- [Convention] resolution ------------------------------------------

    private enum Notation { None = 0, LowerCase = 1, UpperCase = 2, CamelCase = 3, PascalCase = 4 }
    private const int ConvAll = 0, ConvMethod = 0x1, ConvProperty = 0x2, ConvField = 0x4, ConvEvent = 0x8;

    /// <summary>A [Convention] applied directly to a member (e.g. KeyValuePair.Key).</summary>
    private static Notation? MemberConventionNotation(ISymbol symbol)
    {
        var a = symbol.GetAttributes().FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == ConventionAttr);
        if (a is null) return null;
        var notation = a.ConstructorArguments.Length > 0 && a.ConstructorArguments[0].Value is int cn
            ? cn
            : NamedInt(a, "Notation", (int)Notation.None);
        return (Notation)notation;
    }

    private static Notation? ResolveNotation(ITypeSymbol? type, int memberKindFlag)
    {
        if (type is null) return null;
        AttributeData? best = null;
        var bestPriority = int.MinValue;
        foreach (var a in type.GetAttributes().Where(a => a.AttributeClass?.ToDisplayString() == ConventionAttr))
        {
            var member = NamedInt(a, "Member", ConvAll);
            if (member != ConvAll && (member & memberKindFlag) == 0) continue;
            var priority = NamedInt(a, "Priority", 0);
            if (best is null || priority >= bestPriority) { best = a; bestPriority = priority; }
        }
        if (best is null) return null;
        var notation = best.ConstructorArguments.Length > 0 && best.ConstructorArguments[0].Value is int cn
            ? cn
            : NamedInt(best, "Notation", (int)Notation.None);
        return (Notation)notation;
    }

    private static int NamedInt(AttributeData a, string name, int dflt)
    {
        foreach (var na in a.NamedArguments)
            if (na.Key == name && na.Value.Value is int v) return v;
        return dflt;
    }

    private static string Apply(Notation notation, string name)
    {
        if (string.IsNullOrEmpty(name)) return name;
        return notation switch
        {
            Notation.CamelCase => char.ToLowerInvariant(name[0]) + name.Substring(1),
            Notation.PascalCase => char.ToUpperInvariant(name[0]) + name.Substring(1),
            Notation.LowerCase => name.ToLowerInvariant(),
            Notation.UpperCase => name.ToUpperInvariant(),
            _ => name,
        };
    }

    public const string ConventionAttr = "H5.ConventionAttribute";

    public static string CamelCase(string s)
    {
        if (string.IsNullOrEmpty(s) || char.IsLower(s[0])) return s;
        if (s.Length == 1) return s.ToLowerInvariant();
        // Leave ALLCAPS runs mostly alone but lower the first char (H5 behavior is
        // first-letter lowercase for typical PascalCase identifiers).
        return char.ToLowerInvariant(s[0]) + s.Substring(1);
    }
}
