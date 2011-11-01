using System;

namespace Azon.Helpers.Constructs.SwitchType {
    public interface ISwitchTypeWithResult<TResult> {
        ISwitchTypeWithResult<TResult> When<TTry>(Func<TResult> func);

        ISwitchTypeWithResult<TResult> When<TTry>(TResult result);

        TResult Otherwise(Func<TResult> func);

        TResult Otherwise(TResult result);

        TResult OtherwiseThrow(string message, params object[] args);

        TResult OtherwiseThrow<TException>(string message, params object[] args) where TException : Exception;
    }

    public interface ISwitchTypeWithResult<out T, TResult> : ISwitchTypeWithResult<TResult> {
        new ISwitchTypeWithResult<T, TResult> When<TTry>(Func<TResult> func);

        ISwitchTypeWithResult<T, TResult> When<TTry>(Func<TTry, TResult> func);

        new ISwitchTypeWithResult<T, TResult> When<TTry>(TResult result);

        TResult Otherwise(Func<T, TResult> func);
    }
}