using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using Azon.Helpers.Events.Bindings;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Events.Of.Bindings {
    [TestFixture]
    public class DependencyCollectorTests {
        public class Source : INotifyPropertyChanged {
            public event PropertyChangedEventHandler PropertyChanged;

            public Source Inner { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void ShouldCollectNotifiableRootAsDependency() {
            var source = new Source();
            var expression = (Expression<Func<string>>)(() => source.Name);
            var root = new DependencyCollector(expression).Dependencies.Single();

            Assert.AreEqual(source, root.Target);
            Assert.AreEqual("Name", root.PropertyName);
            Assert.IsEmpty(root.SubDependencies);
        }

        [Test]
        public void ShouldCollectLinksAsDependenciesIfNotifiable() {
            var source = new Source();
            var expression = (Expression<Func<string>>)(() => source.Inner.Name);
            var root = new DependencyCollector(expression).Dependencies.Single();

            Assert.AreEqual("Inner", root.PropertyName);
            Assert.AreEqual(source, root.Target);

            var subDependency = root.SubDependencies.Single();
            Assert.AreEqual("Name", subDependency.PropertyName);
            Assert.AreEqual(null, subDependency.Target);
        }
    }
}
