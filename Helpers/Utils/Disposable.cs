using System;

using Azon.Helpers.Asserts;
using Azon.Helpers.Utils.Disposables;

namespace Azon.Helpers.Utils {
    public static class Disposable {
        public static IDisposable Create(Action dispose) {
            Require.NotNull(dispose, "dispose");
            return new AnonymousDisposable(dispose);
        }
    }
}
