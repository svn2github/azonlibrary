using System;
using System.IO;

using Azon.Helpers.Generators.ValueGenerators;
using Azon.Helpers.Generators.ValueGenerators.Constraints;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators {
    [TestFixture]
    public class ObjectValueGeneratorTests : ValueGeneratorTest<ObjectValueGenerator> {
        protected override Type[] SupportedTypes {
            get { return new[] { typeof(object) }; }
        }

        protected override bool SkipUnsupportedTypeTest {
            get { return true; }
        }

        [Test]
        public void ShouldThrowIfGivenTypeIsAbstract() {
            ExceptionAssert.Throws<NotSupportedException>(
                () => this.Generator.GetRandomValue(typeof(Stream), new IConstraint[0]),
                "Type System.IO.Stream is abstract and cannot be instantiated."
            );
        }

        [Test]
        public void ShouldThrowIfGivenTypeDoesNotHavePublicDefaultConstructor() {
            ExceptionAssert.Throws<NotSupportedException>(
                () => this.Generator.GetRandomValue(typeof(Activator), new IConstraint[0]),
                "Type System.Activator does not have public default constructor."
            );
        }
    }
}
