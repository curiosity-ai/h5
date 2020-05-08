namespace Tesserae.Components
{
    /// <summary>
    /// A Layer is a technical component that does not have specific Design guidance.
    /// 
    /// Layers are used to render content outside of a DOM tree, at the end of the document.This allows content to escape traditional boundaries caused by "overflow: hidden" css rules and keeps it on the top without using z-index rules.This is useful for example in
    /// ContextualMenu and Tooltip scenarios, where the content should always overlay everything else.
    /// </summary>
    public sealed class Layer : Layer<Layer> { } // TODO: Explain must be sealed vs Layer<T>
}