using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings {
    internal class Dependency {
        private readonly Expression _expression;
        private readonly Func<INotifyPropertyChanged> _compiled; 

        public IList<Dependency> SubDependencies { get; private set; }
        public INotifyPropertyChanged Target { get; private set; }
        public string PropertyName { get; private set; }
        public PropertyChangedEventHandler Handler { get; set; }

        private Dependency() {
            this.SubDependencies = new List<Dependency>();
        }

        public Dependency(Expression expression, string propertyName) : this() {
            _expression = expression;
            _compiled = this.Compile();

            this.PropertyName = propertyName;
        }

        private Func<INotifyPropertyChanged> Compile() {
            return Expression.Lambda<Func<INotifyPropertyChanged>>(_expression).Compile();
        }

        public void UpdateTarget() {
            this.Target = _compiled();
        }

        public static Dependency Root {
            get { return new Dependency(); }
        }
    }
}