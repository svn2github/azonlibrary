using System;
using System.Collections.Generic;

namespace Azon.Helpers.Extensions {
    public static class ListExtensions {
        public static int RemoveWhere<T>(IList<T> list, Func<T, bool> predicate) {
            return RemoveWhere(list, (item, index) => predicate(item));
        }

        public static int RemoveWhere<T>(IList<T> list, Func<T, int, bool> predicate) {
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
