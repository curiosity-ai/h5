using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace H5.Translator.Roslyn;

public sealed partial class Emitter
{
    // System.Reflection.MemberTypes codes used by h5.js reflection metadata.
    private const int MtConstructor = 1, MtEvent = 2, MtField = 4, MtMethod = 8, MtProperty = 16;

    // Namespace → index map for the $n compaction array, rebuilt per metadata emission.
    private Dictionary<string, int> _nsCache = new(StringComparer.Ordinal);

    /// <summary>
    /// Emits reflection metadata inline (inside the current assembly function): the
    /// <c>var $m = H5.setMetadata, $n = [...]</c> prologue followed by one
    /// <c>$m("Type", function (T…) { return {…}; }, $n)</c> per reflectable source type.
    /// </summary>
    private void EmitReflectionMetadata(IReadOnlyList<INamedTypeSymbol> types)
    {
        var block = BuildMetadataBlock(types);
        if (block is null) return;
        _w.WriteLine();
        foreach (var line in block.Split('\n'))
            _w.WriteLine(line);
    }

    /// <summary>Builds a standalone metadata script (its own H5.assembly wrapper) for the
    /// file/assembly reflection target; null when there is nothing to emit.</summary>
    private string? BuildMetadataFile(IReadOnlyList<INamedTypeSymbol> types)
    {
        var block = BuildMetadataBlock(types);
        if (block is null) return null;
        var sb = new StringBuilder();
        sb.Append("H5.assemblyVersion(\"").Append(_assemblyName).Append("\",\"").Append(AssemblyVersion).Append("\");\n");
        sb.Append("H5.assembly(\"").Append(_assemblyName).Append("\", function ($asm, globals) {\n");
        sb.Append("    \"use strict\";\n\n");
        foreach (var line in block.Split('\n'))
            sb.Append("    ").Append(line).Append('\n');
        sb.Append("});\n");
        return sb.ToString();
    }

    /// <summary>Constructs the shared <c>$m/$n</c> metadata block for the given types, or null
    /// if none are reflectable. Populates the namespace cache as type names are formatted.</summary>
    private string? BuildMetadataBlock(IReadOnlyList<INamedTypeSymbol> types)
    {
        _nsCache = new Dictionary<string, int>(StringComparer.Ordinal);
        var entries = new List<string>();
        foreach (var type in types)
        {
            if (!IsReflectableType(type)) continue;
            string? json;
            try { json = ConstructTypeMetadata(type); }
            catch { continue; } // never let one type's metadata abort the whole build
            if (json is null) continue;

            var typeArgs = type.IsGenericType && !IsIgnoreGeneric(type)
                ? string.Join(", ", type.TypeParameters.Select(tp => tp.Name))
                : "";
            entries.Add($"$m(\"{MetaTypeDefName(type)}\", function ({typeArgs}) {{ return {json}; }}, $n);");
        }
        if (entries.Count == 0) return null;

        var ns = new string[_nsCache.Count];
        foreach (var kv in _nsCache) ns[kv.Value] = kv.Key;

        var sb = new StringBuilder();
        sb.Append("var $m = H5.setMetadata,\n");
        sb.Append("    $n = [").Append(string.Join(",", ns.Select(n => "\"" + n + "\""))).Append("];\n");
        foreach (var e in entries) sb.Append(e).Append('\n');
        return sb.ToString().TrimEnd('\n');
    }

    // ---- type-level metadata ----------------------------------------------

    private string? ConstructTypeMetadata(INamedTypeSymbol type)
    {
        var o = new MetaObj();

        // Declaring type (for nested types) + nested type list.
        if (type.ContainingType is { } dt && dt.Locations.Any(l => l.IsInSource))
            o.Raw("td", MetaTypeName(dt));

        var nested = type.GetTypeMembers().Where(IsReflectableType).ToList();
        if (nested.Count > 0)
            o.RawArray("nested", nested.Select(MetaTypeName));

        o.Num("att", TypeAttributesFlags(type));

        var acc = AccessibilityCode(type.DeclaredAccessibility);
        if (acc != 0) o.Num("a", acc);
        if (type.IsStatic) o.Bool("s", true);

        // Custom attributes.
        var attrs = ReflectableAttributes(type.GetAttributes());
        if (attrs.Count > 0)
            o.RawArray("at", attrs.Select(EmitAttributeInstance));

        // Members.
        if (type.TypeKind is TypeKind.Class or TypeKind.Struct or TypeKind.Interface or TypeKind.Enum)
        {
            var members = new List<string>();
            if (type.TypeKind == TypeKind.Enum)
            {
                members.Add(SyntheticEnumCtor(type));
                foreach (var f in type.GetMembers().OfType<IFieldSymbol>()
                             .Where(f => f.ConstantValue is not null && f.Name != "value__"))
                    members.Add(ConstructEnumField(f, type));
            }
            else
            {
                var reflectable = type.GetMembers().Where(IsReflectableMember).ToList();
                foreach (var m in reflectable.OrderBy(m => m, MemberOrder.Instance))
                {
                    var mi = ConstructMemberInfo(m);
                    if (mi is not null) members.Add(mi);
                }
                // Auto-property backing fields, appended after the members (as h5 does).
                foreach (var p in reflectable.OfType<IPropertySymbol>().Where(IsAutoProperty))
                    members.Add(ConstructBackingField(p));
            }
            if (members.Count > 0) o.RawArray("m", members);
        }

        // AttributeUsage → not-inherited (ni) / allow-multiple (am) flags.
        var aua = type.GetAttributes().FirstOrDefault(a =>
            a.AttributeClass?.ToDisplayString() == "System.AttributeUsageAttribute");
        if (aua is not null)
        {
            var inherited = true;
            var allowMultiple = false;
            foreach (var na in aua.NamedArguments)
            {
                if (na.Key == "AllowMultiple" && na.Value.Value is bool am) allowMultiple = am;
                else if (na.Key == "Inherited" && na.Value.Value is bool inh) inherited = inh;
            }
            if (!inherited) o.Bool("ni", true);
            if (allowMultiple) o.Bool("am", true);
        }

        return o.Count > 0 ? o.ToString() : null;
    }

    // ---- member-level metadata --------------------------------------------

    private string? ConstructMemberInfo(ISymbol m)
    {
        return m switch
        {
            IMethodSymbol { MethodKind: MethodKind.Constructor } c => ConstructConstructorInfo(c),
            IMethodSymbol method => ConstructMethodInfo(method),
            IFieldSymbol field => ConstructFieldInfo(field),
            IPropertySymbol prop => ConstructPropertyInfo(prop),
            IEventSymbol evt => ConstructEventInfo(evt),
            _ => null,
        };
    }

    private void AddCommonMember(MetaObj o, ISymbol m)
    {
        var attrs = ReflectableAttributes(m.GetAttributes());
        if (attrs.Count > 0) o.RawArray("at", attrs.Select(EmitAttributeInstance));
        if (m.IsOverride) o.Bool("ov", true);
        if (m.IsVirtual) o.Bool("v", true);
        if (m.IsAbstract) o.Bool("ab", true);
        var acc = AccessibilityCode(m.DeclaredAccessibility);
        if (acc != 0) o.Num("a", acc);
        if (m.IsSealed) o.Bool("sl", true);
        if (m.IsImplicitlyDeclared) o.Bool("isSynthetic", true);
        o.Str("n", m.Name);
    }

    private string ConstructConstructorInfo(IMethodSymbol ctor)
    {
        var o = new MetaObj();
        AddCommonMember(o, ctor);
        o.Num("t", MtConstructor);
        if (ctor.Parameters.Length > 0)
            o.RawArray("p", ctor.Parameters.Select(p => MetaTypeName(p.Type)));
        var pi = ctor.Parameters.Select(ConstructParameterInfo).ToList();
        if (pi.Count > 0) o.RawArray("pi", pi);
        // The JS constructor slot is "ctor"/"ctor$N" (the metadata name "n" keeps ".ctor").
        var sn = H5Naming.MemberJsName(ctor);
        o.Str("sn", sn.StartsWith(".", StringComparison.Ordinal) ? sn.Substring(1) : sn);
        return o.ToString();
    }

    private string ConstructMethodInfo(IMethodSymbol method)
    {
        var o = new MetaObj();
        AddCommonMember(o, method);
        if (method.IsStatic) o.Bool("is", true);
        o.Num("t", MtMethod);
        var pi = method.Parameters.Select(ConstructParameterInfo).ToList();
        if (pi.Count > 0) o.RawArray("pi", pi);
        if (method.TypeParameters.Length > 0 && !IsIgnoreGeneric(method))
        {
            o.Num("tpc", method.TypeParameters.Length);
            o.RawArray("tprm", method.TypeParameters.Select(tp => "\"" + tp.Name + "\""));
        }
        o.Str("sn", H5Naming.MemberJsName(method));
        o.Raw("rt", method.ReturnsVoid ? MetaTypeName(_compilation.GetSpecialType(SpecialType.System_Void)) : MetaTypeName(method.ReturnType));
        if (method.Parameters.Length > 0)
            o.RawArray("p", method.Parameters.Select(p => MetaTypeName(p.Type)));
        AddBox(o, method.ReturnType);
        return o.ToString();
    }

    private string ConstructFieldInfo(IFieldSymbol field)
    {
        var o = new MetaObj();
        AddCommonMember(o, field);
        if (field.IsStatic) o.Bool("is", true);
        o.Num("t", MtField);
        o.Raw("rt", MetaTypeName(field.Type));
        o.Str("sn", H5Naming.MemberJsName(field));
        if (field.IsReadOnly) o.Bool("ro", true);
        AddBox(o, field.Type);
        return o.ToString();
    }

    private string ConstructEnumField(IFieldSymbol field, INamedTypeSymbol enumType)
    {
        var o = new MetaObj();
        var acc = AccessibilityCode(field.DeclaredAccessibility);
        if (acc != 0) o.Num("a", acc);
        o.Str("n", field.Name);
        o.Bool("is", true);
        o.Num("t", MtField);
        o.Raw("rt", MetaTypeName(enumType));
        o.Str("sn", H5Naming.MemberJsName(field));
        AddBox(o, enumType);
        return o.ToString();
    }

    private string SyntheticEnumCtor(INamedTypeSymbol enumType)
    {
        var o = new MetaObj();
        o.Num("a", AccessibilityCode(Accessibility.Public));
        o.Bool("isSynthetic", true);
        o.Str("n", ".ctor");
        o.Num("t", MtConstructor);
        o.Str("sn", "ctor");
        return o.ToString();
    }

    private string ConstructPropertyInfo(IPropertySymbol prop)
    {
        var o = new MetaObj();
        AddCommonMember(o, prop);
        if (prop.IsStatic) o.Bool("is", true);
        o.Num("t", MtProperty);
        o.Raw("rt", MetaTypeName(prop.Type));
        if (prop.Parameters.Length > 0)
            o.RawArray("p", prop.Parameters.Select(p => MetaTypeName(p.Type)));
        if (prop.IsIndexer)
        {
            o.Bool("i", true);
            // Indexer parameters: the getter's params, or the setter's params minus the value.
            IEnumerable<IParameterSymbol>? src =
                prop.GetMethod is { } g ? g.Parameters
                : prop.SetMethod is { } s ? s.Parameters.Take(s.Parameters.Length - 1)
                : null;
            if (src is not null)
            {
                var ipi = src.Select(ConstructParameterInfo).ToList();
                if (ipi.Count > 0) o.RawArray("ipi", ipi);
            }
        }

        var fn = H5Naming.MemberJsName(prop);
        if (prop.GetMethod is { } getter && !SkipMember(getter))
            o.Raw("g", ConstructAccessor(getter, fn, isGetter: true, prop));
        if (prop.SetMethod is { } setter && !SkipMember(setter))
            o.Raw("s", ConstructAccessor(setter, fn, isGetter: false, prop));
        if (!prop.IsIndexer) o.Str("fn", fn);
        return o.ToString();
    }

    private string ConstructAccessor(IMethodSymbol accessor, string fieldName, bool isGetter, IPropertySymbol prop)
    {
        var o = new MetaObj();
        var acc = AccessibilityCode(accessor.DeclaredAccessibility);
        if (accessor.IsAbstract) o.Bool("ab", true);
        if (acc != 0) o.Num("a", acc);
        o.Str("n", accessor.Name);
        o.Num("t", MtMethod);
        if (accessor.Parameters.Length > 0)
            o.RawArray("p", accessor.Parameters.Select(p => MetaTypeName(p.Type)));
        o.Raw("rt", accessor.ReturnsVoid ? MetaTypeName(_compilation.GetSpecialType(SpecialType.System_Void)) : MetaTypeName(accessor.ReturnType));
        o.Str(isGetter ? "fg" : "fs", fieldName);
        if (accessor.IsStatic) o.Bool("is", true);
        AddBox(o, isGetter ? prop.Type : _compilation.GetSpecialType(SpecialType.System_Void));
        return o.ToString();
    }

    private string ConstructEventInfo(IEventSymbol evt)
    {
        var o = new MetaObj();
        AddCommonMember(o, evt);
        o.Num("t", MtEvent);
        if (evt.AddMethod is { } add) o.Raw("ad", ConstructMethodInfo(add));
        if (evt.RemoveMethod is { } rem) o.Raw("r", ConstructMethodInfo(rem));
        return o.ToString();
    }

    private string ConstructBackingField(IPropertySymbol prop)
    {
        var o = new MetaObj();
        o.Num("a", AccessibilityCode(Accessibility.Private));
        o.Bool("backing", true);
        o.Str("n", $"<{prop.Name}>k__BackingField");
        if (prop.IsStatic) o.Bool("is", true);
        o.Num("t", MtField);
        o.Raw("rt", MetaTypeName(prop.Type));
        o.Str("sn", H5Naming.MemberJsName(prop));
        AddBox(o, prop.Type);
        return o.ToString();
    }

    private string ConstructParameterInfo(IParameterSymbol p)
    {
        var o = new MetaObj();
        o.Str("n", p.Name);
        if (p.IsOptional && p.HasExplicitDefaultValue)
        {
            o.Raw("dv", ConstantLiteral(p.ExplicitDefaultValue, p.Type));
            o.Bool("o", true);
        }
        if (p.RefKind == RefKind.Out) o.Bool("out", true);
        else if (p.RefKind is RefKind.Ref) o.Bool("ref", true);
        if (p.IsParams) o.Bool("ip", true);
        o.Raw("pt", MetaTypeName(p.Type));
        o.Num("ps", p.Ordinal);
        return o.ToString();
    }

    // ---- boxing -----------------------------------------------------------

    private void AddBox(MetaObj o, ITypeSymbol type)
    {
        var t = type;
        if (t is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } n && n.TypeArguments.Length == 1)
            t = n.TypeArguments[0];

        if (t.TypeKind == TypeKind.Enum)
        {
            var tn = MetaTypeName(t);
            o.Raw("box", $"function ($v) {{ return H5.box($v, {tn}, System.Enum.toStringFn({tn}));}}");
            return;
        }
        if (IsBoxablePrimitive(t))
            o.Raw("box", $"function ($v) {{ return H5.box($v, {MetaTypeName(t)});}}");
    }

    private static bool IsBoxablePrimitive(ITypeSymbol t) => t.SpecialType is
        SpecialType.System_SByte or SpecialType.System_Byte or SpecialType.System_Int16
        or SpecialType.System_UInt16 or SpecialType.System_Int32 or SpecialType.System_UInt32
        or SpecialType.System_Int64 or SpecialType.System_UInt64 or SpecialType.System_Single
        or SpecialType.System_Double or SpecialType.System_Boolean or SpecialType.System_Char
        or SpecialType.System_Decimal or SpecialType.System_DateTime;

    // ---- filtering --------------------------------------------------------

    /// <summary>A source type that participates in reflection metadata.</summary>
    private static bool IsReflectableType(INamedTypeSymbol type)
    {
        if (!type.Locations.Any(l => l.IsInSource)) return false;
        if (type.IsImplicitlyDeclared) return false;
        if (H5Naming.IsExternalType(type)) return false;
        if (type.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == "H5.NonScriptableAttribute")) return false;
        return true;
    }

    /// <summary>A member included in a type's reflection metadata (default policy: all members
    /// except explicit interface implementations, accessors, and non-scriptable members).</summary>
    private bool IsReflectableMember(ISymbol m)
    {
        if (SkipMember(m)) return false;
        if (m is IMethodSymbol { MethodKind: MethodKind.Constructor }) return true;
        return m.Kind is SymbolKind.Method or SymbolKind.Field or SymbolKind.Property or SymbolKind.Event;
    }

    private static bool SkipMember(ISymbol m)
    {
        if (!m.ExplicitInterfaceImplementations().IsEmpty()) return true;
        if (m.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == "H5.NonScriptableAttribute")) return true;
        if (m is IMethodSymbol meth)
        {
            if (meth.MethodKind is MethodKind.PropertyGet or MethodKind.PropertySet
                or MethodKind.EventAdd or MethodKind.EventRemove or MethodKind.StaticConstructor) return true;
            if (meth.AssociatedSymbol is not null) return true;
        }
        if (m is IFieldSymbol f && f.AssociatedSymbol is not null) return true; // auto-prop backing
        if (m.Name == "value__") return true;
        return false;
    }

    private static bool IsIgnoreGeneric(ISymbol s)
        => s.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == "H5.IgnoreGenericAttribute");

    private static List<AttributeData> ReflectableAttributes(IEnumerable<AttributeData> attrs)
        => attrs.Where(a => a.AttributeClass is { } ac
                            && ac.Locations.Any(l => l.IsInSource)
                            && !ac.GetAttributes().Any(x => x.AttributeClass?.ToDisplayString() == "H5.NonScriptableAttribute"))
                .ToList();

    // ---- attribute codes / type names -------------------------------------

    /// <summary>NRefactory Accessibility codes used by h5.js: Private=1, Public=2, Protected=3,
    /// Internal=4, ProtectedOrInternal=5, ProtectedAndInternal=6.</summary>
    private static int AccessibilityCode(Accessibility a) => a switch
    {
        Accessibility.Private => 1,
        Accessibility.Public => 2,
        Accessibility.Protected => 3,
        Accessibility.Internal => 4,
        Accessibility.ProtectedOrInternal => 5,
        Accessibility.ProtectedAndInternal => 6,
        _ => 0,
    };

    /// <summary>The System.Reflection.TypeAttributes bitmask h5.js stores in "att".</summary>
    private static int TypeAttributesFlags(INamedTypeSymbol type)
    {
        var nested = type.ContainingType is not null;
        int vis = nested
            ? type.DeclaredAccessibility switch
            {
                Accessibility.Public => 2,
                Accessibility.Private => 3,
                Accessibility.Protected => 4,
                Accessibility.Internal => 5,
                Accessibility.ProtectedAndInternal => 6,
                Accessibility.ProtectedOrInternal => 7,
                _ => 3,
            }
            : (type.DeclaredAccessibility == Accessibility.Public ? 1 : 0);

        const int Interface = 0x20, Abstract = 0x80, Sealed = 0x100, BeforeFieldInit = 0x100000;
        int flags = vis;
        switch (type.TypeKind)
        {
            case TypeKind.Interface: flags |= Interface | Abstract | BeforeFieldInit; break;
            case TypeKind.Enum: flags |= Sealed; break;
            case TypeKind.Struct: flags |= Sealed | BeforeFieldInit; break;
            default: // class
                if (type.IsStatic) flags |= Abstract | Sealed;
                else { if (type.IsAbstract) flags |= Abstract; if (type.IsSealed) flags |= Sealed; }
                flags |= BeforeFieldInit;
                break;
        }
        return flags;
    }

    /// <summary>The name used in the <c>$m("…")</c> key (definition form for generics: Name$arity).</summary>
    private string MetaTypeDefName(INamedTypeSymbol type)
    {
        var name = _names.TypeFullName(type);
        // Only the type's OWN generic arity earns a $N suffix — a non-generic type nested in a
        // generic one (Arity 0 but IsGenericType true) keeps its plain name, as H5.define does.
        return type.Arity > 0 ? name + "$" + type.Arity : name;
    }

    /// <summary>A type reference as it appears in metadata, with the namespace compacted to
    /// <c>$n[k]</c>. Method type parameters and ignore-generic type params erase to object.</summary>
    private string MetaTypeName(ITypeSymbol type)
    {
        if (type is ITypeParameterSymbol tp)
            return tp.TypeParameterKind == TypeParameterKind.Method ? "System.Object" : tp.Name;

        if (type is IArrayTypeSymbol arr)
            return $"System.Array.type({MetaTypeName(arr.ElementType)})";

        var full = TypeRef(type);
        var ns = type.ContainingNamespace?.ToDisplayString();
        if (!string.IsNullOrEmpty(ns) && ns != "<global namespace>" && full.StartsWith(ns + ".", StringComparison.Ordinal))
            return $"$n[{NsIndex(ns!)}]" + full.Substring(ns!.Length);
        return full;
    }

    private int NsIndex(string ns)
    {
        if (_nsCache.TryGetValue(ns, out var k)) return k;
        k = _nsCache.Count;
        _nsCache[ns] = k;
        return k;
    }

    /// <summary>
    /// A single custom-attribute instance: <c>new T(ctorArgs)</c>, wrapped in
    /// <c>H5.apply(..., { Named: value })</c> when the attribute sets named properties/fields.
    /// </summary>
    private string EmitAttributeInstance(AttributeData attr)
    {
        var ctorArgs = string.Join(", ", attr.ConstructorArguments.Select(TypedConstantJs));
        var ctor = $"new {TypeRef(attr.AttributeClass!)}({ctorArgs})";
        if (attr.NamedArguments.Length == 0) return ctor;

        var named = string.Join(", ", attr.NamedArguments.Select(
            kv => $"{NameMangler.JsPropertyKey(kv.Key)}: {TypedConstantJs(kv.Value)}"));
        return $"H5.apply({ctor}, {{{named}}})";
    }

    /// <summary>An attribute argument value as JS: enums fold to their numeric value, typeof to
    /// a type reference, arrays to a JS array literal, and everything else to a plain literal.</summary>
    private string TypedConstantJs(TypedConstant c)
    {
        if (c.IsNull) return "null";
        return c.Kind switch
        {
            TypedConstantKind.Array => "[" + string.Join(", ", c.Values.Select(TypedConstantJs)) + "]",
            TypedConstantKind.Type => c.Value is ITypeSymbol t ? TypeRef(t) : "null",
            _ => ConstantLiteral(c.Value, c.Type!),
        };
    }

    /// <summary>A tiny ordered JSON object writer producing compact h5-style metadata.</summary>
    private sealed class MetaObj
    {
        private readonly StringBuilder _sb = new("{");
        private bool _first = true;
        public int Count { get; private set; }

        private void Key(string k)
        {
            if (!_first) _sb.Append(',');
            _first = false;
            Count++;
            _sb.Append('"').Append(k).Append("\":");
        }

        public void Num(string k, int v) { Key(k); _sb.Append(v.ToString(CultureInfo.InvariantCulture)); }
        public void Bool(string k, bool v) { Key(k); _sb.Append(v ? "true" : "false"); }
        public void Str(string k, string v) { Key(k); _sb.Append('"').Append(v.Replace("\\", "\\\\").Replace("\"", "\\\"")).Append('"'); }
        public void Raw(string k, string rawJs) { Key(k); _sb.Append(rawJs); }
        public void RawArray(string k, IEnumerable<string> items) { Key(k); _sb.Append('[').Append(string.Join(",", items)).Append(']'); }

        public override string ToString() => _sb.ToString() + "}";
    }
}

/// <summary>Member ordering matching the legacy compiler's MemberOrderer: methods (and
/// constructors) first by name/signature, then properties, fields, events — each by name.</summary>
internal sealed class MemberOrder : IComparer<ISymbol>
{
    public static readonly MemberOrder Instance = new();

    private static int Rank(ISymbol s) => s switch
    {
        IMethodSymbol => 0,
        IPropertySymbol => 1,
        IFieldSymbol => 2,
        IEventSymbol => 3,
        _ => 4,
    };

    public int Compare(ISymbol? x, ISymbol? y)
    {
        if (x is null || y is null) return 0;
        var rx = Rank(x); var ry = Rank(y);
        if (rx != ry) return rx - ry;
        var n = string.CompareOrdinal(x.Name, y.Name);
        if (n != 0) return n;
        if (x is IMethodSymbol mx && y is IMethodSymbol my)
        {
            if (mx.Parameters.Length != my.Parameters.Length) return mx.Parameters.Length - my.Parameters.Length;
            var px = string.Join(",", mx.Parameters.Select(p => p.Type.ToDisplayString()));
            var py = string.Join(",", my.Parameters.Select(p => p.Type.ToDisplayString()));
            var pr = string.CompareOrdinal(px, py);
            if (pr != 0) return pr;
            return mx.TypeParameters.Length - my.TypeParameters.Length;
        }
        return 0;
    }
}

internal static class ReflectionSymbolExtensions
{
    public static System.Collections.Immutable.ImmutableArray<ISymbol> ExplicitInterfaceImplementations(this ISymbol s) => s switch
    {
        IMethodSymbol m => m.ExplicitInterfaceImplementations.CastArray<ISymbol>(),
        IPropertySymbol p => p.ExplicitInterfaceImplementations.CastArray<ISymbol>(),
        IEventSymbol e => e.ExplicitInterfaceImplementations.CastArray<ISymbol>(),
        _ => System.Collections.Immutable.ImmutableArray<ISymbol>.Empty,
    };

    public static bool IsEmpty<T>(this System.Collections.Immutable.ImmutableArray<T> a) => a.IsDefaultOrEmpty;
}
