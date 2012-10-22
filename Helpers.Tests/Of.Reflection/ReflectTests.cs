using System;
using System.ComponentModel.DataAnnotations;

using Azon.Helpers.Reflection;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Reflection {
    [TestFixture]
    public class ReflectTests {
        private class Foo {
            public Boo Boo { get; set; }

            public Boo GetBoo() {
                throw new NotImplementedException();
            }
        }

        private class Boo {
            public string Title { get; set; }

            public void Set() { }

            [Display(Name = "Full Name")]
            public string Name { get; set; }

            [Display(Name = "Full Get")]
            public void Get() {}
        }

        [Test]
        public void NameShouldReturnNameOfProperty() {
            Assert.AreEqual("Title", Reflect.Name<Boo>(x => x.Title));
        }

        [Test]
        public void NameShouldReturnNameOfMethod() {
            Assert.AreEqual("Set", Reflect.Name<Boo>(x => x.Set()));
        }

        [Test]
        public void NameShouldReturnNameByPropertyAndProperty() {
            Assert.AreEqual("Title", Reflect.Name<Foo>(x => x.Boo.Title));
        }

        [Test]
        public void NameShouldReturnNameByMethodAndProperty() {
            Assert.AreEqual("Title", Reflect.Name<Foo>(x => x.GetBoo().Title));
        }

        [Test]
        public void DisplayNameShouldRespectDisplayAttributeOnProperty() {
            Assert.AreEqual(
                "Full Name",
                Reflect.DisplayName<Boo>(x => x.Name)
            );
        }

        [Test]
        public void DisplayNameShouldRespectDisplayAttributeOnMethod() {
            Assert.AreEqual(
                "Full Get",
                Reflect.DisplayName<Boo>(x => x.Get())
            );
        }
    }
}
