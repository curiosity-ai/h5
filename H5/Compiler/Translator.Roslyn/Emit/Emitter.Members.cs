using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace H5.Translator.Roslyn;

public sealed partial class Emitter
{
    // ---- statics -----------------------------------------------------------

    private void EmitStatics(INamedTypeSymbol type, string fullName)
    {
        var staticFields = type.GetMembers().Where(m => m.IsStatic).Select(m => m switch
        {
            IFieldSymbol f when !f.IsConst && f.AssociatedSymbol is null => ((string name, string def)?)(H5Naming.MemberJsName(f), DefaultValueLiteral(f.Type)),
            IPropertySymbol p when IsAutoProperty(p) => (H5Naming.MemberJsName(p), DefaultValueLiteral(p.Type)),
            _ => null,
        }).Where(x => x is not null).Select(x => x!.Value).ToList();

        var staticInitAssignments = StaticInitializers(type).ToList();
        var staticCtor = type.StaticConstructors.FirstOrDefault(c => c.DeclaringSyntaxReferences.Length > 0);
        var staticMethods = type.GetMembers().OfType<IMethodSymbol>()
            .Where(m => m.IsStatic && !m.IsImplicitlyDeclared && IsEmittableMethod(m) && !IsEntryPoint(m))
            .ToList();
        var staticProps = type.GetMembers().OfType<IPropertySymbol>()
            .Where(p => p.IsStatic && !p.IsAbstract && !IsAutoProperty(p) && !p.IsIndexer)
            .ToList();

        var sections = new List<Action>();

        // Structs expose a getDefaultValue() static returning a zero-initialized value.
        if (type.TypeKind == TypeKind.Struct)
        {
            var slots = InstanceFieldSlots(type).ToList();
            staticMethods = staticMethods; // (no-op, keep ordering)
            sections.Add(() =>
            {
                _w.Write("methods: ");
                _w.Block(() =>
                {
                    _w.Write("getDefaultValue: function () ");
                    _w.Block(() =>
                    {
                        _w.WriteLine("var $ = Object.create(this.prototype);");
                        foreach (var (name, def, _) in slots) _w.WriteLine($"$.{name} = {def};");
                        _w.WriteLine("return $;");
                    });
                    if (staticMethods.Count > 0)
                    {
                        _w.WriteLine(",");
                        for (var i = 0; i < staticMethods.Count; i++)
                        {
                            EmitMethodEntry(staticMethods[i]);
                            _w.WriteLine(i < staticMethods.Count - 1 ? "," : "");
                        }
                    }
                    else { _w.WriteLine(); }
                });
            });
            staticMethods = new List<IMethodSymbol>(); // consumed above
        }

        if (staticFields.Count > 0)
        {
            sections.Add(() =>
            {
                _w.Write("fields: ");
                _w.Block(() =>
                {
                    for (var i = 0; i < staticFields.Count; i++)
                    {
                        _w.Write($"{staticFields[i].name}: {staticFields[i].def}");
                        _w.WriteLine(i < staticFields.Count - 1 ? "," : "");
                    }
                });
            });
        }

        if (staticInitAssignments.Count > 0 || staticCtor is not null)
        {
            sections.Add(() =>
            {
                _w.Write("ctors: ");
                _w.Block(() =>
                {
                    _w.Write("init: function () ");
                    _w.Block(() =>
                    {
                        // For a generic type, the static init runs per closed instantiation with
                        // `this` bound to that closed type — where its statics live and where
                        // instances read them (Name$arity(args).Field). Assigning through the
                        // open generic-definition name (fullName) would set a property nothing
                        // reads. A non-generic type's static init also runs with `this` = the type,
                        // so `this` is correct for both.
                        var staticRef = EffectiveTypeParameters(type).Count > 0 ? "this" : fullName;
                        foreach (var (target, init) in staticInitAssignments)
                        {
                            _w.Write($"{staticRef}.{target} = ");
                            EmitExpression(init);
                            _w.WriteLine(";");
                        }
                        if (staticCtor?.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is ConstructorDeclarationSyntax { Body: { } body })
                        {
                            foreach (var s in body.Statements) EmitStatement(s);
                        }
                    });
                    _w.WriteLine();
                });
            });
        }

        if (staticMethods.Count > 0)
        {
            sections.Add(() =>
            {
                _w.Write("methods: ");
                EmitMethodMap(staticMethods, fullName);
            });
        }

        if (staticProps.Count > 0)
        {
            sections.Add(() =>
            {
                _w.Write("properties: ");
                EmitPropertyMap(staticProps);
            });
        }

        if (sections.Count == 0) return;

        _w.Block(() =>
        {
            for (var i = 0; i < sections.Count; i++)
            {
                sections[i]();
                _w.WriteLine(i < sections.Count - 1 ? "," : "");
            }
        });
    }

    private IEnumerable<(string target, ExpressionSyntax init)> StaticInitializers(INamedTypeSymbol type)
    {
        foreach (var m in type.GetMembers().Where(m => m.IsStatic))
        {
            if (m is IFieldSymbol f && !f.IsConst && f.AssociatedSymbol is null && FieldInitializerSyntax(f) is { } fi)
                yield return (H5Naming.MemberJsName(f), fi);
            else if (m is IPropertySymbol p && IsAutoProperty(p) && AutoPropertyInitializerSyntax(p) is { } pi)
                yield return (H5Naming.MemberJsName(p), pi);
        }
    }

    // ---- instance constructors ---------------------------------------------

    private readonly Dictionary<ISymbol, string> _ctorNames = new(SymbolEqualityComparer.Default);

    private string CtorName(IMethodSymbol ctor)
    {
        ctor = ctor.OriginalDefinition;
        // External BCL types were baked into h5.js with H5's OverloadsCollection ctor numbering;
        // match it so e.g. new Guid(string) resolves to $ctor4. A referenced H5-compiled package
        // (non-source but non-external) was emitted by THIS compiler's own numbering below, so it
        // must be numbered the same way here — over its full ctor set (private ones included,
        // surfaced via MetadataImportOptions.All) — for call sites to resolve to the same $ctorN.
        if (!H5Naming.IsH5CompiledSource(ctor.ContainingType))
            return H5Naming.ConstructorName(ctor);
        if (_ctorNames.TryGetValue(ctor, out var cached)) return cached;

        var ctors = ctor.ContainingType.InstanceConstructors
            .Where(c => !IsRecordCopyCtor(c))
            .OrderBy(c => c.Parameters.Length)
            .ThenBy(c => string.Join(",", c.Parameters.Select(p => p.Type.ToDisplayString())), StringComparer.Ordinal)
            .ToList();

        if (ctors.Count == 1)
        {
            _ctorNames[ctors[0].OriginalDefinition] = "ctor";
        }
        else
        {
            // A record's positional primary constructor (declared on the record header) is the
            // "ctor"; otherwise the parameterless constructor is. (For a `record struct` the
            // implicit parameterless struct ctor must NOT usurp the positional primary.)
            var primary = ctors.FirstOrDefault(c => c.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is RecordDeclarationSyntax)
                          ?? ctors.FirstOrDefault(c => c.Parameters.Length == 0)
                          ?? ctors[0];
            var n = 1;
            foreach (var c in ctors)
            {
                _ctorNames[c.OriginalDefinition] = ReferenceEquals(c, primary) ? "ctor" : "$ctor" + n++;
            }
        }
        return _ctorNames.TryGetValue(ctor, out var name) ? name : "ctor";
    }

    private static bool IsRecordCopyCtor(IMethodSymbol c)
        => c.ContainingType.IsRecord && c.Parameters.Length == 1
           && SymbolEqualityComparer.Default.Equals(c.Parameters[0].Type, c.ContainingType);

    /// <summary>Ctor name honouring that external (h5) types expose only "ctor".</summary>
    private string ExternalAwareCtorName(IMethodSymbol ctor)
        => H5Naming.IsH5CompiledSource(ctor.ContainingType) ? CtorName(ctor) : "ctor";

    private static bool IsPrimaryCtorSyntax(IMethodSymbol ctor)
        => ctor.MethodKind == MethodKind.Constructor
           && ctor.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is TypeDeclarationSyntax { ParameterList: not null };

    /// <summary>The primary constructor of a non-record class/struct, or null.</summary>
    private static IMethodSymbol? PrimaryConstructor(INamedTypeSymbol type)
    {
        if (type.IsRecord) return null;
        return type.InstanceConstructors.FirstOrDefault(c =>
            c.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is TypeDeclarationSyntax { ParameterList: not null });
    }

    /// <summary>True for a primary-constructor parameter captured into instance state.</summary>
    private bool IsCapturedPrimaryCtorParam(IParameterSymbol param)
        => param.ContainingSymbol is IMethodSymbol { MethodKind: MethodKind.Constructor } ctor
           && ctor.ContainingType is { IsRecord: false } type
           && ctor.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is TypeDeclarationSyntax { ParameterList: not null }
           && CapturedPrimaryParamNames(type).Contains(param.Name);

    private readonly Dictionary<INamedTypeSymbol, HashSet<string>> _capturedParamCache = new(SymbolEqualityComparer.Default);

    /// <summary>
    /// Names of primary-ctor parameters used outside the constructor (in a method or
    /// property body) — these must be stored on the instance. Parameters used only in
    /// field initializers / base-args are consumed within the constructor.
    /// </summary>
    private HashSet<string> CapturedPrimaryParamNames(INamedTypeSymbol type)
    {
        if (_capturedParamCache.TryGetValue(type, out var cached)) return cached;
        var captured = new HashSet<string>();
        var primary = PrimaryConstructor(type);
        if (primary is null) { _capturedParamCache[type] = captured; return captured; }

        var paramNames = primary.Parameters.Select(p => p.Name).ToHashSet();
        foreach (var declRef in type.DeclaringSyntaxReferences)
        {
            if (declRef.GetSyntax() is not TypeDeclarationSyntax typeDecl) continue;
            foreach (var member in typeDecl.Members)
            {
                // Method / property / accessor / indexer bodies capture; field initializers do not.
                foreach (var id in member.DescendantNodes().OfType<IdentifierNameSyntax>())
                {
                    if (!paramNames.Contains(id.Identifier.Text)) continue;
                    if (_model.GetSymbolInfo(id).Symbol is IParameterSymbol p
                        && SymbolEqualityComparer.Default.Equals(p.ContainingSymbol, primary))
                        captured.Add(p.Name);
                }
            }
        }
        _capturedParamCache[type] = captured;
        return captured;
    }

    private void EmitInstanceCtors(INamedTypeSymbol type)
    {
        var ctors = type.InstanceConstructors
            .Where(c => (!c.IsImplicitlyDeclared || IsPrimaryCtorSyntax(c)) && c.DeclaringSyntaxReferences.Length > 0)
            .ToList();
        var hasExplicit = ctors.Count > 0;

        _w.Block(() =>
        {
            if (!hasExplicit)
            {
                // Synthesized default constructor.
                _w.Write("ctor: function () ");
                _w.Block(() =>
                {
                    _w.WriteLine("this.$initialize();");
                    EmitImplicitBaseCall(type);
                    EmitInstanceFieldInitializers(type);
                });
                _w.WriteLine();
                return;
            }

            var all = type.InstanceConstructors.Where(c => c.DeclaringSyntaxReferences.Length > 0).ToList();
            for (var i = 0; i < all.Count; i++)
            {
                var ctor = all[i];
                var syntax = ctor.DeclaringSyntaxReferences[0].GetSyntax();
                var decl = syntax as ConstructorDeclarationSyntax;
                var isPrimary = syntax is TypeDeclarationSyntax { ParameterList: not null };
                _w.Write($"{CtorName(ctor)}: function (");
                EmitParameterList(ctor);
                _w.Write(") ");
                _w.Block(() =>
                {
                    EmitOptionalDefaults(ctor);
                    _w.WriteLine("this.$initialize();");
                    if (isPrimary)
                    {
                        // Primary constructor: chain to base, store captured params, run field
                        // inits. Inside this body, param refs use the raw JS parameter name.
                        _inPrimaryCtorBody = true;
                        EmitPrimaryBaseCall(type, syntax as TypeDeclarationSyntax);
                        var captured = CapturedPrimaryParamNames(type);
                        foreach (var p in ctor.Parameters.Where(p => captured.Contains(p.Name)))
                            _w.WriteLine($"this.{NameMangler.JsIdentifier(p.Name)} = {NameMangler.JsIdentifier(p.Name)};");
                        EmitInstanceFieldInitializers(type);
                        _inPrimaryCtorBody = false;
                    }
                    else
                    {
                        EmitConstructorChain(ctor, decl!, type);
                        if (decl?.Body is not null)
                            EmitStatements(decl.Body.Statements);
                        else if (decl?.ExpressionBody is not null)
                            EmitExpressionStatement(decl.ExpressionBody.Expression);
                    }
                });
                _w.WriteLine(i < all.Count - 1 ? "," : "");
            }
        });
    }

    private void EmitConstructorChain(IMethodSymbol ctor, ConstructorDeclarationSyntax decl, INamedTypeSymbol type)
    {
        var initializer = decl?.Initializer;
        if (initializer is { RawKind: (int)SyntaxKind.ThisConstructorInitializer }
            && _model.GetSymbolInfo(initializer).Symbol is IMethodSymbol thisCtor)
        {
            _w.Write($"this.{CtorName(thisCtor)}(");
            EmitArguments(initializer.ArgumentList, thisCtor);
            _w.WriteLine(");");
            EmitInstanceFieldInitializers(type);
            return;
        }

        EmitInstanceFieldInitializers(type);

        if (initializer is { RawKind: (int)SyntaxKind.BaseConstructorInitializer }
            && _model.GetSymbolInfo(initializer).Symbol is IMethodSymbol baseCtor)
        {
            _w.Write($"{TypeRef(baseCtor.ContainingType)}.{ExternalAwareCtorName(baseCtor)}.call(this");
            if (initializer.ArgumentList.Arguments.Count > 0) { _w.Write(", "); EmitArguments(initializer.ArgumentList, baseCtor); }
            _w.WriteLine(");");
        }
        else
        {
            EmitImplicitBaseCall(type);
        }
    }

    /// <summary>Base-constructor call for a primary constructor (honours `: Base(args)`).</summary>
    private void EmitPrimaryBaseCall(INamedTypeSymbol type, TypeDeclarationSyntax? decl)
    {
        var primaryBase = decl?.BaseList?.Types.OfType<PrimaryConstructorBaseTypeSyntax>().FirstOrDefault();
        if (primaryBase is not null && _model.GetSymbolInfo(primaryBase).Symbol is IMethodSymbol baseCtor)
        {
            _w.Write($"{TypeRef(baseCtor.ContainingType)}.{ExternalAwareCtorName(baseCtor)}.call(this");
            if (primaryBase.ArgumentList.Arguments.Count > 0) { _w.Write(", "); EmitArguments(primaryBase.ArgumentList, baseCtor); }
            _w.WriteLine(");");
            return;
        }
        EmitImplicitBaseCall(type);
    }

    private void EmitImplicitBaseCall(INamedTypeSymbol type)
    {
        var baseType = type.BaseType;
        if (baseType is not null && baseType.SpecialType != SpecialType.System_Object
            && !IsValueTypeBase(baseType) && baseType.TypeKind != TypeKind.Error)
        {
            var baseCtor = baseType.InstanceConstructors.FirstOrDefault(c => c.Parameters.Length == 0);
            var name = baseCtor is not null ? ExternalAwareCtorName(baseCtor) : "ctor";
            _w.WriteLine($"{TypeRef(baseType)}.{name}.call(this);");
        }
    }

    private void EmitInstanceFieldInitializers(INamedTypeSymbol type)
    {
        foreach (var m in type.GetMembers())
        {
            if (m.IsStatic) continue;
            ExpressionSyntax? init = m switch
            {
                IFieldSymbol f when !f.IsConst && f.AssociatedSymbol is null => FieldInitializerSyntax(f),
                IPropertySymbol p when IsAutoProperty(p) => AutoPropertyInitializerSyntax(p),
                _ => null,
            };
            if (init is null) continue;
            _w.Write($"this.{H5Naming.MemberJsName(m)} = ");
            EmitExpression(init);
            _w.WriteLine(";");
        }
    }

    // ---- methods -----------------------------------------------------------

    private bool IsEmittableMethod(IMethodSymbol m)
        => m.MethodKind is MethodKind.Ordinary or MethodKind.UserDefinedOperator or MethodKind.Conversion
               or MethodKind.ExplicitInterfaceImplementation
           && !m.IsAbstract
           && m.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is BaseMethodDeclarationSyntax d
           && (d.Body is not null || d.ExpressionBody is not null);

    private bool IsEntryPoint(IMethodSymbol m)
        => SymbolEqualityComparer.Default.Equals(m, _compilation.GetEntryPoint(System.Threading.CancellationToken.None));

    private void EmitInstanceMethods(INamedTypeSymbol type, IMethodSymbol? entryPoint)
    {
        var entries = new List<Action>();

        foreach (var m in type.GetMembers().OfType<IMethodSymbol>().Where(m => !m.IsStatic && IsEmittableMethod(m)))
        {
            var method = m;
            entries.Add(() => EmitMethodEntry(method));
        }
        foreach (var indexer in type.GetMembers().OfType<IPropertySymbol>().Where(p => !p.IsStatic && p.IsIndexer && !p.IsAbstract))
        {
            var idx = indexer;
            if (idx.GetMethod is not null) entries.Add(() => EmitAccessorEntry("getItem", idx.GetMethod!, true));
            if (idx.SetMethod is not null) entries.Add(() => EmitAccessorEntry("setItem", idx.SetMethod!, false));
        }

        AddValueTypeMethodEntries(type, entries);

        if (entries.Count == 0) return;

        _w.Block(() =>
        {
            for (var i = 0; i < entries.Count; i++)
            {
                entries[i]();
                _w.WriteLine(i < entries.Count - 1 ? "," : "");
            }
        });
    }

    private void EmitAccessorEntry(string name, IMethodSymbol accessor, bool getter)
    {
        _w.Write($"{name}: function (");
        for (var p = 0; p < accessor.Parameters.Length; p++)
        {
            if (p > 0) _w.Write(", ");
            _w.Write(NameMangler.JsIdentifier(accessor.Parameters[p].Name));
        }
        _w.Write(") ");
        EmitAccessorBody(accessor, getter);
    }

    private void EmitMethodEntry(IMethodSymbol m)
    {
        var decl = (BaseMethodDeclarationSyntax)m.DeclaringSyntaxReferences[0].GetSyntax();
        _w.Write($"{H5Naming.MemberJsName(m)}: function (");
        EmitParameterList(m);
        _w.Write(") ");
        if (decl.Body is not null && IsIteratorBody(decl.Body))
            EmitIteratorBody(decl.Body, m);
        else
            EmitMethodBody(decl.Body, decl.ExpressionBody, m.ReturnsVoid, m);
    }

    private void EmitMethodMap(List<IMethodSymbol> methods, string ownerRef)
    {
        _w.Block(() =>
        {
            for (var i = 0; i < methods.Count; i++)
            {
                var m = methods[i];
                var decl = (BaseMethodDeclarationSyntax)m.DeclaringSyntaxReferences[0].GetSyntax();
                _w.Write($"{H5Naming.MemberJsName(m)}: function (");
                EmitParameterList(m);
                _w.Write(") ");
                if (decl.Body is not null && IsIteratorBody(decl.Body))
                    EmitIteratorBody(decl.Body, m);
                else
                    EmitMethodBody(decl.Body, decl.ExpressionBody, m.ReturnsVoid, m);
                _w.WriteLine(i < methods.Count - 1 ? "," : "");
            }
        });
    }

    private void EmitIteratorBody(BlockSyntax body, IMethodSymbol method)
    {
        _w.Block(() =>
        {
            EmitOptionalDefaults(method);
            // A generator function can't be an arrow, so it rebinds `this`; bind it to the
            // enclosing instance so an iterator body that reads `this.field` still works.
            _w.Write("return H5R.iter((function* () ");
            _w.Block(() => { foreach (var s in body.Statements) EmitStatement(s); });
            _w.WriteLine(").bind(this));");
        });
    }

    private static bool IsIteratorBody(SyntaxNode body)
    {
        foreach (var node in body.DescendantNodes(descendIntoChildren: n =>
                     n is not AnonymousFunctionExpressionSyntax and not LocalFunctionStatementSyntax))
        {
            if (node is YieldStatementSyntax) return true;
        }
        return false;
    }

    private void EmitEntryPoint(IMethodSymbol entry)
    {
        var decl = (BaseMethodDeclarationSyntax)entry.DeclaringSyntaxReferences[0].GetSyntax();
        var isAsync = entry.IsAsync || IsTaskType(entry.ReturnType);
        _w.Write("main: function Main () ");

        var moduleInits = ModuleInitializerMethods();
        if (moduleInits.Count == 0)
        {
            EmitMethodBody(decl.Body, decl.ExpressionBody, entry.ReturnsVoid, entry);
            return;
        }

        // Run [ModuleInitializer] methods before the entry point body (they execute at
        // module load in .NET; here we sequence them just ahead of Main).
        _w.Block(() =>
        {
            foreach (var mi in moduleInits)
                _w.WriteLine($"{TypeRef(mi.ContainingType)}.{H5Naming.MemberJsName(mi)}();");
            EmitOptionalDefaults(entry);
            EmitMaybeAsyncBody(isAsync, () =>
            {
                if (decl.Body is not null) EmitStatements(decl.Body.Statements);
                else if (decl.ExpressionBody is not null)
                {
                    if (entry.ReturnsVoid) EmitExpressionStatement(decl.ExpressionBody.Expression);
                    else { _w.Write("return "); EmitExpressionConverted(decl.ExpressionBody.Expression, entry.ReturnType); _w.WriteLine(";"); }
                }
            });
        });
    }

    private List<IMethodSymbol> ModuleInitializerMethods()
        => CollectTypes()
            .SelectMany(t => t.GetMembers().OfType<IMethodSymbol>())
            .Where(m => m.IsStatic && m.GetAttributes().Any(a =>
                a.AttributeClass?.ToDisplayString() == "System.Runtime.CompilerServices.ModuleInitializerAttribute"))
            .ToList();

    private void EmitParameterList(IMethodSymbol method)
    {
        var first = true;
        // A generic method that threads its type arguments receives them as leading
        // parameters (T, args…), so the body can use typeof(T)/default(T)/new T().
        if (ThreadsTypeArgs(method))
        {
            foreach (var tp in method.TypeParameters)
            {
                if (!first) _w.Write(", ");
                _w.Write(tp.Name);
                first = false;
            }
        }
        foreach (var p in method.Parameters)
        {
            if (!first) _w.Write(", ");
            _w.Write(NameMangler.JsIdentifier(p.Name));
            first = false;
        }
    }

    /// <summary>
    /// True if a generic method threads its type arguments at runtime: a source-defined
    /// generic method not marked [IgnoreGeneric]. Its definition takes the type parameters
    /// as leading arguments and every call site passes the concrete type arguments, so
    /// runtime uses of the type parameter (typeof(T), default(T), new T()) resolve.
    /// </summary>
    private static bool ThreadsTypeArgs(IMethodSymbol method)
    {
        // A generic method threads its type arguments as leading JS parameters when its H5-emitted
        // definition takes them — so the call site passes exactly what the definition expects.
        if (!method.IsGenericMethod) return false;
        var def = method.OriginalDefinition;
        if (def.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == "H5.IgnoreGenericAttribute")) return false;
        // A templated method's call shape is the template itself — no separate leading type args.
        if (H5Naming.GetTemplate(def) is not null) return false;
        // Source / referenced-library generic methods always thread. So do H5.dll BCL generic
        // methods that have a real body (a plain C# generic method, e.g.
        // CollectionExtensions.GetValueOrDefault / TryAdd — compiled with leading type parameters).
        // A body-less extern (hand-written JS, incl. the DOM externs in H5.Core like
        // querySelector<T>) uses its native form and must NOT thread.
        if (H5Naming.IsH5CompiledSource(method.ContainingType)) return true;
        return method.ContainingAssembly?.Name == "H5" && !H5Naming.HasNoBody(def);
    }

    private void EmitOptionalDefaults(IMethodSymbol method)
    {
        foreach (var p in method.Parameters)
        {
            var name = NameMangler.JsIdentifier(p.Name);
            if (p.HasExplicitDefaultValue)
            {
                _w.Write($"if ({name} === undefined) {{ {name} = ");
                _w.Write(ConstantLiteral(p.ExplicitDefaultValue, p.Type));
                _w.WriteLine("; }");
            }
            else if (p.IsParams)
            {
                // A params array invoked with no trailing arguments arrives as undefined at the JS
                // boundary (e.g. a reflection/JS caller, or an ExpandParams spread with none); default
                // it to an empty array so the body's enumeration/indexing behaves, matching H5.
                _w.WriteLine($"if ({name} === undefined) {{ {name} = []; }}");
            }
        }
    }

    private void EmitMethodBody(BlockSyntax? block, ArrowExpressionClauseSyntax? arrow, bool returnsVoid, IMethodSymbol method)
    {
        _w.Block(() =>
        {
            EmitOptionalDefaults(method);
            EmitMaybeAsyncBody(method.IsAsync, () =>
            {
                if (block is not null)
                {
                    EmitStatements(block.Statements);
                }
                else if (arrow is not null)
                {
                    // Hoist out-var / is-pattern variables the expression introduces (e.g.
                    // `=> TryParse(s, out var n) && n > 0`) so their write-backs and later reads
                    // resolve — an expression body has no statement to predeclare them otherwise.
                    PredeclareInlineVars(arrow.Expression);
                    if (returnsVoid) EmitExpressionStatement(arrow.Expression);
                    else { _w.Write("return "); EmitExpressionConverted(arrow.Expression, method.ReturnType); _w.WriteLine(";"); }
                }
            });
        });
    }

    /// <summary>
    /// Emits statements directly, or — for an async member — inside a native `async` IIFE
    /// whose promise is adapted to an h5.js Task via H5R.fromPromise. This gives async
    /// methods the same contract as h5.js's own state-machine output: they return a Task
    /// that composes with Task.Run/WhenAll/ContinueWith and carries faults through the Task.
    /// </summary>
    private void EmitMaybeAsyncBody(bool isAsync, Action emitStatements)
    {
        if (!isAsync) { emitStatements(); return; }
        _w.Write("return H5R.fromPromise((async () => ");
        _w.Block(emitStatements);
        _w.WriteLine(")());");
    }

    // ---- properties (with logic) -------------------------------------------

    private void EmitInstanceProperties(INamedTypeSymbol type)
    {
        var props = type.GetMembers().OfType<IPropertySymbol>()
            .Where(p => !p.IsStatic && !p.IsAbstract && !p.IsIndexer && !IsAutoProperty(p)
                        && !p.IsImplicitlyDeclared
                        && p.DeclaringSyntaxReferences.Length > 0
                        && p.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is PropertyDeclarationSyntax)
            .ToList();
        // Indexers → get_Item/set_Item under methods handled separately (skip for now).
        if (props.Count == 0) return;
        EmitPropertyMap(props);
    }

    private void EmitPropertyMap(List<IPropertySymbol> props)
    {
        _w.Block(() =>
        {
            for (var i = 0; i < props.Count; i++)
            {
                var p = props[i];
                _w.Write($"{H5Naming.MemberJsName(p)}: ");
                _w.Block(() =>
                {
                    if (p.GetMethod is not null)
                    {
                        _w.Write("get: function () ");
                        EmitAccessorBody(p.GetMethod, isGetter: true);
                        _w.WriteLine(p.SetMethod is not null ? "," : "");
                    }
                    if (p.SetMethod is not null)
                    {
                        _w.Write("set: function (value) ");
                        EmitAccessorBody(p.SetMethod, isGetter: false);
                        _w.WriteLine();
                    }
                });
                _w.WriteLine(i < props.Count - 1 ? "," : "");
            }
        });
    }

    /// <summary>
    /// A property that needs a compiler-synthesized backing field: it mixes an auto
    /// accessor with a bodied one, or an accessor uses the C# 14 `field` keyword.
    /// </summary>
    internal static bool IsFieldBackedProperty(IPropertySymbol p)
    {
        if (p.IsStatic || p.IsIndexer || p.IsAbstract) return false;
        if (p.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is not PropertyDeclarationSyntax { AccessorList: { } accessors })
            return false;
        var anyAuto = accessors.Accessors.Any(a => a.Body is null && a.ExpressionBody is null);
        var anyBodied = accessors.Accessors.Any(a => a.Body is not null || a.ExpressionBody is not null);
        if (anyAuto && anyBodied) return true;
        return accessors.Accessors.Any(a => a.DescendantNodes().Any(n => n.IsKind(SyntaxKind.FieldExpression)));
    }

    internal static string PropertyBackingName(IPropertySymbol p) => "$" + H5Naming.MemberJsName(p);

    private void EmitAccessorBody(IMethodSymbol accessor, bool isGetter)
    {
        // Field-backed property with an auto accessor → read/write the backing field.
        if (accessor.AssociatedSymbol is IPropertySymbol prop && IsFieldBackedProperty(prop))
        {
            var syn = accessor.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();
            var isAuto = syn is AccessorDeclarationSyntax { Body: null, ExpressionBody: null };
            if (isAuto)
            {
                var backing = PropertyBackingName(prop);
                _w.Block(() => _w.WriteLine(isGetter ? $"return this.{backing};" : $"this.{backing} = value;"));
                return;
            }
        }

        var syntax = accessor.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();
        switch (syntax)
        {
            case AccessorDeclarationSyntax { Body: { } body }:
                _w.Block(() => { foreach (var s in body.Statements) EmitStatement(s); });
                break;
            case AccessorDeclarationSyntax { ExpressionBody: { } arrow }:
            case ArrowExpressionClauseSyntax arrow2 when (arrow2 = (ArrowExpressionClauseSyntax)syntax) != null:
                var arrowExpr = (syntax as AccessorDeclarationSyntax)?.ExpressionBody?.Expression
                                ?? ((ArrowExpressionClauseSyntax)syntax).Expression;
                _w.Block(() =>
                {
                    if (isGetter) { _w.Write("return "); EmitExpression(arrowExpr); _w.WriteLine(";"); }
                    else EmitExpressionStatement(arrowExpr);
                });
                break;
            case PropertyDeclarationSyntax { ExpressionBody: { } arrow3 }:
                _w.Block(() => { _w.Write("return "); EmitExpression(arrow3.Expression); _w.WriteLine(";"); });
                break;
            default:
                _w.Block(() => { });
                break;
        }
    }
}
