using H5;
using System;
using System.Threading.Tasks;
using Tesserae.HTML;
using static Tesserae.UI;
using static H5.dom;

namespace Tesserae.Components
{

    // This class has a different name than the static method in the UI class due to a bug in the bridge compiler that ends up calling the wrong constructor.
    // It is also internal to Tesserae to hide it from the compiler, which then exposes only teh IDefer interface
    internal class DeferedComponent : IDefer
    {
        private bool _needsRefresh;
        private double _refreshTimeout;
        private Func<Task<IComponent>> _asyncGenerator;
        private IComponent _loadMessage;
        internal HTMLElement _container;
        private int _delay = 1;

        private DeferedComponent()
        {
        }

        internal static DeferedComponent Create(Func<Task<IComponent>> asyncGenerator, IComponent loadMessage)
        {
            var d = new DeferedComponent();
            d._loadMessage = loadMessage ?? TextBlock("loading...").XSmall();
            d._asyncGenerator = asyncGenerator;
            d._needsRefresh = true;
            d._container = DIV(d._loadMessage.Render());
            d._container.id = "tss-deferred";
            return d;
        }
        internal static DeferedComponent Create(Func<Task<IComponent>> asyncGenerator) => Create(asyncGenerator, null);

        public void Refresh()
        {
            _needsRefresh = true;
            window.clearTimeout(_refreshTimeout);
            _refreshTimeout = window.setTimeout((t) => TriggerRefresh(), _delay > 0 ? _delay : 1);
        }

        public IDefer Debounce(int milliseconds)
        {
            _delay = milliseconds;
            return this;
        }

        public dom.HTMLElement Render()
        {
            DomObserver.WhenMounted(_container, () => TriggerRefresh());
            return _container;
        }

        internal dom.HTMLElement Container()
        {
            return _container;
        }

        private void TriggerRefresh()
        {
            if (!_needsRefresh) return;
            _needsRefresh = false;
            var container = ScrollBar.GetCorrectContainer(_container);
            //ClearChildren(container);
            //container.appendChild(_loadMessage.Render());
            var task = _asyncGenerator();
            task.ContinueWith(r =>
            {
                ClearChildren(container);
                if (r.IsCompleted)
                {
                    container.appendChild(r.Result.Render());
                }
                else
                {
                    container.appendChild(TextBlock("Error rendering async element").Danger());
                    container.appendChild(TextBlock(r.Exception.ToString()).XSmall());
                }
            }).FireAndForget();
        }

        internal static DeferedComponent Observe<T1>(IObservable<T1> o1, Func<T1, Task<IComponent>> asyncGenerator, IComponent loadMessage = null)
        {
            var d = Create(() => asyncGenerator(o1.Value), loadMessage);
            o1.onValueChanged += (v) => d.Refresh();
            return d;
        }

        internal static DeferedComponent Observe<T1, T2>(IObservable<T1> o1, IObservable<T2> o2, Func<T1, T2, Task<IComponent>> asyncGenerator, IComponent loadMessage = null)
        {
            var d = Create(() => asyncGenerator(o1.Value, o2.Value), loadMessage);
            o1.onValueChanged += (v) => d.Refresh();
            o2.onValueChanged += (v) => d.Refresh();
            return d;
        }

        internal static DeferedComponent Observe<T1, T2, T3>(IObservable<T1> o1, IObservable<T2> o2, IObservable<T3> o3, Func<T1, T2, T3, Task<IComponent>> asyncGenerator, IComponent loadMessage = null)
        {
            var d = Create(() => asyncGenerator(o1.Value, o2.Value, o3.Value), loadMessage);
            o1.onValueChanged += (v) => d.Refresh();
            o2.onValueChanged += (v) => d.Refresh();
            o3.onValueChanged += (v) => d.Refresh();
            return d;
        }

        internal static DeferedComponent Observe<T1, T2, T3, T4>(IObservable<T1> o1, IObservable<T2> o2, IObservable<T3> o3, IObservable<T4> o4, Func<T1, T2, T3, T4, Task<IComponent>> asyncGenerator, IComponent loadMessage = null)
        {
            var d = Create(() => asyncGenerator(o1.Value, o2.Value, o3.Value, o4.Value), loadMessage);
            o1.onValueChanged += (v) => d.Refresh();
            o2.onValueChanged += (v) => d.Refresh();
            o3.onValueChanged += (v) => d.Refresh();
            o4.onValueChanged += (v) => d.Refresh();
            return d;
        }

        internal static DeferedComponent Observe<T1, T2, T3, T4, T5>(IObservable<T1> o1, IObservable<T2> o2, IObservable<T3> o3, IObservable<T4> o4, IObservable<T5> o5, Func<T1, T2, T3, T4, T5, Task<IComponent>> asyncGenerator, IComponent loadMessage = null)
        {
            var d = Create(() => asyncGenerator(o1.Value, o2.Value, o3.Value, o4.Value, o5.Value), loadMessage);
            o1.onValueChanged += (v) => d.Refresh();
            o2.onValueChanged += (v) => d.Refresh();
            o3.onValueChanged += (v) => d.Refresh();
            o4.onValueChanged += (v) => d.Refresh();
            o5.onValueChanged += (v) => d.Refresh();
            return d;
        }

        internal static DeferedComponent Observe<T1, T2, T3, T4, T5, T6>(IObservable<T1> o1, IObservable<T2> o2, IObservable<T3> o3, IObservable<T4> o4, IObservable<T5> o5, IObservable<T6> o6, Func<T1, T2, T3, T4, T5, T6, Task<IComponent>> asyncGenerator, IComponent loadMessage = null)
        {
            var d = Create(() => asyncGenerator(o1.Value, o2.Value, o3.Value, o4.Value, o5.Value, o6.Value), loadMessage);
            o1.onValueChanged += (v) => d.Refresh();
            o2.onValueChanged += (v) => d.Refresh();
            o3.onValueChanged += (v) => d.Refresh();
            o4.onValueChanged += (v) => d.Refresh();
            o5.onValueChanged += (v) => d.Refresh();
            o6.onValueChanged += (v) => d.Refresh();
            return d;
        }

        internal static DeferedComponent Observe<T1, T2, T3, T4, T5, T6, T7>(IObservable<T1> o1, IObservable<T2> o2, IObservable<T3> o3, IObservable<T4> o4, IObservable<T5> o5, IObservable<T6> o6, IObservable<T7> o7, Func<T1, T2, T3, T4, T5, T6, T7, Task<IComponent>> asyncGenerator, IComponent loadMessage = null)
        {
            var d = Create(() => asyncGenerator(o1.Value, o2.Value, o3.Value, o4.Value, o5.Value, o6.Value, o7.Value), loadMessage);
            o1.onValueChanged += (v) => d.Refresh();
            o2.onValueChanged += (v) => d.Refresh();
            o3.onValueChanged += (v) => d.Refresh();
            o4.onValueChanged += (v) => d.Refresh();
            o5.onValueChanged += (v) => d.Refresh();
            o6.onValueChanged += (v) => d.Refresh();
            o7.onValueChanged += (v) => d.Refresh();
            return d;
        }

        internal static DeferedComponent Observe<T1, T2, T3, T4, T5, T6, T7, T8>(IObservable<T1> o1, IObservable<T2> o2, IObservable<T3> o3, IObservable<T4> o4, IObservable<T5> o5, IObservable<T6> o6, IObservable<T7> o7, IObservable<T8> o8, Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<IComponent>> asyncGenerator, IComponent loadMessage = null)
        {
            var d = Create(() => asyncGenerator(o1.Value, o2.Value, o3.Value, o4.Value, o5.Value, o6.Value, o7.Value, o8.Value), loadMessage);
            o1.onValueChanged += (v) => d.Refresh();
            o2.onValueChanged += (v) => d.Refresh();
            o3.onValueChanged += (v) => d.Refresh();
            o4.onValueChanged += (v) => d.Refresh();
            o5.onValueChanged += (v) => d.Refresh();
            o6.onValueChanged += (v) => d.Refresh();
            o7.onValueChanged += (v) => d.Refresh();
            o8.onValueChanged += (v) => d.Refresh();
            return d;
        }

        internal static DeferedComponent Observe<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IObservable<T1> o1, IObservable<T2> o2, IObservable<T3> o3, IObservable<T4> o4, IObservable<T5> o5, IObservable<T6> o6, IObservable<T7> o7, IObservable<T8> o8, IObservable<T9> o9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<IComponent>> asyncGenerator, IComponent loadMessage = null)
        {
            var d = Create(() => asyncGenerator(o1.Value, o2.Value, o3.Value, o4.Value, o5.Value, o6.Value, o7.Value, o8.Value, o9.Value), loadMessage);
            o1.onValueChanged += (v) => d.Refresh();
            o2.onValueChanged += (v) => d.Refresh();
            o3.onValueChanged += (v) => d.Refresh();
            o4.onValueChanged += (v) => d.Refresh();
            o5.onValueChanged += (v) => d.Refresh();
            o6.onValueChanged += (v) => d.Refresh();
            o7.onValueChanged += (v) => d.Refresh();
            o8.onValueChanged += (v) => d.Refresh();
            o9.onValueChanged += (v) => d.Refresh();
            return d;
        }

        internal static DeferedComponent Observe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IObservable<T1> o1, IObservable<T2> o2, IObservable<T3> o3, IObservable<T4> o4, IObservable<T5> o5, IObservable<T6> o6, IObservable<T7> o7, IObservable<T8> o8, IObservable<T9> o9, IObservable<T10> o10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<IComponent>> asyncGenerator, IComponent loadMessage = null)
        {
            var d = Create(() => asyncGenerator(o1.Value, o2.Value, o3.Value, o4.Value, o5.Value, o6.Value, o7.Value, o8.Value, o9.Value, o10.Value), loadMessage);
            o1.onValueChanged += (v) => d.Refresh();
            o2.onValueChanged += (v) => d.Refresh();
            o3.onValueChanged += (v) => d.Refresh();
            o4.onValueChanged += (v) => d.Refresh();
            o5.onValueChanged += (v) => d.Refresh();
            o6.onValueChanged += (v) => d.Refresh();
            o7.onValueChanged += (v) => d.Refresh();
            o8.onValueChanged += (v) => d.Refresh();
            o9.onValueChanged += (v) => d.Refresh();
            o10.onValueChanged += (v) => d.Refresh();
            return d;
        }
    }


    //Generator code:

    //var sb = new StringBuilder(); // For Defer.cs
    //var sb2 = new StringBuilder(); //For UI.Components.cs
    //for(int i = 1; i <= 10; i++)
    //{
    //	var t = string.Join(", ", Enumerable.Range(1, i).Select(a => $"T{a}"));
    //	var ot = string.Join(", ", Enumerable.Range(1, i).Select(a => $"IObservable<T{a}> o{a}"));
    //  var ot2 = string.Join(", ", Enumerable.Range(1, i).Select(a => $"o{a}"));
    //	var vt = string.Join(", ", Enumerable.Range(1, i).Select(a => $"o{a}.Value"));
    //  sb2.AppendLine($"public static Defer Defer<{t}>({ot}, Func<{t}, Task<IComponent>> asyncGenerator, IComponent loadMessage = null) => Components.Defer.Observe({ot2}, asyncGenerator, loadMessage);");
    //	sb.AppendLine($"internal  static Defer Observe<{t}>({ot}, Func<{t}, Task<IComponent>> asyncGenerator, IComponent loadMessage = null)");
    //	sb.AppendLine("{");
    //	sb.AppendLine($"    var d = Create(() => asyncGenerator({vt}), loadMessage);");
    //	for(int j = 1; j <= i; j++)
    //	{
    //		sb.AppendLine($"    o{j}.onValueChanged += ((v) => d.Refresh());");
    //	}
    //	sb.AppendLine("   return d;");
    //	sb.AppendLine("}").AppendLine();
    //}
    //Console.WriteLine(sb.ToString());
    //Console.WriteLine(sb2.ToString());
}