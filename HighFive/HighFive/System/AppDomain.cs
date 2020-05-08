using System.Reflection;

namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    public sealed class AppDomain
    {
        private extern AppDomain();

        public extern Assembly[] GetAssemblies();

        public static extern AppDomain CurrentDomain
        {
            [HighFive.Template("System.AppDomain")]
            get;
        }
    }
}