namespace System.Collections.Generic
{
    [H5.External]
    [H5.Reflectable]
    public interface IEnumerable<out T> : IEnumerable, H5.IH5Class
    {
        [H5.Template("H5.getEnumerator({this}, {T})")]
        new IEnumerator<T> GetEnumerator();
    }
}