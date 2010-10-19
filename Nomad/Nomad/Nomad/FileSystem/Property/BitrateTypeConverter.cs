namespace Nomad.FileSystem.Property
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class BitrateTypeConverter : PropertyTypeConverter
    {
        public static readonly TypeConverter Default = new BitrateTypeConverter();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((value is int) && (destinationType == typeof(string)))
            {
                return string.Format("{0}kbps", value);
            }
            if ((value == null) && (destinationType == typeof(PropertyMeasure)))
            {
                return new PropertyMeasure("999kbps");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

