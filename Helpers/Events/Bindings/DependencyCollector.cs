using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using Azon.Helpers.Extensions;
using Azon.Helpers.Utils;

namespace Azon.Helpers.Events.Bindings {
    internal class DependencyCollector : ExpressionVisitor {
        private readonly Stack<Dependency> _dependencies;

        public Dependency[] Dependencies {
            get { return _dependencies.First().SubDependencies.ToArray(); }
        }

        public DependencyCollector(Expression expression) {
            _dependencies = new Stack<Dependency>();
            _dependencies.Push(Dependency.Root);

            this.Visit(expression);
        }

        protected override Expression VisitMember(MemberExpression node) {
            if (!node.Expression.Type.Implements<INotifyPropertyChanged>())
                return base.VisitMember(node);

            var dependency = this.AddDependency(node.Expression);

            using (this.CollectChildDependencies(dependency)) {
                return base.VisitMember(node);
            }
        }

        private IDisposable CollectChildDependencies(Dependency dependency) {
            _dependencies.Push(dependency);
            return Disposable.Create(() => _dependencies.Pop());
        }

        private Dependency AddDependency(Expression expression) {
            var dependency = new Dependency(expression);
            _dependencies.Peek().SubDependencies.Add(dependency);
            return dependency;
        }
    }
}