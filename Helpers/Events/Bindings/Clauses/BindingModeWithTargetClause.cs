using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    internal class BindingModeWithTargetClause<TSource> : IBindingModeWithTargetClause {
        private readonly Expression<Func<TSource>> _source;
        private readonly IValueConverter _converter;

        public BindingModeWithTargetClause(
            Expression<Func<TSource>> source,
            IValueConverter converter
        ) {
            this._source = source;
            this._converter = converter;
        }

        private IBindingTargetClause With(BindingMode mode) {
            return new BindingModeClause<TSource>(this._source, this._converter).In(mode);
        }

        public IBindingTargetClause In(BindingMode mode) {
            return this.With(mode);
        }

        public void To<TTarget>(Expression<Func<TTarget>> target) {
            this.With(BindingMode.TwoWay).To(target);
        }

        public void OneWayFrom<TTarget>(Expression<Func<TTarget>> target) {
            this.With(BindingMode.OneWayToSource).To(target);
        }

        public void OneWayTo<TTarget>(Expression<Func<TTarget>> target) {
            this.With(BindingMode.OneWay).To(target);
        }
    }
}