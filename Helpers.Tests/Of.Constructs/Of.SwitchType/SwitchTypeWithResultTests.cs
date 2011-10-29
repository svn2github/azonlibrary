using System;

using Azon.Helpers.Constructs;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Constructs.Of.SwitchType {
    [TestFixture]
    public class SwitchTypeWithResultTests {
        [Test]
        public void ShouldTriggerCaseThatMatchesType() {
            Assert.IsTrue(
                Switch.Type<string, bool>("string")
                    .When<string>(str => true)
                    .Otherwise(value => false)
            );
        }

        [Test]
        public void ShouldPassValueIntoMatchedCase() {
            Switch.Type<string, bool>("string")
                .When<string>(str => {
                    Assert.AreEqual("string", str);
                    return true;
                })
                .Otherwise(false);
        }

        [Test]
        public void ShouldTriggerCaseThatMatchesParentType() {
            Assert.IsTrue(
                Switch.Type<string, bool>("string")
                    .When<object>(true)
                    .Otherwise(false)
            );
        }

        [Test]
        public void ShouldTriggerMostAppropriateCaseIfSeveralMatch() {
            Assert.IsTrue(
                Switch.Type<string, bool>("string")
                    .When<string>(true)
                    .When<object>(false)
                    .Otherwise(false)
            );
        }

        [Test]
        public void ShouldNotTriggerCaseIfMoreAppropriateExists() {
            Assert.IsFalse(
                Switch.Type<string, bool>("string")
                    .When<string>(false)
                    .When<object>(true)
                    .Otherwise(false)
            );
        }

        [Test]
        public void ShouldNotTriggerCaseThatDoesMatchType() {
            Assert.IsFalse(
                Switch.Type<string, bool>("string")
                    .When<int>(true)
                    .Otherwise(false)
            );
        }

        [Test]
        public void ShouldNotTriggerDefaultIfOtherMatches() {
            Assert.IsFalse(
                Switch.Type<string, bool>("string")
                    .When<string>(false)
                    .Otherwise(true)
            );
        }

        [Test]
        public void ShouldTriggerDefaultIfNoOtherMatches() {
            Assert.IsTrue(
                Switch.Type<string, bool>("string")
                    .When<int>(false)
                    .Otherwise(true)
            );
        }

        [Test]
        public void ShouldThrowInvalidOperationExceptionByDefaultIfRequiredWhenNoCasesMatched() {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Switch.Type<string, bool>("string").OtherwiseThrow("{0} message", "sample"),
                "sample message"
            );
        }

        [Test]
        public void ShouldThrowGivenExceptionIfRequiredWhenNoCasesMatched() {
            ExceptionAssert.Throws<InvalidCastException>(
                () => Switch.Type<string, bool>("string").OtherwiseThrow<InvalidCastException>("{0} message", "sample"),
                "sample message"
            );
        }

        [Test]
        public void ShouldThrowIfCasesAreInWrongOrder() {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Switch.Type<string, bool>("string")
                        .When<string>(false)
                        .When<int>(false)
                        .When<string>(false)
                        .Otherwise(false),
                "Cases must be placed in accord to their generalization and should not duplicate."
            );
        }

        [Test]
        public void ShouldTriggerCaseThatMatchesByGenericType() {
            Assert.IsTrue(
                Switch.Type<string, bool>()
                    .When<string>(() => true)
                    .Otherwise(() => false)
            );
        }

        [Test]
        public void ShouldTriggerCaseOfExactTypeOnlyIfRequired() {
            Assert.IsTrue(
                Switch.Type<string, bool>(exactType: true)
                    .When<object>(false)
                    .Otherwise(true)
            );
        }
    }
}
