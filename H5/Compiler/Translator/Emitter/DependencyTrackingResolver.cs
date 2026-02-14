using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;
using System;
using System.IO;

namespace H5.Translator
{
    public class DependencyTrackingResolver : IMemberResolver
    {
        private readonly IMemberResolver _inner;
        private readonly IEmitter _emitter;

        public Dictionary<string, HashSet<string>> Dependencies { get; } = new Dictionary<string, HashSet<string>>();

        public DependencyTrackingResolver(IMemberResolver inner, IEmitter emitter)
        {
            _inner = inner;
            _emitter = emitter;
        }

        public CSharpAstResolver Resolver => _inner.Resolver;

        public ICompilation Compilation => _inner.Compilation;

        public ResolveResult ResolveNode(AstNode node)
        {
            var result = _inner.ResolveNode(node);

            if (result != null && !_emitter.DisableDependencyTracking)
            {
                TrackDependency(result);
            }

            return result;
        }

        private void TrackDependency(ResolveResult result)
        {
            IType type = result.Type;
            if (type == null) return;

            // Resolve to definition to find the file
            var typeDef = type.GetDefinition();
            if (typeDef == null) return;

            // We use H5Types.Get(typeDef, true) to avoid exception if not found
            var h5Type = _emitter.H5Types.Get(typeDef, true);

            // System.Console.WriteLine($"TrackDependency: {type.FullName} (TypeDef: {typeDef.FullName}) H5Type: {h5Type != null} TypeInfo: {h5Type?.TypeInfo != null}");

            if (h5Type != null && h5Type.TypeInfo != null)
            {
                var refFile = h5Type.TypeInfo.FileName;

                if (string.IsNullOrEmpty(refFile) && h5Type.TypeInfo.TypeDeclaration != null)
                {
                     var tree = h5Type.TypeInfo.TypeDeclaration.GetParent<SyntaxTree>();
                     refFile = tree?.FileName;
                }

                var currentFile = _emitter.SourceFileName;

                if (!string.IsNullOrEmpty(refFile) && !string.IsNullOrEmpty(currentFile) && refFile != currentFile)
                {
                    // Check if refFile is part of the source files we are compiling
                    if (_emitter.SourceFiles.Contains(refFile))
                    {
                        if (!Dependencies.TryGetValue(currentFile, out var set))
                        {
                            set = new HashSet<string>();
                            Dependencies[currentFile] = set;
                        }
                        set.Add(refFile);
                    }
                }
            }
        }
    }
}
