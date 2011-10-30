using System;
using System.Globalization;

using Azon.Helpers.Generators.ValueGenerators;
using Azon.Helpers.Generators.ValueGenerators.Constraints;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators {
    [TestFixture]
    public class EnumValueGeneratorTests : ValueGeneratorTest<EnumValueGenerator> {
        protected override Type[] SupportedTypes {
            get { return new[] { typeof(Enum) }; }
        }

        public enum Foo {}

        [Test]
        public void ShouldThrowIfGivenEnumIsEmpty() {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => this.Generator.GetRandomValue(typeof(Foo), new IConstraint[0]),
                "Enumeration Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators.EnumValueGeneratorTests+Foo doesn't declare any values."
            );
        }

        [Test]
        public void ShouldReturnValueOfGivenEnum() {
            Assert.IsInstanceOfType<CultureTypes>(
                this.Generator.GetRandomValue(typeof(CultureTypes), new IConstraint[0])
            );
        }
    }
}
