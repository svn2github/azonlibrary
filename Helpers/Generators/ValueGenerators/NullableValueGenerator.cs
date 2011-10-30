using System;
using System.Collections.Generic;

using Azon.Helpers.Extensions;
using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public class NullableValueGenerator : ValueGenerator {
        protected override bool IsTypeSupported(Type type) {
            return type.IsGenericDefinedAs(typeof(Nullable<>));
        }

        protected override object GetRandomValueCore(Type type, IConstraint[] constraints) {
            return Any.Double() > 0.3 ? Any.Value(type, constraints)
                                      : null;
        }

        public override IEnumerable<Type> ForTypes {
            get { yield return typeof(Nullable<>); }
        }
    }
}