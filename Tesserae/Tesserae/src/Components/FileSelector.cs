using H5;
using static H5.Core.dom;
using static Tesserae.UI;
using Tesserae;
using System;

namespace Tesserae.Components
{
    public class FileSelector : IComponent , ICanValidate<FileSelector>
    {
        private HTMLInputElement _fileInput;
        private IComponent _stack;
        private TextBox _textBox;
        private HTMLElement _container;
        private File _selectedFile;

        public delegate void FileSelectedHandler(FileSelector sender, File file);

        public event FileSelectedHandler onFileSelected;

        public File SelectedFile
        {
            get => _selectedFile;
            private set
            {
                _selectedFile = value;
                onFileSelected?.Invoke(this, value);
            }
        }

        public string Placeholder
        {
            get => _textBox.Placeholder;
            set => _textBox.Placeholder = value;
        }

        public string Error
        {
            get => _textBox.Error;
            set => _textBox.Error = value;
        }

        public bool IsInvalid
        {
            get => _textBox.IsInvalid;
            set => _textBox.IsInvalid = value;
        }

        public bool IsRequired
        {
            get => _textBox.IsRequired;
            set => _textBox.IsRequired = value;
        }


        /// <summary>
        /// Gets or sets the type of files accepted by this selector. See https://www.w3schools.com/tags/att_input_accept.asp for more information.
        /// Valid values are a list of extensions, like ".txt|.doc|.docx", of media type, such as  "audio/*|video/*|image/*", or a combination of both
        /// </summary>
        public string Accepts
        {
            get => _fileInput.accept;
            set => _fileInput.accept = value;
        }

        public FileSelector()
        {
            _fileInput = FileInput(_("tss-file-input"));
            _textBox = TextBox().ReadOnly().Grow(1).AlignCenter();
            _stack = Stack().Horizontal().WidthStretch()
                            .Children(_textBox,
                                      Button().SetTitle("Click to select file...").NoWrap().SetIcon("las la-folder-open").OnClick((s,e) => _fileInput.click()).NoBorder().NoBackground(),
                                      Raw(_fileInput));

            _fileInput.onchange = (e) => updateFile();

            _container = Div(_("tss-fileselector"), _stack.Render());
            void updateFile()
            {
                if (_fileInput.files.length > 0)
                {
                    SelectedFile = _fileInput.files[0];
                    _textBox.Text = GetFileName(_fileInput.value);
                }
            };
        }

        public FileSelector OnFileSelected(FileSelectedHandler handler)
        {
            onFileSelected += handler;
            return this;
        }

        public FileSelector SetPlaceholder(string placeholder)
        {
            Placeholder = placeholder;
            return this;
        }

        /// <summary>
        /// Sets the type of files accepted by this selector. See https://www.w3schools.com/tags/att_input_accept.asp for more information.
        /// Valid values are a list of extensions, like ".txt|.doc|.docx", of media type, such as  "audio/*|video/*|image/*", or a combination of both
        /// </summary>
        /// <param name="accepts"></param>
        /// <returns></returns>
        public FileSelector SetAccepts(string accepts)
        {
            Accepts = accepts;
            return this;
        }

        public FileSelector Required()
        {
            IsRequired = true;
            return this;
        }

        public void Attach(EventHandler<Event> handler, Validation.Mode mode)
        {
            onFileSelected += (s, e) => handler(s, null);
        }


        private string GetFileName(string value)
        {
            var lastSep = value.LastIndexOfAny(new[] { '/', '\\'});
            return value.Substring(lastSep + 1);
        }

        public HTMLElement Render()
        {
            return _container;
        }
    }
}
