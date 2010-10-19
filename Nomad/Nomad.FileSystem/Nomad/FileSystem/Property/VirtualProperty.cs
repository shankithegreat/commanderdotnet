namespace Nomad.FileSystem.Property
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Resources;

    public class VirtualProperty
    {
        private static VirtualPropertySet FAllSet;
        public bool FPropertyVisible = true;
        private static VirtualPropertySet FVisibleSet;
        private Func<VirtualProperty, string> GetLocalizedNameCallback;
        public readonly int GroupId;
        private static List<string> GroupList = new List<string>();
        public readonly TypeConverter PropertyConverter;
        public readonly int PropertyId;
        public readonly int PropertyLength;
        internal static List<VirtualProperty> PropertyList = new List<VirtualProperty>();
        private static Dictionary<string, VirtualProperty> PropertyMap = new Dictionary<string, VirtualProperty>();
        public readonly string PropertyName;
        public readonly VirtualPropertyOption PropertyOptions;
        public readonly Type PropertyType;
        public static System.Resources.ResourceManager ResourceManager;

        private VirtualProperty(int id, int groupId, string name, Type type, int length, TypeConverter converter, VirtualPropertyOption options, Func<VirtualProperty, string> getLocalizedNameCallback)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            if (name == string.Empty)
            {
                throw new ArgumentException();
            }
            this.PropertyId = id;
            this.GroupId = groupId;
            this.PropertyName = name;
            this.PropertyType = type;
            this.PropertyLength = length;
            this.PropertyConverter = converter;
            this.PropertyOptions = options;
            this.GetLocalizedNameCallback = getLocalizedNameCallback;
        }

        public string ConvertToString(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (this.PropertyConverter != null)
            {
                return this.PropertyConverter.ConvertToString(value);
            }
            if (this.PropertyType.IsEnum)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(value);
                if (converter != null)
                {
                    return converter.ConvertToString(value);
                }
            }
            return value.ToString();
        }

        public static string ConvertToString(int propertyId, object value)
        {
            if (value == null)
            {
                return null;
            }
            VirtualProperty property = Get(propertyId);
            if (property == null)
            {
                return null;
            }
            return property.ConvertToString(value);
        }

        public static VirtualProperty Get(int propertyId)
        {
            return PropertyList[propertyId];
        }

        public static VirtualProperty Get(string propertyName)
        {
            VirtualProperty property;
            if ((propertyName != null) && PropertyMap.TryGetValue(propertyName, out property))
            {
                return property;
            }
            return null;
        }

        public static string GetGroup(int groupId)
        {
            return GroupList[groupId];
        }

        public static int GetGroup(string groupName)
        {
            for (int i = 0; i < GroupList.Count; i++)
            {
                if (GroupList[i] == groupName)
                {
                    return i;
                }
            }
            return -1;
        }

        public static VirtualPropertySet GetGroupSet(int groupId)
        {
            VirtualPropertySet set = new VirtualPropertySet();
            for (int i = 0; i < PropertyList.Count; i++)
            {
                if (PropertyList[i].GroupId == groupId)
                {
                    set[i] = true;
                }
            }
            return set;
        }

        private static string GetLocalizedName(string prefix, string name)
        {
            if (ResourceManager == null)
            {
                return name;
            }
            string str = ResourceManager.GetString(prefix + name);
            return (string.IsNullOrEmpty(str) ? name : str);
        }

        public static bool IsSlowProperty(int propertyId)
        {
            return ((Get(propertyId).PropertyOptions & VirtualPropertyOption.Slow) > 0);
        }

        public static bool IsVisibleProperty(int propertyId)
        {
            return Visible[propertyId];
        }

        public static int RegisterGroup(string name)
        {
            int index = GroupList.IndexOf(name);
            if (index < 0)
            {
                GroupList.Add(name);
                index = GroupList.Count - 1;
            }
            return index;
        }

        public static int RegisterProperty(string name, int groupId, Type type, int length, TypeConverter converter, VirtualPropertyOption options, Func<VirtualProperty, string> getLocalizedNameCallback)
        {
            VirtualProperty property;
            if (PropertyMap.TryGetValue(name, out property))
            {
                throw new ArgumentException(string.Format("Property with name '{0}' already registered.", name));
            }
            if ((converter == null) && type.IsEnum)
            {
                converter = TypeDescriptor.GetConverter(type);
            }
            property = new VirtualProperty(PropertyList.Count, groupId, name, type, length, converter, options, getLocalizedNameCallback);
            PropertyList.Add(property);
            PropertyMap.Add(name, property);
            FAllSet = null;
            FVisibleSet = null;
            return property.PropertyId;
        }

        public static VirtualPropertySet All
        {
            get
            {
                if (FAllSet == null)
                {
                    FAllSet = new VirtualPropertySet();
                    FAllSet.Not();
                    FAllSet = FAllSet.AsReadOnly();
                }
                return FAllSet;
            }
        }

        public string GroupName
        {
            get
            {
                return GetGroup(this.GroupId);
            }
        }

        public string LocalizedGroupName
        {
            get
            {
                return GetLocalizedName("Group_", this.GroupName);
            }
        }

        public string LocalizedName
        {
            get
            {
                if (this.GetLocalizedNameCallback != null)
                {
                    return this.GetLocalizedNameCallback(this);
                }
                string str = this.GroupName + '.';
                if (this.PropertyName.StartsWith(str, StringComparison.Ordinal))
                {
                    return this.PropertyName.Substring(str.Length);
                }
                return this.PropertyName;
            }
        }

        public bool PropertyVisible
        {
            get
            {
                return (((this.PropertyOptions & VirtualPropertyOption.Hidden) == 0) && this.FPropertyVisible);
            }
            set
            {
                if (this.FPropertyVisible != value)
                {
                    this.FPropertyVisible = value;
                    FVisibleSet = null;
                }
            }
        }

        public static VirtualPropertySet Visible
        {
            get
            {
                if (FVisibleSet == null)
                {
                    FVisibleSet = new VirtualPropertySet();
                    for (int i = 0; i < PropertyList.Count; i++)
                    {
                        if (PropertyList[i].PropertyVisible)
                        {
                            FVisibleSet[i] = true;
                        }
                    }
                    FVisibleSet = FVisibleSet.AsReadOnly();
                }
                return FVisibleSet;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                for (int i = 0; i < PropertyList.Count; i++)
                {
                    PropertyList[i].PropertyVisible = value[i];
                }
            }
        }
    }
}

