using System;

using Azon.Helpers.Constructs;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Constructs.Of.SwitchType {
    [TestFixture]
    public class SwitchTypeTests {
        [Test]
        public void ShouldTriggerCaseThatMatchesType() {
            var called = false;

            Switch.Type("string")
                .When<string>(str => called = true)
                .Otherwise(value => {});

            Assert.IsTrue(called);
        }

        [Test]
        public void ShouldPassValueIntoMatchedCase() {
            Switch.Type("string")
                .When<string>(str => Assert.AreEqual("string", str))
                .Otherwise(value => { });
        }

        [Test]
        public void ShouldTriggerCaseThatMatchesParentType() {
            var called = false;

            Switch.Type("string")
                .When<object>(str => called = true)
                .Otherwise(value => { });

            Assert.IsTrue(called);
        }

        [Test]
        public void ShouldTriggerMostAppropriateCaseIfSeveralMatch() {
            var called = false;

            Switch.Type("string")
                .When<string>(str => called = true)
                .When<object>(obj => { })
                .Otherwise(value => { });

            Assert.IsTrue(called);
        }

        [Test]
        public void ShouldNotTriggerCaseIfMoreAppropriateExists() {
            var called = false;

            Switch.Type("string")
                .When<string>(str => { })
                .When<object>(obj => called = true)
                .Otherwise(value => { });

            Assert.IsFalse(called);
        }

        [Test]
        public void ShouldNotTriggerCaseThatDoesMatchType() {
            var called = false;

            Switch.Type("string")
                .When<int>(number => called = true)
                .Otherwise(value => { });

            Assert.IsFalse(called);
        }

        [Test]
        public void ShouldNotTriggerDefaultIfOtherMatches() {
            var called = false;

            Switch.Type("string")
                .When<string>(str => { })
                .Otherwise(value => called = true);

            Assert.IsFalse(called);
        }

        [Test]
        public void ShouldTriggerDefaultIfNoOtherMatches() {
            var called = false;

            Switch.Type("string")
                .When<int>(number => { })
                .Otherwise(value => called = true);

            Assert.IsTrue(called);
        }

        [Test]
        public void ShouldThrowInvalidOperationExceptionByDefaultIfRequiredWhenNoCasesMatched() {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Switch.Type("string").OtherwiseThrow("{0} message", "sample"),
                "sample message"
            );
        }

        [Test]
        public void ShouldThrowGivenExceptionIfRequiredWhenNoCasesMatched() {
            ExceptionAssert.Throws<InvalidCastException>(
                () => Switch.Type("string").OtherwiseThrow<InvalidCastException>("{0} message", "sample"),
                "sample message"
            );
        }

        [Test]
        public void ShouldThrowIfCasesAreInWrongOrder() {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Switch.Type("string")
                        .When<string>(str => { })
                        .When<int>(number => { })
                        .When<string>(str => { })
                        .Otherwise(obj => { }),
                "Cases must be placed in accord to their generalization and should not duplicate."
            );
        }

        [Test]
        public void ShouldTriggerCaseThatMatchesByGenericType() {
            var called = false;

            Switch.Type<string>()
                .When<string>(() => called = true)
                .Otherwise(() => { });

            Assert.IsTrue(called);
        }

        [Test]
        public void ShouldTriggerCaseOfExactTypeOnlyIfRequired() {
            var called = false;

            Switch.Type<string>(exactType: true)
                .When<object>(() => { })
                .Otherwise(() => called = true);

            Assert.IsTrue(called);
        }

        [Test]
        public void ShouldTriggerCorrectCaseThenGivenInstanceOfType() {
            var called = false;

            Switch.Type(typeof(string))
                .When<string>(() => called = true)
                .Otherwise(() => { });

            Assert.IsTrue(called);
        }
    }
}
