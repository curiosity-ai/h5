namespace System.Collections
{
    [H5.External]
    [H5.Reflectable]
    public interface IEnumerable : H5.IH5Class
    {
        [H5.Template("H5.getEnumerator({this})")]
        IEnumerator GetEnumerator();
    }
}