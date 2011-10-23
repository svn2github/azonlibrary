using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Azon.Helpers.Reflection {
    public class HierarchicalPropertyInfo : PropertyInfo {
        public PropertyInfo[] Hierarchy { get; private set; }

        public PropertyInfo LastProperty {
            get { return this.Hierarchy[this.Hierarchy.Length - 1]; }
        }

        public HierarchicalPropertyInfo(params PropertyInfo[] hierarchy) {
            this.Hierarchy = hierarchy;
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) {
            for (int i = 0; i < this.Hierarchy.Length - 1; i++) {
                obj = this.Hierarchy[i].GetValue(obj, null);
            }
            this.LastProperty.SetValue(obj, value, invokeAttr, binder, index, culture);
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) {
            var value = obj;
            for (var i = 0; i < this.Hierarchy.Length - 1; i++) {
                value = this.Hierarchy[i].GetValue(value, null);
            }
            return this.LastProperty.GetValue(value, invokeAttr, binder, index, culture);
        }

        #region Delegating members

        public override object[] GetCustomAttributes(bool inherit) {
            return LastProperty.GetCustomAttributes(inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) {
            return LastProperty.GetCustomAttributes(attributeType, inherit);
        }

        public override bool IsDefined(Type attributeType, bool inherit) {
            return LastProperty.IsDefined(attributeType, inherit);
        }

        public override IList<CustomAttributeData> GetCustomAttributesData() {
            return LastProperty.GetCustomAttributesData();
        }

        public override string Name {
            get { return LastProperty.Name; }
        }

        public override Type DeclaringType {
            get { return LastProperty.DeclaringType; }
        }

        public override Type ReflectedType {
            get { return LastProperty.ReflectedType; }
        }

        public override int MetadataToken {
            get { return LastProperty.MetadataToken; }
        }

        public override Module Module {
            get { return LastProperty.Module; }
        }

        public override object GetConstantValue() {
            return LastProperty.GetConstantValue();
        }

        public override object GetRawConstantValue() {
            return LastProperty.GetRawConstantValue();
        }

        public override MethodInfo[] GetAccessors(bool nonPublic) {
            return LastProperty.GetAccessors(nonPublic);
        }

        public override MethodInfo GetGetMethod(bool nonPublic) {
            return LastProperty.GetGetMethod(nonPublic);
        }

        public override MethodInfo GetSetMethod(bool nonPublic) {
            return LastProperty.GetSetMethod(nonPublic);
        }

        public override ParameterInfo[] GetIndexParameters() {
            return LastProperty.GetIndexParameters();
        }

        public override Type[] GetRequiredCustomModifiers() {
            return LastProperty.GetRequiredCustomModifiers();
        }

        public override Type[] GetOptionalCustomModifiers() {
            return LastProperty.GetOptionalCustomModifiers();
        }

        public override MemberTypes MemberType {
            get { return LastProperty.MemberType; }
        }

        public override Type PropertyType {
            get { return LastProperty.PropertyType; }
        }

        public override PropertyAttributes Attributes {
            get { return LastProperty.Attributes; }
        }

        public override bool CanRead {
            get { return LastProperty.CanRead; }
        }

        public override bool CanWrite {
            get { return LastProperty.CanWrite; }
        }

        #endregion
    }
}
