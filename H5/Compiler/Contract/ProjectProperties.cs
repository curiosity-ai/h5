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

        public override string ToString()
        {
            return string.Join(", ", GetValues().Select(x => x.Key + ":" + x.Value));
        }

        public List<string> BuildProjects{ get; set; }

        public Dictionary<string, string> GetValues()
        {
            var r = new Dictionary<string, string>()
            {
               { WrapProperty("AssemblyName"), GetString(this.AssemblyName) },
               { WrapProperty("AssemblyVersion"), GetString(this.AssemblyVersion) },
               { WrapProperty("CheckForOverflowUnderflow"), GetString(this.CheckForOverflowUnderflow) },
               { WrapProperty("Configuration"), GetString(this.Configuration) },
               { WrapProperty("DefineConstants"), GetString(this.DefineConstants) },
               { WrapProperty("OutDir"), GetString(this.OutDir) },
               { WrapProperty("OutputPath"), GetString(this.OutputPath) },
               { WrapProperty("OutputType"), GetString(this.OutputType) },
               { WrapProperty("RootNamespace"), GetString(this.RootNamespace) },
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
                    this.AssemblyName = value;
                    break;
                case "assemblyversion":
                    this.AssemblyVersion = value;
                    break;
                case "checkforoverflowunderflow":
                    this.CheckForOverflowUnderflow = GetNullableBool(value);
                    break;
                case "configuration":
                    this.Configuration = value;
                    break;
                case "defineconstants":
                    this.DefineConstants = value;
                    break;
                case "outdir":
                    this.OutDir = value;
                    break;
                case "outputpath":
                    this.OutputPath = value;
                    break;
                case "outputtype":
                    this.OutputType = value;
                    break;
                case "rootnamespace":
                    this.RootNamespace = value;
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