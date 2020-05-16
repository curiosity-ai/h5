using Microsoft.Extensions.Logging;
using Mosaik.Core;
using System;
using System.IO;

namespace H5.Contract
{
    public class TranslatorOutputItem
    {
        private static ILogger Logger = ApplicationLogging.CreateLogger<TranslatorOutputItem>();

        public string Assembly { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public bool HasGeneratedSourceMap { get; set; }

        private Uri fullPath;
        public Uri FullPath
        {
            get
            {
                if (fullPath == null)
                {
                    throw new InvalidOperationException(
                        "Cannot get FullPath of output item as it has not been set ("
                        + string.Format("[{0}, {1}, {2}]", this.Location, this.Name, this.OutputType)
                        + ")");
                }

                return fullPath;
            }
            set
            {
                fullPath = value;
            }
        }

        public TranslatorOutputType OutputType { get; set; }

        public TranslatorOutputKind OutputKind { get; set; }

        public TranslatorOutputItemContent Content { get; set; }

        public bool IsMinified { get; set; }

        public bool LoadInHtml { get; set; } = true;

        private bool isEmpty;
        public bool IsEmpty
        {
            get
            {
                if (Content == null ||
                    ((Content.Buffer == null || Content.Buffer.Length == 0)
                    && (Content.String == null || Content.String.Length == 0)
                    && (Content.Builder == null || Content.Builder.Length == 0)))
                {
                    isEmpty = true;
                }

                return isEmpty;
            }
            set
            {
                isEmpty = value;

                if (value == true && Content != null)
                {
                    Content.SetContent(null);
                }
            }
        }

        public TranslatorOutputItem MinifiedVersion { get; set; }

        public string GetOutputPath(string basePath = null, bool htmlAdjusted = false)
        {
            var item = this;

            if (item.IsEmpty)
            {
                item = item.MinifiedVersion;

                if (item.IsEmpty)
                {
                    return null;
                }
            }

            string path;
            if (item.Location != null)
            {
                path = Path.Combine(item.Location, item.Name);
            }
            else
            {
                path = item.Name;
            }

            if (basePath != null)
            {
                if (!string.IsNullOrEmpty(basePath) && basePath[basePath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    basePath = basePath + Path.DirectorySeparatorChar;
                }

                basePath = Path.GetFullPath(basePath);

                Logger.LogTrace("\tbase: " + basePath);

                path = MakeRelative(path, basePath);

                Logger.LogTrace("\tpath: " + path);
            }

            if (htmlAdjusted)
            {
                path = new ConfigHelper().ConvertPath(path, '/');
            }

            return path;
        }

        private string MakeRelative(string filePath, string referencePath)
        {
            //filePath = Path.GetFullPath((new Uri(filePath)).LocalPath);
            if (!Path.IsPathRooted(filePath))
            {
                filePath = Path.Combine(referencePath, filePath);
            }

            filePath = Path.GetFullPath(filePath);

            var referenceUri = new Uri(referencePath);
            Uri result;
            if (Uri.TryCreate(referenceUri, filePath, out result))
            {
                return referenceUri.MakeRelativeUri(result).ToString();
                //return result.MakeRelativeUri(new Uri(referencePath)).ToString();
            }

            return filePath;
        }
    }
}