using System.Diagnostics;

namespace Azon.Helpers.Generators.ValueGenerators.Constraints {
    [DebuggerStepThrough]
    public class MinSizeConstraint : IConstraint {
        public MinSizeConstraint(int minSize) {
            this.MinSize = minSize;
        }

        public int MinSize { get; private set; }
    }
}