using System;
using System.ComponentModel;

using Azon.Helpers.Extensions;

namespace Azon.Helpers.Events.Bindings {
    internal class DependencyWatcher {
        public event EventHandler ValueChanged = delegate { };

        private readonly Dependency[] _dependencies;

        public DependencyWatcher(Dependency[] dependencies) {
            this._dependencies = dependencies;
        }

        public void BeginWatch() {
            this._dependencies.ForEach(this.BeginObserveDependency);
        }

        private void BeginObserveDependency(Dependency dependency) {
            dependency.Handler = this.OnDependencyValueChanged(dependency);
            dependency.Target.PropertyChanged += dependency.Handler;
            dependency.SubDependencies.ForEach(this.BeginObserveDependency);
        }

        public void EndWatch() {
            this._dependencies.ForEach(this.EndObserveDependency);
        }

        private void EndObserveDependency(Dependency dependency) {
            dependency.Target.PropertyChanged -= dependency.Handler;
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
            dependency.SubDependencies.ForEach(this.UpdateTarget);
            dependency.SubDependencies.ForEach(this.BeginObserveDependency);
        }

        private void UpdateTarget(Dependency dependency) {
            dependency.UpdateTarget();
            dependency.SubDependencies.ForEach(this.UpdateTarget);
        } 
    }
}