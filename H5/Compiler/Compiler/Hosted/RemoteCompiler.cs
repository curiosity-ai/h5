using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using H5.Translator;
using MagicOnion.Client;
using UID;

namespace H5.Compiler.Hosted
{
    public class RemoteCompiler
    {
        public readonly TimeSpan _timeout;

        public RemoteCompiler(Channel channel, TimeSpan timeout)
        {
            _client = MagicOnionClient.Create<IHostedCompiler>(channel);
            _timeout = timeout;
        }

        private IHostedCompiler _client;


        public async Task Ping(CancellationToken cancellationToken)
        {
            await _client.WithDeadline(DateTime.UtcNow.Add(TimeSpan.FromMilliseconds(500))).WithCancellationToken(cancellationToken).Ping();
        }

        public async Task<UID128> RequestCompilationAsync(CompilationRequest request, CancellationToken cancellationToken)
        {
            return await _client.WithDeadline(DateTime.UtcNow.Add(_timeout)).WithCancellationToken(cancellationToken).RequestCompilationAsync(request);
        }

        public async Task<CompilationResult> GetStatusAsync(UID128 compilationUID, CancellationToken cancellationToken)
        {
            return await _client.WithDeadline(DateTime.UtcNow.Add(_timeout)).WithCancellationToken(cancellationToken).GetStatusAsync(compilationUID);
        }

        public async Task AbortAsync(UID128 compilationUID, CancellationToken cancellationToken)
        {
            await _client.WithDeadline(DateTime.UtcNow.Add(_timeout)).WithCancellationToken(cancellationToken).AbortAsync(compilationUID);
        }
    }
}
