namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;

    internal sealed class NonSerializablePropertyDescriptor : PropertyDescriptor
    {
        private PropertyDescriptor _descriptor;

        internal NonSerializablePropertyDescriptor(PropertyDescriptor baseDescriptor) : base(baseDescriptor)
        {
            this._descriptor = baseDescriptor;
        }

        public override bool CanResetValue(object component)
        {
            return this._descriptor.CanResetValue(component);
        }

        public override object GetValue(object component)
        {
            return this._descriptor.GetValue(component);
        }

        public override void ResetValue(object component)
        {
            this._descriptor.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            this._descriptor.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get
            {
                return this._descriptor.ComponentType;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return this._descriptor.IsReadOnly;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this._descriptor.PropertyType;
            }
        }
    }
}

