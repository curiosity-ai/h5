using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace H5.Translator.Roslyn;

public sealed partial class Emitter
{
    private void EmitType(INamedTypeSymbol type)
    {
        switch (type.TypeKind)
        {
            case TypeKind.Enum:
                EmitEnum(type);
                break;
            case TypeKind.Interface:
                EmitInterface(type);
                break;
            case TypeKind.Class:
            case TypeKind.Struct:
                EmitClassLike(type);
                break;
            case TypeKind.Delegate:
                break; // delegates map onto plain functions
            default:
                Unsupported(type.DeclaringSyntaxReferences[0].GetSyntax(), $"type kind {type.TypeKind}");
                break;
        }
    }

    /// <summary>Full JS name a type is registered / referenced under.</summary>
    private string TypeRef(ITypeSymbol type)
    {
        // `dynamic` has no runtime type of its own — it is System.Object at runtime.
        if (type.TypeKind == TypeKind.Dynamic) return "System.Object";

        if (type is INamedTypeSymbol named)
        {
            // External (BCL / DOM) types are named by their runtime binding: [Name], a
            // [Scope]/[GlobalMethods] global (e.g. H5.Core.dom's HTMLElement), or the dotted
            // metadata name. [Name] applies ONLY here — an H5-compiled type ignores it.
            if (!H5Naming.IsH5CompiledSource(named))
            {
                var name = H5Naming.GetName(named);
                if (name is not null) return name;

                if (ScopedExternalName(named) is { } scoped) return scoped;

                var ns = named.ContainingNamespace?.ToDisplayString();
                if (named.IsGenericType && named.TypeArguments.Length > 0)
                {
                    var baseName = (string.IsNullOrEmpty(ns) ? "" : ns + ".") + StripArity(named.Name) + "$" + named.Arity;
                    var args = string.Join(", ", named.TypeArguments.Select(TypeRef));
                    return $"{baseName}({args})";
                }
                return string.IsNullOrEmpty(ns) ? named.Name : ns + "." + named.Name;
            }

            // A type this compiler emits — either from source, or from a referenced H5-compiled
            // assembly (a package built with --emit-package). Both are defined via H5.define under
            // their full nested JS name, so a reference must use the same name (nested-aware).
            // The name carries only the type's OWN arity suffix; the type arguments passed are the
            // EFFECTIVE ones (enclosing + own), so a type nested in a generic (e.g.
            // IconToggle<int>.Item) resolves to tss.IconToggle.Item(System.Int32).
            var effArgs = EffectiveTypeArguments(named);
            var defName = named.Arity > 0 ? _names.TypeFullName(named) + "$" + named.Arity : _names.TypeFullName(named);
            if (effArgs.Count > 0)
                return $"{defName}({string.Join(", ", effArgs.Select(TypeRef))})";
            return defName;
        }
        if (type is IArrayTypeSymbol) return "System.Array";
        return type.Name;
    }

    private static string StripArity(string name)
    {
        var i = name.IndexOf('`');
        return i >= 0 ? name.Substring(0, i) : name;
    }

    /// <summary>
    /// The type parameters a type's H5.define is a function of: its enclosing types' parameters
    /// (outermost first) followed by its own. A type nested in a generic type can reference the
    /// enclosing type parameters in C#, so — like the legacy compiler — its define is emitted as
    /// <c>function (TOuter…) { return {…}; }</c> even when the nested type has no parameters of its
    /// own (e.g. <c>IconToggle&lt;T&gt;.Item</c> → <c>H5.define("tss.IconToggle.Item", function (T){…})</c>).
    /// </summary>
    private static List<ITypeParameterSymbol> EffectiveTypeParameters(INamedTypeSymbol type)
    {
        var result = new List<ITypeParameterSymbol>();
        for (INamedTypeSymbol? t = type; t is not null; t = t.ContainingType)
            result.InsertRange(0, t.TypeParameters);
        return result;
    }

    /// <summary>The type arguments to pass when referencing a type: its enclosing types' arguments
    /// (outermost first) then its own — the mirror of <see cref="EffectiveTypeParameters"/>.</summary>
    private static List<ITypeSymbol> EffectiveTypeArguments(INamedTypeSymbol type)
    {
        var result = new List<ITypeSymbol>();
        for (INamedTypeSymbol? t = type; t is not null; t = t.ContainingType)
            result.InsertRange(0, t.TypeArguments);
        return result;
    }

    /// <summary>
    /// The JS name of an external type nested under a <c>[Scope]</c>/<c>[GlobalMethods]</c>
    /// binding: the scope prefix (empty for a global scope) plus the type names between the
    /// scope and this type — so <c>H5.Core.dom.HTMLElement</c> becomes <c>HTMLElement</c>.
    /// Null when no enclosing scope applies.
    /// </summary>
    /// <summary>
    /// A static member reference: for a <c>[Scope]</c>/<c>[GlobalMethods]</c> binding it is the
    /// bare (or scope-prefixed) member — <c>dom.window</c> → <c>window</c>, <c>dom.alert(…)</c>
    /// → <c>alert(…)</c> — otherwise the qualified <c>Type.member</c>.
    /// </summary>
    private string StaticMemberAccess(ISymbol member)
    {
        var name = H5Naming.MemberJsName(member);
        var prefix = H5Naming.ScopePrefix(member.ContainingType);
        if (prefix is null) return $"{TypeRef(member.ContainingType)}.{name}";
        return prefix.Length == 0 ? name : $"{prefix}.{name}";
    }

    private string? ScopedExternalName(INamedTypeSymbol named)
    {
        var names = new List<string>();
        for (INamedTypeSymbol? t = named; t is not null; t = t.ContainingType)
        {
            if (H5Naming.ScopePrefix(t) is { } prefix)
            {
                if (names.Count == 0) return null; // referencing the scope type itself — not a member
                var path = string.Join(".", names);
                return string.IsNullOrEmpty(prefix) ? path : prefix + "." + path;
            }
            names.Insert(0, H5Naming.GetName(t) ?? StripArity(t.Name));
        }
        return null;
    }

    /// <summary>The JS literal for <c>default(enum)</c>. A string-backed enum
    /// (<c>[Enum(Emit.StringName*)]</c>) defaults to the string of its zero-valued member;
    /// every other mode is a numeric enum at runtime and defaults to 0.</summary>
    private string EnumDefaultLiteral(INamedTypeSymbol enumType)
    {
        var mode = H5Naming.EnumEmitMode(enumType);
        if (mode is 3 or 4 or 5 or 6)
        {
            var zero = enumType.GetMembers().OfType<IFieldSymbol>()
                .FirstOrDefault(f => f.HasConstantValue && Convert.ToInt64(f.ConstantValue) == 0);
            return zero is not null ? JsString(H5Naming.EnumStringName(zero, mode)) : "null";
        }
        return "0";
    }

    private void EmitEnum(INamedTypeSymbol type)
    {
        _w.Write($"H5.define(\"{_names.TypeFullName(type)}\", ");
        var isFlags = type.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == "System.FlagsAttribute");
        var mode = H5Naming.EnumEmitMode(type);
        // Emit.StringName* modes back the enum with strings (its [Name] on each member); every
        // other mode keeps the numeric ordinals. A string-backed enum also declares
        // $utype: System.String so the runtime treats its members as strings (this is what makes
        // `x === "top"`-style comparisons against enum members work).
        var stringMode = mode is 3 or 4 or 5 or 6;
        _w.Block(() =>
        {
            _w.WriteLine("$kind: \"enum\",");
            if (isFlags) _w.WriteLine("$flags: true,");
            _w.Write("statics: ");
            _w.Block(() =>
            {
                _w.Write("fields: ");
                _w.Block(() =>
                {
                    var fields = type.GetMembers().OfType<IFieldSymbol>().Where(f => f.HasConstantValue).ToList();
                    for (var i = 0; i < fields.Count; i++)
                    {
                        var value = stringMode
                            ? JsString(H5Naming.EnumStringName(fields[i], mode))
                            : Convert.ToInt64(fields[i].ConstantValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
                        _w.Write($"{NameMangler.JsPropertyKey(H5Naming.MemberJsName(fields[i]))}: {value}");
                        _w.WriteLine(i < fields.Count - 1 ? "," : "");
                    }
                });
                _w.WriteLine();
            });
            _w.WriteLine(stringMode ? "," : "");
            if (stringMode) _w.WriteLine("$utype: System.String");
        });
        _w.WriteLine(");");
    }

    private void EmitInterface(INamedTypeSymbol type)
    {
        // A generic interface is a function of its type parameters (like generic classes),
        // so references such as IContainer$1(T) resolve at runtime. Effective parameters include
        // an enclosing generic type's, matching the class treatment.
        var typeParams = EffectiveTypeParameters(type);
        var isGeneric = typeParams.Count > 0;
        var fullName = type.Arity > 0 ? _names.TypeFullName(type) + "$" + type.Arity : _names.TypeFullName(type);

        _w.Write($"H5.define(\"{fullName}\", ");
        if (isGeneric) _w.Write($"function ({string.Join(", ", typeParams.Select(p => p.Name))}) {{ return ");
        // $variance records each OWN type parameter's variance so the runtime can model
        // covariant/contravariant interface assignability: 2 = covariant (out), 1 = contravariant
        // (in), 0 = invariant. Only emitted when at least one parameter is variant (as H5 does).
        var variances = type.TypeParameters.Select(p => p.Variance switch
        {
            VarianceKind.Out => 2,
            VarianceKind.In => 1,
            _ => 0,
        }).ToList();
        var hasVariance = variances.Any(v => v != 0);

        _w.Block(() =>
        {
            _w.Write("$kind: \"interface\"");
            var bases = type.Interfaces.Where(i => H5Naming.IsInheritableInterface(i)).ToList();
            if (bases.Count > 0)
            {
                _w.WriteLine(",");
                _w.Write($"inherits: function () {{ return [{string.Join(", ", bases.Select(TypeRef))}]; }}");
            }
            if (hasVariance)
            {
                _w.WriteLine(",");
                _w.Write($"$variance: [{string.Join(", ", variances)}]");
            }
            _w.WriteLine();
        });
        if (isGeneric) _w.Write("; }");
        _w.WriteLine(");");
    }

    private void EmitClassLike(INamedTypeSymbol type)
    {
        var prevEmitType = _currentEmitType;
        _currentEmitType = type;
        try { EmitClassLikeCore(type); }
        finally { _currentEmitType = prevEmitType; }
    }

    private void EmitClassLikeCore(INamedTypeSymbol type)
    {
        var entryPoint = _compilation.GetEntryPoint(System.Threading.CancellationToken.None);

        // A generic type is defined as a function of its type parameters, returning the
        // config object (H5.define("Name$N", function (T) { return { … }; })); the type
        // parameters are then in scope at runtime for new T()/default(T)/typeof(T). A type nested
        // in a generic type is a function of the ENCLOSING parameters too (its own arity may be 0),
        // so the define name carries only its own arity but the function takes every effective one.
        var typeParams = EffectiveTypeParameters(type);
        var isGeneric = typeParams.Count > 0;
        var fullName = type.Arity > 0 ? _names.TypeFullName(type) + "$" + type.Arity : _names.TypeFullName(type);

        _w.Write($"H5.define(\"{fullName}\", ");
        if (isGeneric) _w.Write($"function ({string.Join(", ", typeParams.Select(p => p.Name))}) {{ return ");
        _w.Block(() =>
        {
            var sections = new List<Action>();

            // $kind for structs.
            if (type.TypeKind == TypeKind.Struct)
            {
                sections.Add(() => _w.Write("$kind: \"struct\""));
            }

            // $literal marks an [ObjectLiteral] type: instances are plain JS objects (construction
            // emits {} + initializer), and the runtime treats the type as a literal for is/as/typeof
            // rather than a real class. Matches the legacy compiler's $literal:true flag.
            if (type.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == "H5.ObjectLiteralAttribute"))
            {
                sections.Add(() => _w.Write("$literal: true"));
            }

            // inherits: base class + implemented interfaces the runtime tracks (source or a
            // referenced H5-compiled library — so `x is IFoo`/`as IFoo` against a library
            // interface resolves; external BCL interfaces are omitted, matching H5).
            var inherits = new List<string>();
            if (type.BaseType is { } bt && bt.SpecialType != SpecialType.System_Object
                && bt.TypeKind != TypeKind.Error && !IsValueTypeBase(bt))
            {
                inherits.Add(TypeRef(bt));
            }
            inherits.AddRange(type.Interfaces.Where(i => H5Naming.IsInheritableInterface(i)).Select(TypeRef));
            if (inherits.Count > 0)
            {
                // Lazy inherits (a function, as the legacy compiler emits): the config object
                // is built before H5.define runs, so evaluating an eager array would resolve a
                // self/forward reference (e.g. class C : IFoo<C>) before the type is registered.
                sections.Add(() => _w.Write($"inherits: function () {{ return [{string.Join(", ", inherits)}]; }}"));
            }

            // alias: maps each implicitly-implemented interface member's plain slot to the
            // mangled interface slot, so access through the interface type resolves.
            var aliases = H5Naming.InterfaceAliasPairs(type);
            if (aliases.Count > 0)
            {
                sections.Add(() => _w.Write(
                    $"alias: [{string.Join(", ", aliases.SelectMany(a => new[] { JsString(a.plain), JsString(a.mangled) }))}]"));
            }

            // main: entry point.
            if (entryPoint is not null && SymbolEqualityComparer.Default.Equals(entryPoint.ContainingType, type))
            {
                sections.Add(() => EmitEntryPoint(entryPoint));
            }

            // statics { fields, ctors.init/ctor, methods, properties }
            var staticsBody = Capture(() => EmitStatics(type, fullName));
            if (staticsBody.Trim().Length > 0)
            {
                sections.Add(() => { _w.Write("statics: "); _w.Write(staticsBody); });
            }

            // instance fields
            var fieldsBody = Capture(() => EmitInstanceFields(type));
            if (fieldsBody.Trim().Length > 0)
            {
                sections.Add(() => { _w.Write("fields: "); _w.Write(fieldsBody); });
            }

            // instance ctors
            var ctorsBody = Capture(() => { if (!TryEmitRecordCtors(type)) EmitInstanceCtors(type); });
            if (ctorsBody.Trim().Length > 0)
            {
                sections.Add(() => { _w.Write("ctors: "); _w.Write(ctorsBody); });
            }

            // instance properties (with logic)
            var propsBody = Capture(() => EmitInstanceProperties(type));
            if (propsBody.Trim().Length > 0)
            {
                sections.Add(() => { _w.Write("props: "); _w.Write(propsBody); });
            }

            // instance methods
            var methodsBody = Capture(() => EmitInstanceMethods(type, entryPoint));
            if (methodsBody.Trim().Length > 0)
            {
                sections.Add(() => { _w.Write("methods: "); _w.Write(methodsBody); });
            }

            for (var i = 0; i < sections.Count; i++)
            {
                sections[i]();
                _w.WriteLine(i < sections.Count - 1 ? "," : "");
            }
        });
        if (isGeneric) _w.Write("; }");
        _w.WriteLine(");");
    }

    private static bool IsValueTypeBase(INamedTypeSymbol baseType)
        => baseType.SpecialType is SpecialType.System_ValueType or SpecialType.System_Enum;

    /// <summary>Auto-properties are stored as plain fields; only these + real fields appear here.</summary>
    private void EmitInstanceFields(INamedTypeSymbol type)
    {
        var entries = InstanceFieldSlots(type).ToList();
        if (entries.Count == 0) return;
        _w.Block(() =>
        {
            for (var i = 0; i < entries.Count; i++)
            {
                _w.Write($"{entries[i].name}: {entries[i].def}");
                _w.WriteLine(i < entries.Count - 1 ? "," : "");
            }
        });
    }

    private IEnumerable<(string name, string def, ISymbol symbol)> InstanceFieldSlots(INamedTypeSymbol type)
    {
        foreach (var m in type.GetMembers())
        {
            if (m.IsStatic) continue;
            if (m is IFieldSymbol f && !f.IsConst && f.AssociatedSymbol is null && f.CanBeReferencedByName)
                yield return (H5Naming.MemberJsName(f), DefaultValueLiteral(f.Type), f);
            else if (m is IPropertySymbol p && !p.IsAbstract && !p.IsIndexer
                     && (IsAutoProperty(p) || (type.IsRecord && p.IsImplicitlyDeclared && p.Name != "EqualityContract")))
                yield return (H5Naming.MemberJsName(p), DefaultValueLiteral(p.Type), p);
            else if (m is IEventSymbol ev && IsFieldLikeEvent(ev))
                yield return (H5Naming.MemberJsName(ev), "null", ev);
            else if (m is IPropertySymbol fbp && IsFieldBackedProperty(fbp))
                yield return (PropertyBackingName(fbp), DefaultValueLiteral(fbp.Type), fbp);
        }
    }

    /// <summary>A field-like event (no explicit add/remove) — backed by a null delegate field.</summary>
    internal static bool IsFieldLikeEvent(IEventSymbol ev)
        => ev.AddMethod is null or { IsImplicitlyDeclared: true };

    // ---- shared helpers ----------------------------------------------------

    internal static bool IsAutoProperty(IPropertySymbol prop)
    {
        foreach (var reference in prop.DeclaringSyntaxReferences)
        {
            if (reference.GetSyntax() is PropertyDeclarationSyntax decl)
            {
                if (decl.ExpressionBody is not null) return false;
                if (decl.AccessorList is null) return false;
                return decl.AccessorList.Accessors.All(a => a.Body is null && a.ExpressionBody is null);
            }
        }
        return false;
    }

    private ExpressionSyntax? FieldInitializerSyntax(IFieldSymbol field)
    {
        foreach (var reference in field.DeclaringSyntaxReferences)
            if (reference.GetSyntax() is VariableDeclaratorSyntax { Initializer: { } init })
                return init.Value;
        return null;
    }

    private ExpressionSyntax? AutoPropertyInitializerSyntax(IPropertySymbol prop)
    {
        foreach (var reference in prop.DeclaringSyntaxReferences)
            if (reference.GetSyntax() is PropertyDeclarationSyntax { Initializer: { } init })
                return init.Value;
        return null;
    }

    private string DefaultValueLiteral(ITypeSymbol type)
    {
        // default(T) for an unconstrained/struct type parameter must defer to the runtime, which
        // picks 0 / false / null based on the *actual* T at construction — exactly what H5 emits
        // (H5.getDefaultValue(T)). Emitting a bare null here would wrongly seed value-type T
        // (int/bool/enum/struct) with null instead of its zeroed default. Only safe when T is a
        // type parameter of the type currently being emitted (hence bound as a JS function
        // parameter of the define); a T inherited from an enclosing generic type is not in scope
        // here, so fall through to null (that nested type is emitted non-generically).
        if (type is ITypeParameterSymbol tp)
        {
            // In scope when it is one of the effective type parameters of the type being emitted
            // (own or from an enclosing generic type) — those are the define's JS function
            // parameters. A method type parameter (not threaded here) falls back to null.
            var inScope = _currentEmitType is not null
                && tp.TypeParameterKind == TypeParameterKind.Type
                && EffectiveTypeParameters(_currentEmitType).Any(p => SymbolEqualityComparer.Default.Equals(p, tp));
            return inScope ? $"H5.getDefaultValue({TypeRef(type)})" : "null";
        }
        if (type.TypeKind == TypeKind.Enum) return EnumDefaultLiteral((INamedTypeSymbol)type);
        if (type is INamedTypeSymbol { TypeKind: TypeKind.Struct } st && st.Locations.Any(l => l.IsInSource))
            return $"{TypeRef(st)}.getDefaultValue()";
        switch (type.SpecialType)
        {
            case SpecialType.System_Boolean: return "false";
            case SpecialType.System_Char:
            case SpecialType.System_SByte:
            case SpecialType.System_Byte:
            case SpecialType.System_Int16:
            case SpecialType.System_UInt16:
            case SpecialType.System_Int32:
            case SpecialType.System_UInt32:
            case SpecialType.System_Int64:
            case SpecialType.System_UInt64:
            case SpecialType.System_Single:
            case SpecialType.System_Double:
            case SpecialType.System_Decimal:
                return "0";
            default:
                return "null";
        }
    }
}
