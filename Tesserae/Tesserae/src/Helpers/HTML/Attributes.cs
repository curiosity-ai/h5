using System;
using System.Collections.Generic;
using System.Linq;
using static H5.Core.dom;

namespace Tesserae.HTML
{
    public sealed class Attributes
    {
        private readonly List<(string attributeName, string attributeValue)> _data;

        public Attributes()
        {
            _data = new List<(string name, string value)>();
        }

        public string ClassName { get; internal set; }
        public string Id { get; internal set; }
        public string Title { get; internal set; }

        public Action<HTMLElement>         OnElementCreate = null;
        public Action<CSSStyleDeclaration> Styles = null;

        public string Href         { get; internal set; }
        public string Src          { get; internal set; }
        public string Rel          { get; internal set; }
        public string Target       { get; internal set; }
        public string Text         { get; internal set; }
        public string Type         { get; internal set; }
        public bool?  Disabled     { get; internal set; }
        public string Value        { get; internal set; }
        public string DefaultValue { get; internal set; }
        public string Placeholder  { get; internal set; }
        public string Alt          { get; internal set; }
        public string Role         { get; internal set; }

        public IEnumerable<(string name, string value)> Data => _data.AsReadOnly();

        public void InitElement(HTMLElement element)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                var lines = Text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n'); // Normalise line returns and break on them
                for (var i = 0; i < lines.Length; i++)
                {
                    if (i > 0)
                        element.appendChild(document.createElement("br"));
                    element.appendChild(document.createTextNode(lines[i]));
                }
            }

            if (!string.IsNullOrEmpty(Id))        { element.id = Id; }
            if (!string.IsNullOrEmpty(ClassName)) { element.className = ClassName; }
            if (!string.IsNullOrEmpty(Title))     { element.title = Title; }
            if (!string.IsNullOrEmpty(Alt))       { element.setAttribute("alt", Role); }
            if (!string.IsNullOrEmpty(Role))      { element.setAttribute("role", Role); }

            foreach (var dataAttribute in _data)
            {
                element.setAttribute($"data-{dataAttribute.attributeName}", dataAttribute.attributeValue);
            }

            Styles?.Invoke(element.style);
            OnElementCreate?.Invoke(element);
        }

        public void InitAnchorElement(HTMLAnchorElement element)
        {
            InitElement(element);

            if (!string.IsNullOrEmpty(Href))   { element.href = Href; }
            if (!string.IsNullOrEmpty(Rel))    { element.rel = Rel; }
            if (!string.IsNullOrEmpty(Target)) { element.target = Target; }
            if (!string.IsNullOrEmpty(@Type)) { element.type = @Type; }
        }

        public void InitButtonElement(HTMLButtonElement element)
        {
            InitElement(element);
            if (!string.IsNullOrEmpty(@Type)) { element.type = @Type; }
        }

        public void InitImageElement(HTMLImageElement element)
        {
            InitElement(element);
            if (!string.IsNullOrEmpty(Src)) { element.src = Src; }
        }

        public void InitInputElement(HTMLInputElement element)
        {
            InitElement(element);
            if (!string.IsNullOrEmpty(Placeholder))  { element.placeholder = Placeholder; }
            if (!string.IsNullOrEmpty(DefaultValue)) { element.defaultValue = DefaultValue; }
            if (Disabled.HasValue)                   { element.disabled = Disabled.Value; }
            if (!string.IsNullOrEmpty(Value))        { element.value = Value; }
            if (!string.IsNullOrEmpty(@Type))        { element.type = @Type; }
        }

        public void InitIFrameElement(HTMLIFrameElement element)
        {
            InitElement(element);
            if (!string.IsNullOrEmpty(Src)) { element.src = Src; }
        }

        public void InitOptionElement(HTMLOptionElement element)
        {
            InitElement(element);
            if (Disabled.HasValue) { element.disabled = Disabled.Value; }
            if (!string.IsNullOrEmpty(Value)) { element.value = Value; }
        }

        public void InitTextAreaElement(HTMLTextAreaElement element)
        {
            InitElement(element);
            if (!string.IsNullOrEmpty(Placeholder))  { element.placeholder = Placeholder; }
            if (!string.IsNullOrEmpty(DefaultValue)) { element.defaultValue = DefaultValue; }
            if (Disabled.HasValue)                   { element.disabled = Disabled.Value; }
            if (!string.IsNullOrEmpty(Value))        { element.value = Value; }
        }

        public Attributes WithAlt(string alt)
        {
            if (string.IsNullOrWhiteSpace(alt))
            {
                throw new ArgumentException(nameof(alt));
            }

            Alt = alt;

            return this;
        }

        public Attributes WithRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                throw new ArgumentException(nameof(role));
            }

            Role = role;

            return this;
        }

        public Attributes WithData(string attributeName, string attributeValue)
        {
            if (string.IsNullOrWhiteSpace(attributeName))
            {
                throw new ArgumentException(nameof(attributeName));
            }

            if (string.IsNullOrWhiteSpace(attributeValue))
            {
                throw new ArgumentException(attributeValue);
            }

            _data.Add((attributeName, attributeValue));

            return this;
        }
    }
}
