using System;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Constructs.SwitchType {
    internal class SwitchTypeWithResult<T, TResult> : SwitchTypeBase<T>, ISwitchTypeWithResult<T, TResult> {
        private TResult _result;

        public SwitchTypeWithResult(bool exactType, T value) : base(exactType, value) { }

        public ISwitchTypeWithResult<T, TResult> When<TTry>(Func<TResult> func) {
            return this.When<TTry>(value => func());
        }

        public ISwitchTypeWithResult<T, TResult> When<TTry>(Func<TTry, TResult> func) {
            if (this.Matches<TTry>())
                this._result = func((TTry)(object)this.Value);

            return this;
        }

        public ISwitchTypeWithResult<T, TResult> When<TTry>(TResult result) {
            if (this.Matches<TTry>())
                this._result = result;

            return this;
        }

        public TResult Otherwise(Func<TResult> func) {
            return this.Otherwise(value => func());
        }

        public TResult Otherwise(Func<T, TResult> func) {
            return this.Matched ? this._result
                                : func(this.Value);
        }

        public TResult Otherwise(TResult result) {
            return this.Matched ? this._result
                                : result;
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