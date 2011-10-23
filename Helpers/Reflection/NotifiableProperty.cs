using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Azon.Helpers.Reflection {
    public static class NotifiableProperty {
        private abstract class NotificationSource : INotifyPropertyChanged, INotifyPropertyChanging {
            public event PropertyChangedEventHandler PropertyChanged;
            public event PropertyChangingEventHandler PropertyChanging;

            public void RaiseChanged(string propertyName) {
                if (this.PropertyChanged == null)
                    return;

                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            public void RaiseChanging(string propertyName) {
                if (this.PropertyChanging == null)
                    return;

                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private class ChangedEventOverlap {
            [FieldOffset(0)]
            public INotifyPropertyChanged Target;

            [FieldOffset(0)]
            public NotificationSource Source;
        }

        [StructLayout(LayoutKind.Explicit)]
        private class ChangingEventOverlap {
            [FieldOffset(0)]
            public INotifyPropertyChanging Target;

            [FieldOffset(0)]
            public NotificationSource Source;
        }

        public static TResult Get<T, TResult>(T target, Expression<Func<T, TResult>> reference)
            where T : class, INotifyPropertyChanged
        {
            var propertyName = Property.Name(reference);
            return AttachedProperty.Get<TResult>(target, propertyName);
        }

        public static void Set<T, TValue>(T target, Expression<Func<T, TValue>> reference, TValue value)
            where T : class, INotifyPropertyChanged
        {
            var propertyName = Property.Name(reference);
            var changedOverlap = new ChangedEventOverlap { Target = target };
            var changingOverlap = new ChangingEventOverlap { Target = target as INotifyPropertyChanging };

            if (changingOverlap.Source != null)
                changingOverlap.Source.RaiseChanging(propertyName);
            AttachedProperty.Set(target, propertyName, value);
            if (changedOverlap.Source != null)
                changedOverlap.Source.RaiseChanged(propertyName);
        }
    }
}