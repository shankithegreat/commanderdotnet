namespace Nomad.FileSystem.Property
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class SimplePropertyBag : CustomPropertyProvider, IGetVirtualProperty
    {
        private readonly Dictionary<int, object> FProperties;

        public SimplePropertyBag()
        {
            this.FProperties = new Dictionary<int, object>();
        }

        public SimplePropertyBag(Dictionary<int, object> properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }
            if (properties.Count == 0)
            {
                throw new ArgumentException("You must provide non-empty properties collection");
            }
            this.FProperties = properties;
        }

        public SimplePropertyBag(int property, object value)
        {
            this.FProperties = new Dictionary<int, object>(1);
            this.FProperties.Add(property, value);
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = new VirtualPropertySet();
            foreach (int num in this.FProperties.Keys)
            {
                set[num] = true;
            }
            return set;
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            return (this.FProperties.ContainsKey(propertyId) ? PropertyAvailability.Normal : PropertyAvailability.None);
        }

        public object this[int property]
        {
            get
            {
                object obj2;
                if (this.FProperties.TryGetValue(property, out obj2))
                {
                    return obj2;
                }
                return null;
            }
            set
            {
                this.FProperties[property] = value;
                base.ResetAvailableSet();
            }
        }
    }
}

