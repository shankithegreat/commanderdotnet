namespace Nomad.Configuration
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [SettingsProvider(typeof(ConfigurableSettingsProvider)), CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed class HistorySettings : ApplicationSettingsBase
    {
        private static HistorySettings defaultInstance = ((HistorySettings) SettingsBase.Synchronized(new HistorySettings()));
        private const string PropertyChangeFolder = "ChangeFolder";
        private const string PropertyCopyFilter = "CopyFilter";
        private const string PropertyCopyTo = "CopyTo";
        private const string PropertyExcludeMasks = "ExcludeMasks";
        private const string PropertyFtpFolder = "FtpFolder";
        private const string PropertyIncludeMasks = "IncludeMasks";
        private const string PropertyLookFolder = "LookFolder";
        private const string PropertyMakeLinkName = "MakeLinkName";
        private const string PropertyNewFile = "NewFile";
        private const string PropertyNewFolderName = "NewFolderName";
        private const string PropertyPackTo = "PackTo";
        private const string PropertyRunAsArguments = "RunAsArguments";
        private const string PropertySearchContent = "SearchContent";
        private const string PropertySelectFileMasks = "SelectFileMasks";
        private const string PropertyUserName = "UserName";

        private void AddString(string propertyKey, string newValue)
        {
            if (!string.IsNullOrEmpty(newValue) && this.HistoryEnabled)
            {
                string[] collection = (string[]) this[propertyKey];
                if (collection == null)
                {
                    collection = new string[] { newValue };
                }
                else
                {
                    List<string> list = new List<string>(collection);
                    list.Remove(newValue);
                    list.Insert(0, newValue);
                    string[] array = new string[Math.Min(list.Count, 10)];
                    list.CopyTo(0, array, 0, array.Length);
                    collection = array;
                }
                this[propertyKey] = collection;
            }
        }

        public void AddStringToChangeFolder(string newValue)
        {
            this.AddString("ChangeFolder", newValue);
        }

        public void AddStringToCopyFilter(string newValue)
        {
            this.AddString("CopyFilter", newValue);
        }

        public void AddStringToCopyTo(string newValue)
        {
            this.AddString("CopyTo", newValue);
        }

        public void AddStringToExcludeMasks(string newValue)
        {
            this.AddString("ExcludeMasks", newValue);
        }

        public void AddStringToFtpFolder(string newValue)
        {
            this.AddString("FtpFolder", newValue);
        }

        public void AddStringToIncludeMasks(string newValue)
        {
            this.AddString("IncludeMasks", newValue);
        }

        public void AddStringToLookFolder(string newValue)
        {
            this.AddString("LookFolder", newValue);
        }

        public void AddStringToMakeLinkName(string newValue)
        {
            this.AddString("MakeLinkName", newValue);
        }

        public void AddStringToNewFile(string newValue)
        {
            this.AddString("NewFile", newValue);
        }

        public void AddStringToNewFolderName(string newValue)
        {
            this.AddString("NewFolderName", newValue);
        }

        public void AddStringToPackTo(string newValue)
        {
            this.AddString("PackTo", newValue);
        }

        public void AddStringToRunAsArguments(string newValue)
        {
            this.AddString("RunAsArguments", newValue);
        }

        public void AddStringToSearchContent(string newValue)
        {
            this.AddString("SearchContent", newValue);
        }

        public void AddStringToSelectFileMasks(string newValue)
        {
            this.AddString("SelectFileMasks", newValue);
        }

        public void AddStringToUserName(string newValue)
        {
            this.AddString("UserName", newValue);
        }

        public static void PopulateComboBox(ComboBox box, string[] history)
        {
            box.BeginUpdate();
            try
            {
                box.Items.Clear();
                if (history != null)
                {
                    box.Items.AddRange(history);
                }
                string text = box.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    int index = box.Items.IndexOf(text);
                    if (index >= 0)
                    {
                        box.SelectedIndex = index;
                    }
                    else
                    {
                        box.Items.Insert(0, text);
                        box.SelectedIndex = 0;
                    }
                }
            }
            finally
            {
                box.EndUpdate();
            }
        }

        [UserScopedSetting]
        public string[] ChangeFolder
        {
            get
            {
                return (string[]) this["ChangeFolder"];
            }
            set
            {
                this["ChangeFolder"] = value;
            }
        }

        [UserScopedSetting]
        public string[] CopyFilter
        {
            get
            {
                return (string[]) this["CopyFilter"];
            }
            set
            {
                this["CopyFilter"] = value;
            }
        }

        [UserScopedSetting]
        public string[] CopyTo
        {
            get
            {
                return (string[]) this["CopyTo"];
            }
            set
            {
                this["CopyTo"] = value;
            }
        }

        public static HistorySettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting]
        public string[] ExcludeMasks
        {
            get
            {
                return (string[]) this["ExcludeMasks"];
            }
            set
            {
                this["ExcludeMasks"] = value;
            }
        }

        [UserScopedSetting]
        public string[] FtpFolder
        {
            get
            {
                return (string[]) this["FtpFolder"];
            }
            set
            {
                this["FtpFolder"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("True")]
        public bool HistoryEnabled
        {
            get
            {
                return (bool) this["HistoryEnabled"];
            }
            set
            {
                this["HistoryEnabled"] = value;
            }
        }

        public bool HistoryExists
        {
            get
            {
                foreach (SettingsPropertyValue value2 in this.PropertyValues)
                {
                    if ((value2.Property.PropertyType == typeof(string[])) && (value2.PropertyValue != null))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        [UserScopedSetting]
        public string[] IncludeMasks
        {
            get
            {
                return (string[]) this["IncludeMasks"];
            }
            set
            {
                this["IncludeMasks"] = value;
            }
        }

        [UserScopedSetting]
        public string[] LookFolder
        {
            get
            {
                return (string[]) this["LookFolder"];
            }
            set
            {
                this["LookFolder"] = value;
            }
        }

        [UserScopedSetting]
        public string[] MakeLinkName
        {
            get
            {
                return (string[]) this["MakeLinkName"];
            }
            set
            {
                this["MakeLinkName"] = value;
            }
        }

        [UserScopedSetting]
        public string[] NewFile
        {
            get
            {
                return (string[]) this["NewFile"];
            }
            set
            {
                this["NewFile"] = value;
            }
        }

        [UserScopedSetting]
        public string[] NewFolderName
        {
            get
            {
                return (string[]) this["NewFolderName"];
            }
            set
            {
                this["NewFolderName"] = value;
            }
        }

        [UserScopedSetting]
        public string[] PackTo
        {
            get
            {
                return (string[]) this["PackTo"];
            }
            set
            {
                this["PackTo"] = value;
            }
        }

        [UserScopedSetting]
        public string[] RunAsArguments
        {
            get
            {
                return (string[]) this["RunAsArguments"];
            }
            set
            {
                this["RunAsArguments"] = value;
            }
        }

        [UserScopedSetting]
        public string[] SearchContent
        {
            get
            {
                return (string[]) this["SearchContent"];
            }
            set
            {
                this["SearchContent"] = value;
            }
        }

        [UserScopedSetting]
        public string[] SelectFileMasks
        {
            get
            {
                return (string[]) this["SelectFileMasks"];
            }
            set
            {
                this["SelectFileMasks"] = value;
            }
        }

        [UserScopedSetting]
        public string[] UserName
        {
            get
            {
                return (string[]) this["UserName"];
            }
            set
            {
                this["UserName"] = value;
            }
        }
    }
}

