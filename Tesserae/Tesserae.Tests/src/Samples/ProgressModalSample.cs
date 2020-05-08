using System;
using System.Threading.Tasks;
using Tesserae.Components;
using static H5.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;
using System.Threading;

namespace Tesserae.Tests.Samples
{
    public class ProgressModalSample : IComponent
    {
        private IComponent _content;

        public ProgressModalSample()
        {
            ProgressModal modal;

            CancellationTokenSource cts;

            float progress = 0;

            void ProgressFrame(object a)
            {
                if (cts.IsCancellationRequested)
                {
                    modal.ProgressSpin().Message("Cancelling...");
                    Task.Delay(2000).ContinueWith(_ => modal.Hide()).FireAndForget();
                    return;
                }
                progress++;

                if (progress < 100)
                {
                    modal.Message($"Processing {progress}%").Progress(progress);
                    window.setTimeout(ProgressFrame, 16);
                }
                else
                {
                    modal.Message("Finishing...").ProgressIndeterminated();
                    Task.Delay(5000).ContinueWith(_ => modal.Hide()).FireAndForget();
                }
            }

            async Task PlayModal()
            {
                modal = ProgressModal().Title("Lorem Ipsum");
                cts = new CancellationTokenSource();
                modal.WithCancel((b) => { b.Disabled(); cts.Cancel(); });
                progress = 0;
                modal.Message("Preparing to process...").ProgressSpin().Show();
                await Task.Delay(1500);
                window.setTimeout(ProgressFrame, 16);
            }

            _content = SectionStack()
                .Title(SampleHeader(nameof(ProgressModalSample)))
                .Section(Stack().Children(
                    SampleTitle("Overview"),
                    TextBlock(
                        "TODO"))
                )
                .Section(Stack().Children(
                    SampleTitle("Best Practices"),
                    Stack().Horizontal().Children(
                        Stack().Children(
                            SampleSubTitle("Do"),
                            SampleDo("TODO")
                        ),
                        Stack().Children(
                            SampleSubTitle("Don't"),
                            SampleDont("TODO")
                        ))
                ))
                .Section(
                    Stack().Width(400.px()).Children(
                        SampleTitle("Usage"),
                        Button("Open Modal").OnClick((s, e) => PlayModal().FireAndForget())
                    ));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
