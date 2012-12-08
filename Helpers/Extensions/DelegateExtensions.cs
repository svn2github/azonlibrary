using System;
using System.Collections.Generic;

using Azon.Helpers.Comparers;

namespace Azon.Helpers.Extensions {
    public static class DelegateExtensions {
        public static TDelegate As<TDelegate>(this Delegate @delegate) {
            return (TDelegate)(object)Delegate.CreateDelegate(
                typeof(TDelegate),
                @delegate.Target, 
                @delegate.Method
            );
        }

        public static Comparison<T> AsComparison<T>(this Func<T, T, int> function) {
            return function.As<Comparison<T>>();
        }

        public static Func<T, T, int> AsFunction<T>(this Comparison<T> comparison) {
            return comparison.As<Func<T, T, int>>();
        }

        public static Func<T, bool> AsFunction<T>(this Predicate<T> predicate) {
            return predicate.As<Func<T, bool>>();
        }

        public static Predicate<T> AsPredicate<T>(this Func<T, bool> function) {
            return function.As<Predicate<T>>();
        }

        public static IComparer<T> ToComparer<T>(this Comparison<T> comparison) {
            return new DelegateBasedComparer<T>(comparison);
        }

        public static IComparer<T> ToComparer<T>(this Func<T, T, int> function) {
            return new DelegateBasedComparer<T>(function.AsComparison());
        }
    }
}
