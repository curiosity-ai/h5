using System.Runtime.InteropServices;
using System.Threading;

namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public interface IAsyncResult
    {
        object AsyncState { get; }
        bool CompletedSynchronously { get; }
        bool IsCompleted { get; }
    }
}
