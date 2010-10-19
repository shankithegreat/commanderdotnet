namespace Nomad.FileSystem.Property
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class DurationTypeConverter : PropertyTypeConverter
    {
        public static readonly TypeConverter Default = new DurationTypeConverter();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((value is TimeSpan) && (destinationType == typeof(string)))
            {
                TimeSpan span = (TimeSpan) value;
                TimeSpan span2 = new TimeSpan(span.Days, span.Hours, span.Minutes, span.Seconds);
                return span2.ToString();
            }
            if ((value == null) && (destinationType == typeof(PropertyMeasure)))
            {
                return new PropertyMeasure("23:59:59");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

