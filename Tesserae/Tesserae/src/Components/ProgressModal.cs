using System;
using static Tesserae.UI;

namespace Tesserae.Components
{

    public class ProgressModal
    {
        private Modal _modalHost;
        private Raw _titleHost;
        private Raw _messageHost;
        private Raw _progressHost;
        private Raw _footerHost;
        private ProgressIndicator _progressIndicator;
        private Spinner _spinner;
        private bool _isSpinner = true;

        public ProgressModal()
        {
            var x_TODO = Modal();
            x_TODO.Blocking();


            _titleHost   = Raw().WidthStretch();
            _messageHost = Raw().WidthStretch();
            _footerHost = Raw().WidthStretch();
            _progressHost = Raw();
            _spinner = Spinner().Large().Margin(8.px());
            _progressHost.Content(_spinner);
            _progressIndicator = ProgressIndicator();
            _isSpinner = true;
            _modalHost = Modal().Blocking().NoLightDismiss().HideCloseButton().CenterContent()
                                .Content(Stack()
                                               .AlignCenter()
                                               .WidthStretch()
                                               .Children(_titleHost, _progressHost, _messageHost, _footerHost));

        }

        public ProgressModal Show()
        {
            _modalHost.Show();
            return this;
        }

        public ProgressModal Hide()
        {
            _modalHost.Hide();
            return this;
        }

        public ProgressModal Message(string message)
        {
            _messageHost.Content(TextBlock(message));
            return this;
        }

        public ProgressModal Message(IComponent message)
        {
            _messageHost.Content(message);
            return this;
        }

        public ProgressModal Title(string title)
        {
            _titleHost.Content(TextBlock(title).SemiBold().Primary().PaddingTop(16.px()).PaddingBottom(8.px()));
            return this;
        }

        public ProgressModal Title(IComponent title)
        {
            _titleHost.Content(title);
            return this;
        }

        public ProgressModal Progress(float percent)
        {
            if (_isSpinner)
            {
                _progressHost.Content(_progressIndicator);
                _isSpinner = false;
            }
            _progressIndicator.Progress(percent);
            return this;
        }

        public ProgressModal Progress(int position, int total) => Progress(100f * position / total);

        public ProgressModal ProgressIndeterminated()
        {
            if (_isSpinner)
            {
                _progressHost.Content(_progressIndicator);
                _isSpinner = false;
            }
            _progressIndicator.Indeterminated();
            return this;
        }

        public ProgressModal ProgressSpin()
        {
            if (!_isSpinner)
            {
                _progressHost.Content(_spinner);
                _isSpinner = true;
            }
            return this;
        }

        public ProgressModal WithCancel(Action<Button> onCancel, Action<Button> btnCancel = null)
        {
            var button = Button().SetText("Cancel").SetIcon(LineAwesome.Times).Danger();
            btnCancel?.Invoke(button);
            button.OnClick((b, __) => onCancel(b));
            _footerHost.PaddingTop(16.px()).Content(button.AlignCenter());
            return this;
        }
    }
}
