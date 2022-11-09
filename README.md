#  h5 🚀 - C# to JavaScript compiler

<a href="https://h5.rocks"><img src="https://raw.githubusercontent.com/theolivenbaum/h5/master/logo/h5.svg" width="120" height="120" align="right" /></a>

H5 is a modern fork of the original [Bridge](https://github.com/bridgedotnet/bridge) C# to Javascript compiler, updated to support multi-platform development using .NET 6.0 and .NET Standard 2.1 projects, while dropping support for legacy features and dependencies.


H5 is under active development, and targets a more integrated and faster development experience for C# web-developers. We're also planning to experiment with new ideas to improve compilation speed (such as aggressive caching of emitted code) and possibly integrating [C# Source Generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/)) for even faster code generation.


|  Package | NuGet           | 
| -------------: |:-------------:|
| Compiler | [![Nuget](https://img.shields.io/nuget/v/h5-compiler.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5-compiler/) |
| Base Library | [![Nuget](https://img.shields.io/nuget/v/h5.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5/) |
| Core Library | [![Nuget](https://img.shields.io/nuget/v/h5.core.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.core/) |
| SDK Target | [![Nuget](https://img.shields.io/nuget/v/h5.target.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.target/) |
| Json Library | [![Nuget](https://img.shields.io/nuget/v/h5.Newtonsoft.Json.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.Newtonsoft.Json/) |
| Template | [![Nuget](https://img.shields.io/nuget/v/h5.template.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.template/) |
| UI Toolkit | [![Nuget](https://img.shields.io/nuget/v/tesserae.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/tesserae/)|


##  Getting Started ⚡

[![Gitter](https://badges.gitter.im/curiosityai/h5.svg)](https://gitter.im/curiosityai/h5?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

H5 is fully based on [netstandard2.1](https://github.com/theolivenbaum/h5/blob/master/H5/H5/H5.csproj) & [net7.0](https://github.com/theolivenbaum/h5/blob/master/H5/Compiler/Compiler/H5.Compiler.csproj), and removes all dependencies on the legacy .NET Framework coming from the original Bridge source-code.

To get started with it, you can use the following project template  

````xml
<Project Sdk="h5.Target/0.0.*">
  <PropertyGroup>
    <TargetFramework>netstandard2.1</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="h5" Version="*" />
    <PackageReference Include="h5.Core" Version="*" />
  </ItemGroup>
</Project>
````

The Sdk target above (``<Project Sdk="h5.Target/0.0.*">``) will automatically install (and update) the compiler as a ``dotnet global tool``. You need to update the version ``/0.0.*`` to the latest [![Nuget](https://img.shields.io/nuget/v/h5.target.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.target/) package.


Don't forget to run ``dotnet restore`` to fill the versions with the latest values. You can also manually install it with:

````bash
dotnet tool update --global h5-compiler
````

You can also install a dotnet new template (latest version:  [![Nuget](https://img.shields.io/nuget/v/h5.template.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.template/)):

````bash
dotnet new --install h5.Template::0.0.21601
````

And create a new project with:

````bash
dotnet new h5
```` 

## Samples
The easiest way to get started is to check out some of the examples of using h5 in [this repository](https://github.com/theolivenbaum/h5-samples). 

##  Breaking Changes 💥

This fork introduces a series of breaking changes as part of the modernization effort:
- Projects must explicitly target ``netstandard2.0``.
- Drop support to the legacy *.csproj* format (only SDK-style projects are supported)
- Drop support for legacy (and unused) command line arguments (check h5 -h for the current supported arguments)
- Compiler is now distributed as a ``dotnet global`` tool and have it's own versioning and auto-update on build (this can be disable by setting `<UpdateH5>false<UpdateH5/>` on your project file.
- **Retyped packages are not supported** (as those are maintained by the Bridge authors, and cannot be built separately or consumed without the Bridge NuGet package).
- Logging and Report options have been removed from the h5.json config file. Logging settings will be available only as a command line argument (and exposed as a Project file option in the future)
- Hosted Compiler process (to speed up compilation and avoiding reloading assemblies that don't change often (like nuget packages), h5 introduces an off-process compiler server. *For now, this process will open a terminal with the compilation logs - but this will be hidden in the future)*

##  Update Notes 📑

To avoid any conflicts with the original Bridge ecosystem, all packages have been renamed. For upgrading, you can use the following mapping:
- [H5](https://www.nuget.org/packages/h5/) (replaces the base [Bridge](https://www.nuget.org/packages/Bridge/) library) 
- [H5.Core](https://www.nuget.org/packages/h5.core) (replaces [Retyped.Core](https://www.nuget.org/packages/Retyped.Core/), [Retyped.es5](https://www.nuget.org/packages/Retyped.es5/) and [Retyped.dom](https://www.nuget.org/packages/Retyped.dom/))
- [H5.Newtonsoft.Json](https://www.nuget.org/packages/h5.Newtonsoft.Json/) (replaces [Bridge.Newtonsoft.Json](https://www.nuget.org/packages/Bridge.Newtonsoft.Json/))
- [H5.WebGL2](https://www.nuget.org/packages/h5.webgl2) (replaces [Retyped.WebGl2](https://www.nuget.org/packages/Retyped.Webgl2))

Other packages might be added in the future as we experiment with this fork, but we do not aim on providing any kind of Retyped replacement here.
If you're missing any specific Retyped package, open an issue and I can take a look into publishing it derived from the Retyped ones. 
