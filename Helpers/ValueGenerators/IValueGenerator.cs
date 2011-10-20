using System;
using System.Collections.Generic;

using Azon.Helpers.ValueGenerators.Constraints;

namespace Azon.Helpers.ValueGenerators {
    public interface IValueGenerator {
        object GetRandomValue(Type type, IConstraint[] constraints);
        IEnumerable<Type> ForTypes { get; }
    }

    public interface IValueGenerator<out T> : IValueGenerator {
        new T GetRandomValue(Type type, IConstraint[] constraints);
    }
}
