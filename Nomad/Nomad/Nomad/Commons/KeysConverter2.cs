namespace Nomad.Commons
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Forms;

    public class KeysConverter2 : KeysConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            object obj2 = base.ConvertTo(context, culture, value, destinationType);
            if (((destinationType == typeof(string)) && (culture != CultureInfo.InvariantCulture)) && (value is Keys))
            {
                Keys keys = (Keys) value;
                string str = (string) obj2;
                switch ((keys & Keys.KeyCode))
                {
                    case Keys.Multiply:
                        if (str.EndsWith("Multiply", StringComparison.Ordinal))
                        {
                            obj2 = str.Substring(0, str.Length - 8) + "Num *";
                        }
                        return obj2;

                    case Keys.Add:
                        if (str.EndsWith("Add", StringComparison.Ordinal))
                        {
                            obj2 = str.Substring(0, str.Length - 3) + "Num +";
                        }
                        return obj2;

                    case Keys.Separator:
                    case Keys.Decimal:
                        return obj2;

                    case Keys.Subtract:
                        if (str.EndsWith("Subtract", StringComparison.Ordinal))
                        {
                            obj2 = str.Substring(0, str.Length - 8) + "Num -";
                        }
                        return obj2;

                    case Keys.Divide:
                        if (str.EndsWith("Divide", StringComparison.Ordinal))
                        {
                            obj2 = str.Substring(0, str.Length - 6) + "Num /";
                        }
                        return obj2;

                    case Keys.OemPipe:
                        if (str.EndsWith("Oem5", StringComparison.Ordinal))
                        {
                            obj2 = str.Substring(0, str.Length - 4) + '\\';
                        }
                        return obj2;
                }
            }
            return obj2;
        }
    }
}

