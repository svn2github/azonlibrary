using System;

using Azon.Helpers.Generators.ValueGenerators;
using Azon.Helpers.Generators.ValueGenerators.Constraints;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators {
    [TestFixture]
    public class NullableValueGeneratorTests : ValueGeneratorTest<NullableValueGenerator> {
        protected override Type[] SupportedTypes {
            get { return new[] { typeof(Nullable<>) }; }
        }

        [Test]
        public void ShouldReturnValueOfGivenNullableType() {
            var generated = this.Generator.GetRandomValue(typeof(int?), new IConstraint[0]);
            if (generated == null)
                return;

            Assert.IsInstanceOfType<int?>(generated);
        }
    }
}
