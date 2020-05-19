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

        private static readonly ConcurrentDictionary<UID128, CancellationTokenSource> _abortTokens = new ConcurrentDictionary<UID128, CancellationTokenSource>();
        private static readonly BlockingCollection<(CompilationRequest request, UID128 uid)> _compilationQueue = new BlockingCollection<(CompilationRequest request, UID128 uid)>();
        
        internal static UID128 _currentCompilation;

        internal static void Abort(UID128 compilationUID)
        {
            if(_abortTokens.TryGetValue(compilationUID, out var token))
            {
                token.Cancel();
            }
        }

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
                    var abortToken = _abortTokens[request.uid];
                    _currentCompilation = request.uid;
                    Logger.ZLogInformation("\n\n\n\n");
                    Logger.ZLogInformation("==== BEGIN {0}", request.uid);
                    Logger.ZLogInformation("Setting working directory to '{0}'", request.request.WorkingDirectory);
                    
                    var beforeDir = Environment.CurrentDirectory;

                    Environment.CurrentDirectory = request.request.WorkingDirectory;

                    var cts = CancellationTokenSource.CreateLinkedTokenSource(abortToken.Token, cancellationToken);
                    try
                    {
                        Compile(request.request, request.uid, cts.Token);
                    }
                    catch (Exception E)
                    {
                        // This should never happen, as Compile already handles exceptions 
                        Logger.LogError(E, "H5 Compiler fail"); 
                    }
                    finally
                    {
                        Environment.CurrentDirectory = beforeDir;
                    }

                    Logger.ZLogInformation("==== END {0}", request.uid);
                    Logger.ZLogInformation("\n\n\n\n");
                    _currentCompilation = default;

                    if(cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
                Logger.ZLogInformation("==== HOST Finished compilation thread");
                _compilationFinished.SetResult(new object());
            });

            thread.Start();
        }

        public static int Compile(CompilationRequest compilationRequest, UID128 compilationUID, CancellationToken cancellationToken)
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
                    if (compilationUID.IsNotNull())
                    {
                        Logger.ZLogInformation("==== SUCCESS {0}", compilationUID);
                    }
                    return 0;
                }
            }
            catch (OperationCanceledException)
            {
                //Ignore, probably compilation aborted
                if (compilationUID.IsNotNull())
                {
                    Logger.ZLogInformation("==== CANCELED {0}", compilationUID);
                }
                return 0;
            }
            catch (EmitterException ex)
            {
                LogErrorMarker();

                Logger.LogError(string.Format("H5 Compiler error: {1} ({2}, {3}) {0}", ex.ToString(), ex.FileName, ex.StartLine, ex.StartColumn));
                
                if (ex.StackTrace is object)
                {
                    Logger.LogError(ex.StackTrace.ToString());
                }

                if (compilationUID.IsNotNull())
                {
                    Logger.ZLogInformation("==== FAIL {0}", compilationUID);
                }
                return 1;
            }
            catch (Exception ex)
            {
                var ee = processor.Translator?.CreateExceptionFromLastNode();

                LogErrorMarker();

                if (ee != null)
                {
                    Logger.LogError(string.Format("H5 Compiler error: {1} ({2}, {3}) {0}", ee.ToString(), ee.FileName, ee.StartLine, ee.StartColumn));
                }
                else
                {
                    Logger.LogError(string.Format("H5 Compiler error: {0}", ex.ToString()));
                }

                if(ex.StackTrace is object)
                {
                    Logger.LogError(ex.StackTrace.ToString());
                }

                if (compilationUID.IsNotNull())
                {
                    Logger.ZLogInformation("==== FAIL {0}", compilationUID);
                }

                return 1;
            }
        }

        private static void LogErrorMarker()
        {
            Logger.ZLogInformation(
@"

 __ _  _  _  _ 
|_ |_)|_)/ \|_)
|__| \| \\_/| \


");
        }

        internal static UID128 Enqueue(CompilationRequest request)
        {
            var uid = UID128.New();
            Logger.ZLogInformation("==== RCV {0}", uid);
            _abortTokens[uid] = new CancellationTokenSource();
            _compilationQueue.Add((request, uid));
            return uid;
        }
    }
}