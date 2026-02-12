using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    public static class PlaywrightRunner
    {
        public static async Task<string> RunJs(string jsCode)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            var page = await browser.NewPageAsync();

            var consoleOutput = new StringBuilder();
            page.Console += (_, msg) =>
            {
                if (msg.Type == "log")
                    consoleOutput.AppendLine(msg.Text);
            };

            // Wrap in try-catch to report script errors
            // We assume jsCode contains all necessary definitions and execution logic.
            // H5 usually generates an IIFE or similar, but we might need to trigger the Main method if it's not auto-executed.

            // H5 generated code usually looks like:
            // H5.assembly("App", function ($asm, globals) { ... });

            // To run the main method, we typically rely on H5's bootstrapper or we manually call it.
            // If the compilation included a Main method, H5 might not auto-run it unless configured.

            // For now, let's just evaluate the code. If H5 is set up to run Main, it should run.
            // If not, we might need to append a call to the entry point.

            try
            {
                await page.EvaluateAsync(jsCode);
            }
            catch (Exception e)
            {
                consoleOutput.AppendLine($"PLAYWRIGHT_ERROR: {e.Message}");
            }

            return consoleOutput.ToString();
        }
    }
}
