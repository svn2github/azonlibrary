using System;
using System.Collections.Generic;

using Azon.Helpers.Annotations;
using Azon.Helpers.Comparers;

namespace Azon.Helpers.Extensions {
    public static class DelegateExtensions {
        [NotNull]
        public static TDelegate As<TDelegate>([NotNull] this Delegate @delegate) {
            return (TDelegate)(object)Delegate.CreateDelegate(
                typeof(TDelegate),
                @delegate.Target, 
                @delegate.Method
            );
        }

        [NotNull]
        public static Comparison<T> AsComparison<T>([NotNull] this Func<T, T, int> function) {
            return function.As<Comparison<T>>();
        }

        [NotNull]
        public static Func<T, T, int> AsFunction<T>([NotNull] this Comparison<T> comparison) {
            return comparison.As<Func<T, T, int>>();
        }

        [NotNull]
        public static Func<T, bool> AsFunction<T>([NotNull] this Predicate<T> predicate) {
            return predicate.As<Func<T, bool>>();
        }

        [NotNull]
        public static Predicate<T> AsPredicate<T>([NotNull] this Func<T, bool> function) {
            return function.As<Predicate<T>>();
        }

        [NotNull]
        public static IComparer<T> ToComparer<T>([NotNull] this Comparison<T> comparison) {
            return new DelegateBasedComparer<T>(comparison);
        }

        [NotNull]
        public static IComparer<T> ToComparer<T>([NotNull] this Func<T, T, int> function) {
            return new DelegateBasedComparer<T>(function.AsComparison());
        }
    }
}
