using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace H5.Translator
{
    public static class AwaitersCollector
    {
        private class Analyzer : CSharpSyntaxWalker
        {
            private readonly SemanticModel _semanticModel;
            public HashSet<AwaitExpressionSyntax> Awaiters { get; }

            public Analyzer(SemanticModel semanticModel)
            {
                _semanticModel = semanticModel;
                Awaiters = new HashSet<AwaitExpressionSyntax>();
            }

            public void Analyze(SyntaxNode node)
            {
                Awaiters.Clear();
                Visit(node);
            }

            public override void VisitAwaitExpression(AwaitExpressionSyntax node)
            {
                Awaiters.Add(node);
                base.VisitAwaitExpression(node);
            }
        }

        public static bool HasAwaiters(SemanticModel semanticModel, SyntaxNode node)
        {
            var analyzer = new Analyzer(semanticModel);
            analyzer.Analyze(node);
            return analyzer.Awaiters.Count > 0;
        }
    }
}