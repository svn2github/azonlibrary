using System;
using System.Collections.Generic;
using System.Linq;

using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public class NumericValueGenerator : ValueGenerator {
        private static readonly Random _random = new Random();

        protected override object GetRandomValueCore(Type type, IConstraint[] constraints) {
            var randomNumber = _random.Next(Int32.MinValue, Int32.MaxValue);
            var valueScaled = (decimal)((long)randomNumber - Int32.MinValue) / ((long)Int32.MaxValue - Int32.MinValue);

            if (type == typeof(sbyte))
                return this.ApplyConstraints(
                    constraints, sbyte.MinValue, sbyte.MaxValue,
                    (minValue, maxValue) => minValue + valueScaled * (maxValue - minValue)
                );

            if (type == typeof(short))
                return this.ApplyConstraints(
                    constraints, short.MinValue, short.MaxValue,
                    (minValue, maxValue) => minValue + valueScaled * (maxValue - minValue)
                );

            if (type == typeof(int))
                return this.ApplyConstraints(
                    constraints, int.MinValue, int.MaxValue,
                    (minValue, maxValue) => minValue + valueScaled * ((long)maxValue - minValue)
                );

            if (type == typeof(long))
                return this.ApplyConstraints(
                    constraints, long.MinValue, long.MaxValue,
                    (minValue, maxValue) => this.ConstrictToRange(valueScaled, minValue, maxValue)
                );

            if (type == typeof(byte))
                return this.ApplyConstraints(
                    constraints, byte.MinValue, byte.MaxValue,
                    (minValue, maxValue) => minValue + valueScaled * (maxValue - minValue)
                );

            if (type == typeof(ushort))
                return this.ApplyConstraints(
                    constraints, ushort.MinValue, ushort.MaxValue,
                    (minValue, maxValue) => minValue + valueScaled * (maxValue - minValue)
                );

            if (type == typeof(uint))
                return this.ApplyConstraints(
                    constraints, uint.MinValue, uint.MaxValue,
                    (minValue, maxValue) => minValue + valueScaled * (maxValue - minValue)
                );

            if (type == typeof(ulong))
                return this.ApplyConstraints(
                    constraints, ulong.MinValue, ulong.MaxValue,
                    (minValue, maxValue) => minValue + valueScaled * (maxValue - minValue)
                );

            throw new InvalidOperationException(
                string.Format("{0} does not support generation of {1}", this.GetType(), type
            ));
        }

        private object ConstrictToRange(decimal valueScaled, long minValue, long maxValue) {
            var absolute = new Func<long, ulong>(
                value => (value > 0)
                             ? (ulong)value
                             : ulong.MaxValue - (ulong)value + 1);

            var range = absolute(maxValue) * (ulong)Math.Sign(maxValue) - absolute(minValue) * (ulong)Math.Sign(minValue);

            return minValue + valueScaled * range;
        }

        private T ApplyConstraints<T>(
            IConstraint[] constraints, 
            T minValue, 
            T maxValue, 
            Func<T, T, object> calculate
        )
            where T : struct 
        {
            var minValueConstraint = constraints.OfType<MinValueConstraint<T>>().SingleOrDefault();
            var maxValueConstraint = constraints.OfType<MaxValueConstraint<T>>().SingleOrDefault();

            if (minValueConstraint != null)
                minValue = minValueConstraint.MinValue;

            if (maxValueConstraint != null)
                maxValue = maxValueConstraint.MaxValue;

            return (T)Convert.ChangeType(calculate(minValue, maxValue), typeof(T));
        }

        public override IEnumerable<Type> ForTypes {
            get {
                yield return typeof(sbyte);
                yield return typeof(short);
                yield return typeof(int);
                yield return typeof(long);
                yield return typeof(byte);
                yield return typeof(ushort);
                yield return typeof(uint);
                yield return typeof(ulong);
            }
        }
    }
}