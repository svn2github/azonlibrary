using System;

using Azon.Helpers.Asserts;
using Azon.Helpers.Utils.Disposables;

namespace Azon.Helpers.Utils {
    public static class Disposable {
        public static IDisposable Create(Action dispose) {
            Require.NotNull(dispose, "dispose");
            return new AnonymousDisposable(dispose);
        }

#if !SILVERLIGHT
        /// <summary>
        /// Creates an instance of <see cref="IDisposable"/> from a given action, 
        /// providing the information regarding whether the unwound <see langword="finally" />
        /// block is called after an unhandled exception has occurred to that action.
        /// </summary>
        /// <param name="dispose">The action with a parameter indicating whether a unhandled exception has occured.</param>
        /// <returns>The constructed instance o</returns>
        public static IDisposable Create(Action<bool> dispose) {
            Require.NotNull(dispose, "dispose");
            return new ExceptionAwareAnonymousDisposable(dispose);
        }
#endif
        
        public static IDisposable Create(IDisposable inner, Action<IDisposable> dispose) {
            Require.NotNull(dispose, "dispose");
            return new WrappingAnonymouseDisposable(inner, dispose);
        }
    }
}
