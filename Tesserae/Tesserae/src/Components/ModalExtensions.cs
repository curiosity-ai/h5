namespace Tesserae.Components
{
    public static class ModalExtensions
    {
        public static Modal ShowCloseButton<T>(this T modal) where T : Modal
        {
            modal.ShowCloseButton = true;
            return modal;
        }

        public static Modal HideCloseButton<T>(this T modal) where T : Modal
        {
            modal.ShowCloseButton = false;
            return modal;
        }

        public static Modal LightDismiss<T>(this T modal) where T : Modal
        {
            modal.CanLightDismiss = true;
            return modal;
        }

        public static Modal NoLightDismiss<T>(this T modal) where T : Modal
        {
            modal.CanLightDismiss = false;
            return modal;
        }

        public static Modal Dark<T>(this T modal) where T : Modal
        {
            modal.IsDark = true;
            return modal;
        }

        public static Modal Draggable<T>(this T modal) where T : Modal
        {
            modal.IsDraggable = true;
            return modal;
        }

        public static Modal NonBlocking<T>(this T modal) where T : Modal
        {
            modal.IsNonBlocking = true;
            return modal;
        }

        public static Modal Blocking<T>(this T modal) where T : Modal
        {
            modal.IsNonBlocking = false;
            return modal;
        }
    }
}