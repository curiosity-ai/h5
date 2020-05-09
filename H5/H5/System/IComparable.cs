namespace System
{
    [H5.External]
    [H5.Convention(Target = H5.ConventionTarget.Member, Member = H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.Reflectable]
    public interface IComparable : H5.IH5Class
    {
        [H5.Template("H5.compare({this}, {obj})")]
        int CompareTo(Object obj);
    }

    [H5.External]
    [H5.Convention(Target = H5.ConventionTarget.Member, Member = H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.Reflectable]
    public interface IComparable<in T> : H5.IH5Class
    {
        [H5.Template("H5.compare({this}, {other}, false, {T})")]
        int CompareTo(T other);
    }
}