using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using H5.Contract;
using Microsoft.Extensions.Logging;
using Mosaik.Core;
using ZLogger;

namespace H5.Translator
{
    public class SourceMapGenerator : ISourceMapRecorder
    {
        private static ILogger Logger = ApplicationLogging.CreateLogger<SourceMapGenerator>();

        public UnicodeNewline? ForceEols { get; set; }

        public SourceMapBuilder SourceMapBuilder { get; }

        public SourceMapGenerator(string scriptPath, string sourceRoot, string basePath, UnicodeNewline? forceEols = null)
        {
            string scriptFileName = Path.GetFileName(scriptPath);
            SourceMapBuilder = new SourceMapBuilder(scriptFileName, sourceRoot, basePath);
            ForceEols = forceEols;
        }

        public void RecordLocation(int scriptLine, int scriptCol, string sourcePath, int sourceLine, int sourceCol)
        {
            SourceLocation sourceLocation;
            if (sourcePath == null)
            {
                sourceLocation = new SourceLocation("no-source-location", "", 0, 0);
            }
            else
            {
                sourceLocation = new SourceLocation(sourcePath, "", sourceLine - 1, sourceCol - 1);    // convert line and column to 0-based
            }

            SourceMapBuilder.AddMapping(scriptLine - 1, scriptCol - 1, sourceLocation);
        }

        public string GetSourceMap(string[] sourcesContent)
        {
            return SourceMapBuilder.Build(sourcesContent, ForceEols);
        }

        internal static Regex tokenRegex = new Regex(@"/\*##\|(.+?),(\d+?),(\d+?)\|##\*/", RegexOptions.Compiled);

        public static void Generate(string scriptFileName, string sourceRoot, string basePath, ref string content, Action<SourceMapBuilder> beforeGenerate, Func<string, string> sourceContent, string[] names, IList<string> sourceFiles, UnicodeNewline? forceEols)
        {
            var fileName = Path.GetFileName(scriptFileName);
            var generator = new SourceMapGenerator(fileName, sourceRoot, basePath, forceEols);
            StringLocation location = null;
            string script = content;
            int offset = 0;

            content = tokenRegex.Replace(content, match =>
            {
                location = LocationFromPos(script, match.Index, location, ref offset);
                offset += match.Length;
                var sourceLine = int.Parse(match.Groups[2].Value);
                var sourceCol = int.Parse(match.Groups[3].Value);
                var sourcePath = sourceFiles[int.Parse(match.Groups[1].Value)];
                generator.RecordLocation(location.Line, location.Column, sourcePath, sourceLine, sourceCol);
                return "";
            });

            var sources = generator.SourceMapBuilder.SourceUrlList;
            List<string> contents = new List<string>();

            foreach (var source in sources)
            {
                contents.Add(sourceContent(source));
            }

            // Chrome handles it very strange, need more investigate
            //generator._sourceMapBuilder.SourceNameList.AddRange(names);

            beforeGenerate?.Invoke(generator.SourceMapBuilder);

            var map = generator.GetSourceMap(contents.ToArray());

            Logger.ZLogTrace("Generated source map for {0}", scriptFileName);

            var encoded = "//# sourceMappingURL=data:application/json;base64," + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(map));

            content = content + Emitter.NEW_LINE + encoded + Emitter.NEW_LINE;
        }

        private static StringLocation LocationFromPos(string s, int pos, StringLocation lastLocation, ref int offset)
        {
            int res = lastLocation?.Line ?? 1;
            int startLinePosition = lastLocation?.StartLinePosition ?? 0;
            int i;

            for (i = lastLocation?.Position ?? 0; i <= pos - 1; i++)
            {
                if (s[i] == Emitter.NEW_LINE_CHAR)
                {
                    startLinePosition = i;
                    offset = 0;
                    res++;
                }
            }

            return new StringLocation(res, pos - startLinePosition - offset, i, startLinePosition);
        }

        private class StringLocation
        {
            public StringLocation(int line, int column, int position, int startLinePosition)
            {
                Line = line;
                Column = column;
                StartLinePosition = startLinePosition;
                Position = position;
            }

            public int Line { get; set; }
            public int Column { get; set; }
            public int StartLinePosition { get; set; }
            public int Position { get; set; }
        }
    }
}
