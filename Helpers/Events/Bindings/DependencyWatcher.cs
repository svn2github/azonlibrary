using System;
using System.ComponentModel;
using System.Diagnostics;

using Azon.Helpers.Asserts;
using Azon.Helpers.Extensions;

namespace Azon.Helpers.Events.Bindings {
    internal class DependencyWatcher {
        public event EventHandler ValueChanged = delegate { };

        private readonly Dependency[] _dependencies;
        private readonly BindingMode _mode;
        private readonly BindindErrorOptions _errorOptions;

        public DependencyWatcher(
            Dependency[] dependencies, 
            BindingMode mode,
            BindindErrorOptions errorOptions
        ) {
            _dependencies = dependencies;
            _mode = mode;
            _errorOptions = errorOptions;
        }

        public void BeginWatch() {
            this.Protect(() => {
                _dependencies.ForEach(this.BeginObserveDependency);

                if (_mode.HasFlag(BindingMode.OneWay))
                    this.ValueChanged(this, EventArgs.Empty);
            });
        }

        private void BeginObserveDependency(Dependency dependency) {
            dependency.UpdateTarget();

            Require.NotNull<BindingException>(dependency.Target, dependency.PropertyName);

            dependency.Handler = this.OnDependencyValueChanged(dependency);
            dependency.Target.PropertyChanged += dependency.Handler;
            dependency.SubDependencies.ForEach(this.BeginObserveDependency);
        }

        public void EndWatch() {
            this._dependencies.ForEach(this.EndObserveDependency);
        }

        private void EndObserveDependency(Dependency dependency) {
            if (dependency.Target == null)
                return;

            dependency.Target.PropertyChanged -= dependency.Handler;
            dependency.SubDependencies.ForEach(this.EndObserveDependency);
        }

        private PropertyChangedEventHandler OnDependencyValueChanged(Dependency dependency) {
            return (sender, args) => {
                if (dependency.PropertyName != args.PropertyName)
                    return;

                this.Protect(() => {
                    this.UpdateHandlers(dependency);
                    this.ValueChanged(this, EventArgs.Empty);
                });
            };
        }

        private void UpdateHandlers(Dependency dependency) {
            dependency.SubDependencies.ForEach(this.EndObserveDependency);
            dependency.SubDependencies.ForEach(this.BeginObserveDependency);
        }

        private void Protect(Action action) {
            try {
                action();
            }
            catch (BindingException e) {
                if (_errorOptions.Mode == ErrorMode.Throw)
                    throw;

                if (_errorOptions.Mode == ErrorMode.Notify)
                    _errorOptions.Callback(e);
            }
        }
    }
}