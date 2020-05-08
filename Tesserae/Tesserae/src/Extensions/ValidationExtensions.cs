using System;
using static H5.dom;

namespace Tesserae.Components
{
    public static class ValidationExtensions
    {
        public static T Validation<T>(this T component, Func<T, string> validate, Validator validator = null, Validation.Mode mode = Components.Validation.Mode.OnInput) where T : ICanValidate<T>
        {
            if (validate is null)
            {
                throw new ArgumentNullException(nameof(validate));
            }

            void handler(object sender, Event e)
            {
                var s = (T)sender;
                var msg = validate(s) ?? "";
                var isInvalid = !string.IsNullOrWhiteSpace(msg);
                s.Error = msg;
                s.IsInvalid = isInvalid;
                validator?.RaiseOnValidation();
            }

            component.Attach(handler, mode);

            handler(component, null);

            validator?.Register(component, () => handler(component, null));
            validator?.RaiseOnValidation();

            return component;
        }
    }
}