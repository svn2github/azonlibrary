using System;
using System.Collections.Generic;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Comparers {
    public class PropertyBasedEqualityComparer<T> : IEqualityComparer<T> {
        private readonly Func<T, object> _selector;

        public PropertyBasedEqualityComparer(Func<T, object> selector) {
            Require.NotNull(selector, "selector");

            this._selector = selector;
        }

        public bool Equals(T x, T y) {
            return Equals(this._selector(x), this._selector(y));
        }

        public int GetHashCode(T obj) {
            var value = this._selector(obj);
            if (value == null)
                return 0;

            return value.GetHashCode();
        }
    }
}