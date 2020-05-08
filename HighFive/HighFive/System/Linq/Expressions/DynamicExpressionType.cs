namespace System.Linq.Expressions
{
    [HighFive.External]
    [HighFive.Enum(HighFive.Emit.Value)]
    public enum DynamicExpressionType
    {
        MemberAccess,
        Invocation,
        Index
    }
}