namespace Nomad.FileSystem.Property
{
    using Nomad.FileSystem.Properties;
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class SizeTypeConverter : PropertyTypeConverter
    {
        public static readonly TypeConverter Default = new SizeTypeConverter();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((value != null) && (destinationType == typeof(string)))
            {
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        return FormatSize<long>(Convert.ToInt64(value), Settings.Default.SizeFormat);
                }
            }
            if ((value == null) && (destinationType == typeof(PropertyMeasure)))
            {
                return new PropertyMeasure(FormatSize<long>(0x174876e7ffL, Settings.Default.SizeFormat));
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public static string FormatSize<T>(T size, SizeFormat format) where T: struct
        {
            long num = Convert.ToInt64(size);
            if (format == SizeFormat.Bytes)
            {
                return num.ToString("#,##0");
            }
            if ((num < 0x100000L) || (format == SizeFormat.Kilobytes))
            {
                if (num >= 0x400L)
                {
                    return string.Format("{0:#,##0.0} KB", Convert.ToDouble(size) / 1024.0);
                }
                return size.ToString();
            }
            if (num < 0x40000000L)
            {
                return string.Format("{0:#,##0.0} MB", Convert.ToDouble(size) / 1048576.0);
            }
            return string.Format("{0:#,##0.00} GB", Convert.ToDouble(size) / 1073741824.0);
        }
    }
}

