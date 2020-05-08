namespace System
{
    [H5.External]
    [H5.Convention(Target = H5.ConventionTarget.Member, Member = H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.Reflectable]
    public interface IFormatProvider : H5.IH5Class
    {
        object GetFormat(Type formatType);
    }
}