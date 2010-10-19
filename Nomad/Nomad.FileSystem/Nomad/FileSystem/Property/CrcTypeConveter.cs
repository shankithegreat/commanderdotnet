namespace Nomad.FileSystem.Property
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class CrcTypeConveter : PropertyTypeConverter
    {
        public static readonly TypeConverter Default = new CrcTypeConveter();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((value != null) && (destinationType == typeof(string)))
            {
                ushort num;
                uint num2;
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Int16:
                        num = (ushort) ((short) value);
                        return num.ToString("x4");

                    case TypeCode.UInt16:
                        num = (ushort) value;
                        return num.ToString("x4");

                    case TypeCode.Int32:
                        num2 = (uint) ((int) value);
                        return num2.ToString("x8");

                    case TypeCode.UInt32:
                        num2 = (uint) value;
                        return num2.ToString("x8");
                }
            }
            if ((value == null) && (destinationType == typeof(PropertyMeasure)))
            {
                return new PropertyMeasure("FFFFFFFF");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

