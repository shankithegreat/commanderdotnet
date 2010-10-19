namespace Nomad.FileSystem.Property
{
    using Nomad.FileSystem.Properties;
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class DateTimeTypeConverter : PropertyTypeConverter
    {
        public static readonly TypeConverter Default = new DateTimeTypeConverter();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            DateTime time;
            if ((value is DateTime) && (destinationType == typeof(string)))
            {
                time = (DateTime) value;
                return time.ToString(Settings.Default.DateTimeFormat);
            }
            if ((value == null) && (destinationType == typeof(PropertyMeasure)))
            {
                time = new DateTime(0x7d0, 10, 30, 0x17, 0x3b, 0x3b);
                return new PropertyMeasure(time.ToString(Settings.Default.DateTimeFormat));
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

