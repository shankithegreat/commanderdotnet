namespace Nomad
{
    using Microsoft;
    using Microsoft.COM;
    using Microsoft.IO;
    using Microsoft.Shell;
    using Microsoft.Win32;
    using Microsoft.Win32.Network;
    using Microsoft.Win32.SafeHandles;
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.Commons.IO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Security;
    using System.Text;
    using System.Threading;

    public class CustomImageProvider : ImageProvider
    {
        private const string CategoryImageProvider = "ImageProvider";
        private static Dictionary<string, IconLocation> ExtIconMap = new Dictionary<string, IconLocation>(StringComparer.OrdinalIgnoreCase);
        private static Dictionary<IconLocation, IDictionary<Size, Image>> LocationIconCache = new Dictionary<IconLocation, IDictionary<Size, Image>>();
        private static List<OverlayIdentifier> OverlayCache;
        private static object OverlayCacheLock = new object();
        private static WeakReference ShareCache;
        private static ReaderWriterLock ShareCacheLock = new ReaderWriterLock();
        private static EnumShareSource ShareSource = EnumShareSource.NetApi;
        private static Dictionary<int, IconLocation> ShellDllIconMap = new Dictionary<int, IconLocation>();
        private static string ShellDllLocation = Path.Combine(Environment.SystemDirectory, "shell32.dll");
        private static Dictionary<string, bool> ShortcutCache = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        public const int SI_AUDIO_CD = 40;
        public const int SI_CDROM = 11;
        public const int SI_CONTROLPANEL = 0x23;
        public const int SI_DEF_APPLICATION = 2;
        public const int SI_DEF_DOCUMENT = 1;
        public const int SI_DESKTOP = 0x22;
        public const int SI_DUN = 0x21;
        public const int SI_DVDROM = 0xb1;
        public const int SI_FAVORITES = 0x2b;
        public const int SI_FLASH_CF = 0xbd;
        public const int SI_FLASH_SD = 0xc1;
        public const int SI_FLASH_SM = 0xc2;
        public const int SI_FLOPPY_35 = 6;
        public const int SI_FLOPPY_514 = 5;
        public const int SI_FOLDER_CLOSED = 3;
        public const int SI_FOLDER_OPEN = 4;
        public const int SI_FONT = 0x26;
        public const int SI_HDD = 8;
        public const int SI_HIBERNATE = 0x38;
        public const int SI_LOGOFF = 0x2c;
        public const int SI_MYCOMPUTER = 15;
        public const int SI_MYDOCUMENTS = -235;
        public const int SI_MYMUSIC = -237;
        public const int SI_MYPICTURES = -236;
        public const int SI_MYVIDEOS = -238;
        public const int SI_NETWORK = 13;
        public const int SI_NETWORK_FOLDER = -172;
        public const int SI_NETWORK_FOLDER2 = 0x113;
        public const int SI_NETWORK_NEIGHBORHOOD = 0x11;
        public const int SI_NETWORK_PROVIDER = 14;
        public const int SI_NETWORK_WORKGROUP = 0x12;
        public const int SI_NETWORKDRIVE = 9;
        public const int SI_NETWORKDRIVE_DISCONNECTED = 10;
        public const int SI_PRINTER = 0x25;
        public const int SI_PRINTER_DEFAULT = 30;
        public const int SI_PRINTMANAGER = 0x10;
        public const int SI_PROGRAMGROUPS = 0x24;
        public const int SI_RAMDISK = 12;
        public const int SI_RECYCLEBIN_EMPTY = 0x1f;
        public const int SI_RECYCLEBIN_FULL = 0x20;
        public const int SI_REMOVABLE = 7;
        public const int SI_SEARCH_FOLDER = 0x2d;
        public const int SI_SHARE = 0x1c;
        public const int SI_SHORTCUT = 0x1d;
        public const int SI_STARTMENU_DOCKING = 0x1a;
        public const int SI_STARTMENU_DOCUMENTS = 20;
        public const int SI_STARTMENU_FIND = 0x16;
        public const int SI_STARTMENU_HELP = 0x17;
        public const int SI_STARTMENU_PROGRAMS = 0x13;
        public const int SI_STARTMENU_RUN = 0x18;
        public const int SI_STARTMENU_SETTINGS = 0x15;
        public const int SI_STARTMENU_SHUTDOWN = 0x1b;
        public const int SI_STARTMENU_SUSPEND = 0x19;
        public const int SI_TASKBAR = 0x27;
        public const int SI_UNKNOWN = 0;

        private static void AddIconToCache(IconLocation iconLocation, ref Size size, Image icon)
        {
            lock (LocationIconCache)
            {
                IDictionary<Size, Image> dictionary;
                if ((LocationIconCache.TryGetValue(iconLocation, out dictionary) && (dictionary != null)) && (icon != null))
                {
                    dictionary[size] = icon;
                }
                else if (icon == null)
                {
                    LocationIconCache[iconLocation] = null;
                }
                else
                {
                    dictionary = IconCollection.Create();
                    dictionary.Add(size, icon);
                    LocationIconCache[iconLocation] = dictionary;
                }
            }
        }

        private static void CreateOverlayCache()
        {
            OverlayCache = new List<OverlayIdentifier>();
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\ShellIconOverlayIdentifiers"))
                {
                    if (key != null)
                    {
                        foreach (string str in key.GetSubKeyNames())
                        {
                            using (RegistryKey key2 = key.OpenSubKey(str))
                            {
                                string g = key2.GetValue(null) as string;
                                if (g != null)
                                {
                                    IntPtr ptr;
                                    Guid rclsid = new Guid(g);
                                    if (ActiveX.CoCreateInstance(rclsid, null, CLSCTX.CLSCTX_INPROC_SERVER, typeof(IShellIconOverlayIdentifier).GUID, out ptr) == 0)
                                    {
                                        OverlayCache.Add(new OverlayIdentifier(ptr));
                                    }
                                }
                            }
                        }
                    }
                }
                OverlayCache.Sort(new Comparison<OverlayIdentifier>(OverlayIdentifier.Compare));
            }
            catch (SecurityException)
            {
            }
        }

        private static Image ExtractIcon(object iconHandler, ClassIconLocation iconLocation, bool addToCache, ref Size size)
        {
            Image image;
            if (!GetIconFromCache(iconLocation, ref size, out image))
            {
                IntPtr ptr;
                IntPtr ptr2;
                uint num;
                int num2;
                IntPtr ptr3;
                IntPtr ptr4;
                if (size.Height >= 0x20)
                {
                    num = (uint) (size.Height | 0x100000);
                }
                else
                {
                    num = (uint) (0x20 | (size.Height << 0x10));
                }
                IExtractIconW nw = iconHandler as IExtractIconW;
                if (nw != null)
                {
                    num2 = nw.Extract(iconLocation.IconFileName, iconLocation.IconIndex, out ptr, out ptr2, num);
                }
                else
                {
                    num2 = ((IExtractIconA) iconHandler).Extract(iconLocation.IconFileName, iconLocation.IconIndex, out ptr, out ptr2, num);
                }
                if (num2 != 0)
                {
                    return null;
                }
                if (size.Height >= 0x20)
                {
                    ptr3 = ptr;
                    ptr4 = ptr2;
                }
                else
                {
                    ptr3 = ptr2;
                    ptr4 = ptr;
                }
                if (ptr3 != IntPtr.Zero)
                {
                    image = ImageHelper.IconToBitmap(ptr3);
                    if (addToCache)
                    {
                        AddIconToCache(iconLocation, ref size, image);
                    }
                    Windows.DestroyIcon(ptr3);
                }
                if (!(ptr4 != IntPtr.Zero))
                {
                    return image;
                }
                if (addToCache)
                {
                    Image icon = ImageHelper.IconToBitmap(ptr4);
                    if ((icon != null) && IsIconSizeInCache(iconLocation, icon.Size))
                    {
                        icon.Dispose();
                        icon = null;
                    }
                    if (icon != null)
                    {
                        Size size2 = icon.Size;
                        AddIconToCache(iconLocation, ref size2, icon);
                    }
                }
                Windows.DestroyIcon(ptr4);
            }
            return image;
        }

        protected virtual Image GetClsidIcon(string clsid, ref Size size)
        {
            IconLocation location;
            bool flag;
            Dictionary<string, IconLocation> dictionary;
            lock ((dictionary = ExtIconMap))
            {
                flag = ExtIconMap.TryGetValue(clsid, out location);
            }
            if (!flag)
            {
                location = IconLocation.TryParse(ReadRegistryValue(Registry.ClassesRoot, @"CLSID\" + clsid + @"\DefaultIcon", null) as string);
                lock ((dictionary = ExtIconMap))
                {
                    ExtIconMap[clsid] = location;
                }
            }
            return ((location != null) ? LoadIconFromLocation2(location, ref size) : null);
        }

        public static Image GetClsidIcon(Guid clsid, int shellIconIndex, Size size)
        {
            string extKey = clsid.ToString("B");
            return GetDefaultContainerIcon(@"CLSID\" + extKey, extKey, shellIconIndex, ref size);
        }

        private static Image GetDefaultContainerIcon(string classKey, int shellIconIndex, ref Size size)
        {
            return GetDefaultContainerIcon(classKey, classKey, shellIconIndex, ref size);
        }

        private static Image GetDefaultContainerIcon(string classKey, string extKey, int shellIconIndex, ref Size size)
        {
            IconLocation location;
            Dictionary<string, IconLocation> dictionary;
            lock ((dictionary = ExtIconMap))
            {
                ExtIconMap.TryGetValue(extKey, out location);
            }
            if (location == null)
            {
                location = IconLocation.TryParse(ReadRegistryValue(Registry.ClassesRoot, classKey + @"\DefaultIcon", null) as string);
                if (location == null)
                {
                    location = new IconLocation(ShellDllLocation, shellIconIndex);
                }
                lock ((dictionary = ExtIconMap))
                {
                    ExtIconMap[extKey] = location;
                }
            }
            return LoadIconFromLocation2(location, ref size);
        }

        public override Image GetDefaultFileIcon(string fileName, Size size)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            return this.GetDefaultFileIconFromExt(Path.GetExtension(fileName), ref size);
        }

        protected virtual Image GetDefaultFileIconFromExt(string extension, ref Size size)
        {
            Image defaultIcon = null;
            IconLocation defaultFileIconLocation = GetDefaultFileIconLocation(extension);
            if ((defaultFileIconLocation != null) && (defaultFileIconLocation.IconFileName == "%1"))
            {
                if (extension.Equals(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    defaultIcon = this.GetDefaultIcon(DefaultIcon.DefaultApplication, size);
                }
                else
                {
                    defaultFileIconLocation = null;
                }
            }
            if ((defaultIcon == null) && (defaultFileIconLocation != null))
            {
                defaultIcon = LoadIconFromLocation2(defaultFileIconLocation, ref size);
            }
            if (defaultIcon == null)
            {
                return this.GetDefaultIcon(DefaultIcon.UnknownFile, size);
            }
            return defaultIcon;
        }

        private static IconLocation GetDefaultFileIconLocation(string extension)
        {
            IconLocation location;
            Dictionary<string, IconLocation> dictionary;
            lock ((dictionary = ExtIconMap))
            {
                if (ExtIconMap.TryGetValue(extension, out location))
                {
                    return location;
                }
            }
            string iconLocation = null;
            try
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(extension))
                {
                    if (key != null)
                    {
                        string name = key.GetValue(null) as string;
                        using (RegistryKey key2 = (name != null) ? Registry.ClassesRoot.OpenSubKey(name) : null)
                        {
                            if (key2 != null)
                            {
                                using (RegistryKey key3 = key2.OpenSubKey("DefaultIcon"))
                                {
                                    if (key3 != null)
                                    {
                                        iconLocation = key3.GetValue(null) as string;
                                    }
                                }
                                if (iconLocation == null)
                                {
                                    using (RegistryKey key4 = key2.OpenSubKey("CurVer"))
                                    {
                                        if (key4 != null)
                                        {
                                            name = key4.GetValue(null) as string;
                                            if (name != null)
                                            {
                                                using (RegistryKey key5 = Registry.ClassesRoot.OpenSubKey(name + @"\DefaultIcon"))
                                                {
                                                    if (key5 != null)
                                                    {
                                                        iconLocation = key5.GetValue(null) as string;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (iconLocation == null)
                            {
                                string str3 = key.GetValue("PerceivedType") as string;
                                if (str3 != null)
                                {
                                    using (RegistryKey key6 = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\" + str3 + @"\DefaultIcon"))
                                    {
                                        if (key6 != null)
                                        {
                                            iconLocation = key6.GetValue(null) as string;
                                        }
                                    }
                                }
                            }
                            if ((iconLocation == null) && (key2 != null))
                            {
                                using (RegistryKey key7 = key2.OpenSubKey(@"shell\open\command"))
                                {
                                    if (key7 != null)
                                    {
                                        string str4 = key7.GetValue(null) as string;
                                        if (str4 != null)
                                        {
                                            int index;
                                            int startIndex = 0;
                                            if (str4.StartsWith("\"", StringComparison.Ordinal))
                                            {
                                                startIndex = 1;
                                                index = str4.IndexOf('"', 2) - 1;
                                            }
                                            else
                                            {
                                                index = str4.IndexOf(' ');
                                            }
                                            iconLocation = (index > 0) ? str4.Substring(startIndex, index) : str4;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (SecurityException)
            {
            }
            location = IconLocation.TryParse(iconLocation);
            lock ((dictionary = ExtIconMap))
            {
                ExtIconMap[extension] = location;
            }
            return location;
        }

        public override Image GetDefaultIcon(DefaultIcon icon, Size size)
        {
            Image image;
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
                    if (HRESULT.SUCCEEDED(Shell32.SHGetStockIconInfo(siid, SHGSI.SHGSI_ICONLOCATION, ref shstockiconinfo)))
                    {
                        return LoadIconFromLocation2(new IconLocation(shstockiconinfo.szPath, shstockiconinfo.iIcon), ref size);
                    }
                }
            }
            switch (icon)
            {
                case DefaultIcon.UnknownFile:
                    return GetDefaultContainerIcon("Unknown", 0, ref size);

                case DefaultIcon.DefaultDocument:
                    return LoadShellDllIcon(1, size);

                case DefaultIcon.DefaultApplication:
                    return LoadShellDllIcon(2, size);

                case DefaultIcon.Desktop:
                    return LoadShellDllIcon(0x22, size);

                case DefaultIcon.Drive:
                    return GetDefaultContainerIcon("Drive", 8, ref size);

                case DefaultIcon.Favorites:
                    return GetClsidIcon(CLSID.CLSID_FAVORITES, 0x2b, size);

                case DefaultIcon.Folder:
                    return GetDefaultContainerIcon("Folder", 3, ref size);

                case DefaultIcon.MyComputer:
                    return GetClsidIcon(CLSID.CLSID_MYCOMPUTER, 15, size);

                case DefaultIcon.MyDocuments:
                    return GetClsidIcon(CLSID.CLSID_MYDOCUMENTS, OS.IsWinXP ? -235 : 20, size);

                case DefaultIcon.MyPictures:
                case DefaultIcon.MyMusic:
                case DefaultIcon.MyVideos:
                    if (!OS.IsWinXP)
                    {
                        goto Label_0282;
                    }
                    image = null;
                    switch (icon)
                    {
                        case DefaultIcon.MyPictures:
                            image = LoadShellDllIcon(-236, size);
                            break;

                        case DefaultIcon.MyMusic:
                            image = LoadShellDllIcon(-237, size);
                            break;

                        case DefaultIcon.MyVideos:
                            image = LoadShellDllIcon(-238, size);
                            break;
                    }
                    break;

                case DefaultIcon.SearchFolder:
                    return LoadShellDllIcon(0x2d, size);

                case DefaultIcon.NetworkNeighborhood:
                    return GetClsidIcon(CLSID.CLSID_NETWORK_NEIGHBORHOOD, 0x11, size);

                case DefaultIcon.EntireNetwork:
                    return LoadShellDllIcon(13, size);

                case DefaultIcon.NetworkWorkgroup:
                    return LoadShellDllIcon(0x12, size);

                case DefaultIcon.NetworkProvider:
                    return LoadShellDllIcon(14, size);

                case DefaultIcon.NetworkServer:
                    return LoadShellDllIcon(15, size);

                case DefaultIcon.NetworkFolder:
                    if (!OS.IsWinME && !OS.IsWin2k)
                    {
                        return GetDefaultContainerIcon("Folder", 3, ref size);
                    }
                    return LoadShellDllIcon(-172, size);

                case DefaultIcon.OverlayLink:
                    return LoadShellDllIcon(0x1d, size);

                case DefaultIcon.OverlayShare:
                    return LoadShellDllIcon(0x1c, size);

                case DefaultIcon.OverlayUnreadable:
                    return null;

                default:
                    throw new InvalidEnumArgumentException();
            }
            if (image != null)
            {
                return image;
            }
        Label_0282:
            return GetDefaultContainerIcon("Folder", 3, ref size);
        }

        public override Image GetDriveIcon(DriveInfo drive, Size size)
        {
            IconLocation iconLocation = null;
            char driveChar = drive.Name.Substring(0, 1).ToUpper()[0];
            bool flag = (driveChar == 'A') || (driveChar == 'B');
            bool isReady = false;
            VolumeType unknown = VolumeType.Unknown;
            try
            {
                VolumeInfo info = VolumeCache.Get(drive.Name);
                unknown = info.VolumeType;
                switch (unknown)
                {
                    case VolumeType.Removable:
                        if (!flag)
                        {
                            break;
                        }
                        unknown = VolumeType.Floppy3;
                        isReady = false;
                        goto Label_00B5;

                    case VolumeType.Fixed:
                    case VolumeType.CDRom:
                    case VolumeType.Ram:
                    case VolumeType.DVDRom:
                        isReady = info.IsReady;
                        goto Label_00B5;

                    case VolumeType.Network:
                        goto Label_00B5;

                    case VolumeType.Floppy3:
                    case VolumeType.Floppy5:
                        isReady = false;
                        goto Label_00B5;

                    default:
                        goto Label_00B5;
                }
                isReady = info.IsReady;
            }
            catch
            {
                isReady = !flag && drive.IsReady;
            }
        Label_00B5:
            if (isReady)
            {
                try
                {
                    string path = Path.Combine(drive.RootDirectory.FullName, "autorun.inf");
                    if (System.IO.File.Exists(path))
                    {
                        string str2 = Ini.ReadValue(path, "autorun", "icon");
                        if (!string.IsNullOrEmpty(str2))
                        {
                            iconLocation = IconLocation.TryParse(Path.Combine(drive.RootDirectory.FullName, str2));
                        }
                    }
                }
                catch
                {
                }
            }
            if (iconLocation != null)
            {
                Image image = LoadIconFromLocation2(iconLocation, ref size);
                if (image != null)
                {
                    return image;
                }
            }
            if (unknown == VolumeType.Unknown)
            {
                unknown = (VolumeType) drive.DriveType;
                if ((unknown == VolumeType.Removable) && flag)
                {
                    unknown = VolumeType.Floppy3;
                }
            }
            return this.GetDriveIcon(drive, driveChar, unknown, ref size);
        }

        protected virtual Image GetDriveIcon(DriveInfo drive, char driveChar, VolumeType volumeType, ref Size size)
        {
            Image image = null;
            IconLocation iconLocation = null;
            if (iconLocation == null)
            {
                iconLocation = LoadDefaultDriveIcon(Registry.LocalMachine, string.Format(@"Software\Microsoft\Windows\CurrentVersion\Explorer\DriveIcons\{0}\DefaultIcon", driveChar));
            }
            if (iconLocation == null)
            {
                iconLocation = LoadDefaultDriveIcon(Registry.CurrentUser, string.Format(@"Applications\Explorer.exe\Drives\{0}\DefaultIcon", driveChar));
            }
            if (iconLocation == null)
            {
                iconLocation = LoadDefaultDriveIcon(Registry.ClassesRoot, string.Format(@"Applications\Explorer.exe\Drives\{0}\DefaultIcon", driveChar));
            }
            if (iconLocation != null)
            {
                image = LoadIconFromLocation2(iconLocation, ref size);
            }
            if (image == null)
            {
                image = this.GetVolumeIcon(volumeType, driveChar, ref size);
            }
            return image;
        }

        private static string GetFileClass(string extension)
        {
            return (ReadRegistryValue(Registry.ClassesRoot, extension, null) as string);
        }

        public override Image GetFileIcon(string fileName, Size size)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            return this.GetFileIcon(fileName, Path.GetExtension(fileName), ref size);
        }

        protected virtual Image GetFileIcon(string fileName, string extension, ref Size size)
        {
            string g = null;
            string fileClass = GetFileClass(extension);
            if (!string.IsNullOrEmpty(fileClass))
            {
                g = ReadRegistryValue(Registry.ClassesRoot, fileClass + @"\shellex\IconHandler", null) as string;
            }
            IconLocation iconLocation = null;
            Image defaultFileIconFromExt = null;
            if (g != null)
            {
                object obj2;
                Guid rclsid = new Guid(g);
                if (ActiveX.CoCreateInstance(rclsid, null, CLSCTX.CLSCTX_INPROC_HANDLER | CLSCTX.CLSCTX_INPROC_SERVER, typeof(IPersistFile).GUID, out obj2) == 0)
                {
                    try
                    {
                        ((IPersistFile) obj2).Load(fileName, 0);
                        StringBuilder szIconFile = new StringBuilder(0x400);
                        GIL_OUT pwFlags = 0;
                        string iconFileName = null;
                        int piIndex = 0;
                        int num2 = 1;
                        IExtractIconW nw = obj2 as IExtractIconW;
                        if (nw != null)
                        {
                            num2 = nw.GetIconLocation(GIL_IN.GIL_FORSHELL, szIconFile, (uint) szIconFile.Capacity, out piIndex, out pwFlags);
                        }
                        else
                        {
                            IExtractIconA na = obj2 as IExtractIconA;
                            if (na != null)
                            {
                                num2 = na.GetIconLocation(GIL_IN.GIL_FORSHELL, szIconFile, (uint) szIconFile.Capacity, out piIndex, out pwFlags);
                            }
                        }
                        if (num2 == 0)
                        {
                            iconFileName = Environment.ExpandEnvironmentVariables(szIconFile.ToString());
                            if ((pwFlags & GIL_OUT.GIL_NOTFILENAME) == ((GIL_OUT) 0))
                            {
                                iconLocation = new IconLocation(iconFileName, piIndex);
                            }
                            else
                            {
                                defaultFileIconFromExt = ExtractIcon(obj2, new ClassIconLocation(ref rclsid, iconFileName, piIndex), (pwFlags & GIL_OUT.GIL_DONTCACHE) == ((GIL_OUT) 0), ref size);
                                if (defaultFileIconFromExt != null)
                                {
                                    return defaultFileIconFromExt;
                                }
                                try
                                {
                                    string str4 = Path.GetExtension(iconFileName);
                                    if (!string.IsNullOrEmpty(str4))
                                    {
                                        iconLocation = new IconLocation(iconFileName, 0);
                                        extension = str4;
                                    }
                                }
                                catch (ArgumentException)
                                {
                                }
                            }
                        }
                    }
                    catch (IOException)
                    {
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                    catch (COMException exception)
                    {
                        if (Marshal.GetHRForException(exception) != -2147467259)
                        {
                            Debug.WriteLine(exception, "ImageProvider");
                        }
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(obj2);
                    }
                }
            }
            if (iconLocation == null)
            {
                iconLocation = GetDefaultFileIconLocation(extension);
            }
            if (iconLocation != null)
            {
                if (iconLocation.IconFileName == "%1")
                {
                    iconLocation = new IconLocation(fileName, 0);
                }
                defaultFileIconFromExt = LoadIconFromLocation2(iconLocation, ref size);
            }
            if (defaultFileIconFromExt == null)
            {
                defaultFileIconFromExt = this.GetDefaultFileIconFromExt(extension, ref size);
            }
            return defaultFileIconFromExt;
        }

        public override Image GetFolderIcon(string folderName, Size size)
        {
            StreamReader reader;
            char[] chArray;
            if (folderName == null)
            {
                throw new ArgumentNullException("folderName");
            }
            Image defaultIcon = null;
            FileInfo info = null;
            if (System.IO.Directory.Exists(folderName))
            {
                try
                {
                    if ((System.IO.File.GetAttributes(folderName) & FileAttributes.ReparsePoint) > 0)
                    {
                        ReparsePointInfo reparsePointInfo = ReparsePoint.GetReparsePointInfo(folderName);
                        if (reparsePointInfo.ReparseType == ReparseType.MountPoint)
                        {
                            try
                            {
                                VolumeInfo info3 = VolumeCache.Get(reparsePointInfo.Target);
                                defaultIcon = this.GetVolumeIcon(info3.VolumeType, '\0', ref size);
                            }
                            catch
                            {
                                defaultIcon = this.GetDefaultIcon(DefaultIcon.Drive, size);
                            }
                        }
                    }
                }
                catch
                {
                }
                if (defaultIcon == null)
                {
                    info = new FileInfo(Path.Combine(folderName, "desktop.ini"));
                }
            }
            if (((info == null) || !info.Exists) || ((info.Attributes & (FileAttributes.System | FileAttributes.Hidden)) <= 0))
            {
                goto Label_048D;
            }
            string currentValue = null;
            string a = null;
            string path = null;
            string str4 = null;
            int result = 0;
            ShellFolderType unknown = ShellFolderType.Unknown;
            try
            {
                reader = info.OpenText();
            }
            catch
            {
                reader = null;
            }
            if (reader != null)
            {
                using (IniReader reader2 = new IniReader(reader, true))
                {
                    bool flag = false;
                    while (reader2.Read())
                    {
                        bool flag2 = false;
                        switch (reader2.ElementType)
                        {
                            case IniElementType.Section:
                                if (!flag)
                                {
                                    break;
                                }
                                flag2 = true;
                                goto Label_02BF;

                            case IniElementType.KeyValuePair:
                                if (flag)
                                {
                                    switch (reader2.CurrentKey.ToLower())
                                    {
                                        case "clsid":
                                            goto Label_0239;

                                        case "clsid2":
                                            goto Label_0254;

                                        case "iconresource":
                                            goto Label_025F;

                                        case "iconfile":
                                            goto Label_026F;

                                        case "iconindex":
                                            goto Label_027F;

                                        case "foldertype":
                                            goto Label_0298;

                                        case "uiclsid":
                                            goto Label_02B2;
                                    }
                                }
                                goto Label_02BF;

                            default:
                                goto Label_02BF;
                        }
                        flag = reader2.CheckCurrentSection(".ShellClassInfo");
                        goto Label_02BF;
                    Label_0239:
                        if (string.IsNullOrEmpty(currentValue))
                        {
                            currentValue = reader2.CurrentValue;
                        }
                        goto Label_02BF;
                    Label_0254:
                        a = reader2.CurrentValue;
                        goto Label_02BF;
                    Label_025F:
                        path = Environment.ExpandEnvironmentVariables(reader2.CurrentValue);
                        goto Label_02BF;
                    Label_026F:
                        str4 = Environment.ExpandEnvironmentVariables(reader2.CurrentValue);
                        goto Label_02BF;
                    Label_027F:
                        if (!int.TryParse(reader2.CurrentValue, out result))
                        {
                            result = 0;
                        }
                        goto Label_02BF;
                    Label_0298:
                        if (!EnumHelper.TryParse<ShellFolderType>(reader2.CurrentValue, true, out unknown))
                        {
                            unknown = ShellFolderType.Unknown;
                        }
                        goto Label_02BF;
                    Label_02B2:
                        currentValue = reader2.CurrentValue;
                    Label_02BF:
                        if (flag2)
                        {
                            goto Label_02F5;
                        }
                    }
                }
            }
        Label_02F5:
            chArray = null;
            if ((path != null) && (path.IndexOfAny(chArray ?? (chArray = Path.GetInvalidPathChars())) < 0))
            {
                if (!Path.IsPathRooted(path))
                {
                    path = Path.Combine(folderName, path);
                }
                IconLocation iconLocation = IconLocation.TryParse(path);
                if (iconLocation != null)
                {
                    defaultIcon = LoadIconFromLocation2(iconLocation, ref size);
                }
            }
            if (((defaultIcon == null) && (str4 != null)) && (str4.IndexOfAny(chArray ?? (chArray = Path.GetInvalidPathChars())) < 0))
            {
                if (!Path.IsPathRooted(str4))
                {
                    str4 = Path.Combine(folderName, str4);
                }
                defaultIcon = LoadIconFromLocation2(new IconLocation(str4, result), ref size);
            }
            if ((defaultIcon == null) && (currentValue != null))
            {
                defaultIcon = this.GetClsidIcon(currentValue, ref size);
            }
            if ((defaultIcon == null) && string.Equals(a, "{0AFACED1-E828-11D1-9187-B532F1E9575D}", StringComparison.OrdinalIgnoreCase))
            {
                string str5 = Path.Combine(folderName, "target.lnk");
                if (System.IO.File.Exists(str5))
                {
                    defaultIcon = this.GetFileIcon(str5, size);
                }
            }
            if ((defaultIcon == null) && (unknown != ShellFolderType.Unknown))
            {
                switch (unknown)
                {
                    case ShellFolderType.Documents:
                    case ShellFolderType.MyDocuments:
                        defaultIcon = this.GetDefaultIcon(DefaultIcon.MyDocuments, size);
                        goto Label_048D;

                    case ShellFolderType.Pictures:
                    case ShellFolderType.MyPictures:
                    case ShellFolderType.PhotoAlbum:
                        defaultIcon = this.GetDefaultIcon(DefaultIcon.MyPictures, size);
                        goto Label_048D;

                    case ShellFolderType.Music:
                    case ShellFolderType.MyMusic:
                    case ShellFolderType.MusicArtist:
                    case ShellFolderType.MusicAlbum:
                        defaultIcon = this.GetDefaultIcon(DefaultIcon.MyMusic, size);
                        goto Label_048D;

                    case ShellFolderType.Videos:
                    case ShellFolderType.MyVideos:
                    case ShellFolderType.VideoAlbum:
                        defaultIcon = this.GetDefaultIcon(DefaultIcon.MyVideos, size);
                        goto Label_048D;
                }
            }
        Label_048D:
            if (defaultIcon == null)
            {
                defaultIcon = this.GetDefaultIcon(DefaultIcon.Folder, size);
            }
            return defaultIcon;
        }

        private static bool GetIconFromCache(IconLocation iconLocation, ref Size size, out Image icon)
        {
            if (iconLocation == null)
            {
                throw new ArgumentNullException("iconLocation");
            }
            icon = null;
            lock (LocationIconCache)
            {
                IDictionary<Size, Image> dictionary;
                if (LocationIconCache.TryGetValue(iconLocation, out dictionary))
                {
                    if (dictionary != null)
                    {
                        return dictionary.TryGetValue(size, out icon);
                    }
                    return true;
                }
            }
            return false;
        }

        public override Image GetItemOverlay(string itemName, Size size)
        {
            if (itemName == null)
            {
                throw new ArgumentNullException("itemName");
            }
            bool flag = (PathHelper.IsRootPath(itemName) || PathHelper.HasTrailingDirectorySeparator(itemName)) || System.IO.Directory.Exists(itemName);
            Image defaultIcon = null;
            if (Shell32.ShellDllVersion.Major > 4)
            {
                lock (OverlayCacheLock)
                {
                    if (OverlayCache == null)
                    {
                        CreateOverlayCache();
                    }
                }
                foreach (OverlayIdentifier identifier in OverlayCache)
                {
                    if (identifier.IsMemberOf(itemName, SFGAO.SFGAO_FILESYSTEM | (flag ? SFGAO.SFGAO_FOLDER : ((SFGAO) 0))))
                    {
                        defaultIcon = LoadIconFromLocation2(identifier.IconLocation, ref size);
                        break;
                    }
                }
            }
            if (defaultIcon == null)
            {
                if (flag)
                {
                    if (IsShare(itemName))
                    {
                        defaultIcon = this.GetDefaultIcon(DefaultIcon.OverlayShare, size);
                    }
                    return defaultIcon;
                }
                if (IsShortcut(itemName))
                {
                    defaultIcon = this.GetDefaultIcon(DefaultIcon.OverlayLink, size);
                }
            }
            return defaultIcon;
        }

        protected override Image GetShellLinkIcon(ShellLink link, ref Size size)
        {
            string str;
            int num;
            link.GetIconLocation(out str, out num);
            if (!string.IsNullOrEmpty(str))
            {
                IconLocation iconLocation = new IconLocation(Environment.ExpandEnvironmentVariables(str), num);
                return LoadIconFromLocation2(iconLocation, ref size);
            }
            return null;
        }

        protected virtual Image GetVolumeIcon(VolumeType volumeType, char driveChar, ref Size size)
        {
            int iconIndex = 0;
            SHSTOCKICONID siid = SHSTOCKICONID.SIID_DRIVEUNKNOWN;
            switch (volumeType)
            {
                case VolumeType.Removable:
                    if ((driveChar != 'A') && (driveChar != 'B'))
                    {
                        iconIndex = 7;
                        siid = SHSTOCKICONID.SIID_DRIVEREMOVE;
                        break;
                    }
                    iconIndex = 6;
                    siid = SHSTOCKICONID.SIID_DRIVE35;
                    break;

                case VolumeType.Fixed:
                    iconIndex = 8;
                    siid = SHSTOCKICONID.SIID_DRIVEFIXED;
                    break;

                case VolumeType.Network:
                    iconIndex = 9;
                    siid = SHSTOCKICONID.SIID_DRIVENET;
                    break;

                case VolumeType.CDRom:
                    iconIndex = 11;
                    siid = SHSTOCKICONID.SIID_DRIVECD;
                    break;

                case VolumeType.Ram:
                    iconIndex = 12;
                    siid = SHSTOCKICONID.SIID_DRIVERAM;
                    break;

                case VolumeType.DVDRom:
                    iconIndex = 0xb1;
                    siid = SHSTOCKICONID.SIID_DRIVEDVD;
                    break;

                case VolumeType.Floppy3:
                    iconIndex = 6;
                    siid = SHSTOCKICONID.SIID_DRIVE35;
                    break;

                case VolumeType.Floppy5:
                    iconIndex = 5;
                    siid = SHSTOCKICONID.SIID_DRIVE525;
                    break;

                case VolumeType.Flash:
                    iconIndex = OS.IsWinXP ? 0xbd : 7;
                    siid = SHSTOCKICONID.SIID_MEDIACOMPACTFLASH;
                    break;
            }
            if (OS.IsWinVista)
            {
                SHSTOCKICONINFO shstockiconinfo;
                shstockiconinfo = new SHSTOCKICONINFO {
                    cbSize = Marshal.SizeOf(shstockiconinfo)
                };
                if (HRESULT.SUCCEEDED(Shell32.SHGetStockIconInfo(siid, SHGSI.SHGSI_ICONLOCATION, ref shstockiconinfo)))
                {
                    return LoadIconFromLocation2(new IconLocation(shstockiconinfo.szPath, shstockiconinfo.iIcon), ref size);
                }
            }
            return LoadShellDllIcon(iconIndex, size);
        }

        private static bool IsIconSizeInCache(IconLocation iconLocation, Size size)
        {
            lock (LocationIconCache)
            {
                IDictionary<Size, Image> dictionary;
                if (LocationIconCache.TryGetValue(iconLocation, out dictionary) && (dictionary != null))
                {
                    return dictionary.ContainsKey(size);
                }
            }
            return false;
        }

        public static bool IsShare(string folderName)
        {
            Dictionary<string, int> target;
            if (ShareSource == EnumShareSource.None)
            {
                return false;
            }
            ShareCacheLock.AcquireReaderLock(-1);
            try
            {
                if ((ShareCache != null) && ShareCache.IsAlive)
                {
                    target = (Dictionary<string, int>) ShareCache.Target;
                    return target.ContainsKey(PathHelper.IncludeTrailingDirectorySeparator(folderName));
                }
            }
            finally
            {
                ShareCacheLock.ReleaseReaderLock();
            }
            target = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (ShareSource == EnumShareSource.NetApi)
            {
                try
                {
                    foreach (string str in LmHelper.GetShares(null, STYPE.STYPE_DISKTREE))
                    {
                        target[PathHelper.IncludeTrailingDirectorySeparator(str)] = 0;
                    }
                }
                catch (Win32Exception exception)
                {
                    switch (exception.NativeErrorCode)
                    {
                        case 5:
                            ShareSource = EnumShareSource.Registry;
                            goto Label_013E;

                        case 0x842:
                            ShareSource = EnumShareSource.None;
                            return false;
                    }
                    ShareSource = EnumShareSource.None;
                    new TraceSource("Error").TraceException(TraceEventType.Error, exception);
                    return false;
                }
            }
        Label_013E:
            if (ShareSource == EnumShareSource.Registry)
            {
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Services\LanmanServer\Shares"))
                    {
                        if (key != null)
                        {
                            string[] valueNames = key.GetValueNames();
                            foreach (string str2 in valueNames)
                            {
                                if (!str2.EndsWith("$", StringComparison.OrdinalIgnoreCase))
                                {
                                    string[] strArray2 = key.GetValue(str2) as string[];
                                    if (strArray2 != null)
                                    {
                                        bool flag = false;
                                        string str3 = null;
                                        foreach (string str4 in strArray2)
                                        {
                                            if (str4.StartsWith("Path=", StringComparison.Ordinal))
                                            {
                                                str3 = str4.Substring(5);
                                            }
                                            else if (str4.Equals("Type=0", StringComparison.Ordinal))
                                            {
                                                flag = true;
                                            }
                                        }
                                        if (!(!flag || string.IsNullOrEmpty(str3)))
                                        {
                                            target[PathHelper.IncludeTrailingDirectorySeparator(str3)] = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (SecurityException)
                {
                }
            }
            bool flag2 = target.ContainsKey(PathHelper.IncludeTrailingDirectorySeparator(folderName));
            ShareCacheLock.AcquireWriterLock(-1);
            try
            {
                ShareCache = new WeakReference(target);
            }
            finally
            {
                ShareCacheLock.ReleaseWriterLock();
            }
            return flag2;
        }

        public static bool IsShortcut(string fileName)
        {
            bool flag;
            Dictionary<string, bool> dictionary;
            string extension = Path.GetExtension(fileName);
            lock ((dictionary = ShortcutCache))
            {
                if (ShortcutCache.TryGetValue(extension, out flag))
                {
                    return flag;
                }
            }
            flag = ReadRegistryValue(Registry.ClassesRoot, GetFileClass(extension), "IsShortcut") != null;
            lock ((dictionary = ShortcutCache))
            {
                ShortcutCache[extension] = flag;
            }
            return flag;
        }

        private static IconLocation LoadDefaultDriveIcon(RegistryKey registryRoot, string subKeyName)
        {
            return IconLocation.TryParse(ReadRegistryValue(registryRoot, subKeyName, null) as string);
        }

        public static Image LoadIcon(string fileName, int iconIndex, Size size)
        {
            Debug.WriteLine(string.Format("LoadIcon('{0}', {1}, {2}", fileName, iconIndex, size), "ImageProvider");
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            Icon icon = null;
            IntPtr zero = IntPtr.Zero;
            if (fileName.EndsWith(".ico", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    icon = new Icon(fileName, size);
                }
                catch
                {
                    icon = null;
                }
            }
            try
            {
                if (icon == null)
                {
                    try
                    {
                        LOAD_LIBRARY dwFlags = LOAD_LIBRARY.DONT_RESOLVE_DLL_REFERENCES;
                        if (OS.IsWinNT)
                        {
                            dwFlags |= LOAD_LIBRARY.LOAD_LIBRARY_AS_DATAFILE;
                        }
                        if (OS.IsWinVista)
                        {
                            dwFlags |= LOAD_LIBRARY.LOAD_LIBRARY_AS_IMAGE_RESOURCE;
                        }
                        Microsoft.Win32.SafeHandles.SafeLibraryHandle hInst = Windows.LoadLibraryEx(fileName, IntPtr.Zero, dwFlags);
                        Win32Exception exception = null;
                        if (hInst.IsInvalid)
                        {
                            Debug.WriteLine("> Lib.IsInvalid = true", "ImageProvider");
                            zero = Windows.LoadImage(IntPtr.Zero, fileName, IMAGE.IMAGE_ICON, size.Width, size.Height, LR.LR_DEFAULTCOLOR | LR.LR_LOADFROMFILE);
                            if (zero == IntPtr.Zero)
                            {
                                exception = new Win32Exception();
                            }
                        }
                        else
                        {
                            try
                            {
                                if (iconIndex < 0)
                                {
                                    Debug.WriteLine("> Direct icon load by resource id", "ImageProvider");
                                    zero = Windows.LoadImage(hInst, (IntPtr) -iconIndex, IMAGE.IMAGE_ICON, size.Width, size.Height, LR.LR_DEFAULTCOLOR);
                                }
                                else
                                {
                                    Debug.WriteLine("> Find resource id by index", "ImageProvider");
                                    EnumFileIconsHolder holder = new EnumFileIconsHolder(iconIndex);
                                    Windows.EnumResourceNames(hInst, (IntPtr) 14, holder.EnumFileIconsHandler, IntPtr.Zero);
                                    Debug.WriteLineIf(holder.ResourceName != null, string.Format("> EnumIcons.ResourceName = {0}", holder.ResourceName), "ImageProvider");
                                    Debug.WriteLineIf(holder.ResourceName == null, string.Format("> EnumIcons.ResourceId = {0}", holder.ResourceId), "ImageProvider");
                                    if (holder.ResourceName != null)
                                    {
                                        zero = Windows.LoadImage(hInst, holder.ResourceName, IMAGE.IMAGE_ICON, size.Width, size.Height, LR.LR_DEFAULTCOLOR);
                                    }
                                    else
                                    {
                                        zero = Windows.LoadImage(hInst, (IntPtr) holder.ResourceId, IMAGE.IMAGE_ICON, size.Width, size.Height, LR.LR_DEFAULTCOLOR);
                                    }
                                }
                                if (zero == IntPtr.Zero)
                                {
                                    exception = new Win32Exception();
                                }
                            }
                            finally
                            {
                                hInst.Close();
                            }
                        }
                        if (exception != null)
                        {
                            Debug.WriteLine("> Win32Error - " + exception.Message, "ImageProvider");
                        }
                    }
                    catch (AccessViolationException)
                    {
                        Debug.WriteLine("> AccessViolation - Try to extract associated icon", "ImageProvider");
                        Icon original = Icon.ExtractAssociatedIcon(fileName);
                        if (original.Size == size)
                        {
                            icon = original;
                        }
                        else
                        {
                            icon = new Icon(original, size);
                            original.Dispose();
                        }
                    }
                }
                Debug.WriteLine(string.Format("> IconHandle = {0}", zero), "ImageProvider");
                if (zero != IntPtr.Zero)
                {
                    return ImageHelper.IconToBitmap(zero);
                }
                if (icon != null)
                {
                    return ImageHelper.IconToBitmap(icon, size);
                }
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Windows.DestroyIcon(zero);
                }
                if (icon != null)
                {
                    icon.Dispose();
                }
            }
            return null;
        }

        public static void LoadIconCache(Stream cacheStream)
        {
            BinaryReader reader = new BinaryReader(cacheStream, Encoding.UTF8);
            if (new string(reader.ReadChars(4)) != "IMCH")
            {
                throw new InvalidDataException();
            }
            if (reader.ReadInt32() != 1)
            {
                throw new InvalidDataException("Unknown icon cache format version.");
            }
            lock (LocationIconCache)
            {
                LocationIconCache.Clear();
                int num = reader.ReadInt32();
                for (int i = 0; i < num; i++)
                {
                    IconLocation location;
                    switch (reader.ReadByte())
                    {
                        case 0:
                            location = new IconLocation(reader.ReadString(), reader.ReadInt32());
                            break;

                        case 1:
                        {
                            Guid classId = new Guid(reader.ReadBytes(0x10));
                            location = new ClassIconLocation(ref classId, reader.ReadString(), reader.ReadInt32());
                            break;
                        }
                        default:
                            throw new InvalidDataException("Unknown icon location format.");
                    }
                    IDictionary<Size, Image> dictionary = IconCollection.Create();
                    int num4 = reader.ReadInt32();
                    for (int j = 0; j < num4; j++)
                    {
                        Size key = new Size(reader.ReadInt32(), reader.ReadInt32());
                        int num6 = reader.ReadInt32();
                        long position = cacheStream.Position;
                        using (Image image = Image.FromStream(new SubStream(cacheStream, FileAccess.Read, (long) num6)))
                        {
                            dictionary.Add(key, new Bitmap(image));
                        }
                        cacheStream.Position = position + num6;
                    }
                    LocationIconCache.Add(location, (dictionary.Count == 0) ? null : dictionary);
                }
            }
        }

        public static void LoadIconCache(string fileName)
        {
            using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                LoadIconCache(stream);
            }
        }

        public static Image LoadIconFromLocation(IconLocation iconLocation, Size size)
        {
            Image image;
            if (iconLocation == null)
            {
                throw new ArgumentNullException("iconLocation");
            }
            if (!GetIconFromCache(iconLocation, ref size, out image))
            {
                image = LoadIcon(iconLocation.IconFileName, iconLocation.IconIndex, size);
                AddIconToCache(iconLocation, ref size, image);
            }
            return image;
        }

        private static Image LoadIconFromLocation2(IconLocation iconLocation, ref Size size)
        {
            if (iconLocation.IconFileName.Equals(ShellDllLocation, StringComparison.OrdinalIgnoreCase))
            {
                return LoadShellDllIcon(iconLocation.IconIndex, size);
            }
            return LoadIconFromLocation(iconLocation, size);
        }

        public static Image LoadShellDllIcon(int iconIndex, Size size)
        {
            IconLocation location;
            Dictionary<int, IconLocation> dictionary;
            lock ((dictionary = ShellDllIconMap))
            {
                ShellDllIconMap.TryGetValue(iconIndex, out location);
            }
            if (location == null)
            {
                location = IconLocation.TryParse(ReadRegistryValue(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Icons", iconIndex.ToString()) as string);
                if (location == null)
                {
                    location = new IconLocation(ShellDllLocation, iconIndex);
                }
                lock ((dictionary = ShellDllIconMap))
                {
                    ShellDllIconMap[iconIndex] = location;
                }
            }
            return LoadIconFromLocation(location, size);
        }

        private static object ReadRegistryValue(RegistryKey key, string subKeyName, string valueName)
        {
            object obj2;
            if ((key == null) || string.IsNullOrEmpty(subKeyName))
            {
                return null;
            }
            try
            {
                using (RegistryKey key2 = key.OpenSubKey(subKeyName))
                {
                    obj2 = (key2 != null) ? key2.GetValue(valueName) : null;
                }
            }
            catch (SecurityException)
            {
                obj2 = null;
            }
            return obj2;
        }

        public static void ResetIconCache()
        {
            lock (LocationIconCache)
            {
                LocationIconCache.Clear();
            }
            lock (ShellDllIconMap)
            {
                ShellDllIconMap.Clear();
            }
            lock (ExtIconMap)
            {
                ExtIconMap.Clear();
            }
        }

        public static void SaveIconCache(Stream cacheStream)
        {
            BinaryWriter writer = new BinaryWriter(cacheStream, Encoding.UTF8);
            writer.Write("IMCH".ToCharArray());
            writer.Write(1);
            lock (LocationIconCache)
            {
                writer.Write(LocationIconCache.Count);
                foreach (KeyValuePair<IconLocation, IDictionary<Size, Image>> pair in LocationIconCache)
                {
                    ClassIconLocation key = pair.Key as ClassIconLocation;
                    if (key != null)
                    {
                        writer.Write((byte) 1);
                        byte[] buffer = key.ClassId.ToByteArray();
                        writer.Write(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        writer.Write((byte) 0);
                    }
                    writer.Write(pair.Key.IconFileName);
                    writer.Write(pair.Key.IconIndex);
                    if (pair.Value == null)
                    {
                        writer.Write(0);
                    }
                    else
                    {
                        writer.Write(pair.Value.Count);
                        foreach (KeyValuePair<Size, Image> pair2 in pair.Value)
                        {
                            writer.Write(pair2.Key.Width);
                            writer.Write(pair2.Key.Height);
                            using (MemoryStream stream = new MemoryStream(0x2000))
                            {
                                pair2.Value.Save(stream, ImageFormat.Png);
                                writer.Write((int) stream.Length);
                                stream.WriteTo(cacheStream);
                            }
                        }
                    }
                }
            }
        }

        public static void SaveIconCache(string fileName)
        {
            using (Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                SaveIconCache(stream);
            }
        }

        private class ClassIconLocation : IconLocation
        {
            public readonly Guid ClassId;

            public ClassIconLocation(ref Guid classId, string iconFileName, int iconIndex) : base(iconFileName, iconIndex)
            {
                this.ClassId = classId;
            }

            public override bool Equals(object obj)
            {
                CustomImageProvider.ClassIconLocation location = obj as CustomImageProvider.ClassIconLocation;
                return (((location != null) && base.Equals(obj)) && (this.ClassId == location.ClassId));
            }

            public override int GetHashCode()
            {
                return (base.GetHashCode() ^ this.ClassId.GetHashCode());
            }

            public override string ToString()
            {
                return string.Format("{0},{1},{2}", this.ClassId, base.IconFileName, base.IconIndex);
            }
        }

        private class EnumFileIconsHolder
        {
            public EnumResNameDelegate EnumFileIconsHandler;
            private int IconIndex;
            public int ResourceId;
            public string ResourceName;

            public EnumFileIconsHolder(int iconIndex)
            {
                this.IconIndex = iconIndex;
                this.EnumFileIconsHandler = new EnumResNameDelegate(this.EnumFileIcons);
            }

            private bool EnumFileIcons(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam)
            {
                if (this.IconIndex-- == 0)
                {
                    if (Macros.IS_INTRESOURCE(lpszName))
                    {
                        this.ResourceId = (int) lpszName;
                    }
                    else
                    {
                        this.ResourceName = Marshal.PtrToStringAuto(lpszName);
                    }
                    return false;
                }
                return true;
            }
        }

        private enum EnumShareSource
        {
            None,
            NetApi,
            Registry
        }

        private class OverlayIdentifier : IDisposable
        {
            private IntPtr FIdentifierPtr;
            public readonly Nomad.Commons.Drawing.IconLocation IconLocation;
            public readonly int Priority;

            public OverlayIdentifier(IntPtr identifier)
            {
                int num;
                Debug.WriteLine(string.Format("OverlayIdenfifier({0})", identifier), "ImageProvider");
                this.FIdentifierPtr = identifier;
                StringBuilder pwszIconFile = new StringBuilder(0x400);
                IShellIconOverlayIdentifier typedObjectForIUnknown = (IShellIconOverlayIdentifier) Marshal.GetTypedObjectForIUnknown(this.FIdentifierPtr, typeof(IShellIconOverlayIdentifier));
                try
                {
                    ISIOI isioi;
                    typedObjectForIUnknown.GetOverlayInfo(pwszIconFile, pwszIconFile.Capacity, out num, out isioi);
                    Debug.WriteLine(string.Format("> IconName = '{0}', IconIndex = {1}, Flags = {2}", pwszIconFile.ToString(), num, isioi), "ImageProvider");
                    this.Priority = typedObjectForIUnknown.GetPriority();
                    Debug.WriteLine("> Priority = " + this.Priority.ToString(), "ImageProvider");
                }
                finally
                {
                    Marshal.ReleaseComObject(typedObjectForIUnknown);
                }
                this.IconLocation = new Nomad.Commons.Drawing.IconLocation(pwszIconFile.ToString(), num);
            }

            public static int Compare(CustomImageProvider.OverlayIdentifier x, CustomImageProvider.OverlayIdentifier y)
            {
                return (x.Priority - y.Priority);
            }

            public void Dispose()
            {
                if (!(this.FIdentifierPtr == IntPtr.Zero))
                {
                    this.Dispose(true);
                    GC.SuppressFinalize(this);
                }
            }

            protected virtual void Dispose(bool disposing)
            {
                if (this.FIdentifierPtr != IntPtr.Zero)
                {
                    Marshal.Release(this.FIdentifierPtr);
                    this.FIdentifierPtr = IntPtr.Zero;
                }
            }

            ~OverlayIdentifier()
            {
                this.Dispose(false);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public bool IsMemberOf(string pwszPath, SFGAO dwAttrib)
            {
                bool flag;
                if (this.FIdentifierPtr == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("OverlayIdentifier");
                }
                IShellIconOverlayIdentifier typedObjectForIUnknown = (IShellIconOverlayIdentifier) Marshal.GetTypedObjectForIUnknown(this.FIdentifierPtr, typeof(IShellIconOverlayIdentifier));
                SEM uMode = Windows.SetErrorMode(SEM.SEM_DEFAULT | SEM.SEM_FAILCRITICALERRORS);
                try
                {
                    flag = typedObjectForIUnknown.IsMemberOf(pwszPath, dwAttrib) == 0;
                }
                finally
                {
                    Windows.SetErrorMode(uMode);
                    Marshal.ReleaseComObject(typedObjectForIUnknown);
                }
                return flag;
            }
        }

        private enum ShellFolderType
        {
            Unknown,
            Documents,
            MyDocuments,
            Pictures,
            MyPictures,
            PhotoAlbum,
            Music,
            MyMusic,
            MusicArtist,
            MusicAlbum,
            Videos,
            MyVideos,
            VideoAlbum
        }
    }
}

