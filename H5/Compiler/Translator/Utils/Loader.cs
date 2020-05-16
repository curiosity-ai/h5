using System;
using H5.Contract;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace H5.Translator
{
    public class ModuleLoader : IModuleLoader
    {
        public ModuleLoaderType Type
        {
            get; set;
        }

        public string FunctionName
        {
            get; set;
        }

        public bool ManualLoading
        {
            get; set;
        }

        public string ManualLoadingMask
        {
            get; set;
        }

        public bool SkipManualVariables
        {
            get; set;
        }

        public bool IsManual(string name)
        {
            if (ManualLoading)
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(ManualLoadingMask))
            {
                var parts = ManualLoadingMask.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var part in parts)
                {
                    string pattern;
                    bool exclude = part.StartsWith("!");

                    if (part.StartsWith("regex:"))
                    {
                        pattern = part.Substring(6);
                    }
                    else
                    {
                        pattern = "^" + Regex.Escape(part).Replace("\\*", ".*").Replace("\\?", ".") + "$";
                    }

                    if (Regex.IsMatch(name, pattern))
                    {
                        return !exclude;
                    }
                }
            }

            return false;
        }
    }
}