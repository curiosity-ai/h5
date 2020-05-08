using System;
using System.Threading.Tasks;
using H5;
using static H5.Core.dom;

namespace Tesserae
{
    public static class Require
    {
        private static SingleSemaphoreSlim singleCall = new SingleSemaphoreSlim();
        public static void LoadStyleAsync(params string[] styles)
        {
            for (int i = 0; i < styles.Length; i++)
            {
                var url = styles[i];
                HTMLElement existingStyle = (HTMLElement)document.querySelector($"link[href^='{url}']");
                if (existingStyle == null)
                {
                    var l = document.createElement("link") as HTMLLinkElement;
                    l.rel = "stylesheet";
                    l.href = url;
                    document.head.appendChild(l);
                }
            }
        }

        public static async Task LoadScriptAsync(params string[] libraries)
        {
            await singleCall.WaitAsync();

            var tcs = new TaskCompletionSource<bool>();
            LoadScriptAsync(() => tcs.SetResult(true), (err) => tcs.SetException(new Exception(err)), libraries);
            await tcs.Task;

            singleCall.Release();
        }

        private static void LoadScriptAsync(Action onComplete, Action<string> onFail, params string[] libraries)
        {
            var loadedCount = 0;
            dom.HTMLElement.onactivateFn onScriptLoaded = e =>
            {
                loadedCount++;
                if (loadedCount == libraries.Length) onComplete?.Invoke();
            };
            for (int i = 0; i < libraries.Length; i++)
            {
                var url = libraries[i];
                HTMLElement existingLib = (HTMLElement)document.querySelector($"script[src^='{url}']");
                if (existingLib != null)
                {
                    // Is already loaded?
                    loadedCount++;
                }
                else
                {
                    var script = new HTMLScriptElement();
                    script.type = "text/javascript";
                    script.src = url;
                    script.async = true;
                    script.onerror = e => { onFail?.Invoke(url); loadedCount++; if (loadedCount == libraries.Length) onComplete?.Invoke(); };
                    script.onload = onScriptLoaded;
                    try
                    {
                        document.head.appendChild(script);
                    }
                    catch
                    {
                        onFail?.Invoke(url); loadedCount++;
                    }
                }
            }
            if (loadedCount == libraries.Length) onComplete?.Invoke();
        }
    }
}
