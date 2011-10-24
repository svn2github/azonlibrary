using System;

using Azon.Helpers.Asserts;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Asserts {
    [TestFixture]
    public class RequireTests {
        #region NotNull

        [Test]
        public void NotNullShouldRaiseGivenExceptionIfObjectIsNull() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.NotNull<ArgumentException>(null, string.Empty)
            );
        }

        [Test]
        public void NotNullShouldRaiseArgumentNullExceptionByDefault() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Require.NotNull(null, string.Empty)
            );
        }

        [Test]
        public void NotNullShouldRaiseArgumentNullWithParameterNameSet() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Require.NotNull(null, "param"),
                ex => Assert.AreEqual("param", ex.ParamName)
            );
        }

        [Test]
        public void NotNullShouldRaiseExceptionWithCertainMessage() {
            ExceptionAssert.Throws(
                () => Require.NotNull<Exception>(null, "certain message"),
                "certain message"
            );
        }

        [Test]
        public void NotNullShouldFormatMessageUsingGivenParameters() {
            ExceptionAssert.Throws(
                () => Require.NotNull<Exception>(null, "{0} {1}", 1, "pinguin"),
                "1 pinguin"
            );
        }

        [Test]
        public void NotNullShouldNotThrowIfObjectIsNotNull() {
            ExceptionAssert.DoesNotThrow(
                () => Require.NotNull(new object(), string.Empty)
            );
        }

        #endregion

        #region NotEmpty

        [Test]
        [Row(null)]
        [Row("")]
        public void NotEmptyShouldRaiseGivenExceptionIfStringIsNullOrEmpty(string value) {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Require.NotEmpty<InvalidOperationException>(value, string.Empty)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentNullExceptionByDefaultIfStringIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Require.NotEmpty(null, string.Empty)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentNullExceptionWithParameterNameSet() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Require.NotEmpty(null, "param"),
                ex => Assert.AreEqual("param", ex.ParamName)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentOutOfRangeExceptionByDefaultIfStringIsEmpty() {
            ExceptionAssert.Throws<ArgumentOutOfRangeException>(
                () => Require.NotEmpty(string.Empty, string.Empty)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentOutOfRangeExceptionWithParameterNameSet() {
            ExceptionAssert.Throws<ArgumentOutOfRangeException>(
                () => Require.NotEmpty(string.Empty, "param"),
                ex => Assert.AreEqual("param", ex.ParamName)
            );
        }

        [Test]
        [Row(null)]
        [Row("")]
        public void NotEmptyShouldRaiseEceptionWithCertainMessage(string value) {
            ExceptionAssert.Throws(
                () => Require.NotEmpty<Exception>(value, "certain message"),
                "certain message"
            );
        }

        [Test]
        [Row(null)]
        [Row("")]
        public void NotEmptyShouldFormatMessageUsingGivenParameters(string value) {
            ExceptionAssert.Throws(
                () => Require.NotEmpty<Exception>(value, "{0} {1}", 1, "pinguin"),
                "1 pinguin"
            );
        }

        [Test]
        public void NotEmptyShouldNotRaiseExceptionIfStringIsNotNullOrEmpty() {
            ExceptionAssert.DoesNotThrow(
                () => Require.NotEmpty("valid string", string.Empty)
            );
        }

        #endregion

        #region That

        [Test]
        [Row(null)]
        [Row("")]
        public void ThatShouldRaiseGivenExceptionIfConditionIsFalse(string value) {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Require.That<InvalidOperationException>(false, string.Empty)
            );
        }

        [Test]
        public void ThatShouldRaiseArgumentExceptionByDefault() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.That(false, string.Empty)
            );
        }

        [Test]
        public void ThatShouldRaiseExceptionWithCertainMessage() {
            ExceptionAssert.Throws(
                () => Require.That<Exception>(false, "certain message"),
                "certain message"
            );
        }

        [Test]
        public void ThatShouldFormatMessageUsingGivenParameters() {
            ExceptionAssert.Throws(
                () => Require.That<Exception>(false, "{0} {1}", 1, "pinguin"),
                "1 pinguin"
            );
        }

        [Test]
        public void ThatShouldNotRaiseExceptionIfStringIsNotNullOrEmpty() {
            ExceptionAssert.DoesNotThrow(
                () => Require.That(true, string.Empty)
            );
        }

        #endregion
    }
}
