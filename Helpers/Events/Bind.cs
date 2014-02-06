using System;
using System.Linq.Expressions;

using Azon.Helpers.Annotations;
using Azon.Helpers.Asserts;
using Azon.Helpers.Events.Bindings.Clauses;
using Azon.Helpers.Events.Bindings.Infos;

namespace Azon.Helpers.Events {
    public static class Bind {
        [NotNull]
        public static IBindingOptionsClause<TSource> Property<TSource>([NotNull] Expression<Func<TSource>> source) {
            Require.NotNull(source, "source");
            var info = new PartialBindingInfo<TSource> { Source = source };
            return new BindingOptionsClause<TSource>(info);
        }
    }
}
