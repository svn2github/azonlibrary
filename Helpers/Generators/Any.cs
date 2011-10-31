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
                    new FractionalValueGenerator(),
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

        #region Shortcuts

        public static sbyte SByte() {
            return Any.Value<sbyte>();
        }

        public static sbyte SByte(sbyte maxValue) {
            return Any.Value<sbyte>(
                new MaxValueConstraint<sbyte>(maxValue)
            );
        }

        public static sbyte SByte(sbyte minValue, sbyte maxValue) {
            return Any.Value<sbyte>(
                new MinValueConstraint<sbyte>(minValue),
                new MaxValueConstraint<sbyte>(maxValue)
            );
        }

        public static short Short() {
            return Any.Value<sbyte>();
        }

        public static short Short(short maxValue) {
            return Any.Value<short>(
                new MaxValueConstraint<short>(maxValue)
            );
        }

        public static short Short(short minValue, short maxValue) {
            return Any.Value<short>(
                new MinValueConstraint<short>(minValue),
                new MaxValueConstraint<short>(maxValue)
            );
        }

        public static int Integer() {
            return Any.Value<int>();
        }

        /// <summary>
        /// Returns a random non-negative integer value.
        /// </summary>
        /// <param name="maxValue">A maximal possible value.</param>
        /// <returns>A non-negative integer value.</returns>
        public static int Integer(int maxValue) {
            return Any.Value<int>(
                new MaxValueConstraint<int>(maxValue)
            );
        }

        /// <summary>
        /// Returns an random integer value within a specified range.
        /// </summary>
        /// <param name="minValue">A minimal possible value.</param>
        /// <param name="maxValue">A maximal possible value.</param>
        /// <returns>An integer value within a specified range.</returns>
        public static int Integer(int minValue, int maxValue) {
            return Any.Value<int>(
                new MinValueConstraint<int>(minValue),
                new MaxValueConstraint<int>(maxValue)
            );
        }

        public static long Long() {
            return Any.Value<long>();
        }

        public static long Long(long maxValue) {
            return Any.Value<long>(
                new MaxValueConstraint<long>(maxValue)
            );
        }

        public static long Long(long minValue, long maxValue) {
            return Any.Value<long>(
                new MinValueConstraint<long>(minValue),
                new MaxValueConstraint<long>(maxValue)
            );
        }

        public static byte Byte() {
            return Any.Value<byte>();
        }

        public static byte Byte(byte maxValue) {
            return Any.Value<byte>(
                new MaxValueConstraint<byte>(maxValue)
            );
        }

        public static byte Byte(byte minValue, byte maxValue) {
            return Any.Value<byte>(
                new MinValueConstraint<byte>(minValue),
                new MaxValueConstraint<byte>(maxValue)
            );
        }

        public static ushort UShort() {
            return Any.Value<ushort>();
        }

        public static ushort UShort(ushort maxValue) {
            return Any.Value<ushort>(
                new MaxValueConstraint<ushort>(maxValue)
            );
        }

        public static ushort UShort(ushort minValue, ushort maxValue) {
            return Any.Value<ushort>(
                new MinValueConstraint<ushort>(minValue),
                new MaxValueConstraint<ushort>(maxValue)
            );
        }

        public static uint UInt() {
            return Any.Value<uint>();
        }

        public static uint UInt(uint maxValue) {
            return Any.Value<uint>(
                new MaxValueConstraint<uint>(maxValue)
            );
        }

        public static uint UInt(uint minValue, uint maxValue) {
            return Any.Value<uint>(
                new MinValueConstraint<uint>(minValue),
                new MaxValueConstraint<uint>(maxValue)
            );
        }

        public static ulong ULong() {
            return Any.Value<ulong>();
        }

        public static ulong ULong(ulong maxValue) {
            return Any.Value<ulong>(
                new MaxValueConstraint<ulong>(maxValue)
            );
        }

        public static ulong ULong(ulong minValue, ulong maxValue) {
            return Any.Value<ulong>(
                new MinValueConstraint<ulong>(minValue),
                new MaxValueConstraint<ulong>(maxValue)
            );
        }

        /// <summary>
        /// Returns a random float between 0.0 and 1.0.
        /// </summary>
        /// <returns>A random float between 0.0 and 1.0.</returns>
        public static float Float() {
            return Any.Value<float>();
        }

        public static float Float(float maxValue) {
            return Any.Value<float>(
                new MaxValueConstraint<float>(maxValue)
            );
        }

        public static float Float(float minValue, float maxValue) {
            return Any.Value<float>(
                new MinValueConstraint<float>(minValue),
                new MaxValueConstraint<float>(maxValue)
            );
        }

        /// <summary>
        /// Returns a random double between 0.0 and 1.0.
        /// </summary>
        /// <returns>A random double between 0.0 and 1.0.</returns>
        public static double Double() {
            return Any.Value<double>();
        }

        public static double Double(double maxValue) {
            return Any.Value<double>(
                new MaxValueConstraint<double>(maxValue)
            );
        }

        public static double Double(double minValue, double maxValue) {
            return Any.Value<double>(
                new MinValueConstraint<double>(minValue),
                new MaxValueConstraint<double>(maxValue)
            );
        }

        /// <summary>
        /// Returns a random decimal between 0.0 and 1.0.
        /// </summary>
        /// <returns>A random decimal between 0.0 and 1.0.</returns>
        public static decimal Decimal() {
            return Any.Value<decimal>();
        }

        public static decimal Decimal(decimal maxValue) {
            return Any.Value<decimal>(
                new MaxValueConstraint<decimal>(maxValue)
            );
        }

        public static decimal Decimal(decimal minValue, decimal maxValue) {
            return Any.Value<decimal>(
                new MinValueConstraint<decimal>(minValue),
                new MaxValueConstraint<decimal>(maxValue)
            );
        }

        #endregion
    }
}
