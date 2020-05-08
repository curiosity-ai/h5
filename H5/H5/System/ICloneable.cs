namespace System
{
    [H5.External]
    [H5.Convention(Target = H5.ConventionTarget.Member, Member = H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.Reflectable]
    public interface ICloneable : H5.IH5Class
    {
        [H5.Template("H5.clone({this})")]
        object Clone();
    }
}