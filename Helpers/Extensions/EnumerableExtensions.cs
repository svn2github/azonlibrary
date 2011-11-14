using System;
using System.Collections.Generic;
using System.Linq;

using Azon.Helpers.Asserts;
using Azon.Helpers.Constructs;

namespace Azon.Helpers.Extensions {
    public static class EnumerableExtensions {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
            Require.NotNull(items, "items");
            Require.NotNull(action, "action");

            foreach (var item in items) {
                action(item);
            }
        }

        public static IEnumerable<T> Sort<T>(this IEnumerable<T> items, Func<T, T, bool> predicate) {
            Require.NotNull(items, "items");
            Require.NotNull(predicate, "predicate");

            Comparison<T> comparison = (left, right) => predicate(left, right) ? 1 : -1;

            return Switch.Type<IEnumerable<T>, IEnumerable<T>>(items)
                         .When<T[]>(array => {
                             Array.Sort(array, comparison);
                             return array;
                         })
                         .When<List<T>>(list => {
                             list.Sort(comparison);
                             return list;
                         })
                         .Otherwise(enumerable => {
                             var list = enumerable.ToList();
                             list.Sort(comparison);
                             return list;
                         });
        }
    }
}
