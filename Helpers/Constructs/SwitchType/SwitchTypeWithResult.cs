using System;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Constructs.SwitchType {
    internal class SwitchTypeWithResult<T, TResult> : SwitchTypeBase<T>, ISwitchTypeWithResult<T, TResult> {
        private TResult _result;

        public SwitchTypeWithResult(bool exactType, T value) : base(exactType, value) { }

        #region ISwitchTypeWithResult<TResult>

        ISwitchTypeWithResult<TResult> ISwitchTypeWithResult<TResult>.When<TTry>(Func<TResult> func) {
            return this.When<TTry>(func);
        }

        ISwitchTypeWithResult<TResult> ISwitchTypeWithResult<TResult>.When(Type type, Func<TResult> func) {
            return this.When(type, func);
        }

        ISwitchTypeWithResult<TResult> ISwitchTypeWithResult<TResult>.When<TTry>(TResult result) {
            return this.When<TTry>(result);
        }

        ISwitchTypeWithResult<TResult> ISwitchTypeWithResult<TResult>.When(Type type, TResult result) {
            return this.When(type, result);
        }

        ISwitchTypeWithResult<TResult> ISwitchTypeWithResult<TResult>.WhenOpen(Type type, Func<Type[], TResult> func) {
            return this.WhenOpen(type, func);
        }

        ISwitchTypeWithResult<TResult> ISwitchTypeWithResult<TResult>.WhenOpen(Type type, TResult result) {
            return this.WhenOpen(type, result);
        }

        #endregion

        public ISwitchTypeWithResult<T, TResult> When<TTry>(Func<TResult> func) {
            return this.When<TTry>(value => func());
        }

        public ISwitchTypeWithResult<T, TResult> When(Type type, Func<TResult> func) {
            return this.When(type, value => func());
        }

        public ISwitchTypeWithResult<T, TResult> When<TTry>(TResult result) {
            return this.When<TTry>(value => result);
        }

        public ISwitchTypeWithResult<T, TResult> When(Type type, TResult result) {
            return this.When(type, value => result);
        }
        
        public ISwitchTypeWithResult<T, TResult> When<TTry>(Func<TTry, TResult> func) {
            return this.When(typeof(TTry), value => func((TTry)value));
        }

        public ISwitchTypeWithResult<T, TResult> When(Type type, Func<object, TResult> func) {
            if (this.Matches(type))
                this._result = func(this.Value);

            return this;
        }

        public ISwitchTypeWithResult<T, TResult> WhenOpen(Type type, TResult result) {
            return this.WhenOpen(type, args => result);
        }

        public ISwitchTypeWithResult<T, TResult> WhenOpen(Type type, Func<Type[], TResult> func) {
            Type[] args;
            if (this.MatchesGeneric(type, out args))
                this._result = func(args);

            return this;
        }

        public TResult Otherwise(TResult result) {
            return this.Otherwise(value => result);
        }

        public TResult Otherwise(Func<TResult> func) {
            return this.Otherwise(value => func());
        }

        public TResult Otherwise(Func<T, TResult> func) {
            return this.Matched ? this._result
                                : func(this.Value);
        }

        public TResult OtherwiseThrow(string message, params object[] args) {
            return this.OtherwiseThrow<InvalidOperationException>(message, args);
        }

        public TResult OtherwiseThrow<TException>(string message, params object[] args)
            where TException : Exception
        {
            if (this.Matched)
                return this._result;

            throw Require.Exception<TException>(message, args);
        }
    }
}