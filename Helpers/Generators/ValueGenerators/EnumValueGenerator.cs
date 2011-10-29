using System;
using System.Collections;
using System.Collections.Generic;

using Azon.Helpers.Asserts;
using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public class EnumValueGenerator : IValueGenerator {
        public object GetRandomValue(Type enumType, IConstraint[] constraints) {
            var possibleValues = Enum.GetValues(enumType);

            Require.NotEmpty<InvalidOperationException>(
                possibleValues,
                "Enumeration {0} doesn't declare any values.",
                enumType
            );

            var randomIndex = Any.Integer(possibleValues.Length - 1);
            return (possibleValues as IList)[randomIndex];
        }

        public IEnumerable<Type> ForTypes {
            get { yield return typeof(Enum); }
        }
    }
}