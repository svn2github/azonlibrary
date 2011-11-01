using System.Diagnostics;

namespace Azon.Helpers.Generators.ValueGenerators.Constraints {
    [DebuggerStepThrough]
    public class MaxSizeConstraint : IConstraint {
        public MaxSizeConstraint(int maxSize) {
            this.MaxSize = maxSize;
        }

        public int MaxSize { get; private set; }
    }
}