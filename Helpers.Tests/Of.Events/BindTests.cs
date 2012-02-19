using System.ComponentModel;

using Azon.Helpers.Events;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Events {
    [TestFixture]
    public class BindTests {
        private class Source : INotifyPropertyChanged {
            public event PropertyChangedEventHandler PropertyChanged = delegate { }; 

            private string _name;

            public string SourceName {
                get { return this._name; }
                set {
                    this._name = value;
                    this.PropertyChanged(this, new PropertyChangedEventArgs("SourceName"));
                }
            }

            private Source _innerSource;

            public Source InnerSource {
                get { return this._innerSource; }
                set {
                    this._innerSource = value;
                    this.PropertyChanged(this, new PropertyChangedEventArgs("InnerSource"));
                }
            }
        }

        private class Target : INotifyPropertyChanged {
            public event PropertyChangedEventHandler PropertyChanged = delegate { };

            private string _name;

            public string TargetName {
                get { return this._name; }
                set {
                    this._name = value;
                    this.PropertyChanged(this, new PropertyChangedEventArgs("TargetName"));
                }
            }
        }

        [Test]
        public void ShouldChangeTargetWhenSourceChanged() {
            var source = new Source();
            var target = new Target();

            Bind.Property(() => source.SourceName)
                .To(() => target.TargetName);

            source.SourceName = "changed";

            Assert.AreEqual("changed", target.TargetName);
        }

        [Test]
        public void ShouldChangeTargetWhenAnyPropertyInPathChanged() {
            var source = new Source();
            var target = new Target();

            Bind.Property(() => source.InnerSource.SourceName)
                .To(() => target.TargetName);

            source.InnerSource = new Source { SourceName = "changed" };

            Assert.AreEqual("changed", target.TargetName);
        }

        [Test]
        public void ShouldSubscribeToNewLinkInPath() {
            var source = new Source();
            var target = new Target();

            Bind.Property(() => source.InnerSource.SourceName)
                .To(() => target.TargetName);

            source.InnerSource = new Source { SourceName = "not this" };
            source.InnerSource.SourceName = "changed";

            Assert.AreEqual("changed", target.TargetName);
        }

        [Test]
        public void ShouldUnsubscribeFromBrokenLinkInPath() {
            var source = new Source { InnerSource = new Source() };
            var brokenLink = source.InnerSource;
            var target = new Target();

            Bind.Property(() => source.InnerSource.SourceName)
                .To(() => target.TargetName);

            source.InnerSource = new Source { SourceName = "changed" };
            brokenLink.SourceName = "not this";

            Assert.AreEqual("changed", target.TargetName);
        }
    }
}
