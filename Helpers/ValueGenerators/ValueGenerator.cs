using System;
using System.Collections.Generic;
using System.Linq;

using Azon.Helpers.ValueGenerators.Constraints;

namespace Azon.Helpers.ValueGenerators {
    public abstract class ValueGenerator<T> : IValueGenerator<T> {
        object IValueGenerator.GetRandomValue(Type type, IConstraint[] constraints) {
            if (!this.ForTypes.Any(t => t.IsAssignableFrom(type)))
                throw new InvalidOperationException(
                    string.Format("{0} does not support generation of {1}", this.GetType(), type
                ));

            return GetRandomValue(type, constraints);
        }

        public abstract T GetRandomValue(Type type, IConstraint[] constraints);

        public IEnumerable<Type> ForTypes {
            get { yield return typeof(T); }
        }
    }
}