# Safe Workflow for Implementing New Language Features

This document outlines the workflow for implementing new C# language features in H5, specifically targeting those listed in [Tests/H5.Compiler.IntegrationTests/TODO.md](Tests/H5.Compiler.IntegrationTests/TODO.md).

## Workflow Steps

1.  **Deep Understanding of the Feature**
    -   Before implementation, the agent must deeply understand the feature by reviewing how the code is currently implemented and how it behaves in standard C#.
    -   Analyze existing integration tests or create new ones to understand the feature's requirements and edge cases.

2.  **Rewriter Implementation**
    -   New code transformation steps should be done using Roslyn in order to support all new language features.
    -   This transformation should occur in the `SharpSixRewriter`-based step (see Rewrite in `Translator.InspectAssembly.cs` and `SharpSixRewriter`).
    -   The new rewrite rules should only activate if the target compilation language is > 7.2 and runs before the existing rewriter.

3.  **Implementation Strategy**
    -   Feature support can be implemented in two ways:
        -   **Rewriting**: Transform the code into more standard/older features that are already supported. For example, `record` types can be rewritten as `class` or `struct` types directly.
        -   **New Emit Flows**: Add new Emit flows to support the generation of JavaScript that matches the new feature behavior natively.

## Reference to Codebase

-   **Modern Rewriter Placeholder**: A comment has been added to `H5/Compiler/Translator/Translator/Translator.InspectAssembly.cs` indicating where the Modern Rewriter step should be integrated.
-   **Pending Features**: See [Tests/H5.Compiler.IntegrationTests/TODO.md](Tests/H5.Compiler.IntegrationTests/TODO.md) for the list of pending language features and their priorities.
