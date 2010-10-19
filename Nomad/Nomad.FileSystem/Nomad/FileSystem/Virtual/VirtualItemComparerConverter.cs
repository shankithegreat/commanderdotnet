namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text;

    public class VirtualItemComparerConverter : TypeConverter
    {
        private const string AscendingStr = "Asc";
        private const string DescendingStr = "Desc";

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (destinationType == typeof(string));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string propertyName = value as string;
            if (propertyName != null)
            {
                int propertyId;
                ListSortDirection ascending;
                string[] strArray = propertyName.Split(new char[] { ',' });
                propertyName = strArray[0];
                int index = propertyName.IndexOf(' ');
                string str2 = string.Empty;
                NameComparison comparison = NameComparison.Default;
                for (int i = 1; i < strArray.Length; i++)
                {
                    try
                    {
                        comparison |= (NameComparison) Enum.Parse(typeof(NameComparison), strArray[i]);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
                if (index > -1)
                {
                    str2 = propertyName.Substring(index + 1).TrimStart(new char[0]);
                    propertyName = propertyName.Substring(0, index);
                }
                try
                {
                    propertyId = VirtualProperty.Get(propertyName).PropertyId;
                }
                catch (ArgumentException)
                {
                    propertyId = VirtualItemComparer.DefaultSort.ComparePropertyId;
                }
                if (str2.Equals("Asc", StringComparison.OrdinalIgnoreCase))
                {
                    ascending = ListSortDirection.Ascending;
                }
                else if (str2.Equals("Desc", StringComparison.OrdinalIgnoreCase))
                {
                    ascending = ListSortDirection.Descending;
                }
                else
                {
                    ascending = VirtualItemComparer.DefaultSort.SortDirection;
                }
                return new VirtualItemComparer(propertyId, ascending, comparison);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            VirtualItemComparer comparer = value as VirtualItemComparer;
            if ((destinationType == typeof(string)) && (comparer != null))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(VirtualProperty.Get(comparer.ComparePropertyId).PropertyName);
                builder.Append(' ');
                builder.Append((comparer.SortDirection == ListSortDirection.Descending) ? "Desc" : "Asc");
                foreach (NameComparison comparison in Enum.GetValues(typeof(NameComparison)))
                {
                    if ((comparer.NameComparison & comparison) > NameComparison.Default)
                    {
                        builder.Append(',');
                        builder.Append(comparison);
                    }
                }
                return builder.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

