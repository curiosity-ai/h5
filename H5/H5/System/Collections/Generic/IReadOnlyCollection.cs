using H5;

namespace System.Collections.Generic
{
    [H5.External]
    [H5.Reflectable]
    [H5.Convention(Target = H5.ConventionTarget.Member, Member = H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    public interface IReadOnlyCollection<out T> : IEnumerable<T>
    {
        int Count
        {
            [H5.Template("System.Array.getCount({this}, {T})")]
            get;
        }
    }
}