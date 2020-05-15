#  h5 - C# to JavaScript compiler, now on .NET Core 3.1 🚀

This repository contains an experimental fork of the original [Bridge](https://github.com/bridgedotnet/bridge) C# to Javascript compiler.

The key goal with this fork is to bring it closer to the C# .NET Core 3.1 / .NET Standard world, and experiment with new ideas for supporting a more integrated development experience (such as the awesome new [C# Source Generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/)).


|  Package | NuGet           |  Azure DevOps   |
| -------------: |:-------------:| :-----:|
| Compiler | [![Nuget](https://img.shields.io/nuget/v/h5-compiler.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5-compiler/) |  [![Build Status](https://dev.azure.com/curiosity-ai/mosaik/_apis/build/status/h5-compiler?branchName=master)](https://dev.azure.com/curiosity-ai/mosaik/_build/latest?definitionId=38&branchName=master) |
| Base Library | [![Nuget](https://img.shields.io/nuget/v/h5.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5/) | [![Build Status](https://dev.azure.com/curiosity-ai/mosaik/_apis/build/status/h5-base-nuget?branchName=master)](https://dev.azure.com/curiosity-ai/mosaik/_build/latest?definitionId=39&branchName=master) |
| Core Library | [![Nuget](https://img.shields.io/nuget/v/h5.core.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.core/) |  [![Build Status](https://dev.azure.com/curiosity-ai/mosaik/_apis/build/status/h5.core?branchName=master)](https://dev.azure.com/curiosity-ai/mosaik/_build/latest?definitionId=40&branchName=master) |
| SDK Target | [![Nuget](https://img.shields.io/nuget/v/h5.target.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.target/) |  [![Build Status](https://dev.azure.com/curiosity-ai/mosaik/_apis/build/status/h5.target?branchName=master)](https://dev.azure.com/curiosity-ai/mosaik/_build/latest?definitionId=43&branchName=master) |
| Json Library | [![Nuget](https://img.shields.io/nuget/v/h5.Newtonsoft.Json.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.Newtonsoft.Json/) |  [![Build Status](https://dev.azure.com/curiosity-ai/mosaik/_apis/build/status/h5.json?branchName=master)](https://dev.azure.com/curiosity-ai/mosaik/_build/latest?definitionId=41&branchName=master) |


##  Getting Started ⚡

This new compiler build is fully based on [netstandard2.0](https://github.com/theolivenbaum/h5/blob/master/H5/H5/H5.csproj) & [netcore3.1](https://github.com/theolivenbaum/h5/blob/master/H5/Compiler/Builder/H5.Builder.csproj), and removes all dependencies on the legacy .NET Framework.

To get started with it, you can use the following project template  

````xml
<Project Sdk="h5.Target/0.0.8018">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="h5" Version="*" />
    <PackageReference Include="h5.Core" Version="*" />
  </ItemGroup>
</Project>
````

The Sdk target above (``<Project Sdk="h5.Target/0.0.8018">``) will automatically install (and update) the compiler as a ``dotnet global tool``. Don't forget to run ``dotnet restore`` to fill the versions with the latest values. You can also manually install it with:

````bash
dotnet tool update --global h5-compiler
````

We'll very soon add a `dotnet new` template supporting h5.

##  Breaking Changes 💥

This experimental fork introduces a series of breaking changes as part of the modernization effort:
- Projects must explicitly target ``netstandard2.0``.
- Drop support to the legacy *.csproj* format (only SDK-style projects)
- Drop support for legacy (and unused) command line arguments (check h5 -h for the current supported arguments)
- Compiler is now distributed as a ``dotnet global`` tool and have it's own versioning and auto-update on build (this can be disable by setting `<UpdateH5>false<UpdateH5/>` on your project file.
- **Retyped packages are not supported** (as those are maintained by the Bridge authors, and cannot be built separately or consumed without Bridge).

Other breaking changes will probably be introduced with the goal of supporting:
- Full multiplatform (Windows, Linux & MacOS) compilation without any need for Mono.
- Compiler-as-a-service mode - similar to how [Rosyln](https://github.com/dotnet/roslyn) can be hosted in process.

##  Update Notes 📑

To avoid any conflicts with the original Bridge ecosystem, we've renamed the base library and compiler in this repository, and they're distributed for now completely separated from the officially supported Bridge/Retyped packages.

The currently available NuGet packages are:
- [H5](https://www.nuget.org/packages/h5/) (replaces the base [Bridge](https://www.nuget.org/packages/Bridge/) library) 
- [H5.Core](https://www.nuget.org/packages/h5.core) (replaces [Retyped.Core](https://www.nuget.org/packages/Retyped.Core/), [Retyped.es5](https://www.nuget.org/packages/Retyped.es5/) and [Retyped.dom](https://www.nuget.org/packages/Retyped.dom/))
- [H5.Newtonsoft.Json](https://www.nuget.org/packages/h5.Newtonsoft.Json/) (replaces [Bridge.Newtonsoft.Json](https://www.nuget.org/packages/Bridge.Newtonsoft.Json/))

Other packages might be added in the future as we experiment with this fork, but we do not aim on providing any kind of Retyped replacement here.



