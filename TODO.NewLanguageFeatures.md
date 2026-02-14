# Pending Language Features (C# 7.x - 13.0)

This file tracks the implementation status of pending C# language features.

## C# 7.0 / 7.1 / 7.2

- [ ] **Ref Returns and Locals**: `ref int Method()`, `ref var x = ref y;`.

## C# 7.3

- [ ] **In Method Overload Resolution**: Tiebreaker for `in` parameters. (Priority: Medium)
- [ ] **Fixed Sized Buffers**: Indexing without pinning. (Priority: Not Important)
- [ ] **Unmanaged Constraint**: `where T : unmanaged`. (Priority: Not Important)
- [ ] **Delegate Constraint**: `where T : Delegate`. (Priority: Low)
- [ ] **Enum Constraint**: `where T : Enum`. (Priority: Low)

## C# 8.0

- [ ] **Readonly Members**: `public readonly int Method()`. (Priority: Medium)
- [ ] **Default Interface Methods**: Implementation in interfaces. (Priority: High)
- [ ] **Pattern Matching Enhancements**:
- [x] **Switch Expressions**: `x switch { ... }`. (Priority: High)
    - [ ] **Property Patterns**: `{ P: 1 }`. (Priority: High)
    - [ ] **Tuple Patterns**: `(1, 2)`. (Priority: High)
    - [ ] **Positional Patterns**: `Deconstruct` based. (Priority: High)
- [x] **Using Declarations**: `using var x = ...`. (Priority: High)
- [ ] **Disposable Ref Structs**: `ref struct` with `Dispose`. (Priority: Low)
- [ ] **Nullable Reference Types**: `string?`, `notnull` constraint. (Priority: High)
- [ ] **Async Streams**: `await foreach`, `IAsyncEnumerable`. (Priority: High)
- [x] **Indices and Ranges**: `^1`, `1..5`. (Priority: High)
- [x] **Null-Coalescing Assignment**: `x ??= y`. (Priority: High)
- [ ] **Unmanaged Constructed Types**: `struct S<T> where T : unmanaged`. (Priority: Not Important)
- [ ] **StackAlloc in Nested Expressions**: Inside other expressions. (Priority: Low)

## C# 9.0

- [ ] **Records**: `public record Person(string Name);`. (Priority: High)
- [ ] **Init Only Setters**: `public int X { get; init; }`. (Priority: High)
- [ ] **Top-Level Statements**: `System.Console.WriteLine("Hello");`. (Priority: Medium)
- [ ] **Pattern Matching Enhancements**:
    - [x] **Type Patterns**: `is Type`. (Priority: High)
    - [x] **Parenthesized Patterns**: `(pattern)`. (Priority: High)
    - [x] **Conjunctive Patterns**: `and`. (Priority: High)
    - [x] **Disjunctive Patterns**: `or`. (Priority: High)
    - [x] **Negated Patterns**: `not`. (Priority: High)
    - [x] **Relational Patterns**: `< 10`. (Priority: High)
- [ ] **Target-Typed New**: `Point p = new (1, 2);`. (Priority: Medium)
- [ ] **Static Anonymous Functions**: `static x => x`. (Priority: Low)
- [ ] **Target-Typed Conditional**: `flag ? 1 : 1.0` (inferred). (Priority: Medium)
- [ ] **Covariant Return Types**: Override with derived return type. (Priority: Medium)
- [ ] **Extension GetEnumerator**: `foreach` on types with extension `GetEnumerator`. (Priority: Medium)
- [ ] **Lambda Discard Parameters**: `(_, _) => 0`. (Priority: Medium)
- [ ] **Attributes on Local Functions**: `[Attr] void Local()`. (Priority: Low)
- [ ] **Native Sized Integers**: `nint`, `nuint`. (Priority: Low)
- [ ] **Module Initializers**: `[ModuleInitializer]`. (Priority: Medium)

## C# 10.0

- [ ] **Record Structs**: `public record struct Point(int X, int Y);`. (Priority: Medium)
- [ ] **Struct Improvements**: Parameterless constructors, field initializers. (Priority: Medium)
- [ ] **Global Usings**: `global using System;`. (Priority: Medium)
- [x] **File-Scoped Namespaces**: `namespace MyNamespace;`. (Priority: High)
- [ ] **Extended Property Patterns**: `{ Prop.Child: 1 }`. (Priority: High)
- [ ] **Lambda Improvements**: Attributes, explicit return types. (Priority: Medium)
- [ ] **Sealed ToString in Records**: `sealed override ToString()`. (Priority: Low)
- [ ] **CallerArgumentExpression**: `[CallerArgumentExpression("p")]`. (Priority: High)
- [ ] **AsyncMethodBuilder Attribute**: On methods. (Priority: Not Important)

## C# 11.0

- [x] **Raw String Literals**: `"""..."""`. (Priority: High)
- [ ] **Generic Math**: `static abstract` interface members (syntax support). (Priority: Low)
- [ ] **Generic Attributes**: `class Attr<T> : Attribute`. (Priority: Medium)
- [ ] **UTF-8 String Literals**: `"text"u8`. (Priority: Low)
- [ ] **List Patterns**: `[1, .., 5]`. (Priority: High)
- [ ] **File-Local Types**: `file class Local`. (Priority: Medium)
- [ ] **Required Members**: `required public int X { get; set; }`. (Priority: High)
- [ ] **Pattern Match Span<char>**: Constant string. (Priority: Medium)
- [ ] **Extended Nameof**: `nameof` parameter in attribute. (Priority: Medium)
- [ ] **Numeric IntPtr**: `nint` as alias for `System.IntPtr` (unification). (Priority: Low)
- [ ] **Ref Fields**: `ref int x` in `ref struct`. (Priority: Low)
- [ ] **Scoped Ref**: `scoped ref` parameters/locals. (Priority: Low)

## C# 12.0

- [ ] **Primary Constructors**: `class C(int x) { ... }`. (Priority: High)
- [ ] **Collection Expressions**: `[1, 2, 3]`. (Priority: High)
- [ ] **Inline Arrays**: `[System.Runtime.CompilerServices.InlineArray(10)]`. (Priority: Not Important)
- [ ] **Optional Params in Lambdas**: `(int x = 1) => x`. (Priority: Medium)
- [ ] **Ref Readonly Parameters**: `ref readonly int`. (Priority: Low)
- [ ] **Experimental Attribute**: `[Experimental("ID")]`. (Priority: Low)

## C# 13.0

- [ ] **Params Collections**: `params List<int>`. (Priority: Medium)
- [ ] **Lock Object**: `System.Threading.Lock`. (Priority: Not Important)
- [ ] **Implicit Index Access**: `^1` in object initializers. (Priority: Medium)
- [ ] **Escape Sequence \e**: ESC character. (Priority: Low)
