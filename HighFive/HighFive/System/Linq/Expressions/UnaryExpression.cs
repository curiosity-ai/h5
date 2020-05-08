using System.Reflection;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("[4,10,11,28,29,30,34,40,44,49,54,60,62,77,78,79,80,82,83,84].indexOf({this}.ntype) >= 0")]
    public sealed class UnaryExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Operand { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern MethodInfo Method { get; private set; }

        internal extern UnaryExpression();
    }
}