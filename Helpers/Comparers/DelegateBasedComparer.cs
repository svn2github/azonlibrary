using System;
using System.Collections.Generic;

namespace Azon.Helpers.Comparers {
    public class DelegateBasedComparer<T> : IComparer<T> {
        private readonly Comparison<T> _comparison;

        public DelegateBasedComparer(Comparison<T> comparison) {
            this._comparison = comparison;
        }

        public int Compare(T x, T y) {
            return this._comparison(x, y);
        }
    }
}
