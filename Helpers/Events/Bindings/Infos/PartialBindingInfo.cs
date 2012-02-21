using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Infos {
    internal class PartialBindingInfo<TSource> {
        public PartialBindingInfo() {
            this.ErrorOptions = new BindindErrorOptions();
        }

        public Expression<Func<TSource>> Source { get; set; }
        public BindindErrorOptions ErrorOptions { get; set; }
    }
}