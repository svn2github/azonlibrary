using System;
using System.Collections.Generic;

using Azon.Helpers.Extensions;
using Azon.Helpers.Tests.Utils.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Extensions {
    [TestFixture]
    public class TypeExtensionsTests {
        [Test]
        public void IsGenericDefinedAsShouldThrowIfTestedTypeIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => (null as Type).IsGenericDefinedAs(typeof(int)),
                ex => Assert.AreEqual("type", ex.ParamName)
            );
        }

        [Test]
        public void IsGenericDefinedAsShouldReturnFalseIfGivenTypeIsNull() {
            Assert.IsFalse(
                typeof(int).IsGenericDefinedAs(null)
            );
        }

        [Test]
        public void IsGenericDefinedAsShouldReturnFalseIfTestedTypeIsNotGeneric() {
            Assert.IsFalse(
                typeof(int).IsGenericDefinedAs(typeof(IEnumerable<>))
            );
        }

        [Test]
        public void IsGenericDefinedAsShouldReturnFalseIfGivenTypeIsNotGeneric() {
            Assert.IsFalse(
                typeof(int?).IsGenericDefinedAs(typeof(int))
            );
        }

        [Test]
        public void IsGenericDefinedAsShouldReturnFalseIfTestedTypeIsNotConstructedFromGiven() {
            Assert.IsFalse(
                typeof(int?).IsGenericDefinedAs(typeof(IEnumerable<>))
            );
        }

        [Test]
        public void IsGenericDefinedAsShouldReturnTrueIfTestedTypeIsConstructedFromGiven() {
            Assert.IsTrue(
                typeof(int?).IsGenericDefinedAs(typeof(Nullable<>))
            );
        }
    }
}
