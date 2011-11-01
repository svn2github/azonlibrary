using System;
using System.Collections.Generic;

using Azon.Helpers.Constructs;
using Azon.Helpers.Extensions;
using Azon.Helpers.Generators.ValueGenerators.Constraints;

namespace Azon.Helpers.Generators.ValueGenerators {
    public class CollectionValueGenerator : ValueGenerator {
        protected override bool IsTypeSupported(Type type) {
            return type.IsGenericDefinedAs(typeof(ICollection<>));
        }

        protected override object GetRandomValueCore(Type type, IConstraint[] constraints) {
            //Switch.Type(type)
            throw new NotImplementedException();
        }

        public override IEnumerable<Type> ForTypes {
            get { yield return typeof(ICollection<>); }
        }
    }
}
