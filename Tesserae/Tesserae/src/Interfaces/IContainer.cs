using System;
using System.Collections.Generic;

namespace Tesserae.Components
{
    public interface IContainer<T, TChild> : IComponent where T : IContainer<T, TChild> where TChild : IComponent
    {
        void Add(TChild component);
        void Clear();
        void Replace(TChild newComponent, TChild oldComponent);
    }

    public static class IContainerExtensions
    {
        public static T Children<T>(this T container, IEnumerable<IComponent> children) where T : IContainer<T, IComponent>
        {
            container.Clear();
            foreach (var x in children)
            {
                container.Add(x);
            }

            return container;
        }

        public static T Children<T>(this T container, params IComponent[] children) where T : IContainer<T, IComponent>
        {
            container.Clear();
            children.ForEach(x => container.Add(x));
            return container;
        }

        public static T Children<T>(this T container, IEnumerable<Nav.NavLink> children) where T : IContainer<T, IComponent>
        {
            container.Clear();
            foreach (var x in children)
            {
                container.Add(x);
            }

            return container;
        }

        public static T Children<T>(this T container, params Nav.NavLink[] children) where T : IContainer<T, Nav.NavLink>
        {
            container.Clear();
            children.ForEach(x => container.Add(x));
            return container;
        }
        
        public static T Children<T>(this T container, IEnumerable<ChoiceGroup.Choice> children) where T : IContainer<T, IComponent>
        {
            container.Clear();
            foreach (var x in children)
            {
                container.Add(x);
            }

            return container;
        }

        public static T Children<T>(this T container, params ChoiceGroup.Choice[] children) where T : IContainer<T, ChoiceGroup.Choice>
        {
            container.Clear();
            children.ForEach(x => container.Add(x));
            return container;
        }
    }
}
