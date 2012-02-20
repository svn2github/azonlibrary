using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Azon.Helpers.Extensions;
using Azon.Helpers.Utils;

namespace Azon.Helpers.Events.Bindings {
    //internal class DependencyCollector1 : ExpressionVisitor {
    //    private readonly Stack<Dependency> _dependencies;

    //    public Dependency[] Dependencies {
    //        get { return _dependencies.First().SubDependencies.ToArray(); }
    //    }

    //    public DependencyCollector1(Expression expression) {
    //        _dependencies = new Stack<Dependency>();
    //        _dependencies.Push(Dependency.Root);

    //        this.Visit(expression);
    //    }

    //    protected override Expression VisitMember(MemberExpression node) {
    //        if (!(node.Member is PropertyInfo))
    //            return base.VisitMember(node);
    //        if (!node.Expression.Type.Implements<INotifyPropertyChanged>())
    //            return base.VisitMember(node);

    //        var dependency = this.AddDependency(node.Expression, node.Member.Name);

    //        using (this.CollectChildDependencies(dependency)) {
    //            return base.VisitMember(node);
    //        }
    //    }

    //    private IDisposable CollectChildDependencies(Dependency dependency) {
    //        _dependencies.Push(dependency);
    //        return Disposable.Create(() => _dependencies.Pop());
    //    }

    //    private Dependency AddDependency(Expression expression, string propertyName) {
    //        var dependency = new Dependency(expression, propertyName);
    //        _dependencies.Peek().SubDependencies.Add(dependency);
    //        return dependency;
    //    }
    //}

    internal class DependencyCollector2 : ExpressionVisitor {
        private readonly List<Dependency> _dependencies;
        private readonly Stack<Dependency> _childDependencies;

        public Dependency[] Dependencies {
            get { return _dependencies.ToArray(); }
        }

        public DependencyCollector2(Expression expression) {
            _dependencies = new List<Dependency>();
            _childDependencies = new Stack<Dependency>();

            this.Visit(expression);
        }

        protected override Expression VisitMember(MemberExpression node) {
            if (!(node.Member is PropertyInfo))
                return base.VisitMember(node);
            if (!node.Expression.Type.Implements<INotifyPropertyChanged>())
                return base.VisitMember(node);

            var dependency = this.AddDependency(node.Expression, node.Member.Name);

            using (this.PushChildDependencies(dependency)) {
                return base.VisitMember(node);
            }
        }

        private IDisposable PushChildDependencies(Dependency dependency) {
            _childDependencies.Push(dependency);
            return Disposable.Create(() => _childDependencies.Pop());
        }

        private Dependency AddDependency(Expression expression, string propertyName) {
            var dependency = new Dependency(expression, propertyName);
            dependency.SubDependencies.AddRange(this._childDependencies);
            _dependencies.Add(dependency);
            return dependency;
        }
    }

    internal class DependencyCollector : ExpressionVisitor {
        private IEnumerable<Dependency> _dependencies;

        public Dependency[] Dependencies {
            get { return _dependencies.ToArray(); }
        }

        public DependencyCollector(Expression expression) {
            _dependencies = Enumerable.Empty<Dependency>();

            this.Visit(expression);
        }

        protected override Expression VisitMember(MemberExpression node) {
            if (!(node.Member is PropertyInfo))
                return base.VisitMember(node);
            if (!node.Expression.Type.Implements<INotifyPropertyChanged>())
                return base.VisitMember(node);

            var dependency = new Dependency(node.Expression, node.Member.Name);
            dependency.SubDependencies.AddRange(_dependencies);
            _dependencies = new[] { dependency };

            return base.VisitMember(node);
        }
    }
}