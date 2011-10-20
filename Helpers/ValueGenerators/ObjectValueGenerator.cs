using System;

using Azon.Helpers.ValueGenerators.Constraints;

namespace Azon.Helpers.ValueGenerators {
    public class ObjectValueGenerator : ValueGenerator<Object> {
        public override object GetRandomValue(Type type, IConstraint[] constraints) {
            var constructor = type.GetConstructor(new Type[0]);
            Require.NotNull<NotSupportedException>(constructor, "Type {0} doesn't have a default constructor.", type);
            var value = constructor.Invoke(new object[0]);

            Any.Properties(value);

            return value;
        }
    }
}
