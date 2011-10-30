using System;
using System.Collections;

using Azon.Helpers.Asserts;
using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public class EnumValueGenerator : ValueGenerator<Enum> {
        public override Enum GetRandomValue(Type enumType, IConstraint[] constraints) {
            var possibleValues = Enum.GetValues(enumType);

            Require.NotEmpty<InvalidOperationException>(
                possibleValues,
                "Enumeration {0} doesn't declare any values.",
                enumType
            );

            var randomIndex = Any.Integer(possibleValues.Length - 1);
            return (Enum)(possibleValues as IList)[randomIndex];
        }
    }
}