using System;
using Tesserae.Components;
using static H5.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class EditableLabelSample : IComponent
    {
        private IComponent _content;

        public EditableLabelSample()
        {
            _content = SectionStack()
                        .Title(SampleHeader(nameof(EditableLabelSample)))
                        .Section(Stack()
                                  .Children(SampleTitle("Overview"),
                                            TextBlock("Use for showing information that can be edited by users.")))
                      .Section(Stack()
                                  .Children(SampleTitle("Best Practices"),
                                            Stack()
                                               .Horizontal()
                                               .Children(Stack()
                                                         .Width(40.percent())
                                                         .Children(SampleSubTitle("Do"),
                                                                  SampleDo("Use anywhere information can be edited easily by users.")),
                                                         Stack()
                                                         .Width(40.percent())
                                                         .Children(SampleSubTitle("Don't"),
                                                           SampleDont("Don’t forget to register a OnSave() callback")))))
                      .Section(Stack().Children(
                                                SampleTitle("Usage"),
                                                TextBlock("Label").Medium(),
                                                EditableLabel("You can click to edit me"),
                                                EditableLabel("You can also change the font-size").Large(),
                                                EditableLabel("and weight as a normal label").Large().Bold(),
                                                TextBlock("Text Area").Medium(),
                                                EditableArea("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.\nUt enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.\nDuis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.\nExcepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.").Width(300.px())
                                               ));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
