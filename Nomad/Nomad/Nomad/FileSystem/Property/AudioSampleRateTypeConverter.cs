namespace Nomad.FileSystem.Property
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class AudioSampleRateTypeConverter : PropertyTypeConverter
    {
        public static readonly TypeConverter Default = new AudioSampleRateTypeConverter();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((value is int) && (destinationType == typeof(string)))
            {
                return string.Format("{0} kHz", value);
            }
            if ((value == null) && (destinationType == typeof(PropertyMeasure)))
            {
                return new PropertyMeasure("48000 kHz");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

