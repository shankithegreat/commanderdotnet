namespace TagLib
{
    using System;
    using System.Collections.Generic;

    public static class FileTypes
    {
        private static Dictionary<string, Type> file_types;
        private static Type[] static_file_types = new Type[] { typeof(File), typeof(File), typeof(File), typeof(File), typeof(File), typeof(File), typeof(File), typeof(File), typeof(AudioFile), typeof(File), typeof(File), typeof(File) };

        static FileTypes()
        {
            Init();
        }

        internal static void Init()
        {
            if (file_types == null)
            {
                file_types = new Dictionary<string, Type>();
                foreach (Type type in static_file_types)
                {
                    Register(type);
                }
            }
        }

        public static void Register(Type type)
        {
            Attribute[] customAttributes = Attribute.GetCustomAttributes(type, typeof(SupportedMimeType));
            if ((customAttributes != null) && (customAttributes.Length != 0))
            {
                foreach (SupportedMimeType type2 in customAttributes)
                {
                    file_types.Add(type2.MimeType, type);
                }
            }
        }

        public static IDictionary<string, Type> AvailableTypes
        {
            get
            {
                return file_types;
            }
        }
    }
}

