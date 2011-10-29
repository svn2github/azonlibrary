using System;
using System.Collections.Generic;
using System.Linq;

using Azon.Helpers.Asserts;
using Azon.Helpers.Extensions;

namespace Azon.Helpers.Constructs.SwitchType {
    internal abstract class SwitchTypeBase<T> {
        private readonly bool _exactType;
        private readonly IList<Type> _observed;

        protected T Value { get; private set; }
        protected bool Matched { get; private set; }

        protected SwitchTypeBase(bool exactType, T value) {
            this._observed = new List<Type>();
            this._exactType = exactType;
            this.Value = value;
            this.Matched = false;
        }

        protected bool Matches<TTry>() {
            Require.That<InvalidOperationException>(
                this._observed.All(type => !type.IsAssignableFrom<TTry>()),
                "Cases must be placed in accord to their generalization and should not duplicate."
            );

            this._observed.Add(typeof(TTry));
            
            return !this.Matched
                && this.TypeMatches<TTry>()
                && (this.Matched = true);
        }

        private bool TypeMatches<TTry>() {
            if (Object.Equals(this.Value, default(T)))
                return (this._exactType && typeof(TTry) == typeof(T))
                    || (!this._exactType && typeof(TTry).IsAssignableFrom<T>());

            return (this._exactType && typeof(TTry) == this.Value.GetType())
                || (!this._exactType && typeof(TTry).IsInstanceOfType(this.Value));
        }
    }
}