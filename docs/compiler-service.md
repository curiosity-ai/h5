# H5 Compiler Service

The H5 Compiler Service (`H5.Compiler.Service`) is a library that allows you to invoke the H5 compiler programmatically from your own .NET applications. It acts as a "compiler-as-a-service," enabling scenarios like in-browser REPLs, dynamic compilation, or integration into other build tools.

## Package

The service is available as a NuGet package: `h5.Compiler.Service`.

## Key Components

### CompilationRequest

The `CompilationRequest` class (in `H5.Compiler.Hosted` namespace) represents a request to compile C# source code to JavaScript. It allows you to:

-   **Add Source Code**: Add C# files with their content via `WithSourceFile(fileName, code)`.
-   **Manage References**: Add NuGet package references via `WithPackageReference`.
-   **Configure Settings**: Set properties like `AssemblyName`, `SkipEmbeddingResources`, `SkipHtmlGeneration`, etc.

**Example:**

```csharp
var request = new CompilationRequest()
{
    AssemblyName = "MyDynamicApp",
    SkipHtmlGeneration = true // Only generate JS
}
.WithSourceFile("Program.cs", @"
    using System;
    public class Program {
        public static void Main() {
            Console.WriteLine(""Hello from H5!"");
        }
    }
");
```

### CompilationProcessor

The `CompilationProcessor` class (in `H5.Compiler` namespace) handles the execution of compilation requests.

-   **CompileAsync**: The main method to trigger compilation. It accepts a `CompilationRequest` and an optional `CancellationToken`.

**Method Signature:**

```csharp
public static Task<CompilationOutput> CompileAsync(CompilationRequest request, CancellationToken cancellationToken = default);
```

### CompilationOutput

The `CompilationOutput` represents the result of the compilation. It contains information about where the generated artifacts are located.

## How it Works

1.  **Request Creation**: You create a `CompilationRequest` with the source code and configuration.
2.  **Temporary Project**: When `CompileAsync` is called, the service creates a temporary directory with a dynamically generated `.csproj` file and the provided source files.
3.  **Compilation**: It uses the `TranslatorProcessor` (the core of the H5 compiler) to compile the temporary project.
4.  **Output**: Upon success, it returns a `CompilationOutput`. If compilation fails, the task will fault with an exception (e.g., `EmitterException` or `Exception`).
5.  **Cleanup**: The temporary files are cleaned up after processing (though `CompilationProcessor.Compile` in the service seems to delete the directory in a finally block, make sure to read the code to confirm if artifacts persist long enough for you to read them - *Note: The current implementation deletes the base directory in the `finally` block of `Compile`, so you might need to handle the output data immediately or the implementation might have changed to allow reading it before deletion.*)

*Note: Based on the source code, `Compile` deletes the temporary directory in the `finally` block. This suggests that `CompilationOutput` might return data that was read into memory, or there might be a caveat on how to access the result.*

## Example Usage

```csharp
using H5.Compiler;
using H5.Compiler.Hosted;

public async Task CompileCodeAsync()
{
    var request = new CompilationRequest()
    {
        AssemblyName = "TestApp"
    }
    .WithSourceFile("Main.cs", "class P { static void Main() { System.Console.WriteLine(\"Hi\"); } }");

    try
    {
        var output = await CompilationProcessor.CompileAsync(request);
        // Process output...
    }
    catch (Exception ex)
    {
        // Handle compilation errors
        Console.WriteLine($"Compilation failed: {ex.Message}");
    }
}
```
