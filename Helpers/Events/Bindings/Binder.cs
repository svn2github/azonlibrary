using System;

using Azon.Helpers.Reflection;
using Azon.Helpers.Utils;

namespace Azon.Helpers.Events.Bindings {
    internal class Binder {
        public void Apply<TSource, TTarget>(BindingInfo<TSource, TTarget> info) {
            if (info.Mode.HasFlag(BindingMode.OneWay)) {
                var sourceGetter = info.Source.Compile();
                Action<TTarget> targetSetter = value => Property.Set(info.Target, value);

                var dependencies = new DependencyCollector(info.Source).Dependencies;
                var watcher = new DependencyWatcher(dependencies);

                watcher.ValueChanged += (sender, e) => {
                    Guard.Block(this, () => targetSetter((TTarget)(object)sourceGetter()));
                };
                watcher.BeginWatch();
            }

            if (info.Mode.HasFlag(BindingMode.OneWayToSource)) {
                var targetGetter = info.Target.Compile();
                Action<TSource> sourceSetter = value => Property.Set(info.Source, value);

                var dependencies = new DependencyCollector(info.Target).Dependencies;
                var watcher = new DependencyWatcher(dependencies);

                watcher.ValueChanged += (sender, e) => {
                    Guard.Block(this, () => sourceSetter((TSource)(object)targetGetter()));
                };
                watcher.BeginWatch();
            }
        }
    }
}