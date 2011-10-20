using System;
using System.Collections.Generic;
using System.Linq;

using Azon.Helpers.ValueGenerators.Constraints;

namespace Azon.Helpers.ValueGenerators {
    public class NumericValueGenerator : IValueGenerator {
        private static readonly Random _random = new Random();

        public object GetRandomValue(Type type, IConstraint[] constraints) {
            var randomNumber = _random.Next(Int32.MinValue, Int32.MaxValue);
            var valueScaled = (double)((long)randomNumber - Int32.MinValue) / ((long)Int32.MaxValue - Int32.MinValue);

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

            //if (type == typeof(long))
            //    return this.ApplyConstraints(
            //        constraints, long.MinValue, long.MaxValue,
            //        (minValue, maxValue) => minValue + valueScaled * (maxValue - minValue)
            //    );

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

            //if (type == typeof(ulong))
            //    return this.ApplyConstraints<ulong>(
            //        constraints,
            //        (minValue, maxValue) => minValue + valueScaled * (maxValue - minValue)
            //    );


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

        private T ApplyConstraints<T>(IConstraint[] constraints, T minValue, T maxValue, Func<T, T, object> calculate)
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

        public IEnumerable<Type> ForTypes {
            get {
                yield return typeof(sbyte);
                yield return typeof(short);
                yield return typeof(int);
                //yield return typeof(long);
                yield return typeof(byte);
                yield return typeof(ushort);
                yield return typeof(uint);
                //yield return typeof(ulong);

                yield return typeof(float);
                yield return typeof(double);
                yield return typeof(decimal);
            }
        }
    }
}