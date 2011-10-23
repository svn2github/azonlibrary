using System;
using System.Collections.Generic;
using System.ComponentModel;

using Azon.Helpers.Reflection;

using MbUnit.Framework;

using Moq;

namespace Azon.Helpers.Tests.Of.Reflection {
    [TestFixture]
    public class NotifiablePropertyTests : BaseTestFixture {
        public class Foo : INotifyPropertyChanged, INotifyPropertyChanging {
            public event PropertyChangedEventHandler PropertyChanged;
            public event PropertyChangingEventHandler PropertyChanging;

            public int Age { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void ShouldReturnDefaultIfNoneStored() {
            var foo = new Foo();
            var value = NotifiableProperty.Get(foo, x => x.Age);

            Assert.AreEqual(0, value);
        }

        [Test]
        public void ShouldStoreValueForProperty() {
            var foo = new Foo();

            NotifiableProperty.Set(foo, x => x.Name, "new name");
            var value = NotifiableProperty.Get(foo, x => x.Name);

            Assert.AreEqual("new name", value);
        }

        [Test]
        public void ShouldDistinguishDifferentTargets() {
            var foo1 = new Foo();
            var foo2 = new Foo();

            NotifiableProperty.Set(foo1, x => x.Name, "new name");
            NotifiableProperty.Set(foo2, x => x.Name, "old name");

            var value1 = NotifiableProperty.Get(foo1, x => x.Name);
            var value2 = NotifiableProperty.Get(foo2, x => x.Name);

            Assert.AreEqual("new name", value1);
            Assert.AreEqual("old name", value2);
        }

        [Test]
        public void ShouldRaisePropertyChangedEvent() {
            var mock = this.Mocked<IDisposable>();
            var foo = new Foo();

            foo.PropertyChanged += (sender, e) => mock.Dispose();
            NotifiableProperty.Set(foo, x => x.Name, "new name");

            Mock.Get(mock).Verify(d => d.Dispose(), Times.Once());
        }

        [Test]
        public void ShouldSetCorrentEventSenderOnChanged() {
            var foo = new Foo();
            foo.PropertyChanged += (sender, e) => Assert.AreSame(foo, sender);
            NotifiableProperty.Set(foo, x => x.Name, "new name");
        }

        [Test]
        public void ShouldSetCorrentEventArgsOnChanged() {
            var foo = new Foo();
            foo.PropertyChanged += (sender, e) => Assert.AreEqual("Name", e.PropertyName);
            NotifiableProperty.Set(foo, x => x.Name, "new name");
        }

        [Test]
        public void ShouldRaisePropertyChangingEvent() {
            var mock = this.Mocked<IDisposable>();
            var foo = new Foo();

            foo.PropertyChanging += (sender, e) => mock.Dispose();
            NotifiableProperty.Set(foo, x => x.Name, "new name");

            Mock.Get(mock).Verify(d => d.Dispose(), Times.Once());
        }

        [Test]
        public void ShouldSetCorrentEventSenderOnChanging() {
            var foo = new Foo();
            foo.PropertyChanging += (sender, e) => Assert.AreSame(foo, sender);
            NotifiableProperty.Set(foo, x => x.Name, "new name");
        }

        [Test]
        public void ShouldSetCorrentEventArgsOnChanging() {
            var foo = new Foo();
            foo.PropertyChanging += (sender, e) => Assert.AreEqual("Name", e.PropertyName);
            NotifiableProperty.Set(foo, x => x.Name, "new name");
        }

        [Test]
        public void ShouldRaiseEventsInCorrectOrder() {
            var callstack = new List<string>();
            var foo = new Foo();

            foo.PropertyChanging += (sender, e) => callstack.Add("Changing");
            foo.PropertyChanged += (sender, e) => callstack.Add("Changed");

            NotifiableProperty.Set(foo, x => x.Name, "new name");

            Assert.AreElementsSame(
                new[] { "Changing", "Changed" },
                callstack
            );
        }
    }
}
