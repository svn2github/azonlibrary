using System;
using System.Linq.Expressions;

using Azon.Helpers.Asserts;
using Azon.Helpers.Events.Bindings.Clauses;

namespace Azon.Helpers.Events {
    public static class Bind {
        public static IBindingOptionsClause Property<TSource>(Expression<Func<TSource>> source) {
            Require.NotNull(source, "source");

            return new BindingOptionsClause<TSource>(source);
        }
    }
}
