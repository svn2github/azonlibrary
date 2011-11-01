using System;

namespace Azon.Helpers.Constructs.SwitchType {
    public interface ISwitchType {
        ISwitchType When<TTry>(Action action);

        ISwitchType WhenGeneric(Type type, Action<Type[]> action);

        void Otherwise(Action action);

        void OtherwiseThrow(string message, params object[] args);

        void OtherwiseThrow<TException>(string message, params object[] args) where TException : Exception;
    }

    public interface ISwitchType<out T> : ISwitchType {
        new ISwitchType<T> When<TTry>(Action action);

        ISwitchType<T> When<TTry>(Action<TTry> action);

        new ISwitchType<T> WhenGeneric(Type type, Action<Type[]> action);

        void Otherwise(Action<T> action);
    }
}