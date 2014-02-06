using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

using Azon.Helpers.Annotations;
using Azon.Helpers.Asserts;

using Switch = Azon.Helpers.Constructs.Switch;

namespace Azon.Helpers.Extensions {
    [DebuggerStepThrough]
    public static class EnumerableExtensions {
        public static bool IsNullOrEmpty<TSource>([CanBeNull] this IEnumerable<TSource> source) {
            return source == null
                || !source.Any();
        }

        public static bool Any<TSource>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull, InstantHandle] Func<TSource, int, bool> predicate
        ) {
            Require.NotNull(source, "source");
            Require.NotNull(source, "predicate");

            var index = 0;
            return source.Any(item => predicate(item, index++));
        }

        [NotNull]
        public static IEnumerable<TSource> Concat<TSource>([NotNull] this IEnumerable<TSource> source, TSource item) {
            Require.NotNull(source, "source");

            foreach (var each in source) {
                yield return each;
            }
            yield return item;
        }

        [NotNull]
        public static IEnumerable<TSource> Except<TSource>([NotNull] this IEnumerable<TSource> source, TSource item) {
            Require.NotNull(source, "source");
            return source.Where(eachItem => !Object.Equals(eachItem, item));
        }

        [NotNull]
        public static IEnumerable<TSource> ForEach<TSource>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull, InstantHandle] Action<TSource> action
        ) {
            Require.NotNull(source, "source");
            Require.NotNull(action, "action");

            return source.ForEach((item, index) => action(item));
        }

        [NotNull]
        public static IEnumerable<TSource> ForEach<TSource>(
            [NotNull] this IEnumerable<TSource> source, 
            [NotNull, InstantHandle] Action<TSource, int> action
        ) {
            Require.NotNull(source, "source");
            Require.NotNull(action, "action");

            var index = 0;
            foreach (var item in source) {
                action(item, index);
                index += 1;
            }
            return source;
        }

        [NotNull]
        public static IEnumerable<TSource> HavingMax<TSource, TValue>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull, InstantHandle] Func<TSource, TValue> selector
        ) {
            return source.HavingMaxOrMin(selector, 1);
        }

        [NotNull]
        public static IEnumerable<TSource> HavingMin<TSource, TValue>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull, InstantHandle] Func<TSource, TValue> selector
        ) {
            return source.HavingMaxOrMin(selector, -1);
        }

        [NotNull]
        private static IEnumerable<TSource> HavingMaxOrMin<TSource, TValue>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull, InstantHandle] Func<TSource, TValue> selector,
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

        [NotNull]
        public static IEnumerable<T> Sort<T>(
            [NotNull] this IEnumerable<T> items, 
            [NotNull, InstantHandle] Func<T, T, bool> predicate
        ) {
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

        [NotNull]
        public static IDictionary<TKey, TValue> ToDictionary<T, TKey, TValue>(
            [NotNull] this IEnumerable<T> source,
            [NotNull, InstantHandle] Func<T, int, TKey> keySelector,
            [NotNull, InstantHandle] Func<T, int, TValue> valueSelector
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

        [NotNull]
        public static HashSet<TSource> ToSet<TSource>([NotNull] this IEnumerable<TSource> source) {
            Require.NotNull(source, "source");
            return new HashSet<TSource>(source);
        }

        [NotNull]
        public static HashSet<TSource> ToSet<TSource>(
            [NotNull] this IEnumerable<TSource> source,
            [CanBeNull] IEqualityComparer<TSource> comparer
        ) {
            Require.NotNull(source, "source");
            return new HashSet<TSource>(source, comparer);
        }

        [NotNull]
        public static IEnumerable<T> TakeAllButLast<T>([NotNull] this IEnumerable<T> source) {
            Require.NotNull(source, "source");

            var it = source.GetEnumerator();
            var isFirst = true;
            var item = default(T);

            while (it.MoveNext()) {
                if (!isFirst) 
                    yield return item;

                item = it.Current;
                isFirst = false;
            }
        }

        [NotNull]
        public static IEnumerable<T> TakeAllButLast<T>([NotNull] this IEnumerable<T> source, int count) {
            Require.NotNull(source, "source");

            var it = source.GetEnumerator();
            var queue = new Queue<T>();

            while (it.MoveNext()) {
                queue.Enqueue(it.Current);

                if (queue.Count > count)
                    yield return queue.Dequeue();
            }
        }
    }
}
