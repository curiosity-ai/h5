namespace System.Net.WebSockets
{
    [HighFive.External]
    [HighFive.Enum(HighFive.Emit.StringNameLowerCase)]
    public enum WebSocketMessageType
    {
        Text,
        Binary,
        Close,
    }
}