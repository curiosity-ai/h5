# AGENTS.md - H5 Repository Guide

Welcome to the H5 repository. H5 is a modern fork of the Bridge.NET C# to JavaScript compiler, updated for .NET 8.0 and .NET Standard 2.1.

## Repository Structure

The repository is organized into several key projects under the `H5/` directory:

### Core Projects
- **`H5/Compiler/`**: The brain of the project. It contains the C# to JavaScript translator logic.
  - **Output NuGet**: `h5-compiler` (distributed as a dotnet global tool).
- **`H5/H5/`**: The base library. It contains the core C# attributes used to control code generation and the JavaScript runtime implementation.
  - **Output NuGet**: `h5`.
  - **Note**: This project features a mix of C# (attributes, system types) and JavaScript (found in `Resources/`) which provides the runtime environment for the generated code.
- **`H5/H5.Core/`**: Provides C# bindings for standard web APIs (DOM, ES5, ES6).
  - **Output NuGet**: `h5.core`.
  - **Note**: This package replaces the legacy `Retyped.*` packages.
- **`H5/H5.Build.Target/`**: Contains MSBuild targets and SDK definitions to integrate H5 into the .NET build process.
  - **Output NuGet**: `h5.target`.
- **`H5/H5.Newtonsoft.Json/`**: A port of Newtonsoft.Json that works within the H5 ecosystem.
  - **Output NuGet**: `h5.Newtonsoft.Json`.
- **`H5/H5.Packages/`**: Contains additional specialized bindings for popular JavaScript libraries (e.g., `H5.howler`, `H5.webgl2`).

### Other Folders
- **`External/`**: Contains external dependencies and modified source code of other projects (like NRefactory) used by the compiler.
- **`Template/`**: Contains the `dotnet new` templates for H5 projects.
  - **Output NuGet**: `h5.template`.
- **`Tests/`**: Contains various test suites for the compiler and runtime.

## Working with C# and JavaScript

H5 projects are unique because they involve a mix of C# source code and JavaScript runtime.
- **C# Attributes**: Located in `H5/H5/Attributes/`. These are used to guide how C# code is translated to JavaScript.
- **JavaScript Runtime**: Located in `H5/H5/Resources/`. These files implement the .NET base class library (BCL) in JavaScript. If you modify these, you are changing the behavior of the generated code at runtime.

## Controlling Code Generation

Use the following attributes (from the `H5` namespace) to control the output:
- `[External]`: Mark types/members as defined in external JS.
- `[Name]`: Override the name in the generated JS.
- `[Template]`: Provide a JS code template for a method call.
- `[Script]`: Provide raw JS for a method body.
- `[ObjectLiteral]`: Treat a C# class/struct as a plain JS object `{}`.

## Documentation

For more detailed information about the library and how to use it, refer to the `docs/` folder.
