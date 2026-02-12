# Integration Tests for H5 Compiler

The purpose of these integration tests is to ensure the correctness of the H5 compiler by comparing the execution results of C# code between:
1.  **Roslyn (Native .NET Runtime)**: The standard C# compilation and execution.
2.  **H5 (Compiled to JavaScript)**: The C# code compiled to JavaScript by H5 and executed in a headless browser (Playwright).

## Test Implementation Requirements

### 1. Side-Effects for Output Comparison
Every test **MUST** produce side-effects visible in the console (e.g., via `Console.WriteLine`).
-   The test runner captures the standard output from both the Roslyn execution and the H5/Playwright execution.
-   These outputs are normalized and compared.
-   If the outputs match, the test passes. If they differ, the test fails.

**Example:**
```csharp
[TestMethod]
public async Task SimpleAddition()
{
    var code = """
using System;
public class Program
{
    public static void Main()
    {
        int a = 5;
        int b = 10;
        Console.WriteLine(a + b); // REQUIRED: Output to console for verification
    }
}
""";
    await RunTest(code);
}
```

### 2. Test Structure
-   Tests are located in `Tests/H5.Compiler.IntegrationTests/`.
-   Tests inherit from `IntegrationTestBase`.
-   Use `RunTest(string code)` to execute the comparison.

### 3. Tracking Pending Tests
-   Refer to `TODO.md` in this directory for a list of pending tests and feature coverage requirements.
-   When implementing a test from `TODO.md`, check it off or remove it from the list.

### 4. Handling Failures
-   If a test fails, do not simply comment it out or delete it.
-   Attempt to split the test code into the **minimum failing test** and the **minimum passing test**.
-   This isolation helps identify exactly what part of the feature is broken while ensuring the working parts are still tested.
-   Report the failure in a `FINDINGS.md` file (create if it doesn't exist) with details of the failure.

## Scope & Limitations
-   **Supported Features**: Basic C# language constructs (up to C# 7.2), core .NET types (System.String, System.Int32, etc.), LINQ to Objects, Async/Await (Task-based).
-   **Unsupported/Excluded Features**:
    -   Threading (except `Task` and `Task<T>`)
    -   File I/O
    -   Locking / Synchronization primitives (beyond basic `Task` handling)
    -   Garbage Collection explicit control
    -   Unsafe code
    -   Native code interoperability
    -   Pointers
