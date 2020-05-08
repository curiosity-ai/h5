namespace System
{
    [HighFive.External]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.Reflectable]
    public interface IFormattable : HighFive.IHighFiveClass
    {
        [HighFive.Name("format")]
        [HighFive.Template("HighFive.format({this}, {format}, {formatProvider})")]
        string ToString(string format, IFormatProvider formatProvider);
    }
}