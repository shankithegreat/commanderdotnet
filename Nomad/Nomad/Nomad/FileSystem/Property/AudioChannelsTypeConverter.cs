namespace Nomad.FileSystem.Property
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class AudioChannelsTypeConverter : TypeConverter
    {
        public static readonly TypeConverter Default = new AudioChannelsTypeConverter();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((value is int) && (destinationType == typeof(string)))
            {
                switch (((int) value))
                {
                    case 0:
                        return string.Empty;

                    case 1:
                        return "1 (Mono)";

                    case 2:
                        return "2 (Stereo)";
                }
                return value.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

