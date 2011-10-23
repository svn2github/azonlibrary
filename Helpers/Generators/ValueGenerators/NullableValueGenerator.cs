using System;
using System.Collections.Generic;

using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public class NullableValueGenerator : IValueGenerator {
        public object GetRandomValue(Type type, IConstraint[] constraints) {
            if (Any.Double() > 0.3)
                return Any.Value(type, constraints);

            return null;
        }

        public IEnumerable<Type> ForTypes {
            get { yield return typeof(Nullable<>); }
        }
    }
}