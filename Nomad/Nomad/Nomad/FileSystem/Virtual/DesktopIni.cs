namespace Nomad.FileSystem.Virtual
{
    using Nomad.Commons.IO;
    using Nomad.FileSystem.LocalFile;
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;

    public class DesktopIni : IconCustomizeIni, ICustomizeFolder
    {
        private CustomizeFolderParts FUpdatableParts;
        private const string KeyApplyToChildren = "ApplyToChildren";
        private const string KeyBackColor = "IconArea_TextBackground";
        private const string KeyForeColor = "IconArea_Text";
        private const string SectionFolderSettings = "{BE098140-A513-11D0-A3A4-00C04FD706EC}";

        public DesktopIni(string iniPath) : base(iniPath)
        {
        }

        public static bool CheckApplyToChildren(string iniPath)
        {
            if (File.Exists(iniPath))
            {
                try
                {
                    bool flag;
                    if (bool.TryParse(Ini.ReadValue(iniPath, "Nomad", "ApplyToChildren"), out flag))
                    {
                        return flag;
                    }
                }
                catch
                {
                }
            }
            return false;
        }

        protected Color GetColor(string sectionName, string keyName)
        {
            int num;
            string str = base.Get(sectionName, keyName);
            if ((!string.IsNullOrEmpty(str) && (str.Length == 10)) && int.TryParse(str.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture.NumberFormat, out num))
            {
                Color color = Color.FromArgb(num & 0xff, (num & 0xff00) >> 8, (num & 0xff0000) >> 0x10);
                foreach (KnownColor color2 in Enum.GetValues(typeof(KnownColor)))
                {
                    Color color3 = Color.FromKnownColor(color2);
                    if (((!color3.IsSystemColor && (color3.A == 0xff)) && ((color3.R == color.R) && (color3.G == color.G))) && (color3.B == color.B))
                    {
                        return color3;
                    }
                }
                return color;
            }
            return Color.Empty;
        }

        protected void SetColor(string sectionName, string keyName, Color value)
        {
            base.Set(sectionName, keyName, value.IsEmpty ? null : string.Format("0x00{0:X2}{1:X2}{2:X2}", value.B, value.G, value.R));
        }

        public override void Write()
        {
            WatcherChangeTypes created = WatcherChangeTypes.Created;
            if (File.Exists(base.FileName))
            {
                File.SetAttributes(base.FileName, FileAttributes.Normal);
                created = WatcherChangeTypes.Changed;
            }
            if (!base.HasValues)
            {
                File.Delete(base.FileName);
                created = WatcherChangeTypes.Deleted;
            }
            else
            {
                base.CompactWrite = true;
                base.Write();
                File.SetAttributes(base.FileName, FileAttributes.System | FileAttributes.Hidden);
            }
            LocalFileSystemCreator.RaiseFileChangedEvent(created, base.FileName);
        }

        public bool ApplyToChildren
        {
            get
            {
                bool flag;
                return (bool.TryParse(base.Get("Nomad", "ApplyToChildren"), out flag) && flag);
            }
            set
            {
                base.Set("Nomad", "ApplyToChildren", value ? bool.TrueString : null);
            }
        }

        public Color BackColor
        {
            get
            {
                return this.GetColor("{BE098140-A513-11D0-A3A4-00C04FD706EC}", "IconArea_TextBackground");
            }
            set
            {
                this.SetColor("{BE098140-A513-11D0-A3A4-00C04FD706EC}", "IconArea_TextBackground", value);
            }
        }

        public string Description
        {
            get
            {
                return base.Get(this.GeneralSectionName, "InfoTip");
            }
            set
            {
                base.Set(this.GeneralSectionName, "InfoTip", value);
            }
        }

        public Color ForeColor
        {
            get
            {
                return this.GetColor("{BE098140-A513-11D0-A3A4-00C04FD706EC}", "IconArea_Text");
            }
            set
            {
                this.SetColor("{BE098140-A513-11D0-A3A4-00C04FD706EC}", "IconArea_Text", value);
            }
        }

        protected override string GeneralSectionName
        {
            get
            {
                return ".ShellClassInfo";
            }
        }

        public virtual CustomizeFolderParts UpdatableParts
        {
            get
            {
                if (this.FUpdatableParts == 0)
                {
                    this.FUpdatableParts = CustomizeFolderParts.All;
                    if (PathHelper.IsRootPath(Path.GetDirectoryName(base.FileName)))
                    {
                        this.FUpdatableParts &= ~CustomizeFolderParts.Icon;
                    }
                }
                return this.FUpdatableParts;
            }
        }
    }
}

