using Tesserae.Components;
using static Tesserae.UI;

namespace Tesserae.Tests.Samples
{
    public static class SamplesHelper
    {
        public static IComponent SampleHeader(string sampleName)
        {
            var text = sampleName.Replace("Sample", "");
            return Stack()
                    .Horizontal()
                    .WidthStretch()
                    .Children(
                        TextBlock(text).XLarge().Bold(),
                        Raw().Grow(1),
                        Button().SetIcon(LineAwesome.Code).SetTitle("View code for this sample").OnClick((_,__) => 
                            Modal(text + " sample code")
                                .LightDismiss()
                                .Width(80.vh())
                                .Content(Stack().Children(TextArea(SamplesSourceCode.GetCodeForSample(sampleName)).Height(80.vh()).Width(80.vw())).Stretch())
                                .ShowCloseButton()
                                .Show()));
        }
        public static IComponent SampleTitle(string text) => TextBlock(text).SemiBold().MediumPlus().PaddingBottom(16.px());
        public static IComponent SampleSubTitle(string text) => TextBlock(text).SemiBold().Medium().PaddingBottom(16.px());
        public static IComponent SampleDo(string text) => Label(Raw(I(_("las la-check", styles: s => s.color = "#107c10"))).PaddingRight(8.px())).SetContent(TextBlock(text)).Inline();
        public static IComponent SampleDont(string text) => Label(Raw(I(_("las la-times", styles: s => s.color = "#e81123"))).PaddingRight(8.px())).SetContent(TextBlock(text)).Inline();
    }
}
