namespace Nomad.FileSystem.Property
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class RatingTypeConverter : PropertyTypeConverter
    {
        public static readonly TypeConverter Default = new RatingTypeConverter();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((value is byte) && (destinationType == typeof(string)))
            {
                byte num = (byte) value;
                for (int i = 1; i <= 5; i++)
                {
                    if (num <= (i * 0x33))
                    {
                        return new string('•', i);
                    }
                }
            }
            if ((value == null) && (destinationType == typeof(PropertyMeasure)))
            {
                return new PropertyMeasure(new string('•', 5));
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

