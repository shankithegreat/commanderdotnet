namespace Nomad.FileSystem.Property
{
    using Nomad.Commons;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text;

    public class VirtualPropertySetConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(string)) || base.CanConvertTo(context, destinationType));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                VirtualPropertySet set = new VirtualPropertySet();
                foreach (string str in StringHelper.SplitString((string) value, new char[] { ',', ' ' }))
                {
                    VirtualProperty property = VirtualProperty.Get(str);
                    if (property != null)
                    {
                        set[property.PropertyId] = true;
                    }
                }
                return set;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((value is VirtualPropertySet) && (destinationType == typeof(string)))
            {
                StringBuilder builder = new StringBuilder();
                foreach (VirtualProperty property in (IEnumerable<VirtualProperty>) value)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(property.PropertyName);
                }
                return builder.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return ((value is string) || base.IsValid(context, value));
        }
    }
}

