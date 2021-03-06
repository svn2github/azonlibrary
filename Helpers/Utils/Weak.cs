using System;

namespace Azon.Helpers.Utils {
#if SILVERLIGHT
    public class Weak<T>
        where T : class 
    {
        private readonly WeakReference _inner;

        public Weak(T target) : this(target, true) { }

        public Weak(T target, bool trackResurrection) {
            _inner = new WeakReference(target, trackResurrection);
        }

        public T Target {
            get { return (T)this._inner.Target; }
        }

        public bool IsAlive {
            get { return this._inner.IsAlive; }
        }
    }
#else
    /// <summary>
    /// A typed <see cref="WeakReference"/>.
    /// </summary>
    /// <typeparam name="T">A type of value to store as a weak reference.</typeparam>
    public class Weak<T> : WeakReference
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Weak{T}"/>.
        /// </summary>
        /// <param name="target">An object to track.</param>
        public Weak(T target) : base(target, true) { }

        /// <summary>
        /// Initializes a new instance of <see cref="Weak{T}"/>.
        /// </summary>
        /// <param name="target">An object to track.</param>
        /// <param name="trackResurrection">
        /// Indicates when to stop tracking object. If true, the object is tracked 
        /// after finalization. If false, the object is only tracked until finalization.
        /// </param>
        public Weak(T target, bool trackResurrection) : base(target, trackResurrection) { }

        /// <summary>
        /// Gets the object referenced by the current <see cref="Weak{T}"/> object.
        /// </summary>
        public new T Target {
            get { return (T)base.Target; }
        }
    }
#endif
}