namespace System.Collections
{
    [HighFive.External]
    [HighFive.Reflectable]
    public interface IEnumerable : HighFive.IHighFiveClass
    {
        [HighFive.Template("HighFive.getEnumerator({this})")]
        IEnumerator GetEnumerator();
    }
}