using System.Collections.Generic;
using System.Linq;

namespace Tesserae.Components
{
    internal static class EnumerableExtensions
    {
        internal static List<List<T>> InGroupsOf<T>(
            this IEnumerable<T> source,
            int groupSize)
        {
            return source
                .Select((item, index) => new { Index = index, Item = item })
                .GroupBy(item => item.Index / groupSize)
                .Select(groupItem => groupItem.Select(item => item.Item).ToList())
                .ToList();
        }
    }
}