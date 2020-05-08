using System;
using Tesserae.HTML;
using static H5.dom;

namespace Tesserae
{
    public static partial class UI
    {
        //Overloads for most used cases:
        public static Attributes _() => new Attributes();
        public static Attributes _(string className) => new Attributes() { ClassName = className };

        public static Attributes _(string className                         = null,
                                    string id                               = null,
                                    string src                              = null,
                                    string href                             = null,
                                    string rel                              = null,
                                    string target                           = null,
                                    string text                             = null,
                                    string type                             = null,
                                    bool?  disabled                         = null,
                                    string value                            = null,
                                    string placeholder                      = null,
                                    string defaultValue                     = null,
                                    string title                            = null,
                                    Action<HTMLElement> el                  = null,
                                    Action<CSSStyleDeclaration> styles      = null)
        {
            var a = new Attributes();

            a.ClassName       = className;
            a.Id              = id;
            a.OnElementCreate = el;
            a.Styles          = styles;

            //TODO: remove all of this too:
            a.Title  = title;
            a.Href   = href;
            a.Src    = src;
            a.Rel    = rel;
            a.Target = target;

            a.Text         = text;
            a.Type         = type;
            a.Disabled     = disabled;
            a.Value        = value;
            a.DefaultValue = defaultValue;
            a.Placeholder  = placeholder;

            return a;
        }
    }
}
