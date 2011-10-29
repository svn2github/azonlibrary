using System;

namespace Azon.Helpers.Constructs.SwitchType {
    public interface ISwitchType<out T> {
        ISwitchType<T> When<TTry>(Action action);

        ISwitchType<T> When<TTry>(Action<TTry> action);

        void Otherwise(Action action);

        void Otherwise(Action<T> action);

        void OtherwiseThrow(string message, params object[] args);

        void OtherwiseThrow<TException>(string message, params object[] args) where TException : Exception;
    }
}