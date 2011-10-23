using System;

namespace Azon.Helpers.Generators {
    public static class Fx {
        public static Func<T, T> Fix<T>(Func<Func<T, T>, Func<T, T>> f) {
            return t => f(Fix(f))(t);
        }

        public static Action<T> Fix<T>(Func<Action<T>, Action<T>> f) {
            return t => f(Fix(f))(t);
        }

        public static Action Fix(Func<Action, Action> f) {
            return () => f(Fix(f))();
        }

        public static Action Recursive(Action action) {
            return Fix(self => () => {
                action();
                self();
            });
        }
    }
}