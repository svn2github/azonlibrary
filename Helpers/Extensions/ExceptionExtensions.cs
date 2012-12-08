using System;
using System.Reflection;

namespace Azon.Helpers.Extensions {
    public static class ExceptionExtensions {
        private static readonly MethodInfo _prepareForRethrowMethod;

        static ExceptionExtensions() {
            _prepareForRethrowMethod = typeof(Exception).GetMethod(
                "PrepForRemoting", 
                BindingFlags.NonPublic | BindingFlags.Instance
            );
        }

        /// <summary>
        /// Preserves the stack trace on a given exception.
        /// </summary>
        public static Exception Rethrow(this Exception exception) {
            _prepareForRethrowMethod.Invoke(exception, new object[0]);
            return exception;
        }
    }
}
