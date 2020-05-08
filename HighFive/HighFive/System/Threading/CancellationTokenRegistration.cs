namespace System.Threading
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public struct CancellationTokenRegistration : IEquatable<CancellationTokenRegistration>, IDisposable
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern bool Equals(CancellationTokenRegistration other);

        public extern void Dispose();

        [H5.Template("H5.equals({left}, {right})")]
        public static extern bool operator ==(CancellationTokenRegistration left, CancellationTokenRegistration right);

        [H5.Template("!H5.equals({left}, {right})")]
        public static extern bool operator !=(CancellationTokenRegistration left, CancellationTokenRegistration right);
    }
}