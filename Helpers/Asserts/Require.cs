using System;

namespace Azon.Helpers.Asserts {
    public static class Require {
        public static void NotNull(object value, string message, params object[] args) {
            Require.NotNull<ArgumentNullException>(value, message);
        }

        public static void NotNull<T>(object value, string message, params object[] args) 
            where T : Exception
        {
            if (value == null)
                throw BuildException<T>(message, args);
        }

        public static void NotEmpty(string value, string message, params object[] args) {
            Require.NotNull(value, message, args);
            Require.NotEmpty<ArgumentException>(value, message, args);
        }

        public static void NotEmpty<T>(string value, string message, params object[] args)
            where T : Exception
        {
            Require.NotNull<T>(value, message, args);

            if (string.IsNullOrEmpty(value))
                throw BuildException<T>(message, args);
        }

        public static void That(bool condition, string message, params object[] args) {
            Require.That<ArgumentException>(condition, message, args);
        }

        public static void That<T>(bool condition, string message, params object[] args)
            where T : Exception
        {
            if (!condition)
                throw BuildException<T>(message, args);
        }

        private static T BuildException<T>(string message, params object[] args) {
            var formattedMessage = string.Format(message, args);
            var constructor = typeof(T).GetConstructor(new[] { typeof(string) });

            Require.NotNull(
                constructor, 
                "{0} doesn't have a public constructor matching ctor(string message).",
                typeof(T)
            );

            return (T)constructor.Invoke(new object[] { formattedMessage });
        }
    }
}
