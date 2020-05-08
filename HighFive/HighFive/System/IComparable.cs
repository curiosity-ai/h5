namespace System
{
    [HighFive.External]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.Reflectable]
    public interface IComparable : HighFive.IHighFiveClass
    {
        [HighFive.Template("HighFive.compare({this}, {obj})")]
        int CompareTo(Object obj);
    }

    [HighFive.External]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.Reflectable]
    public interface IComparable<in T> : HighFive.IHighFiveClass
    {
        [HighFive.Template("HighFive.compare({this}, {other}, false, {T})")]
        int CompareTo(T other);
    }
}