using System;
using System.Threading;

namespace Azon.Helpers.Utils.Disposables {
    public class AnonymousDisposable : IDisposable {
        private readonly Action _dispose;
        private int _isDisposed;

        public bool IsDisposed {
            get { return Interlocked.Exchange(ref this._isDisposed, 1) == 1; }
        }

        public AnonymousDisposable(Action dispose) {
            this._dispose = dispose;
        }

        public void Dispose() {
            if (!this.IsDisposed)
                this._dispose();
        }
    }
}