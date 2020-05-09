namespace System.Collections.Generic
{
    [H5.External]
    [H5.Reflectable]
    [H5.Convention(Target = H5.ConventionTarget.Member, Member = H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    public interface IReadOnlyList<out T> : IReadOnlyCollection<T>
    {
        T this[int index]
        {
            [H5.Template("System.Array.getItem({this}, {0}, {T})")]
            get;
        }
    }
}