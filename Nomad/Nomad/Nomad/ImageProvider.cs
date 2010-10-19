namespace Nomad
{
    using Microsoft.Shell;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.Properties;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public abstract class ImageProvider
    {
        public static EventHandler DefaultImageProviderCreated;
        private static ImageProvider FDefault;

        protected ImageProvider()
        {
        }

        public abstract Image GetDefaultFileIcon(string fileName, Size size);
        public Image GetDefaultFolderIcon(string folderName, Size size)
        {
            DefaultIcon folder = DefaultIcon.Folder;
            PathType pathType = PathHelper.GetPathType(folderName);
            if ((pathType & (PathType.File | PathType.Folder)) == PathType.Unknown)
            {
                if ((pathType & PathType.NetworkShare) > PathType.Unknown)
                {
                    folder = DefaultIcon.NetworkFolder;
                }
                else if ((pathType & PathType.NetworkServer) > PathType.Unknown)
                {
                    folder = DefaultIcon.NetworkServer;
                }
                else if ((pathType & PathType.Volume) > PathType.Unknown)
                {
                    folder = DefaultIcon.Drive;
                }
            }
            return this.GetDefaultIcon(folder, size);
        }

        public abstract Image GetDefaultIcon(DefaultIcon icon, Size size);
        public abstract Image GetDriveIcon(DriveInfo drive, Size size);
        public abstract Image GetFileIcon(string fileName, Size size);
        public abstract Image GetFolderIcon(string folderName, Size size);
        public abstract Image GetItemOverlay(string itemName, Size size);
        protected virtual Image GetShellLinkIcon(ShellLink link, ref Size size)
        {
            return null;
        }

        protected Image GetShellLinkIcon(string fileName, ref Size size)
        {
            Image shellLinkIcon = null;
            try
            {
                string path = null;
                using (ShellLink link = new ShellLink(fileName))
                {
                    shellLinkIcon = this.GetShellLinkIcon(link, ref size);
                    if (shellLinkIcon != null)
                    {
                        return shellLinkIcon;
                    }
                    path = link.Path;
                    if (string.IsNullOrEmpty(path))
                    {
                        IShellFolder desktopFolder = ShellItem.GetDesktopFolder();
                        try
                        {
                            path = desktopFolder.GetDisplayNameOf(link.IdList, SHGNO.SHGDN_FORPARSING);
                        }
                        finally
                        {
                            Marshal.ReleaseComObject(desktopFolder);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(path))
                {
                    switch (PathHelper.GetPathType(path))
                    {
                        case PathType.Volume:
                            return this.GetDriveIcon(new DriveInfo(path), size);

                        case PathType.Folder:
                        case PathType.File:
                            return shellLinkIcon;

                        case (PathType.Folder | PathType.Volume):
                            return this.GetFolderIcon(path, size);

                        case (PathType.File | PathType.Volume):
                            if (!Directory.Exists(path))
                            {
                                goto Label_0125;
                            }
                            return this.GetFolderIcon(path, size);

                        case PathType.NetworkServer:
                            return this.GetDefaultIcon(DefaultIcon.NetworkServer, size);

                        case PathType.NetworkShare:
                            return this.GetDefaultIcon(DefaultIcon.NetworkFolder, size);
                    }
                }
                return shellLinkIcon;
            Label_0125:
                shellLinkIcon = this.GetFileIcon(path, size);
            }
            catch (Exception exception)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
            }
            return shellLinkIcon;
        }

        public static void ResetDefaultImageProvider()
        {
            IDisposable fDefault = FDefault as IDisposable;
            if (fDefault != null)
            {
                fDefault.Dispose();
            }
            FDefault = null;
        }

        public static ImageProvider Default
        {
            get
            {
                if (FDefault == null)
                {
                    string imageProvider = null;
                    if (!SettingsManager.CheckSafeMode(SafeMode.UseShellImageProvider | SafeMode.SkipImageProvider))
                    {
                        imageProvider = Settings.Default.ImageProvider;
                    }
                    if (!string.IsNullOrEmpty(imageProvider))
                    {
                        try
                        {
                            System.Type type = System.Type.GetType(imageProvider);
                            if (type != null)
                            {
                                FDefault = Activator.CreateInstance(type) as ImageProvider;
                            }
                            if (FDefault == null)
                            {
                                if (!Path.IsPathRooted(imageProvider))
                                {
                                    imageProvider = Path.Combine(Application.StartupPath, imageProvider);
                                }
                                if (File.Exists(imageProvider))
                                {
                                    FDefault = new IconPackImageProvider(imageProvider);
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                        }
                    }
                    if (FDefault == null)
                    {
                        if (SettingsManager.CheckSafeMode(SafeMode.UseShellImageProvider))
                        {
                            FDefault = new ShellImageProvider();
                        }
                        else
                        {
                            FDefault = new CustomImageProvider();
                        }
                    }
                    if (DefaultImageProviderCreated != null)
                    {
                        DefaultImageProviderCreated(FDefault, EventArgs.Empty);
                    }
                }
                return FDefault;
            }
        }
    }
}

