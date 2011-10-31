using System;
using System.Collections.Generic;

using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public class FractionalValueGenerator : BaseNumberValueGenerator {
        private static readonly Random _random = new Random();

        protected override object GetRandomValueCore(Type type, IConstraint[] constraints) {
            var valueScaled = _random.NextDouble();

            if (type == typeof(float))
                return this.ApplyConstraints(
                    constraints, 0.0f, 1.0f,
                    (minValue, maxValue) => minValue + valueScaled * (maxValue - minValue)
                );

            if (type == typeof(double))
                return this.ApplyConstraints(
                    constraints, 0.0d, 1.0d,
                    (minValue, maxValue) => minValue + valueScaled * (maxValue - minValue)
                );

            if (type == typeof(decimal))
                return this.ApplyConstraints(
                    constraints, 0.0m, 1.0m,
                    (minValue, maxValue) => minValue + (decimal)valueScaled * (maxValue - minValue)
                );

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