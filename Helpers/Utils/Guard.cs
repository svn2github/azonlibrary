using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Azon.Helpers.Utils {
    /// <summary>
    /// A helper class that simplifies <see cref="StackOverflowException"/> prevention.
    /// </summary>
    public static class Guard {
        private class Protector : IDisposable {
            private int _isOnLookout;

            public bool IsOnLookout {
                get { return Interlocked.Exchange(ref this._isOnLookout, 1) == 1; }
            }

            public void Dispose() {
                Interlocked.Exchange(ref this._isOnLookout, 0);
            }
        }

        private static readonly ConditionalWeakTable<object, ConditionalWeakTable<object, Protector>> _store =
            new ConditionalWeakTable<object, ConditionalWeakTable<object, Protector>>();

        /// <summary>
        /// Prevents recursive reentrancy into a block of code.
        /// </summary>
        /// <remarks>
        /// Does not work with closures. Use method overload with an explicit key.
        /// </remarks>
        /// <param name="action">A block of code to guard.</param>
        public static void Block(Action action) {
            Guard.Block(action.Target, action.Method, action);
        }

        /// <summary>
        /// Executes a block of code on the sole condition, that there is no
        /// another block of code running with the same key down in the stack.
        /// </summary>
        /// <param name="key">A key object to identify a block of code.</param>
        /// <param name="action">A block of code to guard.</param>
        public static void Block(object key, Action action) {
            Guard.Block(key, key, action);
        }

        private static void Block(object objectKey, object methodKey, Action action) {
            var methods = _store.GetOrCreateValue(objectKey);
            var protector = methods.GetOrCreateValue(methodKey);

            using (protector) {
                if (protector.IsOnLookout)
                    return;

                action();
            }
        }

        #region Optimization shortcuts

        public static void Block<T>(Action<T> action, T arg1) {
            var methods = _store.GetOrCreateValue(action.Target);
            var protector = methods.GetOrCreateValue(action.Method);

            using (protector) {
                if (protector.IsOnLookout)
                    return;

                action(arg1);
            }
        }

        public static void Block<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2) {
            var methods = _store.GetOrCreateValue(action.Target);
            var protector = methods.GetOrCreateValue(action.Method);

            using (protector) {
                if (protector.IsOnLookout)
                    return;

                action(arg1, arg2);
            }
        }

        public static void Block<T1, T2, T3>(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3) {
            var methods = _store.GetOrCreateValue(action.Target);
            var protector = methods.GetOrCreateValue(action.Method);

            using (protector) {
                if (protector.IsOnLookout)
                    return;

                action(arg1, arg2, arg3);
            }
        }

        #endregion
    }
}
