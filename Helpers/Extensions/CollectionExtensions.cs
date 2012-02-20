using System;
using System.Collections.Generic;
using System.Linq;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Extensions {
    public static class CollectionExtensions {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values) {
            Require.NotNull(collection, "collection");
            Require.NotNull(values, "values");

            var list = collection as List<T>;
            if (list != null) {
                list.AddRange(values);
            }
            else {
                var set = collection as HashSet<T>;
                if (set != null) {
                    set.UnionWith(values);
                }
                else {
                    values.ForEach(collection.Add);
                }
            }
        }

        public static void RemoveAll<T>(this ICollection<T> collection, IEnumerable<T> values) {
            Require.NotNull(collection, "collection");
            Require.NotNull(values, "values");

            values.ForEach(item => collection.Remove(item));
        }

        public static int RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> predicate) {
            Require.NotNull(collection, "collection");
            Require.NotNull(predicate, "predicate");

            var concreteList = collection as List<T>;
            if (concreteList != null)
                return concreteList.RemoveAll(new Predicate<T>(predicate));

            var virtualList = collection as IList<T>;
            if (virtualList != null)
                return ListExtensions.RemoveWhere(virtualList, predicate);

            var set = collection as HashSet<T>;
            if (set != null)
                return set.RemoveWhere(new Predicate<T>(predicate));

            var values = collection.Where(predicate).ToList();
            collection.RemoveAll(values);
            return values.Count;
        }

        public static int RemoveWhere<T>(this ICollection<T> collection, Func<T, int, bool> predicate) {
            Require.NotNull(collection, "collection");
            Require.NotNull(predicate, "predicate");

            var virtualList = collection as IList<T>;
            if (virtualList != null)
                return ListExtensions.RemoveWhere(virtualList, predicate);

            var values = collection.Where(predicate).ToList();
            collection.RemoveAll(values);
            return values.Count;
        }
    }
}
