using ICSharpCode.NRefactory.CSharp;
using System.Linq;

namespace Bridge.Translator
{
    public class ArrayInitializerVisitor : Visitor
    {
        public override void VisitArrayCreateExpression(ArrayCreateExpression node)
        {
            node.Initializer.Elements.ToList().ForEach
            (
                item => item.AcceptVisitor(this)
            );
        }

        public override void VisitPrimitiveExpression(PrimitiveExpression node)
        {
        }
    }
}