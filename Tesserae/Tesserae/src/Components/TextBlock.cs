using H5;
using System;
using System.Linq;
using static H5.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public class TextBlock : ComponentBase<TextBlock, HTMLElement>, IHasTextSize, IHasBackgroundColor, IHasForegroundColor
    {
        public TextBlock(string text = string.Empty)
        {
            text = text ?? string.Empty;
            InnerElement = Div(_("tss-textBlock tss-fontsize-small tss-fontweight-regular", text: text));
            AttachClick();
        }

        public string Background { get => InnerElement.style.background; set => InnerElement.style.background = value; }

        public string Foreground { get => InnerElement.style.color; set => InnerElement.style.color = value; }

        public bool IsEnabled
        {
            get => !InnerElement.classList.contains("tss-disabled");
            set
            {
                if (value)
                {
                    InnerElement.classList.remove("tss-disabled");
                }
                else
                {
                    InnerElement.classList.add("tss-disabled");
                }
            }
        }

        public bool IsSelectable
        {
            get => InnerElement.style.userSelect != "none";
            set => InnerElement.style.userSelect = value ? "" : "none";
        }

        public virtual string Text
        {
            get => InnerElement.innerText;
            set => InnerElement.innerText = value;
        }

        public string Title
        {
            get => InnerElement.title;
            set => InnerElement.title = value;
        }

        public virtual TextSize Size
        {
            get => TextSizeExtensions.FromClassList(InnerElement, TextSize.Small);
            set
            {
                InnerElement.classList.remove(Size.ToClassName());
                InnerElement.classList.add(value.ToClassName());
            }
        }

        public virtual TextWeight Weight
        {
            get => TextSizeExtensions.FromClassList(InnerElement, TextWeight.Regular);
            set
            {
                InnerElement.classList.remove(Weight.ToClassName());
                InnerElement.classList.add(value.ToClassName());
            }
        }

        public TextAlign TextAlign
        {
            get
            {
                var curFontSize = InnerElement.classList.FirstOrDefault(t => t.StartsWith("tss-textalign-"));
                if (curFontSize is object && Enum.TryParse<TextAlign>(curFontSize.Substring("tss-textalign-".Length), true, out var result))
                {
                    return result;
                }
                else
                {
                    return TextAlign.Left;
                }
            }
            set
            {
                var curFontSize = InnerElement.classList.FirstOrDefault(t => t.StartsWith("tss-textalign-"));
                if (curFontSize is object)
                {
                    InnerElement.classList.remove(curFontSize);
                }
                InnerElement.classList.add($"tss-textalign-{value.ToString().ToLower()}");
            }
        }

        /// <summary>
        /// Gets or set whenever text block color is primary
        /// </summary>
        public bool IsPrimary
        {
            get => InnerElement.classList.contains("tss-fontcolor-primary");
            set
            {
                if (value)
                {
                    InnerElement.classList.add("tss-fontcolor-primary");
                    InnerElement.classList.remove("tss-fontcolor-invalid");
                    InnerElement.classList.remove("tss-fontcolor-success");
                    InnerElement.classList.remove("tss-fontcolor-secondary");
                    InnerElement.classList.remove("tss-fontcolor-danger");
                }
                else
                {
                    InnerElement.classList.remove("tss-fontcolor-invalid");
                    InnerElement.classList.remove("tss-fontcolor-success");
                    InnerElement.classList.remove("tss-fontcolor-danger");
                    InnerElement.classList.remove("tss-fontcolor-secondary");
                    InnerElement.classList.remove("tss-fontcolor-primary");
                }
            }
        }
        
        /// <summary>
         /// Gets or set whenever text block color is primary
         /// </summary>
        public bool IsSecondary
        {
            get => InnerElement.classList.contains("tss-fontcolor-secondary");
            set
            {
                if (value)
                {
                    InnerElement.classList.add("tss-fontcolor-secondary");
                    InnerElement.classList.remove("tss-fontcolor-primary");
                    InnerElement.classList.remove("tss-fontcolor-invalid");
                    InnerElement.classList.remove("tss-fontcolor-success");
                    InnerElement.classList.remove("tss-fontcolor-danger");
                }
                else
                {
                    InnerElement.classList.remove("tss-fontcolor-invalid");
                    InnerElement.classList.remove("tss-fontcolor-success");
                    InnerElement.classList.remove("tss-fontcolor-danger");
                    InnerElement.classList.remove("tss-fontcolor-secondary");
                    InnerElement.classList.remove("tss-fontcolor-primary");
                }
            }
        }

        /// <summary>
        /// Gets or set whenever text block color is success
        /// </summary>
        public bool IsSuccess
        {
            get => InnerElement.classList.contains("tss-fontcolor-success");
            set
            {
                if (value)
                {
                    InnerElement.classList.add("tss-fontcolor-success");
                    InnerElement.classList.remove("tss-fontcolor-invalid");
                    InnerElement.classList.remove("tss-fontcolor-primary");
                    InnerElement.classList.remove("tss-fontcolor-secondary");
                    InnerElement.classList.remove("tss-fontcolor-danger");
                }
                else
                {
                    InnerElement.classList.remove("tss-fontcolor-invalid");
                    InnerElement.classList.remove("tss-fontcolor-success");
                    InnerElement.classList.remove("tss-fontcolor-danger");
                    InnerElement.classList.remove("tss-fontcolor-secondary");
                    InnerElement.classList.remove("tss-fontcolor-primary");
                }
            }
        }

        /// <summary>
        /// Gets or set whenever text block color is danger
        /// </summary>
        public bool IsDanger
        {
            get => InnerElement.classList.contains("tss-fontcolor-danger");
            set
            {
                if (value)
                {
                    InnerElement.classList.add("tss-fontcolor-danger");
                    InnerElement.classList.remove("tss-fontcolor-invalid");
                    InnerElement.classList.remove("tss-fontcolor-primary");
                    InnerElement.classList.remove("tss-fontcolor-secondary");
                    InnerElement.classList.remove("tss-fontcolor-success");
                }
                else
                {
                    InnerElement.classList.remove("tss-fontcolor-invalid");
                    InnerElement.classList.remove("tss-fontcolor-success");
                    InnerElement.classList.remove("tss-fontcolor-danger");
                    InnerElement.classList.remove("tss-fontcolor-secondary");
                    InnerElement.classList.remove("tss-fontcolor-primary");
                }
            }
        }

        /// <summary>
        /// Gets or set whenever text block color is invalid
        /// </summary>
        public bool IsInvalid
        {
            get => InnerElement.classList.contains("tss-fontcolor-invalid");
            set
            {
                if (value)
                {
                    InnerElement.classList.add("tss-fontcolor-invalid");
                }
                else
                {
                    InnerElement.classList.remove("tss-fontcolor-invalid");
                }
            }
        }

        public virtual bool IsRequired
        {
            get => InnerElement.classList.contains("tss-required");
            set
            {
                if (value)
                {
                    InnerElement.classList.add("tss-required");
                }
                else
                {
                    InnerElement.classList.remove("tss-required");
                }
            }
        }

        public bool CanWrap
        {
            get => !InnerElement.classList.contains("tss-text-nowrap");
            set => InnerElement.UpdateClassIfNot(value, "tss-text-nowrap");
        }

        public bool EnableEllipsis
        {
            get => !InnerElement.classList.contains("tss-text-ellipsis");
            set => InnerElement.UpdateClassIf(value, "tss-text-ellipsis");
        }

        public string Cursor
        {
            get => InnerElement.style.cursor;
            set => InnerElement.style.cursor = value;
        }

        public override HTMLElement Render()
        {
            return InnerElement;
        }
    }

    public static class TextBlockExtensions
    {
        public static T Text<T>(this T textBlock, string text) where T : TextBlock
        {
            textBlock.Text = text;
            return textBlock;
        }

        public static T Title<T>(this T textBlock, string title) where T : TextBlock
        {
            textBlock.Title = title;
            return textBlock;
        }

        public static T Required<T>(this T textBlock) where T : TextBlock
        {
            textBlock.IsRequired = true;
            return textBlock;
        }

        public static T Wrap<T>(this T textBlock) where T : TextBlock
        {
            textBlock.CanWrap = true;
            return textBlock;
        }

        public static T Ellipsis<T>(this T textBlock) where T : TextBlock
        {
            textBlock.EnableEllipsis= true;
            return textBlock;
        }

        public static T NoWrap<T>(this T textBlock) where T : TextBlock
        {
            textBlock.CanWrap = false;
            return textBlock;
        }

        public static T Disabled<T>(this T textBlock) where T : TextBlock
        {
            textBlock.IsEnabled = false;
            return textBlock;
        }

        public static T NonSelectable<T>(this T textBlock) where T : TextBlock
        {
            textBlock.IsSelectable = false;
            return textBlock;
        }

        public static T Primary<T>(this T textBlock) where T : TextBlock
        {
            textBlock.IsPrimary = true;
            return textBlock;
        }

        public static T Success<T>(this T textBlock) where T : TextBlock
        {
            textBlock.IsSuccess = true;
            return textBlock;
        }

        public static T Danger<T>(this T textBlock) where T : TextBlock
        {
            textBlock.IsDanger = true;
            return textBlock;
        }

        public static T Secondary<T>(this T textBlock) where T : TextBlock
        {
            textBlock.IsSecondary = true;
            return textBlock;
        }
    }
}
