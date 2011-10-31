using System;
using System.Globalization;

using Azon.Helpers.Generators.ValueGenerators;
using Azon.Helpers.Generators.ValueGenerators.Constraints;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators {
    [TestFixture]
    public class EnumValueGeneratorTests : ValueGeneratorTest<EnumValueGenerator> {
        public enum Foo { }

        protected override Type[] SupportedTypes {
            get { return new[] { typeof(Enum) }; }
        }

        protected override Type[] SampleTypes {
            get { return new[] { typeof(CultureTypes) }; }
        }

        [Test]
        public void ShouldThrowIfGivenEnumIsEmpty() {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => this.Generator.GetRandomValue(typeof(Foo), new IConstraint[0]),
                "Enumeration Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators.EnumValueGeneratorTests+Foo doesn't declare any values."
            );
        }
    }
}
