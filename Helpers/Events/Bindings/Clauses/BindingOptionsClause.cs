using System;
using System.Linq.Expressions;

using Azon.Helpers.Asserts;
using Azon.Helpers.Events.Bindings.Infos;

namespace Azon.Helpers.Events.Bindings.Clauses {
    internal class BindingOptionsClause<TSource> : IBindingOptionsClause<TSource> {
        private readonly PartialBindingInfo<TSource> _info;

        public BindingOptionsClause(PartialBindingInfo<TSource> info) {
            _info = info;
        }

        public IBindingOptionsClause<TSource> ThrowingOnBindingErrors() {
            _info.ErrorOptions.Mode = ErrorMode.Throw;
            return this;
        }

        public IBindingOptionsClause<TSource> SkippingBindingErrors() {
            _info.ErrorOptions.Mode = ErrorMode.Skip;
            return this;
        }

        public IBindingOptionsClause<TSource> NotifyingOnBindingErrors(Action<BindingException> callback) {
            _info.ErrorOptions.Mode = ErrorMode.Notify;
            _info.ErrorOptions.Callback = callback;
            return this;
        }

        public void To(Expression<Func<TSource>> target) {
            this.In(BindingMode.TwoWay).To(target);
        }

        public void OneWayFrom(Expression<Func<TSource>> target) {
            this.In(BindingMode.OneWayToSource).To(target);
        }

        public void OneWayTo(Expression<Func<TSource>> target) {
            this.In(BindingMode.OneWay).To(target);
        }

        public IBindingTargetClause<TSource> In(BindingMode mode) {
            return this.Using(new ValueConverter<TSource, TSource>()).In(mode);
        }

        public IBindingModeWithTargetClause<TTarget> Using<TTarget>(IValueConverter<TSource, TTarget> converter) {
            Require.NotNull(converter, "converter");
            var info = new BindingInfo<TSource, TTarget>(_info) {
                Converter = converter
            };
            return new BindingTargetWithModeClause<TSource, TTarget>(info);
        }
    }
}