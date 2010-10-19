namespace Nomad.Controls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;

    public class PropertyValue
    {
        private object FDataObject;
        private PropertyInfo FProperty;
        private string FPropertyName;
        private object FPropertyValue;

        public PropertyValue()
        {
        }

        public PropertyValue(object dataObject, string propertyName)
        {
            this.FDataObject = dataObject;
            this.FPropertyName = propertyName;
        }

        public void RememberValue()
        {
            if (this.FDataObject == null)
            {
                this.FProperty = null;
            }
            else
            {
                this.FProperty = this.FDataObject.GetType().GetProperty(this.FPropertyName);
                if (this.FProperty != null)
                {
                    this.FPropertyValue = this.FProperty.GetValue(this.FDataObject, null);
                }
            }
        }

        [AttributeProvider(typeof(IComponent))]
        public object DataObject
        {
            get
            {
                return this.FDataObject;
            }
            set
            {
                this.FDataObject = value;
                if ((this.FDataObject != null) && string.IsNullOrEmpty(this.FPropertyName))
                {
                    object[] customAttributes = this.FDataObject.GetType().GetCustomAttributes(typeof(DefaultPropertyAttribute), true);
                    if ((customAttributes.Length > 0) && (customAttributes[0] is DefaultPropertyAttribute))
                    {
                        this.FPropertyName = ((DefaultPropertyAttribute) customAttributes[0]).Name;
                    }
                }
                this.FProperty = null;
            }
        }

        public string PropertyName
        {
            get
            {
                return this.FPropertyName;
            }
            set
            {
                this.FPropertyName = value;
                this.FProperty = null;
            }
        }

        [Browsable(false)]
        public bool ValueChanged
        {
            get
            {
                if (this.FProperty == null)
                {
                    return false;
                }
                object a = this.FProperty.GetValue(this.FDataObject, null);
                if ((a != null) && (this.FPropertyValue != null))
                {
                    Type type = this.FPropertyValue.GetType();
                    if (a.GetType() != type)
                    {
                        return true;
                    }
                    if (type == typeof(Color))
                    {
                        return (((Color) this.FPropertyValue) != ((Color) a));
                    }
                    return (Comparer.Default.Compare(a, this.FPropertyValue) != 0);
                }
                return ((a == null) ^ (this.FPropertyValue == null));
            }
        }
    }
}

