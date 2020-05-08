namespace System.Linq.Expressions
{
    [HighFive.External]
    [HighFive.Enum(HighFive.Emit.Value)]
    public enum GotoExpressionKind
    {
        Goto,
        Return,
        Break,
        Continue,
    }
}