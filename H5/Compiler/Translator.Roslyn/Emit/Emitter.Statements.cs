using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace H5.Translator.Roslyn;

public sealed partial class Emitter
{
    // Break-target stack: null = plain `break` (loops / native switch);
    // a label string = `break <label>` (pattern-switch labelled block).
    private readonly Stack<string?> _breakTargets = new();

    private void EmitStatement(StatementSyntax statement)
    {
        // C# out-var declarations and is-pattern variables have block scope; declare
        // them before the statement that introduces them.
        PredeclareInlineVars(statement);

        switch (statement)
        {
            case BlockSyntax block:
                EmitBlock(block);
                break;
            case LocalDeclarationStatementSyntax local:
                EmitLocalDeclaration(local);
                break;
            case ExpressionStatementSyntax expr:
                if (expr.Expression is AssignmentExpressionSyntax da && IsDeconstruction(da))
                {
                    EmitDeconstruction(da);
                }
                else
                {
                    EmitExpressionStatement(expr.Expression);
                }
                break;
            case IfStatementSyntax ifStmt:
                EmitIf(ifStmt);
                break;
            case ForStatementSyntax forStmt:
                EmitFor(forStmt);
                break;
            case ForEachStatementSyntax forEach:
                EmitForEach(forEach);
                break;
            case ForEachVariableStatementSyntax forEachVar:
                EmitForEachVariable(forEachVar);
                break;
            case WhileStatementSyntax whileStmt:
                EmitWhile(whileStmt);
                break;
            case DoStatementSyntax doStmt:
                EmitDo(doStmt);
                break;
            case ReturnStatementSyntax ret:
                EmitReturn(ret);
                break;
            case BreakStatementSyntax:
                if (_breakTargets.Count > 0 && _breakTargets.Peek() is { } lbl)
                    _w.WriteLine($"break {lbl};");
                else
                    _w.WriteLine("break;");
                break;
            case ContinueStatementSyntax:
                _w.WriteLine("continue;");
                break;
            case ThrowStatementSyntax throwStmt:
                EmitThrow(throwStmt);
                break;
            case TryStatementSyntax tryStmt:
                EmitTry(tryStmt);
                break;
            case UsingStatementSyntax usingStmt:
                EmitUsing(usingStmt);
                break;
            case SwitchStatementSyntax switchStmt:
                EmitSwitch(switchStmt);
                break;
            case LocalFunctionStatementSyntax localFn:
                EmitLocalFunction(localFn);
                break;
            case LockStatementSyntax lockStmt:
                // Single-threaded: the lock is a no-op, emit the body.
                EmitStatement(lockStmt.Statement);
                break;
            case CheckedStatementSyntax checkedStmt:
                EmitBlock(checkedStmt.Block);
                break;
            case YieldStatementSyntax yieldStmt:
                if (yieldStmt.IsKind(SyntaxKind.YieldReturnStatement))
                {
                    _w.Write("yield ");
                    if (yieldStmt.Expression is not null) EmitExpression(yieldStmt.Expression);
                    _w.WriteLine(";");
                }
                else
                {
                    _w.WriteLine("return;"); // yield break
                }
                break;
            case GotoStatementSyntax gotoStmt:
                EmitGoto(gotoStmt);
                break;
            case LabeledStatementSyntax labeled:
                // Reached outside a state machine (label with no goto to it): emit its body.
                EmitStatement(labeled.Statement);
                break;
            case EmptyStatementSyntax:
                break;
            default:
                Unsupported(statement, statement.Kind().ToString());
                break;
        }
    }

    /// <summary>
    /// Declares block-scoped variables introduced inline by a statement's — or an
    /// expression-bodied member's / lambda's — own expressions (out-var declarations, is-pattern
    /// variables) at the top of the current JS block. Does not descend into nested
    /// statements/blocks or lambdas — those manage their own.
    /// </summary>
    private void PredeclareInlineVars(SyntaxNode scope)
    {
        // A local declaration's own initializer may contain out-vars, but its declared
        // variables are emitted normally; only collect designation-introduced names.
        var names = new List<string>();
        CollectInlineDesignations(scope, isRoot: true, names);

        foreach (var name in names.Distinct())
        {
            _w.WriteLine($"let {NameMangler.JsIdentifier(name)};");
        }
    }

    private void CollectInlineDesignations(SyntaxNode node, bool isRoot, List<string> names)
    {
        foreach (var child in node.ChildNodes())
        {
            // Do not cross scope boundaries.
            if (!isRoot && child is StatementSyntax) continue;
            if (child is AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax) continue;

            if (child is SingleVariableDesignationSyntax single)
            {
                var parent = single.Parent;
                var include = parent is DeclarationPatternSyntax or RecursivePatternSyntax or VarPatternSyntax
                    // out-var declarations (not tuple-deconstruction, which declares its own).
                    || (parent is DeclarationExpressionSyntax { Parent: ArgumentSyntax arg }
                        && (arg.RefKindKeyword.IsKind(SyntaxKind.OutKeyword) || arg.RefKindKeyword.IsKind(SyntaxKind.RefKeyword)));
                if (include) names.Add(single.Identifier.Text);
            }

            CollectInlineDesignations(child, isRoot: false, names);
        }
    }

    private void EmitBlock(BlockSyntax block)
    {
        _w.Block(() => EmitStatements(block.Statements));
        _w.WriteLine();
    }

    /// <summary>
    /// Emits a statement sequence, desugaring `using var x = e;` declarations into a
    /// try/finally whose body is the remainder of the sequence (C# block-scoped dispose).
    /// </summary>
    internal void EmitStatements(IReadOnlyList<StatementSyntax> statements, int start = 0)
    {
        // A body containing labels (goto targets) is lowered into a state machine so that
        // `goto` can jump between top-level sections (works for backward loops and forward
        // skips, and — since async bodies are native `async` IIFEs — across `await`).
        if (start == 0 && statements.Any(s => s is LabeledStatementSyntax))
        {
            EmitGotoStateMachine(statements);
            return;
        }

        // C# local functions are hoisted (callable before their textual position), so emit
        // them first (as arrow closures) at the top of the block.
        if (start == 0)
            foreach (var fn in statements.OfType<LocalFunctionStatementSyntax>())
                EmitLocalFunction(fn);

        for (var i = start; i < statements.Count; i++)
        {
            var s = statements[i];
            if (s is LocalFunctionStatementSyntax) continue; // already hoisted above
            if (s is LocalDeclarationStatementSyntax { UsingKeyword.RawKind: not 0 } u)
            {
                var resources = new List<string>();
                foreach (var v in u.Declaration.Variables)
                {
                    var name = NameMangler.JsIdentifier(v.Identifier.Text);
                    resources.Add(name);
                    _w.Write($"let {name} = ");
                    if (v.Initializer is not null) EmitExpression(v.Initializer.Value); else _w.Write("null");
                    _w.WriteLine(";");
                }
                _w.Write("try ");
                _w.Block(() => EmitStatements(statements, i + 1));
                _w.WriteLine();
                _w.Write("finally ");
                _w.Block(() => { foreach (var r in Enumerable.Reverse(resources)) _w.WriteLine($"H5R.dispose({r});"); });
                _w.WriteLine();
                return; // the rest of the sequence was emitted inside the try
            }
            EmitStatement(s);
        }
    }

    /// <summary>
    /// Lowers a statement body containing labels into a `for(;;) switch($state)` machine.
    /// Top-level statements are split into sections at each label; sequential flow is the
    /// switch's fall-through, and `goto L` sets $state and re-enters the loop. Locals are
    /// declared with `var` (function-scoped) so their values survive across loop iterations.
    /// </summary>
    private void EmitGotoStateMachine(IReadOnlyList<StatementSyntax> statements)
    {
        var sections = new List<(string? label, List<StatementSyntax> stmts)> { (null, new List<StatementSyntax>()) };
        var labelIndex = new Dictionary<string, int>();
        foreach (var s in statements)
        {
            if (s is LabeledStatementSyntax lbl)
            {
                labelIndex[lbl.Identifier.Text] = sections.Count;
                sections.Add((lbl.Identifier.Text, new List<StatementSyntax> { lbl.Statement }));
            }
            else
            {
                sections[^1].stmts.Add(s);
            }
        }

        var depth = _gotoContexts.Count;
        var loopLabel = depth == 0 ? "$goto" : "$goto" + depth;
        var stateVar = depth == 0 ? "$state" : "$state" + depth;

        _w.WriteLine($"var {stateVar} = 0;");
        _w.Write($"{loopLabel}: for (;;) ");
        _gotoContexts.Push((labelIndex, loopLabel, stateVar));
        _w.Block(() =>
        {
            _w.Write($"switch ({stateVar}) ");
            _w.Block(() =>
            {
                for (var i = 0; i < sections.Count; i++)
                {
                    _w.WriteLine($"case {i}:");
                    _w.Indent();
                    foreach (var st in sections[i].stmts) EmitStatement(st);
                    _w.Outdent();
                }
            });
            _w.WriteLine($"break {loopLabel};");
        });
        _gotoContexts.Pop();
        _w.WriteLine();
    }

    private void EmitGoto(GotoStatementSyntax gotoStmt)
    {
        if (gotoStmt.IsKind(SyntaxKind.GotoStatement) && gotoStmt.Expression is IdentifierNameSyntax id
            && _gotoContexts.Count > 0 && _gotoContexts.Peek().labels.TryGetValue(id.Identifier.Text, out var idx))
        {
            var ctx = _gotoContexts.Peek();
            _w.WriteLine($"{ctx.stateVar} = {idx}; continue {ctx.loopLabel};");
            return;
        }
        Unsupported(gotoStmt, "goto");
    }

    private void EmitLocalDeclaration(LocalDeclarationStatementSyntax local)
    {
        if (local.UsingKeyword != default)
        {
            EmitUsingDeclaration(local);
            return;
        }

        // A local declared inside a loop body is emitted with `let` so each iteration gets a fresh
        // binding — a closure created in the loop then captures that iteration's value (C# block
        // scoping), not the final one. Outside a loop, `var` (function scope) is kept: it matches
        // H5's model and tolerates the same-name redeclarations across flattened scopes that some
        // code relies on (which `let` would reject). A goto state machine also needs `var` so a
        // local persists across `case` transitions as the loop re-enters the switch.
        var kw = _loopDepth > 0 && _gotoContexts.Count == 0 ? "let" : "var";
        foreach (var variable in local.Declaration.Variables)
        {
            _w.Write($"{kw} {NameMangler.JsIdentifier(variable.Identifier.Text)}");
            if (variable.Initializer is not null)
            {
                _w.Write(" = ");
                EmitExpressionConverted(variable.Initializer.Value, _model.GetTypeInfo(variable.Initializer.Value).ConvertedType);
            }
            _w.WriteLine(";");
        }
    }

    private void EmitIf(IfStatementSyntax ifStmt)
    {
        _w.Write("if (");
        EmitExpression(ifStmt.Condition);
        _w.Write(") ");
        EmitStatementAsBlock(ifStmt.Statement);

        if (ifStmt.Else is not null)
        {
            _w.Write("else ");
            if (ifStmt.Else.Statement is IfStatementSyntax elseIf)
            {
                EmitIf(elseIf);
            }
            else
            {
                EmitStatementAsBlock(ifStmt.Else.Statement);
            }
        }
    }

    /// <summary>Emits a statement, wrapping single statements into a block for safety.</summary>
    private void EmitStatementAsBlock(StatementSyntax statement)
    {
        if (statement is BlockSyntax block)
        {
            EmitBlock(block);
        }
        else
        {
            _w.Block(() => EmitStatement(statement));
            _w.WriteLine();
        }
    }

    private void EmitFor(ForStatementSyntax forStmt)
    {
        _w.Write("for (");
        if (forStmt.Declaration is not null)
        {
            _w.Write("let ");
            var first = true;
            foreach (var v in forStmt.Declaration.Variables)
            {
                if (!first) _w.Write(", ");
                first = false;
                _w.Write(NameMangler.JsIdentifier(v.Identifier.Text));
                if (v.Initializer is not null) { _w.Write(" = "); EmitExpression(v.Initializer.Value); }
            }
        }
        else
        {
            var first = true;
            foreach (var init in forStmt.Initializers)
            {
                if (!first) _w.Write(", ");
                first = false;
                EmitExpression(init);
            }
        }
        _w.Write("; ");
        if (forStmt.Condition is not null) EmitExpression(forStmt.Condition);
        _w.Write("; ");
        var firstInc = true;
        foreach (var inc in forStmt.Incrementors)
        {
            if (!firstInc) _w.Write(", ");
            firstInc = false;
            EmitExpression(inc);
        }
        _w.Write(") ");
        _breakTargets.Push(null);
        _loopDepth++;
        EmitStatementAsBlock(forStmt.Statement);
        _loopDepth--;
        _breakTargets.Pop();
    }

    private void EmitForEach(ForEachStatementSyntax forEach)
    {
        var iterVar = NameMangler.JsIdentifier(forEach.Identifier.Text);
        var enumVar = EmitEnumeratorInit(forEach, forEach.Expression);
        _w.Write($"while ({enumVar}.moveNext()) ");
        _breakTargets.Push(null);
        _loopDepth++;
        _w.Block(() =>
        {
            _w.WriteLine($"let {iterVar} = {enumVar}.current;");
            EmitForEachBody(forEach.Statement);
        });
        _loopDepth--;
        _breakTargets.Pop();
        _w.WriteLine();
    }

    /// <summary>foreach with deconstruction: foreach (var (a, b) in seq) — bind each element.</summary>
    private void EmitForEachVariable(ForEachVariableStatementSyntax forEach)
    {
        var enumVar = EmitEnumeratorInit(forEach, forEach.Expression);
        var elementIsTuple = _model.GetForEachStatementInfo(forEach).ElementType is { IsTupleType: true };
        var targets = CollectDeconstructionTargets(forEach.Variable).ToList();
        _w.Write($"while ({enumVar}.moveNext()) ");
        _breakTargets.Push(null);
        _loopDepth++;
        _w.Block(() =>
        {
            var cur = enumVar + "c";
            _w.WriteLine($"let {cur} = {enumVar}.current;");
            EmitDeconstructionBindings(targets, cur, elementIsTuple);
            EmitForEachBody(forEach.Statement);
        });
        _loopDepth--;
        _breakTargets.Pop();
        _w.WriteLine();
    }

    /// <summary>
    /// Emits <c>var $e = H5R.getEnumerator(source)</c>, routing through an extension
    /// GetEnumerator when the foreach binds to one, and returns the enumerator variable name.
    /// </summary>
    private string EmitEnumeratorInit(CommonForEachStatementSyntax forEach, ExpressionSyntax source)
    {
        var enumVar = "$e" + forEach.GetHashCode().ToString("x").Substring(0, 4);
        var getEnum = _model.GetForEachStatementInfo(forEach).GetEnumeratorMethod;
        var ext = getEnum is { IsExtensionMethod: true } ? (getEnum.ReducedFrom ?? getEnum) : null;
        _w.Write($"var {enumVar} = H5R.getEnumerator(");
        if (ext is not null && ext.Locations.Any(l => l.IsInSource))
        {
            _w.Write($"{TypeRef(ext.ContainingType)}.{H5Naming.MemberJsName(ext)}(");
            EmitExpression(source);
            _w.Write(")");
        }
        else
        {
            EmitExpression(source);
        }
        _w.WriteLine(");");
        return enumVar;
    }

    private void EmitForEachBody(StatementSyntax body)
    {
        if (body is BlockSyntax block)
        {
            foreach (var s in block.Statements) EmitStatement(s);
        }
        else
        {
            EmitStatement(body);
        }
    }

    private void EmitWhile(WhileStatementSyntax whileStmt)
    {
        _w.Write("while (");
        EmitExpression(whileStmt.Condition);
        _w.Write(") ");
        _breakTargets.Push(null);
        _loopDepth++;
        EmitStatementAsBlock(whileStmt.Statement);
        _loopDepth--;
        _breakTargets.Pop();
    }

    private void EmitDo(DoStatementSyntax doStmt)
    {
        _w.Write("do ");
        _breakTargets.Push(null);
        _loopDepth++;
        EmitStatementAsBlock(doStmt.Statement);
        _loopDepth--;
        _breakTargets.Pop();
        _w.Write("while (");
        EmitExpression(doStmt.Condition);
        _w.WriteLine(");");
    }

    private void EmitReturn(ReturnStatementSyntax ret)
    {
        if (ret.Expression is null)
        {
            _w.WriteLine("return;");
            return;
        }
        _w.Write("return ");
        var method = _model.GetEnclosingSymbol(ret) as IMethodSymbol;
        EmitExpressionConverted(ret.Expression, _model.GetTypeInfo(ret.Expression).ConvertedType);
        _w.WriteLine(";");
    }

    private void EmitThrow(ThrowStatementSyntax throwStmt)
    {
        if (throwStmt.Expression is null)
        {
            _w.WriteLine("throw $ex;"); // rethrow inside catch
            return;
        }
        _w.Write("throw ");
        EmitExpression(throwStmt.Expression);
        _w.WriteLine(";");
    }

    private void EmitTry(TryStatementSyntax tryStmt)
    {
        _w.Write("try ");
        EmitBlock(tryStmt.Block);

        if (tryStmt.Catches.Count > 0)
        {
            _w.WriteLine("catch ($ex) {");
            _w.Indent();

            // Bind each catch variable to $ex up front so it is in scope for exception
            // filters (`when (...)`), which are evaluated in the guard before the body.
            var boundNames = new System.Collections.Generic.HashSet<string>(System.StringComparer.Ordinal);
            foreach (var katch in tryStmt.Catches)
            {
                var id = katch.Declaration?.Identifier;
                if (id is { RawKind: not 0 } token && !string.IsNullOrEmpty(token.Text))
                {
                    var jsName = NameMangler.JsIdentifier(token.Text);
                    if (boundNames.Add(jsName)) _w.WriteLine($"let {jsName} = $ex;");
                }
            }

            var first = true;
            var hasCatchAll = false;
            foreach (var katch in tryStmt.Catches)
            {
                var typeSyntax = katch.Declaration?.Type;
                var exType = typeSyntax is not null ? _model.GetTypeInfo(typeSyntax).Type : null;
                var isCatchAll = exType is null || exType.SpecialType == SpecialType.System_Object
                    || exType.ToDisplayString() == "System.Exception";

                var condition = isCatchAll ? null : $"H5R.is($ex, {ExceptionTypeRef(exType!)})";
                if (katch.Filter is not null)
                {
                    // exception filter appended
                }

                if (condition is null && katch.Filter is null)
                {
                    hasCatchAll = true;
                    if (first)
                    {
                        // Only/first clause with no type filter: body runs unconditionally.
                        EmitCatchBody(katch, null);
                    }
                    else
                    {
                        _w.Write("else ");
                        EmitCatchBodyBlock(katch);
                    }
                }
                else
                {
                    // A filter-only catch (catch (Exception) when (...)) still needs a guard.
                    condition ??= "true";
                    _w.Write(first ? "if (" : "else if (");
                    _w.Write(condition);
                    if (katch.Filter is not null)
                    {
                        _w.Write(" && (");
                        EmitExpression(katch.Filter.FilterExpression);
                        _w.Write(")");
                    }
                    _w.Write(") ");
                    EmitCatchBodyBlock(katch);
                }
                first = false;
            }
            if (!hasCatchAll)
            {
                _w.WriteLine("else { throw $ex; }");
            }
            _w.Outdent();
            _w.WriteLine("}");
        }

        if (tryStmt.Finally is not null)
        {
            _w.Write("finally ");
            EmitBlock(tryStmt.Finally.Block);
        }
    }

    private void EmitCatchBody(CatchClauseSyntax katch, string? prefix)
    {
        // The catch variable is bound once at the top of the catch block (see EmitTry).
        foreach (var s in katch.Block.Statements) EmitStatement(s);
    }

    private void EmitCatchBodyBlock(CatchClauseSyntax katch)
    {
        _w.Block(() =>
        {
            foreach (var s in katch.Block.Statements) EmitStatement(s);
        });
        _w.WriteLine();
    }

    private string ExceptionTypeRef(ITypeSymbol type) => TypeRef(type);

    private void EmitUsing(UsingStatementSyntax usingStmt)
    {
        // using (var x = expr) body  =>  { let x = expr; try { body } finally { H5R.dispose(x); } }
        _w.Block(() =>
        {
            string? resourceVar = null;
            if (usingStmt.Declaration is not null)
            {
                foreach (var v in usingStmt.Declaration.Variables)
                {
                    resourceVar = NameMangler.JsIdentifier(v.Identifier.Text);
                    _w.Write($"let {resourceVar} = ");
                    if (v.Initializer is not null) EmitExpression(v.Initializer.Value); else _w.Write("null");
                    _w.WriteLine(";");
                }
            }
            else if (usingStmt.Expression is not null)
            {
                resourceVar = "$using" + Math.Abs(usingStmt.GetHashCode() % 10000);
                _w.Write($"let {resourceVar} = ");
                EmitExpression(usingStmt.Expression);
                _w.WriteLine(";");
            }

            _w.Write("try ");
            EmitStatementAsBlock(usingStmt.Statement);
            _w.Write("finally ");
            _w.Block(() => _w.WriteLine($"H5R.dispose({resourceVar});"));
            _w.WriteLine();
        });
        _w.WriteLine();
    }

    private void EmitUsingDeclaration(LocalDeclarationStatementSyntax local)
    {
        // using var x = expr;  — dispose at end of enclosing block. Simplified: declare now.
        foreach (var variable in local.Declaration.Variables)
        {
            _w.Write($"let {NameMangler.JsIdentifier(variable.Identifier.Text)}");
            if (variable.Initializer is not null) { _w.Write(" = "); EmitExpression(variable.Initializer.Value); }
            _w.WriteLine(";");
        }
        // Note: deterministic dispose for using-declarations is a later phase.
    }

    private void EmitSwitch(SwitchStatementSyntax switchStmt)
    {
        // Pattern-based switch → if/else-if chain over a temp subject.
        var hasPatterns = switchStmt.Sections
            .SelectMany(s => s.Labels)
            .Any(l => l is CasePatternSwitchLabelSyntax);

        if (hasPatterns)
        {
            EmitPatternSwitch(switchStmt);
            return;
        }

        _w.Write("switch (");
        EmitExpression(switchStmt.Expression);
        _w.WriteLine(") {");
        _w.Indent();
        _breakTargets.Push(null);
        foreach (var section in switchStmt.Sections)
        {
            foreach (var label in section.Labels)
            {
                switch (label)
                {
                    case CaseSwitchLabelSyntax caseLabel:
                        _w.Write("case ");
                        EmitExpression(caseLabel.Value);
                        _w.WriteLine(":");
                        break;
                    case DefaultSwitchLabelSyntax:
                        _w.WriteLine("default:");
                        break;
                    default:
                        Unsupported(label, "pattern switch label");
                        break;
                }
            }
            _w.Indent();
            foreach (var stmt in section.Statements) EmitStatement(stmt);
            _w.Outdent();
        }
        _breakTargets.Pop();
        _w.Outdent();
        _w.WriteLine("}");
    }

    private void EmitPatternSwitch(SwitchStatementSyntax switchStmt)
    {
        var label = NextTemp("$switch");
        var subject = NextTemp("$subj");

        _w.WriteLine($"{label}: {{");
        _w.Indent();
        _w.Write($"let {subject} = ");
        EmitExpression(switchStmt.Expression);
        _w.WriteLine(";");

        _breakTargets.Push(label);

        SwitchSectionSyntax? defaultSection = null;
        var first = true;

        foreach (var section in switchStmt.Sections)
        {
            if (section.Labels.Any(l => l is DefaultSwitchLabelSyntax))
            {
                defaultSection = section;
                continue;
            }

            _w.Write(first ? "if (" : "else if (");
            first = false;

            for (var i = 0; i < section.Labels.Count; i++)
            {
                if (i > 0) _w.Write(" || ");
                _w.Write("(");
                switch (section.Labels[i])
                {
                    case CasePatternSwitchLabelSyntax patternLabel:
                        EmitPatternTest(subject, patternLabel.Pattern);
                        if (patternLabel.WhenClause is not null)
                        {
                            _w.Write(" && (");
                            EmitExpression(patternLabel.WhenClause.Condition);
                            _w.Write(")");
                        }
                        break;
                    case CaseSwitchLabelSyntax constLabel:
                        _w.Write($"{subject} === ");
                        EmitExpression(constLabel.Value);
                        break;
                }
                _w.Write(")");
            }

            _w.Write(") ");
            _w.Block(() => { foreach (var stmt in section.Statements) EmitStatement(stmt); });
            _w.WriteLine();
        }

        if (defaultSection is not null)
        {
            _w.Write(first ? "" : "else ");
            _w.Block(() => { foreach (var stmt in defaultSection.Statements) EmitStatement(stmt); });
            _w.WriteLine();
        }

        _breakTargets.Pop();
        _w.Outdent();
        _w.WriteLine("}");
    }

    private void EmitLocalFunction(LocalFunctionStatementSyntax localFn)
    {
        var symbol = _model.GetDeclaredSymbol(localFn) as IMethodSymbol;
        var isAsync = localFn.Modifiers.Any(SyntaxKind.AsyncKeyword);
        // Arrow function so `this` is captured lexically (C# local functions close over `this`);
        // the `var` binding keeps the name in scope for recursion.
        _w.Write($"var {NameMangler.JsIdentifier(localFn.Identifier.Text)} = (");
        if (symbol is not null) EmitParameterList(symbol);
        _w.Write(") => ");
        _w.Block(() =>
        {
            if (symbol is not null) EmitOptionalDefaults(symbol);
            EmitMaybeAsyncBody(isAsync, () =>
            {
                if (localFn.Body is not null)
                {
                    EmitStatements(localFn.Body.Statements);
                }
                else if (localFn.ExpressionBody is not null)
                {
                    if (symbol?.ReturnsVoid == true) EmitExpressionStatement(localFn.ExpressionBody.Expression);
                    else { _w.Write("return "); EmitExpression(localFn.ExpressionBody.Expression); _w.WriteLine(";"); }
                }
            });
        });
        _w.WriteLine(";");
    }
}
