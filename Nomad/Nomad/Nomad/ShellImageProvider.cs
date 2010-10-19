namespace Nomad
{
    using Microsoft;
    using Microsoft.Shell;
    using Microsoft.Win32;
    using Nomad.Commons.Drawing;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;

    public class ShellImageProvider : ImageProvider
    {
        private Dictionary<DefaultIcon, IDictionary<Size, Image>> DefaultIconCache = new Dictionary<DefaultIcon, IDictionary<Size, Image>>();
        private Dictionary<int, IDictionary<Size, Image>> IconCache = new Dictionary<int, IDictionary<Size, Image>>();

        public override Image GetDefaultFileIcon(string fileName, Size size)
        {
            return this.GetIcon("file" + Path.GetExtension(fileName), FileAttributes.Normal, true, ref size);
        }

        public override Image GetDefaultIcon(DefaultIcon icon, Size size)
        {
            Image specialFolderIcon = null;
            IDictionary<Size, Image> dictionary;
            Dictionary<DefaultIcon, IDictionary<Size, Image>> dictionary2;
            lock ((dictionary2 = this.DefaultIconCache))
            {
                if (this.DefaultIconCache.TryGetValue(icon, out dictionary))
                {
                    if (dictionary == null)
                    {
                        return null;
                    }
                    if (dictionary.TryGetValue(size, out specialFolderIcon))
                    {
                        return specialFolderIcon;
                    }
                }
            }
            if (OS.IsWinVista)
            {
                SHSTOCKICONID siid = SHSTOCKICONID.SIID_INVALID;
                switch (icon)
                {
                    case DefaultIcon.UnknownFile:
                        siid = SHSTOCKICONID.SIID_DOCNOASSOC;
                        break;

                    case DefaultIcon.DefaultDocument:
                        siid = SHSTOCKICONID.SIID_DOCASSOC;
                        break;

                    case DefaultIcon.DefaultApplication:
                        siid = SHSTOCKICONID.SIID_APPLICATION;
                        break;

                    case DefaultIcon.Drive:
                        siid = SHSTOCKICONID.SIID_DRIVEUNKNOWN;
                        break;

                    case DefaultIcon.Folder:
                        siid = SHSTOCKICONID.SIID_FOLDER;
                        break;

                    case DefaultIcon.MyComputer:
                        siid = SHSTOCKICONID.SIID_DESKTOPPC;
                        break;

                    case DefaultIcon.NetworkNeighborhood:
                        siid = SHSTOCKICONID.SIID_WORLD;
                        break;

                    case DefaultIcon.EntireNetwork:
                        siid = SHSTOCKICONID.SIID_MYNETWORK;
                        break;

                    case DefaultIcon.NetworkServer:
                        siid = SHSTOCKICONID.SIID_SERVER;
                        break;

                    case DefaultIcon.NetworkFolder:
                        siid = SHSTOCKICONID.SIID_SERVERSHARE;
                        break;

                    case DefaultIcon.OverlayLink:
                        siid = SHSTOCKICONID.SIID_LINK;
                        break;

                    case DefaultIcon.OverlayShare:
                        siid = SHSTOCKICONID.SIID_SHARE;
                        break;
                }
                if (siid != SHSTOCKICONID.SIID_INVALID)
                {
                    SHSTOCKICONINFO shstockiconinfo;
                    shstockiconinfo = new SHSTOCKICONINFO {
                        cbSize = Marshal.SizeOf(shstockiconinfo)
                    };
                    if (HRESULT.SUCCEEDED(Shell32.SHGetStockIconInfo(siid, SHGSI.SHGSI_ICON | ((size.Height < 0x20) ? SHGSI.SHGSI_SMALLICON : SHGSI.SHGSI_ICONLOCATION), ref shstockiconinfo)))
                    {
                        specialFolderIcon = IconToImage(shstockiconinfo.hIcon);
                    }
                }
            }
            if (specialFolderIcon == null)
            {
                switch (icon)
                {
                    case DefaultIcon.OverlayLink:
                    case DefaultIcon.OverlayShare:
                    case DefaultIcon.OverlayUnreadable:
                        return null;

                    case DefaultIcon.UnknownFile:
                        break;

                    case DefaultIcon.DefaultDocument:
                        specialFolderIcon = this.GetIcon("file.txt", FileAttributes.Normal, true, ref size);
                        break;

                    case DefaultIcon.DefaultApplication:
                        specialFolderIcon = this.GetIcon("file.exe", FileAttributes.Normal, true, ref size);
                        break;

                    case DefaultIcon.Desktop:
                        specialFolderIcon = this.GetIcon("file.desklink", FileAttributes.Normal, true, ref size);
                        break;

                    case DefaultIcon.Drive:
                        specialFolderIcon = this.GetIcon(@"c:\", FileAttributes.Directory, true, ref size);
                        break;

                    case DefaultIcon.Favorites:
                        specialFolderIcon = this.GetIcon(Environment.GetFolderPath(Environment.SpecialFolder.Favorites), 0, false, ref size);
                        break;

                    case DefaultIcon.Folder:
                    case DefaultIcon.MyPictures:
                    case DefaultIcon.MyMusic:
                    case DefaultIcon.MyVideos:
                    case DefaultIcon.NetworkWorkgroup:
                    case DefaultIcon.NetworkProvider:
                    case DefaultIcon.NetworkFolder:
                        specialFolderIcon = this.GetIcon("folder", FileAttributes.Directory, true, ref size);
                        break;

                    case DefaultIcon.MyComputer:
                    case DefaultIcon.NetworkServer:
                        specialFolderIcon = GetSpecialFolderIcon(Shell32.GetClsidFolderParseName(CLSID.CLSID_MYCOMPUTER), ref size);
                        break;

                    case DefaultIcon.MyDocuments:
                        specialFolderIcon = GetSpecialFolderIcon(Shell32.GetClsidFolderParseName(CLSID.CLSID_MYDOCUMENTS), ref size);
                        break;

                    case DefaultIcon.SearchFolder:
                        specialFolderIcon = GetSpecialFolderIcon(Shell32.GetClsidFolderParseName(CLSID.CLSID_SEARCHRESULTS), ref size);
                        break;

                    case DefaultIcon.NetworkNeighborhood:
                        specialFolderIcon = GetSpecialFolderIcon(Shell32.GetClsidFolderParseName(CLSID.CLSID_NETWORK_NEIGHBORHOOD), ref size);
                        break;

                    case DefaultIcon.EntireNetwork:
                        if (OS.IsWinVista)
                        {
                            specialFolderIcon = GetSpecialFolderIcon(Shell32.GetClsidFolderParseName(CLSID.CLSID_NETWORK), ref size);
                        }
                        else
                        {
                            specialFolderIcon = GetSpecialFolderIcon(Shell32.GetClsidFolderParseName(CLSID.CLSID_NETWORK_NEIGHBORHOOD) + Path.DirectorySeparatorChar + "EntireNetwork", ref size);
                        }
                        break;

                    default:
                        throw new InvalidEnumArgumentException();
                }
                if (specialFolderIcon == null)
                {
                    specialFolderIcon = this.GetIcon("unknown", FileAttributes.Normal, true, ref size);
                }
            }
            lock ((dictionary2 = this.DefaultIconCache))
            {
                if (dictionary != null)
                {
                    dictionary.Add(size, specialFolderIcon);
                }
                else
                {
                    dictionary = IconCollection.Create();
                    dictionary.Add(size, specialFolderIcon);
                    this.DefaultIconCache.Add(icon, dictionary);
                }
            }
            return specialFolderIcon;
        }

        public override Image GetDriveIcon(DriveInfo drive, Size size)
        {
            Image image = this.GetIcon(drive.Name, 0, false, ref size);
            if (image != null)
            {
                return image;
            }
            return this.GetDefaultIcon(DefaultIcon.Drive, size);
        }

        public override Image GetFileIcon(string fileName, Size size)
        {
            Image image = this.GetIcon(fileName, 0, false, ref size);
            if (image != null)
            {
                return image;
            }
            return this.GetDefaultFileIcon(fileName, size);
        }

        public override Image GetFolderIcon(string folderName, Size size)
        {
            Image image = this.GetIcon(folderName, 0, false, ref size);
            if (image != null)
            {
                return image;
            }
            return this.GetDefaultIcon(DefaultIcon.Folder, size);
        }

        private Image GetIcon(string fileName, FileAttributes attributes, bool useAttributes, ref Size size)
        {
            Image image;
            IDictionary<Size, Image> dictionary;
            Dictionary<int, IDictionary<Size, Image>> dictionary2;
            SHFILEINFO psfi = new SHFILEINFO();
            Shell32.SHGetFileInfo(fileName, attributes, ref psfi, (useAttributes ? (SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_USEFILEATTRIBUTES) : SHGFI.SHGFI_LARGEICON) | (SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_SYSICONINDEX));
            lock ((dictionary2 = this.IconCache))
            {
                if (this.IconCache.TryGetValue(psfi.iIcon, out dictionary))
                {
                    if (dictionary == null)
                    {
                        return null;
                    }
                    if (dictionary.TryGetValue(size, out image))
                    {
                        return image;
                    }
                }
            }
            psfi = new SHFILEINFO();
            Shell32.SHGetFileInfo(fileName, attributes, ref psfi, ((useAttributes ? (SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_USEFILEATTRIBUTES) : SHGFI.SHGFI_LARGEICON) | (SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_ICON)) | ((size.Height < 0x20) ? SHGFI.SHGFI_SMALLICON : SHGFI.SHGFI_LARGEICON));
            image = IconToImage(psfi.hIcon);
            lock ((dictionary2 = this.IconCache))
            {
                if (dictionary != null)
                {
                    dictionary.Add(size, image);
                }
                else
                {
                    dictionary = IconCollection.Create();
                    dictionary.Add(size, image);
                    this.IconCache.Add(psfi.iIcon, dictionary);
                }
            }
            return image;
        }

        public override Image GetItemOverlay(string itemName, Size size)
        {
            return null;
        }

        private static Image GetSpecialFolderIcon(string clsid, ref Size size)
        {
            Image image;
            IShellFolder desktopFolder = ShellItem.GetDesktopFolder();
            try
            {
                uint num;
                IntPtr ptr;
                SFGAO pdwAttributes = 0;
                desktopFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, clsid, out num, out ptr, ref pdwAttributes);
                SHFILEINFO psfi = new SHFILEINFO();
                Shell32.SHGetFileInfo(ptr, FileAttributes.Directory, ref psfi, (SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_PIDL | SHGFI.SHGFI_USEFILEATTRIBUTES | SHGFI.SHGFI_ICON) | ((size.Height < 0x20) ? SHGFI.SHGFI_SMALLICON : SHGFI.SHGFI_LARGEICON));
                image = IconToImage(psfi.hIcon);
            }
            catch (ArgumentException)
            {
                image = null;
            }
            finally
            {
                Marshal.ReleaseComObject(desktopFolder);
            }
            return image;
        }

        private static Image IconToImage(IntPtr icon)
        {
            Image image;
            try
            {
                image = ImageHelper.IconToBitmap(icon);
            }
            finally
            {
                if (icon != IntPtr.Zero)
                {
                    Windows.DestroyIcon(icon);
                }
            }
            return image;
        }
    }
}

