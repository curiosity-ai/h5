using H5.Compiler.Hosted;
using H5.Contract;
using H5.Translator;
using ICSharpCode.NRefactory.TypeSystem;
using MagicOnion;
using Microsoft.Extensions.Logging;
using Mosaik.Core;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UID;
using ZLogger;

namespace H5.Compiler
{
    public sealed class CompilationProcessor
    {
        private static readonly ILogger Logger = ApplicationLogging.CreateLogger<CompilationProcessor>();

        private static readonly BlockingCollection<(CompilationRequest request, UID128 uid)> _compilationQueue = new BlockingCollection<(CompilationRequest request, UID128 uid)>();
        
        internal static UID128 _currentCompilation;

        private static TaskCompletionSource<object> _compilationFinished = new TaskCompletionSource<object>();

        public static async Task StopAsync()
        {
            _compilationQueue.CompleteAdding();
            await _compilationFinished.Task;
        }

        public static void CompileForever(CancellationToken cancellationToken)
        {
            var thread = new Thread(() =>
            {
                Logger.ZLogInformation("==== HOST Starting compilation thread");
                foreach (var request in _compilationQueue.GetConsumingEnumerable())
                {
                    _currentCompilation = request.uid;
                    Logger.ZLogInformation("\n\n\n\n");
                    Logger.ZLogInformation("==== BEGIN {0}", request.uid);
                    Compile(request.request, request.uid, cancellationToken);
                    Logger.ZLogInformation("==== END {0}", request.uid);
                    Logger.ZLogInformation("\n\n\n\n");
                    _currentCompilation = default;
                }
                Logger.ZLogInformation("==== HOST Finished compilation thread");
                _compilationFinished.SetResult(new object());
            });

            thread.Start();
        }

        public static void Compile(CompilationRequest compilationRequest, UID128 compilationUID, CancellationToken cancellationToken)
        {
            var compilationOptions = compilationRequest.ToOptions();

            var processor = new TranslatorProcessor(compilationOptions, cancellationToken);
            
            try
            {
                using (new Measure(Logger, "H5 Compilation"))
                {
                    processor.PreProcess();
                    processor.Process();
                    processor.PostProcess();
                    Logger.ZLogInformation("==== SUCCESS {0}", compilationUID);
                }
            }
            catch (EmitterException ex)
            {
                Logger.LogError(string.Format("H5 Compiler error: {1} ({2}, {3}) {0}", ex.ToString(), ex.FileName, ex.StartLine, ex.StartColumn));
                Logger.LogError(ex.StackTrace.ToString());
                Logger.ZLogInformation("==== FAIL {0}", compilationUID);
            }
            catch (Exception ex)
            {
                var ee = processor.Translator?.CreateExceptionFromLastNode();

                if (ee != null)
                {
                    Logger.LogError(string.Format("H5 Compiler error: {1} ({2}, {3}) {0}", ee.ToString(), ee.FileName, ee.StartLine, ee.StartColumn));
                }
                else
                {
                    Logger.LogError(string.Format("H5 Compiler error: {0}", ex.ToString()));
                }

                Logger.LogError(ex.StackTrace.ToString());

                Logger.ZLogInformation("==== FAIL {0}", compilationUID);
            }
        }

        internal static UID128 Enqueue(CompilationRequest request)
        {
            var uid = UID128.New();
            Logger.ZLogInformation("==== RCV {0}", uid);
            _compilationQueue.Add((request, uid));
            return uid;
        }
    }
}