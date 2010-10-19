namespace Nomad.Configuration
{
    using Nomad.Commons;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;

    public class ConfigurableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        private const string ApplicationSettingsGroupName = "applicationSettings";
        private const string ApplicationSettingsGroupPrefix = "applicationSettings/";
        private string AppName = string.Empty;
        private static string FPreviousUserConfigPath;
        private static string FUserConfigPath;
        public const string SettingsConfigurationGroupName = "settingsConfiguration";
        private static ExeConfigurationFileMap SettingsFileMap;
        public const string SettingsProviderSectionName = "settingsConfiguration/settingsProvider";
        private static Dictionary<string, string> SpecialFolderMap;
        public const string SpecialFolderSectionName = "settingsConfiguration/specialFolders";
        private const string UserConfigName = "user.config";
        private const string UserSettingsGroupName = "userSettings";
        private const string UserSettingsGroupPrefix = "userSettings/";
        private static ConfigurationUserLevel UserSettingsLevel;
        private XmlElement XmlDummy = new XmlDocument().CreateElement("dummy");

        private ClientSettingsSection DeclareUserSection(System.Configuration.Configuration config, string sectionName)
        {
            ConfigurationSectionGroup sectionGroup = config.GetSectionGroup("userSettings");
            if (sectionGroup == null)
            {
                sectionGroup = new UserSettingsGroup();
                config.SectionGroups.Add("userSettings", sectionGroup);
            }
            bool flag = false;
            ConfigurationSection section = sectionGroup.Sections[sectionName];
            if (section == null)
            {
                section = new ClientSettingsSection();
                flag = true;
            }
            else if (section is DefaultSection)
            {
                section = this.ReadClientSettingsSection(section);
                if (section != null)
                {
                    sectionGroup.Sections.Remove(sectionName);
                    flag = true;
                }
            }
            if (flag)
            {
                section.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;
                section.SectionInformation.RequirePermission = false;
                sectionGroup.Sections.Add(sectionName, section);
            }
            return (section as ClientSettingsSection);
        }

        private string Escape(string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString))
            {
                return xmlString;
            }
            this.XmlDummy.InnerText = xmlString;
            return this.XmlDummy.InnerXml;
        }

        private System.Configuration.Configuration GetClientConfig(ConfigurationUserLevel userLevel)
        {
            if (UseDefaultConfig(SettingsFileMap, userLevel))
            {
                return ConfigurationManager.OpenExeConfiguration(userLevel);
            }
            return ConfigurationManager.OpenMappedExeConfiguration(SettingsFileMap, userLevel);
        }

        public static string GetDefaultExeConfigPath(ConfigurationUserLevel userLevel)
        {
            try
            {
                return ConfigurationManager.OpenExeConfiguration(userLevel).FilePath;
            }
            catch (ConfigurationException exception)
            {
                return exception.Filename;
            }
        }

        private static System.Configuration.Configuration GetFileConfig(string fileName)
        {
            return ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = fileName }, ConfigurationUserLevel.None);
        }

        private static string GetPreviousUserConfigPath(ConfigurationUserLevel userLevel)
        {
            Version version;
            if (userLevel == ConfigurationUserLevel.None)
            {
                return null;
            }
            string defaultExeConfigPath = GetDefaultExeConfigPath(userLevel);
            string directoryName = Path.GetDirectoryName(defaultExeConfigPath);
            defaultExeConfigPath = Path.GetFileName(defaultExeConfigPath);
            string path = Path.GetDirectoryName(directoryName);
            if (!Directory.Exists(path))
            {
                return null;
            }
            if (!VersionHelper.TryParse(Path.GetFileName(directoryName), VersionStyles.AllowMajorMinorBuildRevision, out version))
            {
                return null;
            }
            List<Version> source = new List<Version>();
            foreach (string str4 in Directory.GetDirectories(path))
            {
                Version version2;
                if (VersionHelper.TryParse(Path.GetFileName(str4), VersionStyles.AllowMajorMinorBuildRevision, out version2) && (version2 != version))
                {
                    source.Add(version2);
                }
            }
            if (source.Count == 0)
            {
                return null;
            }
            if (source.Count > 1)
            {
                source.Sort();
            }
            return Path.Combine(Path.Combine(path, source.Last<Version>().ToString()), defaultExeConfigPath);
        }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            string previousUserConfigPath = PreviousUserConfigPath;
            if (!string.IsNullOrEmpty(previousUserConfigPath))
            {
                SettingsPropertyCollection properties = new SettingsPropertyCollection();
                properties.Add(property);
                return this.GetPropertyValuesFromFile(previousUserConfigPath, context, properties)[property.Name];
            }
            return new SettingsPropertyValue(property) { PropertyValue = null };
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
        {
            string sectionName = GetSectionName(context);
            Dictionary<string, SettingElement> dictionary = null;
            Dictionary<string, SettingElement> dictionary2 = null;
            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();
            foreach (SettingsProperty property in properties)
            {
                Dictionary<string, SettingElement> dictionary3;
                SettingElement element;
                if (IsUserSetting(property))
                {
                    if (dictionary2 == null)
                    {
                        dictionary2 = this.ReadSettings(UserSettingsLevel, sectionName);
                    }
                    dictionary3 = dictionary2;
                }
                else
                {
                    if (dictionary == null)
                    {
                        dictionary = this.ReadSettings(ConfigurationUserLevel.None, sectionName);
                    }
                    dictionary3 = dictionary;
                }
                SettingsPropertyValue value2 = new SettingsPropertyValue(property);
                if (dictionary3.TryGetValue(property.Name, out element))
                {
                    string innerXml = element.Value.ValueXml.InnerXml;
                    if (element.SerializeAs == SettingsSerializeAs.String)
                    {
                        innerXml = this.Unescape(innerXml);
                    }
                    if ((property.PropertyType == typeof(string)) && (innerXml.Length == 0))
                    {
                        value2.PropertyValue = string.Empty;
                    }
                    else
                    {
                        value2.SerializedValue = innerXml;
                    }
                }
                else if (property.DefaultValue != null)
                {
                    value2.SerializedValue = property.DefaultValue;
                }
                else
                {
                    value2.PropertyValue = null;
                }
                value2.IsDirty = false;
                values.Add(value2);
            }
            return values;
        }

        private SettingsPropertyValueCollection GetPropertyValuesFromFile(string fileName, SettingsContext context, SettingsPropertyCollection properties)
        {
            System.Configuration.Configuration fileConfig = GetFileConfig(fileName);
            Dictionary<string, SettingElement> dictionary = this.ReadSettings(fileConfig, "userSettings/" + GetSectionName(context));
            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();
            foreach (SettingsProperty property in properties)
            {
                SettingElement element;
                SettingsPropertyValue value2 = new SettingsPropertyValue(property);
                if (dictionary.TryGetValue(property.Name, out element))
                {
                    string innerXml = element.Value.ValueXml.InnerXml;
                    if (element.SerializeAs == SettingsSerializeAs.String)
                    {
                        innerXml = this.Unescape(innerXml);
                    }
                    value2.SerializedValue = innerXml;
                    value2.IsDirty = true;
                    values.Add(value2);
                }
            }
            return values;
        }

        private static string GetSectionName(SettingsContext context)
        {
            string str = (string) context["GroupName"];
            string str2 = (string) context["SettingsKey"];
            System.Diagnostics.Debug.Assert(str != null, "SettingsContext did not have a GroupName!");
            string name = str;
            if (!string.IsNullOrEmpty(str2))
            {
                name = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[] { name, str2 });
            }
            return XmlConvert.EncodeLocalName(name);
        }

        public static string GetSpecialFolder(string key)
        {
            string str2;
            if (SpecialFolderMap == null)
            {
                SpecialFolderMap = new Dictionary<string, string>();
                NameValueCollection section = ConfigurationManager.GetSection("settingsConfiguration/specialFolders") as NameValueCollection;
                if (section != null)
                {
                    foreach (string str in section.AllKeys)
                    {
                        SpecialFolderMap.Add(str, Path.GetFullPath(Environment.ExpandEnvironmentVariables(section[str])));
                    }
                }
            }
            if (SpecialFolderMap.TryGetValue(key, out str2))
            {
                return str2;
            }
            return null;
        }

        private ClientSettingsSection GetUserSection(System.Configuration.Configuration config, string sectionName, bool declare)
        {
            string str = "userSettings/" + sectionName;
            ClientSettingsSection section = null;
            if (config != null)
            {
                section = config.GetSection(str) as ClientSettingsSection;
                if ((section == null) && declare)
                {
                    section = this.DeclareUserSection(config, sectionName);
                }
            }
            return section;
        }

        private static ConfigurationUserLevel GetValidUserLevel(ConfigurationUserLevel userLevel)
        {
            switch (userLevel)
            {
                case ConfigurationUserLevel.PerUserRoaming:
                case ConfigurationUserLevel.PerUserRoamingAndLocal:
                    return userLevel;
            }
            return ConfigurationUserLevel.PerUserRoamingAndLocal;
        }

        private static void Initialize()
        {
            if (UserSettingsLevel == ConfigurationUserLevel.None)
            {
                SettingsConfigurationSection configSection = ConfigurationManager.GetSection("settingsConfiguration/settingsProvider") as SettingsConfigurationSection;
                if (configSection != null)
                {
                    UserSettingsLevel = configSection.UserLevel;
                    SettingsFileMap = ReadConfigurationFileMap(configSection);
                }
                UserSettingsLevel = GetValidUserLevel(UserSettingsLevel);
                if (UseDefaultConfig(SettingsFileMap, UserSettingsLevel))
                {
                    FUserConfigPath = GetDefaultExeConfigPath(UserSettingsLevel);
                }
                else if (UserSettingsLevel == ConfigurationUserLevel.PerUserRoaming)
                {
                    FUserConfigPath = SettingsFileMap.RoamingUserConfigFilename;
                }
                else
                {
                    FUserConfigPath = SettingsFileMap.LocalUserConfigFilename;
                }
            }
        }

        public override void Initialize(string name, NameValueCollection values)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "ConfigurableSettingsProvider";
            }
            Initialize();
            base.Initialize(name, values);
        }

        private static bool IsUserSetting(SettingsProperty setting)
        {
            bool flag = setting.Attributes[typeof(UserScopedSettingAttribute)] is UserScopedSettingAttribute;
            bool flag2 = setting.Attributes[typeof(ApplicationScopedSettingAttribute)] is ApplicationScopedSettingAttribute;
            if (flag && flag2)
            {
                throw new ConfigurationErrorsException("Both scope attributes provided");
            }
            if (!(flag || flag2))
            {
                throw new ConfigurationErrorsException("No scope attributes provided");
            }
            return flag;
        }

        private ClientSettingsSection ReadClientSettingsSection(ConfigurationSection section)
        {
            ClientSettingsSection section2 = section as ClientSettingsSection;
            if ((section2 == null) && (section is DefaultSection))
            {
                MethodInfo info = typeof(ClientSettingsSection).GetMethod("DeserializeSection", BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[] { typeof(XmlReader) }, null);
                if (info == null)
                {
                    return section2;
                }
                section2 = new ClientSettingsSection();
                using (XmlReader reader = XmlReader.Create(new StringReader(section.SectionInformation.GetRawXml())))
                {
                    info.Invoke(section2, new object[] { reader });
                }
            }
            return section2;
        }

        private static ExeConfigurationFileMap ReadConfigurationFileMap(SettingsConfigurationSection configSection)
        {
            if (configSection == null)
            {
                return null;
            }
            Directory.SetCurrentDirectory(Application.StartupPath);
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            try
            {
                map.ExeConfigFilename = Environment.ExpandEnvironmentVariables(configSection.ExeConfigFilename);
                flag = !string.IsNullOrEmpty(map.ExeConfigFilename);
                if (!(!flag || Path.IsPathRooted(map.ExeConfigFilename)))
                {
                    map.ExeConfigFilename = Path.GetFullPath(map.ExeConfigFilename);
                }
            }
            catch (ArgumentException)
            {
            }
            try
            {
                map.LocalUserConfigFilename = Environment.ExpandEnvironmentVariables(configSection.LocalUserConfigFilename);
                if (!(!string.IsNullOrEmpty(map.LocalUserConfigFilename) || string.IsNullOrEmpty(configSection.LocalUserConfigDir)))
                {
                    map.LocalUserConfigFilename = Path.Combine(Environment.ExpandEnvironmentVariables(configSection.LocalUserConfigDir), "user.config");
                }
                flag2 = !string.IsNullOrEmpty(map.LocalUserConfigFilename);
                if (!(!flag2 || Path.IsPathRooted(map.LocalUserConfigFilename)))
                {
                    map.LocalUserConfigFilename = Path.GetFullPath(map.LocalUserConfigFilename);
                }
            }
            catch (ArgumentException)
            {
            }
            try
            {
                map.RoamingUserConfigFilename = Environment.ExpandEnvironmentVariables(configSection.RoamingUserConfigFilename);
                if (!(!string.IsNullOrEmpty(map.RoamingUserConfigFilename) || string.IsNullOrEmpty(configSection.RoamingUserConfigDir)))
                {
                    map.RoamingUserConfigFilename = Path.Combine(Environment.ExpandEnvironmentVariables(configSection.RoamingUserConfigDir), "user.config");
                }
                flag3 = !string.IsNullOrEmpty(map.RoamingUserConfigFilename);
                if (!(!flag3 || Path.IsPathRooted(map.RoamingUserConfigFilename)))
                {
                    map.RoamingUserConfigFilename = Path.GetFullPath(map.RoamingUserConfigFilename);
                }
            }
            catch (ArgumentException)
            {
            }
            if (!(flag || (!flag2 && !flag3)))
            {
                map.ExeConfigFilename = Application.ExecutablePath + ".config";
            }
            return map;
        }

        private Dictionary<string, SettingElement> ReadSettings(System.Configuration.Configuration clientConfig, string sectionName)
        {
            Dictionary<string, SettingElement> dictionary = new Dictionary<string, SettingElement>();
            ClientSettingsSection section = this.ReadClientSettingsSection(clientConfig.GetSection(sectionName));
            if (section != null)
            {
                foreach (SettingElement element in section.Settings)
                {
                    dictionary.Add(element.Name, element);
                }
            }
            return dictionary;
        }

        private Dictionary<string, SettingElement> ReadSettings(ConfigurationUserLevel userLevel, string sectionName)
        {
            System.Configuration.Configuration clientConfig = this.GetClientConfig(SkipUserConfig ? ConfigurationUserLevel.None : userLevel);
            string str = (userLevel == ConfigurationUserLevel.None) ? "applicationSettings/" : "userSettings/";
            return this.ReadSettings(clientConfig, str + sectionName);
        }

        public static void Reinitialize()
        {
            UserSettingsLevel = ConfigurationUserLevel.None;
            SettingsFileMap = null;
            SpecialFolderMap = null;
            ConfigurationManager.RefreshSection("settingsConfiguration/settingsProvider");
            ConfigurationManager.RefreshSection("settingsConfiguration/specialFolders");
            Initialize();
        }

        public void Reset(SettingsContext context)
        {
            this.RevertToParent(UserSettingsLevel, GetSectionName(context));
        }

        private void RevertToParent(ConfigurationUserLevel userLevel, string sectionName)
        {
            System.Configuration.Configuration clientConfig = this.GetClientConfig(userLevel);
            bool flag = false;
            if (UseDefaultConfig(SettingsFileMap, userLevel))
            {
                ClientSettingsSection section = this.GetUserSection(clientConfig, "userSettings/" + sectionName, false);
                if (section != null)
                {
                    section.SectionInformation.RevertToParent();
                    flag = true;
                }
            }
            else
            {
                ConfigurationSectionGroup sectionGroup = clientConfig.GetSectionGroup("userSettings");
                if (sectionGroup != null)
                {
                    sectionGroup.SectionGroups.Remove(sectionName);
                    flag = true;
                }
            }
            if (flag)
            {
                clientConfig.Save();
            }
        }

        private XmlNode SerializeToXmlElement(SettingsProperty setting, SettingsPropertyValue value)
        {
            XmlElement element = new XmlDocument().CreateElement("value");
            string serializedValue = value.SerializedValue as string;
            if ((serializedValue == null) && (setting.SerializeAs == SettingsSerializeAs.Binary))
            {
                byte[] inArray = value.SerializedValue as byte[];
                if (inArray != null)
                {
                    serializedValue = Convert.ToBase64String(inArray);
                }
            }
            if (serializedValue == null)
            {
                serializedValue = string.Empty;
            }
            if (setting.SerializeAs == SettingsSerializeAs.String)
            {
                serializedValue = this.Escape(serializedValue);
            }
            element.InnerXml = serializedValue;
            XmlNode oldChild = null;
            foreach (XmlNode node2 in element.ChildNodes)
            {
                if (node2.NodeType == XmlNodeType.XmlDeclaration)
                {
                    oldChild = node2;
                    break;
                }
            }
            if (oldChild != null)
            {
                element.RemoveChild(oldChild);
            }
            return element;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values)
        {
            string sectionName = GetSectionName(context);
            List<SettingsPropertyValue> newSettings = new List<SettingsPropertyValue>();
            foreach (SettingsPropertyValue value2 in values)
            {
                if (value2.IsDirty && IsUserSetting(value2.Property))
                {
                    newSettings.Add(value2);
                    value2.IsDirty = false;
                }
            }
            if (newSettings.Count > 0)
            {
                this.WriteSettings(sectionName, UserSettingsLevel, newSettings);
            }
        }

        private string Unescape(string escapedString)
        {
            if (string.IsNullOrEmpty(escapedString))
            {
                return escapedString;
            }
            this.XmlDummy.InnerXml = escapedString;
            return this.XmlDummy.InnerText;
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
            string previousUserConfigPath = PreviousUserConfigPath;
            if (!string.IsNullOrEmpty(previousUserConfigPath))
            {
                SettingsPropertyCollection propertys = new SettingsPropertyCollection();
                foreach (SettingsProperty property in properties)
                {
                    if (!(property.Attributes[typeof(NoSettingsVersionUpgradeAttribute)] is NoSettingsVersionUpgradeAttribute))
                    {
                        propertys.Add(property);
                    }
                }
                SettingsPropertyValueCollection collection = this.GetPropertyValuesFromFile(previousUserConfigPath, context, propertys);
                this.SetPropertyValues(context, collection);
            }
        }

        private static bool UseDefaultConfig(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel)
        {
            if (fileMap != null)
            {
                switch (userLevel)
                {
                    case ConfigurationUserLevel.None:
                        return string.IsNullOrEmpty(fileMap.ExeConfigFilename);

                    case ConfigurationUserLevel.PerUserRoaming:
                        return string.IsNullOrEmpty(fileMap.RoamingUserConfigFilename);

                    case ConfigurationUserLevel.PerUserRoamingAndLocal:
                        return string.IsNullOrEmpty(fileMap.LocalUserConfigFilename);
                }
            }
            return true;
        }

        private void WriteSettings(string sectionName, ConfigurationUserLevel userLevel, List<SettingsPropertyValue> newSettings)
        {
            System.Configuration.Configuration clientConfig = this.GetClientConfig(userLevel);
            ClientSettingsSection section = this.GetUserSection(clientConfig, sectionName, true);
            if (section == null)
            {
                throw new ConfigurationErrorsException("Failed to save settings. No settings section found");
            }
            foreach (SettingsPropertyValue value2 in newSettings)
            {
                SettingElement element = section.Settings.Get(value2.Name);
                if (element == null)
                {
                    element = new SettingElement {
                        Name = value2.Name
                    };
                    section.Settings.Add(element);
                }
                element.SerializeAs = value2.Property.SerializeAs;
                element.Value.ValueXml = this.SerializeToXmlElement(value2.Property, value2);
            }
            try
            {
                clientConfig.Save();
            }
            catch (ConfigurationErrorsException exception)
            {
                throw new ConfigurationErrorsException(string.Format("Failed to save settings. {0}", exception.Message), exception);
            }
        }

        public override string ApplicationName
        {
            get
            {
                return this.AppName;
            }
            set
            {
                this.AppName = value;
            }
        }

        public static string PreviousUserConfigPath
        {
            get
            {
                if (!string.IsNullOrEmpty(FPreviousUserConfigPath))
                {
                    return FPreviousUserConfigPath;
                }
                if (UseDefaultConfig(SettingsFileMap, UserSettingsLevel))
                {
                    return GetPreviousUserConfigPath(UserSettingsLevel);
                }
                return null;
            }
            set
            {
                FPreviousUserConfigPath = value;
            }
        }

        public static bool SkipUserConfig
        {
            [CompilerGenerated]
            get
            {
                return <SkipUserConfig>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <SkipUserConfig>k__BackingField = value;
            }
        }

        public static string UserConfigPath
        {
            get
            {
                Initialize();
                return FUserConfigPath;
            }
        }
    }
}

