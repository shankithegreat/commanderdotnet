namespace Nomad.FileSystem.Archive
{
    using Nomad.FileSystem.Archive.Common;
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class ArchiveFormatConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            ArchiveFormatInfo info = value as ArchiveFormatInfo;
            if ((info != null) && (destinationType == typeof(string)))
            {
                return info.Name;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

