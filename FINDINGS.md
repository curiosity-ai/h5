# Findings

## Integers_ArithmeticAndBitwise Failure

The test `Integers_ArithmeticAndBitwise` failed because `Console.WriteLine` with a `long` argument prints the type name `System.Int64` instead of the numeric value in H5.

**Roslyn Output:**
```
...
2469135780246
...
```

**H5 Output:**
```
...
System.Int64
...
```

## Enums Failure

The test `Enums` failed because `Console.WriteLine(enumValue)` prints the underlying integer value instead of the string representation (name), and `[Flags]` enums are not formatted as comma-separated strings.

**Roslyn Output:**
```
Green
2
Read, Write
True
False
```

**H5 Output:**
```
2
2
3
True
False
```

## C# 6.0 Exception Filters Failure (FIXED)

The `ExceptionFilters` test in `CSharp60Tests.cs` was failing due to improper casting in the rewritten filter expression.

### Fix
The `SharpSixRewriter` was generating `(Exception)ex.Message`, which was interpreted as `(Exception)(ex.Message)` due to operator precedence, resulting in `H5.cast(ex.Message, Exception)`. This caused the filter to fail as it tried to cast a string to an Exception. The fix involved wrapping the cast expression in parentheses to ensure `((Exception)ex).Message` is generated.

### Status
The test `ExceptionFilters_Failing` has been renamed to `ExceptionFilters_PropertyCheck` and now passes.

## Synchronous Local Functions inside Async Lambdas with Outer Local Functions

The H5 compiler fails to generate valid JavaScript when a **synchronous** local function is defined inside an **async lambda**, AND there is another local function defined in the outer scope (whether hoisted or not).

The generated JavaScript produces a `SyntaxError: Unexpected identifier 'LocalName'`, suggesting that the inner local function is not being emitted correctly or its scope is being mishandled during the async state machine generation or closure lifting.

### Minimal Failing Case
```csharp
public static async Task Main()
{
    Func<Task> lambda = async () =>
    {
        void InnerLocal() { } // Synchronous local function inside async lambda
        InnerLocal();
        await Task.Delay(1);
    };
    await lambda();

    void OuterLocal() { } // Presence of this outer local function triggers the bug
}
```

### Minimal Passing Case
Removing `OuterLocal` makes the test pass.
```csharp
public static async Task Main()
{
    Func<Task> lambda = async () =>
    {
        void InnerLocal() { }
        InnerLocal();
        await Task.Delay(1);
    };
    await lambda();
}
```

### Affected Test Cases
- `H5.Compiler.IntegrationTests.NestedFunctionsTests.ComplexNestingAndHoisting` (Ignored)
- `H5.Compiler.IntegrationTests.NestedFunctionsTests.SyncLocalFunctionInsideAsyncLambda_WithOuterLocalFunction` (Ignored)
