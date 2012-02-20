using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings {
    internal class Dependency {
        public static readonly Dependency Root = new Dependency(null);

        public IList<Dependency> SubDependencies { get; private set; }
        public INotifyPropertyChanged Target { get; private set; }
        public string PropertyName { get; private set; }

        public Dependency(Expression expression) {
            this.SubDependencies = new List<Dependency>();
        }
    }
}