namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.Name("Function")]
    [H5.External]
    public delegate int Comparison<in T>(T x, T y);
}