using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using Mono.Cecil;
using System;
using System.Collections.Generic;

namespace H5.Translator
{
    public partial class Translator
    {
        public IAssemblyInfo AssemblyInfo { get; set; }

        public AssemblyDefinition AssemblyDefinition { get; set; }

        public IValidator Validator { get; private set; }

        public string H5Location { get; set; }

        public Dictionary<string, string> PackageReferencesDiscoveredPaths { get; set; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public string Location
        {
            get;
            protected set;
        }

        public string AssemblyLocation
        {
            get;
            protected set;
        }

        public ProjectProperties ProjectProperties { get; set; }

        public string DefaultNamespace { get; set; }

        public string BuildArguments { get; set; }

        private string msbuildVersion = "4.0.30319";

        public string MSBuildVersion
        {
            get
            {
                return msbuildVersion;
            }
            set
            {
                msbuildVersion = value;
            }
        }

        public IList<string> SourceFiles
        {
            get;
            protected set;
        }

        public ParsedSourceFile[] ParsedSourceFiles
        {
            get;
            protected set;
        }

        private bool rebuild = true;

        public bool Rebuild
        {
            get
            {
                return rebuild;
            }
            set
            {
                rebuild = value;
            }
        }

        protected Dictionary<string, TypeDefinition> TypeDefinitions { get; set; }

        public Dictionary<string, ITypeInfo> TypeInfoDefinitions { get; set; }

        public List<ITypeInfo> Types
        {
            get;
            protected set;
        }

        public TranslatorOutput Outputs
        {
            get;
            protected set;
        }

        public IEmitterOutputs EmitterOutputs { get; set; }

        public IPlugins Plugins { get; set; }

        public H5Types H5Types { get; set; }

        public AstNode EmitNode { get; set; }

        public List<string> DefineConstants { get; set; }

        public bool FromTask { get; set; }

        public virtual IEnumerable<AssemblyDefinition> References { get; set; }

        /// <summary>
        /// Indicates whether strict mode will be added to generated script files
        /// </summary>
        public bool NoStrictMode { get; set; }

        public string[] SkipPluginAssemblies { get; set; }

        public OverflowMode? OverflowMode { get; set; }

        public HashSet<string> ExtractedScripts
        {
            get; set;
        } = new HashSet<string>();
    }
}