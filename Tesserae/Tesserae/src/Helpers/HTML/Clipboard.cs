using static H5.Core.dom;
using static Tesserae.UI;

namespace Tesserae
{
    public static class Clipboard
    {
        public static void Copy(string valueToCopy, bool showMessage = true, string customMessage = null)
        {
            var ta = TextBox(_());
            ta.style.opacity = "0";
            ta.style.position = "absolute";
            document.body.appendChild(ta);

            try
            {
                var curEl = (HTMLElement)document.activeElement;
                ta.value = valueToCopy;
                ta.@select();
                document.execCommand("copy");

                if (curEl != null)
                {
                    curEl.focus();
                }
                if (showMessage)
                {
                    Toast().Success("", customMessage ?? $"📋 Copied\n{valueToCopy}");
                }
            }
            finally
            {
                document.body.removeChild(ta);
            }
        }
    }
}