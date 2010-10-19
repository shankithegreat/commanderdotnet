namespace Nomad.Configuration
{
    using System;
    using System.ComponentModel;

    public static class XmlSerializable
    {
        public static string ObjectToString<T>(T value)
        {
            if (value.Equals(default(T)))
            {
                return null;
            }
            return TypeDescriptor.GetConverter(typeof(T)).ConvertToInvariantString(value);
        }

        public static T StringToObject<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value);
        }
    }
}

