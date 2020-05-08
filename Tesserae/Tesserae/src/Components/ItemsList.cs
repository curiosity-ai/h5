using System;
using System.Linq;
using System.Threading.Tasks;
using static H5.Core.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public class ItemsList: IComponent, ISpecialCaseStyling
    {
        private readonly Grid _grid;
        private readonly Stack _stack;
        private readonly UnitSize _maxStackItemSize;
        private readonly IDefer _defered;
        private Func<IComponent> _emptyListMessageGenerator;

        public ObservableList<IComponent> Items { get; }

        public HTMLElement StylingContainer    => ((DeferedComponent)_defered)._container;

        public bool PropagateToStackItemParent => true;

        public ItemsList(IComponent[] items, params UnitSize[] columns) : this(new ObservableList<IComponent>(items ?? new IComponent[0]), columns)
        {
        }

        public ItemsList(ObservableList<IComponent> items, params UnitSize[] columns)
        {
            Items = items ?? new ObservableList<IComponent>();

            if (columns.Length < 2)
            {
                _stack = Stack().Horizontal().Wrap().WidthStretch().MaxHeight(100.percent()).Scroll();
                _maxStackItemSize = columns.FirstOrDefault() ?? 100.percent();
            }
            else
            {
                _grid = Grid(columns).WidthStretch().MaxHeight(100.percent()).Scroll();
            }
            _emptyListMessageGenerator = null;

            _defered = Defer(Items, observedItems =>
            {
                if (!observedItems.Any())
                {
                    if (_emptyListMessageGenerator is object)
                    {
                        if(_grid is object)
                        {
                            return _grid.Children(_emptyListMessageGenerator().GridColumnStretch()).AsTask();
                        }
                        else
                        {
                            return _stack.Children(_emptyListMessageGenerator().WidthStretch().HeightStretch()).AsTask();
                        }
                    }
                    else
                    {
                        if(_grid is object)
                        {
                            _grid.Clear();
                            return _grid.AsTask();
                        }
                        else
                        {
                            _stack.Clear();
                            return _stack.AsTask();
                        }
                    }
                }
                else
                {
                    if(_grid is object)
                    {
                        return _grid.Children(Items.ToArray()).AsTask();
                    }
                    else
                    {
                        return _stack.Children(Items.Select(i => i.Width(_maxStackItemSize)).ToArray()).AsTask();
                    }
                }
            });
        }

        public ItemsList WithEmptyMessage(Func<IComponent> emptyListMessageGenerator)
        {
            _emptyListMessageGenerator = emptyListMessageGenerator ?? throw new ArgumentNullException(nameof(emptyListMessageGenerator));
            _defered.Refresh();
            return this;
        }

        public HTMLElement Render() => _defered.Render();
    }
}
