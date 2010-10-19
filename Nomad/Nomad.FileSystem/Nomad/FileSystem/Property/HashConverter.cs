namespace Nomad.FileSystem.Property
{
    using Nomad.Commons;
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class HashConverter : TypeConverter
    {
        public static readonly TypeConverter Default = new HashConverter();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            byte[] data = value as byte[];
            if ((data != null) && (destinationType == typeof(string)))
            {
                return ByteArrayHelper.ToString(data);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

