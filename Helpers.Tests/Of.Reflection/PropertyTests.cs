using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Azon.Helpers.Reflection;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Reflection {
    [TestFixture]
    public class PropertyTests {
        private class Foo {
            public Boo Boo { get; set; }
        }

        private class Boo {
            public string Name { get; set; }
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
    }
}
