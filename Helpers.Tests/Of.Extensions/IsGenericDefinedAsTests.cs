using System;
using System.Collections.Generic;

using Azon.Helpers.Extensions;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Extensions {
    [TestFixture]
    public class IsGenericDefinedAsTests {
        #region IsGenericDefinedAs

        public class ConcreteList : List<int> { }
        public interface IConcreteList : IList<int> { }

        [Test]
        public void ShouldThrowIfTestedTypeIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => (null as Type).IsGenericDefinedAs(typeof(int)),
                ex => Assert.AreEqual("type", ex.ParamName)
            );
        }

        [Test]
        public void ShouldThrowIfTemplateIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => typeof(int).IsGenericDefinedAs(null),
                ex => Assert.AreEqual("template", ex.ParamName)
            );
        }

        [Test]
        public void ShouldReturnFalseIfTestedTypeIsNotGeneric() {
            Assert.IsFalse(
                typeof(int).IsGenericDefinedAs(typeof(IEnumerable<>))
            );
        }

        [Test]
        public void ShouldReturnFalseIfTemplateIsNotGeneric() {
            Assert.IsFalse(
                typeof(int?).IsGenericDefinedAs(typeof(int))
            );
        }

        [Test]
        public void ShouldReturnFalseIfTestedTypeIsNotConstructedFromTemplate() {
            Assert.IsFalse(
                typeof(int?).IsGenericDefinedAs(typeof(IEnumerable<>))
            );
        }

        [Test]
        [Row(typeof(List<int>), typeof(List<>))]
        [Row(typeof(IList<int>), typeof(IList<>))]
        public void ShouldReturnTrueIfTypeIsConstructedFromTemplate(Type type, Type template) {
            Assert.IsTrue(type.IsGenericDefinedAs(template));
        }

        [Test]
        [Row(typeof(IConcreteList), typeof(IList<>))]
        [Row(typeof(ConcreteList), typeof(List<>))]
        [Row(typeof(ConcreteList), typeof(IList<>))]
        public void ShouldReturnTrueForTypeIndirectlyConstructedFromTemplateWhenRecursiveIsTrue(
            Type type,
            Type template
        ) {
            Assert.IsTrue(
                type.IsGenericDefinedAs(template, recursive: true)
            );
        }

        [Test]
        [Row(typeof(IConcreteList), typeof(IList<>))]
        [Row(typeof(ConcreteList), typeof(List<>))]
        [Row(typeof(ConcreteList), typeof(IList<>))]
        public void ShouldReturnFalseForTypeIndirectlyConstructedFromTemplateWhenRecursiveIsFalse(
            Type type,
            Type template
        ) {
            Assert.IsTrue(
                type.IsGenericDefinedAs(template, recursive: false)
            );
        }


        #endregion
    }
}
