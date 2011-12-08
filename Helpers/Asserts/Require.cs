using System;
using System.Collections;
using System.Linq;

using Azon.Helpers.Constructs;

namespace Azon.Helpers.Asserts {
    /// <summary>
    /// Contains a set of methods to check that objects qualify certain conditions.
    /// </summary>
    public static class Require {
        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if tested object is null.
        /// </summary>
        /// <param name="value">An object to test.</param>
        /// <param name="parameterName">A name of parameter for the exception to throw.</param>
        public static void NotNull(object value, string parameterName) {
            Require.NotNull<ArgumentNullException>(value, parameterName);
        }

        /// <summary>
        /// Throws an exception of a given type if tested object is null.
        /// </summary>
        /// <typeparam name="T">A type of exception to throw.</typeparam>
        /// <param name="value">An object to test.</param>
        /// <param name="message">A message for the exception to throw.</param>
        /// <param name="args">Optional parameters to format a message with.</param>
        public static void NotNull<T>(object value, string message, params object[] args) 
            where T : Exception
        {
            if (value == null)
                Require.Exception<T>(message, args);
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if tested string is null.<para />
        /// Throws <see cref="ArgumentException"/> if tested string is empty.
        /// </summary>
        /// <param name="value">A string to test.</param>
        /// <param name="parameterName">A name of parameter for the exception to throw.</param>
        public static void NotEmpty(string value, string parameterName) {
            Require.NotNull(value, parameterName);
            Require.NotEmpty<ArgumentException>(value, "String should not be empty.", parameterName);
        }

        /// <summary>
        /// Throws an exception of a given type if tested string is null or empty.
        /// </summary>
        /// <typeparam name="T">A type of exception to throw.</typeparam>
        /// <param name="value">A string to test.</param>
        /// <param name="message">A message for the exception to throw.</param>
        /// <param name="args">Optional parameters to format a message with.</param>
        public static void NotEmpty<T>(string value, string message, params object[] args)
            where T : Exception
        {
            Require.NotNull<T>(value, message, args);

            if (string.IsNullOrEmpty(value))
                Require.Exception<T>(message, args);
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException" /> if a given enumeration is null.<para />
        /// Throws <see cref="ArgumentException" /> if a given enumeration has no elements.
        /// </summary>
        /// <param name="items">An enumeration to test.</param>
        /// <param name="parameterName">A name of parameter for the exception to throw.</param>
        public static void NotEmpty(IEnumerable items, string parameterName) {
            Require.NotNull(items, parameterName);
            Require.NotEmpty<ArgumentException>(items, "Sequence should contain elements.", parameterName);
        }

        /// <summary>
        /// Throws an exception of a given type if a given enumeration is null or has no elements.
        /// </summary>
        /// <typeparam name="T">A type of exception to throw.</typeparam>
        /// <param name="items">An enumeration to test.</param>
        /// <param name="message">A message for the exception to throw.</param>
        /// <param name="args">Optional parameters to format a message with.</param>
        public static void NotEmpty<T>(IEnumerable items, string message, params object[] args) 
            where T : Exception 
        {
            Require.NotNull<T>(items, message, args);

            var hasItems = 
                Switch.Type<IEnumerable, bool>(items)
                    .When<ICollection>(collection => collection.Count > 0)
                    .Otherwise(enumerable => enumerable.Cast<object>().Any());

            if (!hasItems)
                Require.Exception<T>(message, args);
        }

        /// <summary>
        /// Throws <see cref="ArgumentException"/> if tested condition fails.
        /// </summary>
        /// <param name="condition">A condition to test.</param>
        /// <param name="message">A message for exception to throw.</param>
        /// <param name="args">Optional parameters to format a message with.</param>
        public static void That(bool condition, string message, params object[] args) {
            Require.That<ArgumentException>(condition, message, args);
        }

        /// <summary>
        /// Throws an exception of a given type if tested condition fails.
        /// </summary>
        /// <typeparam name="T">A type of exception to throw.</typeparam>
        /// <param name="condition">A condition to test.</param>
        /// <param name="message">A message for exception to throw.</param>
        /// <param name="args">Optional parameters to format a message with.</param>
        public static void That<T>(bool condition, string message, params object[] args)
            where T : Exception
        {
            if (!condition)
                Require.Exception<T>(message, args);
        }

        public static T Exception<T>(string message, params object[] args)
            where T : Exception
        {
            throw Switch.Type<T, Exception>(exactType: true)
                        .When<ArgumentException>(() => BuildArgumentException(message, args))
                        .Otherwise(() => BuildExceptionFallback<T>(message, args));
        }

        private static ArgumentException BuildArgumentException(string message, params object[] args) {
            var formattedMessage = string.Format(message, args);
            var parameterName = args.Length == 0 ? null : (string)args[0];

            return new ArgumentException(formattedMessage, parameterName);
        }

        private static T BuildExceptionFallback<T>(string message, params object[] args)
            where T : Exception
        {
            var formattedMessage = string.Format(message, args);
            var constructor = typeof(T).GetConstructor(new[] { typeof(string) });

            Require.NotNull<InvalidOperationException>(
                constructor,
                "{0} doesn't have a public constructor matching ctor(string message).",
                typeof(T)
            );

            return (T)constructor.Invoke(new object[] { formattedMessage });
        }
    }
}
