using System;

namespace Azon.Helpers.Constructs.SwitchType {
    public interface ISwitchTypeWithResult<out T, TResult> {
        ISwitchTypeWithResult<T, TResult> When<TTry>(Func<TResult> func);

        ISwitchTypeWithResult<T, TResult> When<TTry>(Func<TTry, TResult> func);

        ISwitchTypeWithResult<T, TResult> When<TTry>(TResult result);

        TResult Otherwise(Func<TResult> func);

        TResult Otherwise(Func<T, TResult> func);

        TResult Otherwise(TResult result);

        TResult OtherwiseThrow(string message, params object[] args);

        TResult OtherwiseThrow<TException>(string message, params object[] args) where TException : Exception;
    }
}