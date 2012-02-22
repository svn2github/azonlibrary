using System;
using System.Linq.Expressions;

using Azon.Helpers.Events.Bindings.Infos;
using Azon.Helpers.Utils;

namespace Azon.Helpers.Events.Bindings {
    internal class Binder {
        public void Apply<TSource, TTarget>(BindingInfo<TSource, TTarget> info) {
            if (info.Mode.HasFlag(BindingMode.OneWay)) {
                var sourceGetter = info.Source.Compile();
                var targetSetter = this.CreateSetter(info.Target);

                var dependencies = new DependencyCollector(info.Source).Dependencies;
                var watcher = new DependencyWatcher(dependencies, BindingMode.OneWay, info.ErrorOptions);
                var handler = this.CreateHandler(sourceGetter, targetSetter, info.Converter.ConvertTo);

                watcher.ValueChanged += handler;
                watcher.BeginWatch();
            }

            if (info.Mode.HasFlag(BindingMode.OneWayToSource)) {
                var targetGetter = info.Target.Compile();
                var sourceSetter = this.CreateSetter(info.Source);

                var dependencies = new DependencyCollector(info.Target).Dependencies;
                var watcher = new DependencyWatcher(dependencies, BindingMode.OneWayToSource, info.ErrorOptions);
                var handler = this.CreateHandler(targetGetter, sourceSetter, info.Converter.ConvertFrom);

                watcher.ValueChanged += handler;
                watcher.BeginWatch();
            }
        }

        private Action<T> CreateSetter<T>(Expression<Func<T>> reference) {
            var parameter = Expression.Parameter(typeof(T), "value");

            return Expression.Lambda<Action<T>>(
                Expression.Assign(
                    reference.Body,
                    parameter
                ),
                parameter
            ).Compile();
        }

        private EventHandler CreateHandler<TSource, TTarget>(
            Func<TSource> getter,
            Action<TTarget> setter,
            Func<TSource, TTarget> converter
        ) {
            return (sender, e) => Guard.Block(this, () => this.ApplyValue(getter, setter, converter));
        }

        private void ApplyValue<TSource, TTarget>(
            Func<TSource> getter,
            Action<TTarget> setter,
            Func<TSource, TTarget> converter
        ) {
            var value = default(TSource);
            var converted = default(TTarget);

            this.Try(() => value = getter(), "get value");
            this.Try(() => converted = converter(value), "convert value");
            this.Try(() => setter(converted), "apply value");
        }

        private void Try(Action action, string error) {
            try {
                action();
            }
            catch (Exception e) {
                throw new BindingException("An attempt to " + error + " for binding has failed.", e);
            }
        }
    }
}