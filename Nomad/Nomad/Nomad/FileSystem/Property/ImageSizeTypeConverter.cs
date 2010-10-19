namespace Nomad.FileSystem.Property
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;

    public class ImageSizeTypeConverter : PropertyTypeConverter
    {
        public static readonly TypeConverter Default = new ImageSizeTypeConverter();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((value is Size) && (destinationType == typeof(string)))
            {
                Size size = (Size) value;
                return string.Format("{0} x {1}", size.Width, size.Height);
            }
            if ((value == null) && (destinationType == typeof(PropertyMeasure)))
            {
                return new PropertyMeasure("9999 x 9999");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

