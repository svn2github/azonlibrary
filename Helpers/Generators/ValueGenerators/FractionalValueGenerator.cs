using System;
using System.Collections.Generic;

using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public class FractionalValueGenerator : ValueGenerator {
        private static readonly Random _random = new Random();

        protected override object GetRandomValueCore(Type type, IConstraint[] constraints) {
            if (type == typeof(float))
                return (float)_random.NextDouble();

            if (type == typeof(double))
                return _random.NextDouble();

            if (type == typeof(decimal))
                return (decimal)_random.NextDouble();

            throw new InvalidOperationException(
                string.Format("{0} does not support generation of {1}", this.GetType(), type
            ));
        }

        public override IEnumerable<Type> ForTypes {
            get {
                yield return typeof(float);
                yield return typeof(double);
                yield return typeof(decimal);
            }
        }
    }
}