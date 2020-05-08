using Tesserae.Components;
using static H5.dom;
using static Tesserae.Tests.Samples.SamplesHelper;
using static Tesserae.UI;

namespace Tesserae.Tests.Samples
{
    public class LayerSample : IComponent
    {
        private readonly IComponent _content;
        public LayerSample()
        {
            var layer = Layer();
            var layerHost = LayerHost();
            _content = SectionStack()
                        .Title(SampleHeader(nameof(LayerSample)))
                        .Section(Stack().Children(
                            SampleTitle("Overview"),
                            TextBlock("A Layer is a technical component that does not have specific Design guidance."),
                            TextBlock("Layers are used to render content outside of a DOM tree, at the end of the document. This allows content to escape traditional boundaries caused by \"overflow: hidden\" css rules and keeps it on the top without using z-index rules. This is useful for example in ContextualMenu and Tooltip scenarios, where the content should always overlay everything else.")))
                        .Section(Stack().Children(
                            SampleTitle("Usage"),
                            TextBlock("Basic layered content").Medium(),
                            layer.Content(Stack().Horizontal().Children(TextBlock("This is example layer content."),Toggle(), Toggle(), Toggle())),
                                Toggle("Toggle Component Layer").OnChange((s, e) => layer.IsVisible = s.IsChecked),
                                TextBlock("Using LayerHost to control projection").Medium(),
                                Toggle("Show on Host").OnChange((s, e) => layer.Host = s.IsChecked ? layerHost : null),
                                layerHost));
        }

        public HTMLElement Render() => _content.Render();
    }
}