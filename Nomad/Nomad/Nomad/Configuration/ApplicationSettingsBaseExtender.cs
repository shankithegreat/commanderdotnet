namespace Nomad.Configuration
{
    using System;
    using System.Configuration;
    using System.Runtime.CompilerServices;

    public static class ApplicationSettingsBaseExtender
    {
        public static void CompactProperties(this ApplicationSettingsBase config)
        {
            foreach (SettingsProperty property in config.Properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    string defaultValue = property.DefaultValue as string;
                    if (defaultValue != null)
                    {
                        property.DefaultValue = (defaultValue.Length == 0) ? string.Empty : string.Intern(defaultValue);
                    }
                }
            }
        }

        public static void CompactPropertyValues(this ApplicationSettingsBase config)
        {
            foreach (SettingsPropertyValue value2 in config.PropertyValues)
            {
                if (value2.Property.PropertyType == typeof(string))
                {
                    string defaultValue = value2.Property.DefaultValue as string;
                    if ((defaultValue != null) && string.Equals(defaultValue, value2.PropertyValue as string, StringComparison.Ordinal))
                    {
                        bool isDirty = value2.IsDirty;
                        value2.SerializedValue = null;
                        value2.PropertyValue = (defaultValue.Length == 0) ? string.Empty : string.Intern(defaultValue);
                        value2.IsDirty = isDirty;
                    }
                }
            }
        }
    }
}

