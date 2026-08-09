# Ported H5 integration tests — status

All **340** existing H5 integration test methods (from `Tests/H5.Compiler.IntegrationTests`)
are ported here into the `H5.Translator.Roslyn.Tests.Ported` namespace, running the
**new Roslyn translator** against the **real h5.js runtime** (extracted from H5.dll) and
diffing output against native .NET — the same contract as the original suite.

## Current results

- **303 passing**, 20 failing, **17 skipped** (`WebApiTests` — need the h5.core browser/DOM
  bindings, out of scope for a runtime-only harness).

Explicit **and** implicit interface dispatch is implemented: a member accessed through a
source interface routes to H5's mangled interface slot (`Namespace$IFace$member`); explicit
implementations are emitted under that slot, and implicit implementations publish an
`alias` mapping their plain slot to it (so both resolve at runtime). BCL interfaces keep
plain-name access, since their h5.js implementers expose the member directly.

Fixed since the re-target: LINQ/extension templates, enum values/ToString, `[Flags]` enums,
relative external templates, non-generic/`[External]` BCL `new`, local functions,
`throw`/`checked`, user `ToString`/`Equals`/`GetHashCode` naming, external base-ctor naming,
user indexers (`getItem`/`setItem`) and named indexers (`[Name]`/`[AccessorsIndexer]`),
deconstruction, records, struct `$clone`/`getDefaultValue`, `nameof`, out/ref args in
`[Template]` calls, base instance calls (`.prototype`), covariant returns, catch-clause
ordering, events + multicast delegates, property/indexer setter `[Template]`s, discards
(assignment/`out _`/lambda/deconstruction), nullable `.Value`, named-argument defaults,
optional lambda parameters, 32-bit integer multiply wrap (`Math.imul`), member-level
`[Convention]` (e.g. `KeyValuePair.Key`), `GetType()` (`{this:type}`/`<self>`), null-conditional
element access + property templates, switch-expression pattern variables, `using var`
block-scoped dispose, C#12 collection expressions, C#12 primary constructors (with capture
analysis), C#14 `field` keyword, `[ModuleInitializer]`, `Index`/`Range` on arrays (`^n`,
`a..b`), `H5.Script.Write` raw-JS interop, `[ObjectLiteral]`, universal
`ToString`/`GetHashCode`/`Equals` lowercasing for BCL types, and rejecting top-level
statements / global usings as unsupported.

The translator emits H5-runtime-format code (`H5.assembly` + `H5.define`) and drives BCL
interop through H5's `[Template]`/`[Name]`/`[External]`/`[Convention]` attributes read from
H5.dll, so it composes with the real runtime.

The translator now ports H5's exact `OverloadsCollection` ordering for method overload
suffixes, reads type- and member-level `[Convention]`, honours `[Name]`/`[Template]`/
`[External]`, and emits universal `toString`/`getHashCode`/`equals` names — so most member
naming now matches h5.js.

64-bit integers (`long`/`ulong`) and `decimal` are emitted as h5.js `System.Int64`/`UInt64`/
`System.Decimal` objects (literals, arithmetic/comparison operators, conversions, and
constants such as `long.MinValue`/`decimal.MaxValue`). `goto`/labels lower to a
label-dispatch state machine (works across `await`). Lifted nullable operators propagate
null. C#12 generic classes/interfaces thread their type parameters at runtime
(`Factory$1(Item)`, `new T()` → `H5.createInstance(T)`). LINQ query syntax lowers to the
h5.js `Enumerable.from(src).where(...).select(...)` chain. C#11 list patterns match
arrays; foreach honours an extension `GetEnumerator`.

Async constructs are aligned with h5.js's contract: an `async` method/lambda/local
function emits a plain outer function whose body runs in a native `async` IIFE, and the
resulting promise is adapted to an h5.js **Task** via `H5R.fromPromise` (a
`TaskCompletionSource`). So async methods return real Tasks that compose with
`Task.Run`/`Task.WhenAll`/`ContinueWith` and carry faults through the Task (enabling
exception aggregation), while `await x` drives any Task or promise through `H5.toPromise`.
`async ValueTask` is emitted identically to `Task` (the H5.dll-only task-like-metadata
errors are suppressed, since they don't arise in the real-BCL native comparison).

Hand-written BCL runtime quirks are now handled by porting H5's actual rules rather than
per-type maps: an `extern` (body-less) method on a non-external type is left out of the
overload set (so `Regex.Replace` → `replace`, no suffix), the parameterless object
`ToString` still occupies overload slot 0 (so `Version.ToString(int)` → `toString$1`),
`[Enum(Emit.X)]` value/string-name modes are honoured (RegexOptions, `[Enum]` test),
`[Template]` type-parameter tokens (`{T}`) are substituted with the call-site type argument
(`Comparer<T>`/`EqualityComparer<T>` defaults), and a method-level `[Convention]` is read
(e.g. `IComparer<T>.Compare` → `compare`). Guid, Regex, Version, CultureInfo and Comparer
now match h5.js. Exception filters (`when`) bind the catch variable before the guard runs.

## Remaining failure categories (long tail, ~21)

1. **Generic method type arguments** — a *source* generic method threads its type
   parameters (`typeof(T)`/`default(T)`/casts to `T` work). Still open: `new T()` on a
   *method* type parameter, and BCL/external generic methods that expect threaded type args.
2. **Reflection metadata** — the `H5.setMetadata` block is not emitted, so richer
   `GetType()` details differ.
3. **Newer/edge C# forms** — C#14 `extension` members and ref-lambda params,
   null-conditional assignment, `params List<T>`/`Span<T>` (C#13), multi-dimensional-array
   indexing, `System.Threading.Lock`, `nint`/`nuint`, `checked` overflow throwing,
   and the `[ObjectLiteral]` Ignore/Initializer modes.
4. **File I/O** — `MemoryStream`/`BinaryWriter` (largely reported unsupported by design).

These are the same kinds of features the legacy emitter handles via its metadata/overload
machinery; porting them incrementally (each with its mirrored test) is the path to parity.
