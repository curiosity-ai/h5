using System;

namespace H5.Translator
{
    public class SourceMapEntry
    {
        public int ScriptLine
        {
            get; private set;
        }
        public int ScriptColumn
        {
            get; private set;
        }
        public SourceLocation SourceLocation
        {
            get; private set;
        }

        public SourceMapEntry(SourceLocation sourceLocation, int scriptLine, int scriptColumn)
        {
            SourceLocation = sourceLocation;
            ScriptLine = scriptLine;
            ScriptColumn = scriptColumn;
        }
    }
}
