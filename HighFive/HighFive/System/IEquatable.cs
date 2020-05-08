namespace System
{
    [HighFive.External]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.Reflectable]
    public interface IEquatable<T> : HighFive.IHighFiveClass
    {
        [HighFive.Template("HighFive.equalsT({this}, {other}, {T})")]
        [HighFive.Name("equalsT")]
        bool Equals(T other);
    }
}