using System;
using System.Collections.Generic;

using Azon.Helpers.Annotations;
using Azon.Helpers.Asserts;

namespace Azon.Helpers.Extensions {
    public static class ListExtensions {
        public static int RemoveWhere<T>(
            [NotNull] IList<T> list, 
            [NotNull, InstantHandle] Func<T, bool> predicate
        ) {
            Require.NotNull(list, "list");
            Require.NotNull(predicate, "predicate");

            return RemoveWhere(list, (item, index) => predicate(item));
        }

        public static int RemoveWhere<T>(
            [NotNull] IList<T> list, 
            [NotNull, InstantHandle] Func<T, int, bool> predicate
        ) {
            Require.NotNull(list, "list");
            Require.NotNull(predicate, "predicate");

            var num = 0;
            for (var i = list.Count - 1; i >= 0; i--) {
                if (!predicate(list[i], i))
                    continue;

                list.RemoveAt(i);
                num++;
            }
            return num;
        }
    }
}
