namespace Nomad
{
    using Microsoft.IO;
    using Nomad.Commons.Drawing;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Resources;

    public class IconPackImageProvider : CustomImageProvider, IDisposable
    {
        private Dictionary<DefaultIcon, string> DefaultIconMap = new Dictionary<DefaultIcon, string>();
        private List<Size> DefaultIconSize;
        private Dictionary<string, IDictionary<Size, Image>> IconCache = new Dictionary<string, IDictionary<Size, Image>>(StringComparer.InvariantCultureIgnoreCase);
        private ResourceSet IconPack;
        private Dictionary<VolumeType, string> VolumeTypeMap = new Dictionary<VolumeType, string>();

        public IconPackImageProvider(string iconPackPath)
        {
            this.IconPack = new ResourceSet(iconPackPath);
            foreach (DefaultIcon icon in Enum.GetValues(typeof(DefaultIcon)))
            {
                this.DefaultIconMap.Add(icon, string.Intern("DefaultIcon." + icon.ToString()));
            }
            foreach (VolumeType type in Enum.GetValues(typeof(VolumeType)))
            {
                this.VolumeTypeMap.Add(type, string.Intern("VolumeType." + type.ToString()));
            }
        }

        public void Dispose()
        {
            if (this.IconPack != null)
            {
                this.IconPack.Dispose();
            }
            this.IconPack = null;
            this.DefaultIconMap = null;
            this.VolumeTypeMap = null;
            this.DefaultIconSize = null;
            this.IconCache = null;
        }

        protected override Image GetClsidIcon(string clsid, ref Size size)
        {
            Image iconFromCache = this.GetIconFromCache(clsid, ref size);
            if (iconFromCache == null)
            {
                return base.GetClsidIcon(clsid, ref size);
            }
            return iconFromCache;
        }

        protected override Image GetDefaultFileIconFromExt(string extension, ref Size size)
        {
            Image iconFromCache = this.GetIconFromCache(extension, ref size);
            if (iconFromCache == null)
            {
                return base.GetDefaultFileIconFromExt(extension, ref size);
            }
            return iconFromCache;
        }

        public override Image GetDefaultIcon(DefaultIcon icon, Size size)
        {
            Image iconFromCache = this.GetIconFromCache(this.DefaultIconMap[icon], ref size);
            if (iconFromCache == null)
            {
                return base.GetDefaultIcon(icon, size);
            }
            return iconFromCache;
        }

        protected override Image GetDriveIcon(DriveInfo drive, char driveChar, VolumeType volumeType, ref Size size)
        {
            Image iconFromCache = this.GetIconFromCache(this.VolumeTypeMap[volumeType], ref size);
            if (iconFromCache == null)
            {
                return base.GetDriveIcon(drive, driveChar, volumeType, ref size);
            }
            return iconFromCache;
        }

        protected override Image GetFileIcon(string fileName, string extension, ref Size size)
        {
            Image iconFromCache = this.GetIconFromCache(extension.ToLower(), ref size);
            if (((iconFromCache == null) && CustomImageProvider.IsShortcut(fileName)) && string.Equals(extension, ".lnk", StringComparison.OrdinalIgnoreCase))
            {
                iconFromCache = base.GetShellLinkIcon(fileName, ref size);
            }
            if (iconFromCache == null)
            {
                return base.GetFileIcon(fileName, extension, ref size);
            }
            return iconFromCache;
        }

        private Image GetIcon(string resourceKey, ref Size size)
        {
            string str6;
            string str = this.IconPack.GetString(resourceKey, true);
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Size));
            Dictionary<Size, string> dictionary = new Dictionary<Size, string>();
            string str2 = this.IconPack.GetString(str);
            if (string.IsNullOrEmpty(str2))
            {
                if (this.DefaultIconSize == null)
                {
                    this.DefaultIconSize = new List<Size>();
                    str2 = this.IconPack.GetString("IconSize");
                    if (!string.IsNullOrEmpty(str2))
                    {
                        foreach (string str3 in str2.Split(new char[] { '|' }))
                        {
                            this.DefaultIconSize.Add((Size) converter.ConvertFromInvariantString(str3));
                        }
                    }
                }
                if (this.DefaultIconSize.Count == 0)
                {
                    return null;
                }
                foreach (Size size2 in this.DefaultIconSize)
                {
                    dictionary.Add(size2, string.Format("{0}_{1}x{2}", str, size2.Width, size2.Height));
                }
            }
            else
            {
                foreach (string str3 in str2.Split(new char[] { '|' }))
                {
                    string str4;
                    string str5 = null;
                    int index = str3.IndexOf('=');
                    if (index > 0)
                    {
                        str4 = str3.Substring(0, index);
                        str5 = str3.Substring(index + 1);
                    }
                    else
                    {
                        str4 = str3;
                    }
                    Size key = (Size) converter.ConvertFromInvariantString(str4);
                    if (str5 == null)
                    {
                        str5 = string.Format("{0}_{1}x{2}", str, key.Width, key.Height);
                    }
                    dictionary.Add(key, str5);
                }
            }
            if (dictionary.TryGetValue(size, out str6))
            {
                return (Image) this.IconPack.GetObject(str6);
            }
            Size size4 = new Size();
            foreach (Size size5 in dictionary.Keys)
            {
                if ((size5.Width >= size4.Width) && (size5.Height >= size4.Height))
                {
                    size4 = size5;
                }
            }
            return new Bitmap((Image) this.IconPack.GetObject(dictionary[size4]), size);
        }

        private Image GetIconFromCache(string resourceKey, ref Size size)
        {
            IDictionary<Size, Image> dictionary;
            if (this.IconCache == null)
            {
                throw new ObjectDisposedException("IconPackImageProvider");
            }
            Image icon = null;
            if (this.IconCache.TryGetValue(resourceKey, out dictionary))
            {
                if (dictionary == null)
                {
                    return icon;
                }
                if (dictionary.TryGetValue(size, out icon))
                {
                    return icon;
                }
            }
            icon = this.GetIcon(resourceKey, ref size);
            if ((dictionary != null) && (icon != null))
            {
                dictionary.Add(size, icon);
            }
            else if (icon == null)
            {
                this.IconCache.Add(resourceKey, null);
            }
            else
            {
                dictionary = IconCollection.Create();
                dictionary.Add(size, icon);
                this.IconCache.Add(resourceKey, dictionary);
            }
            return icon;
        }

        protected override Image GetVolumeIcon(VolumeType volumeType, char driveChar, ref Size size)
        {
            Image iconFromCache = this.GetIconFromCache(this.VolumeTypeMap[volumeType], ref size);
            if (iconFromCache == null)
            {
                iconFromCache = base.GetVolumeIcon(volumeType, driveChar, ref size);
            }
            return iconFromCache;
        }
    }
}

