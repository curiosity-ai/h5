using System;
using H5;
using static H5.Core.dom;

namespace Placeholder
{
    internal static class App
    {
        private static void Main()
        {
            var hello = "Hello";
            var world = World();
            document.body.innerHTML = $"{hello} {world}!";
            string World() => "World";
        }
    }
}
