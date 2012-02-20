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

        public IBindingTargetClause In(BindingMode mode) {
            return new BindingTargetClause<TSource>(_source, _converter, mode);
        }
    }
}