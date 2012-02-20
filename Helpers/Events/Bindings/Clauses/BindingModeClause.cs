using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    internal class BindingModeClause<TSource> : IBindingModeClause {
        private readonly Expression<Func<TSource>> _source;
        private readonly IValueConverter _converter;

        public BindingModeClause(
            Expression<Func<TSource>> source,
            IValueConverter converter
            ) {
            this._source = source;
            this._converter = converter;
        }

        public void OneWayFrom<TTarget>(Expression<Func<TTarget>> target) {
            var info = new BindingInfo<TSource, TTarget> {
                Source = this._source,
                Target = target,
                Mode = BindingMode.OneWayToSource,
                Converter = this._converter
            };

            new Binder().Apply(info);
        }

        public void OneWayTo<TTarget>(Expression<Func<TTarget>> target) {
            var info = new BindingInfo<TSource, TTarget> {
                Source = this._source,
                Target = target,
                Mode = BindingMode.OneWay,
                Converter = this._converter
            };

            new Binder().Apply(info);
        }

        public void To<TTarget>(Expression<Func<TTarget>> target) {
            var info = new BindingInfo<TSource, TTarget> {
                Source = this._source,
                Target = target,
                Mode = BindingMode.TwoWay,
                Converter = this._converter
            };

            new Binder().Apply(info);
        }
    }
}