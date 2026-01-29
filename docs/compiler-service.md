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

The `CompilationOutput` represents the result of the compilation. It contains the actual content of the generated files.

-   **Output**: A `Dictionary<string, string>` where the key is the file path (relative to the output directory) and the value is the file content.

## How it Works

1.  **Request Creation**: You create a `CompilationRequest` with the source code and configuration.
2.  **Temporary Project**: When `CompileAsync` is called, the service creates a temporary directory with a dynamically generated `.csproj` file and the provided source files.
3.  **Compilation**: It uses the `TranslatorProcessor` (the core of the H5 compiler) to compile the temporary project.
4.  **Result Retrieval**: Upon success, the service reads all generated files from the output directory into memory and creates a `CompilationOutput` object.
5.  **Cleanup**: The temporary directory (including source and output files) is deleted immediately after the result is created. Since the output is cached in memory within `CompilationOutput`, you don't need to worry about file persistence.

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
