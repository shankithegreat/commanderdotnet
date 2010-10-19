namespace Nomad.FileSystem.Property
{
    using Microsoft.IO;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Security.AccessControl;
    using System.Windows.Forms;

    public static class DefaultProperty
    {
        public const int ACL = 14;
        public const int Attributes = 6;
        public const int CompressedSize = 5;
        public const int CompressionMethod = 20;
        public const int Copyright = 0x13;
        public const int CRC = 0x18;
        public const int CreationTime = 7;
        public const int CustomizeFolder = 0x17;
        public const int Description = 11;
        public const int Extension = 1;
        private static VirtualPropertySet FDefaultSet;
        public const int FileSystem = 0x1c;
        public const int FileVersion = 0x12;
        private static VirtualPropertySet FNameAttrPropertySet;
        public const int FreeSize = 0x1b;
        private static Func<VirtualProperty, string> GetLocalizedNameCallback = new Func<VirtualProperty, string>(DefaultProperty.GetLocalizedPropertyName);
        public const int LastAccessTime = 9;
        private static int LastDefaultPropertyId;
        public const int LastWriteTime = 8;
        public const int LinkTarget = 10;
        public const int Location = 12;
        public const int MD5 = 0x19;
        public const int Name = 0;
        public const int NumberOfDataStreams = 0x11;
        public const int NumberOfHardLinks = 0x10;
        public const int Owner = 15;
        public const int Relevance = 13;
        public static System.Resources.ResourceManager ResourceManager;
        public const int ShortcutKeys = 0x16;
        public const int Size = 3;
        public const int SizeOnDisk = 4;
        public const int Thumbnail = 0x15;
        public const int TotalSize = 0x1a;
        public const int Type = 2;
        public const int VolumeLabel = 30;
        public const int VolumeType = 0x1d;

        public static VirtualPropertySet FromNotifyFilters(NotifyFilters filters)
        {
            VirtualPropertySet set = new VirtualPropertySet();
            if ((filters & NotifyFilters.FileName) > 0)
            {
                set[0] = true;
            }
            if ((filters & NotifyFilters.Attributes) > 0)
            {
                set[6] = true;
            }
            if ((filters & NotifyFilters.Size) > 0)
            {
                set[3] = true;
            }
            if ((filters & NotifyFilters.LastWrite) > 0)
            {
                set[8] = true;
            }
            if ((filters & NotifyFilters.LastAccess) > 0)
            {
                set[9] = true;
            }
            if ((filters & NotifyFilters.CreationTime) > 0)
            {
                set[7] = true;
            }
            if ((filters & NotifyFilters.Security) > 0)
            {
                set[14] = true;
            }
            return set;
        }

        private static string GetLocalizedPropertyName(VirtualProperty property)
        {
            if (ResourceManager != null)
            {
                string str = ResourceManager.GetString("VirtualProperty_" + property.PropertyName);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
            }
            return property.PropertyName;
        }

        public static void Initialize()
        {
            int groupId = VirtualProperty.RegisterGroup("Default");
            RegisterProperty("Name", groupId, typeof(string), -1, null, 0);
            RegisterProperty("Extension", groupId, typeof(string), 4, null, 0);
            RegisterProperty("Type", groupId, typeof(string), -1, null, 0);
            RegisterProperty("Size", groupId, typeof(long), -1, SizeTypeConverter.Default, 0);
            RegisterProperty("SizeOnDisk", groupId, typeof(long), -1, SizeTypeConverter.Default, 0);
            RegisterProperty("CompressedSize", groupId, typeof(long), -1, SizeTypeConverter.Default, 0);
            RegisterProperty("Attributes", groupId, typeof(FileAttributes), -1, new AttributesTypeConverter(), 0);
            RegisterProperty("CreationTime", groupId, typeof(DateTime), -1, DateTimeTypeConverter.Default, 0);
            RegisterProperty("LastWriteTime", groupId, typeof(DateTime), -1, DateTimeTypeConverter.Default, 0);
            RegisterProperty("LastAccessTime", groupId, typeof(DateTime), -1, DateTimeTypeConverter.Default, 0);
            RegisterProperty("LinkTarget", groupId, typeof(string), -1, null, 0);
            RegisterProperty("Description", groupId, typeof(string), -1, null, 0);
            RegisterProperty("Location", groupId, typeof(string), -1, null, 0);
            RegisterProperty("Relevance", groupId, typeof(byte), 3, new RelevanceTypeConverter(), 0);
            RegisterProperty("ACL", groupId, typeof(FileSystemSecurity), -1, null, VirtualPropertyOption.Hidden);
            RegisterProperty("Owner", groupId, typeof(string), -1, null, 0);
            RegisterProperty("NumberOfHardLinks", groupId, typeof(int), 2, null, 0);
            RegisterProperty("NumberOfDataStreams", groupId, typeof(int), 2, null, 0);
            RegisterProperty("FileVersion", groupId, typeof(Version), -1, null, 0);
            RegisterProperty("Copyright", groupId, typeof(string), -1, null, 0);
            RegisterProperty("CompressionMethod", groupId, typeof(string), 6, null, 0);
            RegisterProperty("Thumbnail", groupId, typeof(Image), -1, null, VirtualPropertyOption.Hidden);
            RegisterProperty("ShortcutKeys", groupId, typeof(Keys), -1, null, VirtualPropertyOption.Hidden);
            RegisterProperty("CustomizeFolder", groupId, typeof(object), -1, null, VirtualPropertyOption.Hidden);
            groupId = VirtualProperty.RegisterGroup("Hash");
            RegisterProperty("CRC", groupId, typeof(int), 8, CrcTypeConveter.Default, 0);
            RegisterProperty("MD5", groupId, typeof(byte[]), 0x20, HashConverter.Default, 0);
            groupId = VirtualProperty.RegisterGroup("Volume");
            RegisterProperty("TotalSize", groupId, typeof(long), -1, SizeTypeConverter.Default, 0);
            RegisterProperty("FreeSize", groupId, typeof(long), -1, SizeTypeConverter.Default, 0);
            RegisterProperty("FileSystem", groupId, typeof(string), 5, null, 0);
            RegisterProperty("VolumeType", groupId, typeof(Microsoft.IO.VolumeType), -1, null, 0);
            RegisterProperty("VolumeLabel", groupId, typeof(string), 8, null, 0);
            LastDefaultPropertyId = 30;
        }

        public static int RegisterProperty(string name, int groupId, System.Type type, int length)
        {
            return RegisterProperty(name, groupId, type, length, null, 0);
        }

        public static int RegisterProperty(string name, int groupId, System.Type type, int length, VirtualPropertyOption options)
        {
            return RegisterProperty(name, groupId, type, length, null, options);
        }

        public static int RegisterProperty(string name, int groupId, System.Type type, int length, TypeConverter converter, VirtualPropertyOption options)
        {
            VirtualProperty property = VirtualProperty.Get(name);
            if (property != null)
            {
                return property.PropertyId;
            }
            return VirtualProperty.RegisterProperty(name, groupId, type, length, converter, options, new Func<VirtualProperty, string>(DefaultProperty.GetLocalizedPropertyName));
        }

        public static VirtualPropertySet DefaultSet
        {
            get
            {
                if (FDefaultSet == null)
                {
                    FDefaultSet = new VirtualPropertySet();
                    for (int i = 0; i <= LastDefaultPropertyId; i++)
                    {
                        FDefaultSet[i] = true;
                    }
                    FDefaultSet = FDefaultSet.AsReadOnly();
                }
                return FDefaultSet;
            }
        }

        public static VirtualPropertySet NameAttrPropertySet
        {
            get
            {
                int[] numArray;
                if (FNameAttrPropertySet == null)
                {
                    VirtualPropertySet fNameAttrPropertySet = FNameAttrPropertySet;
                    numArray = new int[2];
                    numArray[1] = 6;
                }
                return (FNameAttrPropertySet = new VirtualPropertySet(numArray));
            }
        }
    }
}

