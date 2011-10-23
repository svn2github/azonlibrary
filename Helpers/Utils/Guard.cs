using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Azon.Helpers.Utils {
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

        private static readonly ConditionalWeakTable<object, Protector> _store = new ConditionalWeakTable<object, Protector>();

        /// <summary>
        /// Prevents recursive reentrancy into a block of code.
        /// </summary>
        /// <param name="action">Block of code to guard.</param>
        public static void Block(Action action) {
            Guard.Block(action.Target, action);
        }

        /// <summary>
        /// Executes a block of code on the sole condition, that there is no<para />
        /// another block of code running with the same key down in the stack.
        /// </summary>
        /// <param name="key">Key object to identify a block of code.</param>
        /// <param name="action">Block of code to guard.</param>
        public static void Block(object key, Action action) {
            using (var protector = _store.GetOrCreateValue(key)) {
                if (protector.IsOnLookout)
                    return;

                action();
            }
        }
    }
}
