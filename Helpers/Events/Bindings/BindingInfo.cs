using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings {
    internal class BindingInfo<TSource, TTarget> {
        public Expression<Func<TSource>> Source { get; set; }
        public Expression<Func<TTarget>> Target { get; set; }
        public BindingMode Mode { get; set; }
        public ErrorMode ErrorMode { get; set; }
        public IValueConverter Converter { get; set; }
    }
}