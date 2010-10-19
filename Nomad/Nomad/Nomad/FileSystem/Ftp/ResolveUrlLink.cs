namespace Nomad.FileSystem.Ftp
{
    using Microsoft.Win32;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;
    using System.Windows.Forms;

    [Serializable]
    public class ResolveUrlLink : IResolveLink
    {
        private string _UrlPath;
        [NonSerialized]
        private Keys FHotkey = ~Keys.KeyCode;
        [NonSerialized]
        private IVirtualItem FTarget;
        [NonSerialized]
        private string FTargetPath;

        private ResolveUrlLink(string urlPath, string targetPath)
        {
            this._UrlPath = urlPath;
            this.FTargetPath = targetPath;
        }

        public static ResolveUrlLink Create(string urlPath)
        {
            if (urlPath == null)
            {
                throw new ArgumentNullException();
            }
            if (urlPath == string.Empty)
            {
                throw new ArgumentException();
            }
            if (!IsUrlLink(urlPath))
            {
                return null;
            }
            string str = Ini.ReadValue(urlPath, "InternetShortcut", "URL");
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return new ResolveUrlLink(urlPath, str);
        }

        public static bool IsUrlLink(string name)
        {
            return ((name != null) && name.EndsWith(".url", StringComparison.OrdinalIgnoreCase));
        }

        public Keys Hotkey
        {
            get
            {
                if ((this.FHotkey == ~Keys.KeyCode) && (this._UrlPath != null))
                {
                    short num;
                    if (short.TryParse(Ini.ReadValue(this._UrlPath, "InternetShortcut", "HotKey"), out num))
                    {
                        this.FHotkey = WindowsWrapper.HotkeyToShortcutKeys(num);
                    }
                    else
                    {
                        this.FHotkey = Keys.None;
                    }
                }
                return this.FHotkey;
            }
        }

        public IVirtualItem Target
        {
            get
            {
                if ((this.FTarget == null) && (this._UrlPath != null))
                {
                    using (Stream stream = File.OpenRead(this._UrlPath))
                    {
                        this.FTarget = FtpItem.FromUrlFile(stream, null);
                    }
                }
                return this.FTarget;
            }
        }

        public string TargetPath
        {
            get
            {
                if ((this.FTargetPath == null) && (this._UrlPath != null))
                {
                    this.FTargetPath = Ini.ReadValue(this._UrlPath, "InternetShortcut", "URL");
                }
                return this.FTargetPath;
            }
        }

        public string UrlPath
        {
            get
            {
                return this._UrlPath;
            }
        }
    }
}

