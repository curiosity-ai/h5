using ICSharpCode.NRefactory.CSharp;
using System.Text;

namespace H5.Contract
{
    public interface IAsyncJumpLabel
    {
        StringBuilder Output { get; set; }

        AstNode Node { get; set; }
    }
}