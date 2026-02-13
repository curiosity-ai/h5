namespace H5.Contract
{
    using MessagePack;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [MessagePackObject(keyAsPropertyName:true)]
    public class ProjectProperties
    {
        public string AssemblyName{ get; set; }

        public string AssemblyVersion { get; set; }

        public bool? CheckForOverflowUnderflow{ get; set; }

        public string Configuration{ get; set; }

        public string DefineConstants{ get; set; }

        public string OutputPath{ get; set; }

        public string OutDir{ get; set; }

        public string OutputType{ get; set; }

        public string RootNamespace{ get; set; }

        public string LanguageVersion { get; set; }

        public override string ToString()
        {
            return string.Join(", ", GetValues().Select(x => x.Key + ":" + x.Value));
        }

        public List<string> BuildProjects{ get; set; }

        public Dictionary<string, string> GetValues()
        {
            var r = new Dictionary<string, string>()
            {
               { WrapProperty("AssemblyName"), GetString(AssemblyName) },
               { WrapProperty("AssemblyVersion"), GetString(AssemblyVersion) },
               { WrapProperty("CheckForOverflowUnderflow"), GetString(CheckForOverflowUnderflow) },
               { WrapProperty("Configuration"), GetString(Configuration) },
               { WrapProperty("DefineConstants"), GetString(DefineConstants) },
               { WrapProperty("OutDir"), GetString(OutDir) },
               { WrapProperty("OutputPath"), GetString(OutputPath) },
               { WrapProperty("OutputType"), GetString(OutputType) },
               { WrapProperty("RootNamespace"), GetString(RootNamespace) },
               { WrapProperty("LangVersion"), GetString(LanguageVersion) },
            };

            return r;
        }

        public void SetValues(Dictionary<string, string> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            foreach (var kv in values)
            {
                SetValue(kv.Key, kv.Value);
            }
        }

        public void SetValue(string key, string value)
        {
            switch (key.ToLowerInvariant())
            {
                case "assemblyname":
                    AssemblyName = value;
                    break;
                case "assemblyversion":
                    AssemblyVersion = value;
                    break;
                case "checkforoverflowunderflow":
                    CheckForOverflowUnderflow = GetNullableBool(value);
                    break;
                case "configuration":
                    Configuration = value;
                    break;
                case "defineconstants":
                    DefineConstants = value;
                    break;
                case "outdir":
                    OutDir = value;
                    break;
                case "outputpath":
                    OutputPath = value;
                    break;
                case "outputtype":
                    OutputType = value;
                    break;
                case "rootnamespace":
                    RootNamespace = value;
                    break;
                case "langversion":
                    LanguageVersion = value;
                    break;
                default:
                    break;
            }
        }

        protected string WrapProperty(string name)
        {
            return "$(" + name + ")";
        }

        protected string GetString(string s)
        {
            return s ?? "";
        }

        protected string GetString(bool? b)
        {
            return b.HasValue ? GetString(b.Value) : GetString((string)null);
        }

        protected string GetString(bool b)
        {
            return b.ToString().ToLowerInvariant();
        }

        protected bool? GetNullableBool(string s, bool safe = false)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            if ((new string[] { "true", "1" }).Contains(s.ToLowerInvariant()))
            {
                return true;
            }

            if ((new string[] { "false", "0" }).Contains(s.ToLowerInvariant()))
            {
                return false;
            }

            if (!safe)
            {
                throw new ArgumentException("Could not parse value {0} into bool?", s);
            }

            return null;
        }
    }
}