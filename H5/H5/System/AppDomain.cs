using System.Reflection;

namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    public sealed class AppDomain
    {
        private extern AppDomain();

        public extern Assembly[] GetAssemblies();

        public static extern AppDomain CurrentDomain
        {
            [H5.Template("System.AppDomain")]
            get;
        }
    }
}