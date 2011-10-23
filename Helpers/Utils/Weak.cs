using System;

namespace Azon.Helpers.Utils {
    public class Weak<T> : WeakReference {
        public Weak(T target) : base(target, true) { }
        public Weak(T target, bool trackResurrection) : base(target, trackResurrection) { }

        public new T Target {
            get { return (T)base.Target; }
        }
    }
}