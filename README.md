#  h5 - C# to JavaScript compiler, now on .NET Core 3.1 🚀

H5 is a modern fork of the original [Bridge](https://github.com/bridgedotnet/bridge) C# to Javascript compiler, updated to support multi-platform development using .NET Core 3.1 and .NET Standard 2.0 projects, while dropping support for legacy features and dependencies.

H5 is under active development, and targets a more integrated and faster development experience for C# web-developers. We're also planning to experiment with new ideas to improve compilation speed (such as aggressive caching of emitted code) and possibly integrating [C# Source Generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/)) for even faster code generation.


|  Package | NuGet           |  Azure DevOps   |
| -------------: |:-------------:| :-----:|
| Compiler | [![Nuget](https://img.shields.io/nuget/v/h5-compiler.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5-compiler/) |  [![Build Status](https://dev.azure.com/curiosity-ai/mosaik/_apis/build/status/h5-compiler?branchName=master)](https://dev.azure.com/curiosity-ai/mosaik/_build/latest?definitionId=38&branchName=master) |
| Base Library | [![Nuget](https://img.shields.io/nuget/v/h5.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5/) | [![Build Status](https://dev.azure.com/curiosity-ai/mosaik/_apis/build/status/h5-base-nuget?branchName=master)](https://dev.azure.com/curiosity-ai/mosaik/_build/latest?definitionId=39&branchName=master) |
| Core Library | [![Nuget](https://img.shields.io/nuget/v/h5.core.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.core/) |  [![Build Status](https://dev.azure.com/curiosity-ai/mosaik/_apis/build/status/h5.core?branchName=master)](https://dev.azure.com/curiosity-ai/mosaik/_build/latest?definitionId=40&branchName=master) |
| SDK Target | [![Nuget](https://img.shields.io/nuget/v/h5.target.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.target/) |  [![Build Status](https://dev.azure.com/curiosity-ai/mosaik/_apis/build/status/h5.target?branchName=master)](https://dev.azure.com/curiosity-ai/mosaik/_build/latest?definitionId=43&branchName=master) |
| Json Library | [![Nuget](https://img.shields.io/nuget/v/h5.Newtonsoft.Json.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.Newtonsoft.Json/) |  [![Build Status](https://dev.azure.com/curiosity-ai/mosaik/_apis/build/status/h5.json?branchName=master)](https://dev.azure.com/curiosity-ai/mosaik/_build/latest?definitionId=41&branchName=master) |
| dotnet template | [![Nuget](https://img.shields.io/nuget/v/h5.template.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/h5.template/) | [![Build Status](https://dev.azure.com/curiosity-ai/mosaik/_apis/build/status/h5.template?branchName=master)](https://dev.azure.com/curiosity-ai/mosaik/_build/latest?definitionId=44&branchName=master) |
| Tesserae UI toolkit | [![Nuget](https://img.shields.io/nuget/v/tesserae.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/tesserae/) | [![Build Status](https://dev.azure.com/curiosity-ai/mosaik/_apis/build/status/h5.tesserae?branchName=master)](https://dev.azure.com/curiosity-ai/mosaik/_build/latest?definitionId=42&branchName=master) |


##  Getting Started ⚡

H5 is fully based on [netstandard2.0](https://github.com/theolivenbaum/h5/blob/master/H5/H5/H5.csproj) & [netcore3.1](https://github.com/theolivenbaum/h5/blob/master/H5/Compiler/Builder/H5.Builder.csproj), and removes all dependencies on the legacy .NET Framework coming from the original source-code.

To get started with it, you can use the following project template  

````xml
<Project Sdk="h5.Target/0.0.*">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
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
dotnet new --install h5.Template::0.0.8152
````

And create a new project with:

````bash
dotnet new h5
```` 

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

Other packages might be added in the future as we experiment with this fork, but we do not aim on providing any kind of Retyped replacement here.
