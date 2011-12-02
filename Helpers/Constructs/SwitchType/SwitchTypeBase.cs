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
        protected Type TestedType { get; private set; }

        protected SwitchTypeBase(bool exactType, T value) {
            this._observed = new List<Type>();
            this._exactType = exactType;

            this.Value = value;
            this.Matched = false;
            this.TestedType = Object.Equals(this.Value, default(T)) ? typeof(T)
                                                                    : this.Value.GetType();
        }

        protected bool Matches<TTry>() {
            this.Observe(typeof(TTry));
            
            return !this.Matched
                && this.TypeMatches<TTry>()
                && (this.Matched = true);
        }

        protected bool MatchesGeneric(Type type, out Type[] args) {
            Require.That(
                type.IsGenericTypeDefinition,
                "Type {1} is not an open generic type.",
                "type", type
            );

            this.Observe(type);
            args = null;

            return !this.Matched
                && this.TypeMatchesGeneric(type, out args)
                && (this.Matched = true);
        }

        private void Observe(Type type) {
            Require.That<InvalidOperationException>(
                !this._observed.Any(t => type.Inherits(t) || type.Implements(t)),
                "Cases must be placed in accord to their generalization and should not duplicate."
            );

            this._observed.Add(type);
        }

        private bool TypeMatches<TTry>() {
            return (this._exactType && typeof(TTry) == this.TestedType)
                || (!this._exactType && typeof(TTry).IsAssignableFrom(this.TestedType));
        }

        private bool TypeMatchesGeneric(Type type, out Type[] args) {
            if (this._exactType) {
                args = this.TestedType.IsGenericDefinedAs(type)
                            ? this.TestedType.GetGenericArguments()
                            : null;
            }
            else {
                var testingTypes = type.IsInterface ? this.TestedType.GetInterfaces(includeSelf: true)
                                                    : this.TestedType.GetHierarchy(includeSelf: true);
                var closedGeneric = testingTypes.FirstOrDefault(i => i.IsGenericDefinedAs(type));

                args = (closedGeneric != null) ? closedGeneric.GetGenericArguments()
                                               : null;
            }

            return args != null;
        }
    }
}