using System;
using System.Diagnostics;

namespace Azon.Helpers.Generators.ValueGenerators.Constraints {
    [DebuggerStepThrough]
    public class MinValueConstraint<T> : IConstraint 
        where T : struct, IComparable<T>
    {
        public MinValueConstraint(T minValue) {
            this.MinValue = minValue;
        }

        public T MinValue { get; private set; }
    }
}