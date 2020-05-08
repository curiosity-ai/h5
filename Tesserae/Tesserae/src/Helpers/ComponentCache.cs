using System;
using System.Collections.Generic;
using System.Linq;
using static H5.Core.dom;

namespace Tesserae
{
    public class ComponentCache<TComponent> : ComponentCacheBase<TComponent> where TComponent : class
    {
        private readonly Func<(int Key, TComponent Component), HTMLElement> _createComponentExpression;

        private  List<(int Key, HTMLElement HtmlElement)> _componentCache;

        public ComponentCache(Func<(int Key, TComponent Component), HTMLElement> createComponentExpression)
        {
            _createComponentExpression = createComponentExpression ?? throw new ArgumentNullException(nameof(createComponentExpression));

            _componentCache = new List<(int Key, HTMLElement HtmlElement)>();
        }

        public ComponentCache<TComponent> AddComponents(IEnumerable<TComponent> components)
        {
            AddToComponents(components);

            return this;
        }

        public IEnumerable<HTMLElement> GetAllRenderedComponentsFromCache()
        {
            foreach (var componentAndKey in _componentsAndKeys)
            {
                var cachedComponent = _componentCache.SingleOrDefault(component => component.Key == componentAndKey.Key);

                if (cachedComponent.HtmlElement != null)
                {
                    yield return cachedComponent.HtmlElement;
                }
                else
                {
                    var htmlElement = _createComponentExpression(componentAndKey);

                    _componentCache.Add((componentAndKey.Key, htmlElement));

                    yield return htmlElement;
                }
            }
        }

        public ComponentCache<TComponent> SortComponents(Comparison<TComponent> comparison)
        {
            if (HasComponents)
            {
                _componentsAndKeys.Sort((componentAndKey, otherComponentAndKey) => comparison(componentAndKey.Component, otherComponentAndKey.Component));
            }

            return this;
        }

        public ComponentCache<TComponent> ReverseComponentOrder()
        {
            _componentsAndKeys.Reverse();

            return this;
        }

        public ComponentCache<TComponent> Clear()
        {
            _componentsAndKeys.Clear();
            _componentCache.Clear();

            return this;
        }

    }
}
