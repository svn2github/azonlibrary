using System;
using System.ComponentModel;

using Azon.Helpers.Extensions;
using Azon.Helpers.Reflection;

namespace Azon.Helpers.Events.Bindings {
    internal class Binder {
        public void Apply<TSource, TTarget>(BindingInfo<TSource, TTarget> info) {
            if (info.Mode.HasFlag(BindingMode.OneWay)) {
                var sourceGetter = info.Source.Compile();
                Action<TTarget> targetSetter = value => Property.Set(info.Target, value);

                var dependencies = new DependencyCollector(info.Source).Dependencies;
                new DependencyWatcher(dependencies).BeginWatch();
            }

            if (info.Mode.HasFlag(BindingMode.OneWayToSource)) {
                var targetGetter = info.Target.Compile();
                Action<TSource> sourceSetter = value => Property.Set(info.Source, value);

                var dependencies = new DependencyCollector(info.Source).Dependencies;
            }
        }
    }

    internal class DependencyWatcher {
        public event EventHandler ValueChanged = delegate { };

        private readonly Dependency[] _dependencies;

        public DependencyWatcher(Dependency[] dependencies) {
            _dependencies = dependencies;
            throw new NotImplementedException();
        }

        public void BeginWatch() {
            _dependencies.ForEach(this.BeginObserveDependency);
        }

        private void BeginObserveDependency(Dependency dependency) {
            dependency.Target.PropertyChanged += this.OnDependencyValueChanged(dependency);
            dependency.SubDependencies.ForEach(this.BeginObserveDependency);
        }

        public void EndWatch() {
            _dependencies.ForEach(this.EndObserveDependency);
        }

        private void EndObserveDependency(Dependency dependency) {
            dependency.Target.PropertyChanged -= this.OnDependencyValueChanged(dependency);
            dependency.SubDependencies.ForEach(this.EndObserveDependency);
        }

        private PropertyChangedEventHandler OnDependencyValueChanged(Dependency dependency) {
            return (sender, args) => {
                if (dependency.PropertyName != args.PropertyName)
                    return;

                this.UpdateHandlers(dependency);
                this.ValueChanged(this, EventArgs.Empty);
            };
        }

        private void UpdateHandlers(Dependency dependency) {
            dependency.SubDependencies.ForEach(this.EndObserveDependency);
            dependency.SubDependencies.ForEach(this.BeginObserveDependency);
        }
    }
}