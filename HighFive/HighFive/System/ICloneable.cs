namespace System
{
    [HighFive.External]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.Reflectable]
    public interface ICloneable : HighFive.IHighFiveClass
    {
        [HighFive.Template("HighFive.clone({this})")]
        object Clone();
    }
}