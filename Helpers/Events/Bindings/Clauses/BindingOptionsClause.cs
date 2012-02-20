using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public class BindingOptionsClause<TSource> : IBindingOptionsClause {
        private readonly Expression<Func<TSource>> _source;

        public BindingOptionsClause(Expression<Func<TSource>> source) {
            this._source = source;
        }

        public IBindingModeClause Using(IValueConverter converter) {
            return new UsingConverterClause<TSource>(this._source).Using(converter);
        }

        public void OneWayFrom<TTarget>(Expression<Func<TTarget>> target) {
            new BindingModeClause<TSource>(this._source, null).OneWayFrom(target);
        }

        public void OneWayTo<TTarget>(Expression<Func<TTarget>> target) {
            new BindingModeClause<TSource>(this._source, null).OneWayTo(target);
        }

        public void To<TTarget>(Expression<Func<TTarget>> target) {
            new BindingModeClause<TSource>(this._source, null).To(target);
        }
    }
}