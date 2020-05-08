using System.Runtime.InteropServices;
using System.Threading;

namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public interface IAsyncResult
    {
        object AsyncState { get; }
        bool CompletedSynchronously { get; }
        bool IsCompleted { get; }
    }
}
