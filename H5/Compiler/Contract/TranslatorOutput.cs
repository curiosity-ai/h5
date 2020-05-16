using System;
using System.Collections.Generic;
using System.Linq;

namespace H5.Contract
{
    public class TranslatorOutput
    {
        public List<TranslatorOutputItem> Main { get; private set; }

        public List<TranslatorOutputItem> References { get; private set; }

        public List<TranslatorOutputItem> Locales { get; private set; }

        public TranslatorOutputItem Combined { get; set; }

        public Dictionary<H5ResourceInfoPart, string> CombinedResourcePartsNonMinified { get; set; }

        public Dictionary<H5ResourceInfoPart, string> CombinedResourcePartsMinified { get; set; }

        public TranslatorOutputItem CombinedLocales { get; set; }

        public List<TranslatorOutputItem> ResourcesForHtml { get; private set; }

        public IEnumerable<TranslatorOutputItem> GetOutputs(bool projectOutputOnly = false)
        {
            if (Combined != null)
            {
                if (!Combined.IsEmpty)
                {
                    yield return Combined;
                }

                if (Combined.MinifiedVersion != null && !Combined.MinifiedVersion.IsEmpty)
                {
                    yield return Combined.MinifiedVersion;
                }
            }

            if (!projectOutputOnly)
            {
                foreach (var o in References)
                {
                    if (!o.IsEmpty)
                    {
                        yield return o;
                    }

                    if (o.MinifiedVersion != null && !o.MinifiedVersion.IsEmpty)
                    {
                        yield return o.MinifiedVersion;
                    }
                }
            }

            if (CombinedLocales != null)
            {
                if (!CombinedLocales.IsEmpty)
                {
                    yield return CombinedLocales;
                }

                if (CombinedLocales.MinifiedVersion != null && !CombinedLocales.MinifiedVersion.IsEmpty)
                {
                    yield return CombinedLocales.MinifiedVersion;
                }
            }

            foreach (var o in Locales)
            {
                if (!o.IsEmpty)
                {
                    yield return o;
                }

                if (o.MinifiedVersion != null && !o.MinifiedVersion.IsEmpty)
                {
                    yield return o.MinifiedVersion;
                }
            }

            foreach (var o in Main)
            {
                if (!o.IsEmpty)
                {
                    yield return o;
                }

                if (o.MinifiedVersion != null && !o.MinifiedVersion.IsEmpty)
                {
                    yield return o.MinifiedVersion;
                }
            }
        }

        public TranslatorOutput()
        {
            Main = new List<TranslatorOutputItem>();
            References = new List<TranslatorOutputItem>();
            Locales = new List<TranslatorOutputItem>();
            ResourcesForHtml = new List<TranslatorOutputItem>();
        }
    }

    public enum TranslatorOutputType
    {
        None,
        JavaScript,
        TypeScript,
        StyleSheets
    }

    [Flags]
    public enum TranslatorOutputKind
    {
        None = 0,
        Reference = 1,
        Resource = 2,
        Locale = 4,
        ProjectOutput = 8,
        PluginOutput = 16,
        Minified = 32,
        Combined = 64,
        Metadata = 128,
        Report = 256
    }
}