using System.ComponentModel;
using System.Diagnostics;

using Azon.Helpers.Events;
using Azon.Helpers.Events.Bindings;
using Azon.Helpers.Extensions;
using Azon.Helpers.Tests.Internal.Asserts;

using Gallio.Framework;

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

            private Target _innerTarget;

            public Target InnerTarget {
                get { return this._innerTarget; }
                set {
                    this._innerTarget = value;
                    this.PropertyChanged(this, new PropertyChangedEventArgs("InnerTarget"));
                }
            }
        }

        [Test]
        [Row(BindingMode.OneWay)]
        [Row(BindingMode.TwoWay)]
        public void ShouldApplyBindingToTargetAfterCreation(BindingMode mode) {
            var source = new Source { SourceName = "changed" };
            var target = new Target();

            Bind.Property(() => source.SourceName)
                .In(mode)
                .To(() => target.TargetName);

            Assert.AreEqual("changed", source.SourceName);
            Assert.AreEqual("changed", target.TargetName);
        }

        [Test]
        public void ShouldNotApplyBindingToSourceAfterCreationInTwoWayMode() {
            var calls = 0;
            var source = new Source { SourceName = "changed" };
            var target = new Target();

            source.PropertyChanged += (sender, e) => calls++;

            Bind.Property(() => source.SourceName)
                .To(() => target.TargetName);

            Assert.AreEqual(0, calls);
        }

        [Test]
        [Row(BindingMode.OneWay)]
        [Row(BindingMode.TwoWay)]
        public void ShouldApplyBindingOnceAfterCreation(BindingMode mode) {
            var calls = 0;
            var source = new Source { SourceName = "changed" };
            var target = new Target();

            target.PropertyChanged += (sender, e) => calls++;

            Bind.Property(() => source.SourceName)
                .In(mode)
                .To(() => target.TargetName);

            target.PropertyChanged += (sender, e) => { };

            Assert.AreEqual(1, calls);
        }

        [Test]
        [Row(BindingMode.OneWayToSource)]
        public void ShouldNotApplyBindingAfterCreation(BindingMode mode) {
            var source = new Source();
            var target = new Target { TargetName = "changed" };

            Bind.Property(() => source.SourceName)
                .In(mode)
                .To(() => target.TargetName);

            Assert.AreEqual(null, source.SourceName);
            Assert.AreEqual("changed", target.TargetName);
        }

        /// <summary>
        /// This test will fail if we try to resolve a dependency's target in Dependency constructor.
        /// </summary>
        [Test]
        public void ShouldDelayDependeciesTargetsResolutionUntilBindingApplication() {
            var source = new Source();
            var target = new Target();

            ExceptionAssert.Throws<BindingException>(
                () => Bind.Property(() => source.InnerSource.InnerSource.SourceName)
                          .To(() => target.InnerTarget.InnerTarget.TargetName)
            );
        }

        [Test]
        [Row(BindingMode.OneWay)]
        [Row(BindingMode.TwoWay)]
        public void ShouldThrowWhenImpossibleToApplyBindingIfThrowingOnErrors(BindingMode mode) {
            var source = new Source();
            var target = new Target();

            ExceptionAssert.Throws<BindingException>(
                () => Bind.Property(() => source.SourceName)
                          .ThrowingOnBindingErrors().In(mode)
                          .To(() => target.InnerTarget.TargetName)
            );
        }

        [Test]
        [Row(BindingMode.OneWay)]
        [Row(BindingMode.TwoWay)]
        public void ShouldNotThrowWhenImpossibleToApplyBindingIfSKippingErrors(BindingMode mode) {
            var source = new Source();
            var target = new Target();

            ExceptionAssert.DoesNotThrow(
                () => Bind.Property(() => source.SourceName)
                          .SkippingBindingErrors().In(mode)
                          .To(() => target.InnerTarget.TargetName)
            );
        }

        [Test]
        [Row(BindingMode.OneWay)]
        [Row(BindingMode.TwoWay)]
        public void ShouldThrowWhenImpossibleToSubscribeSourceIfThrowingOnErrors(BindingMode mode) {
            var source = new Source();
            var target = new Target();

            ExceptionAssert.Throws<BindingException>(
                () => Bind.Property(() => source.InnerSource.SourceName)
                          .ThrowingOnBindingErrors().In(mode)
                          .To(() => target.TargetName)
            );
        }

        [Test]
        [Row(BindingMode.OneWay)]
        [Row(BindingMode.TwoWay)]
        public void ShouldNotThrowWhenImpossibleToSubscribeSourceIfSKippingErrors(BindingMode mode) {
            var source = new Source();
            var target = new Target();

            ExceptionAssert.DoesNotThrow(
                () => Bind.Property(() => source.InnerSource.SourceName)
                          .SkippingBindingErrors().In(mode)
                          .To(() => target.TargetName)
            );
        }

        [Test]
        [Row(BindingMode.OneWay)]
        [Row(BindingMode.TwoWay)]
        public void ShouldThrowWhenLinkBecomesBrokenIfThrowingOnErrors(BindingMode mode) {
            var source = new Source { InnerSource = new Source() };
            var target = new Target();

            Bind.Property(() => source.InnerSource.SourceName)
                .ThrowingOnBindingErrors().In(mode)
                .To(() => target.TargetName);

            ExceptionAssert.Throws<BindingException>(() => source.InnerSource = null);
        }

        [Test]
        [Row(BindingMode.OneWay)]
        [Row(BindingMode.TwoWay)]
        public void ShouldNotThrowWheLinkBecomesBrokenIfSKippingErrors(BindingMode mode) {
            var source = new Source { InnerSource = new Source() };
            var target = new Target();

            Bind.Property(() => source.InnerSource.SourceName)
                .SkippingBindingErrors()
                .To(() => target.TargetName);

            ExceptionAssert.DoesNotThrow(() => source.InnerSource = null);
        }

        [Test]
        [Row(BindingMode.OneWay)]
        [Row(BindingMode.TwoWay)]
        public void ShouldChangeTargetAfterLinkIsReestablished(BindingMode mode) {
            var source = new Source();
            var target = new Target();

            Bind.Property(() => source.InnerSource.SourceName)
                .SkippingBindingErrors()
                .To(() => target.TargetName);

            source.InnerSource = new Source();
            source.InnerSource.SourceName = "changed";

            Assert.AreEqual("changed", target.TargetName);
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
        public void ShouldChangeTargetOnceWhenSourceChanged() {
            var calls = 0;
            var source = new Source();
            var target = new Target();

            Bind.Property(() => source.SourceName)
                .To(() => target.TargetName);

            target.PropertyChanged += (sender, e) => calls++;

            source.SourceName = "changed";

            Assert.AreEqual(1, calls);
        }

        [Test]
        public void ShouldNotCyclicallyChangeSourceWhenSourceChanged() {
            var calls = 0;
            var source = new Source();
            var target = new Target();

            Bind.Property(() => source.SourceName)
                .To(() => target.TargetName);

            source.PropertyChanged += (sender, e) => calls++;
            source.SourceName = "changed";

            Assert.AreEqual(1, calls);
        }

        [Test]
        public void ShouldChangeTargetWhenAnyPropertyInPathChanged() {
            var source = new Source { InnerSource = new Source() };
            var target = new Target();

            Bind.Property(() => source.InnerSource.SourceName)
                .To(() => target.TargetName);

            source.InnerSource = new Source { SourceName = "changed" };

            Assert.AreEqual("changed", target.TargetName);
        }

        [Test]
        public void ShouldSubscribeToNewLinkInPath() {
            var source = new Source { InnerSource = new Source() };
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

        #region Performance Tests

        [Test]
        public void ShouldBeOfAcceptablePerformanceInSimpleCase() {
            var source = new Source();
            var target = new Target();

            Bind.Property(() => source.SourceName)
                .To(() => target.TargetName);

            var iteration = 0;
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < 1.Seconds()) {
                iteration++;
                source.SourceName = string.Empty;
            }

            Assert.GreaterThanOrEqualTo(iteration, 150000);
        }

        [Test]
        public void ShouldBeOfAcceptablePerformanceInAdvancedCase() {
            var source = new Source { InnerSource = new Source { InnerSource = new Source() } };
            var target = new Target();

            Bind.Property(() => source.InnerSource.InnerSource.SourceName)
                .To(() => target.TargetName);

            var iteration = 0;
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < 1.Seconds()) {
                iteration++;
                source.InnerSource = new Source { InnerSource = new Source() };
            }

            Assert.GreaterThanOrEqualTo(iteration, 100000);
        }

        [Test]
        public void ShouldBeOfAcceptablePerformanceWhenBindingCausesErrors() {
            var source = new Source();
            var target = new Target();

            Bind.Property(() => source.InnerSource.SourceName)
                .SkippingBindingErrors()
                .To(() => target.TargetName);

            var iteration = 0;
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < 1.Seconds()) {
                iteration++;
                source.InnerSource = null;
            }

            Assert.GreaterThanOrEqualTo(iteration, 50000);
        }

        #endregion
    }
}
