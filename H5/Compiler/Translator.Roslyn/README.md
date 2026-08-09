# H5.Translator.Roslyn

A clean-room, **Roslyn-only** C# → JavaScript translator for H5. Unlike the legacy
pipeline (`H5/Compiler/Translator`), it uses **no NRefactory** and **no `SharpSixRewriter`
lowering pass**: it compiles and parses with Roslyn, walks the syntax tree guided by
the semantic model, and emits JavaScript directly.

See [`../../../H5.Translator.Roslyn.PORT_PLAN.md`](../../../H5.Translator.Roslyn.PORT_PLAN.md)
for the full design and the feature-by-feature roadmap.

## Pipeline

```
sources ──► CompilationBuilder ──► CSharpCompilation + SemanticModel
                                         │
                                         ├─► Roslyn errors ──────────► fail
                                         ├─► UnsupportedFeatureScanner ► fail (browser-incompatible)
                                         └─► Emitter (syntax walk) ────► JavaScript
                                                                         (+ embedded runtime)
```

Entry point: `RoslynTranslator.Translate(source)` → `TranslationResult { Javascript, Diagnostics, Success }`.

## Layout

| File / folder | Responsibility |
|---|---|
| `Compilation/RoslynTranslator.cs` | Public entry point; orchestrates the pipeline. |
| `Compilation/CompilationBuilder.cs` | Builds the `CSharpCompilation` (C# Latest, ref assemblies). |
| `Support/UnsupportedFeatureScanner.cs` | Reports browser-incompatible features as errors (H5R0001). |
| `Support/NameMangler.cs` | C# symbol → safe JS name; overload disambiguation. |
| `Emit/Emitter*.cs` | Syntax-tree walking emitter (types, members, statements, expressions). |
| `Emit/JsWriter.cs` | Indentation-aware output writer. |
| `Runtime/h5roslyn.runtime.js` | Embedded minimal JS runtime (Console, formatting, helpers). |

## Currently implemented

- Classes / structs, inheritance (`virtual`/`override`/`abstract`), interfaces (structural).
- Instance & static fields, auto- and full properties, indexers, methods, overloads.
- Constructors: overloads, `: this(...)` / `: base(...)` chaining, field initializers, static ctors.
- Statements: locals, `if/else`, `for`, `foreach`, `while`, `do`, `switch` (constant + pattern),
  `try/catch/finally`, `using`, `throw`, `break`/`continue`/`return`, `lock` (no-op body), local functions.
- Expressions: literals, arithmetic/relational/logical/bitwise ops, integer division, string concat &
  interpolation, ternary, null-coalescing, casts (numeric truncation), `is`/`as`, object & collection
  initializers, arrays, element access, lambdas / anonymous methods, `?.`, `await`, method groups, enums,
  `ref`/`out` parameters (holder objects), `params`, named/optional arguments.
- Modern C#: pattern matching (type/constant/relational/logical/property/positional), switch expressions,
  tuples + deconstruction, records (positional, value equality, `with`, `Deconstruct`, ToString).
- Collections: `List<T>`, `Dictionary<K,V>`, `HashSet<T>`, arrays; LINQ-to-objects; iterators (`yield`).
- BCL surface: `Console`, `String` (+ static), `StringBuilder`, `Math`/`MathF`, `Convert`, numeric parsing,
  `Char`, `Task`/`Task<T>` (→ Promise), `TaskCompletionSource`, exceptions with `Message`/`InnerException`.
- Async/await → native JS `async`/`await`; async `Main` bootstrap.
- Unsupported-feature reporting: pointers, `unsafe`, `fixed`, `stackalloc`, P/Invoke, File I/O, sockets,
  threading primitives (Task-based async is allowed).

## Extending

1. Add a language feature by handling its `SyntaxNode` in the relevant `Emitter.*.cs` file.
2. Grow `h5roslyn.runtime.js` only as much as the feature needs (keep it minimal).
3. Add a mirrored test in `Tests/H5.Translator.Roslyn.Tests/` — it diffs JS output against
   native Roslyn execution, so behavior parity is enforced automatically.
4. If a feature can't run in a browser, report it from `UnsupportedFeatureScanner` instead of emitting.
