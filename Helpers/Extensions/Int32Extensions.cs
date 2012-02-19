using System;
using System.Linq;

namespace Azon.Helpers.Extensions {
    public static class Int32Extensions {
        public static bool IsBetween(this int value, int left, int right) {
            return value >= left && value <= right;
        }

        public static void Times(this int count, Action action) {
            Times(count, i => action());
        }

        public static void Times(this int count, Action<int> action) {
            Enumerable.Range(0, count).ForEach(action);
        }

        public static T[] Times<T>(this int count, Func<T> func) {
            return Times(count, i => func());
        }

        public static T[] Times<T>(this int count, Func<int, T> func) {
            return Enumerable.Range(0, count).Select(func).ToArray();
        }
    }
}
