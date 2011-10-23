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

        public static void Block(Action action) {
            Block(action.Target, action);
        }

        public static void Block(object key, Action action) {
            using (var protector = _store.GetOrCreateValue(key)) {
                if (protector.IsOnLookout)
                    return;

                action();
            }
        }
    }
}
