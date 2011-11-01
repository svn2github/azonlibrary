using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Azon.Helpers.Asserts;
using Azon.Helpers.Constructs;
using Azon.Helpers.Extensions;
using Azon.Helpers.Generators.ValueGenerators.Constraints;
using Azon.Helpers.Utils;

namespace Azon.Helpers.Generators.ValueGenerators {
    public class CollectionValueGenerator : ValueGenerator {
        protected override bool IsTypeSupported(Type type) {
            return type.IsGenericDefinedAs(typeof(ICollection<>));
        }

        protected override object GetRandomValueCore(Type type, IConstraint[] constraints) {
            return type.IsInterface ? this.TryConstructFromInterface(type)
                                    : this.TryConstructFromType(type);
        }

        private object TryConstructFromType(Type type) {
            Require.That(
                !type.IsAbstract,
                "Type {0} is abstract and cannot be instantiated.",
                type
            );

            throw new NotImplementedException();
        }

        private object TryConstructFromInterface(Type type) {
            return Switch
                .Type<object>(type)
                    .WhenGeneric(typeof(IList<>), args => Call.Generic(() => new List<Type>(), args))
                    .WhenGeneric(typeof(ISet<>), args => Call.Generic(() => new HashSet<Type>(), args))
                    .WhenGeneric(typeof(ICollection<>), args => Call.Generic(() => new Collection<Type>(), args))
                    .OtherwiseThrow<NotSupportedException>("Interface {0} is not supported.", type);
        }

        public override IEnumerable<Type> ForTypes {
            get { yield return typeof(ICollection<>); }
        }
    }
}
