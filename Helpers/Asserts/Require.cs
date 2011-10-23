using System;

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
                throw BuildException<T>(message, args);
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if tested string is null.<para />
        /// Throws <see cref="ArithmeticException"/> if tested string is empty.
        /// </summary>
        /// <param name="value">A string to test.</param>
        /// <param name="parameterName">A name of parameter for the exception to throw.</param>
        public static void NotEmpty(string value, string parameterName) {
            Require.NotNull(value, parameterName);
            Require.NotEmpty<ArgumentException>(value, parameterName);
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
                throw BuildException<T>(message, args);
        }

        /// <summary>
        /// Throws <see cref="ArgumentException"/> if tested condition fails.
        /// </summary>
        /// <param name="condition">A condition to test.</param>
        /// <param name="message">A message for the exception to throw.</param>
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
                throw BuildException<T>(message, args);
        }

        private static T BuildException<T>(string message, params object[] args) {
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
