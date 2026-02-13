# Pending Integration Tests (C# <= 7.2)

This file lists the C# language features and core library types that need integration tests.
Each test must demonstrate side-effects (e.g., `Console.WriteLine`) to be verifiable.

## C# 1.0 - 2.0: Core Language Features

### Basic Types & Operations
- [x] **Integers**: `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong` (arithmetic, bitwise, overflow checking `checked`/`unchecked` where applicable). Reference: `H5/H5/System/Int32.cs`, etc.
- [x] **Floating Point**: `float`, `double`, `decimal` (precision, rounding, special values like NaN/Infinity). Reference: `H5/H5/System/Double.cs`, `Decimal.cs`.
- [x] **Booleans**: `bool` (logical operators, short-circuiting). Reference: `H5/H5/System/Boolean.cs`.
- [x] **Characters & Strings**: `char` operations, `string` concatenation, indexing, comparison, escaping sequences. Reference: `H5/H5/System/String.cs`.
- [x] **Enumerations**: `enum` definition, underlying types, flags attribute, conversion to/from int/string. Reference: `H5/H5/System/Enum.cs`.

### Arrays & Collections
- [x] **Single-dimensional Arrays**: Creation, indexing, length, iteration (`foreach`). Reference: `H5/H5/System/Array.cs`.
- [x] **Multi-dimensional Arrays**: Rectangular arrays (`[,]`), nested loops access.
- [x] **Jagged Arrays**: Arrays of arrays (`[][]`), initialization, access.

### Control Flow
- [x] **Conditionals**: `if`, `else if`, `else`, nested `if`.
- [x] **Switch**: `switch` on integral types, strings, enums; `default` case; fall-through behavior (should be error in C# unless empty).
- [x] **Loops**: `for`, `foreach` (arrays, collections), `while`, `do-while`.
- [x] **Jumps**: `break`, `continue`, `return`, `goto` (basic label usage).
- [x] **Deeply nested functions**: 20 layers of mixed constructs (local functions, lambdas, nested classes, async/await).

### Object-Oriented Programming
- [x] **Classes**: Fields, Methods (instance/static), Constructors (default/parameterized), `this` keyword. Reference: `H5/H5/System/Object.cs`.
- [x] **Inheritance**: Base classes, `virtual`/`override`/`abstract`/`sealed` methods, polymorphism, `base` keyword.
- [x] **Interfaces**: Definition, implementation (implicit/explicit), interface inheritance.
- [x] **Structs**: Value semantics, constructors, methods. Reference: `H5/H5/System/ValueType.cs`.
- [x] **Properties**: Getters, Setters, computed properties.
- [x] **Indexers**: `this[...]` implementation and usage.
- [x] **Events**: Declaration, subscription (`+=`), unsubscription (`-=`), invocation. Reference: `H5/H5/System/Delegate.cs`.
- [x] **Delegates**: Definition, instantiation, invocation, multicast delegates. Reference: `H5/H5/System/Delegate.cs`.
- [x] **Operators**: Overloading binary/unary operators, implicit/explicit conversions.

### C# 2.0 Specifics
- [x] **Generics**: Generic classes (`Class<T>`), methods (`Method<T>`), interfaces, constraints (`where T : class, new()`, etc.).
- [x] **Nullable Types**: `int?`, `bool?`, `HasValue`, `Value`, null coalescing (`??`). Reference: `H5/H5/System/Nullable.cs`.
- [x] **Anonymous Methods**: `delegate { ... }` syntax.
- [x] **Iterators**: `yield return`, `yield break` in methods returning `IEnumerable` or `IEnumerator`. Reference: `H5/H5/System/Collections/IEnumerator.cs`.
- [x] **Static Classes**: Classes declared as `static` with only static members.
- [x] **Partial Types**: Partial classes (compiler feature).

### Exceptions
- [x] **Try-Catch-Finally**: `try`, `catch` (specific/general), `finally` blocks.
- [x] **Throw**: `throw new Exception("...")`.
- [x] **Re-throw**: `throw;` inside catch block.
- [x] **Custom Exceptions**: Inheriting from `System.Exception`.
- [x] **Properties**: `Message`, `InnerException`, `StackTrace` (basic verification).

## C# 3.0: LINQ & Functional Features

- [x] **Auto-Implemented Properties**: `public int Prop { get; set; }`.
- [x] **Object Initializers**: `new Person { Name = "X", Age = 10 }`.
- [x] **Collection Initializers**: `new List<int> { 1, 2, 3 }`.
- [x] **Anonymous Types**: `new { Name = "X", Value = 1 }`.
- [x] **Implicitly Typed Locals**: `var x = 10;`.
- [x] **Extension Methods**: Defining and calling extension methods on types. Reference: `H5/H5/System/Runtime/CompilerServices/ExtensionAttribute.cs`.
- [x] **Lambda Expressions**: `x => x + 1`, `(x, y) => { return x + y; }`.
- [x] **LINQ to Objects**: `Where`, `Select`, `OrderBy`, `GroupBy`, `Any`, `All`, `First`, `FirstOrDefault`. Reference: `H5/H5/System/Linq/Enumerable.cs`.
- [x] **Query Expressions**: `from x in list where x > 5 select x`.

## C# 4.0: Dynamic & Named Arguments

- [x] **Named Arguments**: `Method(arg2: 10, arg1: 5)`.
- [x] **Optional Arguments**: `void Method(int x = 10)`.
- [x] **Dynamic Binding**: Basic usage of `dynamic` (if supported by H5 runtime, otherwise skip or verify compilation error/runtime behavior). Note: Might rely on `System.Dynamic` support.

## C# 5.0: Async/Await

- [x] **Async/Await**: `async Task Method()`, `await Task.Delay()`, `await Task.WhenAll()`, `Task.Run()`, `TaskCompletionSource`. Reference: `H5/H5/System/Threading/Tasks/Task.cs`.
- [x] **Task<T>**: Returning values from async methods.
- [ ] **Caller Info Attributes**: `[CallerMemberName]`, `[CallerFilePath]`, `[CallerLineNumber]`.

## C# 6.0: Syntactic Sugar

- [x] **String Interpolation**: `$"Value: {x}"`, formatting `$"Date: {d:yyyy-MM-dd}"`.
- [x] **Null-Conditional Operator**: `obj?.Property`, `list?[0]`.
- [x] **Expression-Bodied Members**: Methods `=>`, Properties `=>`.
- [x] **Exception Filters**: `catch (Exception ex) when (ex.Message == "X")`. (Note: Property access in filter is buggy, see FINDINGS.md)
- [x] **Nameof Operator**: `nameof(variable)`, `nameof(Class.Property)`.
- [x] **Auto-Property Initializers**: `public int Prop { get; set; } = 10;`.
- [x] **Getter-Only Auto-Properties**: `public int Prop { get; } = 10;`.
- [x] **Index Initializers**: `new Dictionary<int, string> { [1] = "One" }`.
- [x] **Static Imports**: `using static System.Math;` then `Sqrt(4)`.

## C# 7.0 / 7.1 / 7.2: Modern Features

- [x] **Out Variables**: `int.TryParse("123", out int result);`.
- [x] **Tuples (ValueTuple)**: `(int, string) t = (1, "A");`, naming elements `(Val: 1, Name: "A")`. Reference: `H5/H5/System/ValueTuple.cs`.
- [x] **Deconstruction**: `(var x, var y) = t;`.
- [x] **Pattern Matching**: `if (obj is int i)`, `switch` with type patterns and `when` clauses.
- [x] **Local Functions**: Defining functions inside methods.
- [x] **Discards**: `_ = Method();`.
- [x] **Literal Improvements**: Binary literals `0b101`, Digit separators `1_000`.
- [x] **Throw Expressions**: `int x = val ?? throw new Exception();`.
- [x] **Default Literal**: `int x = default;`.
- [x] **Private Protected**: `private protected` accessibility modifier.
- [x] **Non-Trailing Named Arguments**: `Method(1, arg2: 2)`.
- [x] **In Parameters**: `void Method(in int x)` (verify basic behavior).
- [x] **ReadOnly Structs**: `readonly struct Point { ... }`.
- [ ] **Ref Returns and Locals**: `ref int Method()`, `ref var x = ref y;`.

## C# 7.3: Refinements

- [x] **StackAlloc Initializers**: `Span<int> x = stackalloc[] { 1, 2, 3 };`. (Placeholder test)
- [x] **ignored** **In Method Overload Resolution**: Tiebreaker for `in` parameters.
- [x] **Attributes on Backing Fields**: `[field: NonSerialized] public int X { get; set; }`.
- [x] **ignored** **Fixed Sized Buffers**: Indexing without pinning.
- [x] **Expression Variables**: In initializers and queries.
- [x] **ignored** **Unmanaged Constraint**: `where T : unmanaged`.
- [x] **ignored** **Delegate Constraint**: `where T : Delegate`.
- [x] **ignored** **Enum Constraint**: `where T : Enum`.

## C# 8.0: Major Features

- [x] **ignored** **Readonly Members**: `public readonly int Method()`.
- [x] **ignored** **Default Interface Methods**: Implementation in interfaces.
- [x] **ignored** **Pattern Matching Enhancements**:
    - [x] **ignored** **Switch Expressions**: `x switch { ... }`.
    - [x] **ignored** **Property Patterns**: `{ P: 1 }`.
    - [x] **ignored** **Tuple Patterns**: `(1, 2)`.
    - [x] **ignored** **Positional Patterns**: `Deconstruct` based.
- [x] **ignored** **Using Declarations**: `using var x = ...`.
- [x] **Static Local Functions**: `static void Local()`.
- [x] **ignored** **Disposable Ref Structs**: `ref struct` with `Dispose`.
- [x] **ignored** **Nullable Reference Types**: `string?`, `notnull` constraint.
- [x] **ignored** **Async Streams**: `await foreach`, `IAsyncEnumerable`.
- [x] **ignored** **Indices and Ranges**: `^1`, `1..5`.
- [x] **ignored** **Null-Coalescing Assignment**: `x ??= y`.
- [x] **ignored** **Unmanaged Constructed Types**: `struct S<T> where T : unmanaged`.
- [x] **ignored** **StackAlloc in Nested Expressions**: Inside other expressions.

## C# 9.0: Records & More

- [x] **ignored** **Records**: `public record Person(string Name);`.
- [x] **ignored** **Init Only Setters**: `public int X { get; init; }`.
- [x] **ignored** **Top-Level Statements**: `System.Console.WriteLine("Hello");`.
- [x] **ignored** **Pattern Matching Enhancements**:
    - [x] **ignored** **Type Patterns**: `is Type`.
    - [x] **ignored** **Parenthesized Patterns**: `(pattern)`.
    - [x] **ignored** **Conjunctive Patterns**: `and`.
    - [x] **ignored** **Disjunctive Patterns**: `or`.
    - [x] **ignored** **Negated Patterns**: `not`.
    - [x] **ignored** **Relational Patterns**: `< 10`.
- [x] **ignored** **Target-Typed New**: `Point p = new (1, 2);`.
- [x] **ignored** **Static Anonymous Functions**: `static x => x`.
- [x] **ignored** **Target-Typed Conditional**: `flag ? 1 : 1.0` (inferred).
- [x] **ignored** **Covariant Return Types**: Override with derived return type.
- [x] **ignored** **Extension GetEnumerator**: `foreach` on types with extension `GetEnumerator`.
- [x] **ignored** **Lambda Discard Parameters**: `(_, _) => 0`.
- [x] **ignored** **Attributes on Local Functions**: `[Attr] void Local()`.
- [x] **ignored** **Native Sized Integers**: `nint`, `nuint`.
- [x] **ignored** **Module Initializers**: `[ModuleInitializer]`.

## C# 10.0: Structs & Interpolation

- [x] **ignored** **Record Structs**: `public record struct Point(int X, int Y);`.
- [x] **ignored** **Struct Improvements**: Parameterless constructors, field initializers.
- [x] **ignored** **Global Usings**: `global using System;`.
- [x] **ignored** **File-Scoped Namespaces**: `namespace MyNamespace;`.
- [x] **ignored** **Extended Property Patterns**: `{ Prop.Child: 1 }`.
- [x] **Constant Interpolated Strings**: `const string s = $"{c}..."`.
- [x] **ignored** **Lambda Improvements**: Attributes, explicit return types.
- [x] **ignored** **Sealed ToString in Records**: `sealed override ToString()`.
- [x] **Deconstruction Mix**: `(x, var y) = ...`.
- [x] **ignored** **CallerArgumentExpression**: `[CallerArgumentExpression("p")]`.
- [x] **ignored** **AsyncMethodBuilder Attribute**: On methods.

## C# 11.0: Raw Strings & Generics

- [x] **ignored** **Raw String Literals**: `"""..."""`.
- [x] **ignored** **Generic Math**: `static abstract` interface members (syntax support).
- [x] **ignored** **Generic Attributes**: `class Attr<T> : Attribute`.
- [x] **ignored** **UTF-8 String Literals**: `"text"u8`.
- [x] **Newlines in Interpolation**: Inside `{...}`.
- [x] **ignored** **List Patterns**: `[1, .., 5]`.
- [x] **ignored** **File-Local Types**: `file class Local`.
- [x] **ignored** **Required Members**: `required public int X { get; set; }`.
- [x] **Auto-Default Structs**: Compiler sets unassigned fields to default.
- [x] **ignored** **Pattern Match Span<char>**: Constant string.
- [x] **ignored** **Extended Nameof**: `nameof` parameter in attribute.
- [x] **ignored** **Numeric IntPtr**: `nint` as alias for `System.IntPtr` (unification).
- [x] **ignored** **Ref Fields**: `ref int x` in `ref struct`.
- [x] **ignored** **Scoped Ref**: `scoped ref` parameters/locals.

## C# 12.0: Collection Expressions & Primary Constructors

- [x] **ignored** **Primary Constructors**: `class C(int x) { ... }`.
- [x] **ignored** **Collection Expressions**: `[1, 2, 3]`.
- [x] **ignored** **Inline Arrays**: `[System.Runtime.CompilerServices.InlineArray(10)]`.
- [x] **ignored** **Optional Params in Lambdas**: `(int x = 1) => x`.
- [x] **ignored** **Ref Readonly Parameters**: `ref readonly int`.
- [x] **Alias Any Type**: `using IntList = System.Collections.Generic.List<int>;`.
- [x] **ignored** **Experimental Attribute**: `[Experimental("ID")]`.

## C# 13.0: Refinements (Preview/Latest)

- [x] **ignored** **Params Collections**: `params List<int>`.
- [x] **ignored** **Lock Object**: `System.Threading.Lock`.
- [x] **ignored** **Implicit Index Access**: `^1` in object initializers.
- [x] **ignored** **Escape Sequence \e**: ESC character.

## H5 Standard Library Support (To Be Verified)

### System.Math
- [x] **Basic Arithmetic**: `Abs`, `Min`, `Max`, `Sign`, `DivRem`.
- [x] **Rounding**: `Round`, `Ceiling`, `Floor`, `Truncate`.
- [x] **Powers & Roots**: `Pow`, `Sqrt`, `Exp`, `Log`, `Log10`.
- [x] **Trigonometry**: `Sin`, `Cos`, `Tan`, `Asin`, `Acos`, `Atan`, `Atan2`.
- [x] **Hyperbolic**: `Sinh`, `Cosh`, `Tanh`.

### System.Random
- [x] **Construction**: `new Random()`, `new Random(seed)`.
- [x] **Generation**: `Next()`, `Next(max)`, `Next(min, max)`, `NextDouble()`, `NextBytes(buffer)`.

### System.DateTime & TimeSpan
- [x] **DateTime Properties**: `Now`, `UtcNow`, `Today`, `Year`, `Month`, `Day`, `Hour`, `Minute`, `Second`, `Millisecond`.
- [x] **DateTime Methods**: `AddDays`, `AddHours`, etc., `ToString()`, `ToUniversalTime()`, `ToLocalTime()`.
- [x] **TimeSpan**: Construction, `TotalMilliseconds`, `TotalDays`, etc., Arithmetic (`+`, `-`).
- [x] **Formatting & Parsing**: `DateTime.Parse`, `DateTime.TryParse` (if supported), Custom format strings.

### System.Guid
- [x] **Guid**: `Guid.NewGuid()`, `Guid.Parse()`, `ToString()`, `Guid.Empty`, Equality comparison.

### System.Text
- [x] **StringBuilder**: `Append`, `AppendLine`, `Insert`, `Remove`, `Replace`, `Clear`, `ToString`, `Length`, `Capacity`.
- [x] **RegularExpressions**: `Regex.Match`, `Regex.Matches`, `Regex.Replace`, `Regex.IsMatch`, `Regex.Split`.
- [x] **Encoding**: `Encoding.UTF8`, `Encoding.ASCII`, `GetBytes`, `GetString`.

### System.Collections.Generic
- [x] **List<T>**: `Add`, `AddRange`, `Insert`, `Remove`, `RemoveAt`, `Contains`, `IndexOf`, `Sort`, `Reverse`, `ToArray`, `BinarySearch`.
- [x] **Dictionary<TKey, TValue>**: `Add`, `Remove`, `ContainsKey`, `TryGetValue`, `Keys`, `Values`, Indexer `[]`.
- [x] **HashSet<T>**: `Add`, `Remove`, `Contains`, `UnionWith`, `IntersectWith` (if supported).
- [x] **Queue<T>**: `Enqueue`, `Dequeue`, `Peek`, `Count`.
- [x] **Stack<T>**: `Push`, `Pop`, `Peek`, `Count`.
- [x] **LinkedList<T>**: `AddFirst`, `AddLast`, `RemoveFirst`, `RemoveLast`.

### System.IO (In-Memory Only)
- [x] **MemoryStream**: `Write`, `Read`, `Seek`, `Position`, `Length`, `SetLength`, `ToArray`.
- [x] **BinaryWriter**: `Write(int)`, `Write(string)`, etc.
- [x] **BinaryReader**: `ReadInt32()`, `ReadString()`, etc.
- [x] **Stream**: Abstract base class methods (if applicable).

### System.Convert & BitConverter
- [x] **Convert**: `ToInt32`, `ToBoolean`, `ToString`, `FromBase64String`, `ToBase64String`.
- [x] **BitConverter**: `GetBytes`, `ToInt32`, `ToString`.

### System.Uri
- [x] **Uri**: Constructor, `Scheme`, `Host`, `Port`, `AbsolutePath`, `Query`, `ToString()`.

### System.Version
- [x] **Version**: Constructor, `Major`, `Minor`, `Build`, `Revision`, Comparison, `ToString()`.

### System.Globalization
- [x] **CultureInfo**: `CurrentCulture`, `InvariantCulture`, `DateTimeFormat`, `NumberFormat`.

### System.Diagnostics
- [x] **Stopwatch**: `Start`, `Stop`, `Reset`, `Restart`, `Elapsed`, `ElapsedMilliseconds`.

### System.Reflection
- [x] **Type Info**: `typeof(T).Name`, `typeof(T).FullName`, `typeof(T).IsClass`, `GetProperties()`, `GetMethods()`.
- [x] **Attributes**: `GetCustomAttributes`.

## Excluded Features (Do Not Implement)

- [ ] **Threading**: `Thread` class, `ThreadPool`, `Monitor`, `lock` statement (unless implemented via simple JS constructs, generally avoid).
- [ ] **File I/O**: `System.IO.File`, `StreamReader` (except `MemoryStream` if available).
- [ ] **Unsafe Code**: `unsafe`, `fixed`, pointers `*`.
- [ ] **Native Interop**: `DllImport`, `extern`.
- [ ] **Garbage Collection**: `GC.Collect()`, finalizers (JS GC is non-deterministic and not controllable).

## Detailed Test Scenarios (Extracted from Old Tests)

### System.DateTime
- [x] **Constructors**: `DateTime(long ticks, DateTimeKind kind)`, `DateTime(y, m, d, h, m, s, ms, DateTimeKind kind)`.
- [x] **Kind**: verify `Kind` property propagation and `SpecifyKind`.
- [x] **Arithmetic**: `AddDays` (DST transitions), `AddMonths` (end of month adjustments), `AddYears` (leap years).
- [x] **Comparison**: behavior of operators `<`, `<=`, `>`, `>=` with different `DateTimeKind`.
- [x] **Parsing**: `Parse` and `ParseExact` with standard and custom format strings (e.g., "o", "r", "u").
- [x] **Output**: `ToString` with various formats ("yyyy-MM-dd", "o", etc.).

### System.TimeSpan
- [x] **Factories**: `FromDays`, `FromHours`, `FromMinutes`, `FromSeconds`, `FromMilliseconds`, `FromTicks`.
- [x] **Arithmetic**: `Add`, `Subtract`, `Duration`, `Negate`, `+`, `-`.
- [x] **Parsing**: `Parse` with formats like "d.hh:mm:ss", "hh:mm:ss.fff".
- [x] **Properties**: `TotalDays`, `TotalHours`, etc. precision.

### System.Text.StringBuilder
- [x] **Chaining**: `sb.Append().AppendLine().Append()`.
- [x] **Capacity**: behavior when exceeding capacity, explicit capacity in constructor.
- [x] **Modification**: `Insert`, `Remove`, `Replace` (string and char), `Length` (truncating or expanding with nulls).
- [x] **Indexer**: read and write access `sb[i]`.

### System.Text.RegularExpressions
- [x] **Syntax**: Anchors (`^`, `$`, `\b`), Character Classes (`\d`, `\w`, `[a-z]`), Quantifiers (`*`, `+`, `?`, `{n,m}`), Alternation (`|`), Groups (`(...)`).
- [x] **Methods**: `IsMatch`, `Match`, `Matches`, `Replace` (string and `MatchEvaluator`), `Split`.
- [x] **Options**: `IgnoreCase`, `Multiline`, `Singleline`.

### System.Collections.Generic
- [x] **SortedList<TKey, TValue>**: Add, Remove, Indexer, Keys, Values.
- [x] **SortedSet<T>**: Add, Remove, Set operations (Union, Intersect).
- [x] **IReadOnlyInterfaces**: `IReadOnlyList<T>`, `IReadOnlyDictionary<TKey, TValue>`.
- [x] **Comparers**: `EqualityComparer<T>.Default`, `Comparer<T>.Default`.

### System.Linq
- [x] **Set Operators**: `Distinct`, `Union`, `Intersect`, `Except`.
- [x] **Partitioning**: `Take`, `Skip`, `TakeWhile`, `SkipWhile`.
- [x] **Grouping**: `GroupBy` with various overloads (key selector, element selector, result selector).
- [x] **Joins**: `Join`, `GroupJoin`.
- [x] **Generation**: `Range`, `Repeat`, `Empty`.

### System.Globalization
- [x] **CultureInfo**: `GetCultureInfo`, `CurrentCulture`, `InvariantCulture`.
- [x] **Formats**: `DateTimeFormat`, `NumberFormat` access.

### System.Guid
- [x] **Formats**: "N", "D", "B", "P", "X".
- [x] **Parsing**: `Parse`, `TryParse`, `ParseExact`, `TryParseExact`.

### System.Version
- [x] **Parsing**: "Major.Minor", "Major.Minor.Build", "Major.Minor.Build.Revision".
- [x] **Comparison**: `CompareTo` and operators.
