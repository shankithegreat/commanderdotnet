namespace Nomad.Commons.Resources
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Forms;

    public abstract class BasicFormStringLocalizer : BasicFormLocalizer
    {
        private Dictionary<System.Type, PropertyDescriptorCollection> TypePropertyCache;

        protected BasicFormStringLocalizer()
        {
        }

        protected override void ApplyResources(Component component, string componentName, CultureInfo culture)
        {
            foreach (PropertyDescriptor descriptor in this.GetLocalizableStringProperties(component.GetType()))
            {
                string str = this.GetString(string.Format("{0}.{1}", componentName, descriptor.Name), culture);
                if (str != null)
                {
                    descriptor.SetValue(component, str);
                }
            }
            ComboBox box = component as ComboBox;
            if ((box != null) && (box.DataSource == null))
            {
                string item = this.GetString(string.Format("{0}.Items", box.Name), culture);
                if (item != null)
                {
                    int selectedIndex = box.SelectedIndex;
                    box.BeginUpdate();
                    box.Items.Clear();
                    int num2 = 1;
                    while (item != null)
                    {
                        box.Items.Add(item);
                        item = this.GetString(string.Format("{0}.Items{1}", box.Name, num2++), culture);
                    }
                    if (selectedIndex < box.Items.Count)
                    {
                        box.SelectedIndex = selectedIndex;
                    }
                    box.EndUpdate();
                }
            }
        }

        protected PropertyDescriptorCollection GetLocalizableStringProperties(System.Type type)
        {
            PropertyDescriptorCollection descriptors;
            if ((this.TypePropertyCache == null) || !this.TypePropertyCache.TryGetValue(type, out descriptors))
            {
                ICustomTypeDescriptor typeDescriptor = TypeDescriptor.GetProvider(type).GetTypeDescriptor(type);
                descriptors = new PropertyDescriptorCollection(null);
                foreach (PropertyDescriptor descriptor2 in typeDescriptor.GetProperties())
                {
                    if ((descriptor2.PropertyType == typeof(string)) && descriptor2.IsLocalizable)
                    {
                        descriptors.Add(descriptor2);
                    }
                }
                if (this.TypePropertyCache == null)
                {
                    this.TypePropertyCache = new Dictionary<System.Type, PropertyDescriptorCollection>();
                }
                this.TypePropertyCache.Add(type, descriptors);
            }
            return descriptors;
        }

        protected abstract string GetString(string name, CultureInfo culture);
    }
}

