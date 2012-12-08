using System;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Constructs.SwitchType {
    internal class SwitchType<T> : SwitchTypeBase<T>, ISwitchType<T> {
        public SwitchType(bool exactType, T value) : base(exactType, value) { }

        public ISwitchType<T> When<TTry>(Action action) {
            return When<TTry>(value => action());
        }

        public ISwitchType<T> When(Type type, Action action) {
            return When(type, value => action());
        }

        #region ISwitchType

        ISwitchType ISwitchType.When<TTry>(Action action) {
            return this.When<TTry>(action);
        }

        ISwitchType ISwitchType.When(Type type, Action action) {
            return this.When(type, action);
        }

        ISwitchType ISwitchType.WhenOpen(Type type, Action<Type[]> action) {
            return this.WhenOpen(type, action);
        }

        #endregion

        public ISwitchType<T> When<TTry>(Action<TTry> action) {
            this.When(typeof(TTry), x => action((TTry)x));
            return this;
        }

        public ISwitchType<T> When(Type tryType, Action<object> action) {
            if (this.Matches(tryType))
                action(this.Value);

            return this;
        }

        public ISwitchType<T> WhenOpen(Type type, Action<Type[]> action) {
            Type[] args;
            if (this.MatchesGeneric(type, out args))
                action(args);

            return this;
        }

        public void Otherwise(Action action) {
            this.Otherwise(value => action());
        }

        public void Otherwise(Action<T> action) {
            if (this.Matched)
                return;

            action(this.Value);
        }

        public void OtherwiseThrow(string message, params object[] args) {
            this.OtherwiseThrow<InvalidOperationException>(message, args);
        }

        public void OtherwiseThrow<TException>(string message, params object[] args) 
            where TException : Exception
        {
            if (this.Matched)
                return;

            throw Require.Exception<TException>(message, args);
        }
    }
}