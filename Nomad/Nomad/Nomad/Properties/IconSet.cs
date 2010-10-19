namespace Nomad.Properties
{
    using Microsoft.Win32;
    using Nomad.Commons.Drawing;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;

    public static class IconSet
    {
        private static ResourceManager DefaultIconSet;
        private static ResourceSet FIconSet;
        private static bool FIconSetAcquired;
        private static Image FImageInformation;
        private static Image FImageWarning;
        public const string ImageCloseStrip = "CloseStrip";
        public const string ImageFolder = "folder";
        public const string ImageInformation = "information";
        public const string ImagePackage = "package";
        public const string ImageSaveAs = "SaveAs";
        public const string ImageTab = "tab";
        public const string ImageWarning = "warning";

        public static Icon GetIcon(string name)
        {
            IconSetNeeded();
            if (FIconSet == null)
            {
                return null;
            }
            name = ResolveName(name);
            if (name == null)
            {
                return null;
            }
            object obj2 = FIconSet.GetObject(name);
            Bitmap bitmap = obj2 as Bitmap;
            if (bitmap != null)
            {
                IntPtr hicon = bitmap.GetHicon();
                try
                {
                    using (Icon icon = Icon.FromHandle(hicon))
                    {
                        return new Icon(icon, icon.Size);
                    }
                }
                finally
                {
                    Windows.DestroyIcon(hicon);
                }
            }
            return (obj2 as Icon);
        }

        public static Image GetImage(string name)
        {
            IconSetNeeded();
            object obj2 = null;
            if (FIconSet != null)
            {
                string str = ResolveName(name);
                if (str != null)
                {
                    obj2 = FIconSet.GetObject(str);
                }
            }
            if (obj2 == null)
            {
                obj2 = DefaultIconSet.GetObject(name);
                string str2 = obj2 as string;
                if (str2 != null)
                {
                    obj2 = DefaultIconSet.GetObject(str2);
                }
            }
            Icon icon = obj2 as Icon;
            if (icon != null)
            {
                return ImageHelper.IconToBitmap(icon, icon.Size);
            }
            return (obj2 as Image);
        }

        private static void IconSetNeeded()
        {
            if (!FIconSetAcquired)
            {
                string path = Path.Combine(Application.StartupPath, "IconSet.resources");
                if (File.Exists(path))
                {
                    FIconSet = new ResourceSet(path);
                }
                FIconSetAcquired = true;
            }
            if (DefaultIconSet == null)
            {
                DefaultIconSet = new ResourceManager("Nomad.Properties.DefaultIconSet", Assembly.GetExecutingAssembly());
            }
        }

        public static string ResolveName(string name)
        {
            if (FIconSet == null)
            {
                return null;
            }
            object obj2 = FIconSet.GetObject(name);
            while (obj2 is string)
            {
                name = (string) obj2;
                obj2 = FIconSet.GetObject(name);
            }
            return ((obj2 != null) ? name : null);
        }

        public static bool Available
        {
            get
            {
                IconSetNeeded();
                return (FIconSet != null);
            }
        }

        public static ResourceSet IconResourceSet
        {
            get
            {
                return FIconSet;
            }
        }

        public static Image Information
        {
            get
            {
                if (FImageInformation == null)
                {
                    Image fImageInformation = FImageInformation;
                }
                return (FImageInformation = GetImage("information") ?? ImageHelper.IconToBitmap(SystemIcons.Information, ImageHelper.DefaultSmallIconSize));
            }
        }

        public static Image Warning
        {
            get
            {
                if (FImageWarning == null)
                {
                    Image fImageWarning = FImageWarning;
                }
                return (FImageWarning = GetImage("warning") ?? ImageHelper.IconToBitmap(SystemIcons.Warning, ImageHelper.DefaultSmallIconSize));
            }
        }
    }
}

