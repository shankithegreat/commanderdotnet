namespace Nomad.FileSystem.Property
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;

    public class DPITypeConverter : PropertyTypeConverter
    {
        public static readonly TypeConverter Default = new DPITypeConverter();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((value is Size) && (destinationType == typeof(string)))
            {
                Size size = (Size) value;
                if (size.Width == size.Height)
                {
                    return string.Format("{0} dpi", size.Width);
                }
                return string.Format("{0} x {1} dpi", size.Width, size.Height);
            }
            if ((value == null) && (destinationType == typeof(PropertyMeasure)))
            {
                return new PropertyMeasure("4800 dpi");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

