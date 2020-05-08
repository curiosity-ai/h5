using System;
using Tesserae.Components;
using static H5.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;
using System.Collections.Generic;
using System.Linq;

namespace Tesserae.Tests.Samples
{
    public class PivotSample : IComponent
    {
        private IComponent content;

        public PivotSample()
        {
            content = SectionStack()
                     .Title(SampleHeader(nameof(PivotSample)))
                     .Section(Stack().Children(
                                  SampleTitle("Overview"),
                                  TextBlock("TODO"),
                                  TextBlock("Examples of experiences that use Panels").MediumPlus()))
                     .Section(Stack().Children(
                                  SampleTitle("Best Practices"),
                                  Stack().Horizontal().Children(
                                      Stack().Width(40.percent()).Children(
                                          SampleSubTitle("Do"),
                                          SampleDo("TODO")
                                      ),
                                      Stack().Width(40.percent()).Children(
                                          SampleSubTitle("Don't"),
                                          SampleDont("TODO")
                                      )
                                  )))
                     .Section(Stack().Children(
                                  SampleTitle("Usage"),
                                  SampleSubTitle("Cached vs. Not Cached Tabs"),
                                  Pivot().Pivot("tab1", () => Button().SetText("Cached").NoBorder().NoBackground().Link(),
                                                () => TextBlock(DateTimeOffset.UtcNow.ToString()).Regular(), cached: true)
                                         .Pivot("tab2",                                                      () => Button().SetText("Not Cached").SetIcon("las la-sync").NoBorder().NoBackground().Link(),
                                                () => TextBlock(DateTimeOffset.UtcNow.ToString()).Regular(), cached: false),
                                  SampleSubTitle("Cached vs. Not Cached Tabs"),
                                  SampleSubTitle("Scroll with limited height"),
                                  Pivot().MaxHeight(500.px())
                                         .Pivot("tab1",                                                  () => Button().SetText("5 Items").NoBorder().NoBackground().Link(),
                                                () => ItemsList(GetSomeItems(5)).PaddingBottom(16.px()), cached: true)
                                         .Pivot("tab2",                                                   () => Button().SetText("10 Items").NoBorder().NoBackground().Link(),
                                                () => ItemsList(GetSomeItems(20)).PaddingBottom(16.px()), cached: true)
                                         .Pivot("tab3",                                                   () => Button().SetText("50 Items").NoBorder().NoBackground().Link(),
                                                () => ItemsList(GetSomeItems(50)).PaddingBottom(16.px()), cached: true)
                                         .Pivot("tab4",                                                    () => Button().SetText("100 Items").NoBorder().NoBackground().Link(),
                                                () => ItemsList(GetSomeItems(100)).PaddingBottom(16.px()), cached: true),
                                  SampleSubTitle("Too many tabs (WIP)"),
                                  Pivot().MaxHeight(500.px()).MaxWidth(300.px())
                                         .Pivot("tab1",                                                  () => Button().SetText("5 Items").NoBorder().NoBackground().Link().Ellipsis(),
                                                () => ItemsList(GetSomeItems(5)).PaddingBottom(16.px()), cached: true)
                                         .Pivot("tab2",                                                   () => Button().SetText("10 Items").NoBorder().NoBackground().Link().Ellipsis(),
                                                () => ItemsList(GetSomeItems(20)).PaddingBottom(16.px()), cached: true)
                                         .Pivot("tab3",                                                   () => Button().SetText("50 Items").NoBorder().NoBackground().Link().Ellipsis(),
                                                () => ItemsList(GetSomeItems(50)).PaddingBottom(16.px()), cached: true)
                                         .Pivot("tab4",                                                    () => Button().SetText("100 Items").NoBorder().NoBackground().Link().Ellipsis(),
                                                () => ItemsList(GetSomeItems(100)).PaddingBottom(16.px()), cached: true)
                                         .Pivot("tab5",                                                  () => Button().SetText("5 Items").NoBorder().NoBackground().Link().Ellipsis(),
                                                () => ItemsList(GetSomeItems(5)).PaddingBottom(16.px()), cached: true)
                                         .Pivot("tab6",                                                   () => Button().SetText("10 Items").NoBorder().NoBackground().Link().Ellipsis(),
                                                () => ItemsList(GetSomeItems(20)).PaddingBottom(16.px()), cached: true)
                                         .Pivot("tab7",                                                   () => Button().SetText("50 Items").NoBorder().NoBackground().Link().Ellipsis(),
                                                () => ItemsList(GetSomeItems(50)).PaddingBottom(16.px()), cached: true)
                                         .Pivot("tab8",                                                    () => Button().SetText("100 Items").NoBorder().NoBackground().Link().Ellipsis(),
                                                () => ItemsList(GetSomeItems(100)).PaddingBottom(16.px()), cached: true)
                              ));
        }

        public HTMLElement Render()
        {
            return content.Render();
        }

        private IComponent[] GetSomeItems(int count)
        {
            return Enumerable
                  .Range(1, count)
                  .Select(number => Card(TextBlock($"Lorem Ipsum {number}").NonSelectable()).MinWidth(200.px()))
                  .ToArray();
        }
    }
}