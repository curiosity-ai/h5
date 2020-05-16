using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using H5.Contract;

namespace H5.Translator
{
    public class LoopVariablesAnalyzer : DepthFirstAstVisitor
    {
        public List<string> VariableNames { get; } = new List<string>();

        public List<IVariable> Variables { get; } = new List<IVariable>();

        public IEmitter Emitter { get; set; }

        public LoopVariablesAnalyzer(IEmitter emitter, bool excludeReadOnly)
        {
            Emitter = emitter;
            ExcludeReadOnly = excludeReadOnly;
        }

        public bool ExcludeReadOnly { get; set; }

        public void Analyze(AstNode node)
        {
            if (node is ForStatement)
            {
                node = ((ForStatement)node).EmbeddedStatement;
            }

            VariableNames.Clear();

            if (node is ForeachStatement && !ExcludeReadOnly)
            {
                var foreachStatement = (ForeachStatement)node;

                if (foreachStatement.VariableNameToken != null && !foreachStatement.VariableNameToken.IsNull)
                {
                    VariableNames.Add(foreachStatement.VariableName);
                    var rr = (ForEachResolveResult)Emitter.Resolver.ResolveNode(foreachStatement);
                    Variables.Add(rr.ElementVariable);
                }
            }

            node.AcceptVisitor(this);
        }

        public override void VisitForeachStatement(ForeachStatement foreachStatement)
        {
            if (foreachStatement.VariableNameToken != null && !foreachStatement.VariableNameToken.IsNull)
            {
                VariableNames.Add(foreachStatement.VariableName);
                var rr = (ForEachResolveResult)Emitter.Resolver.ResolveNode(foreachStatement);
                Variables.Add(rr.ElementVariable);
            }

            base.VisitForeachStatement(foreachStatement);
        }

        public override void VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement)
        {
            foreach (var variable in variableDeclarationStatement.Variables)
            {
                VariableNames.Add(variable.Name);
                var lrr = (LocalResolveResult)Emitter.Resolver.ResolveNode(variable);
                Variables.Add(lrr.Variable);
            }
            base.VisitVariableDeclarationStatement(variableDeclarationStatement);
        }

        public override void VisitCatchClause(CatchClause catchClause)
        {
            if (!ExcludeReadOnly && catchClause.VariableNameToken != null && !catchClause.VariableNameToken.IsNull)
            {
                VariableNames.Add(catchClause.VariableName);
                var lrr = (LocalResolveResult)Emitter.Resolver.ResolveNode(catchClause.VariableNameToken);
                Variables.Add(lrr.Variable);
            }

            base.VisitCatchClause(catchClause);
        }

        public override void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
            //base.VisitLambdaExpression(lambdaExpression);
        }

        public override void VisitAnonymousMethodExpression(AnonymousMethodExpression anonymousMethodExpression)
        {
            //base.VisitAnonymousMethodExpression(anonymousMethodExpression);
        }
    }
}