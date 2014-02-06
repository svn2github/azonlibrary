using System;
using System.Linq;

using Azon.Helpers.Annotations;
using Azon.Helpers.Asserts;

namespace Azon.Helpers.Extensions {
    public static class Int32Extensions {
        public static bool IsBetween(this int value, int left, int right) {
            return value >= left && value <= right;
        }

        public static void Times(this int count, [NotNull, InstantHandle] Action action) {
            Require.NotNull(action, "action");
            Times(count, i => action());
        }

        public static void Times(this int count, [NotNull, InstantHandle] Action<int> action) {
            Require.NotNull(action, "action");
            Enumerable.Range(0, count).ForEach(action);
        }

        [NotNull]
        public static T[] Times<T>(this int count, [NotNull, InstantHandle] Func<T> func) {
            Require.NotNull(func, "func");
            return Times(count, i => func());
        }

        [NotNull]
        public static T[] Times<T>(this int count, [NotNull, InstantHandle] Func<int, T> func) {
            Require.NotNull(func, "func");
            return Enumerable.Range(0, count).Select(func).ToArray();
        }
    }
}
