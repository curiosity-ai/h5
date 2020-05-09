using System.Collections.Generic;
using System.Linq;

namespace Tesserae
{
    public abstract class ComponentCacheBase<TComponent> where TComponent : class
    {
        protected readonly List<(int Key, TComponent Component)> _componentsAndKeys;

        protected ComponentCacheBase()
        {
            _componentsAndKeys = new List<(int Key, TComponent Component)>();
        }

        public int ComponentsCount => _componentsAndKeys.Count;

        public bool HasComponents  => _componentsAndKeys.Any();

        protected void AddToComponents(IEnumerable<TComponent> components)
        {
            var componentsCount = ComponentsCount;

            var componentsToAdd = components.Select((component, index) => new { component, key = index + componentsCount + 1 });

            foreach (var componentToAdd in componentsToAdd)
            {
                _componentsAndKeys.Add((componentToAdd.key, componentToAdd.component));
            }
        }
    }
}
