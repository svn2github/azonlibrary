using System;
using System.Collections;
using System.Collections.Generic;

using Azon.Helpers.Asserts;
using Azon.Helpers.Tests.Internal.Asserts;

using Gallio.Framework.Data;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Asserts {
    [TestFixture]
    public class RequireTests {
        #region NotNull

        [Test]
        public void NotNullShouldRaiseGivenExceptionIfObjectIsNull() {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Require.NotNull<InvalidOperationException>(null, string.Empty)
            );
        }

        [Test]
        public void NotNullShouldRaiseGivenExceptionWithCertainMessageIfObjectIsNull() {
            ExceptionAssert.Throws(
                () => Require.NotNull<Exception>(null, "certain message"),
                "certain message"
            );
        }

        [Test]
        public void NotNullShouldFormatMessageUsingGivenParametersIfObjectIsNull() {
            ExceptionAssert.Throws(
                () => Require.NotNull<Exception>(null, "{0} {1}", 1, "pinguin"),
                "1 pinguin"
            );
        }

        [Test]
        public void NotNullShouldNotThrowIfObjectIsNotNull() {
            ExceptionAssert.DoesNotThrow(
                () => Require.NotNull<Exception>(new object(), string.Empty)
            );
        }

        #region Default

        [Test]
        public void NotNullShouldRaiseArgumentNullExceptionByDefaultIsObjectIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Require.NotNull(null, string.Empty)
            );
        }

        [Test]
        public void NotNullShouldRaiseArgumentNullExceptionWithParameterNameSetIfObjectIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Require.NotNull(null, "param"),
                ex => Assert.AreEqual("param", ex.ParamName)
            );
        }

        #endregion

        #endregion

        #region NotEmpty String

        [Test]
        [Row(null)]
        [Row("")]
        public void NotEmptyShouldRaiseGivenExceptionIfStringIsNullOrEmpty(string value) {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Require.NotEmpty<InvalidOperationException>(value, string.Empty)
            );
        }

        [Test]
        [Row(null)]
        [Row("")]
        public void NotEmptyShouldRaiseGivenExceptionWithCertainMessageIsStringIsNullOrEmpty(string value) {
            ExceptionAssert.Throws(
                () => Require.NotEmpty<Exception>(value, "certain message"),
                "certain message"
            );
        }

        [Test]
        [Row(null)]
        [Row("")]
        public void NotEmptyShouldFormatMessageUsingGivenParametersIsStringIsNullOrEmpty(string value) {
            ExceptionAssert.Throws(
                () => Require.NotEmpty<Exception>(value, "{0} {1}", 1, "pinguin"),
                "1 pinguin"
            );
        }

        [Test]
        public void NotEmptyShouldNotRaiseExceptionIfStringIsNotNullOrEmpty() {
            ExceptionAssert.DoesNotThrow(
                () => Require.NotEmpty<Exception>("valid string", string.Empty)
            );
        }

        #region Default

        [Test]
        public void NotEmptyShouldRaiseArgumentNullExceptionByDefaultIfStringIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Require.NotEmpty((string)null, string.Empty)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentNullExceptionWithParameterNameSetIfStringIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Require.NotEmpty((string)null, "param"),
                ex => Assert.AreEqual("param", ex.ParamName)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentExceptionByDefaultIfStringIsEmpty() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.NotEmpty(string.Empty, string.Empty)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentExceptionWithParameterNameSetIfStringIsEmpty() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.NotEmpty(string.Empty, "param"),
                ex => Assert.AreEqual("param", ex.ParamName)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentExceptionWithCertainMessageIfStringIsEmpty() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.NotEmpty(string.Empty, "param"),
                "String should not be empty.\r\nParameter name: param"
            );
        }

        #endregion

        #endregion

        #region NotEmpty Guid

        private IEnumerable<Guid?> InvalidGuidValues() {
            yield return null;
            yield return Guid.Empty;
        }

        [Test]
        [Factory("InvalidGuidValues", Kind = FactoryKind.Object)]
        public void NotEmptyShouldRaiseGivenExceptionIfGuidIsNullOrEmpty(Guid? value) {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Require.NotEmpty<InvalidOperationException>(value, string.Empty)
            );
        }

        [Test]
        [Factory("InvalidGuidValues", Kind = FactoryKind.Object)]
        public void NotEmptyShouldRaiseGivenExceptionWithCertainMessageIsGuidIsNullOrEmpty(Guid? value) {
            ExceptionAssert.Throws(
                () => Require.NotEmpty<Exception>(value, "certain message"),
                "certain message"
            );
        }

        [Test]
        [Factory("InvalidGuidValues", Kind = FactoryKind.Object)]
        public void NotEmptyShouldFormatMessageUsingGivenParametersIfGuidIsNullOrEmpty(Guid? value) {
            ExceptionAssert.Throws(
                () => Require.NotEmpty<Exception>(value, "{0} {1}", 1, "pinguin"),
                "1 pinguin"
            );
        }

        [Test]
        public void NotEmptyShouldNotRaiseExceptionIfGuidIsNotNullOrEmpty() {
            ExceptionAssert.DoesNotThrow(
                () => Require.NotEmpty<Exception>("valid string", string.Empty)
            );
        }

        #region Default

        [Test]
        public void NotEmptyShouldRaiseArgumentNullExceptionByDefaultIfGuidIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Require.NotEmpty((Guid?)null, string.Empty)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentNullExceptionWithParameterNameSetIfGuidIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Require.NotEmpty((Guid?)null, "param"),
                ex => Assert.AreEqual("param", ex.ParamName)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentExceptionByDefaultIfGuidIsEmpty() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.NotEmpty(Guid.Empty, string.Empty)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentExceptionWithParameterNameSetIfGuidIsEmpty() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.NotEmpty(Guid.Empty, "param"),
                ex => Assert.AreEqual("param", ex.ParamName)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentExceptionWithCertainMessageIfGuidIsEmpty() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.NotEmpty(Guid.Empty, "param"),
                "Guid should not be empty.\r\nParameter name: param"
            );
        }

        #endregion

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
        public void ThatShouldRaiseExceptionWithCertainMessageIfConditionIsFalse() {
            ExceptionAssert.Throws(
                () => Require.That<Exception>(false, "certain message"),
                "certain message"
            );
        }

        [Test]
        public void ThatShouldFormatMessageUsingGivenParametersIfConditionIsFalse() {
            ExceptionAssert.Throws(
                () => Require.That<Exception>(false, "{0} {1}", 1, "pinguin"),
                "1 pinguin"
            );
        }

        [Test]
        public void ThatShouldNotRaiseExceptionIfStringIsNotNullOrEmptyIfConditionIsFalse() {
            ExceptionAssert.DoesNotThrow(
                () => Require.That(true, string.Empty)
            );
        }

        [Test]
        public void ShouldNotThrowInvalidCastException() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.That<ArgumentException>(
                    0 != 0,
                    "Actual count ({0}) does not equal expected count ({1})",
                    0,
                    0
                )
            );
        }

        #region Default

        [Test]
        public void ThatShouldRaiseArgumentExceptionByDefaultIfConditionIsFalse() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.That(false, string.Empty)
            );
        }

        #endregion

        #endregion

        #region NotEmpty Collection

        private IEnumerable<IEnumerable> InvalidCollectionValues() {
            yield return null;
            yield return new int[0];
        }

        [Test]
        [Factory("InvalidCollectionValues", Kind = FactoryKind.Object)]
        public void NotEmptyShouldRaiseGivenExceptionIfCollectionIsInvalid(IEnumerable items) {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Require.NotEmpty<InvalidOperationException>(items, string.Empty)
            );
        }

        [Test]
        [Factory("InvalidCollectionValues", Kind = FactoryKind.Object)]
        public void NotEmptyShouldRaiseGivenExceptionWithCertainMessageIfCollectionIsInvalid(IEnumerable items) {
            ExceptionAssert.Throws(
                () => Require.NotEmpty<Exception>(items, "certain message"),
                "certain message"
            );
        }

        [Test]
        [Factory("InvalidCollectionValues", Kind = FactoryKind.Object)]
        public void NotEmptyShouldFormatMessageUsingGivenParametersIfCollectionIsInvalid(IEnumerable items) {
            ExceptionAssert.Throws(
                () => Require.NotEmpty<Exception>(items, "{0} {1}", 1, "pinguin"),
                "1 pinguin"
            );
        }

        [Test]
        public void NotEmptyShouldNotRaiseExceptionIfCollectionHasElements() {
            ExceptionAssert.DoesNotThrow(
                () => Require.NotEmpty(new[] { 1 }, string.Empty)
            );
        }

        #region Default

        [Test]
        public void NotEmptyShouldRaiseArgumentNullExceptionByDefaultIfCollectionIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Require.NotEmpty((IEnumerable)null, string.Empty)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentNullExceptionWithParameterNameSetIfCollectionIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Require.NotEmpty((IEnumerable)null, "array"),
                ex => Assert.AreEqual("array", ex.ParamName)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentExceptionByDefaultIfCollectionIsEmpty() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.NotEmpty(new object[0], string.Empty)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentExceptionWithParameterNameSetIfCollectionIsEmpty() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.NotEmpty(new object[0], "array"),
                ex => Assert.AreEqual("array", ex.ParamName)
            );
        }

        [Test]
        public void NotEmptyShouldRaiseArgumentExceptionWithCertainMessageIfCollectionIsEmpty() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Require.NotEmpty(new object[0], "array"),
                "Sequence should contain elements.\r\nParameter name: array"
            );
        }

        #endregion

        #endregion

        #region Exception

        [Test]
        public void ExceptionShouldThrowException() {
            ExceptionAssert.Throws(
                () => Require.Exception<InvalidOperationException>(string.Empty)
            );
        }

        [Test]
        public void ExceptionShouldThrowExceptionOfGivenType() {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Require.Exception<InvalidOperationException>(string.Empty)
            );
        }

        [Test]
        public void ExceptionShouldThrowExceptionWithCertainMessage() {
            ExceptionAssert.Throws(
                () => Require.Exception<InvalidOperationException>("certain message"),
                "certain message"
            );
        }

        [Test]
        public void ExceptionShouldFormatMessageUsingGivenParameters() {
            ExceptionAssert.Throws(
                () => Require.Exception<InvalidOperationException>("{0} {1}", 1, "pinguin"),
                "1 pinguin"
            );
        }

        #endregion
    }
}
