using H5.Contract;
using H5.Contract.Constants;
using Microsoft.Ajax.Utilities;
using Microsoft.Extensions.Logging;
using Mono.Cecil;
using Mosaik.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace H5.Translator
{
    public static class HtmlTokens
    {
        public const string META   = "{-!-meta-!-}";
        public const string TITLE  = "{-!-title-!-}";
        public const string CSS    = "{-!-css-!-}";
        public const string SCRIPT = "{-!-script-!-}";
        public const string HEAD   = "{-!-head-!-}";
        public const string BODY   = "{-!-body-!-}";
        
        //The template must match the tokens above!

        public const string TEMPLATE =
@"<!doctype html>
<html lang=en>
<head>
    <meta charset=""utf-8"" />
    {-!-meta-!-}
    <title>{-!-title-!-}</title>
    {-!-css-!-}
    {-!-script-!-}
    {-!-head-!-}
</head>
<body>
{-!-body-!-}
</body>
</html>";
    }

    internal class HtmlGenerator
    {
        private static ILogger Logger = ApplicationLogging.CreateLogger<HtmlGenerator>();

        private readonly IAssemblyInfo _assemblyConfig;

        private readonly TranslatorOutput _translatorOutputs;

        private readonly string _title;

        private readonly string _buildConfig;

        public HtmlGenerator(IAssemblyInfo config, TranslatorOutput outputs, string title, string buildConfig)
        {
            _assemblyConfig = config;
            _translatorOutputs = outputs;
            _title = title;
            _buildConfig = buildConfig;
        }

        public void GenerateHtml(string outputPath)
        {
            Logger.LogTrace("GenerateHtml...");
            
            Logger.LogTrace("Applying default html template");

            var htmlTemplate = HtmlTokens.TEMPLATE;

            var indexCss    = htmlTemplate.IndexOf(HtmlTokens.CSS, StringComparison.InvariantCultureIgnoreCase);
            var indexScript = htmlTemplate.IndexOf(HtmlTokens.SCRIPT, StringComparison.InvariantCultureIgnoreCase);

            var cssLinkTemplate = "<link rel=\"stylesheet\" href=\"{0}\">";
            var scriptTemplate = "<script src=\"{0}\" defer></script>";

            var indentCss = GetIndent(htmlTemplate, indexCss);
            var indentScript = GetIndent(htmlTemplate, indexScript);

            var cssBuffer = new StringBuilder();
            var jsBuffer = new StringBuilder();
            var jsMinBuffer = new StringBuilder();

            var outputForHtml = _translatorOutputs.GetOutputs();

            if (_translatorOutputs.ResourcesForHtml.Count > 0)
            {
                outputForHtml = outputForHtml.Concat(_translatorOutputs.ResourcesForHtml);
            }

            var firstJs = true;
            var firstMinJs = true;
            var firstCss = true;

            foreach (var output in outputForHtml.Where(o => o.LoadInHtml))
            {
                if (output.OutputType == TranslatorOutputType.JavaScript && indexScript >= 0)
                {
                    if (output.IsMinified)
                    {
                        if (!firstMinJs)
                        {
                            jsMinBuffer.Append(Emitter.NEW_LINE);
                            jsMinBuffer.Append(indentScript);
                        }

                        firstMinJs = false;

                        jsMinBuffer.Append(string.Format(scriptTemplate, output.GetOutputPath(outputPath, true)));
                    }
                    else
                    {
                        if (!firstJs)
                        {
                            jsBuffer.Append(Emitter.NEW_LINE);
                            jsBuffer.Append(indentScript);
                        }

                        firstJs = false;

                        jsBuffer.Append(string.Format(scriptTemplate, output.GetOutputPath(outputPath, true)));
                    }
                } else if (output.OutputType == TranslatorOutputType.StyleSheets && indexCss >= 0)
                {
                    if (!firstCss)
                    {
                        cssBuffer.Append(Emitter.NEW_LINE);
                        cssBuffer.Append(indentCss);
                    }

                    firstCss = false;

                    cssBuffer.Append(string.Format(cssLinkTemplate, output.GetOutputPath(outputPath, true)));
                }
            }

            var tokens = new Dictionary<string, string>()
            {
                [HtmlTokens.TITLE]  = _title,
                [HtmlTokens.CSS]    = cssBuffer.ToString(),
                [HtmlTokens.SCRIPT] = jsBuffer.ToString(),
                [HtmlTokens.BODY] = _assemblyConfig.Html.Body ?? "",
                [HtmlTokens.HEAD] = _assemblyConfig.Html.Head ?? "",
                [HtmlTokens.META] = _assemblyConfig.Html.Meta ?? "",
            };

            string htmlName = null;
            string htmlMinName = null;

            if (jsBuffer.Length > 0 || cssBuffer.Length > 0)
            {
                htmlName = "index.html";
            }

            if (jsMinBuffer.Length > 0)
            {
                htmlMinName = htmlName is null ? "index.html" : "index.min.html";
            }

            //Adds an extra logic here to only keep "one" index.html in the final output folder, depending on the type of build requested
            //This is useful when we want to generate both normal and minified formatings, but want to "consume" only one of them as the final output
            
            if (!string.IsNullOrEmpty(_buildConfig))
            {
                if(string.Equals(_buildConfig, "Release", StringComparison.InvariantCultureIgnoreCase))
                {
                    if(htmlMinName is object)
                    {
                        htmlName = null;
                        htmlMinName = "index.html";
                    }
                }
                else if (string.Equals(_buildConfig, "Debug", StringComparison.InvariantCultureIgnoreCase))
                {
                    if(htmlMinName is object && htmlName is object)
                    {
                        htmlMinName = null;
                    }
                }
            }

            var configHelper = new ConfigHelper();

            if (htmlName != null)
            {
                var html = configHelper.ApplyTokens(tokens, htmlTemplate);

                htmlName = Path.Combine(outputPath, htmlName);
                File.WriteAllText(htmlName, html, Translator.OutputEncoding);
            }

            if (htmlMinName != null)
            {
                tokens[HtmlTokens.SCRIPT] = jsMinBuffer.ToString();
                var html = configHelper.ApplyTokens(tokens, htmlTemplate);

                htmlMinName = Path.Combine(outputPath, htmlMinName);
                File.WriteAllText(htmlMinName, html, Translator.OutputEncoding);
            }

            Logger.LogTrace("GenerateHtml done");
        }

        private string GetIndent(string input, int index)
        {
            if (index <= 0 || input == null || index >= input.Length)
            {
                return "";
            }

            var indent = 0;

            while (index-- > 0)
            {
                if (input[index] != ' ')
                {
                    break;
                }

                indent++;
            }

            return new string(' ', indent);
        }

        private string ReadEmbeddedResource(string name)
        {
            var assembly = System.Reflection. Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(name))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}