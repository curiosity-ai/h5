namespace System.Collections.Generic
{
    [HighFive.External]
    [HighFive.Reflectable]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    public interface IList<T> : ICollection<T>
    {
        T this[int index]
        {
            [HighFive.Template("System.Array.getItem({this}, {index}, {T})")]
            get;
            [HighFive.Template("System.Array.setItem({this}, {index}, {value}, {T})")]
            set;
        }

        [HighFive.Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
        int IndexOf(T item);

        [HighFive.Template("System.Array.insert({this}, {index}, {item}, {T})")]
        void Insert(int index, T item);

        [HighFive.Template("System.Array.removeAt({this}, {index}, {T})")]
        void RemoveAt(int index);
    }
}