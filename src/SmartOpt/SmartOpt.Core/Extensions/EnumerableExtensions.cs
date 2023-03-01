using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartOpt.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> collection) =>
            collection.Where(t => t is not null);

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> collection, int count)
        {
            return collection
                .Reverse()
                .Take(count);
        }
    }
}
