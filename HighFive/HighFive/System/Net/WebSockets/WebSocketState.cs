namespace System.Net.WebSockets
{
    [H5.External]
    [H5.Enum(H5.Emit.StringNameLowerCase)]
    public enum WebSocketState
    {
        None = 0,
        Connecting = 1,
        Open = 2,
        CloseSent = 3,
        CloseReceived = 4,
        Closed = 5,
        Aborted = 6,
    }
}