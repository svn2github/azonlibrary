using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    internal class UsingConverterClause<TSource> : IUsingConverterClause {
        private readonly Expression<Func<TSource>> _source;

        public UsingConverterClause(Expression<Func<TSource>> source) {
            this._source = source;
        }

        public IBindingModeWithTargetClause Using(IValueConverter converter) {
            return new BindingModeWithTargetClause<TSource>(this._source, converter);
        }
    }
}