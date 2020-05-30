namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.NonScriptable]
    public interface IServiceProvider
    {
        object GetService(Type serviceType);
    }
}