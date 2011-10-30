using System;

using Azon.Helpers.Asserts;
using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public class ObjectValueGenerator : ValueGenerator<object> {
        public override object GetRandomValue(Type type, IConstraint[] constraints) {
            Require.That<NotSupportedException>(
                !type.IsAbstract,
                "Type {0} is abstract and cannot be instantiated.",
                type
            );

            var constructor = type.GetConstructor(new Type[0]);

            Require.NotNull<NotSupportedException>(
                constructor,
                "Type {0} does not have public default constructor.",
                type
            );

            var value = constructor.Invoke(new object[0]);

            Any.Properties(value);

            return value;
        }
    }
}
