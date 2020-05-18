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
        private static readonly ILogger Logger = ApplicationLogging.CreateLogger<HostedCompiler>();

        public HostedCompiler()
        {
        }

        public async UnaryResult<Nil> AbortAsync(UID128 compilationUID)
        {
            Logger.ZLogInformation("==== ABORT {0}", compilationUID);
            CompilationProcessor.Abort(compilationUID);
            return Nil.Default;
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
            Logger.ZLogInformation("==== PING");
            return new UnaryResult<Nil>(Nil.Default);
        }

        public UnaryResult<UID128> RequestCompilationAsync(CompilationRequest request)
        {
            return new UnaryResult<UID128>(CompilationProcessor.Enqueue(request));
        }
    }
}
