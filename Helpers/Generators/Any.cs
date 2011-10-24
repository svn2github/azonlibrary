using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Extensions;

using Azon.Helpers.Asserts;
using Azon.Helpers.Generators.ValueGenerators;
using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators {
    /// <summary>
    /// Contains a set of static methods to generate objects of desired type.
    /// </summary>
    public static class Any {
        private static readonly KeyValuePair<Type, IValueGenerator> _notFound;
        private static readonly List<KeyValuePair<Type, IValueGenerator>> _generators;

        static Any() {
            _notFound = default(KeyValuePair<Type, IValueGenerator>);
            _generators = (
                from generator in new List<IValueGenerator> { 
                    new PrimitiveValueGenerator(), 
                    new NumericValueGenerator(),
                    new EnumValueGenerator(),
                    new NullableValueGenerator(),
                    new ObjectValueGenerator()
                }
                from type in generator.ForTypes
                select new KeyValuePair<Type, IValueGenerator>(type, generator)
            ).ToList();

            _generators.Sort((x, y) => x.Key.IsAssignableFrom(y.Key) ? 0 : 1);
        }

        /// <summary>
        /// Sets public properties of a given object to random values.
        /// </summary>
        /// <typeparam name="T">A type of object.</typeparam>
        /// <param name="instance">An object to generate values for.</param>
        public static void Properties<T>(T instance) {
            Require.NotNull(instance, "instance");

            var properties = instance.GetType().GetProperties().Where(p => p.CanWrite);

            foreach (var property in properties) {
                property.SetValue(instance, Any.Value(property.PropertyType));
            }
        }

        /// <summary>
        /// Returns a random value of a specified type using registered generators.
        /// </summary>
        /// <typeparam name="T">A type of value to generate.</typeparam>
        /// <param name="constraints">Optional constraints to be applied to a generated value.</param>
        /// <returns>A value of a specified type.</returns>
        public static T Value<T>(params IConstraint[] constraints) {
            return (T)Any.Value(typeof(T), constraints);
        }

        /// <summary>
        /// Returns a random value of a specified type using registered generators.
        /// </summary>
        /// <param name="type">A type of value to generate.</param>
        /// <param name="constraints">Optional constraints to be applied to a generated value.</param>
        /// <returns>An value of a specified type.</returns>
        public static object Value(Type type, params IConstraint[] constraints) {
            Require.NotNull(type, "type");

            var generator = _generators.FirstOrDefault(pair => pair.Key.IsAssignableFrom(type)
                                                            || type.IsGenericTypeDefinedAs(pair.Key));
            if (generator.Equals(_notFound))
                throw new NotSupportedException(string.Format("No generators supporting type {0} is registered.", type));

            return generator.Value.GetRandomValue(type, constraints);
        }

        /// <summary>
        /// Returns a random non-negative integer value.
        /// </summary>
        /// <param name="maxValue">A maximal possible value.</param>
        /// <returns>A non-negative integer value.</returns>
        public static int Integer(int maxValue) {
            Require.That(maxValue >= 0, "maxValue must be greater than or equal to zero.");
            return Any.Integer(0, maxValue);
        }

        /// <summary>
        /// Returns an random integer value within a specified range.
        /// </summary>
        /// <param name="minValue">A minimal possible value.</param>
        /// <param name="maxValue">A maximal possible value.</param>
        /// <returns>An integer value within a specified range.</returns>
        public static int Integer(int minValue, int maxValue) {
            Require.That(minValue <= maxValue, "minValue must be less than or equal to maxValue.");

            return Any.Value<int>(
                new MinValueConstraint<int>(minValue),
                new MaxValueConstraint<int>(maxValue)
            );
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>A random number between 0.0 and 1.0.</returns>
        public static double Double() {
            return Any.Value<double>();
        }
    }
}
