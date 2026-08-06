using System;

namespace PlaceholderLib
{
    /// <summary>
    /// Trivial library type used by Tests/Placeholder to validate that ESM output
    /// produced by the h5 compiler links across multiple assemblies / generated files.
    /// </summary>
    public static class Greeter
    {
        public static string Greet(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "Hello, world!";
            }

            return "Hello, " + name + "!";
        }

        public static int Add(int a, int b)
        {
            return a + b;
        }
    }
}
