namespace Nomad.Commons.Resources
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.CompilerServices;

    public static class ResourceSetExtender
    {
        public static void ApplyResources(this ResourceSet set, object value, string objectName)
        {
            BindingFlags bindingAttr = BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance;
            SortedList<string, object> list = new SortedList<string, object>();
            foreach (DictionaryEntry entry in set)
            {
                list[(string) entry.Key] = entry.Value;
            }
            foreach (KeyValuePair<string, object> pair in list)
            {
                string key = pair.Key;
                if ((key != null) && string.Equals(key, objectName, StringComparison.Ordinal))
                {
                    int length = objectName.Length;
                    if ((key.Length > length) && (key[length] == '.'))
                    {
                        string name = key.Substring(length + 1);
                        PropertyInfo property = null;
                        try
                        {
                            property = value.GetType().GetProperty(name, bindingAttr);
                        }
                        catch (AmbiguousMatchException)
                        {
                            Type baseType = value.GetType();
                            do
                            {
                                property = baseType.GetProperty(name, bindingAttr | BindingFlags.DeclaredOnly);
                                baseType = baseType.BaseType;
                            }
                            while (((property == null) && (baseType != null)) && (baseType != typeof(object)));
                        }
                        if (((property != null) && property.CanWrite) && ((pair.Value == null) || property.PropertyType.IsInstanceOfType(pair.Value)))
                        {
                            property.SetValue(value, pair.Value, null);
                        }
                    }
                }
            }
        }
    }
}

