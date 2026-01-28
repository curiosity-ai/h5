# Introduction to H5

H5 is a modern C# to JavaScript compiler. It is a fork of the original Bridge.NET project, redesigned to support modern .NET versions (.NET 8.0+) and .NET Standard 2.1.

## Why H5?

- **C# on the Web**: Write your frontend logic in C#, leveraging strong typing, LINQ, and other modern C# features. H5 supports **C# version 7.3**.
- **Shared Code**: Share models and logic between your C# backend and your JavaScript frontend.
- **Productivity**: Use familiar tools like Visual Studio or VS Code with full IntelliSense and refactoring support for your web code.
- **Modern .NET**: Built on top of the latest .NET technologies.

## How it works

The H5 compiler parses your C# source code and generates equivalent JavaScript code. It also provides a JavaScript implementation of the .NET Base Class Library (BCL), so you can use types like `List<T>`, `Dictionary<K,V>`, `DateTime`, and even Task-based asynchrony in your JavaScript code.

## Key Components

- **Compiler**: The `h5-compiler` tool that performs the translation.
- **Base Library (`h5`)**: Provides the attributes and the core JavaScript runtime.
- **Core Library (`h5.core`)**: Provides C# bindings for browser APIs (DOM, ES5/6).

## Related Projects

- **[Tesserae](https://github.com/curiosity-ai/tesserae)**: A powerful UI component library built on top of H5, providing a rich set of widgets for building modern web interfaces.
