using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace H5.Translator.Roslyn;

public sealed partial class Emitter
{
    // ---- invocation --------------------------------------------------------

    private void EmitInvocation(InvocationExpressionSyntax invocation)
    {
        // nameof(...) is a compile-time constant string.
        if (invocation.Expression is IdentifierNameSyntax { Identifier.Text: "nameof" }
            && _model.GetConstantValue(invocation) is { HasValue: true, Value: string nameofValue })
        {
            _w.Write(JsString(nameofValue));
            return;
        }

        var symbol = _model.GetSymbolInfo(invocation).Symbol as IMethodSymbol;

        if (symbol is null)
        {
            EmitExpression(invocation.Expression);
            _w.Write("(");
            EmitArguments(invocation.ArgumentList, null);
            _w.Write(")");
            return;
        }

        // Inside a null-conditional continuation, an invocation whose target is a member
        // binding (a?.M(...)) resolves M against the captured receiver.
        var condRecv = invocation.Expression is MemberBindingExpressionSyntax && _condReceiver is not null
            ? _condReceiver : null;

        // Delegate invocation: d(...) or d.Invoke(...) — the delegate is a plain callable.
        if (symbol.MethodKind == MethodKind.DelegateInvoke)
        {
            // For d.Invoke(...) call the receiver directly, dropping the ".Invoke".
            if (condRecv is not null)
                _w.Write(condRecv);
            else if (invocation.Expression is MemberAccessExpressionSyntax { Name.Identifier.Text: "Invoke" } dm)
                EmitExpression(dm.Expression);
            else
                EmitExpression(invocation.Expression);
            _w.Write("(");
            EmitArguments(invocation.ArgumentList, symbol);
            _w.Write(")");
            return;
        }

        // Local function call → bare name.
        if (symbol.MethodKind == MethodKind.LocalFunction)
        {
            _w.Write(NameMangler.JsIdentifier(symbol.Name));
            _w.Write("(");
            EmitArguments(invocation.ArgumentList, symbol);
            _w.Write(")");
            return;
        }

        // enum.ToString() → System.Enum.toString(EnumType, value)
        if (symbol is { Name: "ToString", Parameters.Length: 0 }
            && invocation.Expression is MemberAccessExpressionSyntax { Expression: { } enumRecv }
            && _model.GetTypeInfo(enumRecv).Type is { TypeKind: TypeKind.Enum } enumType)
        {
            _w.Write($"System.Enum.toString({TypeRef(enumType)}, ");
            EmitExpression(enumRecv);
            _w.Write(")");
            return;
        }

        // H5.Script.Write(code, args) — inject raw JavaScript, substituting {0},{1}… with args.
        if (symbol is { Name: "Write" } && symbol.ContainingType?.ToDisplayString() == "H5.Script"
            && invocation.ArgumentList.Arguments.Count >= 1
            && _model.GetConstantValue(invocation.ArgumentList.Arguments[0].Expression).Value is string rawJs)
        {
            var argJs = invocation.ArgumentList.Arguments.Skip(1)
                .Select(a => Capture(() => EmitExpression(a.Expression))).ToList();
            _w.Write(SubstituteTemplate(rawJs, null, new(), argJs));
            return;
        }

        var origin = symbol.OriginalDefinition;
        var template = H5Naming.GetTemplate(origin) ?? H5Naming.GetTemplate(symbol);

        // by-ref args (no template): holder objects with write-back.
        if (template is null && HasByRefArguments(invocation.ArgumentList, symbol))
        {
            EmitByRefInvocation(invocation, symbol);
            return;
        }

        var isBase = invocation.Expression is MemberAccessExpressionSyntax { Expression: BaseExpressionSyntax };
        var receiverExpr = invocation.Expression is MemberAccessExpressionSyntax ma && !isBase ? ma.Expression : null;

        if (template is not null && HasByRefArguments(invocation.ArgumentList, symbol))
        {
            EmitByRefTemplateInvocation(invocation, symbol, template);
            return;
        }

        if (template is not null)
        {
            var (byName, byPos) = CaptureArguments(invocation.ArgumentList, symbol);
            var receiver = symbol.IsStatic && !symbol.IsExtensionMethod ? null
                : receiverExpr is not null ? Capture(() => EmitExpression(receiverExpr))
                : condRecv ?? "this";

            // Reduced extension method: the receiver binds to the original first
            // parameter (e.g. {source}); actual args map to the remaining params.
            if (symbol is { IsExtensionMethod: true, ReducedFrom: { } reduced } && receiver is not null)
            {
                var rebuilt = new Dictionary<string, string>();
                rebuilt[reduced.Parameters[0].Name] = receiver;
                for (var i = 0; i < byPos.Count && i + 1 < reduced.Parameters.Length; i++)
                    rebuilt[reduced.Parameters[i + 1].Name] = byPos[i];
                byName = rebuilt;
                byPos = new List<string> { receiver }.Concat(byPos).ToList();
                AddTypeArguments(byName, reduced, symbol);
            }
            else
            {
                AddTypeArguments(byName, symbol.OriginalDefinition, symbol);
            }

            WriteTemplate(template, symbol.IsStatic, symbol.IsExtensionMethod, receiver, byName, byPos);
            return;
        }

        // base.Method(...) → Base.prototype.Method.call(this, args) for instance methods
        // (instance methods live on the prototype in the H5 runtime); statics on the type.
        if (isBase)
        {
            var baseAccess = symbol.IsStatic ? "" : ".prototype";
            _w.Write($"{TypeRef(symbol.ContainingType)}{baseAccess}.{H5Naming.MemberJsName(symbol)}.call(this");
            if (invocation.ArgumentList.Arguments.Count > 0) { _w.Write(", "); EmitArguments(invocation.ArgumentList, symbol); }
            _w.Write(")");
            return;
        }

        // Extension method (no template) → StaticType.Method([typeArgs, ] receiver, args).
        // The static signature is Method(T…, this-param, params…): a generic extension threads
        // its *inferred* type arguments (from the constructed symbol) as the leading arguments,
        // then the receiver, then the remaining arguments mapped through the reduced symbol
        // (whose parameters already exclude `this` and preserve params-array collection).
        if (symbol is { IsExtensionMethod: true, ReducedFrom: { } reducedFrom } && (receiverExpr is not null || condRecv is not null))
        {
            _w.Write($"{TypeRef(symbol.ContainingType)}.{H5Naming.MemberJsName(reducedFrom)}(");
            var lead = EmitLeadingTypeArgs(symbol);
            if (lead) _w.Write(", ");
            if (receiverExpr is not null) EmitExpression(receiverExpr); else _w.Write(condRecv!);
            if (invocation.ArgumentList.Arguments.Count > 0)
            {
                _w.Write(", ");
                EmitArguments(invocation.ArgumentList, symbol, threadTypeArgs: false);
            }
            _w.Write(")");
            return;
        }

        // Ordinary call.
        if (symbol.IsStatic)
        {
            _w.Write(StaticMemberAccess(symbol));
        }
        else if (receiverExpr is not null)
        {
            EmitExpression(receiverExpr);
            _w.Write($".{H5Naming.MemberJsName(symbol)}");
        }
        else if (condRecv is not null)
        {
            _w.Write($"{condRecv}.{H5Naming.MemberJsName(symbol)}");
        }
        else
        {
            _w.Write($"this.{H5Naming.MemberJsName(symbol)}");
        }
        _w.Write("(");
        EmitArguments(invocation.ArgumentList, symbol);
        _w.Write(")");
    }

    /// <summary>Captures each argument's JS, keyed by parameter name and by position.</summary>
    private (Dictionary<string, string> byName, List<string> byPos) CaptureArguments(ArgumentListSyntax argList, IMethodSymbol method)
    {
        var byName = new Dictionary<string, string>();
        var byPos = new List<string>();
        var args = argList.Arguments;

        for (var i = 0; i < args.Count; i++)
        {
            var pType = i < method.Parameters.Length ? method.Parameters[i].Type : null;
            var idx = i;
            byPos.Add(Capture(() => EmitExpressionConverted(args[idx].Expression, pType)));
        }

        for (var pi = 0; pi < method.Parameters.Length; pi++)
        {
            var p = method.Parameters[pi];
            if (p.IsParams)
            {
                byName[p.Name] = "[" + string.Join(", ", byPos.Skip(pi)) + "]";
            }
            else if (pi < byPos.Count)
            {
                byName[p.Name] = byPos[pi];
            }
        }
        return (byName, byPos);
    }

    /// <summary>Adds generic type-parameter bindings (e.g. {TSource} → System.Int32) for templates.</summary>
    private void AddTypeArguments(Dictionary<string, string> byName, IMethodSymbol definition, IMethodSymbol constructed)
    {
        for (var i = 0; i < definition.TypeParameters.Length && i < constructed.TypeArguments.Length; i++)
        {
            byName[definition.TypeParameters[i].Name] = TypeRef(constructed.TypeArguments[i]);
            byName[definition.TypeParameters[i].Name + ":default"] = DefaultValueLiteral(constructed.TypeArguments[i]);
        }

        var defType = definition.ContainingType;
        var conType = constructed.ContainingType;
        if (defType is not null && conType is not null)
        {
            for (var i = 0; i < defType.TypeParameters.Length && i < conType.TypeArguments.Length; i++)
            {
                byName[defType.TypeParameters[i].Name] = TypeRef(conType.TypeArguments[i]);
                byName[defType.TypeParameters[i].Name + ":default"] = DefaultValueLiteral(conType.TypeArguments[i]);
            }
        }
    }

    private bool HasByRefArguments(ArgumentListSyntax argList, IMethodSymbol symbol)
    {
        foreach (var arg in argList.Arguments)
        {
            if (arg.RefKindKeyword.RawKind != 0
                && (arg.RefKindKeyword.IsKind(SyntaxKind.OutKeyword) || arg.RefKindKeyword.IsKind(SyntaxKind.RefKeyword)))
            {
                return true;
            }
        }
        return false;
    }

    private void EmitCallee(InvocationExpressionSyntax invocation, IMethodSymbol symbol)
    {
        if (symbol.IsStatic)
        {
            _w.Write(StaticMemberAccess(symbol));
        }
        else if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
        {
            EmitExpression(memberAccess.Expression);
            _w.Write($".{H5Naming.MemberJsName(symbol)}");
        }
        else
        {
            _w.Write($"this.{H5Naming.MemberJsName(symbol)}");
        }
    }

    /// <summary>Template call with out/ref args → holder objects + write-back.</summary>
    private void EmitByRefTemplateInvocation(InvocationExpressionSyntax invocation, IMethodSymbol symbol, string template)
    {
        var args = invocation.ArgumentList.Arguments;
        var holders = new string?[args.Count];
        var byName = new Dictionary<string, string>();
        var byPos = new List<string>();

        // Arrow (not `function`) so a `this`-qualified receiver inside the call resolves to the
        // enclosing instance rather than being rebound to undefined in strict mode.
        _w.Write("(() => { ");
        for (var i = 0; i < args.Count; i++)
        {
            var isRef = args[i].RefKindKeyword.IsKind(SyntaxKind.OutKeyword) || args[i].RefKindKeyword.IsKind(SyntaxKind.RefKeyword);
            string val;
            if (isRef)
            {
                var holder = "$ref" + i;
                holders[i] = holder;
                var t = i < symbol.Parameters.Length ? symbol.Parameters[i].Type : null;
                var seed = args[i].RefKindKeyword.IsKind(SyntaxKind.OutKeyword) ? (t is not null ? DefaultValueLiteral(t) : "null") : Capture(() => EmitExpression(args[i].Expression));
                _w.Write($"var {holder} = {{ v: {seed} }}; ");
                val = holder;
            }
            else
            {
                var pType = i < symbol.Parameters.Length ? symbol.Parameters[i].Type : null;
                var idx = i;
                val = Capture(() => EmitExpressionConverted(args[idx].Expression, pType));
            }
            byPos.Add(val);
            if (i < symbol.Parameters.Length) byName[symbol.Parameters[i].Name] = val;
        }

        // Bind generic type parameters referenced by the template (e.g. Enum.TryParse<TEnum>).
        AddTypeArguments(byName, symbol.OriginalDefinition, symbol);

        // Skip write-back for discard targets (out _).
        for (var i = 0; i < args.Count; i++)
            if (holders[i] is not null && IsDiscardTarget(args[i].Expression)) holders[i] = null;

        _w.Write("var $ret = ");
        _w.Write(SubstituteTemplate(template, symbol.IsStatic ? null : "this", byName, byPos));
        _w.Write("; ");

        for (var i = 0; i < args.Count; i++)
        {
            if (holders[i] is null) continue;
            EmitByRefWriteBackTarget(args[i].Expression);
            _w.Write($" = {holders[i]}.v; ");
        }
        _w.Write("return $ret; })()");
    }

    private void EmitByRefInvocation(InvocationExpressionSyntax invocation, IMethodSymbol symbol)
    {
        var args = invocation.ArgumentList.Arguments;
        var holders = new string?[args.Count];

        // Arrow (not `function`) so a `this`-qualified receiver inside the call resolves to the
        // enclosing instance rather than being rebound to undefined in strict mode.
        _w.Write("(() => { ");

        for (var i = 0; i < args.Count; i++)
        {
            var arg = args[i];
            var isOut = arg.RefKindKeyword.IsKind(SyntaxKind.OutKeyword);
            var isRef = arg.RefKindKeyword.IsKind(SyntaxKind.RefKeyword);
            if (!isOut && !isRef) continue;

            var holder = "$ref" + i;
            holders[i] = holder;
            _w.Write($"var {holder} = {{ v: ");
            if (isOut)
            {
                var t = i < symbol.Parameters.Length ? symbol.Parameters[i].Type : null;
                _w.Write(t is not null ? DefaultValueLiteral(t) : "null");
            }
            else
            {
                EmitExpression(arg.Expression); // ref: seed with current value
            }
            _w.Write(" }; ");
        }

        _w.Write("var $ret = ");
        // An extension method with a by-ref/out parameter (e.g. IComponent.Var<T>(this T, out T))
        // is still a static call: emit ExtClass.Method(typeArgs, receiver, args…), not
        // receiver.Method(args…) — the receiver is the reduced method's first argument.
        var reduced = symbol.IsExtensionMethod ? symbol.ReducedFrom : null;
        var extReceiver = reduced is not null && invocation.Expression is MemberAccessExpressionSyntax ma ? ma.Expression : null;
        if (extReceiver is not null)
            _w.Write($"{TypeRef(symbol.ContainingType)}.{H5Naming.MemberJsName(reduced!)}");
        else
            EmitCallee(invocation, symbol);
        _w.Write("(");
        var lead = EmitLeadingTypeArgs(symbol);
        var first = !lead;
        if (extReceiver is not null)
        {
            if (!first) _w.Write(", ");
            EmitExpression(extReceiver);
            first = false;
        }
        for (var i = 0; i < args.Count; i++)
        {
            if (!first) _w.Write(", ");
            first = false;
            if (holders[i] is not null) _w.Write(holders[i]!);
            else EmitExpressionConverted(args[i].Expression, i < symbol.Parameters.Length ? symbol.Parameters[i].Type : null);
        }
        _w.Write("); ");

        // Write back out/ref values to their targets (skip discards: out _).
        for (var i = 0; i < args.Count; i++)
        {
            if (holders[i] is null) continue;
            if (IsDiscardTarget(args[i].Expression)) continue;
            EmitByRefWriteBackTarget(args[i].Expression);
            _w.Write($" = {holders[i]}.v; ");
        }

        _w.Write("return $ret; })()");
    }

    /// <summary>True for a discard target (out _ or a discard designation).</summary>
    private bool IsDiscardTarget(ExpressionSyntax expr)
        => expr is DeclarationExpressionSyntax { Designation: DiscardDesignationSyntax }
           || _model.GetSymbolInfo(expr).Symbol is IDiscardSymbol
           || (expr is IdentifierNameSyntax { Identifier.Text: "_" } && _model.GetSymbolInfo(expr).Symbol is null);

    private void EmitByRefWriteBackTarget(ExpressionSyntax expr)
    {
        if (expr is DeclarationExpressionSyntax { Designation: SingleVariableDesignationSyntax decl })
        {
            _w.Write(NameMangler.JsIdentifier(decl.Identifier.Text));
        }
        else
        {
            EmitExpression(expr);
        }
    }

    /// <summary>
    /// Emits a generic method's type arguments as leading call arguments (matching the
    /// leading type parameters in its definition). Returns true if anything was written.
    /// </summary>
    private bool EmitLeadingTypeArgs(IMethodSymbol? method)
    {
        if (method is null || !ThreadsTypeArgs(method) || method.TypeArguments.Length == 0) return false;
        for (var i = 0; i < method.TypeArguments.Length; i++)
        {
            if (i > 0) _w.Write(", ");
            _w.Write(TypeRef(method.TypeArguments[i]));
        }
        return true;
    }

    private void EmitArguments(ArgumentListSyntax argList, IMethodSymbol? method, bool threadTypeArgs = true)
    {
        var args = argList.Arguments;
        var lead = threadTypeArgs && EmitLeadingTypeArgs(method);

        // Reorder named arguments to parameter order when we know the method.
        if (method is not null && args.Any(a => a.NameColon is not null))
        {
            var ordered = new ExpressionSyntax?[method.Parameters.Length];
            var positional = 0;
            foreach (var arg in args)
            {
                if (arg.NameColon is not null)
                {
                    var idx = method.Parameters.ToList().FindIndex(p => p.Name == arg.NameColon.Name.Identifier.Text);
                    if (idx >= 0) ordered[idx] = arg.Expression;
                }
                else
                {
                    ordered[positional++] = arg.Expression;
                }
            }

            // Positional JS calls can't skip a hole, so fill any omitted argument that
            // precedes a provided one with its parameter's default value. Trailing
            // omitted optionals are left off (the callee supplies its own defaults).
            var lastProvided = -1;
            for (var i = 0; i < ordered.Length; i++) if (ordered[i] is not null) lastProvided = i;

            var first = !lead;
            for (var i = 0; i <= lastProvided; i++)
            {
                if (!first) _w.Write(", ");
                first = false;
                if (ordered[i] is not null)
                    EmitExpressionConverted(ordered[i]!, method.Parameters[i].Type);
                else if (method.Parameters[i].HasExplicitDefaultValue)
                    _w.Write(ConstantLiteral(method.Parameters[i].ExplicitDefaultValue, method.Parameters[i].Type));
                else
                    _w.Write("null");
            }
            return;
        }

        // [ExpandParams]: a native variadic function (e.g. DOMTokenList.add(params string[])).
        // Its trailing args are spread as individual arguments (add("a","b")), and a single array
        // argument is spread with JS spread (add(...arr)) so it reaches the native function as
        // separate tokens. Without this the params would be passed as ONE array argument —
        // classList.add(["a","b"]) coerces to the single malformed token "a,b".
        if (method is { Parameters.Length: > 0 } && method.Parameters[^1].IsParams && HasExpandParams(method))
        {
            var fixedCount = method.Parameters.Length - 1;
            var first = !lead;
            for (var i = 0; i < fixedCount && i < args.Count; i++)
            {
                if (!first) _w.Write(", ");
                first = false;
                EmitExpressionConverted(args[i].Expression, method.Parameters[i].Type);
            }
            var trailing = args.Skip(fixedCount).ToList();
            var elem = (method.Parameters[^1].Type as IArrayTypeSymbol)?.ElementType;
            if (trailing.Count == 1 && _model.GetTypeInfo(trailing[0].Expression).Type is IArrayTypeSymbol)
            {
                if (!first) _w.Write(", ");
                _w.Write("...");
                EmitExpression(trailing[0].Expression);
            }
            else
            {
                for (var i = 0; i < trailing.Count; i++)
                {
                    if (!first) _w.Write(", ");
                    first = false;
                    EmitExpressionConverted(trailing[i].Expression, elem);
                }
            }
            return;
        }

        // params array handling: collect trailing args into a JS array (except for
        // the variadic BCL formatters, whose runtime functions take individual args).
        if (method is { Parameters.Length: > 0 } && method.Parameters[^1].IsParams && ShouldWrapParams(method))
        {
            var fixedCount = method.Parameters.Length - 1;
            var first = !lead;
            for (var i = 0; i < fixedCount && i < args.Count; i++)
            {
                if (!first) _w.Write(", ");
                first = false;
                EmitExpressionConverted(args[i].Expression, method.Parameters[i].Type);
            }

            var trailing = args.Skip(fixedCount).ToList();
            if (!first) _w.Write(", ");

            var paramsArrayType = (method.Parameters[^1].Type as IArrayTypeSymbol)?.ElementType;
            // A single array argument is passed through as the params array itself.
            if (trailing.Count == 1 && _model.GetTypeInfo(trailing[0].Expression).Type is IArrayTypeSymbol)
            {
                EmitExpression(trailing[0].Expression);
            }
            else
            {
                _w.Write("[");
                for (var i = 0; i < trailing.Count; i++)
                {
                    if (i > 0) _w.Write(", ");
                    EmitExpressionConverted(trailing[i].Expression, paramsArrayType);
                }
                _w.Write("]");
            }
            return;
        }

        for (var i = 0; i < args.Count; i++)
        {
            if (i > 0 || lead) _w.Write(", ");
            var targetType = method is not null && i < method.Parameters.Length ? method.Parameters[i].Type : null;
            EmitExpressionConverted(args[i].Expression, targetType);
        }
    }

    /// <summary>
    /// Whether a params argument should be wrapped into a JS array. The variadic BCL
    /// formatters (Console.Write/WriteLine, String.Format/Concat) take individual
    /// arguments in the runtime, so they are excluded.
    /// </summary>
    /// <summary>A method whose <c>params</c> array must be expanded (spread) at the call site,
    /// per H5's <c>[ExpandParams]</c> — the native variadic DOM/JS functions.</summary>
    private static bool HasExpandParams(IMethodSymbol method)
        => method.OriginalDefinition.GetAttributes()
            .Any(a => a.AttributeClass?.ToDisplayString() == "H5.ExpandParamsAttribute");

    private static bool ShouldWrapParams(IMethodSymbol method)
    {
        var containing = method.ContainingType?.ToDisplayString();
        var name = method.Name;
        if (containing == "System.Console" && name is "Write" or "WriteLine") return false;
        if (containing == "System.String" && name is "Format" or "Concat") return false;
        return true;
    }

    private void EmitArgumentList(ArgumentListSyntax argList) => EmitArguments(argList, null);
    private void EmitArgumentList(BracketedArgumentListSyntax argList)
    {
        for (var i = 0; i < argList.Arguments.Count; i++)
        {
            if (i > 0) _w.Write(", ");
            EmitExpression(argList.Arguments[i].Expression);
        }
    }

    // ---- object creation ---------------------------------------------------

    private void EmitObjectCreation(ObjectCreationExpressionSyntax creation)
    {
        // new T() on a generic type parameter (new() constraint) → runtime instantiation. Both a
        // *type* parameter (threaded via the generic type's defining function) and a *method*
        // parameter (threaded as a leading JS argument of the generic method — the new() constraint
        // guarantees the method threads T) are in scope as the identifier tp.Name at runtime.
        if (_model.GetTypeInfo(creation).Type is ITypeParameterSymbol tp)
        {
            _w.Write($"H5.createInstance({tp.Name})");
            return;
        }
        var symbol = _model.GetSymbolInfo(creation).Symbol as IMethodSymbol;
        var type = _model.GetTypeInfo(creation).Type as INamedTypeSymbol ?? symbol?.ContainingType;
        EmitConstructionCore(type, symbol, creation.ArgumentList, creation.Initializer);
    }

    private void EmitImplicitObjectCreation(ImplicitObjectCreationExpressionSyntax creation)
    {
        var symbol = _model.GetSymbolInfo(creation).Symbol as IMethodSymbol;
        var type = _model.GetTypeInfo(creation).Type as INamedTypeSymbol ?? symbol?.ContainingType;
        EmitConstructionCore(type, symbol, creation.ArgumentList, creation.Initializer);
    }

    private void EmitConstructionCore(INamedTypeSymbol? type, IMethodSymbol? ctor, ArgumentListSyntax? argList, InitializerExpressionSyntax? initializer)
    {
        if (initializer is { Expressions.Count: > 0 })
        {
            _w.Write("(() => { var $o = ");
            EmitBareConstruction(type, ctor, argList);
            _w.Write("; ");
            EmitInitializer("$o", initializer);
            _w.Write("return $o; })()");
            return;
        }

        EmitBareConstruction(type, ctor, argList);
    }

    private void EmitBareConstruction(INamedTypeSymbol? type, IMethodSymbol? ctor, ArgumentListSyntax? argList)
    {
        if (type is null)
        {
            _w.Write("{}");
            return;
        }

        // A constructor [Template] defines how `new T(...)` is built — e.g. a DOM element's
        // ctor maps to document.createElement("span") instead of an (illegal) native `new`.
        if (ctor is not null && H5Naming.GetTemplate(ctor.OriginalDefinition) is { } ctorTemplate)
        {
            var (byName, byPos) = argList is not null ? CaptureArguments(argList, ctor) : (new(), new List<string>());
            WriteTemplate(ctorTemplate, isStatic: true, isExtension: false, receiver: null, byName, byPos, TemplateTypeArgs(ctor));
            return;
        }

        // Delegate construction: new Func<...>(target) → target
        if (type.TypeKind == TypeKind.Delegate)
        {
            if (argList is { Arguments.Count: 1 }) EmitExpression(argList.Arguments[0].Expression);
            else _w.Write("null");
            return;
        }

        // `new object()` is a plain empty JS object ({}), not an H5 System.Object instance — matching
        // the legacy compiler. Code commonly uses it as a dynamic property bag whose own keys are
        // iterated (e.g. a Baklava node's inputs/outputs map); an H5 instance would carry prototype
        // members that pollute that iteration.
        if (type.SpecialType == SpecialType.System_Object)
        {
            _w.Write("{ }");
            return;
        }

        // [ObjectLiteral] type → a plain JS object ({}); the initializer sets its members.
        if (type.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == "H5.ObjectLiteralAttribute"))
        {
            _w.Write("{}");
            return;
        }

        // Constructor [Template] (some BCL types).
        var template = ctor is not null ? H5Naming.GetTemplate(ctor.OriginalDefinition) : null;
        if (template is not null && argList is not null)
        {
            var (byName, byPos) = CaptureArguments(argList, ctor!);
            _w.Write(SubstituteTemplate(template, null, byName, byPos));
            return;
        }

        var ctorName = ctor is not null ? CtorName(ctor) : "ctor";
        var typeRef = TypeRef(type);
        // A generic instantiation like Factory$1(Item) must be parenthesized before `new`.
        var newTarget = typeRef.Contains('(') ? $"({typeRef})" : typeRef;

        if (type.Locations.Any(l => l.IsInSource))
        {
            // User type: new Type(args) for the primary ctor, new Type.$ctorN(args) otherwise.
            _w.Write(ctorName == "ctor" ? $"new {newTarget}(" : $"new {newTarget}.{ctorName}(");
            if (argList is not null) EmitArguments(argList, ctor);
            _w.Write(")");
        }
        else if (type.ToDisplayString() == "System.Exception" || H5Naming.IsExternalType(type))
        {
            // External / ambient-JS types (StringBuilder, Exception, DOM globals like
            // MutationObserver, …) map to a native constructor that dispatches on arguments.
            _w.Write($"new {typeRef}(");
            if (argList is not null) EmitArguments(argList, ctor);
            _w.Write(")");
        }
        else
        {
            // H5-generated BCL type: new (TypeRef).ctorName(args) — named-constructor form.
            _w.Write($"new ({typeRef}).{ctorName}(");
            if (argList is not null) EmitArguments(argList, ctor);
            _w.Write(")");
        }
    }

    private void EmitInitializer(string target, InitializerExpressionSyntax initializer)
    {
        foreach (var expr in initializer.Expressions)
        {
            switch (expr)
            {
                // Object initializer member: X = value
                case AssignmentExpressionSyntax { Left: IdentifierNameSyntax name } assign:
                    var memberSym = _model.GetSymbolInfo(name).Symbol;
                    var memberName = memberSym is not null ? H5Naming.MemberJsName(memberSym) : NameMangler.JsIdentifier(name.Identifier.Text);
                    _w.Write($"{target}.{memberName} = ");
                    EmitExpression(assign.Right);
                    _w.Write("; ");
                    break;
                // Index initializer: [key] = value
                case AssignmentExpressionSyntax { Left: ImplicitElementAccessSyntax idx } assign:
                    _w.Write($"{target}.setItem(");
                    EmitArgumentList(idx.ArgumentList);
                    _w.Write(", ");
                    EmitExpression(assign.Right);
                    _w.Write("); ");
                    break;
                // Collection element with multiple values: { k, v }  (e.g. dictionary)
                case InitializerExpressionSyntax nested:
                    _w.Write($"{target}.{AddMethodName(nested)}(");
                    for (var i = 0; i < nested.Expressions.Count; i++)
                    {
                        if (i > 0) _w.Write(", ");
                        EmitExpression(nested.Expressions[i]);
                    }
                    _w.Write("); ");
                    break;
                // Collection element: single value
                default:
                    _w.Write($"{target}.{AddMethodName(expr)}(");
                    EmitExpression(expr);
                    _w.Write("); ");
                    break;
            }
        }
    }

    /// <summary>Resolves the JS name of the Add method a collection initializer element binds to.</summary>
    private string AddMethodName(ExpressionSyntax element)
    {
        if (_model.GetCollectionInitializerSymbolInfo(element).Symbol is IMethodSymbol add)
            return H5Naming.MemberJsName(add);
        return "add";
    }

    // ---- tuples ------------------------------------------------------------

    private void EmitTuple(TupleExpressionSyntax tuple)
    {
        // Tuples are represented as { Item1, Item2, ... }; named-element access is
        // mapped to the corresponding ItemN at the access site.
        _w.Write("{ ");
        for (var i = 0; i < tuple.Arguments.Count; i++)
        {
            if (i > 0) _w.Write(", ");
            _w.Write($"Item{i + 1}: ");
            EmitExpression(tuple.Arguments[i].Expression);
        }
        _w.Write(" }");
    }

    private void EmitAnonymousObject(AnonymousObjectCreationExpressionSyntax anon)
    {
        _w.Write("{ ");
        for (var i = 0; i < anon.Initializers.Count; i++)
        {
            if (i > 0) _w.Write(", ");
            var init = anon.Initializers[i];
            // Member name: explicit (Name = expr) or inferred from the expression.
            var name = init.NameEquals?.Name.Identifier.Text
                ?? (init.Expression as MemberAccessExpressionSyntax)?.Name.Identifier.Text
                ?? (init.Expression as IdentifierNameSyntax)?.Identifier.Text
                ?? $"Item{i + 1}";
            _w.Write($"{NameMangler.JsIdentifier(name)}: ");
            EmitExpression(init.Expression);
        }
        _w.Write(" }");
    }

    // ---- binary / unary / assignment --------------------------------------

    private void EmitBinary(BinaryExpressionSyntax binary)
    {
        var op = binary.OperatorToken.Text;

        var leftType = _model.GetTypeInfo(binary.Left).ConvertedType ?? _model.GetTypeInfo(binary.Left).Type;
        var rightType = _model.GetTypeInfo(binary.Right).ConvertedType ?? _model.GetTypeInfo(binary.Right).Type;
        var resultType = _model.GetTypeInfo(binary).Type;

        // User-defined operator overloads → static op_ method call.
        // (Records synthesize op_Equality/op_Inequality; those are implicitly declared
        // and handled by the value-equality path below, so exclude them here.)
        if (_model.GetSymbolInfo(binary).Symbol is IMethodSymbol { MethodKind: MethodKind.UserDefinedOperator, IsImplicitlyDeclared: false } opMethod
            && opMethod.Locations.Any(l => l.IsInSource))
        {
            _w.Write($"{TypeRef(opMethod.ContainingType)}.{H5Naming.MemberJsName(opMethod)}(");
            EmitExpression(binary.Left);
            _w.Write(", ");
            EmitExpression(binary.Right);
            _w.Write(")");
            return;
        }

        // is / as
        if (binary.IsKind(SyntaxKind.IsExpression))
        {
            var t = _model.GetTypeInfo(binary.Right).Type;
            _w.Write("H5R.is(");
            EmitExpression(binary.Left);
            _w.Write($", {TypeRef(t!)})");
            return;
        }
        if (binary.IsKind(SyntaxKind.AsExpression))
        {
            var t = _model.GetTypeInfo(binary.Right).Type;
            // `x as dynamic` is an identity in JS — member access on the result resolves
            // dynamically against the value itself, so there's no runtime type to check.
            if (t is null || t.TypeKind == TypeKind.Dynamic)
            {
                EmitExpression(binary.Left);
                return;
            }
            _w.Write("H5R.as(");
            EmitExpression(binary.Left);
            _w.Write($", {TypeRef(t)})");
            return;
        }

        // DateTime / TimeSpan arithmetic.
        var lName = leftType?.ToDisplayString();
        var rName = rightType?.ToDisplayString();
        if ((lName is "System.DateTime" or "System.TimeSpan") && (binary.IsKind(SyntaxKind.AddExpression) || binary.IsKind(SyntaxKind.SubtractExpression)))
        {
            var helper = (lName, rName, sub: binary.IsKind(SyntaxKind.SubtractExpression)) switch
            {
                ("System.DateTime", "System.DateTime", true) => "H5R.dtSub",
                ("System.DateTime", "System.TimeSpan", true) => "H5R.dtSubTs",
                ("System.DateTime", "System.TimeSpan", false) => "H5R.dtAddTs",
                ("System.TimeSpan", "System.TimeSpan", true) => "H5R.tsSub",
                ("System.TimeSpan", "System.TimeSpan", false) => "H5R.tsAdd",
                _ => null,
            };
            if (helper is not null)
            {
                _w.Write($"{helper}(");
                EmitExpression(binary.Left);
                _w.Write(", ");
                EmitExpression(binary.Right);
                _w.Write(")");
                return;
            }
        }

        // String concatenation
        if (binary.IsKind(SyntaxKind.AddExpression)
            && (IsStringType(leftType) || IsStringType(rightType) || IsStringType(resultType)))
        {
            // Use each operand's own type (not the converted/boxed type) so char
            // operands render as characters rather than their code points.
            EmitConcatOperand(binary.Left, _model.GetTypeInfo(binary.Left).Type ?? leftType);
            _w.Write(" + ");
            EmitConcatOperand(binary.Right, _model.GetTypeInfo(binary.Right).Type ?? rightType);
            return;
        }

        // 64-bit integer arithmetic/comparison → System.Int64/UInt64 method calls. Decide on the
        // operands' DECLARED types, not the converted ones: `int >= uint` is promoted to `long` by
        // C#, but int/uint are plain JS numbers (only actual long/ulong are boxed Int64/UInt64
        // instances with .gte/.add/… methods), so such a comparison must stay a plain operator.
        var leftDeclared = _model.GetTypeInfo(binary.Left).Type ?? leftType;
        var rightDeclared = _model.GetTypeInfo(binary.Right).Type ?? rightType;
        if ((Is64BitInteger(leftDeclared) || Is64BitInteger(rightDeclared)) && Long64Op(binary) is not null)
        {
            EmitLong64Binary(binary, leftDeclared, rightDeclared);
            return;
        }

        // decimal arithmetic/comparison → System.Decimal method calls.
        if ((IsDecimalType(leftType) || IsDecimalType(rightType)) && DecimalOp(binary) is { } decOp)
        {
            if (IsDecimalType(leftType)) EmitExpression(binary.Left);
            else { _w.Write("System.Decimal("); EmitExpression(binary.Left); _w.Write(")"); }
            _w.Write($".{decOp}(");
            EmitExpression(binary.Right);
            _w.Write(")");
            return;
        }

        // Integer division
        if (binary.IsKind(SyntaxKind.DivideExpression) && IsIntegerType(leftType) && IsIntegerType(rightType))
        {
            _w.Write("H5R.idiv(");
            EmitExpression(binary.Left);
            _w.Write(", ");
            EmitExpression(binary.Right);
            _w.Write(")");
            return;
        }

        // 32-bit integer multiplication wraps (unchecked C# semantics); JS "*" does not,
        // so route through Math.imul via H5.Int.mul / umul.
        if (binary.IsKind(SyntaxKind.MultiplyExpression)
            && resultType?.SpecialType is SpecialType.System_Int32 or SpecialType.System_UInt32)
        {
            _w.Write(resultType.SpecialType == SpecialType.System_UInt32 ? "H5.Int.umul(" : "H5.Int.mul(");
            EmitExpression(binary.Left);
            _w.Write(", ");
            EmitExpression(binary.Right);
            _w.Write(")");
            return;
        }

        // Null-coalescing
        if (binary.IsKind(SyntaxKind.CoalesceExpression))
        {
            _w.Write("(");
            EmitExpression(binary.Left);
            _w.Write(" ?? ");
            EmitExpression(binary.Right);
            _w.Write(")");
            return;
        }

        // Value equality for == / != on records and non-primitive structs (TimeSpan,
        // DateTime, DateTimeOffset, Guid, user structs…), which need memberwise/operator
        // equality rather than JS reference identity.
        if ((binary.IsKind(SyntaxKind.EqualsExpression) || binary.IsKind(SyntaxKind.NotEqualsExpression))
            && (IsValueEqualityType(leftType) || IsValueEqualityType(rightType)))
        {
            if (binary.IsKind(SyntaxKind.NotEqualsExpression)) _w.Write("!");
            _w.Write("H5R.equals(");
            EmitExpression(binary.Left);
            _w.Write(", ");
            EmitExpression(binary.Right);
            _w.Write(")");
            return;
        }

        // Lifted operators on Nullable<T>: a null operand makes an arithmetic result null
        // and a relational result false (C# semantics). (Equality is fine with ===/!== since
        // nullable is represented as value-or-null.)
        var arith = op is "+" or "-" or "*" or "%";
        var relational = op is "<" or ">" or "<=" or ">=";
        if ((arith || relational) && (IsNullableValueType(leftType) || IsNullableValueType(rightType)))
        {
            var l = Capture(() => EmitExpression(binary.Left));
            var r = Capture(() => EmitExpression(binary.Right));
            _w.Write($"({l} == null || {r} == null ? {(relational ? "false" : "null")} : {l} {op} {r})");
            return;
        }

        var jsOp = op switch
        {
            "==" => "===",
            "!=" => "!==",
            _ => op,
        };

        EmitExpression(binary.Left);
        _w.Write($" {jsOp} ");
        EmitExpression(binary.Right);
    }

    private static bool IsNullableValueType(ITypeSymbol? t)
        => t is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T };

    /// <summary>
    /// A type whose == / != is value equality rather than JS reference identity: records and
    /// non-primitive structs (TimeSpan, DateTime, Guid, user structs…). Primitive numerics,
    /// bool, char, enums, and the specially-handled long/decimal are excluded.
    /// </summary>
    private static bool IsValueEqualityType(ITypeSymbol? t)
    {
        if (t is { IsRecord: true }) return true;
        if (t is not { TypeKind: TypeKind.Struct }) return false;
        if (t.SpecialType != SpecialType.None) return false; // primitive value types
        if (Is64BitInteger(t) || IsDecimalType(t)) return false; // handled by their own ops
        if (t is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T }) return false;
        return true;
    }

    /// <summary>The System.Int64/UInt64 method name for a binary operator, or null.</summary>
    private static string? Long64Op(BinaryExpressionSyntax b) => b.Kind() switch
    {
        SyntaxKind.AddExpression => "add",
        SyntaxKind.SubtractExpression => "sub",
        SyntaxKind.MultiplyExpression => "mul",
        SyntaxKind.DivideExpression => "div",
        SyntaxKind.ModuloExpression => "mod",
        SyntaxKind.LessThanExpression => "lt",
        SyntaxKind.GreaterThanExpression => "gt",
        SyntaxKind.LessThanOrEqualExpression => "lte",
        SyntaxKind.GreaterThanOrEqualExpression => "gte",
        SyntaxKind.EqualsExpression => "eq",
        SyntaxKind.NotEqualsExpression => "ne",
        SyntaxKind.BitwiseAndExpression => "and",
        SyntaxKind.BitwiseOrExpression => "or",
        SyntaxKind.ExclusiveOrExpression => "xor",
        SyntaxKind.LeftShiftExpression => "shl",
        SyntaxKind.RightShiftExpression => "shr",
        _ => null,
    };

    private static bool IsDecimalType(ITypeSymbol? t) => t?.SpecialType == SpecialType.System_Decimal;

    /// <summary>The System.Decimal method name for a binary operator, or null.</summary>
    private static string? DecimalOp(BinaryExpressionSyntax b) => b.Kind() switch
    {
        SyntaxKind.AddExpression => "add",
        SyntaxKind.SubtractExpression => "sub",
        SyntaxKind.MultiplyExpression => "mul",
        SyntaxKind.DivideExpression => "div",
        SyntaxKind.ModuloExpression => "mod",
        SyntaxKind.LessThanExpression => "lt",
        SyntaxKind.GreaterThanExpression => "gt",
        SyntaxKind.LessThanOrEqualExpression => "lte",
        SyntaxKind.GreaterThanOrEqualExpression => "gte",
        SyntaxKind.EqualsExpression => "equals",
        SyntaxKind.NotEqualsExpression => "ne",
        _ => null,
    };

    private void EmitLong64Binary(BinaryExpressionSyntax binary, ITypeSymbol? leftType, ITypeSymbol? rightType)
    {
        var op = Long64Op(binary)!;
        var unsigned = Is64BitUnsigned(leftType) || Is64BitUnsigned(rightType);
        if (op == "shr" && unsigned) op = "shru";

        // The receiver must be a 64-bit instance; lift the left operand if it is a plain number.
        if (Is64BitInteger(leftType))
        {
            EmitExpression(binary.Left);
        }
        else
        {
            _w.Write(unsigned ? "System.UInt64(" : "System.Int64(");
            EmitExpression(binary.Left);
            _w.Write(")");
        }
        _w.Write($".{op}(");
        EmitExpression(binary.Right);
        _w.Write(")");
    }

    private void EmitConcatOperand(ExpressionSyntax operand, ITypeSymbol? type)
    {
        if (IsStringType(type))
        {
            EmitExpression(operand);
        }
        else if (IsCharType(type))
        {
            _w.Write("H5R.chr(");
            EmitExpression(operand);
            _w.Write(")");
        }
        else if (type is { TypeKind: TypeKind.Enum })
        {
            _w.Write($"System.Enum.toString({TypeRef(type)}, ");
            EmitExpression(operand);
            _w.Write(")");
        }
        else
        {
            _w.Write("H5R.toStr(");
            EmitExpression(operand);
            _w.Write(")");
        }
    }

    private void EmitAssignment(AssignmentExpressionSyntax assignment)
    {
        var op = assignment.OperatorToken.Text;
        var leftType = _model.GetTypeInfo(assignment.Left).Type;
        var rightType = _model.GetTypeInfo(assignment.Right).Type;

        // Discard assignment: _ = expr → evaluate expr for its side effects only.
        if (op == "=" && _model.GetSymbolInfo(assignment.Left).Symbol is IDiscardSymbol)
        {
            EmitExpression(assignment.Right);
            return;
        }

        // Indexer set on a collection: coll[i] = v → coll.setItem(i, v)
        if (op == "=" && assignment.Left is ElementAccessExpressionSyntax ea
            && _model.GetSymbolInfo(ea).Symbol is IPropertySymbol { IsIndexer: true } idx
            && idx.ContainingType.SpecialType != SpecialType.System_String)
        {
            // Indexer setter [Template].
            if (idx.SetMethod is { } setIdx && H5Naming.GetTemplate(setIdx.OriginalDefinition) is { } setIdxTpl)
            {
                var recv = Capture(() => EmitExpression(ea.Expression));
                var args = ea.ArgumentList.Arguments.Select(a => Capture(() => EmitExpression(a.Expression))).ToList();
                args.Add(Capture(() => EmitExpressionConverted(assignment.Right, leftType)));
                WriteTemplate(setIdxTpl, idx.IsStatic, isExtension: false, recv, new(), args);
                return;
            }
            // An [External] type's plain indexer sets via native bracket access (domElement["name"] = v).
            if (H5Naming.IsNativeIndexer(idx))
            {
                EmitExpression(ea.Expression);
                _w.Write("[");
                EmitArgumentList(ea.ArgumentList);
                _w.Write("] = ");
                EmitExpressionConverted(assignment.Right, leftType);
                return;
            }
            EmitExpression(ea.Expression);
            _w.Write("." + H5Naming.IndexerAccessorName(idx, isGet: false) + "(");
            EmitArgumentList(ea.ArgumentList);
            _w.Write(", ");
            EmitExpressionConverted(assignment.Right, leftType);
            _w.Write(")");
            return;
        }

        // Property setter with a [Template] (e.g. StringBuilder.Length → setLength({0})).
        if (op == "=" && _model.GetSymbolInfo(assignment.Left).Symbol is IPropertySymbol { SetMethod: { } setter } setProp
            && !setProp.IsIndexer
            && H5Naming.GetTemplate(setter.OriginalDefinition) is { } setTemplate)
        {
            var recv = setProp.IsStatic ? TypeRef(setProp.ContainingType)
                : assignment.Left is MemberAccessExpressionSyntax sma ? Capture(() => EmitExpression(sma.Expression))
                : "this";
            var val = Capture(() => EmitExpressionConverted(assignment.Right, leftType));
            WriteTemplate(setTemplate, setProp.IsStatic, isExtension: false, recv,
                new() { ["value"] = val }, new() { val });
            return;
        }

        // Delegate / event subscription: d += h, d -= h, ev += h, ev -= h → combine/remove.
        if ((op == "+=" || op == "-=")
            && (leftType is { TypeKind: TypeKind.Delegate }
                || _model.GetSymbolInfo(assignment.Left).Symbol is IEventSymbol))
        {
            EmitExpression(assignment.Left);
            _w.Write($" = H5R.{(op == "+=" ? "combine" : "remove")}(");
            EmitExpression(assignment.Left);
            _w.Write(", ");
            EmitExpression(assignment.Right);
            _w.Write(")");
            return;
        }

        // Compound string concat: s += x
        if (op == "+=" && IsStringType(leftType))
        {
            EmitExpression(assignment.Left);
            _w.Write(" = ");
            EmitConcatOperand(assignment.Left, leftType);
            _w.Write(" + ");
            EmitConcatOperand(assignment.Right, rightType);
            return;
        }

        // Compound integer division: x /= y
        if (op == "/=" && IsIntegerType(leftType) && IsIntegerType(rightType))
        {
            EmitExpression(assignment.Left);
            _w.Write(" = H5R.idiv(");
            EmitExpression(assignment.Left);
            _w.Write(", ");
            EmitExpression(assignment.Right);
            _w.Write(")");
            return;
        }

        EmitExpression(assignment.Left);
        _w.Write($" {op} ");
        EmitExpressionConverted(assignment.Right, leftType);
    }

    private void EmitPrefixUnary(PrefixUnaryExpressionSyntax prefix)
    {
        if (_model.GetSymbolInfo(prefix).Symbol is IMethodSymbol { MethodKind: MethodKind.UserDefinedOperator } opm
            && opm.Locations.Any(l => l.IsInSource)
            && !prefix.IsKind(SyntaxKind.PreIncrementExpression) && !prefix.IsKind(SyntaxKind.PreDecrementExpression))
        {
            _w.Write($"{TypeRef(opm.ContainingType)}.{H5Naming.MemberJsName(opm)}(");
            EmitExpression(prefix.Operand);
            _w.Write(")");
            return;
        }

        // 64-bit negation / bitwise-not → System.Int64/UInt64 methods.
        if (Is64BitInteger(_model.GetTypeInfo(prefix.Operand).Type))
        {
            if (prefix.IsKind(SyntaxKind.UnaryMinusExpression)) { EmitExpression(prefix.Operand); _w.Write(".neg()"); return; }
            if (prefix.IsKind(SyntaxKind.BitwiseNotExpression)) { EmitExpression(prefix.Operand); _w.Write(".not()"); return; }
            if (prefix.IsKind(SyntaxKind.UnaryPlusExpression)) { EmitExpression(prefix.Operand); return; }
        }

        // decimal negation → System.Decimal.neg().
        if (IsDecimalType(_model.GetTypeInfo(prefix.Operand).Type))
        {
            if (prefix.IsKind(SyntaxKind.UnaryMinusExpression)) { EmitExpression(prefix.Operand); _w.Write(".neg()"); return; }
            if (prefix.IsKind(SyntaxKind.UnaryPlusExpression)) { EmitExpression(prefix.Operand); return; }
        }

        _w.Write(prefix.OperatorToken.Text);
        EmitExpression(prefix.Operand);
    }

    private void EmitPostfixUnary(PostfixUnaryExpressionSyntax postfix)
    {
        EmitExpression(postfix.Operand);
        _w.Write(postfix.OperatorToken.Text);
    }

    // ---- cast --------------------------------------------------------------

    private void EmitCast(CastExpressionSyntax cast)
    {
        var targetType = _model.GetTypeInfo(cast.Type).Type;
        var sourceType = _model.GetTypeInfo(cast.Expression).Type;

        // 64-bit conversions: to long/ulong → wrap; from long/ulong to a 32-bit/float → number.
        if (Is64BitInteger(targetType) && !Is64BitInteger(sourceType) && !IsDecimalType(sourceType))
        {
            _w.Write(Is64BitUnsigned(targetType) ? "System.UInt64(" : "System.Int64(");
            EmitExpression(cast.Expression);
            _w.Write(")");
            return;
        }
        if (Is64BitInteger(sourceType) && !Is64BitInteger(targetType) && (IsIntegerType(targetType) || IsFloatingType(targetType)))
        {
            _w.Write("("); EmitExpression(cast.Expression); _w.Write(").toNumber()");
            return;
        }

        // decimal conversions: to decimal → wrap; decimal → float/int → toFloat (truncated for int).
        if (IsDecimalType(targetType) && !IsDecimalType(sourceType))
        {
            _w.Write("System.Decimal("); EmitExpression(cast.Expression); _w.Write(")");
            return;
        }
        if (IsDecimalType(sourceType) && !IsDecimalType(targetType) && (IsIntegerType(targetType) || IsFloatingType(targetType)))
        {
            if (IsIntegerType(targetType) && !Is64BitInteger(targetType)) { _w.Write("H5R.trunc("); EmitExpression(cast.Expression); _w.Write(".toFloat())"); }
            else { _w.Write("("); EmitExpression(cast.Expression); _w.Write(").toFloat()"); }
            return;
        }

        // Numeric narrowing to an integer type truncates toward zero.
        if (IsIntegerType(targetType) && IsFloatingType(sourceType))
        {
            _w.Write("H5R.trunc(");
            EmitExpression(cast.Expression);
            _w.Write(")");
            return;
        }

        // char <-> int: chars are represented as their code point (number).
        // Other reference casts are erased.
        EmitExpression(cast.Expression);
    }

    // ---- interpolated string -----------------------------------------------

    private void EmitInterpolatedString(InterpolatedStringExpressionSyntax interp)
    {
        _w.Write("(");
        var first = true;
        var hadContent = false;
        foreach (var content in interp.Contents)
        {
            switch (content)
            {
                case InterpolatedStringTextSyntax text:
                    if (!first) _w.Write(" + ");
                    _w.Write(JsString(text.TextToken.ValueText));
                    first = false; hadContent = true;
                    break;
                case InterpolationSyntax interpolation:
                    if (!first) _w.Write(" + ");
                    if (interpolation.FormatClause is not null)
                    {
                        _w.Write("H5R.formatValue(");
                        EmitExpression(interpolation.Expression);
                        _w.Write($", {JsString(interpolation.FormatClause.FormatStringToken.ValueText)})");
                    }
                    else if (IsCharType(_model.GetTypeInfo(interpolation.Expression).Type))
                    {
                        _w.Write("H5R.chr(");
                        EmitExpression(interpolation.Expression);
                        _w.Write(")");
                    }
                    else if (_model.GetTypeInfo(interpolation.Expression).Type is { TypeKind: TypeKind.Enum } enumT)
                    {
                        _w.Write($"System.Enum.toString({TypeRef(enumT)}, ");
                        EmitExpression(interpolation.Expression);
                        _w.Write(")");
                    }
                    else
                    {
                        _w.Write("H5R.toStr(");
                        EmitExpression(interpolation.Expression);
                        _w.Write(")");
                    }
                    first = false; hadContent = true;
                    break;
            }
        }
        if (!hadContent) _w.Write("\"\"");
        _w.Write(")");
    }

    // ---- element access ----------------------------------------------------

    private void EmitElementAccess(ElementAccessExpressionSyntax element)
    {
        var symbol = _model.GetSymbolInfo(element).Symbol;

        if (symbol is IPropertySymbol { IsIndexer: true } indexer)
        {
            // string[i] yields a char (code point).
            if (indexer.ContainingType.SpecialType == SpecialType.System_String)
            {
                EmitExpression(element.Expression);
                _w.Write(".charCodeAt(");
                EmitArgumentList(element.ArgumentList);
                _w.Write(")");
                return;
            }

            // Indexer getter [Template] (e.g. some BCL indexers).
            if (indexer.GetMethod is { } getM && H5Naming.GetTemplate(getM.OriginalDefinition) is { } getTpl)
            {
                var recv = Capture(() => EmitExpression(element.Expression));
                var args = element.ArgumentList.Arguments.Select(a => Capture(() => EmitExpression(a.Expression))).ToList();
                WriteTemplate(getTpl, indexer.IsStatic, isExtension: false, recv, new(), args);
                return;
            }

            // An [External] type's plain indexer is native bracket access (e.g. domElement["name"]).
            if (H5Naming.IsNativeIndexer(indexer))
            {
                EmitExpression(element.Expression);
                _w.Write("[");
                EmitArgumentList(element.ArgumentList);
                _w.Write("]");
                return;
            }

            // Source types and BCL collections route through the indexer accessor.
            EmitExpression(element.Expression);
            _w.Write("." + H5Naming.IndexerAccessorName(indexer, isGet: true) + "(");
            EmitArgumentList(element.ArgumentList);
            _w.Write(")");
            return;
        }

        // Arrays with an Index (^n) or Range (a..b) argument.
        var arg = element.ArgumentList.Arguments.Count == 1 ? element.ArgumentList.Arguments[0].Expression : null;
        if (arg is RangeExpressionSyntax range)
        {
            var arr = Capture(() => EmitExpression(element.Expression));
            _w.Write($"{arr}.slice(");
            EmitIndexValue(range.LeftOperand, arr, isEnd: false);
            _w.Write(", ");
            EmitIndexValue(range.RightOperand, arr, isEnd: true);
            _w.Write(")");
            return;
        }
        if (arg is not null && arg.IsKind(SyntaxKind.IndexExpression))
        {
            var arr = Capture(() => EmitExpression(element.Expression));
            _w.Write($"{arr}[");
            EmitIndexValue(arg, arr, isEnd: false);
            _w.Write("]");
            return;
        }

        // Arrays: native element access.
        EmitExpression(element.Expression);
        _w.Write("[");
        EmitArgumentList(element.ArgumentList);
        _w.Write("]");
    }

    /// <summary>Emits a JS index/bound for C# Index/Range on an array (`^n` → len-n).</summary>
    private void EmitIndexValue(ExpressionSyntax? expr, string arrRef, bool isEnd)
    {
        if (expr is null) { _w.Write(isEnd ? $"{arrRef}.length" : "0"); return; }
        if (expr.IsKind(SyntaxKind.IndexExpression) && expr is PrefixUnaryExpressionSyntax fromEnd)
        {
            _w.Write($"{arrRef}.length - ");
            EmitExpression(fromEnd.Operand);
            return;
        }
        EmitExpression(expr);
    }

    // ---- arrays ------------------------------------------------------------

    private void EmitArrayCreation(ArrayCreationExpressionSyntax array)
    {
        if (array.Initializer is not null)
        {
            EmitInitializerArray(array.Initializer);
            return;
        }

        // new T[n] → H5R.array(n, default)
        var rankSpec = array.Type.RankSpecifiers.FirstOrDefault();
        var elementType = _model.GetTypeInfo(array.Type.ElementType).Type;
        if (rankSpec is { Sizes.Count: 1 } && rankSpec.Sizes[0] is not OmittedArraySizeExpressionSyntax)
        {
            _w.Write("H5R.array(");
            EmitExpression(rankSpec.Sizes[0]);
            _w.Write($", {DefaultValueLiteral(elementType!)})");
            return;
        }

        _w.Write("[]");
    }

    private void EmitInitializerArray(InitializerExpressionSyntax initializer)
    {
        _w.Write("[");
        for (var i = 0; i < initializer.Expressions.Count; i++)
        {
            if (i > 0) _w.Write(", ");
            EmitExpression(initializer.Expressions[i]);
        }
        _w.Write("]");
    }

    /// <summary>
    /// C# 12 collection expression `[a, b, ..spread]`. Arrays / spans map directly to a
    /// JS array literal (spreads become `...`); a List&lt;T&gt; (or other constructible
    /// collection) is built from that array via its enumerable constructor.
    /// </summary>
    private void EmitCollectionExpression(CollectionExpressionSyntax collection)
    {
        void EmitArray()
        {
            _w.Write("[");
            for (var i = 0; i < collection.Elements.Count; i++)
            {
                if (i > 0) _w.Write(", ");
                if (collection.Elements[i] is SpreadElementSyntax spread)
                {
                    // Arrays spread directly; other enumerables are drained to an array.
                    if (_model.GetTypeInfo(spread.Expression).Type is IArrayTypeSymbol)
                    {
                        _w.Write("...");
                        EmitExpression(spread.Expression);
                    }
                    else
                    {
                        _w.Write("...H5R.spread(");
                        EmitExpression(spread.Expression);
                        _w.Write(")");
                    }
                }
                else if (collection.Elements[i] is ExpressionElementSyntax elem)
                {
                    EmitExpression(elem.Expression);
                }
            }
            _w.Write("]");
        }

        var target = _model.GetTypeInfo(collection).ConvertedType;
        // Arrays, spans, and collection interfaces (IEnumerable/IReadOnlyList/…) are all
        // represented as plain JS arrays (h5.js enumerates arrays natively).
        if (target is IArrayTypeSymbol
            || target is { TypeKind: TypeKind.Interface }
            || target?.OriginalDefinition.ToDisplayString() is "System.Span<T>" or "System.ReadOnlySpan<T>"
            || target is null)
        {
            EmitArray();
            return;
        }

        // A concrete collection type (e.g. List<T>) — build a fresh instance and add each
        // element (works regardless of the type's constructor overload numbering).
        _w.Write($"(() => {{ var $c = new ({TypeRef(target)})(); ");
        _w.Write($"var $s = ");
        EmitArray();
        _w.Write("; for (var $i = 0; $i < $s.length; $i++) { $c.add($s[$i]); } return $c; })()");
    }

    // ---- lambda ------------------------------------------------------------

    private void EmitLambda(IEnumerable<string> parameters, CSharpSyntaxNode body, bool isAsync,
        SeparatedSyntaxList<ParameterSyntax>? paramSyntax = null)
    {
        // Emit an arrow function so `this` is captured lexically, matching C# lambda semantics
        // (a plain `function` would rebind `this` and break `this`-referencing closures).
        // An async lambda returns an h5.js Task (via the H5R.fromPromise wrapper in
        // EmitMaybeAsyncBody), so it composes with Task.Run/WhenAll/ContinueWith; the outer
        // function is therefore not itself `async`.
        _w.Write("(");
        // Uniquify discard parameters ("_") so JS doesn't see duplicate names.
        var discardN = 0;
        _w.Write(string.Join(", ", parameters.Select(p =>
            p == "_" ? "$d" + discardN++ : NameMangler.JsIdentifier(p))));
        _w.Write(") => ");

        // Optional lambda parameters (C# 12): default when undefined.
        var defaults = paramSyntax?.Where(p => p.Default is not null).ToList();

        _w.Block(() =>
        {
            EmitLambdaParamDefaults(defaults);
            EmitMaybeAsyncBody(isAsync, () =>
            {
                if (body is BlockSyntax block)
                {
                    EmitStatements(block.Statements);
                }
                else if (body is ExpressionSyntax exprBody)
                {
                    // Hoist out-var / is-pattern variables the body introduces (e.g.
                    // `p => dict.TryGetValue(p, out var i) ? i : 0`) before returning it.
                    PredeclareInlineVars(exprBody);
                    // If the lambda's body has a value, return it.
                    _w.Write("return ");
                    EmitExpression(exprBody);
                    _w.WriteLine(";");
                }
            });
        });
    }

    private void EmitLambdaParamDefaults(System.Collections.Generic.List<ParameterSyntax>? defaults)
    {
        if (defaults is null) return;
        foreach (var p in defaults)
        {
            var name = NameMangler.JsIdentifier(p.Identifier.Text);
            _w.Write($"if ({name} === undefined) {{ {name} = ");
            EmitExpression(p.Default!.Value);
            _w.WriteLine("; }");
        }
    }

    // ---- constant / type helpers -------------------------------------------

    private string ConstantLiteral(object? value, ITypeSymbol type)
    {
        if (value is null) return type.SpecialType == SpecialType.System_String ? "null" : DefaultValueLiteral(type);
        // A string-backed enum ([Enum(Emit.StringName*)]) constant emits its string name, not the
        // numeric ordinal — so a defaulted enum parameter (e.g. `format = default`) seeds the
        // string the runtime actually compares against.
        if (type is INamedTypeSymbol { TypeKind: TypeKind.Enum } en && H5Naming.EnumEmitMode(en) is 3 or 4 or 5 or 6)
            return EnumConstantLiteral(en, value);
        // 64-bit integer constants (e.g. long.MinValue) must be System.Int64/UInt64 instances.
        if (value is long or ulong && Is64BitInteger(type))
            return Long64Literal(value, Is64BitUnsigned(type));
        // decimal constants (e.g. decimal.MaxValue) must be System.Decimal instances.
        if (value is decimal && IsDecimalType(type))
            return $"System.Decimal(\"{((decimal)value).ToString(CultureInfo.InvariantCulture)}\")";
        return value switch
        {
            bool b => b ? "true" : "false",
            string s => JsString(s),
            char c => ((int)c).ToString(CultureInfo.InvariantCulture),
            double d => FormatDouble(d),
            float f => FormatDouble(f),
            decimal m => m.ToString(CultureInfo.InvariantCulture),
            IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? "null",
        };
    }

    /// <summary>The string literal for a string-backed enum constant with the given numeric value.</summary>
    private string EnumConstantLiteral(INamedTypeSymbol enumType, object value)
    {
        var mode = H5Naming.EnumEmitMode(enumType);
        var v = Convert.ToInt64(value);
        var field = enumType.GetMembers().OfType<IFieldSymbol>()
            .FirstOrDefault(f => f.HasConstantValue && Convert.ToInt64(f.ConstantValue) == v);
        return field is not null ? JsString(H5Naming.EnumStringName(field, mode)) : "null";
    }

    private static bool IsStringType(ITypeSymbol? type) => type?.SpecialType == SpecialType.System_String;
    private static bool IsCharType(ITypeSymbol? type) => type?.SpecialType == SpecialType.System_Char;

    private static bool IsIntegerType(ITypeSymbol? type)
    {
        if (type is null) return false;
        if (type.TypeKind == TypeKind.Enum) return true;
        return type.SpecialType is SpecialType.System_SByte or SpecialType.System_Byte
            or SpecialType.System_Int16 or SpecialType.System_UInt16
            or SpecialType.System_Int32 or SpecialType.System_UInt32
            or SpecialType.System_Int64 or SpecialType.System_UInt64;
    }

    private static bool IsFloatingType(ITypeSymbol? type)
        => type?.SpecialType is SpecialType.System_Single or SpecialType.System_Double;

    /// <summary>64-bit integer type (long/ulong) — h5.js models these as System.Int64/UInt64 objects.</summary>
    private static bool Is64BitInteger(ITypeSymbol? type)
        => type?.SpecialType is SpecialType.System_Int64 or SpecialType.System_UInt64;

    private static bool Is64BitUnsigned(ITypeSymbol? type)
        => type?.SpecialType == SpecialType.System_UInt64;
}
