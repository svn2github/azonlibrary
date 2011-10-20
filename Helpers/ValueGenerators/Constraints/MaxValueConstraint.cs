using System.Diagnostics;

namespace Azon.Helpers.ValueGenerators.Constraints {
    [DebuggerStepThrough]
    public class MaxValueConstraint<T> : IConstraint
        where T : struct 
    {
        public MaxValueConstraint(T maxValue) {
            this.MaxValue = maxValue;
        }

        public T MaxValue { get; private set; }
    }
}