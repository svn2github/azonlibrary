using System;
using System.Collections.Generic;
using System.IO;

using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public class PrimitiveValueGenerator : IValueGenerator {
        private static readonly Random _random = new Random();

        public object GetRandomValue(Type type, IConstraint[] constraints) {
            if (type == typeof(char))
                return (char)_random.Next(Char.MinValue, Char.MaxValue);

            if (type == typeof(bool))
                return _random.NextDouble() > 0.5;

            if (type == typeof(string))
                return Path.GetRandomFileName();
            
            if (type == typeof(Guid))
                return Guid.NewGuid();


            throw new InvalidOperationException(
                string.Format("{0} does not support generation of {1}", this.GetType(), type
            ));
        }

        public IEnumerable<Type> ForTypes {
            get {
                yield return typeof(char);
                yield return typeof(bool);
                yield return typeof(string);
                yield return typeof(Guid);
            }
        }
    }
}