namespace System.Collections.Generic
{
    [H5.External]
    [H5.Reflectable]
    [H5.Convention(Target = H5.ConventionTarget.Member, Member = H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    public interface IList<T> : ICollection<T>
    {
        T this[int index]
        {
            [H5.Template("System.Array.getItem({this}, {index}, {T})")]
            get;
            [H5.Template("System.Array.setItem({this}, {index}, {value}, {T})")]
            set;
        }

        [H5.Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
        int IndexOf(T item);

        [H5.Template("System.Array.insert({this}, {index}, {item}, {T})")]
        void Insert(int index, T item);

        [H5.Template("System.Array.removeAt({this}, {index}, {T})")]
        void RemoveAt(int index);
    }
}