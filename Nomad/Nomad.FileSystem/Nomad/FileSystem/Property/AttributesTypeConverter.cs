namespace Nomad.FileSystem.Property
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public class AttributesTypeConverter : PropertyTypeConverter
    {
        private static FileAttributes[] VisibleAttributes = new FileAttributes[] { FileAttributes.Archive, FileAttributes.Compressed, FileAttributes.Directory, FileAttributes.Encrypted, FileAttributes.Hidden, FileAttributes.ReadOnly, FileAttributes.ReparsePoint, FileAttributes.System };

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((value is FileAttributes) && (destinationType == typeof(string)))
            {
                FileAttributes attributes = (FileAttributes) value;
                StringBuilder builder = new StringBuilder(8);
                foreach (FileAttributes attributes2 in VisibleAttributes)
                {
                    if ((attributes & attributes2) > 0)
                    {
                        if (attributes2 == FileAttributes.ReparsePoint)
                        {
                            builder.Append('p');
                        }
                        else
                        {
                            builder.Append(attributes2.ToString()[0]);
                        }
                    }
                    else
                    {
                        builder.Append('-');
                    }
                }
                return builder.ToString().ToLower();
            }
            if ((value == null) && (destinationType == typeof(PropertyMeasure)))
            {
                return new PropertyMeasure("acdehrps");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

