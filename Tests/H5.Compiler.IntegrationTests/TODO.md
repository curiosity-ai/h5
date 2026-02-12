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
- [ ] **Try-Catch-Finally**: `try`, `catch` (specific/general), `finally` blocks.
- [ ] **Throw**: `throw new Exception("...")`.
- [ ] **Re-throw**: `throw;` inside catch block.
- [ ] **Custom Exceptions**: Inheriting from `System.Exception`.
- [ ] **Properties**: `Message`, `InnerException`, `StackTrace` (basic verification).

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

- [ ] **Named Arguments**: `Method(arg2: 10, arg1: 5)`.
- [ ] **Optional Arguments**: `void Method(int x = 10)`.
- [ ] **Dynamic Binding**: Basic usage of `dynamic` (if supported by H5 runtime, otherwise skip or verify compilation error/runtime behavior). Note: Might rely on `System.Dynamic` support.

## C# 5.0: Async/Await

- [ ] **Async/Await**: `async Task Method()`, `await Task.Delay()`, `await Task.WhenAll()`. Reference: `H5/H5/System/Threading/Tasks/Task.cs`.
- [ ] **Task<T>**: Returning values from async methods.
- [ ] **Caller Info Attributes**: `[CallerMemberName]`, `[CallerFilePath]`, `[CallerLineNumber]` (if supported).

## C# 6.0: Syntactic Sugar

- [ ] **String Interpolation**: `$"Value: {x}"`, formatting `$"Date: {d:yyyy-MM-dd}"`.
- [ ] **Null-Conditional Operator**: `obj?.Property`, `list?[0]`.
- [ ] **Expression-Bodied Members**: Methods `=>`, Properties `=>`.
- [ ] **Exception Filters**: `catch (Exception ex) when (ex.Message == "X")`.
- [ ] **Nameof Operator**: `nameof(variable)`, `nameof(Class.Property)`.
- [ ] **Auto-Property Initializers**: `public int Prop { get; set; } = 10;`.
- [ ] **Getter-Only Auto-Properties**: `public int Prop { get; } = 10;`.
- [ ] **Index Initializers**: `new Dictionary<int, string> { [1] = "One" }`.
- [ ] **Static Imports**: `using static System.Math;` then `Sqrt(4)`.

## C# 7.0 / 7.1 / 7.2: Modern Features

- [ ] **Out Variables**: `int.TryParse("123", out int result);`.
- [ ] **Tuples (ValueTuple)**: `(int, string) t = (1, "A");`, naming elements `(Val: 1, Name: "A")`. Reference: `H5/H5/System/ValueTuple.cs`.
- [ ] **Deconstruction**: `(var x, var y) = t;`.
- [ ] **Pattern Matching**: `if (obj is int i)`, `switch` with type patterns and `when` clauses.
- [ ] **Local Functions**: Defining functions inside methods.
- [ ] **Discards**: `_ = Method();`.
- [ ] **Literal Improvements**: Binary literals `0b101`, Digit separators `1_000`.
- [ ] **Throw Expressions**: `int x = val ?? throw new Exception();`.
- [ ] **Default Literal**: `int x = default;`.
- [ ] **Private Protected**: `private protected` accessibility modifier.
- [ ] **Non-Trailing Named Arguments**: `Method(1, arg2: 2)`.
- [ ] **In Parameters**: `void Method(in int x)` (verify basic behavior).
- [ ] **ReadOnly Structs**: `readonly struct Point { ... }`.

## Excluded Features (Do Not Implement)

- [ ] **Threading**: `Thread` class, `ThreadPool`, `Monitor`, `lock` statement (unless implemented via simple JS constructs, generally avoid).
- [ ] **File I/O**: `System.IO.File`, `StreamReader` (except `MemoryStream` if available).
- [ ] **Unsafe Code**: `unsafe`, `fixed`, pointers `*`.
- [ ] **Native Interop**: `DllImport`, `extern`.
- [ ] **Garbage Collection**: `GC.Collect()`, finalizers (JS GC is non-deterministic and not controllable).
