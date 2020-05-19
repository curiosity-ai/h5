using Cysharp.Text;
using H5.Compiler.Hosted;
using MessagePack;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UID;
using ZLogger;

namespace H5.Compiler
{
    internal static class Logging
    {
        private readonly static ConcurrentDictionary<UID128, ConcurrentQueue<LogMessage>> _perCompilation = new ConcurrentDictionary<UID128, ConcurrentQueue<LogMessage>>();
        private readonly static ConcurrentDictionary<UID128, CompilationStatus> _perCompilationStatus = new ConcurrentDictionary<UID128, CompilationStatus>();

        private static UID128 _currentUID = default;

        internal static ZLoggerOptions options = new ZLoggerOptions()
        {
            PrefixFormatter = (buf, info) => ZString.Utf8Format(buf, "[{0}] [{1}:{2}:{3}] ", Logging.GetLogLevelString(info.LogLevel), info.Timestamp.LocalDateTime.Hour, info.Timestamp.LocalDateTime.Minute, info.Timestamp.LocalDateTime.Second)
        };

        internal static string GetLogLevelString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace: return "trce";
                case LogLevel.Debug: return "dbug";
                case LogLevel.Information: return "info";
                case LogLevel.Warning: return "warn";
                case LogLevel.Error: return "fail";
                case LogLevel.Critical: return "crit";
                case LogLevel.None: return "none";
                default: throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public static CompilationStatus GetStatusOf(UID128 uid)
        {
            if(_perCompilationStatus.TryGetValue(uid, out var status))
            {
                return status;
            }
            return CompilationStatus.Pending;
        }

        public static IEnumerable<LogMessage> GetMessagesOf(UID128 uid)
        {
            if(!_perCompilation.TryGetValue(uid, out var queue))
            {
                yield break;
            }

            while(queue.TryDequeue(out var message))
            {
                yield return message;
            }
        }

        private static void WriteEntry(IZLoggerEntry entry)
        {
            var boxedBuilder = (IBufferWriter<byte>)new Utf8ValueStringBuilder(false);
            try
            {
                entry.FormatUtf8(boxedBuilder, options, null);
                
                var message = boxedBuilder.ToString();

                var marker = message.IndexOf("====");

                if(marker >= 0)
                {
                    var content = message.Substring(marker + 5);
                    if(content.StartsWith("RCV"))
                    {
                        var uid = UID128.Parse(content.Substring(4, 22));
                        _perCompilationStatus[uid] = CompilationStatus.Pending;
                    }
                    else if(content.StartsWith("BEGIN"))
                    {
                        _currentUID = UID128.Parse(content.Substring(6, 22));
                        _perCompilation[_currentUID] = new ConcurrentQueue<LogMessage>();
                        _perCompilationStatus[_currentUID] = CompilationStatus.OnGoing;
                    }
                    else if (content.StartsWith("SUCCESS"))
                    {
                        _perCompilationStatus[_currentUID] = CompilationStatus.Success;
                        _currentUID = default;
                    }
                    else if (content.StartsWith("FAIL"))
                    {
                        _perCompilationStatus[_currentUID] = CompilationStatus.Fail;
                        _currentUID = default;
                    }
                    else if (content.StartsWith("ABORT"))
                    {
                        _perCompilationStatus[_currentUID] = CompilationStatus.Fail;
                        _currentUID = default;
                    }
                    else if (content.StartsWith("CANCELED"))
                    {
                        _perCompilationStatus[_currentUID] = CompilationStatus.Fail;
                        _currentUID = default;
                    }
                }
                else
                {
                    if (_currentUID.IsNotNull())
                    {
                        _perCompilation[_currentUID].Enqueue(new LogMessage()
                        {
                            Message = message,
                            Timestamp = entry.LogInfo.Timestamp.ToUnixTimeMilliseconds(),
                            LogLevel = entry.LogInfo.LogLevel
                        });
                    }
                }
            }
            finally
            {
            }
        }

        internal static string RemoveTimeAndLogLevel(string message)
        {
            var first  = message.IndexOf(']');
            if(first > 0)
            {
                var second = message.IndexOf(']', first+1);
                if(second > 0)
                {
                    return message.Substring(second + 1);
                }
            }
            return message;
        }

        internal class InMemoryPerCompilationProvider : IAsyncLogProcessor
        {
            public ValueTask DisposeAsync()
            {
                return default;
            }

            public void Post(IZLoggerEntry log)
            {
                WriteEntry(log);
            }
        }
    }
}