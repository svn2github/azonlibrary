using System;
using System.Globalization;

using Azon.Helpers.Generators.ValueGenerators;
using Azon.Helpers.Generators.ValueGenerators.Constraints;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators {
    [TestFixture]
    public class EnumValueGeneratorTests : ValueGeneratorTest<EnumValueGenerator> {
        public enum Foo {}

        [Test]
        public void ShouldReturnEnumAsSupportedType() {
            Assert.AreElementsSame(
                new[] { typeof(Enum) },
                this.Generator.ForTypes
            );
        }

        [Test]
        public void ShouldThrowIfGivenTypeIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => this.Generator.GetRandomValue(null, new IConstraint[0]),
                ex => Assert.AreEqual("enumType", ex.ParamName)
            );
        }

        [Test]
        public void ShouldThrowIfGivenTypeIsNotInheritedForEnum() {
            ExceptionAssert.Throws<ArgumentException>(
                () => this.Generator.GetRandomValue(typeof(int), new IConstraint[0]),
                "Type provided must be an Enum.\r\nParameter name: enumType"
            );
        }

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
