using System;
using System.Collections.Generic;
using System.Linq;
using HighFive;
using Tesserae.Components;
using Tesserae.HTML;
using static H5.dom;

namespace Tesserae
{
    public static partial class UI
    {
        public static HTMLElement TryToFindAncestor(this HTMLElement source, string tagNameToFind)
        {
            while (source.parentElement != null)
            {
                if (source.parentElement.tagName.ToUpper() == tagNameToFind)
                    return source.parentElement;
                source = source.parentElement;
            }
            return null;
        }

        public static bool IsMounted(this HTMLElement source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var top = document.querySelector("html");
            while (source != null)
            {
                if (source == top)
                    return true;
                source = source.parentElement;
            }
            return false;
        }

        public static void AppendChildren(this HTMLElement source, params HTMLElement[] children)
        {
            foreach (var child in children)
                source.appendChild(child);
        }

        public static void StopEvent(Event e)
        {
            e.preventDefault();
            e.stopImmediatePropagation();
        }

        public static List<Element> ToList(HTMLCollection c)
        {
            var l = new List<Element>();
            for (uint i = 0; i < c.length; i++)
            {
                l.Add(c.item(i));
            }
            return l;
        }

        public static void RemoveChildElements(this HTMLElement source)
        {
            while (source.firstElementChild != null)
                source.firstElementChild.remove();
        }

        public static HTMLElement UpdateClassIf(this HTMLElement htmlElement, bool value, string cssClass)
        {
            if (value)
            {
                htmlElement.classList.add(cssClass);
            }
            else
            {
                htmlElement.classList.remove(cssClass);
            }

            return htmlElement;
        }

        public static HTMLElement UpdateClassIfNot(this HTMLElement htmlElement, bool value, string cssClass)
        {
            if (!value)
            {
                htmlElement.classList.add(cssClass);
            }
            else
            {
                htmlElement.classList.remove(cssClass);
            }

            if(cssClass == "tss-disabled" && !Script.Write<bool>("(typeof {0}.disabled === 'undefined')", htmlElement))
            {
                Script.Write("{0}.disabled = {1}", htmlElement, !value);
            }

            return htmlElement;
        }

        public static HTMLElement RemoveClassIf(this HTMLElement htmlElement, bool value, string cssClass)
        {
            if (value)
            {
                htmlElement.classList.remove(cssClass);
            }

            return htmlElement;
        }

        public static (TextSize? textSize, string currentTextSizeCssClass) GetTextSize(this HTMLElement htmlElement)
        {
            return GetCssClassValue<TextSize>(htmlElement, "tss-fontsize-");
        }

        public static (TextWeight? textWeight, string currentTextWeightCssClass) GetTextWeight(this HTMLElement htmlElement)
        {
            return GetCssClassValue<TextWeight>(htmlElement, "tss-fontweight-");
        }

        public static (TextAlign? textAlign, string currentTextAlignCssClass) GetTextAlign(this HTMLElement htmlElement)
        {
            return GetCssClassValue<TextAlign>(htmlElement, "tss-textalign-");
        }

        private static (T? value, string currentCssClass) GetCssClassValue<T>(HTMLElement htmlElement, string cssClass) where T : struct
        {
            var currentCssClass = htmlElement.classList.FirstOrDefault(t => t.StartsWith(cssClass));

            if (currentCssClass != null && Enum.TryParse<T>(currentCssClass.Substring(cssClass.Length), true, out var value))
            {
                return (value, currentCssClass);
            }

            return (null, string.Empty);
        }

        public static void ReplaceElement(HTMLElement source, HTMLElement replaceWith)
        {
            if (source.parentElement == null)
            {
                // If the element is isn't (or is no longer) mounted then do nothing (this may happen if an element is rendered and then async work is required to populate
                // some data in it and then the element is destroyed before the async data comes back)
                return;
            }
            source.parentElement.replaceChild(replaceWith, source);
        }

        public static void AppendElements(HTMLElement parent, IEnumerable<HTMLElement> children)
        {
            if (children != null)
            {
                foreach (var child in children)
                {
                    parent.appendChild(child);
                }
            }
        }

        public static void AppendElements(HTMLElement parent, params HTMLElement[] children)
        {
            if(children != null)
            {
                foreach (var child in children)
                {
                    if(child != null)
                    {
                        parent.appendChild(child);
                    }
                }
            }
        }

        public static void ClearChildren(HTMLElement element)
        {
            while (element.lastChild != null)
            {
                element.removeChild(element.lastChild);
            }
        }

        public static HTMLElement AppendClass(HTMLElement element, params string[] classes)
        {
            element.classList.add(classes);
            return element;
        }

        public static HTMLElement SetStyle(HTMLElement element, Action<CSSStyleDeclaration> style)
        {
            style(element.style);
            return element;
        }

        public static T SetStyle<T>(this T element, Action<CSSStyleDeclaration> style) where T : HTMLElement
        {
            style(element.style);
            return element;
        }

        [IgnoreGeneric] // 2019-07-10 DWR: This is needed so that we can call it with types such as HTMLTableHeaderCellElement that seem to be [External] (and so can't be passed as a generic type param at runtime)
        static T InitElement<T>(T element, Attributes init, params HTMLElement[] children) where T : HTMLElement
        {
            init?.InitElement(element);
            if (children != null)
            {
                if (Script.Write<bool>("Array.isArray(children)"))
                {
                    AppendElements(element, children);
                }
                else
                {
                    element.appendChild(children);
                }
            }
            return element;
        }

        public static HTMLElement Raw(string html, bool forceParseAsHTML = false)
        {
            if (string.IsNullOrWhiteSpace(html))
                return SPAN();

            if (IsProbablyHtml(html) || forceParseAsHTML)
            {
                var wrapper = new HTMLSpanElement();
                wrapper.innerHTML = html;
                return wrapper;
            }
            else
            {
                var text = new HTMLSpanElement();
                text.textContent = html;
                return text;
            }
        }

        private static bool IsProbablyHtml(string html)
        {
            //Super simple heuristic to detect if text contains any <> html tags
            var open = html.IndexOf('<');
            if (open >= 0)
            {
                var close = html.IndexOf('>', open);
                if (close >= 1) return true;
            }

            if (html.IndexOf("&nbsp;") >= 0)
            {
                return true;
            }

            return false;
        }

        public static Text Text(string text)
        {
            return document.createTextNode(text);
        }

        public static HTMLDivElement Div(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLDivElement(), init, children);
        }

        public static HTMLSpanElement Span(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLSpanElement(), init, children);
        }

        public static HTMLParagraphElement P(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLParagraphElement(), init, children);
        }

        public static HTMLPreElement Pre(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLPreElement(), init, children);
        }

        public static HTMLElement Strong(Attributes init, params HTMLElement[] children)
        {
            return InitElement(document.createElement("strong"), init, children);
        }

        public static HTMLElement I(Attributes init, params HTMLElement[] children)
        {
            return InitElement(document.createElement("i"), init, children);
        }

        public static HTMLElement I(LineAwesome icon, LineAwesomeWeight size = LineAwesomeWeight.Default, string cssClass = null)
        {
            return I(_($"{size} {icon} {cssClass}"));
        }

        public static HTMLElement Sup(Attributes init, params HTMLElement[] children)
        {
            return InitElement(document.createElement("sup"), init, children);
        }

        public static HTMLBRElement Br(Attributes init)
        {
            return InitElement(new HTMLBRElement(), init);
        }

        public static HTMLOListElement Ol(Attributes init, params HTMLLIElement[] children)
        {
            var a = new HTMLOListElement();
            AppendElements(a, children);
            return a;
        }

        public static HTMLUListElement Ul(Attributes init, params HTMLLIElement[] children)
        {
            return InitElement(new HTMLUListElement(), init, children);
        }

        public static HTMLLIElement Li(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLLIElement(), init, children);
        }

        public static HTMLElement Main(Attributes init, params HTMLElement[] children)
        {
            return InitElement(document.createElement("main"), init, children);
        }

        public static HTMLElement Header(Attributes init, params HTMLElement[] children)
        {
            return InitElement(document.createElement("header"), init, children);
        }

        public static HTMLElement Footer(Attributes init, params HTMLElement[] children)
        {
            return InitElement(document.createElement("footer"), init, children);
        }

        public static HTMLElement Section(Attributes init, params HTMLElement[] children)
        {
            return InitElement(document.createElement("section"), init, children);
        }

        public static HTMLElement Nav(Attributes init, params HTMLElement[] children)
        {
            return InitElement(document.createElement("nav"), init, children);
        }

        public static HTMLHRElement HR(Attributes init)
        {
            return InitElement(new HTMLHRElement(), init);
        }

        public static HTMLAnchorElement A(Attributes init, params HTMLElement[] children)
        {
            var a = new HTMLAnchorElement();
            init?.InitAnchorElement(a);
            AppendElements(a, children);
            return a;
        }

        public static HTMLImageElement Image(Attributes init)
        {
            var a = new HTMLImageElement();
            init?.InitImageElement(a);
            return a;
        }

        public static HTMLCanvasElement Canvas(Attributes init)
        {
            return InitElement(new HTMLCanvasElement(), init);
        }

        static HTMLElement H(Attributes init, string type, params HTMLElement[] children)
        {
            var h = document.createElement(type);
            init?.InitElement(h);
            AppendElements(h, children);
            return h;
        }

        public static HTMLElement H1(Attributes init, params HTMLElement[] children)
        {
            return H(init, "h1", children);
        }

        public static HTMLElement H2(Attributes init, params HTMLElement[] children)
        {
            return H(init, "h2", children);
        }

        public static HTMLElement H3(Attributes init, params HTMLElement[] children)
        {
            return H(init, "h3", children);
        }

        public static HTMLElement H4(Attributes init, params HTMLElement[] children)
        {
            return H(init, "h4", children);
        }

        public static HTMLElement H5(Attributes init, params HTMLElement[] children)
        {
            return H(init, "h5", children);
        }

        public static HTMLElement H6(Attributes init, params HTMLElement[] children)
        {
            return H(init, "h6", children);
        }

        public static HTMLFormElement FormE(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLFormElement(), init, children);
        }

        public static HTMLFieldSetElement FieldSet(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLFieldSetElement(), init, children);
        }

        public static HTMLOptGroupElement OptGroup(Attributes init, params HTMLOptionElement[] children)
        {
            return InitElement(new HTMLOptGroupElement(), init, children);
        }

        public static HTMLOptionElement Option(Attributes init)
        {
            var f = new HTMLOptionElement();
            init?.InitOptionElement(f);
            return f;
        }

        public static HTMLLegendElement Legend(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLLegendElement(), init, children);
        }

        public static HTMLButtonElement Button(Attributes init, params HTMLElement[] children)
        {
            var f = new HTMLButtonElement();
            init?.InitButtonElement(f);
            AppendElements(f, children);
            return f;
        }

        public static HTMLSelectElement Select(Attributes init, params HTMLOptionElement[] children)
        {
            return InitElement(new HTMLSelectElement(), init, children);
        }

        public static HTMLDataListElement DataList(Attributes init, params HTMLOptionElement[] children)
        {
            var f = new HTMLDataListElement();
            AppendElements(f, children);
            return f;
        }

        public static HTMLInputElement TextBox(Attributes init)
        {
            var f = new HTMLInputElement();
            init?.InitInputElement(f);
            return f;
        }

        public static HTMLInputElement CheckBox(Attributes init)
        {
            var input = new HTMLInputElement();
            input.type = "checkbox";
            init?.InitInputElement(input);
            return input;
        }

        public static HTMLInputElement RadioButton(Attributes init)
        {
            var input = new HTMLInputElement();
            input.type = "radio";
            init?.InitInputElement(input);
            return input;
        }

        public static HTMLInputElement FileInput(Attributes init)
        {
            var input = new HTMLInputElement();
            input.type = "file";
            init?.InitInputElement(input);
            return input;
        }

        public static HTMLTextAreaElement TextArea(Attributes init)
        {
            var a = new HTMLTextAreaElement();
            init?.InitTextAreaElement(a);
            return a;
        }

        public static HTMLLabelElement Label(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLLabelElement(), init, children);
        }

        public static HTMLTableElement Table(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLTableElement(), init, children);
        }

        public static HTMLTableCaptionElement Caption(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLTableCaptionElement(), init, children);
        }

        public static HTMLElement THeader(Attributes init, params HTMLTableRowElement[] children)
        {
            return InitElement(document.createElement("thead"), init, children);
        }

        public static HTMLTableSectionElement TBody(Attributes init, params HTMLTableRowElement[] children)
        {
            return InitElement(new HTMLTableSectionElement(), init, children);
        }

        public static HTMLTableSectionElement TFooter(Attributes init, params HTMLTableRowElement[] children)
        {
            return InitElement(new HTMLTableSectionElement(), init, children);
        }

        public static HTMLTableRowElement TRow(Attributes init, params HTMLTableDataCellElement[] children)
        {
            return InitElement(new HTMLTableRowElement(), init, children);
        }

        public static HTMLTableRowElement TRow(Attributes init, params HTMLTableHeaderCellElement[] children)
        {
            return InitElement(new HTMLTableRowElement(), init, children);
        }

        public static HTMLTableDataCellElement Td(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLTableDataCellElement(), init, children);
        }

        public static HTMLTableHeaderCellElement Th(Attributes init, params HTMLElement[] children)
        {
            return InitElement(new HTMLTableHeaderCellElement(), init, children);
        }

        public static HTMLIFrameElement IFrame(Attributes init, params HTMLElement[] children)
        {
            var a = new HTMLIFrameElement();
            init?.InitIFrameElement(a);
            AppendElements(a, children);
            return a;
        }

        public static HTMLDivElement DIV(params HTMLElement[] child)
        {
            return InitElement(new HTMLDivElement(), null, child);
        }

        public static HTMLSpanElement SPAN(params HTMLElement[] child)
        {
            return InitElement(new HTMLSpanElement(), null, child);
        }

        public static void MakeScrollableOnHover(HTMLElement element, int initialDelay = 1000, double pixelsPerSecond = 100)
        {
            double progress = 0;
            bool stop = false;
            double initial = 0;
            double maxDelta = 0;
            double t0 = -1;
            double ratio = 1;
            FrameRequestCallback animateElement = null;

            var target = ((HTMLElement)element.firstElementChild);
            animateElement = (t) =>
            {
                if(t0 < 0)
                {
                    t0 = t;
                    ratio = (pixelsPerSecond)/1000;
                }
                progress = (t-t0) * ratio;
                if (progress > maxDelta || stop)
                {
                    if (stop)
                    {
                        target.style.marginLeft = "";
                    }
                    else
                    {
                        target.style.marginLeft = "-" + maxDelta + "px";
                    }
                }
                else
                {
                    target.style.marginLeft = "-" + (progress) + "px";
                    window.requestAnimationFrame(animateElement);
                }
            };

            element.onmouseenter = (e) =>
            {
                stop = false;
                progress = 0;
                t0 = -1;
                maxDelta = -((DOMRect)element.getBoundingClientRect()).width;

                foreach (var c in element.children)
                {
                    maxDelta += ((DOMRect)c.getBoundingClientRect()).width;
                }

                if(maxDelta > 0)
                {
                    initial = window.setTimeout((t) =>
                    {
                        window.requestAnimationFrame(animateElement);
                    }, initialDelay);
                }
            };

            element.onmouseleave = (e) =>
            {
                stop = true;
                window.clearTimeout(initial);
                target.style.marginLeft = "";
            };
        }
    }
}
