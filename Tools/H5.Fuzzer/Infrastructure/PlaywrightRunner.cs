using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace H5.Fuzzer.Infrastructure
{
    public class PlaywrightRunner : IAsyncDisposable
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;

        public async Task StartAsync()
        {
            if (_browser != null) return;
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        }

        public async Task<string> RunJsAsync(string jsCode, string? waitForOutput = null)
        {
            if (_browser == null) await StartAsync();

            var page = await _browser!.NewPageAsync();
            var consoleOutput = new StringBuilder();
            var outputComplete = new TaskCompletionSource<bool>();

            page.Console += (_, msg) =>
            {
                if (msg.Type == "log")
                {
                    var text = msg.Text;
                    consoleOutput.AppendLine(text);

                    if (waitForOutput != null && text.Contains(waitForOutput))
                    {
                        outputComplete.TrySetResult(true);
                    }
                }
            };

            try
            {
                await page.EvaluateAsync(jsCode);

                if (waitForOutput != null)
                {
                    var completedTask = await Task.WhenAny(outputComplete.Task, Task.Delay(30000));
                    if (completedTask != outputComplete.Task)
                    {
                        consoleOutput.AppendLine("PLAYWRIGHT_TIMEOUT: Did not receive expected output.");
                    }
                }
            }
            catch (Exception e)
            {
                consoleOutput.AppendLine($"PLAYWRIGHT_ERROR: {e.Message}");
            }
            finally
            {
                await page.CloseAsync();
            }

            return consoleOutput.ToString();
        }

        public async ValueTask DisposeAsync()
        {
            if (_browser != null)
            {
                await _browser.DisposeAsync();
                _browser = null;
            }
            if (_playwright != null)
            {
                _playwright.Dispose();
                _playwright = null;
            }
        }

        // Static helper for compatibility/simple usage if needed, but not efficient
        public static async Task<string> RunJs(string jsCode, string? waitForOutput = null)
        {
            await using var runner = new PlaywrightRunner();
            await runner.StartAsync();
            return await runner.RunJsAsync(jsCode, waitForOutput);
        }
    }
}
