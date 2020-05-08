using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace H5.Translator
{
    public interface ICSharpReplacer
    {
        SyntaxNode Replace(SyntaxNode root, SemanticModel model, SharpSixRewriter rewriter);
    }
}
