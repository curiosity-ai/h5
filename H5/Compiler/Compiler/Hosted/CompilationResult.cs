using MessagePack;
using Microsoft.Extensions.Logging;
using System;

namespace H5.Compiler.Hosted
{
    [MessagePackObject(keyAsPropertyName:true)]
    public class CompilationResult
    {
        public CompilationStatus Status { get; set; }

        public LogMessage[] Messages { get; set; }

        internal CompilationResult OnGoing(LogMessage[] messages)
        {
            Status = CompilationStatus.OnGoing;
            Messages = messages;
            return this;
        }

        internal CompilationResult Success()
        {
            Status = CompilationStatus.Success;
            Messages = null;
            return this;
        }

        internal CompilationResult Fail()
        {
            Status = CompilationStatus.Fail;
            Messages = null;
            return this;
        }
    }

    [MessagePackObject]
    public class LogMessage
    {
        public LogMessage()
        {

        }
        public LogMessage(LogLevel logLevel, string logName, int eventId, string message, string exceptionMessage)
        {
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            LogLevel = logLevel;
            LogName = logName;
            EventId = eventId;
            Message = message;
            ExceptionMessage = exceptionMessage;
        }

        [Key(0)] public long Timestamp { get; set; }
        [Key(1)] public LogLevel LogLevel { get; set; }
        [Key(2)] public string LogName { get; set; }
        [Key(3)] public int EventId { get; set; }
        [Key(4)] public string Message { get; set; }
        [Key(5)] public string ExceptionMessage { get; set; }
    }

}
