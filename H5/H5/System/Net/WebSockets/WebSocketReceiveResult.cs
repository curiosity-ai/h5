namespace System.Net.WebSockets
{
    /// <summary>
    /// An instance of this class represents the result of performing a single ReceiveAsync operation on a WebSocket.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public class WebSocketReceiveResult
    {
        public extern WebSocketReceiveResult(int count, WebSocketMessageType messageType, bool endOfMessage);

        public extern WebSocketReceiveResult(int count, WebSocketMessageType messageType, bool endOfMessage,
            WebSocketCloseStatus? closeStatus, string closeStatusDescription);

        /// <summary>
        /// Indicates the number of bytes that the WebSocket received.
        /// </summary>
        public extern int Count
        {
            [H5.Template("getCount()")]
            get;
        }

        /// <summary>
        /// Indicates whether the message has been received completely.
        /// </summary>
        public extern bool EndOfMessage
        {
            [H5.Template("getEndOfMessage()")]
            get;
        }

        /// <summary>
        /// Indicates whether the current message is a UTF-8 message or a binary message.
        /// </summary>
        public extern WebSocketMessageType MessageType
        {
            [H5.Template("getMessageType()")]
            get;
        }

        /// <summary>
        /// Indicates the reason why the remote endpoint initiated the close handshake.
        /// </summary>
        public extern WebSocketCloseStatus? CloseStatus
        {
            [H5.Template("getCloseStatus()")]
            get;
        }

        /// <summary>
        /// Returns the optional description that describes why the close handshake has been initiated by the remote endpoint.
        /// </summary>
        public extern string CloseStatusDescription
        {
            [H5.Template("getCloseStatusDescription()")]
            get;
        }
    }
}