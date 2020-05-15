using System;
using System.Collections.Generic;
using System.Linq;
using H5.Contract;

namespace H5.Translator
{
    public class H5Options
    {
        public string Name { get; set; }

        public ProjectProperties ProjectProperties { get; set; }

        public string ProjectLocation { get; set; }

        internal bool SkipEmbeddingResources => ProjectLocation.Contains("H5.Core.csproj");

        public string OutputLocation { get; set; }
        public string DefaultFileName { get; set; }
        public string H5Location { get; set; }
        public bool Rebuild { get; set; }
        public bool ExtractCore { get; set; }
        public bool NoCompilation { get; set; }
        public bool Run { get; set; }
        public bool FromTask { get; set; }
        public bool NoLoggerSetUp { get; set; }
        public string Sources { get; set; }
        public string ReferencesPath { get; set; }

        public H5Options()
        {
            ExtractCore = true;
        }

        public override string ToString()
        {
            return string.Join(", ", GetValues().Select(x => x.Key + ":" + x.Value));
        }

        protected Dictionary<string, string> GetValues()
        {
            var r = new Dictionary<string, string>()
            {
                { WrapProperty("Name"), GetString(this.Name) },
                { WrapProperty("ProjectProperties"), GetString(this.ProjectProperties) },
                { WrapProperty("ProjectLocation"), GetString(this.ProjectLocation) },
                { WrapProperty("OutputLocation"), GetString(this.OutputLocation) },
                { WrapProperty("DefaultFileName"), GetString(this.DefaultFileName) },
                { WrapProperty("H5Location"), GetString(this.H5Location) },
                { WrapProperty("Rebuild"), GetString(this.Rebuild) },
                { WrapProperty("ExtractCore"), GetString(this.ExtractCore) },
                { WrapProperty("Help"), GetString(this.NoCompilation) },
                { WrapProperty("Run"), GetString(this.Run) },
                { WrapProperty("FromTask"), GetString(this.FromTask) },
                { WrapProperty("NoLoggerSetUp"), GetString(this.NoLoggerSetUp) },
                { WrapProperty("Sources"), GetString(this.Sources) },
                { WrapProperty("ReferencesPath"), GetString(this.ReferencesPath) }
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