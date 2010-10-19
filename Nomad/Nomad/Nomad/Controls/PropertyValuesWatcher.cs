namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;

    [DefaultProperty("Items"), DesignerCategory("Code")]
    public class PropertyValuesWatcher : Component, ISupportInitialize
    {
        private PropertyValueList FItems = new PropertyValueList();

        public bool IsValueChanged(object dataObject)
        {
            foreach (PropertyValue value2 in this.FItems)
            {
                if (value2.DataObject == dataObject)
                {
                    return value2.ValueChanged;
                }
            }
            return false;
        }

        public void RememberValues()
        {
            if (this.FItems != null)
            {
                foreach (PropertyValue value2 in this.FItems)
                {
                    value2.RememberValue();
                }
            }
        }

        void ISupportInitialize.BeginInit()
        {
        }

        void ISupportInitialize.EndInit()
        {
            this.RememberValues();
        }

        [Browsable(false)]
        public bool AnyValueChanged
        {
            get
            {
                if (this.FItems != null)
                {
                    foreach (PropertyValue value2 in this.FItems)
                    {
                        if (value2.ValueChanged)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PropertyValueList Items
        {
            get
            {
                return this.FItems;
            }
        }
    }
}

