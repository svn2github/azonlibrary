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

        ISwitchTypeWithResult<TResult> ISwitchTypeWithResult<TResult>.When<TTry>(TResult result) {
            return this.When<TTry>(result);
        }

        ISwitchTypeWithResult<TResult> ISwitchTypeWithResult<TResult>.WhenGeneric(Type type, Func<Type[], TResult> func) {
            return this.WhenGeneric(type, func);
        }

        ISwitchTypeWithResult<TResult> ISwitchTypeWithResult<TResult>.WhenGeneric(Type type, TResult result) {
            return this.WhenGeneric(type, result);
        }

        #endregion

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

        public ISwitchTypeWithResult<T, TResult> WhenGeneric(Type type, Func<Type[], TResult> func) {
            throw new NotImplementedException();
        }

        public ISwitchTypeWithResult<T, TResult> WhenGeneric(Type type, TResult result) {
            throw new NotImplementedException();
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