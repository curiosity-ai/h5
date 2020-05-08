using System;
using static H5.dom;

namespace Tesserae.Components
{
    public interface IPickerItem
    {
        string Name { get; }

        bool IsSelected { get; set; }

        IComponent Render();
    }

    public static class IPickerItemExtensions
    {
        public static T SelectedIf<T>(this T This, bool shouldSelect) where T : IPickerItem
        {
            if (shouldSelect)
            {
                This.IsSelected = true;
            }
            return This;
        }
    }
}
