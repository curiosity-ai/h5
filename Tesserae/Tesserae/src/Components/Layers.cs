using System;
using System.Collections.Generic;
using static H5.Core.dom;

namespace Tesserae.Components
{
    public static class Layers
    {
        private static int CurrentZIndex = 1000;
        private static HashSet<HTMLElement> CurrentLayers = new HashSet<HTMLElement>();
        public static string PushLayer(HTMLElement element)
        {
            if (CurrentLayers.Add(element))
            {
                CurrentZIndex += 10;
                return CurrentZIndex.ToString();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public static string AboveCurrent()
        {
            return (CurrentZIndex + 5).ToString();
        }

        public static void PopLayer(HTMLElement element)
        {
            if (CurrentLayers.Remove(element))
            {
                CurrentZIndex -= 10;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}