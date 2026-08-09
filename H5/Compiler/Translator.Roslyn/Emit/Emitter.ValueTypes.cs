using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace H5.Translator.Roslyn;

public sealed partial class Emitter
{
    /// <summary>Record positional properties, exposed as fields named after the parameter.</summary>
    private IEnumerable<IPropertySymbol> RecordPositionalProps(INamedTypeSymbol type)
        => type.IsRecord
            ? type.GetMembers().OfType<IPropertySymbol>()
                .Where(p => !p.IsStatic && !p.IsIndexer && p.IsImplicitlyDeclared && p.Name != "EqualityContract")
            : Enumerable.Empty<IPropertySymbol>();

    /// <summary>All instance properties that participate in a record's value equality / ToString.</summary>
    private List<string> RecordValuePropNames(INamedTypeSymbol type)
        => type.GetMembers().OfType<IPropertySymbol>()
            .Where(p => !p.IsStatic && p.GetMethod is not null && !p.IsIndexer && p.Name != "EqualityContract")
            .Select(p => H5Naming.MemberJsName(p))
            .Distinct()
            .ToList();

    /// <summary>Appends synthesized value-type methods (struct $clone/equals, record members).</summary>
    private void AddValueTypeMethodEntries(INamedTypeSymbol type, List<Action> entries)
    {
        // Everything a value-copy must carry: backing fields/auto-props plus (for record
        // structs) the positional value properties, which are synthesized without slots.
        var fields = InstanceFieldSlots(type).Select(f => f.name).ToList();
        if (type.IsRecord)
            foreach (var p in RecordValuePropNames(type))
                if (!fields.Contains(p)) fields.Add(p);

        if (type.TypeKind == TypeKind.Struct)
        {
            entries.Add(() =>
            {
                _w.Write("$clone: function (to) ");
                _w.Block(() =>
                {
                    _w.WriteLine($"var s = to || new {TypeRef(type)}();");
                    foreach (var f in fields) _w.WriteLine($"s.{f} = this.{f};");
                    _w.WriteLine("return s;");
                });
            });
        }

        if (type.IsRecord)
        {
            var props = RecordValuePropNames(type);
            entries.Add(() =>
            {
                _w.Write("toString: function () ");
                _w.Block(() =>
                {
                    if (props.Count == 0) { _w.WriteLine($"return \"{type.Name} {{ }}\";"); return; }
                    var parts = string.Join(" + \", \" + ", props.Select(p => $"\"{p} = \" + H5R.toStr(this.{p})"));
                    _w.WriteLine($"return \"{type.Name} {{ \" + {parts} + \" }}\";");
                });
            });
            entries.Add(() =>
            {
                _w.Write("equals: function (o) ");
                _w.Block(() =>
                {
                    _w.WriteLine("if (o == null || o.constructor !== this.constructor) { return false; }");
                    _w.Write("return ");
                    _w.Write(props.Count == 0 ? "true" : string.Join(" && ", props.Select(p => $"H5R.equals(this.{p}, o.{p})")));
                    _w.WriteLine(";");
                });
            });
            entries.Add(() =>
            {
                _w.Write("getHashCode: function () ");
                _w.Block(() =>
                {
                    _w.WriteLine("var h = 17;");
                    foreach (var p in props) _w.WriteLine($"h = (h * 31 + H5R.hash(this.{p})) | 0;");
                    _w.WriteLine("return h;");
                });
            });

            var positional = RecordPositionalProps(type).Select(p => H5Naming.MemberJsName(p)).ToList();
            if (positional.Count > 0)
            {
                entries.Add(() =>
                {
                    var holders = positional.Select((_, i) => "$p" + i).ToList();
                    _w.Write($"Deconstruct: function ({string.Join(", ", holders)}) ");
                    _w.Block(() =>
                    {
                        for (var i = 0; i < positional.Count; i++)
                            _w.WriteLine($"{holders[i]}.v = this.{positional[i]};");
                    });
                });
            }
        }
    }

    /// <summary>Emits a record's synthesized primary constructor (sets positional fields).</summary>
    private bool TryEmitRecordCtors(INamedTypeSymbol type)
    {
        if (!type.IsRecord) return false;
        var recordDecl = type.DeclaringSyntaxReferences.Select(r => r.GetSyntax()).OfType<RecordDeclarationSyntax>().FirstOrDefault();
        var positional = recordDecl?.ParameterList?.Parameters.Select(p => NameMangler.JsIdentifier(p.Identifier.Text)).ToList() ?? new List<string>();

        // Only user-written constructors (with real ConstructorDeclarationSyntax); the
        // primary constructor lives on the record header and is emitted as "ctor" above.
        var explicitCtors = type.InstanceConstructors
            .Where(c => c.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is ConstructorDeclarationSyntax)
            .ToList();

        _w.Block(() =>
        {
            // primary ctor
            _w.Write($"ctor: function ({string.Join(", ", positional)}) ");
            _w.Block(() =>
            {
                _w.WriteLine("this.$initialize();");
                var baseType = type.BaseType;
                if (baseType is { } bt && bt.SpecialType != SpecialType.System_Object && bt.TypeKind != TypeKind.Error && bt.IsRecord)
                    _w.WriteLine($"{TypeRef(bt)}.ctor.call(this);");
                foreach (var p in positional) _w.WriteLine($"this.{p} = {p};");
            });
            _w.WriteLine(explicitCtors.Count > 0 ? "," : "");
            // explicit ctors kept as $ctorN
            for (var i = 0; i < explicitCtors.Count; i++)
            {
                var decl = explicitCtors[i].DeclaringSyntaxReferences[0].GetSyntax() as ConstructorDeclarationSyntax;
                _w.Write($"{CtorName(explicitCtors[i])}: function (");
                EmitParameterList(explicitCtors[i]);
                _w.Write(") ");
                _w.Block(() =>
                {
                    _w.WriteLine("this.$initialize();");
                    if (decl?.Body is not null) foreach (var s in decl.Body.Statements) EmitStatement(s);
                });
                _w.WriteLine(i < explicitCtors.Count - 1 ? "," : "");
            }
        });
        return true;
    }
}
