using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Azon.Helpers.Generators.ValueGenerators;
using Azon.Helpers.Generators.ValueGenerators.Constraints;
using Azon.Helpers.Reflection;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators {
    internal class ValueGeneratorContract<T> : AbstractContract 
        where T : IValueGenerator, new()
    {
        protected T Generator {
            get { return new T(); }
        }

        public Type[] SupportedTypes { get; set; }
        public Type[] SampleTypes { get; set; }
        public bool SkipUnsupportedTypeTest { get; set; }

        protected override IEnumerable<Test> GetContractVerificationTests() {
            foreach (var type in this.SupportedTypes) {
                var supportedType = type;

                yield return new TestCase(
                    string.Format("ShouldDeclareType{0}AsSupported", supportedType.Name),
                    () => Assert.Contains(this.Generator.ForTypes, supportedType)
                );
            }

            foreach (var type in SampleTypes) {
                var sampleType = type;

                yield return new TestCase(
                    string.Format("ShouldReturnValueOf{0}", sampleType.Name),
                    () => Assert.IsInstanceOfType(sampleType, this.Generator.GetRandomValue(sampleType, new IConstraint[0]))
                );
            }

            yield return this.CreateTest(() => this.ShouldThrowArgumentNullExceptionIfGivenTypeIsNull());

            if (!this.SkipUnsupportedTypeTest)
                yield return this.CreateTest(() => this.ShouldThrowIfGivenTypeIsNotSupported());
        }

        private Test CreateTest(Expression<Action> expression) {
            var testName = Reflect.Name(expression);
            var testMethod = expression.Compile();

            return new TestCase(testName, () => testMethod());
        }

        private void ShouldThrowArgumentNullExceptionIfGivenTypeIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => this.Generator.GetRandomValue(null, new IConstraint[0]),
                ex => Assert.AreEqual("type", ex.ParamName)
            );
        }

        private void ShouldThrowIfGivenTypeIsNotSupported() {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => this.Generator.GetRandomValue(typeof(Exception), new IConstraint[0]),
                string.Format(
                    "{0} does not support generation of System.Exception.",
                    this.Generator.GetType()
                )
            );
        }
    }
}
