using ICSharpCode.NRefactory.CSharp;
using Mono.Cecil;
using System.Collections.Generic;
using System.Threading;

namespace H5.Contract
{
    public interface ITranslator
    {
        AssemblyDefinition AssemblyDefinition { get; set; }

        IH5DotJson_AssemblySettings AssemblyInfo { get; set; }

        string AssemblyLocation { get; }

        string H5Location { get; set; }

        ProjectProperties ProjectProperties { get; set; }

        string DefaultNamespace { get; set; }

        string Location { get; }

        string MSBuildVersion { get; set; }

        TranslatorOutput Outputs { get; }

        bool Rebuild { get; set; }

        void Save(string path, string defaultFileName);

        IList<string> SourceFiles { get; }

        void Translate(CancellationToken cancellationToken);

        Dictionary<string, ITypeInfo> TypeInfoDefinitions { get; set; }

        List<ITypeInfo> Types { get; }

        IValidator Validator { get; }

        H5Types H5Types { get; set; }

        AstNode EmitNode { get; set; }

        EmitterException CreateExceptionFromLastNode();

        List<string> DefineConstants { get; set; }

        IEnumerable<AssemblyDefinition> References { get; set; }

        /// <summary>
        /// Indicates whether strict mode will be added to generated script files
        /// </summary>
        bool NoStrictMode { get; set; }

        Dictionary<string, int> Stats { get; set; }

        VersionContext GetVersionContext();
    }
}