using System;
using System.Collections;
using System.Collections.Generic;

using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public class EnumValueGenerator : IValueGenerator {
        public object GetRandomValue(Type type, IConstraint[] constraints) {
            var possibleValues = Enum.GetValues(type);
            if (possibleValues.Length == 0)
                throw new InvalidOperationException(string.Format("Enumeration {0} doesn't declare any values.", type));

            var randomIndex = Any.Integer(possibleValues.Length - 1);
            return (possibleValues as IList)[randomIndex];
        }

        public IEnumerable<Type> ForTypes {
            get { yield return typeof(Enum); }
        }
    }
}