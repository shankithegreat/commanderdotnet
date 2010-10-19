namespace Nomad.FileSystem.Virtual
{
    using Microsoft.Win32;
    using Nomad.Commons.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class UrlIni : IconCustomizeIni, ICustomizeFolder
    {
        private const string KeyBackColor = "BackColor";
        private const string KeyForeColor = "ForeColor";
        private const string KeyHotkey = "HotKey";

        public UrlIni(string iniPath) : base(iniPath)
        {
        }

        protected System.Drawing.Color GetColor(string sectionName, string keyName)
        {
            string str = base.Get(sectionName, keyName);
            if (string.IsNullOrEmpty(str))
            {
                return System.Drawing.Color.Empty;
            }
            return (System.Drawing.Color) TypeDescriptor.GetConverter(typeof(System.Drawing.Color)).ConvertFromInvariantString(str);
        }

        protected void SetColor(string sectionName, string keyName, System.Drawing.Color value)
        {
            if (value.IsEmpty)
            {
                base.Set(sectionName, keyName, null);
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(System.Drawing.Color));
                base.Set(sectionName, keyName, converter.ConvertToInvariantString(value));
            }
        }

        public System.Drawing.Color BackColor
        {
            get
            {
                return this.GetColor("Nomad", "BackColor");
            }
            set
            {
                this.SetColor("Nomad", "BackColor", value);
            }
        }

        public System.Drawing.Color ForeColor
        {
            get
            {
                return this.GetColor("Nomad", "ForeColor");
            }
            set
            {
                this.SetColor("Nomad", "ForeColor", value);
            }
        }

        protected override string GeneralSectionName
        {
            get
            {
                return "InternetShortcut";
            }
        }

        public Keys Hotkey
        {
            get
            {
                short num;
                if (short.TryParse(base.Get(this.GeneralSectionName, "HotKey"), out num))
                {
                    return WindowsWrapper.HotkeyToShortcutKeys(num);
                }
                return Keys.None;
            }
            set
            {
                base.Set(this.GeneralSectionName, "HotKey", (value == Keys.None) ? null : WindowsWrapper.ShortcutKeysToHotkey(value).ToString());
            }
        }

        public override IconLocation Icon
        {
            set
            {
                if (value != null)
                {
                    base.Icon = new IconLocation(Environment.ExpandEnvironmentVariables(value.IconFileName), value.IconIndex);
                }
                else
                {
                    base.Icon = null;
                }
            }
        }

        public CustomizeFolderParts UpdatableParts
        {
            get
            {
                return CustomizeFolderParts.All;
            }
        }
    }
}

