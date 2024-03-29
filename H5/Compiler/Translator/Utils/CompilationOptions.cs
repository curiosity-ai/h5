using System;
using System.Collections.Generic;
using System.Linq;
using H5.Contract;

namespace H5.Translator
{
    public class CompilationOptions
    {
        public ProjectProperties ProjectProperties { get; set; }

        public string ProjectLocation { get; set; }
        public string DefaultFileName { get; set; }
        public string H5Location { get; set; }
        public bool Rebuild { get; set; }

        public string ReferencesPath { get; set; }

        public CompilationOptions()
        {
        }

        public override string ToString()
        {
            return string.Join(", ", GetValues().Select(x => x.Key + ":" + x.Value));
        }

        protected Dictionary<string, string> GetValues()
        {
            var r = new Dictionary<string, string>()
            {
                { WrapProperty("ProjectProperties"), GetString(ProjectProperties) },
                { WrapProperty("ProjectLocation"), GetString(ProjectLocation) },
                { WrapProperty("DefaultFileName"), GetString(DefaultFileName) },
                { WrapProperty("H5Location"), GetString(H5Location) },
                { WrapProperty("Rebuild"), GetString(Rebuild) },
                { WrapProperty("ReferencesPath"), GetString(ReferencesPath) }
            };

            return r;
        }

        protected string WrapProperty(string name)
        {
            return name;
        }

        protected string GetString(string s)
        {
            return s ?? "";
        }

        protected string GetString(ProjectProperties p)
        {
            return p != null ? p.ToString() : "";
        }

        protected string GetString(bool? b)
        {
            return b.HasValue ? GetString(b.Value) : GetString((string)null);
        }

        protected string GetString(bool b)
        {
            return b.ToString().ToLowerInvariant();
        }
    }
}