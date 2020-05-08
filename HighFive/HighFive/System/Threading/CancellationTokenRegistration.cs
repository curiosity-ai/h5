namespace System.Threading
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public struct CancellationTokenRegistration : IEquatable<CancellationTokenRegistration>, IDisposable
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern bool Equals(CancellationTokenRegistration other);

        public extern void Dispose();

        [HighFive.Template("HighFive.equals({left}, {right})")]
        public static extern bool operator ==(CancellationTokenRegistration left, CancellationTokenRegistration right);

        [HighFive.Template("!HighFive.equals({left}, {right})")]
        public static extern bool operator !=(CancellationTokenRegistration left, CancellationTokenRegistration right);
    }
}