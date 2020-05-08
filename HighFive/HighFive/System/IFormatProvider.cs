namespace System
{
    [HighFive.External]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.Reflectable]
    public interface IFormatProvider : HighFive.IHighFiveClass
    {
        object GetFormat(Type formatType);
    }
}