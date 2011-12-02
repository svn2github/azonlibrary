using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;

using Azon.Helpers.Constructs;
using Azon.Helpers.Constructs.SwitchType;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Constructs.Of.SwitchType {
    [TestFixture]
    public class SwitchTypeWithResultTests {
        public class Data {
            public Type TestedType { get; private set; }
            public Func<ISwitchTypeWithResult<bool>, ISwitchTypeWithResult<bool>> ApplyCases { get; private set; }

            public Data(
                Type testedType,
                Func<ISwitchTypeWithResult<bool>, ISwitchTypeWithResult<bool>> applyCases
            ) {
                this.TestedType = testedType;
                this.ApplyCases = applyCases;
            }
        }

        public IEnumerable<Data> SingleCases() {
            yield return new Data(typeof(string), sw => sw.When<string>(true));
            yield return new Data(typeof(string), sw => sw.When<object>(true));

            yield return new Data(typeof(List<string>), sw => sw.When<IList<string>>(true));
            yield return new Data(typeof(List<string>), sw => sw.When<IList>(true));
            yield return new Data(typeof(List<string>), sw => sw.WhenOpen(typeof(IList<>), true));
            yield return new Data(typeof(List<string>), sw => sw.WhenOpen(typeof(IEnumerable<>), true));

            yield return new Data(typeof(IList<string>), sw => sw.When<IList<string>>(true));
            yield return new Data(typeof(IList<string>), sw => sw.When<IEnumerable<string>>(true));
            yield return new Data(typeof(IList<string>), sw => sw.When<IEnumerable>(true));
            yield return new Data(typeof(IList<string>), sw => sw.WhenOpen(typeof(IList<>), true));
            yield return new Data(typeof(IList<string>), sw => sw.WhenOpen(typeof(IEnumerable<>), true));

            yield return new Data(typeof(Expression<string>), sw => sw.When<Expression<string>>(true));
            yield return new Data(typeof(Expression<string>), sw => sw.When<LambdaExpression>(true));
            yield return new Data(typeof(Expression<string>), sw => sw.When<Expression>(true));
            yield return new Data(typeof(Expression<string>), sw => sw.WhenOpen(typeof(Expression<>), true));
        }

        [Test]
        [Factory("SingleCases")]
        public void ShouldTriggerCaseThatMatchesType(Data data) {
            Assert.IsTrue(
                data.ApplyCases(Switch.Type<bool>(data.TestedType)).Otherwise(false)
            );
        }

        [Test]
        public void ShouldTriggerCaseBasingOfActualTypeInsteadOfDeclared() {
            var exception = new FaultException<string>("string");
            var result = Switch
                .Type<object, bool>(exception)
                    .WhenOpen(typeof(FaultException<>), true)
                    .Otherwise(false);

            Assert.IsTrue(result);
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
        public void ShouldPassGenericParametersIntoMatchedOpenCase() {
            Switch.Type<IList<string>, bool>()
                .WhenOpen(typeof(ICollection<>), args => {
                    Assert.AreEqual(typeof(string), args.Single());
                    return true;
                })
                .Otherwise(false);
        }

        public IEnumerable<Data> UnmatchedCases() {
            yield return new Data(typeof(string), sw => sw.When<int>(true));
            yield return new Data(typeof(string), sw => sw.When<string>(false)
                                                          .When<object>(true));

            yield return new Data(typeof(List<string>), sw => sw.When<IList<string>>(false)
                                                                .When<IList>(true));
            yield return new Data(typeof(List<string>), sw => sw.When<IList<string>>(false)
                                                                .When<IEnumerable<string>>(true));
            yield return new Data(typeof(List<string>), sw => sw.When<IList<string>>(false)
                                                                .WhenOpen(typeof(IList<>), true));
            yield return new Data(typeof(List<string>), sw => sw.WhenOpen(typeof(IList<>), false)
                                                                .WhenOpen(typeof(IEnumerable<>), true));

            yield return new Data(typeof(IList<string>), sw => sw.When<IList<string>>(false)
                                                                 .When<IList>(true));
            yield return new Data(typeof(IList<string>), sw => sw.When<IList<string>>(false)
                                                                 .When<IEnumerable<string>>(true));
            yield return new Data(typeof(IList<string>), sw => sw.When<IList<string>>(false)
                                                                 .WhenOpen(typeof(IList<>), true));
            yield return new Data(typeof(IList<string>), sw => sw.WhenOpen(typeof(IList<>), false)
                                                                 .WhenOpen(typeof(IEnumerable<>), true));

            yield return new Data(typeof(Expression<string>), sw => sw.When<Expression<string>>(false)
                                                                      .When<LambdaExpression>(true));
            yield return new Data(typeof(Expression<string>), sw => sw.WhenOpen(typeof(Expression<>), false)
                                                                      .When<LambdaExpression>(true));
        }

        [Test]
        [Factory("UnmatchedCases")]
        public void ShouldNotTriggerCaseIfUnappropriate(Data data) {
            Assert.IsFalse(
                data.ApplyCases(Switch.Type<bool>(data.TestedType)).Otherwise(false)
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
                Switch.Type<string, bool>("string").Otherwise(true)
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

        public IEnumerable<Func<ISwitchTypeWithResult<bool>, ISwitchTypeWithResult<bool>>> WronglyOrderedCases() {
            yield return sw => sw.When<string>(false)
                                 .When<string>(false);

            yield return sw => sw.When<object>(false)
                                 .When<string>(false);

            yield return sw => sw.When<IEnumerable>(false)
                                 .When<IList<int>>(false);

            yield return sw => sw.When<IEnumerable<int>>(false)
                                 .When<List<int>>(false);

            yield return sw => sw.WhenOpen(typeof(ICollection<>), false)
                                 .WhenOpen(typeof(IList<>), false);

            yield return sw => sw.WhenOpen(typeof(ICollection<>), false)
                                 .WhenOpen(typeof(List<>), false);

            yield return sw => sw.When<IEnumerable>(false)
                                 .WhenOpen(typeof(IList<>), false);

            yield return sw => sw.When<IEnumerable>(false)
                                 .WhenOpen(typeof(List<>), false);

            yield return sw => sw.WhenOpen(typeof(IEnumerable<>), false)
                                 .When<IList<int>>(false);

            yield return sw => sw.WhenOpen(typeof(IEnumerable<>), false)
                                 .When<List<int>>(false);
        }

        [Test]
        [Factory("WronglyOrderedCases")]
        public void ShouldThrowIfCasesAreInWrongOrder(
            Func<ISwitchTypeWithResult<bool>, ISwitchTypeWithResult<bool>> applyCases
        ) {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => applyCases(Switch.Type<string, bool>("string")).Otherwise(false),
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

        [Test]
        public void ShouldTriggerCorrectCaseThenGivenInstanceOfType() {
            Assert.IsTrue(
                Switch.Type<bool>(typeof(string))
                    .When<string>(true)
                    .Otherwise(false)
            );
        }
    }
}
