using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Azon.Helpers.Extensions;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Extensions {
    [TestFixture]
    public class TypeExtensionsTests {
        #region IsGenericDefinedAs

        [Test]
        public void IsGenericDefinedAsShouldThrowIfTestedTypeIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => (null as Type).IsGenericDefinedAs(typeof(int)),
                ex => Assert.AreEqual("type", ex.ParamName)
            );
        }

        [Test]
        public void IsGenericDefinedAsShouldThrowIfGivenIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => typeof(int).IsGenericDefinedAs(null),
                ex => Assert.AreEqual("otherType", ex.ParamName)
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

        #endregion

        #region IsAssignableFrom

        [Test]
        public void IsAssignableFromShouldThrowIfTestedTypeIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => (null as Type).IsAssignableFrom<int>(),
                ex => Assert.AreEqual("type", ex.ParamName)
            );
        }

        [Test]
        [Row(typeof(IList<int>))]
        [Row(typeof(IList<string>))]
        [Row(typeof(IList<>))]
        [Row(typeof(IList))]
        [Row(typeof(List<int>))]
        [Row(typeof(List<>))]
        [Row(typeof(IEnumerable<int>))]
        [Row(typeof(IEnumerable<>))]
        [Row(typeof(IEnumerable))]
        [Row(typeof(DateTime))]
        public void IsAssignableFromShouldReturnSameResultAsNonGenericVersion(Type type) {
            Assert.AreEqual(
                type.IsAssignableFrom<IList<int>>(),
                type.IsAssignableFrom(typeof(IList<int>))
            );
        }

        #endregion

        #region GetHierarchy

        [Test]
        public void GetHierarchyShouldThrowIfTestedTypeIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => (null as Type).GetHierarchy().ToArray(),
                ex => Assert.AreEqual("type", ex.ParamName)
            );
        }

        [Test]
        public void GetHierarchyShouldReturnGivenTypeByDefault() {
            Assert.Contains(
                typeof(List<string>).GetHierarchy(),
                typeof(List<string>)
            );
        }

        [Test]
        public void GetHierarchyShouldNotReturnGivenTypeIfNotRequired() {
            Assert.DoesNotContain(
                typeof(List<string>).GetHierarchy(includeSelf: false),
                typeof(List<string>)
            );
        }

        [Test]
        public void GetHierarchyShouldReturnAllInheritedTypesFromChildToRoot() {
            var hierarchy = typeof(Expression<string>).GetHierarchy()
                                                      .SkipWhile(type => type == typeof(Expression<string>));

            Assert.AreElementsEqual(
                new[] {
                    typeof(LambdaExpression),
                    typeof(Expression),
                    typeof(Object)
                },
                hierarchy
            );
        }

        #endregion GetHierarchy

        #region GetInterfaces

        [Test]
        public void GetInterfacesShouldThrowIfTestedTypeIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => (null as Type).GetInterfaces(includeSelf: true).ToArray(),
                ex => Assert.AreEqual("type", ex.ParamName)
            );
        }

        [Test]
        public void GetInterfacesShouldReturnGivenTypeIfInterface() {
            Assert.Contains(
                typeof(IEnumerable).GetInterfaces(includeSelf: true),
                typeof(IEnumerable)
            );
        }

        [Test]
        public void GetInterfacesShouldNotReturnGivenTypeIfNotRequired() {
            Assert.DoesNotContain(
                typeof(IEnumerable).GetInterfaces(includeSelf: false),
                typeof(IEnumerable)
            );
        }

        [Test]
        public void GetHierarchyShouldReturnAllImplementedInterfacesFromChildToRoot() {
            var interfaces = typeof(List<string>).GetInterfaces(includeSelf: false);

            Assert.AreElementsEqual(
                new[] {
                    typeof(IList<string>),
                    typeof(ICollection<string>),
                    typeof(IEnumerable<string>),
                    typeof(IList),
                    typeof(ICollection),
                    typeof(IEnumerable)
                },
                interfaces
            );
        }

        #endregion

        #region Inherits

        [Test]
        public void InheritsShouldThrowIfTestedTypeIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => (null as Type).Inherits(typeof(int)),
                ex => Assert.AreEqual("type", ex.ParamName)
            );
        }

        [Test]
        public void InheritsShouldThrowIfGivenTypeIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => typeof(int).Inherits(null),
                ex => Assert.AreEqual("parentType", ex.ParamName)
            );
        }

        [Test]
        [Row(typeof(string), typeof(object))]
        [Row(typeof(Expression<string>), typeof(LambdaExpression))]
        [Row(typeof(Expression<>), typeof(LambdaExpression))]
        [Row(typeof(Expression<string>), typeof(Expression<>))]
        public void InheritsShouldReturnTrueForInheritedType(Type inheritor, Type inherited) {
            Assert.IsTrue(
                inheritor.Inherits(inherited)
            );
        }

        [Test]
        [Row(typeof(string), typeof(object))]
        [Row(typeof(Expression<string>), typeof(LambdaExpression))]
        [Row(typeof(Expression<>), typeof(LambdaExpression))]
        [Row(typeof(Expression<string>), typeof(Expression<>))]
        public void InheritsShouldReturnFalseForNotInheritedType(Type inherited, Type inheritor) {
            Assert.IsFalse(
                inheritor.Inherits(inherited)
            );
        }

        #endregion

        #region Implements

        [Test]
        public void ImplementsShouldThrowIfTestedTypeIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => (null as Type).Implements(typeof(int)),
                ex => Assert.AreEqual("type", ex.ParamName)
            );
        }

        [Test]
        public void ImplementsShouldThrowIfGivenInterfaceIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => typeof(int).Implements(null),
                ex => Assert.AreEqual("interface", ex.ParamName)
            );
        }

        [Test]
        [Row(typeof(IList), typeof(IEnumerable))]
        [Row(typeof(IList<string>), typeof(IEnumerable))]
        [Row(typeof(IList<string>), typeof(IEnumerable<string>))]
        [Row(typeof(IList<string>), typeof(IEnumerable<>))]
        [Row(typeof(IList<>), typeof(IEnumerable<>))]
        [Row(typeof(List<string>), typeof(IEnumerable))]
        [Row(typeof(List<string>), typeof(IEnumerable<string>))]
        [Row(typeof(List<string>), typeof(IEnumerable<>))]
        [Row(typeof(List<>), typeof(IEnumerable<>))]
        public void ImplementsShouldReturnTrueForImplementedInterface(Type implementor, Type implemented) {
            Assert.IsTrue(
                implementor.Implements(implemented)
            );
        }

        [Test]
        [Row(typeof(IList), typeof(IEnumerable))]
        [Row(typeof(IList<string>), typeof(IEnumerable))]
        [Row(typeof(IList<string>), typeof(IEnumerable<string>))]
        [Row(typeof(IList<string>), typeof(IEnumerable<>))]
        [Row(typeof(IList<>), typeof(IEnumerable<>))]
        [Row(typeof(List<string>), typeof(IEnumerable))]
        [Row(typeof(List<string>), typeof(IEnumerable<string>))]
        [Row(typeof(List<string>), typeof(IEnumerable<>))]
        [Row(typeof(List<>), typeof(IEnumerable<>))]
        public void ImplementsShouldReturnFalseForNotImplementedInterface(Type implemented, Type implementor) {
            Assert.IsFalse(
                implementor.Implements(implemented)
            );
        }

        #endregion
    }
}
