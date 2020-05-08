using System.ComponentModel;

namespace System
{
    [HighFive.External]
    public abstract class ValueType
    {
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public struct IntPtr
    {
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public struct UIntPtr
    {
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public class ParamArrayAttribute
    {
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public struct RuntimeTypeHandle
    {
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public struct RuntimeFieldHandle
    {
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [HighFive.NonScriptable]
    public struct RuntimeMethodHandle
    {
    }
}