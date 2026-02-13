# Safe Workflow for Implementing New Language Features

This document outlines the workflow for implementing new C# language features in H5, specifically targeting those listed in [TODO.NewLanguageFeatures.md](TODO.NewLanguageFeatures.md).

## Workflow Steps

1.  **Deep Understanding of the Feature**
    -   Before implementation, the agent must deeply understand the feature by reviewing how the code is currently implemented and how it behaves in standard C#.
    -   Analyze existing integration tests or create new ones to understand the feature's requirements and edge cases.

2.  **Rewriter Implementation**
    -   New code transformation steps should be done using Roslyn in order to support all new language features.
    -   This transformation should occur in the `SharpSixRewriter`-based step (see Rewrite in `Translator.InspectAssembly.cs` and `SharpSixRewriter`).
    -   The new rewrite rules should only activate only if the target compilation language is > 7.2 and runs before the existing rewriter.

3.  **Implementation Strategy**
    -   Feature support can be implemented in two ways:
        -   **Rewriting**: Transform the code into more standard/older features that are already supported. For example, `record` types can be rewritten as `class` or `struct` types directly.
        -   **New Emit Flows**: Add new Emit flows to support the generation of JavaScript that matches the new feature behavior natively.

## Reference to Codebase

-   **Rewriter Reference**: Start at `H5/Compiler/Translator/Utils/Roslyn/SharpSixRewriter.cs` to understand how code rewrite happens.
-   **Pending Features**: See [TODO.NewLanguageFeatures.md](TODO.NewLanguageFeatures.md) for the list of pending language features and their priorities.

## Tracking Progress

When working on a new language feature, follow this process to avoid conflicts and track status:

1.  **Check Status**:
    -   Review `WIP.NewLanguageFeatures.md` to ensure no other agent is currently working on the same or a conflicting feature.
    -   If the file is not empty, verify if the task is stale (check the Start Date).

2.  **Select Feature**:
    -   Choose a feature from `TODO.NewLanguageFeatures.md` that is unchecked (`[ ]`).

3.  **Update WIP**:
    -   Move the feature details (Name, Priority, Description) to `WIP.NewLanguageFeatures.md`.
    -   Fill in the **Start Date** and set **Status** to "In Progress".

4.  **Implement**:
    -   Perform the implementation following the "Workflow Steps" above.
    -   Update the **Notes** section in `WIP.NewLanguageFeatures.md` with any findings or sub-tasks.

5.  **Complete**:
    -   Once the feature is fully implemented and all integration tests pass:
        1.  Mark the feature as completed in `TODO.NewLanguageFeatures.md` (change `[ ]` to `[x]`).
        2.  Clear the content of `WIP.NewLanguageFeatures.md` or reset it to the empty template.
