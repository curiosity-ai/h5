using System;
using static H5.dom;

namespace Tesserae.Components
{
    public interface ICanValidate<T> : ICanValidate
    {
        void Attach(EventHandler<Event> handler, Validation.Mode mode);
    }

    public interface ICanValidate
    {
        string Error { get; set; }
        bool IsInvalid { get; set; }
    }
}
