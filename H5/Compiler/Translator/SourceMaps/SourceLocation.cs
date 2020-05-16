using System;

namespace H5.Translator
{
    public class SourceLocation
    {
        public String SourceUrl
        {
            get; private set;
        }
        public int Line
        {
            get; private set;
        }
        public int Column
        {
            get; private set;
        }
        public String SourceName
        {
            get; private set;
        }

        public SourceLocation(String sourceUrl, String sourceName, int line, int column)
        {
            SourceUrl = sourceUrl;
            SourceName = sourceName;
            Line = line;
            Column = column;
        }
    }
}
