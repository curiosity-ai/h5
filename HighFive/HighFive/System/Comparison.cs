namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.Name("Function")]
    [HighFive.External]
    public delegate int Comparison<in T>(T x, T y);
}