using ICSharpCode.NRefactory.CSharp;
using System.Text;

namespace HighFive.Contract
{
    public interface IAsyncJumpLabel
    {
        StringBuilder Output
        {
            get;
            set;
        }

        AstNode Node
        {
            get;
            set;
        }
    }
}