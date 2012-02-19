using System;
using System.Linq;
using System.Threading;

using Azon.Helpers.Asserts;
using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public abstract class BaseNumberValueGenerator : ValueGenerator {
        protected T ApplyConstraints<T>(
            IConstraint[] constraints, 
            T minValue, T maxValue, 
            Func<T, T, object> calculate
        )
            where T : struct, IComparable<T>
        {
            var minValueConstraint = constraints.OfType<MinValueConstraint<T>>().SingleOrDefault();
            var maxValueConstraint = constraints.OfType<MaxValueConstraint<T>>().SingleOrDefault();

            if (minValueConstraint != null)
                minValue = minValueConstraint.MinValue;

            if (maxValueConstraint != null)
                maxValue = maxValueConstraint.MaxValue;

            Require.That<InvalidOperationException>(
                minValue.CompareTo(maxValue) <= 0,
                "minValue ({0}) should not be greater than maxValue ({1}).",
                minValue, maxValue
            );

            return (T)Convert.ChangeType(
                calculate(minValue, maxValue),
                typeof(T),
                Thread.CurrentThread.CurrentCulture
            );
        }
    }
}