using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagicOnion;
using MessagePack;
using UID;

namespace H5.Compiler.Hosted
{
    public interface IHostedCompiler: IService<IHostedCompiler>
    {
        UnaryResult<Nil> Ping();
        UnaryResult<UID128> RequestCompilationAsync(CompilationRequest request);
        UnaryResult<CompilationResult> GetStatusAsync(UID128 compilationUID);
        UnaryResult<Nil> AbortAsync(UID128 compilationUID);
    }
}
