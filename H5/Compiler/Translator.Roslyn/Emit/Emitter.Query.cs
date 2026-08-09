using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace H5.Translator.Roslyn;

public sealed partial class Emitter
{
    /// <summary>
    /// Translates a LINQ query expression into the h5.js Enumerable chain
    /// (<c>System.Linq.Enumerable.from(src).where(fn).orderBy(fn).select(fn)</c>) — the same
    /// runtime API the method-syntax <c>[Template]</c>s target. Supports single-source
    /// from/where/orderby/select/group by; multi-from, let, and join are reported unsupported.
    /// </summary>
    private void EmitQuery(QueryExpressionSyntax query)
    {
        Action emit = () =>
        {
            _w.Write("System.Linq.Enumerable.from(");
            EmitExpression(query.FromClause.Expression);
            _w.Write(")");
        };
        EmitQueryBody(emit, NameMangler.JsIdentifier(query.FromClause.Identifier.Text), query.Body);
    }

    private void EmitQueryBody(Action emitSource, string range, QueryBodySyntax body)
    {
        var emit = emitSource;

        foreach (var clause in body.Clauses)
        {
            var inner = emit;
            switch (clause)
            {
                case WhereClauseSyntax where:
                    emit = () =>
                    {
                        inner();
                        _w.Write($".where(function ({range}) {{ return ");
                        EmitExpression(where.Condition);
                        _w.Write("; })");
                    };
                    break;

                case OrderByClauseSyntax orderBy:
                    emit = () => EmitOrderBy(inner, range, orderBy);
                    break;

                default:
                    Unsupported(clause, $"query clause {clause.Kind()} (use method syntax)");
                    break;
            }
        }

        // Produce the select/group result sequence.
        Action result;
        switch (body.SelectOrGroup)
        {
            case SelectClauseSyntax select when select.Expression is IdentifierNameSyntax sid && sid.Identifier.Text == range:
                result = emit; // identity projection — the sequence is already the result
                break;
            case SelectClauseSyntax select:
                {
                    var inner = emit;
                    result = () =>
                    {
                        inner();
                        _w.Write($".select(function ({range}) {{ return ");
                        EmitExpression(select.Expression);
                        _w.Write("; })");
                    };
                    break;
                }
            case GroupClauseSyntax group:
                {
                    var inner = emit;
                    result = () =>
                    {
                        inner();
                        _w.Write($".groupBy(function ({range}) {{ return ");
                        EmitExpression(group.ByExpression);
                        _w.Write("; }");
                        if (!(group.GroupExpression is IdentifierNameSyntax gid && gid.Identifier.Text == range))
                        {
                            _w.Write($", function ({range}) {{ return ");
                            EmitExpression(group.GroupExpression);
                            _w.Write("; }");
                        }
                        _w.Write(")");
                    };
                    break;
                }
            default:
                Unsupported(body.SelectOrGroup, "query body");
                return;
        }

        // `into` continuation: the result becomes the source of a new query body.
        if (body.Continuation is not null)
        {
            EmitQueryBody(result, NameMangler.JsIdentifier(body.Continuation.Identifier.Text), body.Continuation.Body);
        }
        else
        {
            result();
        }
    }

    private void EmitOrderBy(Action inner, string range, OrderByClauseSyntax orderBy)
    {
        inner();
        for (var i = 0; i < orderBy.Orderings.Count; i++)
        {
            var descending = orderBy.Orderings[i].IsKind(SyntaxKind.DescendingOrdering);
            var method = i == 0
                ? (descending ? "orderByDescending" : "orderBy")
                : (descending ? "thenByDescending" : "thenBy");
            _w.Write($".{method}(function ({range}) {{ return ");
            EmitExpression(orderBy.Orderings[i].Expression);
            _w.Write("; })");
        }
    }
}
