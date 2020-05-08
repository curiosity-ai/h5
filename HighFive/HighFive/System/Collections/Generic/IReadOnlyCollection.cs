using HighFive;

namespace System.Collections.Generic
{
    [HighFive.External]
    [HighFive.Reflectable]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    public interface IReadOnlyCollection<out T> : IEnumerable<T>
    {
        int Count
        {
            [HighFive.Template("System.Array.getCount({this}, {T})")]
            get;
        }
    }
}