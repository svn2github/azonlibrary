using System;
using System.ComponentModel;

using Azon.Helpers.Events;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Events {
    [TestFixture]
    public class SubscribeTests : BaseTestFixture {
        public class Foo : INotifyPropertyChanged {
            private string _name;
            private string _date;

            public event PropertyChangedEventHandler PropertyChanged;

            public string Name {
                get { return this._name; }
                set {
                    this._name = value;
                    OnPropertyChanged("Name");
                }
            }

            public string Date {
                get { return this._date; }
                set {
                    this._date = value;
                    OnPropertyChanged("Date");
                }
            }

            private void OnPropertyChanged(string propertyName) {
                if (this.PropertyChanged == null)
                    return;

                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [Test]
        public void ShouldCallHandlerOnPropertyChange() {
            var called = false;
            var foo = new Foo();

            Subscribe.When.Object(foo)
                     .HasChanged(x => x.Name)
                     .Call(() => called = true);

            foo.Name = "new name";

            Assert.IsTrue(called);
        }

        [Test]
        public void ShouldUnsubscribeFromProperty() {
            var foo = new Foo();
            var subscription = Subscribe.When
                                        .Object(foo)
                                        .HasChanged(x => x.Name)
                                        .Call(Assert.Fail);

            subscription.Unsubscribe();

            foo.Name = "new name";
        }

        [Test]
        public void ShouldPassSenderIntoHandlerIfDesired() {
            var foo = new Foo();

            Subscribe.When.Object(foo)
                     .HasChanged(x => x.Name)
                     .Call(sender => Assert.AreSame(foo, sender));

            foo.Name = "new name";
        }

        [Test]
        public void ShouldPassEventArgsIntoHandlerIfDesired() {
            var foo = new Foo();

            Subscribe.When.Object(foo)
                     .HasChanged(x => x.Name)
                     .Call((sender, args) => Assert.IsNotNull(args));

            foo.Name = "new name";
        }

        [Test]
        public void ShouldAllowSubscriptionOntoSeveralProperties() {
            var nameCalled = false;
            var dateCalled = false;
            var foo = new Foo();

            Subscribe.When
                .Object(foo)
                     .HasChanged(x => x.Name).Call(() => nameCalled = true)
                     .HasChanged(x => x.Date).Call(() => dateCalled = true);

            foo.Name = "new name";
            foo.Date = "new date";

            Assert.IsTrue(nameCalled);
            Assert.IsTrue(dateCalled);
        }

        [Test]
        public void ShouldAllowSubscriptionOntoSeveralObjects() {
            var nameCalled = false;
            var dateCalled = false;
            var foo1 = new Foo();
            var foo2 = new Foo();

            Subscribe.When
                .Object(foo1)
                     .HasChanged(x => x.Name).Call(() => nameCalled = true)
                .Object(foo2)
                     .HasChanged(x => x.Date).Call(() => dateCalled = true);

            foo1.Name = "new name";
            foo2.Date = "new date";

            Assert.IsTrue(nameCalled);
            Assert.IsTrue(dateCalled);
        }

        [Test]
        public void ShouldUnsubscribeFromAllProperties() {
            var foo = new Foo();
            var subscription = Subscribe.When
                .Object(foo)
                     .HasChanged(x => x.Name).Call(Assert.Fail)
                     .HasChanged(x => x.Date).Call(Assert.Fail);

            subscription.Unsubscribe();

            foo.Name = "new name";
            foo.Date = "new date";
        }
        
        [Test]
        public void ShouldUnsubscribeFromAllObjects() {
            var foo1 = new Foo();
            var foo2 = new Foo();

            var subscription = Subscribe.When
                .Object(foo1)
                     .HasChanged(x => x.Name).Call(Assert.Fail)
                .Object(foo2)
                     .HasChanged(x => x.Date).Call(Assert.Fail);

            subscription.Unsubscribe();

            foo1.Name = "new name";
            foo2.Date = "new date";
        }

        [Test]
        public void ShouldUnsubscribeFromCollectedObjects() {
            var foo = new Foo();
            var weak = new WeakReference(
                this.Mocked<IDisposable>(
                    m => m.Setup(d => d.Dispose()).Callback(Assert.Fail)
                )
            );

            Subscribe.When
                     .Object(foo)
                     .HasChanged(x => x.Name)
                     .Call(((IDisposable)weak.Target).Dispose);

            GC.Collect();
            GC.WaitForFullGCComplete();

            foo.Name = "new name";
        }
    }
}
