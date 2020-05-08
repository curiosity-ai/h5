using static H5.dom;

namespace Tesserae.Components
{
    public interface ISpecialCaseStyling
    {
        HTMLElement StylingContainer { get; }
        bool PropagateToStackItemParent { get; }
    }
}
