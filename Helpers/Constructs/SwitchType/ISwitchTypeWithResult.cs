using System;

namespace Azon.Helpers.Constructs.SwitchType {
    public interface ISwitchTypeWithResult<TResult> {
        /// <summary>
        /// Executes a given function if a type of the tested value matches to the given type.
        /// </summary>
        /// <typeparam name="TTry">A type to test the value against.</typeparam>
        /// <param name="func">A function to execute.</param>
        /// <returns>The current <see cref="ISwitchTypeWithResult{TResult}" /> instance.</returns>
        ISwitchTypeWithResult<TResult> When<TTry>(Func<TResult> func);

        /// <summary>
        /// Returns a given result if a type of the tested value matches to the given type.
        /// </summary>
        /// <typeparam name="TTry">A type to test the value against.</typeparam>
        /// <param name="result">A value to return.</param>
        /// <returns>The current <see cref="ISwitchTypeWithResult{TResult}" /> instance.</returns>
        ISwitchTypeWithResult<TResult> When<TTry>(TResult result);

        /// <summary>
        /// Executes a given function if a type of the tested value is constructed from the given generic type.
        /// </summary>
        /// <param name="type">An open generic type to test the value against.</param>
        /// <param name="func">A function to execute.</param>
        /// <returns>The current <see cref="ISwitchTypeWithResult{TResult}" /> instance.</returns>
        ISwitchTypeWithResult<TResult> WhenOpen(Type type, Func<Type[], TResult> func);

        /// <summary>
        /// Returns a given result if a type of the tested value is constructed from the given generic type.
        /// </summary>
        /// <param name="type">An open generic type to test the value against.</param>
        /// <param name="result">A value to return.</param>
        /// <returns>The current <see cref="ISwitchTypeWithResult{TResult}" /> instance.</returns>
        ISwitchTypeWithResult<TResult> WhenOpen(Type type, TResult result);

        /// <summary>
        /// Executes a given function if no cases matched.
        /// </summary>
        /// <param name="func">A function to execute.</param>
        /// <returns>The result produced by the matched case.</returns>
        TResult Otherwise(Func<TResult> func);

        /// <summary>
        /// Returns a given result if no cases matched.
        /// </summary>
        /// <param name="result">A value to return.</param>
        /// <returns>The result produced by the matched case.</returns>
        TResult Otherwise(TResult result);

        /// <summary>
        /// Throws <see cref="InvalidOperationException" /> if no cases matched.
        /// </summary>
        /// <param name="message">A message for the exception to throw.</param>
        /// <param name="args">Optional parameters to format a message with.</param>
        /// <returns>The result produced by the matched case.</returns>
        TResult OtherwiseThrow(string message, params object[] args);

        /// <summary>
        /// Throws an exception of a given type if no cases matched.
        /// </summary>
        /// <typeparam name="TException">A type of exception to throw.</typeparam>
        /// <param name="message">A message for the exception to throw.</param>
        /// <param name="args">Optional parameters to format a message with.</param>
        /// <returns>The result produced by the matched case.</returns>
        TResult OtherwiseThrow<TException>(string message, params object[] args) where TException : Exception;
    }

    public interface ISwitchTypeWithResult<out T, TResult> : ISwitchTypeWithResult<TResult> {
        /// <summary>
        /// Executes a given function if a type of the tested value matches to the given type.
        /// </summary>
        /// <typeparam name="TTry">A type to test the value against.</typeparam>
        /// <param name="func">A function to execute.</param>
        /// <returns>The current <see cref="ISwitchTypeWithResult{T,TResult}" /> instance.</returns>
        new ISwitchTypeWithResult<T, TResult> When<TTry>(Func<TResult> func);

        /// <summary>
        /// Executes a given function if a type of the tested value matches to the given type.
        /// </summary>
        /// <typeparam name="TTry">A type to test the value against.</typeparam>
        /// <param name="func">A function to execute.</param>
        /// <returns>The current <see cref="ISwitchTypeWithResult{T,TResult}" /> instance.</returns>
        ISwitchTypeWithResult<T, TResult> When<TTry>(Func<TTry, TResult> func);

        /// <summary>
        /// Returns a given result if a type of the tested value matches to the given type.
        /// </summary>
        /// <typeparam name="TTry">A type to test the value against.</typeparam>
        /// <param name="result">A value to return.</param>
        /// <returns>The current <see cref="ISwitchTypeWithResult{T,TResult}" /> instance.</returns>
        new ISwitchTypeWithResult<T, TResult> When<TTry>(TResult result);

        /// <summary>
        /// Executes a given function if a type of the tested value is constructed from the given generic type.
        /// </summary>
        /// <param name="type">An open generic type to test the value against.</param>
        /// <param name="func">A function to execute.</param>
        /// <returns>The current <see cref="ISwitchTypeWithResult{T,TResult}" /> instance.</returns>
        new ISwitchTypeWithResult<T, TResult> WhenOpen(Type type, Func<Type[], TResult> func);

        /// <summary>
        /// Returns a given result if a type of the tested value is constructed from the given generic type.
        /// </summary>
        /// <param name="type">An open generic type to test the value against.</param>
        /// <param name="result">A value to return.</param>
        /// <returns>The current <see cref="ISwitchTypeWithResult{T,TResult}" /> instance.</returns>
        new ISwitchTypeWithResult<T, TResult> WhenOpen(Type type, TResult result);

        /// <summary>
        /// Executes a given function if no cases matched.
        /// </summary>
        /// <param name="func">A function to execute.</param>
        /// <returns>The result produced by the matched case.</returns>
        TResult Otherwise(Func<T, TResult> func);
    }
}
