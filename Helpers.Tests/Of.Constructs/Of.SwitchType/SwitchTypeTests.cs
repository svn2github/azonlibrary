using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Azon.Helpers.Constructs;
using Azon.Helpers.Constructs.SwitchType;
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

        public IEnumerable<Func<ISwitchType, ISwitchType>> WronglyOrderedCases() {
            yield return sw => sw.When<string>(() => { })
                                 .When<string>(() => { });

            yield return sw => sw.When<object>(() => { })
                                 .When<string>(() => { });

            yield return sw => sw.When<IEnumerable>(() => { })
                                 .When<IList<int>>(() => { });

            yield return sw => sw.When<IEnumerable<int>>(() => { })
                                 .When<List<int>>(() => { });

            yield return sw => sw.WhenOpen(typeof(ICollection<>), args => { })
                                 .WhenOpen(typeof(IList<>), args => { });

            yield return sw => sw.WhenOpen(typeof(ICollection<>), args => { })
                                 .WhenOpen(typeof(List<>), args => { });

            yield return sw => sw.When<IEnumerable>(() => { })
                                 .WhenOpen(typeof(IList<>), args => { });

            yield return sw => sw.When<IEnumerable>(() => { })
                                 .WhenOpen(typeof(List<>), args => { });

            yield return sw => sw.WhenOpen(typeof(IEnumerable<>), args => { })
                                 .When<IList<int>>(() => { });

            yield return sw => sw.WhenOpen(typeof(IEnumerable<>), args => { })
                                 .When<List<int>>(() => { });
        }
            
        [Test]
        [Factory("WronglyOrderedCases")]
        public void ShouldThrowIfCasesAreInWrongOrder(Func<ISwitchType, ISwitchType> applyCases) {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => applyCases(Switch.Type("string")).Otherwise(() => { }),
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
        public void ShouldNotTriggerCaseMatchedByParentTypeIfExactTypeSpecified() {
            var called = false;

            Switch.Type<string>(exactType: true)
                .When<object>(() => called = true)
                .Otherwise(() => { });

            Assert.IsFalse(called);
        }

        [Test]
        public void ShouldTriggerCorrectCaseThenGivenInstanceOfType() {
            var called = false;

            Switch.Type(typeof(string))
                .When<string>(() => called = true)
                .Otherwise(() => { });

            Assert.IsTrue(called);
        }

        [Test]
        [Row(typeof(ICollection<string>))]
        [Row(typeof(IList<string>))]
        [Row(typeof(List<string>))]
        public void ShouldTriggerGenericCaseThatMatchesType(Type type) {
            var called = false;

            Switch.Type(type)
                .WhenOpen(typeof(ICollection<>), args => called = true)
                .Otherwise(() => { });

            Assert.IsTrue(called);
        }

        [Test]
        public void ShouldPassGenericParametersIntoMatchedOpenCase() {
            Switch.Type<IList<string>>()
                .WhenOpen(typeof(ICollection<>), args => Assert.AreEqual(typeof(string), args.Single()))
                .Otherwise(() => { });
        }

        [Test]
        public void ShouldNotTriggerOpenCaseMatchedByParentTypeIfExactTypeSpecified() {
            var called = false;

            Switch.Type(typeof(IList<string>), exactType: true)
                .WhenOpen(typeof(IEnumerable<>), args => called = true)
                .Otherwise(() => { });

            Assert.IsFalse(called);
        }

        [Test]
        public void WhenGenericShouldThrowIfGivenTypeIsNotOpenGeneric() {
            ExceptionAssert.Throws<ArgumentException>(
                () => Switch.Type(typeof(IList<string>))
                            .WhenOpen(typeof(int), args => { })
                            .Otherwise(() => { }),
                "Type System.Int32 is not an open generic type.\r\nParameter name: type"
            );
        }
    }
}
