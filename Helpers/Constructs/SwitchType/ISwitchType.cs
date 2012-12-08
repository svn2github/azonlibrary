using System;

namespace Azon.Helpers.Constructs.SwitchType {
    public interface ISwitchType {
        /// <summary>
        /// Executes a given action if a type of the tested value matches to the given type.
        /// </summary>
        /// <typeparam name="TTry">A type to test the value against.</typeparam>
        /// <param name="action">An action to execute.</param>
        /// <returns>The current <see cref="ISwitchType" /> instance.</returns>
        ISwitchType When<TTry>(Action action);

        /// <summary>
        /// Executes a given action if a type of the tested value matches to the given type.
        /// </summary>
        /// <param name="type">A type to test the value against.</param>
        /// /// <param name="action">An action to execute.</param>
        /// <returns>The current <see cref="ISwitchType" /> instance.</returns>
        ISwitchType When(Type type, Action action);

        /// <summary>
        /// Executes a given action if a type of the tested value is constructed from the given generic type.
        /// </summary>
        /// <param name="type">An open generic type to test the value against.</param>
        /// <param name="action">An action to execute.</param>
        /// <returns>The current <see cref="ISwitchType" /> instance.</returns>
        ISwitchType WhenOpen(Type type, Action<Type[]> action);

        /// <summary>
        /// Executes a given action if no cases matched.
        /// </summary>
        /// <param name="action">An action to execute.</param>
        void Otherwise(Action action);

        /// <summary>
        /// Throws <see cref="InvalidOperationException" /> if no cases matched.
        /// </summary>
        /// <param name="message">A message for the exception to throw.</param>
        /// <param name="args">Optional parameters to format a message with.</param>
        void OtherwiseThrow(string message, params object[] args);

        /// <summary>
        /// Throws an exception of a given type if no cases matched.
        /// </summary>
        /// <typeparam name="TException">A type of exception to throw.</typeparam>
        /// <param name="message">A message for the exception to throw.</param>
        /// <param name="args">Optional parameters to format a message with.</param>
        void OtherwiseThrow<TException>(string message, params object[] args) where TException : Exception;
    }

    public interface ISwitchType<out T> : ISwitchType {
        /// <summary>
        /// Executes a given action if a type of the tested value matches to the given type.
        /// </summary>
        /// <typeparam name="TTry">A type to test the value against.</typeparam>
        /// <param name="action">An action to execute.</param>
        /// <returns>The current <see cref="ISwitchType{T}" /> instance.</returns>
        new ISwitchType<T> When<TTry>(Action action);

        /// <summary>
        /// Executes a given action if a type of the tested value matches to the given type.
        /// </summary>
        /// <param name="type">A type to test the value against.</param>
        /// <param name="action">An action to execute.</param>
        /// <returns>The current <see cref="ISwitchType{T}" /> instance.</returns>
        new ISwitchType<T> When(Type type, Action action);

        /// <summary>
        /// Executes a given action if a type of the tested value matches to the given type.
        /// </summary>
        /// <typeparam name="TTry">A type to test the value against.</typeparam>
        /// <param name="action">An action to execute.</param>
        /// <returns>The current <see cref="ISwitchType{T}" /> instance.</returns>
        ISwitchType<T> When<TTry>(Action<TTry> action);

        /// <summary>
        /// Executes a given action if a type of the tested value is constructed from the given generic type.
        /// </summary>
        /// <param name="type">An open generic type to test the value against.</param>
        /// <param name="action">An action to execute.</param>
        /// <returns>The current <see cref="ISwitchType{T}" /> instance.</returns>
        new ISwitchType<T> WhenOpen(Type type, Action<Type[]> action);

        /// <summary>
        /// Executes a given action if no cases matched.
        /// </summary>
        /// <param name="action">An action to execute.</param>
        void Otherwise(Action<T> action);
    }
}