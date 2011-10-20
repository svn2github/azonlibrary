using System;
using System.Collections.Generic;
using System.Linq;

using Azon.Helpers.Extensions;
using Azon.Helpers.ValueGenerators;
using Azon.Helpers.ValueGenerators.Constraints;

namespace Azon.Helpers {
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

        public static void Properties<T>(T instance) {
            var properties = instance.GetType().GetProperties().Where(p => p.CanWrite);

            foreach (var property in properties) {
                property.SetValue(instance, Any.Value(property.PropertyType));
            }
        }

        public static T Value<T>(params IConstraint[] constraints) {
            return (T)Any.Value(typeof(T), constraints);
        }

        public static object Value(Type type, params IConstraint[] constraints) {
            var generator = _generators.FirstOrDefault(pair => pair.Key.IsAssignableFrom(type)
                                                            || type.IsGenericDefinedAs(pair.Key));
            if (generator.Equals(_notFound))
                throw new NotSupportedException(string.Format("No generators supporting type {0} is registered.", type));

            return generator.Value.GetRandomValue(type, constraints);
        }

        public static int Integer(int maxValue) {
            return Any.Integer(0, maxValue);
        }

        public static int Integer(int minValue, int maxValue) {
            return Any.Value<int>(
                new MinValueConstraint<int>(minValue),
                new MaxValueConstraint<int>(maxValue)
            );
        }

        public static double Double() {
            return Any.Value<double>();
        }
    }
}
