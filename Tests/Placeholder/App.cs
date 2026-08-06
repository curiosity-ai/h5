using System;
using H5;
using PlaceholderLib;
using static H5.Core.dom;

namespace Placeholder
{
    internal static class App
    {
        private static void Main()
        {
            var greeting = Greeter.Greet("h5");
            var sum = Greeter.Add(40, 2);
            var sumString = sum.ToString();
            var message = greeting + " (" + sumString + ")";

            document.body.innerHTML = "<h1 id=\"hello\">" + message + "</h1>";

            // Stash strings under window for Playwright to read back without
            // dealing with how H5 boxes value types into JS objects.
            window["__h5_test_greeting"] = greeting;
            window["__h5_test_sum"]      = sumString;
            window["__h5_test_message"]  = message;
        }
    }
}
