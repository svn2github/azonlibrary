using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    internal class OnErrorBehaviourClause<TSource> : IOnErrorBehaviourClause {
        private readonly Expression<Func<TSource>> _source;

        public OnErrorBehaviourClause(Expression<Func<TSource>> source) {
            this._source = source;
        }

        public IBindingOptionsClause ThrowingOnBindingErrors() {
            throw new System.NotImplementedException();
        }

        public IBindingOptionsClause SkippingBindingErrors() {
            throw new System.NotImplementedException();
        }

        public IBindingOptionsClause NotifyingOnBindingErrors() {
            throw new System.NotImplementedException();
        }
    }
}