using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Infos {
    internal class BindingInfo<TSource, TTarget> {
        public BindingInfo(PartialBindingInfo<TSource> info) {
            this.ErrorOptions = info.ErrorOptions;
            this.Source = info.Source;
            this.Mode = BindingMode.TwoWay;
        }

        public Expression<Func<TTarget>> Target { get; set; }
        public IValueConverter<TSource, TTarget> Converter { get; set; }
        public Expression<Func<TSource>> Source { get; set; }
        public BindingMode Mode { get; set; }
        public BindindErrorOptions ErrorOptions { get; set; }
    }
}