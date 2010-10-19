namespace Nomad.Globalization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Resources;

    public class ResourceEnumConverter : EnumConverter
    {
        private Dictionary<CultureInfo, LookupTable> _lookupTables;
        private ResourceManager _resourceManager;

        public ResourceEnumConverter(Type type, ResourceManager resourceManager) : base(type)
        {
            this._lookupTables = new Dictionary<CultureInfo, LookupTable>();
            this._resourceManager = resourceManager;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                LookupTable lookupTable = this.GetLookupTable(CultureInfo.CurrentUICulture);
                object obj2 = null;
                if (!lookupTable.TryGetValue(value as string, out obj2))
                {
                    obj2 = base.ConvertFrom(context, culture, value);
                }
                return obj2;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (((value != null) && (destinationType == typeof(string))) && (culture != CultureInfo.InvariantCulture))
            {
                Type type = value.GetType();
                string name = string.Format("{0}_{1}", type.Name, value.ToString());
                string str2 = this._resourceManager.GetString(name);
                if (str2 == null)
                {
                    str2 = value.ToString();
                }
                return str2;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        private LookupTable GetLookupTable(CultureInfo culture)
        {
            LookupTable table = null;
            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }
            if (!this._lookupTables.TryGetValue(culture, out table))
            {
                table = new LookupTable();
                foreach (object obj2 in base.GetStandardValues())
                {
                    string key = base.ConvertToString(null, culture, obj2);
                    if (key != null)
                    {
                        table.Add(key, obj2);
                    }
                }
                this._lookupTables.Add(culture, table);
            }
            return table;
        }

        private class LookupTable : Dictionary<string, object>
        {
        }
    }
}

