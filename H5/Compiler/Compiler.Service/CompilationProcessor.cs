using H5.Compiler.Hosted;
using H5.Contract;
using H5.Translator;
using ICSharpCode.NRefactory.TypeSystem;
using MagicOnion;
using Microsoft.Extensions.Logging;
using Mosaik.Core;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UID;
using ZLogger;

namespace H5.Compiler
{

    public sealed class CompilationProcessor
    {
        private static readonly object _threadLock = new object();
        private static Thread _thread = null;
        private static readonly ILogger Logger = ApplicationLogging.CreateLogger<CompilationProcessor>();

        private static readonly BlockingCollection<(CompilationRequest request, CancellationToken token, TaskCompletionSource<CompilationOutput> output)> _compilationQueue = new BlockingCollection<(CompilationRequest request, CancellationToken token, TaskCompletionSource<CompilationOutput> output)>();

        private static CancellationTokenSource _tcs = new CancellationTokenSource();
        private static TaskCompletionSource<object> _compilationFinished = new TaskCompletionSource<object>();

        public static async Task StopAsync()
        {
            if (_thread is null) return;

            _compilationQueue.CompleteAdding();
            _tcs?.Cancel();
            await _compilationFinished.Task;
        }
        
        private static void StartCompilationThreadIfNeeded()
        {
            if(_thread is null)
            {
                lock (_threadLock)
                {
                    if(_thread is null)
                    {
                        DefaultEncoding.ForceInvariantCultureAndUTF8Output();
                        _thread = CompileForever(_tcs.Token);
                    }
                }
            }
        }

        private static Thread CompileForever(CancellationToken cancellationToken)
        {
            var thread = new Thread(() =>
            {
                Logger.ZLogTrace("==== HOST Starting compilation thread");
                foreach (var request in _compilationQueue.GetConsumingEnumerable())
                {
                    var cts = CancellationTokenSource.CreateLinkedTokenSource(request.token, cancellationToken);
                    try
                    {
                        Compile(request.request, cts.Token, request.output);
                    }
                    catch (Exception E)
                    {
                        // This should never happen, as Compile already handles exceptions 
                        Logger.LogError(E, "H5 Compiler fail"); 
                    }

                    if(cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
                Logger.ZLogTrace("==== HOST Finished compilation thread");
                _compilationFinished.SetResult(new object());
            });
            
            thread.IsBackground = true;

            thread.Start();
            return thread;
        }

        private static void Compile(CompilationRequest compilationRequest, CancellationToken cancellationToken, TaskCompletionSource<CompilationOutput> output)
        {
            var baseDir = Path.Combine(Path.GetTempPath(), UID128.New().ToString()) + Path.DirectorySeparatorChar; //Need to end in a directory separator
            
            var outDir = Path.Combine(baseDir, "bin") + Path.DirectorySeparatorChar; //Need to end in a directory separator
            var srcDir = Path.Combine(baseDir, "src") + Path.DirectorySeparatorChar; //Need to end in a directory separator
            
            Directory.CreateDirectory(srcDir);
            Directory.CreateDirectory(outDir);

            var compilationOptions = compilationRequest.ToOptions(srcDir, SdkTargetVersion.Latest);

            compilationOptions.ProjectProperties.OutDir = outDir;

            var processor = new TranslatorProcessor(compilationOptions, cancellationToken);
            
            try
            {
                using (new Measure(Logger, "H5 Compilation"))
                {
                    processor.PreProcess(compilationRequest.Settings);
                    processor.Process();
                    processor.PostProcess();
                    var compilationOutput = CompilationOutput.FromOutputLocation(processor.Translator.AssemblyInfo.Output);
                    compilationOutput.Stats = processor.Translator.Stats;
                    output.SetResult(compilationOutput);
                }
            }
            catch (OperationCanceledException)
            {
                //Ignore, probably compilation aborted
                output.TrySetCanceled();
                return;
            }
            catch (EmitterException ex)
            {

                Logger.ZLogTrace("\n{0} ({1},{2},{3},{4}): error H5001: {5}", ex.FileName, ex.StartLine, ex.StartColumn, ex.EndLine, ex.EndColumn, ex.Message);
                
                if (ex.StackTrace is object)
                {
                    Logger.ZLogTrace("\nStack Trace: {0}", ex.StackTrace.ToString());
                }

                output.TrySetException(ex);

                return;
            }
            catch (Exception ex)
            {
                var ee = processor.Translator?.CreateExceptionFromLastNode();

                if (ee != null)
                {
                    output.TrySetException(ee);
                    Logger.ZLogError("\n{0} ({1},{2},{3},{4}): error H5002: {5}", ee.FileName, ee.StartLine, ee.StartColumn, ee.EndLine, ee.EndColumn, ee.Message);
                }
                else
                {
                    output.TrySetException(ex);
                }
                return;
            }
            finally
            {
                processor.Translator.PostBuildStreamCacheCleanup();
                try
                {
                    Directory.Delete(baseDir, true);
                }
                catch(Exception E)
                {
                    Logger.ZLogError(E, "Failed to delete temporary folder");
                }
            }
        }

        public static Task<CompilationOutput> CompileAsync(CompilationRequest request, CancellationToken cancellationToken = default)
        {
            StartCompilationThreadIfNeeded();

            var tcs = new TaskCompletionSource<CompilationOutput>();
            _compilationQueue.Add((request, cancellationToken, tcs));
            return tcs.Task;
        }

    }
}