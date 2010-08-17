using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace Shell
{
    public static class Shell32
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SHCreateShellItem(IntPtr pidlParent, [In, MarshalAs(UnmanagedType.Interface)] IShellFolder psfParent, IntPtr pidl, [MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        /// <summary>
        /// Returns the size, in bytes, of an ITEMIDLIST structure.
        /// </summary>
        /// <param name="pidl">A pointer to an ITEMIDLIST structure.</param>
        /// <returns>The size of the ITEMIDLIST structure specified by pidl, in bytes.</returns>
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern uint ILGetSize(IntPtr pidl);

        // Retrieves information about an object in the file system,
        // such as a file, a folder, a directory, or a drive root.
        [DllImport("shell32", EntryPoint = "SHGetFileInfo", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SHGetFileInfo(string pszPath, FileAttribute dwFileAttributes, ref ShFileInfo sfi, int cbFileInfo, SHGFI uFlags);

        // Retrieves information about an object in the file system,
        // such as a file, a folder, a directory, or a drive root.
        [DllImport("shell32", EntryPoint = "SHGetFileInfo", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SHGetFileInfo(IntPtr ppidl, FileAttribute dwFileAttributes, ref ShFileInfo sfi, int cbFileInfo, SHGFI uFlags);

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string path, int fileAttributes, ref ShFileInfo psfi, int fileInfo, SHGFI flags);

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string path, FileAttributes fileAttributes, ref ShFileInfo psfi, int fileInfo, SHGFI flags);

        [DllImport("shell32.dll")]
        public static extern IntPtr ExtractAssociatedIcon(HandleRef handle, StringBuilder iconPath, ref int index);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal extern static bool DestroyIcon(IntPtr handle);

        // Takes the CSIDL of a folder and returns the pathname.
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetFolderPath(IntPtr hwndOwner, CSIDL nFolder, IntPtr hToken, SHGFP dwFlags, StringBuilder pszPath);

        // Retrieves the IShellFolder interface for the desktop folder,
        // which is the root of the Shell's namespace. 
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetDesktopFolder([MarshalAs(UnmanagedType.Interface)] out IShellFolder ppshf);

        [DllImport("shell32.dll")]
        public static extern Int32 SHGetDesktopFolder(out IntPtr ppshf);

        // Retrieves ppidl of special folder
        [DllImport("Shell32", EntryPoint = "SHGetSpecialFolderLocation", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern Int32 SHGetSpecialFolderLocation(IntPtr hwndOwner, CSIDL nFolder, out IntPtr ppidl);

        // This function takes the fully-qualified pointer to an item
        // identifier list (PIDL) of a namespace object, and returns a specified
        // interface pointer on the parent object.
        [DllImport("shell32.dll")]
        public static extern Int32 SHBindToParent(IntPtr pidl, ref Guid riid, out IntPtr ppv, out IntPtr ppidlLast);

        // Registers a window that receives notifications from the file system or shell
        [DllImport("shell32.dll", EntryPoint = "#2", CharSet = CharSet.Auto)]
        public static extern uint SHChangeNotifyRegister(IntPtr hwnd, SHCNRF fSources, SHCNE fEvents, WM wMsg, int cEntries, [MarshalAs(UnmanagedType.LPArray)] SHChangeNotifyEntry[] pfsne);

        // Unregisters the client's window process from receiving SHChangeNotify
        [DllImport("shell32.dll", EntryPoint = "#4", CharSet = CharSet.Auto)]
        public static extern bool SHChangeNotifyDeregister(uint hNotify);

        // Converts an item identifier list to a file system path
        [DllImport("shell32.dll")]
        public static extern bool SHGetPathFromIDList(IntPtr pidl, StringBuilder pszPath);

        // SHGetRealIDL converts a simple PIDL to a full PIDL
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetRealIDL(IShellFolder psf, IntPtr pidlSimple, out IntPtr ppidlReal);

        // Tests whether two ITEMIDLIST structures are equal in a binary comparison
        [DllImport("shell32.dll", EntryPoint = "ILIsEqual", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool ILIsEqual(IntPtr pidl1, IntPtr pidl2);
    }

    public static class User32
    {
        // Sends the specified message to a window or windows
        [DllImport("user32.dll", EntryPoint = "SendMessage", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, WM wMsg, int wParam, IntPtr lParam);

        // Destroys an icon and frees any memory the icon occupied
        [DllImport("user32.dll", EntryPoint = "DestroyIcon", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        // Displays a shortcut menu at the specified location and 
        // tracks the selection of items on the shortcut menu
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern uint TrackPopupMenuEx(IntPtr hmenu, TPM flags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        // Creates a popup-menu. The menu is initially empty, but it can be filled with 
        // menu items by using the InsertMenuItem, AppendMenu, and InsertMenu functions
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreatePopupMenu();

        // Destroys the specified menu and frees any memory that the menu occupies
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DestroyMenu(IntPtr hMenu);

        // appends a new item to the end of the specified menu bar, drop-down menu, submenu, 
        // or shortcut menu. You can use this function to specify the content, appearance, and 
        // behavior of the menu item
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool AppendMenu(IntPtr hMenu, MFT uFlags, uint uIDNewItem, [MarshalAs(UnmanagedType.LPTStr)] string lpNewItem);

        // Inserts a new menu item into a menu, moving other items down the menu
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool InsertMenu(IntPtr hmenu, uint uPosition, MFT uflags, uint uIDNewItem, [MarshalAs(UnmanagedType.LPTStr)] string lpNewItem);

        // Inserts a new menu item at the specified position in a menu
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, ref MENUITEMINFO lpmii);

        // Deletes a menu item or detaches a submenu from the specified menu
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, MFT uFlags);

        // Retrieves information about a menu item
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool GetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPos, ref MENUITEMINFO lpmii);

        // Changes information about a menu item.
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPos, ref MENUITEMINFO lpmii);

        // Determines the default menu item on the specified menu
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetMenuDefaultItem(IntPtr hMenu, bool fByPos, uint gmdiFlags);

        // Sets the default menu item for the specified menu
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetMenuDefaultItem(IntPtr hMenu, uint uItem, bool fByPos);

        // Retrieves a handle to the drop-down menu or submenu activated by the specified menu item
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetSubMenu(IntPtr hMenu, int nPos);

        // Retrieves information about the specified combo box
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref COMBOBOXINFO info);
    }

    public static class ComCtl32
    {
        // Replaces an image with an icon or cursor
        [DllImport("comctl32", EntryPoint = "ImageList_ReplaceIcon", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ImageList_ReplaceIcon(IntPtr himl, int index, IntPtr hicon);

        // Adds an image or images to an image list
        [DllImport("comctl32", EntryPoint = "ImageList_Add", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ImageList_Add(IntPtr himl, IntPtr hbmImage, IntPtr hbmMask);

        // Creates an icon from an image and mask in an image list
        [DllImport("comctl32", EntryPoint = "ImageList_GetIcon", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr ImageList_GetIcon(IntPtr himl, int index, ILD flags);
    }

    public static class Ole32
    {
        // Registers the specified window as one that can be the target of an OLE drag-and-drop 
        // operation and specifies the IDropTarget instance to use for drop operations
        //[DllImport("ole32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern int RegisterDragDrop(IntPtr hWnd, IDropTarget IdropTgt);

        // Revokes the registration of the specified application window as a potential target for 
        // OLE drag-and-drop operations
        [DllImport("ole32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int RevokeDragDrop(IntPtr hWnd);

        // This function frees the specified storage medium
        [DllImport("ole32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void ReleaseStgMedium(ref STGMEDIUM pmedium);

        /*// Carries out an OLE drag and drop operation
        [DllImport("ole32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DoDragDrop(IntPtr pDataObject, [MarshalAs(UnmanagedType.Interface)] IDropSource pDropSource, DragDropEffects dwOKEffect, out DragDropEffects pdwEffect);
        */
        // Retrieves a drag/drop helper interface for drawing the drag/drop images
        [DllImport("ole32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CoCreateInstance(ref Guid rclsid, IntPtr pUnkOuter, CLSCTX dwClsContext, ref Guid riid, out IntPtr ppv);

        // Retrieves a data object that you can use to access the contents of the clipboard
        [DllImport("ole32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int OleGetClipboard(out IntPtr ppDataObj);
    }

    public static class ShellApi
    {
        public const int MaxPath = 260;
        public const uint CmdFirst = 1;
        public const uint CmdLast = 30000;


        // Takes a STRRET structure returned by IShellFolder::GetDisplayNameOf,
        // converts it to a string, and places the result in a buffer. 
        [DllImport("shlwapi.dll", EntryPoint = "StrRetToBuf", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Int32 StrRetToBuf(IntPtr pstr, IntPtr pidl, StringBuilder pszBuf, int cchBuf);

        public static DateTime FileTimeToDateTime(FILETIME fileTime)
        {
            long ticks = (((long)fileTime.HighDateTime) << 32) + fileTime.LowDateTime;
            return DateTime.FromFileTimeUtc(ticks);
        }
    }


    /// <summary>
    /// Indicate flags that modify the property store object retrieved by methods 
    /// that create a property store, such as IShellItem2::GetPropertyStore or 
    /// IPropertyStoreFactory::GetPropertyStore.
    /// </summary>
    [Flags]
    internal enum GETPROPERTYSTOREFLAGS : uint
    {
        /// <summary>
        /// Meaning to a calling process: Return a read-only property store that contains all 
        /// properties. Slow items (offline files) are not opened. 
        /// Combination with other flags: Can be overridden by other flags.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Meaning to a calling process: Include only properties directly from the property
        /// handler, which opens the file on the disk, network, or device. Meaning to a file 
        /// folder: Only include properties directly from the handler.
        /// 
        /// Meaning to other folders: When delegating to a file folder, pass this flag on 
        /// to the file folder; do not do any multiplexing (MUX). When not delegating to a 
        /// file folder, ignore this flag instead of returning a failure code.
        /// 
        /// Combination with other flags: Cannot be combined with GPS_TEMPORARY, 
        /// GPS_FASTPROPERTIESONLY, or GPS_BESTEFFORT.
        /// </summary>
        HandlerPropertiesOnly = 0x1,
        /// <summary>
        /// Meaning to a calling process: Can write properties to the item. 
        /// Note: The store may contain fewer properties than a read-only store. 
        /// 
        /// Meaning to a file folder: ReadWrite.
        /// 
        /// Meaning to other folders: ReadWrite. Note: When using default MUX, 
        /// return a single unmultiplexed store because the default MUX does not support ReadWrite.
        /// 
        /// Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_FASTPROPERTIESONLY, 
        /// GPS_BESTEFFORT, or GPS_DELAYCREATION. Implies GPS_HANDLERPROPERTIESONLY.
        /// </summary>
        ReadWrite = 0x2,
        /// <summary>
        /// Meaning to a calling process: Provides a writable store, with no initial properties, 
        /// that exists for the lifetime of the Shell item instance; basically, a property bag 
        /// attached to the item instance. 
        /// 
        /// Meaning to a file folder: Not applicable. Handled by the Shell item.
        /// 
        /// Meaning to other folders: Not applicable. Handled by the Shell item.
        /// 
        /// Combination with other flags: Cannot be combined with any other flag. Implies GPS_READWRITE
        /// </summary>
        Temporary = 0x4,
        /// <summary>
        /// Meaning to a calling process: Provides a store that does not involve reading from the 
        /// disk or network. Note: Some values may be different, or missing, compared to a store 
        /// without this flag. 
        /// 
        /// Meaning to a file folder: Include the "innate" and "fallback" stores only. Do not load the handler.
        /// 
        /// Meaning to other folders: Include only properties that are available in memory or can 
        /// be computed very quickly (no properties from disk, network, or peripheral IO devices). 
        /// This is normally only data sources from the IDLIST. When delegating to other folders, pass this flag on to them.
        /// 
        /// Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_READWRITE, 
        /// GPS_HANDLERPROPERTIESONLY, or GPS_DELAYCREATION.
        /// </summary>
        FastPropertiesOnly = 0x8,
        /// <summary>
        /// Meaning to a calling process: Open a slow item (offline file) if necessary. 
        /// Meaning to a file folder: Retrieve a file from offline storage, if necessary. 
        /// Note: Without this flag, the handler is not created for offline files.
        /// 
        /// Meaning to other folders: Do not return any properties that are very slow.
        /// 
        /// Combination with other flags: Cannot be combined with GPS_TEMPORARY or GPS_FASTPROPERTIESONLY.
        /// </summary>
        OpensLowItem = 0x10,
        /// <summary>
        /// Meaning to a calling process: Delay memory-intensive operations, such as file access, until 
        /// a property is requested that requires such access. 
        /// 
        /// Meaning to a file folder: Do not create the handler until needed; for example, either 
        /// GetCount/GetAt or GetValue, where the innate store does not satisfy the request. 
        /// Note: GetValue might fail due to file access problems.
        /// 
        /// Meaning to other folders: If the folder has memory-intensive properties, such as 
        /// delegating to a file folder or network access, it can optimize performance by 
        /// supporting IDelayedPropertyStoreFactory and splitting up its properties into a 
        /// fast and a slow store. It can then use delayed MUX to recombine them.
        /// 
        /// Combination with other flags: Cannot be combined with GPS_TEMPORARY or 
        /// GPS_READWRITE
        /// </summary>
        DelayCreation = 0x20,
        /// <summary>
        /// Meaning to a calling process: Succeed at getting the store, even if some 
        /// properties are not returned. Note: Some values may be different, or missing,
        /// compared to a store without this flag. 
        /// 
        /// Meaning to a file folder: Succeed and return a store, even if the handler or 
        /// innate store has an error during creation. Only fail if substores fail.
        /// 
        /// Meaning to other folders: Succeed on getting the store, even if some properties 
        /// are not returned.
        /// 
        /// Combination with other flags: Cannot be combined with GPS_TEMPORARY, 
        /// GPS_READWRITE, or GPS_HANDLERPROPERTIESONLY.
        /// </summary>
        BestEffort = 0x40,
        /// <summary>
        /// Mask for valid GETPROPERTYSTOREFLAGS values.
        /// </summary>
        MaskValid = 0xff,
    }

    // Contains strings returned from the IShellFolder interface methods
    [StructLayout(LayoutKind.Explicit)]
    public struct STRRET
    {
        [FieldOffset(0)]
        public uint Type;

        [FieldOffset(4)]
        public IntPtr OleStringPtr;

        [FieldOffset(4)]
        public IntPtr StringPtr;

        [FieldOffset(4)]
        public uint Offset;

        [FieldOffset(4)]
        public IntPtr String;
    }

    // Contains information about a file object
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct ShFileInfo
    {
        public IntPtr IconHandle;

        public int IconIndex;

        public SFGAO Attributes;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string DisplayName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string TypeName;
    }

    // Contains extended information about a shortcut menu command
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct CMINVOKECOMMANDINFOEX
    {
        public int cbSize;
        public CMIC fMask;
        public IntPtr hwnd;
        public IntPtr lpVerb;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpParameters;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpDirectory;

        public SW nShow;
        public int dwHotKey;
        public IntPtr hIcon;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpTitle;

        public IntPtr lpVerbW;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpParametersW;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpDirectoryW;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpTitleW;

        public POINT ptInvoke;
    }

    // Contains information about a menu item
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MENUITEMINFO
    {
        public MENUITEMINFO(string text)
        {
            cbSize = Marshal.SizeOf(typeof(MENUITEMINFO));
            dwTypeData = text;
            cch = text.Length;
            fMask = 0;
            fType = 0;
            fState = 0;
            wID = 0;
            hSubMenu = IntPtr.Zero;
            hbmpChecked = IntPtr.Zero;
            hbmpUnchecked = IntPtr.Zero;
            dwItemData = IntPtr.Zero;
            hbmpItem = IntPtr.Zero;
        }

        public int cbSize;
        public MIIM fMask;
        public MFT fType;
        public MFS fState;
        public uint wID;
        public IntPtr hSubMenu;
        public IntPtr hbmpChecked;
        public IntPtr hbmpUnchecked;
        public IntPtr dwItemData;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string dwTypeData;

        public int cch;
        public IntPtr hbmpItem;
    }

    // Contains extended parameters for the TrackPopupMenuEx function
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct TPMPARAMS
    {
        private int Size;
        private RECT ExcludeRectangle;
    }

    // Contains combo box status information
    [StructLayout(LayoutKind.Sequential)]
    public struct COMBOBOXINFO
    {
        public int Size;
        public RECT ItemRectangle;
        public RECT ButtonRectangle;
        public IntPtr StateButton;
        public IntPtr ComboHwnd;
        public IntPtr EditHwnd;
        public IntPtr ListHwnd;
    }

    // A generalized Clipboard format, it is enhanced to encompass a 
    // target device, the aspect or view of the data, and a storage medium indicator
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct FORMATETC
    {
        public CF cfFormat;
        public IntPtr ptd;
        public DVASPECT dwAspect;
        public int lindex;
        public TYMED Tymd;
    }

    // A generalized global memory handle used for data transfer operations by the 
    // IAdviseSink, IDataObject, and IOleCache interfaces
    [StructLayout(LayoutKind.Sequential)]
    public struct STGMEDIUM
    {
        public TYMED tymed;
        public IntPtr hBitmap;
        public IntPtr hMetaFilePict;
        public IntPtr hEnhMetaFile;
        public IntPtr hGlobal;
        public IntPtr lpszFileName;
        public IntPtr pstm;
        public IntPtr pstg;
        public IntPtr pUnkForRelease;
    }

    // Contains the information needed to create a drag image
    [StructLayout(LayoutKind.Sequential)]
    public struct SHDRAGIMAGE
    {
        public SIZE DragImageSize;
        public POINT Offset;
        public IntPtr DragImagePtr;
        public Color Color;
    }

    // Contains and receives information for change notifications
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHChangeNotifyEntry
    {
        public IntPtr pIdl;
        public bool Recursively;
    }

    // Contains two PIDLs concerning the notify message
    [StructLayout(LayoutKind.Sequential)]
    public struct SHNOTIFYSTRUCT
    {
        public IntPtr dwItem1;
        public IntPtr dwItem2;
    }

    // Contains statistical data about an open storage, stream, or byte-array object
    [StructLayout(LayoutKind.Sequential)]
    public struct STATSTG
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pwcsName;

        public STGTY Type;
        public long Size;
        public FILETIME mtime;
        public FILETIME ctime;
        public FILETIME atime;
        public STGM grfMode;
        public LOCKTYPE grfLocksSupported;
        public Guid clsid;
        public int grfStateBits;
        public int reserved;
    }

    // Represents the number of 100-nanosecond intervals since January 1, 1601
    [StructLayout(LayoutKind.Sequential)]
    public struct FILETIME
    {
        public int LowDateTime;

        public int HighDateTime;
    }

    // Defines the x- and y-coordinates of a point
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct POINT
    {
        public POINT(Point point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }


        public int X;

        public int Y;
    }

    // Defines the coordinates of the upper-left and lower-right corners of a rectangle
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct RECT
    {
        public RECT(Rectangle rect)
        {
            Left = rect.Left;
            Top = rect.Top;
            Right = rect.Right;
            Bottom = rect.Bottom;
        }


        public int Left;

        public int Top;

        public int Right;

        public int Bottom;
    }

    // The SIZE structure specifies the width and height of a rectangle
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SIZE
    {
        public SIZE(Size size)
        {
            this.Width = size.Width;
            this.Height = size.Height;
        }

        public SIZE(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }


        public int Width;

        public int Height;
    }

    public enum CSIDL
    {
        AdminTools = 0x30,
        AltStartup = 0x1d,
        AppData = 0x1a,
        BitBucket = 10,
        CdBurnArea = 0x3b,
        CommonAdminTools = 0x2f,
        CommonAltStartup = 30,
        CommonAppData = 0x23,
        CommonDesktopDirectory = 0x19,
        CommonDocuments = 0x2e,
        CommonFavorites = 0x1f,
        CommonMusic = 0x35,
        CommonPictures = 0x36,
        CommonPrograms = 0x17,
        CommonStartMenu = 0x16,
        CommonStartup = 0x18,
        CommonTemplates = 0x2d,
        CommonVideo = 0x37,
        Controls = 3,
        Cookies = 0x21,
        Desktop = 0,
        DesktopDirectory = 0x10,
        Drives = 0x11,
        Favorites = 6,
        FlagCreate = 0x8000,
        Fonts = 20,
        History = 0x22,
        Internet = 1,
        InternetCache = 0x20,
        LocalAppData = 0x1c,
        MyDocuments = 12,
        MyMusic = 13,
        MyPictures = 0x27,
        MyVideo = 14,
        NetHood = 0x13,
        NetWork = 0x12,
        Personal = 5,
        Printers = 4,
        PrintHood = 0x1b,
        Profile = 40,
        Profiles = 0x3e,
        ProgramFiles = 0x26,
        ProgramFilesCommon = 0x2b,
        Programs = 2,
        Recent = 8,
        SendTo = 9,
        StartMenu = 11,
        Startup = 7,
        System = 0x25,
        Templates = 0x15,
        Windows = 0x24
    }

    // Defines the values used with the IShellFolder::GetDisplayNameOf and IShellFolder::SetNameOf 
    // methods to specify the type of file or folder names used by those methods
    [Flags]
    public enum SHGNO
    {
        Normal = 0x0000,
        InFolder = 0x0001,
        ForEditing = 0x1000,
        ForAddressBar = 0x4000,
        ForParsing = 0x8000
    }

    [Flags]
    public enum SIGDN : uint
    {
        NormalDisplay = 0,
        ParentRelativeParsing = 0x80018001,
        ParentRelativeForAddressBar = 0x8001c001,
        DesktopAbsoluteParsing = 0x80028000,
        ParentRelativeEditing = 0x80031001,
        DesktopAbsoluteEditing = 0x8004c000,
        FileSysPath = 0x80058000,
        Url = 0x80068000
    }

    // Flags to specify which path is to be returned with SHGetFolderPath
    [Flags]
    public enum SHGFP
    {
        TYPE_CURRENT = 0,
        TYPE_DEFAULT = 1
    }

    // The attributes that the caller is requesting, when calling IShellFolder::GetAttributesOf
    [Flags]
    public enum SFGAO : uint
    {
        Browsable = 0x8000000,
        CanCopy = 1,
        CanDelete = 0x20,
        CanLink = 4,
        CanMoniker = 0x400000,
        CanMove = 2,
        CanRename = 0x10,
        CapabilityMask = 0x177,
        Compressed = 0x4000000,
        ContentsMask = 0x80000000,
        DisplayAttrMask = 0xfc000,
        DropTarget = 0x100,
        Encrypted = 0x2000,
        FileSysAncestor = 0x10000000,
        FileSystem = 0x40000000,
        Folder = 0x20000000,
        Ghosted = 0x8000,
        HasPropSheet = 0x40,
        HasStorage = 0x400000,
        HasSubFolder = 0x80000000,
        Hidden = 0x80000,
        IsSlow = 0x4000,
        Link = 0x10000,
        NewContent = 0x200000,
        NonEnumerated = 0x100000,
        Readonly = 0x40000,
        Removable = 0x2000000,
        Share = 0x20000,
        Storage = 8,
        StorageAncestor = 0x800000,
        StorageCapMask = 0x70c50008,
        Stream = 0x400000,
        Validate = 0x1000000
    }

    // Determines the type of items included in an enumeration. 
    // These values are used with the IShellFolder::EnumObjects method
    [Flags]
    public enum SHCONT
    {
        Folders = 0x0020,
        NonFolders = 0x0040,
        IncludeHidden = 0x0080,
        InitOnFirstNext = 0x0100,
        NetPrintersrch = 0x0200,
        Shareable = 0x0400,
        Storage = 0x0800,
    }

    // Specifies how the shortcut menu can be changed when calling IContextMenu::QueryContextMenu
    [Flags]
    public enum CMF : uint
    {
        Normal = 0x00000000,
        DefaultOnly = 0x00000001,
        VerbsOnly = 0x00000002,
        Explore = 0x00000004,
        NoVerbs = 0x00000008,
        CanRename = 0x00000010,
        NodeFault = 0x00000020,
        IncludeStatic = 0x00000040,
        ExtendedVerbs = 0x00000100,
        Reserved = 0xffff0000
    }

    // Flags specifying the information to return when calling IContextMenu::GetCommandString
    [Flags]
    public enum GCS : uint
    {
        VerbA = 0,
        HelpTextA = 1,
        ValidateA = 2,
        VerbW = 4,
        HelpTextW = 5,
        ValidateW = 6
    }

    // Flags that specify the file information to retrieve with SHGetFileInfo
    [Flags]
    public enum SHGFI : uint
    {
        /// <summary>
        /// Get icon
        /// </summary>
        Icon = 0x000000100,
        /// <summary>
        /// Get display name
        /// </summary>
        DisplayName = 0x000000200,
        /// <summary>
        /// Get type name
        /// </summary>
        TypeName = 0x000000400,
        /// <summary>
        /// Get attributes
        /// </summary>
        Attributes = 0x000000800,
        /// <summary>
        /// Get icon location
        /// </summary>
        IconLocation = 0x000001000,
        /// <summary>
        /// Return exe type
        /// </summary>
        ExeType = 0x000002000,
        /// <summary>
        /// Get system icon index
        /// </summary>
        SysIconIndex = 0x000004000,
        /// <summary>
        /// Put a link overlay on icon
        /// </summary>
        LinkOverlay = 0x000008000,
        /// <summary>
        /// Show icon in selected state
        /// </summary>
        Selected = 0x000010000,
        /// <summary>
        /// Get only specified attributes
        /// </summary>
        AttrSpecified = 0x000020000,
        /// <summary>
        /// Get large icon
        /// </summary>
        LargeIcon = 0x000000000,
        /// <summary>
        /// Get small icon
        /// </summary>
        SmallIcon = 0x000000001,
        /// <summary>
        /// Get open icon
        /// </summary>
        OpenIcon = 0x000000002,
        /// <summary>
        /// Get shell size icon
        /// </summary>
        ShellIconSize = 0x000000004,
        /// <summary>
        /// Path is a pidl
        /// </summary>
        Pidl = 0x000000008,
        /// <summary>
        /// Use passed dwFileAttribute
        /// </summary>
        UseFileAttributes = 0x000000010,
        /// <summary>
        /// Apply the appropriate overlays
        /// </summary>
        AddOverlays = 0x000000020,
        /// <summary>
        /// Get the index of the overlay in the upper 8 bits of the iIcon
        /// </summary>
        OverlayIndex = 0x000000040,
    }

    // Flags that specify the file information to retrieve with SHGetFileInfo
    [Flags]
    public enum FileAttribute
    {
        ReadOnly = 0x00000001,
        Hidden = 0x00000002,
        System = 0x00000004,
        Directory = 0x00000010,
        Archive = 0x00000020,
        Device = 0x00000040,
        Normal = 0x00000080,
        Temporary = 0x00000100,
        SParseFile = 0x00000200,
        ReparsePoint = 0x00000400,
        Compressed = 0x00000800,
        Offline = 0x00001000,
        NotContentIndexed = 0x00002000,
        Encrypted = 0x00004000
    }

    // Specifies how TrackPopupMenuEx positions the shortcut menu horizontally
    [Flags]
    public enum TPM : uint
    {
        LeftButton = 0x0000,
        RightButton = 0x0002,
        LeftAlign = 0x0000,
        CenterAlign = 0x0004,
        RightAlign = 0x0008,
        TopAlign = 0x0000,
        VCenterAlign = 0x0010,
        BottomAlign = 0x0020,
        Horizontal = 0x0000,
        Vertical = 0x0040,
        NoNotify = 0x0080,
        ReturnCmd = 0x0100,
        Recurse = 0x0001,
        HorPosAnimation = 0x0400,
        HorNegAnimation = 0x0800,
        VerPosAnimation = 0x1000,
        VerNegAnimation = 0x2000,
        NoAnimation = 0x4000,
        LayoutRtl = 0x8000
    }

    // Flags used with the CMINVOKECOMMANDINFOEX structure
    [Flags]
    public enum CMIC : uint
    {
        Hotkey = 0x00000020,
        Icon = 0x00000010,
        FlagNoUi = 0x00000400,
        Unicode = 0x00004000,
        NoConsole = 0x00008000,
        Asyncok = 0x00100000,
        NoZoneChecks = 0x00800000,
        ShiftDown = 0x10000000,
        ControlDown = 0x40000000,
        FlagLogUsage = 0x04000000,
        PtInvoke = 0x20000000
    }

    // Flags that specify the drawing style when calling ImageList_GetIcon
    [Flags]
    public enum ILD : uint
    {
        Normal = 0x0000,
        Transparent = 0x0001,
        Mask = 0x0010,
        Blend25 = 0x0002,
        Blend50 = 0x0004
    }

    // Specifies how the window is to be shown
    [Flags]
    public enum SW
    {
        Hide = 0,
        ShowNormal = 1,
        Normal = 1,
        ShowMinimized = 2,
        ShowMaximized = 3,
        Maximize = 3,
        ShowNoActivate = 4,
        Show = 5,
        Minimize = 6,
        ShowMinNoActive = 7,
        ShowNa = 8,
        Restore = 9,
        ShowDefault = 10,
    }

    // Window message flags
    [Flags]
    public enum WM : uint
    {
        Activate = 0x6,
        ActivateApp = 0x1C,
        AfxFirst = 0x360,
        AfxLast = 0x37F,
        App = 0x8000,
        AskCbFormatName = 0x30C,
        CancelJournal = 0x4B,
        CancelMode = 0x1F,
        CaptureChanged = 0x215,
        ChangeCbChain = 0x30D,
        Char = 0x102,
        CharToItem = 0x2F,
        ChildActivate = 0x22,
        Clear = 0x303,
        Close = 0x10,
        Command = 0x111,
        Compacting = 0x41,
        CompareItem = 0x39,
        ContextMenu = 0x7B,
        Copy = 0x301,
        CopyData = 0x4A,
        Create = 0x1,
        CtlColorBtn = 0x135,
        CtlColorDlg = 0x136,
        CtlColorEdit = 0x133,
        CtlColorListBox = 0x134,
        CtlColorMsgBox = 0x132,
        CtlColorScrollBar = 0x137,
        CtlColorStatic = 0x138,
        Cut = 0x300,
        DeadChar = 0x103,
        DeleteItem = 0x2D,
        Destroy = 0x2,
        DestroyClipboard = 0x307,
        DeviceChange = 0x219,
        DevModeChange = 0x1B,
        DisplayChange = 0x7E,
        DrawClipboard = 0x308,
        DrawItem = 0x2B,
        DropFiles = 0x233,
        Enable = 0xA,
        EndSession = 0x16,
        EnterIdle = 0x121,
        EnterMenuLoop = 0x211,
        EnterSizeMove = 0x231,
        EraseBkgnd = 0x14,
        ExitMenuLoop = 0x212,
        ExitSizeMove = 0x232,
        FontChange = 0x1D,
        GetDlgCode = 0x87,
        GetFont = 0x31,
        GetHotKey = 0x33,
        GetIcon = 0x7F,
        GetMinMaxInfo = 0x24,
        GetObject = 0x3D,
        GetSysMenu = 0x313,
        GetText = 0xD,
        GetTextLength = 0xE,
        HandHeldFirst = 0x358,
        HandHeldLast = 0x35F,
        Help = 0x53,
        Hotkey = 0x312,
        HScroll = 0x114,
        HScrollClipboard = 0x30E,
        IconEraseBkgnd = 0x27,
        ImeChar = 0x286,
        ImeComposition = 0x10F,
        ImeCompositionFull = 0x284,
        ImeControl = 0x283,
        ImeEndComposition = 0x10E,
        ImeKeyDown = 0x290,
        ImeKeyLast = 0x10F,
        ImeKeyUp = 0x291,
        ImeNotify = 0x282,
        ImeRequest = 0x288,
        ImeSelect = 0x285,
        ImeSetContext = 0x281,
        ImeStartComposition = 0x10D,
        InitDialog = 0x110,
        InitMenu = 0x116,
        InitMenuPopup = 0x117,
        InputLangChange = 0x51,
        InputLangChangeRequest = 0x50,
        KeyDown = 0x100,
        KeyFirst = 0x100,
        KeyLast = 0x108,
        KeyUp = 0x101,
        KillFocus = 0x8,
        LButtonDblclk = 0x203,
        LButtonDown = 0x201,
        LButtonUp = 0x202,
        LvmGetEditControl = 0x1018,
        LvmSetImageList = 0x1003,
        MButtonDblclk = 0x209,
        MButtonDown = 0x207,
        MButtonUp = 0x208,
        MdiActivate = 0x222,
        MdiCascade = 0x227,
        MdiCreate = 0x220,
        MdiDestroy = 0x221,
        MdiGetActive = 0x229,
        MdiIconArrange = 0x228,
        MdiMaximize = 0x225,
        MdiNext = 0x224,
        MdiRefreshMenu = 0x234,
        MdiRestore = 0x223,
        MdiSetMenu = 0x230,
        MdiTile = 0x226,
        MeasureItem = 0x2C,
        MenuChar = 0x120,
        MenuCommand = 0x126,
        MenuDrag = 0x123,
        MenuGetObject = 0x124,
        MenuRButtonUp = 0x122,
        MenuSelect = 0x11F,
        MouseActivate = 0x21,
        MouseFirst = 0x200,
        MouseHover = 0x2A1,
        MouseLast = 0x20A,
        MouseLeave = 0x2A3,
        MouseMove = 0x200,
        MouseWheel = 0x20A,
        Move = 0x3,
        Moving = 0x216,
        NcActivate = 0x86,
        NcCalcSize = 0x83,
        NcCreate = 0x81,
        NcDestroy = 0x82,
        NcHittest = 0x84,
        NcLButtonDblclk = 0xA3,
        NcLButtonDown = 0xA1,
        NcLButtonUp = 0xA2,
        NcMButtonDblclk = 0xA9,
        NcMButtonDown = 0xA7,
        NcMButtonUp = 0xA8,
        NcMouseHover = 0x2A0,
        NcMouseLeave = 0x2A2,
        NcMouseMove = 0xA0,
        NcPaint = 0x85,
        NcRButtonDblclk = 0xA6,
        NcRButtonDown = 0xA4,
        NcRButtonUp = 0xA5,
        NextDlgctl = 0x28,
        NextMenu = 0x213,
        Notify = 0x4E,
        NotifyFormat = 0x55,
        Null = 0x0,
        Paint = 0xF,
        PaintClipboard = 0x309,
        PaintIcon = 0x26,
        PaletteChanged = 0x311,
        PaletteIsChanging = 0x310,
        ParentNotify = 0x210,
        Paste = 0x302,
        PenWinFirst = 0x380,
        PenWinLast = 0x38F,
        Power = 0x48,
        Print = 0x317,
        PrintClient = 0x318,
        QueryDragIcon = 0x37,
        QueryEndSession = 0x11,
        QueryNewPalette = 0x30F,
        QueryOpen = 0x13,
        QueueSync = 0x23,
        Quit = 0x12,
        RButtonDblclk = 0x206,
        RButtonDown = 0x204,
        RButtonUp = 0x205,
        RenderAllFormats = 0x306,
        RenderFormat = 0x305,
        SetCursor = 0x20,
        SetFocus = 0x7,
        SetFont = 0x30,
        SetHotKey = 0x32,
        SetIcon = 0x80,
        SetMargins = 0xD3,
        SetRedraw = 0xB,
        SetText = 0xC,
        SetTingChange = 0x1A,
        ShowWindow = 0x18,
        Size = 0x5,
        SizeClipboard = 0x30B,
        Sizing = 0x214,
        SpoolerStatus = 0x2A,
        StyleChanged = 0x7D,
        StyleChanging = 0x7C,
        SyncPaint = 0x88,
        SysChar = 0x106,
        SysColorChange = 0x15,
        SysCommand = 0x112,
        SysDeadChar = 0x107,
        SysKeyDown = 0x104,
        SysKeyUp = 0x105,
        TCard = 0x52,
        TimeChange = 0x1E,
        Timer = 0x113,
        TvmGetEditControl = 0x110F,
        TvmSetImageList = 0x1109,
        Undo = 0x304,
        UnInitMenuPopup = 0x125,
        User = 0x400,
        UserChanged = 0x54,
        VKeyToItem = 0x2E,
        VScroll = 0x115,
        VScrollClipboard = 0x30A,
        WindowPosChanged = 0x47,
        WindowPosChanging = 0x46,
        WinIniChange = 0x1A,
        ShNotify = 0x0401
    }

    // Specifies the content of the new menu item
    [Flags]
    public enum MFT : uint
    {
        GRAYED = 0x00000003,
        DISABLED = 0x00000003,
        CHECKED = 0x00000008,
        SEPARATOR = 0x00000800,
        RADIOCHECK = 0x00000200,
        BITMAP = 0x00000004,
        OWNERDRAW = 0x00000100,
        MENUBARBREAK = 0x00000020,
        MENUBREAK = 0x00000040,
        RIGHTORDER = 0x00002000,
        BYCOMMAND = 0x00000000,
        BYPOSITION = 0x00000400,
        POPUP = 0x00000010
    }

    // Specifies the state of the new menu item
    [Flags]
    public enum MFS : uint
    {
        GRAYED = 0x00000003,
        DISABLED = 0x00000003,
        CHECKED = 0x00000008,
        HILITE = 0x00000080,
        ENABLED = 0x00000000,
        UNCHECKED = 0x00000000,
        UNHILITE = 0x00000000,
        DEFAULT = 0x00001000
    }

    // Specifies the content of the new menu item
    [Flags]
    public enum MIIM : uint
    {
        BITMAP = 0x80,
        CHECKMARKS = 0x08,
        DATA = 0x20,
        FTYPE = 0x100,
        ID = 0x02,
        STATE = 0x01,
        STRING = 0x40,
        SUBMENU = 0x04,
        TYPE = 0x10
    }

    // Particular clipboard format of interest. 
    // There are three types of formats recognized by OLE
    public enum CF
    {
        BITMAP = 2,
        DIB = 8,
        DIF = 5,
        DSPBITMAP = 130,
        DSPENHMETAFILE = 0x8e,
        DSPMETAFILEPICT = 0x83,
        DSPTEXT = 0x81,
        ENHMETAFILE = 14,
        GDIOBJFIRST = 0x300,
        GDIOBJLAST = 0x3ff,
        HDROP = 15,
        LOCALE = 0x10,
        MAX = 0x11,
        METAFILEPICT = 3,
        OEMTEXT = 7,
        OWNERDISPLAY = 0x80,
        PALETTE = 9,
        PENDATA = 10,
        PRIVATEFIRST = 0x200,
        PRIVATELAST = 0x2ff,
        RIFF = 11,
        SYLK = 4,
        TEXT = 1,
        TIFF = 6,
        UNICODETEXT = 13,
        WAVE = 12
    }

    // Specifies the desired data or view aspect of the object when drawing or getting data
    [Flags]
    public enum DVASPECT
    {
        CONTENT = 1,
        DOCPRINT = 8,
        ICON = 4,
        THUMBNAIL = 2
    }

    // Indicates the type of storage medium being used in a data transfer
    [Flags]
    public enum TYMED
    {
        ENHMF = 0x40,
        FILE = 2,
        GDI = 0x10,
        HGLOBAL = 1,
        ISTORAGE = 8,
        ISTREAM = 4,
        MFPICT = 0x20,
        NULL = 0
    }

    // Specifies a group of flags for controlling the advisory connection
    [Flags]
    public enum ADVF
    {
        CACHE_FORCEBUILTIN = 0x10,
        CACHE_NOHANDLER = 8,
        CACHE_ONSAVE = 0x20,
        DATAONSTOP = 0x40,
        NODATA = 1,
        ONLYONCE = 4,
        PRIMEFIRST = 2
    }

    // Flags indicating which mouse buttons are clicked and which modifier keys are pressed
    [Flags]
    public enum MK
    {
        LBUTTON = 0x0001,
        RBUTTON = 0x0002,
        SHIFT = 0x0004,
        CONTROL = 0x0008,
        MBUTTON = 0x0010,
        ALT = 0x0020
    }

    // Are used in activation calls to indicate the execution contexts in which an object is to be run
    [Flags]
    public enum CLSCTX : uint
    {
        INPROC_SERVER = 0x1,
        INPROC_HANDLER = 0x2,
        LOCAL_SERVER = 0x4,
        INPROC_SERVER16 = 0x8,
        REMOTE_SERVER = 0x10,
        INPROC_HANDLER16 = 0x20,
        RESERVED1 = 0x40,
        RESERVED2 = 0x80,
        RESERVED3 = 0x100,
        RESERVED4 = 0x200,
        NO_CODE_DOWNLOAD = 0x400,
        RESERVED5 = 0x800,
        NO_CUSTOM_MARSHAL = 0x1000,
        ENABLE_CODE_DOWNLOAD = 0x2000,
        NO_FAILURE_LOG = 0x4000,
        DISABLE_AAA = 0x8000,
        ENABLE_AAA = 0x10000,
        FROM_DEFAULT_CONTEXT = 0x20000,
        INPROC = INPROC_SERVER | INPROC_HANDLER,
        SERVER = INPROC_SERVER | LOCAL_SERVER | REMOTE_SERVER,
        ALL = SERVER | INPROC_HANDLER
    }

    // Describes the event that has occurred
    [Flags]
    public enum SHCNE : uint
    {
        RENAMEITEM = 0x00000001,
        CREATE = 0x00000002,
        DELETE = 0x00000004,
        MKDIR = 0x00000008,
        RMDIR = 0x00000010,
        MEDIAINSERTED = 0x00000020,
        MEDIAREMOVED = 0x00000040,
        DRIVEREMOVED = 0x00000080,
        DRIVEADD = 0x00000100,
        NETSHARE = 0x00000200,
        NETUNSHARE = 0x00000400,
        ATTRIBUTES = 0x00000800,
        UPDATEDIR = 0x00001000,
        UPDATEITEM = 0x00002000,
        SERVERDISCONNECT = 0x00004000,
        UPDATEIMAGE = 0x00008000,
        DRIVEADDGUI = 0x00010000,
        RENAMEFOLDER = 0x00020000,
        FREESPACE = 0x00040000,
        EXTENDED_EVENT = 0x04000000,
        ASSOCCHANGED = 0x08000000,
        DISKEVENTS = 0x0002381F,
        GLOBALEVENTS = 0x0C0581E0,
        ALLEVENTS = 0x7FFFFFFF,
        INTERRUPT = 0x80000000
    }

    // Flags that indicate the meaning of the dwItem1 and dwItem2 parameters
    [Flags]
    public enum SHCNF
    {
        IDLIST = 0x0000,
        PATHA = 0x0001,
        PRINTERA = 0x0002,
        DWORD = 0x0003,
        PATHW = 0x0005,
        PRINTERW = 0x0006,
        TYPE = 0x00FF,
        FLUSH = 0x1000,
        FLUSHNOWAIT = 0x2000
    }

    // Indicate the type of events for which to receive notifications
    [Flags]
    public enum SHCNRF
    {
        InterruptLevel = 0x0001,
        ShellLevel = 0x0002,
        RecursiveInterrupt = 0x1000,
        NewDelivery = 0x8000
    }

    // Indicate whether the method should try to return a name in the pwcsName member of the STATSTG structure
    [Flags]
    public enum STATFLAG
    {
        Default = 0,
        NoName = 1,
        NoOpen = 2
    }

    // Indicate the type of locking requested for the specified range of bytes
    [Flags]
    public enum LOCKTYPE
    {
        Write = 1,
        Exclusive = 2,
        OnlyOnce = 4
    }

    // Used in the type member of the STATSTG structure to indicate the type of the storage element
    public enum STGTY
    {
        Storage = 1,
        Stream = 2,
        LockBytes = 3,
        Property = 4
    }

    // Indicate conditions for creating and deleting the object and access modes for the object
    [Flags]
    public enum STGM
    {
        Direct = 0x00000000,
        Transacted = 0x00010000,
        Simple = 0x08000000,
        Read = 0x00000000,
        Write = 0x00000001,
        ReadWrite = 0x00000002,
        ShareDenyNone = 0x00000040,
        ShareDenyRead = 0x00000030,
        ShareDenyWrite = 0x00000020,
        ShareExclusive = 0x00000010,
        Priority = 0x00040000,
        DeleteOnRelease = 0x04000000,
        Noscratch = 0x00100000,
        Create = 0x00001000,
        Convert = 0x00020000,
        FailIfThere = 0x00000000,
        NoSnapsHot = 0x00200000,
        DirectSwmr = 0x00400000,
    }

    // Indicate whether a storage element is to be moved or copied
    public enum STGMOVE
    {
        Move = 0,
        Copy = 1,
        ShallowCopy = 2
    }

    // Specify the conditions for performing the commit operation in the IStorage::Commit and IStream::Commit methods
    [Flags]
    public enum STGC
    {
        Default = 0,
        /// <summary>
        /// The commit operation can overwrite existing data to reduce overall space requirements. This value is not recommended for typical usage because it is not as robust as the default value. In this case, it is possible for the commit operation to fail after the old data is overwritten, but before the new data is completely committed. Then, neither the old version nor the new version of the storage object will be intact.
        /// You can use this value in the following cases:
        ///    - The user is willing to risk losing the data.
        ///    - The low-memory save sequence will be used to safely save the storage object to a smaller file.
        ///    - A previous commit returned STG_E_MEDIUMFULL, but overwriting the existing data would provide enough space to commit changes to the storage object.
        /// Be aware that the commit operation verifies that adequate space exists before any overwriting occurs. Thus, even with this value specified, if the commit operation fails due to space requirements, the old data is safe. It is possible, however, for data loss to occur with the STGC_OVERWRITE value specified if the commit operation fails for any reason other than lack of disk space.
        /// </summary>
        Overwrite = 1,
        /// <summary>
        /// Prevents multiple users of a storage object from overwriting each other's changes. The commit operation occurs only if there have been no changes to the saved storage object because the user most recently opened it. Thus, the saved version of the storage object is the same version that the user has been editing. If other users have changed the storage object, the commit operation fails and returns the STG_E_NOTCURRENT value. To override this behavior, call the IStorage::Commit or IStream::Commit method again using the STGC_DEFAULT value.
        /// </summary>
        OnlyIfCurrent = 2,
        /// <summary>
        /// Commits the changes to a write-behind disk cache, but does not save the cache to the disk. In a write-behind disk cache, the operation that writes to disk actually writes to a disk cache, thus increasing performance. The cache is eventually written to the disk, but usually not until after the write operation has already returned. The performance increase comes at the expense of an increased risk of losing data if a problem occurs before the cache is saved and the data in the cache is lost. 
        /// </summary>
        DangerouslyCommitMerelyToDiskCache = 4,
        /// <summary>
        /// Indicates that a storage should be consolidated after it is committed, resulting in a smaller file on disk. This flag is valid only on the outermost storage object that has been opened in transacted mode. It is not valid for streams. The STGC_CONSOLIDATE flag can be combined with any other STGC flags.
        /// </summary>
        Consolidate = 8
    }

    // Directing the handling of the item from which you're retrieving the info tip text
    [Flags]
    public enum QITIPF
    {
        Default = 0x00000,
        UseName = 0x00001,
        LinkNoTarget = 0x00002,
        LinkUseTarget = 0x00004,
        UsesLowTip = 0x00008
    }
}