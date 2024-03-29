using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Contract
{
    public interface IMemberResolver
    {
        ResolveResult ResolveNode(AstNode node);

        CSharpAstResolver Resolver { get; }

        ICompilation Compilation { get; }
    }
}