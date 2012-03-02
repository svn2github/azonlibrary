using System;
using System.Linq.Expressions;

using Azon.Helpers.Asserts;
using Azon.Helpers.Events.Bindings.Infos;

namespace Azon.Helpers.Events.Bindings.Clauses {
    internal class BindingTargetWithModeClause<TSource, TTarget> : IBindingModeWithTargetClause<TTarget> {
        private readonly BindingInfo<TSource, TTarget> _info;

        public BindingTargetWithModeClause(BindingInfo<TSource, TTarget> info) {
            _info = info;
        }

        public IBindingTargetClause<TTarget> In(BindingMode mode) {
            _info.Mode = mode;
            return this;
        }
        
        public void OneWayFrom(Expression<Func<TTarget>> target) {
            this.In(BindingMode.OneWayToSource).To(target);
        }

        public void OneWayTo(Expression<Func<TTarget>> target) {
            this.In(BindingMode.OneWay).To(target);
        }

        public void To(Expression<Func<TTarget>> target) {
            Require.NotNull(target, "target");
            _info.Target = target;
            new Binder().Apply(this._info);
        }
    }
}