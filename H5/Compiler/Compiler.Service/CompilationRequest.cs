using H5.Contract;
using H5.Translator;
using MessagePack;
using NuGet.Versioning;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace H5.Compiler.Hosted
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class CompilationRequest
    {
        public CompilationRequest(string assemblyName, H5DotJson_AssemblySettings settings)
        {
            AssemblyName = assemblyName;
            Settings = settings;
        }

        private Dictionary<string, string> SourceCode { get; set; } = new Dictionary<string, string>();
        private Dictionary<string, string> References { get; set; } = new Dictionary<string, string>();

        public string AssemblyName { get; set; } = "App";

        public bool SkipResourcesExtraction { get; set; } = false;

        public bool SkipEmbeddingResources { get; set; } = true;

        public bool SkipHtmlGeneration { get; set; } = false;


        public H5DotJson_AssemblySettings Settings { get; set; } 

        private ProjectProperties ProjectProperties { get; set; } = new ProjectProperties();

        public CompilationRequest WithSourceFile(string fileName, string code)
        {
            SourceCode.Add(fileName, code);
            return this;
        }

        public CompilationRequest NoHTML()
        {
            SkipHtmlGeneration = true;
            return this;
        }
        
        public CompilationRequest NoPackageResources()
        {
            SkipResourcesExtraction = true;
            return this;
        }

        public CompilationRequest WithPackageReference(string packageId, NuGetVersion nuGetVersion)
        {
            References[packageId] =  $"<PackageReference Include=\"{packageId}\" Version=\"{nuGetVersion.ToString()}\" />";
            return this;
        }

        internal CompilationOptions ToOptions(string sourceDirectory, NuGetVersion sdkTargetVersion)
        {
            foreach(var (file, code) in SourceCode)
            {
                var fileName = Path.Combine(sourceDirectory, file);
                File.WriteAllText(fileName, code);
            }
            
            var projFile = Path.Combine(sourceDirectory, "auto-generated-project.csproj");

            File.WriteAllText(projFile,
@"

<Project Sdk=""h5.Target/$(SDKTARGET)"">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <AssemblyName>$(ASSEMBLYNAME)</AssemblyName>
    <GeneratePackageOnBuild>false</GeneratePackageOnBuild>
    <DebugType>None</DebugType>
    <DebugSymbols>false</DebugSymbols>
    <$(SER)>$(SERVAL)</$(SER)>
    <$(SHG)>$(SHGVAL)</$(SHG)>
    <$(SRE)>$(SREVAL)</$(SRE)>
  </PropertyGroup>

  <ItemGroup>
$(PKGREF)
  </ItemGroup>
</Project>"

.Replace("$(PKGREF)", string.Join("\n", References.Values))
.Replace("$(SDKTARGET)", sdkTargetVersion.ToString())
.Replace("$(ASSEMBLYNAME)", AssemblyName)
.Replace("$(SER)", H5.Translator.Translator.ProjectPropertyNames.H5_Specific.SkipEmbeddingResources)
.Replace("$(SERVAL)", SkipEmbeddingResources ? "true" : "false")
.Replace("$(SHG)", H5.Translator.Translator.ProjectPropertyNames.H5_Specific.SkipHtmlGeneration)
.Replace("$(SHGVAL)", SkipHtmlGeneration ? "true" : "false")
.Replace("$(SRE)", H5.Translator.Translator.ProjectPropertyNames.H5_Specific.SkipResourcesExtraction)
.Replace("$(SREVAL)", SkipResourcesExtraction ? "true" : "false")
);

            return new CompilationOptions()
            {
                ProjectLocation = projFile,
                DefaultFileName = AssemblyName,
                H5Location = null,
                Rebuild = true,
                ProjectProperties = ProjectProperties
            };
        }
    }
}
