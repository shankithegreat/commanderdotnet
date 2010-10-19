namespace Nomad.FileSystem.Property
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    public abstract class CustomPropertyProvider
    {
        private VirtualPropertySet FAvailableSet;

        public event EventHandler AvailablePropertiesChanged;

        protected CustomPropertyProvider()
        {
        }

        protected virtual VirtualPropertySet CreateAvailableSet()
        {
            return null;
        }

        public virtual PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            if (this.AvailableProperties[propertyId])
            {
                if (VirtualProperty.IsSlowProperty(propertyId))
                {
                    return PropertyAvailability.Slow;
                }
                return PropertyAvailability.Normal;
            }
            return PropertyAvailability.None;
        }

        protected virtual object InternalClone()
        {
            return FormatterServices.GetSafeUninitializedObject(base.GetType());
        }

        protected void OnAvailablePropertiesChanged(EventArgs e)
        {
            if (this.AvailablePropertiesChanged != null)
            {
                this.AvailablePropertiesChanged(this, e);
            }
        }

        protected void ResetAvailableSet()
        {
            bool flag = this.FAvailableSet != null;
            this.FAvailableSet = null;
            if (flag)
            {
                this.OnAvailablePropertiesChanged(EventArgs.Empty);
            }
        }

        public virtual VirtualPropertySet AvailableProperties
        {
            get
            {
                if (this.FAvailableSet == null)
                {
                    this.FAvailableSet = this.CreateAvailableSet();
                }
                return this.FAvailableSet;
            }
        }

        protected bool HasAvailableSet
        {
            get
            {
                return (this.FAvailableSet != null);
            }
        }
    }
}

