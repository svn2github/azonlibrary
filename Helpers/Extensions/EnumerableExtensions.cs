using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Azon.Helpers.Asserts;

using Switch = Azon.Helpers.Constructs.Switch;

namespace Azon.Helpers.Extensions {
    [DebuggerStepThrough]
    public static class EnumerableExtensions {
        public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) {
            Require.NotNull(source, "source");
            Require.NotNull(source, "predicate");

            var index = 0;
            return source.Any(item => predicate(item, index++));
        }

        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> source, TSource item) {
            Require.NotNull(source, "source");

            foreach (var each in source) {
                yield return each;
            }
            yield return item;
        }

        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> source, TSource item) {
            Require.NotNull(source, "source");
            return source.Where(eachItem => !Object.Equals(eachItem, item));
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action) {
            source.ForEach((item, index) => action(item));
        }

        [DebuggerStepThrough]
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action) {
            Require.NotNull(source, "source");
            Require.NotNull(action, "action");

            var index = 0;
            foreach (var item in source) {
                action(item, index);
                index += 1;
            }
        }

        [DebuggerStepThrough]
        public static IEnumerable<TSource> HavingMax<TSource, TValue>(
            this IEnumerable<TSource> source,
            Func<TSource, TValue> selector
        ) {
            return source.HavingMaxOrMin(selector, 1);
        }

        [DebuggerStepThrough]
        public static IEnumerable<TSource> HavingMin<TSource, TValue>(
            this IEnumerable<TSource> source,
            Func<TSource, TValue> selector
        ) {
            return source.HavingMaxOrMin(selector, -1);
        }

        [DebuggerStepThrough]
        private static IEnumerable<TSource> HavingMaxOrMin<TSource, TValue>(
            this IEnumerable<TSource> source,
            Func<TSource, TValue> selector,
            int comparison
        ) {
            Require.NotNull(source, "source");
            Require.NotNull(selector, "selector");

            var selectedItems = new List<TSource>();
            var selectedValue = default(TValue);
            var selected = false;
            var comparer = Comparer<TValue>.Default;

            foreach (var item in source) {
                var compared = comparer.Compare(selector(item), selectedValue);
                
                if (!selected || compared == comparison) {
                    selectedItems = new List<TSource> { item };
                    selectedValue = selector(item);
                    selected = true;

                    continue;
                }

                if (compared == 0)
                    selectedItems.Add(item);
            }

            return selectedItems;
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

        [DebuggerStepThrough]
        public static IDictionary<TKey, TValue> ToDictionary<T, TKey, TValue>(
            this IEnumerable<T> source,
            Func<T, int, TKey> keySelector,
            Func<T, int, TValue> valueSelector
        ) {
            Require.NotNull(source, "source");
            Require.NotNull(keySelector, "keySelector");
            Require.NotNull(valueSelector, "valueSelector");

            var i = 0;
            return source.ToDictionary(
                item => keySelector(item, i),
                item => valueSelector(item, i++)
            );
        }

        [DebuggerStepThrough]
        public static HashSet<TSource> ToSet<TSource>(this IEnumerable<TSource> source) {
            Require.NotNull(source, "source");
            return new HashSet<TSource>(source);
        }

        [DebuggerStepThrough]
        public static HashSet<TSource> ToSet<TSource>(
            this IEnumerable<TSource> source,
            IEqualityComparer<TSource> comparer
        ) {
            Require.NotNull(source, "source");
            return new HashSet<TSource>(source, comparer);
        }
    }
}
