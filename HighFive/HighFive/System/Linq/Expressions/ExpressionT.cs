using System.Collections.Generic;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 18")]
    [HighFive.IgnoreGeneric]
    public sealed class Expression<TDelegate> : LambdaExpression
    {
        public extern Expression<TDelegate> Update(Expression body, IEnumerable<ParameterExpression> parameters);

        internal extern Expression();
    }
}