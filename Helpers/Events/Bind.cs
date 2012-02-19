using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events {
    public static class Bind {
        public static BindingBuilder Property<TSource>(Expression<Func<TSource>> reference) {
            return new BindingBuilder();
        }
    }

    public class BindingBuilder {
        public BindingBuilder() {
            
        }

        public void To<TTarget>(Expression<Func<TTarget>> reference) {
        }
    }
}
