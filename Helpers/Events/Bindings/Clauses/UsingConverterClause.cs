using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    internal class UsingConverterClause<TSource> : IUsingConverterClause {
        private readonly Expression<Func<TSource>> _source;

        public UsingConverterClause(Expression<Func<TSource>> source) {
            this._source = source;
        }

        public IBindingModeClause Using(IValueConverter converter) {
            return new BindingModeClause<TSource>(this._source, converter);
        }
    }
}