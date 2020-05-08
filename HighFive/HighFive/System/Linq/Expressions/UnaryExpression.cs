using System.Reflection;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("[4,10,11,28,29,30,34,40,44,49,54,60,62,77,78,79,80,82,83,84].indexOf({this}.ntype) >= 0")]
    public sealed class UnaryExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Operand { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern MethodInfo Method { get; private set; }

        internal extern UnaryExpression();
    }
}