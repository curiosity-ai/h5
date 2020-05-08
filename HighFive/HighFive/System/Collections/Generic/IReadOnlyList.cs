namespace System.Collections.Generic
{
    [HighFive.External]
    [HighFive.Reflectable]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    public interface IReadOnlyList<out T> : IReadOnlyCollection<T>
    {
        T this[int index]
        {
            [HighFive.Template("System.Array.getItem({this}, {0}, {T})")]
            get;
        }
    }
}