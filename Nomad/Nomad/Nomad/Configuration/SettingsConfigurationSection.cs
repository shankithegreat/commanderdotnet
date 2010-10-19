namespace Nomad.Configuration
{
    using System;
    using System.Configuration;

    public class SettingsConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("exeConfigFileName")]
        public string ExeConfigFilename
        {
            get
            {
                return (string) base["exeConfigFileName"];
            }
        }

        [ConfigurationProperty("localUserConfigDir")]
        public string LocalUserConfigDir
        {
            get
            {
                return (string) base["localUserConfigDir"];
            }
            set
            {
                base["localUserConfigDir"] = value;
            }
        }

        [ConfigurationProperty("localUserConfigFileName")]
        public string LocalUserConfigFilename
        {
            get
            {
                return (string) base["localUserConfigFileName"];
            }
        }

        [ConfigurationProperty("roamingUserConfigDir")]
        public string RoamingUserConfigDir
        {
            get
            {
                return (string) base["roamingUserConfigDir"];
            }
            set
            {
                base["roamingUserConfigDir"] = value;
            }
        }

        [ConfigurationProperty("roamingUserConfigFileName")]
        public string RoamingUserConfigFilename
        {
            get
            {
                return (string) base["roamingUserConfigFileName"];
            }
        }

        [ConfigurationProperty("userLevel", DefaultValue=20)]
        public ConfigurationUserLevel UserLevel
        {
            get
            {
                return (ConfigurationUserLevel) base["userLevel"];
            }
            set
            {
                base["userLevel"] = value;
            }
        }
    }
}

