using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Azon.Helpers.Extensions;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

using Moq;

namespace Azon.Helpers.Tests.Of.Extensions {
    [TestFixture]
    public class TypeExtensionsTests {
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

        private class Test : IList, IList<string>, ICollection<string>, IEnumerable<string> {
            public IEnumerator<string> GetEnumerator() {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }

            public void Add(string item) {
                throw new NotImplementedException();
            }

            public int Add(object value) {
                throw new NotImplementedException();
            }

            public bool Contains(object value) {
                throw new NotImplementedException();
            }

            public void Clear() {
                throw new NotImplementedException();
            }

            public int IndexOf(object value) {
                throw new NotImplementedException();
            }

            public void Insert(int index, object value) {
                throw new NotImplementedException();
            }

            public void Remove(object value) {
                throw new NotImplementedException();
            }

            public bool Contains(string item) {
                throw new NotImplementedException();
            }

            public void CopyTo(string[] array, int arrayIndex) {
                throw new NotImplementedException();
            }

            public bool Remove(string item) {
                throw new NotImplementedException();
            }

            public void CopyTo(Array array, int index) {
                throw new NotImplementedException();
            }

            public int Count { get; private set; }
            public object SyncRoot { get; private set; }
            public bool IsSynchronized { get; private set; }
            public bool IsReadOnly { get; private set; }
            public bool IsFixedSize { get; private set; }

            public int IndexOf(string item) {
                throw new NotImplementedException();
            }

            public void Insert(int index, string item) {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index) {
                throw new NotImplementedException();
            }

            object IList.this[int index] {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public string this[int index] {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
        }

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
        public void GetHierarchyShouldReturnAllImplementedInterfacesInCorrectOrder() {
            var interfaces = typeof(Test).GetInterfaces(includeSelf: false);

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

        #region Real

        [Test]
        public void ShouldReturnGivenTypeIfItIsNotDynamicallyGenerated() {
            Assert.AreEqual(
                typeof(LambdaExpression), 
                typeof(LambdaExpression).Real()
            );
        }

        [Test]
        public void ShouldReturnFirstNotDynamicallyGeneratedBaseOfGivenType() {
            var value = new Mock<object>().Object;

            Assert.AreEqual(
                typeof(object),
                value.GetType().Real()
            );
        }

        #endregion

        #region GetGenericArgumentsOf

        public class ConcreteList : List<int> {}
        public interface IConcreteList : IList<int> {}

        [Test]
        [Row(typeof(IList<int>), typeof(IList<>))]
        [Row(typeof(List<int>), typeof(List<>))]
        public void ShouldReturnGenericArgumentForTypeDirectlyConstructedFromTemplate(Type type, Type template) {
            Assert.AreEqual(typeof(int), type.GetGenericArgumentsOf(template).Single());
        }

        [Test]
        [Row(typeof(ConcreteList), typeof(List<>))]
        [Row(typeof(IConcreteList), typeof(IList<>))]
        [Row(typeof(ConcreteList), typeof(IList<>))]
        public void ShouldReturnGenericArgumentsForTypeIndirectlyConstructedFromTemplate(Type type, Type template) {
            Assert.AreEqual(typeof(int), type.GetGenericArgumentsOf(template).Single());
        }

        [Test]
        public void ShouldReturnEmptyArrayForTypeNotConstructedFromTemplate() {
            Assert.IsEmpty(typeof(string).GetGenericArgumentsOf(typeof(IList<>)));
        }

        #endregion
    }
}
