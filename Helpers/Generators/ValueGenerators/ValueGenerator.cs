using System;
using System.Collections.Generic;
using System.Linq;

using Azon.Helpers.Asserts;
using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public abstract class ValueGenerator : IValueGenerator {
        public object GetRandomValue(Type type, IConstraint[] constraints) {
            Require.NotNull(type, "type");
            Require.That<InvalidOperationException>(
                this.IsTypeSupported(type),
                "{0} does not support generation of {1}.",
                this.GetType(), type
            );

            return this.GetRandomValueCore(type, constraints);
        }

        protected virtual bool IsTypeSupported(Type type) {
            return this.ForTypes.Any(t => t.IsAssignableFrom(type));
        }

        protected abstract object GetRandomValueCore(Type type, IConstraint[] constraints);

        public abstract IEnumerable<Type> ForTypes { get; }
    }

    public abstract class ValueGenerator<T> : ValueGenerator, IValueGenerator<T> {
        protected override object GetRandomValueCore(Type type, IConstraint[] constraints) {
            return this.GetRandomValue(type, constraints);
        }

        public new abstract T GetRandomValue(Type type, IConstraint[] constraints);

        public override IEnumerable<Type> ForTypes {
            get { yield return typeof(T); }
        }
    }
}