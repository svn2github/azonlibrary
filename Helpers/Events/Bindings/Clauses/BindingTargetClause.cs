using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    internal class BindingTargetClause<TSource> : IBindingTargetClause {
        private readonly Expression<Func<TSource>> _source;
        private readonly IValueConverter _converter;
        private readonly BindingMode _mode;

        public BindingTargetClause(
            Expression<Func<TSource>> source, 
            IValueConverter converter, 
            BindingMode mode
        ) {
            this._source = source;
            this._converter = converter;
            this._mode = mode;
        }

        public void To<TTarget>(Expression<Func<TTarget>> target) {
            var info = new BindingInfo<TSource, TTarget> {
                Source    = this._source,
                Target    = target,
                Mode      = this._mode,
                Converter = this._converter
            };

            new Binder().Apply(info);
        }
    }
}