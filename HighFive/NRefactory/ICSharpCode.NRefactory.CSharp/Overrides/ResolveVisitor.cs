using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using ICSharpCode.NRefactory.CSharp.Analysis;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.CSharp.Resolver
{
    sealed class ResolveVisitor : IAstVisitor<ResolveResult>
    {
        IMember GetMemberFromLocation(AstNode node)
        {
            ITypeDefinition typeDef = resolver.CurrentTypeDefinition;
            if (typeDef == null)
                return null;
            TextLocation location = TypeSystemConvertVisitor.GetStartLocationAfterAttributes(node);
            return typeDef.GetMembers(
                delegate (IUnresolvedMember m) {
                    if (m.UnresolvedFile.FileName != unresolvedFile.FileName)
                        return false;
                    DomRegion region = m.Region;
                    return !region.IsEmpty && region.Begin <= location && region.End > location;
                },
                GetMemberOptions.IgnoreInheritedMembers | GetMemberOptions.ReturnMemberDefinitions
            ).FirstOrDefault();
        }
    }
}
