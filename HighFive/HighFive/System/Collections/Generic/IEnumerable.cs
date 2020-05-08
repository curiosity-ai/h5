namespace System.Collections.Generic
{
    [HighFive.External]
    [HighFive.Reflectable]
    public interface IEnumerable<out T> : IEnumerable, HighFive.IHighFiveClass
    {
        [HighFive.Template("HighFive.getEnumerator({this}, {T})")]
        new IEnumerator<T> GetEnumerator();
    }
}