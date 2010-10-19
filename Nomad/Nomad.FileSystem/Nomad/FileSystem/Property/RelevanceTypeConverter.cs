namespace Nomad.FileSystem.Property
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class RelevanceTypeConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((value is byte) && (destinationType == typeof(string)))
            {
                byte num = (byte) value;
                if (num == 0)
                {
                    return string.Empty;
                }
                if (num <= 0x55)
                {
                    return "*";
                }
                if (num <= 170)
                {
                    return "**";
                }
                return "***";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

