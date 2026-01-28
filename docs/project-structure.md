# Project Structure in H5

An H5 project is a standard .NET project that targets `netstandard2.1` and uses the `h5.Target` SDK.

## Example `.csproj`

```xml
<Project Sdk="h5.Target/25.11.62725">
  <PropertyGroup>
    <TargetFramework>netstandard2.1</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="h5" Version="25.11.62743" />
    <PackageReference Include="h5.Core" Version="25.11.62743" />
  </ItemGroup>
</Project>
```

## Key Files

### `h5.json`
This is the configuration file for the H5 compiler. It resides in the project root and controls various aspects of code generation, such as output directory, module type, and more.

### `App.cs` (or any C# file)
Your C# source code that will be translated to JavaScript.

## Folders in the Repository

If you are contributing to H5 itself, here is the project layout:

- `H5/Compiler`: The source code for the compiler.
- `H5/H5`: The core library containing C# attributes and the JavaScript runtime implementation (in `Resources/`).
- `H5/H5.Core`: C# definitions for the DOM and other browser APIs.
- `H5/H5.Build.Target`: MSBuild targets for integration.
