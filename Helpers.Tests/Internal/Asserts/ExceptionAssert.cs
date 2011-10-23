using System;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Internal.Asserts {
    public static class ExceptionAssert {
        public static void Throws(Action action) {
            ExceptionAssert.Throws<Exception>(action);
        }

        public static void Throws(Action action, string message) {
            ExceptionAssert.Throws<Exception>(action, message);
        }

        public static void Throws(Action action, Action<Exception> asserter) {
            ExceptionAssert.Throws<Exception>(action, asserter);
        }

        public static void Throws<T>(Action action)
            where T : Exception 
        {
            ExceptionAssert.Throws<T>(action, (Action<Exception>)null);
        }

        public static void Throws<T>(Action action, string message)
            where T : Exception 
        {
            ExceptionAssert.Throws<T>(
                action,
                ex => Assert.AreEqual(message, ex.Message)
            );
        }

        public static void Throws<T>(Action action, Action<T> asserter)
            where T : Exception 
        {
            var ex = Assert.Throws<T>(() => action());

            if (asserter != null)
                asserter(ex);
        }

        public static void DoesNotThrow(Action action) {
            Assert.DoesNotThrow(() => action());
        }
    }
}
