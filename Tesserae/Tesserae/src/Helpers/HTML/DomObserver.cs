using System;
using System.Collections.Generic;
using System.Linq;
using static H5.Core.dom;

namespace Tesserae.HTML
{
    public static class DomObserver
    {
        private static List<(HTMLElement element, Action callback)> _elementsToTrackMountingOf;
        private static List<(HTMLElement element, Action callback)> _elementsToTrackRemovalOf;

        static DomObserver()
        {
            _elementsToTrackMountingOf = new List<(HTMLElement, Action)>();
            _elementsToTrackRemovalOf = new List<(HTMLElement, Action)>();

            var observer = new MutationObserver((mutationRecords, _) =>
            {
                CheckMounted(mutationRecords);
                CheckUnmounted(mutationRecords);

            });
            observer.observe(document.body, new MutationObserverInit { childList = true, subtree = true });
        }

        private static void CheckMounted(MutationRecord[] mutationRecords)
        {
            if (_elementsToTrackMountingOf.Count == 0)
                return;

            var elementsMountedThatWeCareAbout = new List<(HTMLElement element, Action callback)>();
            foreach (var mutationRecord in mutationRecords)
            {
                foreach (var mountedElement in mutationRecord.addedNodes)
                {
                    foreach (var elementToTrackMountingOf in _elementsToTrackMountingOf)
                    {
                        if (IsEqualToOrIsChildOf(elementToTrackMountingOf.element, mountedElement))
                            elementsMountedThatWeCareAbout.Add(elementToTrackMountingOf);
                    }
                }
            }
            if (elementsMountedThatWeCareAbout.Count == 0)
                return;

            _elementsToTrackMountingOf = _elementsToTrackMountingOf.Except(elementsMountedThatWeCareAbout).ToList();
            
            window.requestAnimationFrame(_ =>
            {
                foreach (var callbackToMake in elementsMountedThatWeCareAbout.Select(entry => entry.callback))
                {
                    callbackToMake();
                }
            });
        }

        private static void CheckUnmounted(MutationRecord[] mutationRecords)
        {
            if (_elementsToTrackRemovalOf.Count == 0)
                return;

            var elementsRemovedThatWeCareAbout = new List<(HTMLElement element, Action callback)>();
            foreach (var mutationRecord in mutationRecords)
            {
                foreach (var removedElement in mutationRecord.removedNodes)
                {
                    // 2019-10-28 DWR: The intent behind the NotifyWhenRemoved method is to fire a callback when an element is removed from the document, so that any related tidy-up / disposal
                    // may be performed. However, this will also be fired if an element (or one of its ancestors) is RE-rendered somewhere and that's not really what we want, so if the element
                    // that has been identified as being "removed" is actually still part of a branch that reaches back up to the html element then don't consider it removed.
                    var highestAncestorElementIfAny = removedElement.parentElement;
                    while (highestAncestorElementIfAny?.parentElement != null)
                        highestAncestorElementIfAny = highestAncestorElementIfAny.parentElement;
                    if ((highestAncestorElementIfAny != null) && highestAncestorElementIfAny.tagName.Equals("HTML", StringComparison.OrdinalIgnoreCase))
                        continue;

                    foreach (var elementToTrackRemovalOf in _elementsToTrackRemovalOf)
                    {
                        if (IsEqualToOrIsChildOf(elementToTrackRemovalOf.element, removedElement))
                            elementsRemovedThatWeCareAbout.Add(elementToTrackRemovalOf);
                    }
                }
            }
            if (elementsRemovedThatWeCareAbout.Count == 0)
                return;

            _elementsToTrackRemovalOf = _elementsToTrackRemovalOf.Except(elementsRemovedThatWeCareAbout).ToList();
            window.requestAnimationFrame(_ =>
            {
                foreach (var callbackToMake in elementsRemovedThatWeCareAbout.Select(entry => entry.callback))
                {
                    callbackToMake();
                }
            });
        }

        /// <summary>
        /// Some rendering libraries don't support rendering to a container until that container is mounted but the way that we commonly write components is to return an element that the caller will
        /// mount, which is a problem for componentizing those libraries. One workaround is to postpone the initialization until the element is mounted, which is made possible by this method. It
        /// will execute the specified action when the element is added to the document body. While there is at least one element being tracked in this manner, there is a marginal cost as all
        /// DOM manipulations will be tracked and any added elements will be checked (and all of their child elements checked) to see if they match one of the elements that we're interested
        /// in. The cost should be negligible but if there is a process that is going to make large and frequent updates to the DOM then it may be better to avoid having any elements in
        /// the notify-when-mounted list.
        /// </summary>
        public static void WhenMounted(HTMLElement element, Action callback)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (IsEqualToOrIsChildOf(element, document.body))
            {
                //Already mounted
                callback();
            }
            else
            {
                _elementsToTrackMountingOf.Add((element, callback));
            }
        }

        /// <summary>
        /// When there is some relating tidying up that must be done when a component is removed from the DOM, this method may be used to enable that - it will execute the specified action when
        /// the element is removed. While there is at least one element being tracked in this manner, there is a marginal cost as all DOM manipulations will be tracked and any removed elements
        /// will be checked (and all of their child elements checked) to see if they match one of the elements that we're interested in. The cost should be negligible but if there is a process
        /// that is going to make large and frequent updates to the DOM then it may be better to avoid having any elements in the notify-when-removed list.
        /// </summary>
        public static void WhenRemoved(HTMLElement element, Action callback)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            _elementsToTrackRemovalOf.Add((element, callback));
        }

        public static bool IsEqualToOrIsChildOf(HTMLElement element, Node possibleParentElement)
        {
            while (element != null)
            {
                if (element == possibleParentElement)
                {
                    return true;
                }
                element = element.parentElement;
            }
            return false;
        }
    }
}
