using System;

namespace HighFive
{
    /// <summary>
    /// The output folder path for generated JavaScript. A non-absolute path is concatenated with a project's root.
    /// Examples: "HighFive/output/", "../HighFive/output/", "c:\\output\\"
    /// </summary>
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class OutputAttribute : Attribute
    {
        public OutputAttribute(string path)
        {
        }
    }
}