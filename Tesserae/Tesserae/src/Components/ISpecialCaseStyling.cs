using static H5.Core.dom;

namespace Tesserae.Components
{
    public interface ISpecialCaseStyling
    {
        HTMLElement StylingContainer { get; }
        bool PropagateToStackItemParent { get; }
    }
}
