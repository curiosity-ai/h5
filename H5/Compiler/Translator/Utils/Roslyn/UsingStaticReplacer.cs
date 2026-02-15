using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace H5.Translator
{
    public class UsingStaticReplacer : ICSharpReplacer
    {
        public SyntaxNode Replace(SyntaxNode root, SemanticModel model, SharpSixRewriter rewriter)
        {
            var removingUsings = new List<UsingDirectiveSyntax>();
            foreach (var u in root.DescendantNodes().OfType<UsingDirectiveSyntax>())
            {
                try
                {
                    if (u.StaticKeyword.IsKind(SyntaxKind.StaticKeyword) || u.Alias != null)
                    {
                        removingUsings.Add(u);
                    }
                }
                catch (Exception e)
                {
                    throw new ReplacerException(u, e);
                }
            }

            return root.RemoveNodes(removingUsings, SyntaxRemoveOptions.KeepDirectives);
        }
    }
}