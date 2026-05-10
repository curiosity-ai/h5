# H5.Compiler.Next

This directory contains the next generation of the H5 compiler. The new compiler is a pure Roslyn-based implementation that directly emits JavaScript code, without relying on the NRefactory-based rewriting steps present in the current H5 compiler.

## Goals

1. **Pure Roslyn:** Drop NRefactory entirely. Use Roslyn (`Microsoft.CodeAnalysis.CSharp`) for all syntax parsing, semantic analysis, and compilation steps.
2. **Direct Emission:** Walk the Roslyn syntax trees or symbols and emit JavaScript directly.
3. **Compatibility:** The new compiler must be able to compile the H5 core library and pass the same test suite as the current compiler.
4. **Performance:** Ideally faster than the current rewrite-and-emit pipeline.

## Implementation Details

- Start with `H5.Compiler.Next` (executable) and `H5.Compiler.Service.Next` (library containing the core logic).
- We'll need a mechanism to walk Roslyn trees and generate JS (e.g., a `CSharpSyntaxWalker` or visitor).
- We should reuse the final JS output format where possible.
