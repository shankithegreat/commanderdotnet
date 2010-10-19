namespace Nomad.FileSystem.Property
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text;

    public class EncodingConveter : TypeConverter
    {
        public static readonly TypeConverter Default = new EncodingConveter();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Encoding encoding = value as Encoding;
            if ((encoding != null) && (destinationType == typeof(string)))
            {
                return encoding.EncodingName;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

