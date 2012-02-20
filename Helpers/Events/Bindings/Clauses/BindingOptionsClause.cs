using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public class BindingOptionsClause<TSource> : IBindingOptionsClause {
        private readonly Expression<Func<TSource>> _source;

        public BindingOptionsClause(Expression<Func<TSource>> source) {
            _source = source;
        }

        public IBindingModeWithTargetClause Using(IValueConverter converter) {
            return new UsingConverterClause<TSource>(_source).Using(converter);
        }

        public IBindingTargetClause In(BindingMode mode) {
            return this.Using(null).In(mode);
        }

        public void OneWayFrom<TTarget>(Expression<Func<TTarget>> target) {
            this.Using(null).OneWayFrom(target);
        }

        public void OneWayTo<TTarget>(Expression<Func<TTarget>> target) {
            this.Using(null).OneWayTo(target);
        }

        public void To<TTarget>(Expression<Func<TTarget>> target) {
            this.Using(null).To(target);
        }

        public IBindingOptionsClause ThrowingOnBindingErrors() {
            return new OnErrorBehaviourClause<TSource>(_source).ThrowingOnBindingErrors();
        }

        public IBindingOptionsClause SkippingBindingErrors() {
            return new OnErrorBehaviourClause<TSource>(_source).SkippingBindingErrors();
        }

        public IBindingOptionsClause NotifyingOnBindingErrors() {
            return new OnErrorBehaviourClause<TSource>(_source).NotifyingOnBindingErrors();
        }
    }
}