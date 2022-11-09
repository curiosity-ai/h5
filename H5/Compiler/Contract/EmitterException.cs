using ICSharpCode.NRefactory.CSharp;
using System;

namespace H5.Contract
{
    public class EmitterException : Exception, IVisitorException
    {
        public EmitterException(AstNode node)
            : base()
        {
            Node = node;
        }

        public EmitterException(AstNode node, string message)
            : base(message)
        {
            Node = node;
        }

        public EmitterException(AstNode node, string message, Exception innerException)
            : base(message, innerException)
        {
            Node = node;
        }

        public AstNode Node
        {
            get;
            private set;
        }

        public string FileName
        {
            get
            {
                if (Node != null)
                {
                    var parent = Node.GetParent<SyntaxTree>();

                    if (parent != null)
                    {
                        return Node.GetParent<SyntaxTree>().FileName;
                    }


                    if (Node is ICSharpCode.NRefactory.CSharp.SyntaxTree tree)
                    {
                        return tree.FileName;
                    }
                }

                return null;
            }
        }

        public int StartLine
        {
            get
            {
                if (Node != null)
                {
                    return Node.StartLocation.Line;
                }
                return 0;
            }
        }

        public int StartColumn
        {
            get
            {
                if (Node != null)
                {
                    return Node.StartLocation.Column;
                }
                return 0;
            }
        }

        public int EndLine
        {
            get
            {
                if (Node != null)
                {
                    return Node.EndLocation.Line;
                }
                return 0;
            }
        }

        public int EndColumn
        {
            get
            {
                if (Node != null)
                {
                    return Node.EndLocation.Column;
                }
                return 0;
            }
        }
    }
}