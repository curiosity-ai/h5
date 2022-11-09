using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H5.Translator;
using MagicOnion;
using MagicOnion.Server;
using MessagePack;
using Microsoft.Extensions.Logging;
using Mosaik.Core;
using UID;
using ZLogger;

namespace H5.Compiler.Hosted
{
    public class HostedCompiler : ServiceBase<IHostedCompiler>, IHostedCompiler
    {
        private static readonly ILogger _logger = ApplicationLogging.CreateLogger<HostedCompiler>();

        public HostedCompiler()
        {
        }

        public UnaryResult<Nil> AbortAsync(UID128 compilationUID)
        {
            _logger.ZLogInformation("==== ABORT {0}", compilationUID);
            CompilationProcessor.Abort(compilationUID);
            return new UnaryResult<Nil>(Nil.Default);
        }

        public async UnaryResult<CompilationResult> GetStatusAsync(UID128 compilationUID)
        {
            var messages = Logging.GetMessagesOf(compilationUID).ToArray();
            return new CompilationResult()
            {
                Status = Logging.GetStatusOf(compilationUID),
                Messages = messages
            };
        }

        public UnaryResult<Nil> Ping()
        {
            _logger.ZLogInformation("==== PING");
            return new UnaryResult<Nil>(Nil.Default);
        }

        public UnaryResult<UID128> RequestCompilationAsync(CompilationRequest request)
        {
            return new UnaryResult<UID128>(CompilationProcessor.Enqueue(request));
        }
    }
}
