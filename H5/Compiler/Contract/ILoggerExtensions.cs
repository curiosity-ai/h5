using Microsoft.Extensions.Logging;
using ZLogger;

namespace H5.Contract
{
    public static class ILoggerExtensions
    {
        public static void Error(this ILogger logger, string message, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber)
        {
            logger.ZLogError("{0}({1},{2}) : error BR001: {3}", file, lineNumber, columnNumber, message);
        }
    }
}