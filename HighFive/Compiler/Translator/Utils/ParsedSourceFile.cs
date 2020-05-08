using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.TypeSystem;

namespace HighFive.Translator
{
    public class ParsedSourceFile
    {
        public ParsedSourceFile(SyntaxTree syntaxTree, CSharpUnresolvedFile parsedFile)
        {
            SyntaxTree = syntaxTree;
            ParsedFile = parsedFile;
        }

        public SyntaxTree SyntaxTree
        {
            get;
            set;
        }

        public CSharpUnresolvedFile ParsedFile
        {
            get;
            private set;
        }
    }
}