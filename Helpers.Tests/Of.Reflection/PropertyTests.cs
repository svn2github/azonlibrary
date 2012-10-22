using System;
using System.Linq.Expressions;

using Azon.Helpers.Reflection;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Reflection {
    [TestFixture]
    public class PropertyTests {
        private class Foo {
            public Boo Boo { get; set; }
        }

        private class Boo {
            public string Name { get; set; }
            public bool Trigger { get; set; }
        }

        [Test]
        public void HasShouldReturnTrueIfPropertyExists() {
            var item = new { Name = "me" };

            Assert.IsTrue(Property.Has(item, "Name"));
        }

        [Test]
        public void HasShouldReturnTrueIfPropertyExistsForPath() {
            var item = new { Inner = new { Name = "me" } };

            Assert.IsTrue(Property.Has(item, "Inner.Name"));
        }

        [Test]
        public void HasShouldReturnFalseIfPropertyDoesNotExist() {
            Assert.IsFalse(Property.Has(new object(), "Name"));
        }

        [Test]
        public void GetShouldReturnValueForSimpleProperty() {
            var foo = new Foo { Boo = new Boo() };

            Assert.AreSame(
                foo.Boo, 
                Property.Get(foo, "Boo")
            );
        }

        [Test]
        public void GetShouldReturnValueForExistingPath() {
            var foo = new Foo {
                Boo = new Boo { Name = "me" }
            };

            Assert.AreEqual("me", Property.Get(foo, "Boo.Name"));
        }

        [Test]
        public void GetShouldThrowInvalidOperationIfPathDoesNotExist() {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Property.Get(new object(), "Name")
            );
        }

        [Test]
        public void GetShouldThrowNullReferenceIfPathIsChainedToNull() {
            ExceptionAssert.Throws<NullReferenceException>(
                () => Property.Get(new Foo(), "Boo.Name")
            );
        }

        [Test]
        public void GetOrDefaultShouldReturnDefaultIfPathDoesNotExist() {
            var @default = new object();

            Assert.AreSame(
                @default,
                Property.GetOrDefault(new object(), "Name", @default)
            );
        }

        [Test]
        public void GetOrDefaultShouldReturnDefaultIfPathIsChainedToNull() {
            var @default = new object();

            Assert.AreSame(
                @default,
                Property.GetOrDefault(new Foo(), "Boo.Name", @default)
            );
        }

        [Test]
        public void ShouldSetProperty() {
            var foo = new Foo();
            var boo = new Boo();

            Property.Set(() => foo.Boo, boo);

            Assert.AreSame(boo, foo.Boo);
        }

        [Test]
        public void ShouldSetPropertyOnInnerProperty() {
            var foo = new Foo { Boo = new Boo() };

            Property.Set(() => foo.Boo.Name, "changed");

            Assert.AreSame("changed", foo.Boo.Name);
        }

        [Test]
        public void HierarchyShouldNotFailWhenEncounterUnaryExpression() {
            Expression<Func<Boo, object>> reference = b => b.Trigger;
            var result = Property.Hierarchy(reference);

            Assert.AreEqual("Trigger", result.Name);
        }
    }
}
