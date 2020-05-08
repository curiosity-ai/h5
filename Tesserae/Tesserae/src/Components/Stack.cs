using static H5.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{

    /// <summary>
    /// A Stack is a container-type component that abstracts the implementation of a flexbox in order to define the layout of its children components.
    /// </summary>
    public class Stack : IContainer<Stack, IComponent>, IHasBackgroundColor, IHasMarginPadding, ISpecialCaseStyling
    {
        public Orientation StackOrientation
        {
            get
            {
                switch (InnerElement.style.flexDirection)
                {
                    case "row": return Orientation.Horizontal;
                    case "column": return Orientation.Vertical;
                    case "row-reverse": return Orientation.HorizontalReverse;
                    case "column-reverse": return Orientation.VerticalReverse;
                }

                return Orientation.Vertical;
            }
            set
            {
                switch (value)
                {
                    case Orientation.Horizontal: InnerElement.style.flexDirection = "row"; break;
                    case Orientation.Vertical: InnerElement.style.flexDirection = "column"; break;
                    case Orientation.HorizontalReverse: InnerElement.style.flexDirection = "row-reverse"; break;
                    case Orientation.VerticalReverse: InnerElement.style.flexDirection = "column-reverse"; break;
                }
            }
        }

        public bool CanWrap
        {
            get => InnerElement.style.flexWrap != "nowrap";
            set => InnerElement.style.flexWrap = value ? "wrap" : "nowrap";
        }

        public HTMLElement InnerElement { get; private set; }
        public string Background { get => InnerElement.style.background; set => InnerElement.style.background = value; }
        public string Margin { get => InnerElement.style.margin; set => InnerElement.style.margin = value; }
        public string Padding { get => InnerElement.style.padding; set => InnerElement.style.padding = value; }

        public HTMLElement StylingContainer => InnerElement;

        public bool PropagateToStackItemParent => true;

        public static void SetAlign(IComponent component, ItemAlign align)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            string cssAlign = align.ToString().ToLower();
            if (cssAlign == "end" || cssAlign == "start") cssAlign = $"flex-{cssAlign}";
            correct.item.style.alignSelf = cssAlign;
            if(correct.remember) correct.item.setAttribute("tss-stk-as", "");
        }

        /// <summary>
        /// Sets the align-items css property for this stack
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        public Stack AlignItems(ItemAlign align)
        {
            string cssAlign = align.ToString().ToLower();
            if (cssAlign == "end" || cssAlign == "start") cssAlign = $"flex-{cssAlign}";
            InnerElement.style.alignItems = cssAlign;
            return this;
        }

        /// <summary>
        /// Sets the align-items css property for this stack
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        public Stack AlignContent(ItemAlign align)
        {
            string cssAlign = align.ToString().ToLower();
            if (cssAlign == "end" || cssAlign == "start") cssAlign = $"flex-{cssAlign}";
            InnerElement.style.alignContent = cssAlign;
            return this;
        }

        /// <summary>
        /// Sets the justify-content css property for this stack
        /// </summary>
        /// <param name="justify"></param>
        /// <returns></returns>
        public Stack JustifyContent(ItemJustify justify)
        {
            string cssJustify = justify.ToString().ToLower();
            if (cssJustify == "end" || cssJustify == "start") cssJustify = $"flex-{cssJustify}";
            if (cssJustify == "between" || cssJustify == "around" || cssJustify == "evenly") cssJustify = $"space-{cssJustify}";
            InnerElement.style.justifyContent = cssJustify;
            return this;
        }

        /// <summary>
        /// Sets the justify-content css property for this stack
        /// </summary>
        /// <param name="justify"></param>
        /// <returns></returns>
        public Stack JustifyItems(ItemJustify justify)
        {
            string cssJustify = justify.ToString().ToLower();
            if (cssJustify == "end" || cssJustify == "start") cssJustify = $"flex-{cssJustify}";
            if (cssJustify == "between" || cssJustify == "around" || cssJustify == "evenly") cssJustify = $"space-{cssJustify}";
            InnerElement.style.justifyItems = cssJustify;
            return this;
        }

        public static void SetWidth(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.width = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-w","");
        }

        internal static (HTMLElement item, bool remember) GetCorrectItemToApplyStyle(IComponent component)
        {
            if (component is ISpecialCaseStyling specialCase)
            {
                return (specialCase.StylingContainer, specialCase.PropagateToStackItemParent);
            }
            else
            {
                return (GetItem(component), true);
            }
        }

        public static void SetMinWidth(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.minWidth = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-mw", "");
        }

        public static void SetMaxWidth(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.maxWidth = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-mxw", "");
        }

        public static void SetHeight(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.height = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-h", "");
        }

        public static void SetMinHeight(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.minHeight = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-mh", "");
        }

        public static void SetMaxHeight(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.maxHeight = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-mxh", "");
        }

        public static void SetMarginLeft(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.marginLeft = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-m", "");
        }

        public static void SetMarginRight(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.marginRight = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-m", "");
        }

        public static void SetMarginTop(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.marginTop= unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-m", "");
        }

        public static void SetMarginBottom(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.marginBottom = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-m", "");
        }


        public static void SetPaddingLeft(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.paddingLeft = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-p", "");
        }

        public static void SetPaddingRight(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.paddingRight = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-p", "");
        }

        public static void SetPaddingTop(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.paddingTop = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-p", "");
        }

        public static void SetPaddingBottom(IComponent component, UnitSize unitSize)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.paddingBottom = unitSize.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-p", "");
        }


        public static void SetGrow(IComponent component, int grow)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.flexGrow = grow.ToString();
            if(correct.remember) correct.item.setAttribute("tss-stk-fg", "");
        }

        public static void SetShrink(IComponent component, bool shrink)
        {
            var correct = GetCorrectItemToApplyStyle(component);
            correct.item.style.flexShrink = shrink ? "1" : "0";
            if(correct.remember) correct.item.setAttribute("tss-stk-fs", "");
        }

        public Stack(Orientation orientation = Orientation.Vertical)
        {
            InnerElement = Div(_("tss-stack"));
            this.StackOrientation = orientation;
        }

        public void Add(IComponent component)
        {
            ScrollBar.GetCorrectContainer(InnerElement).appendChild(GetItem(component, true));
        }

        public virtual void Clear()
        {
            ClearChildren(ScrollBar.GetCorrectContainer(InnerElement));
        }

        public void Replace(IComponent newComponent, IComponent oldComponent)
        {
            ScrollBar.GetCorrectContainer(InnerElement).replaceChild(GetItem(newComponent), GetItem(oldComponent));
        }

        public virtual HTMLElement Render()
        {
            return InnerElement;
        }

        public Stack Horizontal()
        {
            StackOrientation = Stack.Orientation.Horizontal;
            return this;
        }

        public Stack Vertical()
        {
            StackOrientation = Stack.Orientation.Vertical;
            return this;
        }

        public Stack HorizontalReverse()
        {
            StackOrientation = Stack.Orientation.HorizontalReverse;
            return this;
        }

        public Stack VerticalReverse()
        {
            StackOrientation = Stack.Orientation.VerticalReverse;
            return this;
        }

        public Stack Wrap()
        {
            CanWrap = true;
            return this;
        }

        public Stack NoWrap()
        {
            CanWrap = false;
            return this;
        }

        internal static HTMLElement GetItem(IComponent component, bool forceAdd = false)
        {
            if (!((component as dynamic).StackItem is HTMLElement item))
            {
                var rendered = component.Render();
                if (forceAdd || (rendered.parentElement is object && rendered.parentElement.classList.contains("tss-stack")))
                {
                    item = Div(_("tss-stack-item", styles: s =>
                    {
                        s.alignSelf = "auto";
                        s.width = "auto";
                        s.height = "auto";
                        s.flexShrink = "1";
                    }), component.Render());
                    (component as dynamic).StackItem = item;

                    if (forceAdd)
                    {
                        CopyStylesDefinedWithExtension(rendered, item);
                    }

                }
                else
                {
                    item = rendered;
                }
            }
            return item;
        }

        internal static void CopyStylesDefinedWithExtension(HTMLElement from, HTMLElement to)
        {
            // RFO: this class does some magic to move any styles applied to an element using the extensions methods like Width, etc... to the actual StackItem HTML element
            // so that they're relevant on the flex-box and not only inside of each child item of the flexbox

            var fs = from.style;
            var ts = to.style;

            bool has(string att)
            {
                bool ha = from.hasAttribute(att);
                if (ha)
                {
                    from.removeAttribute(att);
                }
                return ha;
            }

            if (has("tss-stk-w"))   { ts.width     = fs.width;  fs.width = "100%"; }
            if (has("tss-stk-mw"))  { ts.minWidth  = fs.minWidth; fs.minWidth = "100%"; }
            if (has("tss-stk-mxw")) { ts.maxWidth  = fs.maxWidth; fs.maxWidth = "100%"; }
            if (has("tss-stk-h"))   { ts.height    = fs.height; fs.height = "100%"; }
            if (has("tss-stk-mh"))  { ts.minHeight = fs.minHeight; fs.minHeight = "100%"; }
            if (has("tss-stk-mxh")) { ts.maxHeight = fs.maxHeight; fs.maxHeight = "100%"; }

            if (has("tss-stk-m"))
            {
                ts.marginLeft   = fs.marginLeft;
                ts.marginTop    = fs.marginTop;
                ts.marginRight  = fs.marginRight;
                ts.marginBottom = fs.marginBottom;
                fs.marginLeft   = fs.marginTop = fs.marginRight = fs.marginBottom = "";
            }

            if (has("tss-stk-p"))
            {
                ts.paddingLeft   = fs.paddingLeft;
                ts.paddingTop    = fs.paddingTop;
                ts.paddingRight  = fs.paddingRight;
                ts.paddingBottom = fs.paddingBottom;
                fs.paddingLeft   = fs.paddingTop = fs.paddingRight = fs.paddingBottom = "";
            }

            //TODO: check if should clear this here:
            if (has("tss-stk-fg")) { ts.flexGrow   = fs.flexGrow;  /*fs.flexGrow = ""; */}
            if (has("tss-stk-fs")) { ts.flexShrink = fs.flexShrink;  /*fs.flexShrink = ""; */}
            if (has("tss-stk-as")) { ts.alignSelf  = fs.alignSelf; /*fs.alignSelf = "";*/ }
        }

        public enum Orientation
        {
            Vertical,
            Horizontal,
            VerticalReverse,
            HorizontalReverse,
        }



        public struct ItemSize
        {
            public Unit Type { get; set; }
            public float Value { get; set; }
        }
    }
    public enum ItemAlign
    {
        Auto,
        Stretch,
        Baseline,
        Start,
        Center,
        End
    }

    public enum ItemJustify
    {
        Between,
        Around,
        Evenly,
        Start,
        Center,
        End
    }
}
