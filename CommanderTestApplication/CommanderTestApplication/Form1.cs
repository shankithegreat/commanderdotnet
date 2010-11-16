using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CommanderTestApplication
{
    public partial class Form1 : Form
    {
        private const int TVSIL_NORMAL = 0;
        private const int TLVSIL_SMALL = 1;
        private const int TLVSIL_STATE = 2;


        public Form1()
        {
            InitializeComponent();
            //
            var fileInfo2 = new ShFileInfo();
            var largeImageList = Shell32.SHGetFileInfo(".txt", FileAttributes.Normal, ref fileInfo2, Marshal.SizeOf(fileInfo2), SHGFI.UseFileAttributes | SHGFI.SysIconIndex | SHGFI.LargeIcon | SHGFI.OverlayIndex | SHGFI.AddOverlays);

            var fileInfo3 = new ShFileInfo();
            var smallImageList = Shell32.SHGetFileInfo(".txt", FileAttributes.Normal, ref fileInfo3, Marshal.SizeOf(fileInfo3), SHGFI.UseFileAttributes | SHGFI.SysIconIndex | SHGFI.OverlayIndex | SHGFI.SmallIcon | SHGFI.AddOverlays);



            //Guid iidImageList = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
            //IntPtr h = IntPtr.Zero;
            //Shell32.SHGetImageList((int)SysImageListSize.extraLargeIcons, ref iidImageList, ref h);

            //SysImageList list = new SysImageList(SysImageListSize.extraLargeIcons);

            //User32.SendMessage(listView1.Handle, WM.LvmSetImageList, TVSIL_NORMAL, list.Handle);
            User32.SendMessage(listView1.Handle, WM.LvmSetImageList, TLVSIL_SMALL, smallImageList);
            User32.SendMessage(listView1.Handle, WM.LvmSetImageList, TVSIL_NORMAL, largeImageList);
            //User32.SendMessage(listView1.Handle, WM.LvmSetImageList, 3, smallImageList);
            //
            //this.listView1.LargeImageList = this.imageList2;
            //this.listView1.SmallImageList = this.imageList1;


            var fileInfo = new ShFileInfo();

            Shell32.SHGetFileInfo(@"D:\Projects\Commander.Net\", 0, ref fileInfo, Marshal.SizeOf(fileInfo), SHGFI.Icon | SHGFI.OverlayIndex | SHGFI.LargeIcon);

            var fileInfo0 = new ShFileInfo();
            Shell32.SHGetFileInfo(@"D:\Projects\Commander.Net\Microsoft.WindowsAPICodePack.ShellExtensions.dll", 0, ref fileInfo0, Marshal.SizeOf(fileInfo0), SHGFI.Icon | SHGFI.LargeIcon | SHGFI.AddOverlays);
            Image image = ImageHelper.IconToBitmap(fileInfo0.IconHandle);
            pictureBox1.Image = ImageHelper.IconToBitmap(fileInfo0.IconHandle);

            Shell32.DestroyIcon(fileInfo.IconHandle);
            Shell32.DestroyIcon(fileInfo0.IconHandle);

            fileInfo = new ShFileInfo();
            Shell32.SHGetFileInfo(@"D:\Projects\Commander.Net\Microsoft.WindowsAPICodePack.ShellExtensions.dll", 0, ref fileInfo, Marshal.SizeOf(fileInfo), SHGFI.Icon | SHGFI.OverlayIndex | SHGFI.LargeIcon);


            //int i = Shell32.SHGetIconOverlayIndex(@"D:\Projects\Commander.Net", fileInfo.IconIndex);


            //var ico = list.Icon(5);
            //if (ico != null)
            //{
            //    pictureBox1.Image = ImageHelper.IconToBitmap(ico, ico.Size);
            //}

            //listView1.Items.Add("Commander.Net", fileInfo.IconIndex);

            listView1.Items.Add("Commander.Net", fileInfo.IconIndex);
            listView1.Items.Add("Commander.Net", fileInfo.IconIndex + 1);


            ////---
            //fileInfo = new ShFileInfo();

            //Shell32.SHGetFileInfo(@"D:\Projects\ConsoleApplication1", 0, ref fileInfo, Marshal.SizeOf(fileInfo), SHGFI.ShellIconSize | SHGFI.Icon | SHGFI.OverlayIndex | SHGFI.LargeIcon | SHGFI.AddOverlays);

            ////imageList2.Images.Add(Icon.FromHandle(fileInfo.IconHandle));

            //listView1.Items.Add("ConsoleApplication1", fileInfo.IconIndex);


            SysImageList list = new SysImageList(SysImageListSize.largeIcons);

            var ico = list.Icon(fileInfo.IconIndex >> 24);
            //pictureBox1.Image = ImageHelper.IconToBitmap(ico, ico.Size);

            ThreadPool.QueueUserWorkItem(delegate
                                             {
                                                 try
                                                 {


                                                     for (int i = 0; i < 30; i++)
                                                     {
                                                         Thread.Sleep(500);
                                                         this.Invoke((MethodInvoker)delegate
                                                                                         {
                                                                                             var ico2 = list.Icon(i);
                                                                                             if (ico2 != null)
                                                                                             {
                                                                                                 pictureBox1.Image = ImageHelper.IconToBitmap(ico2, ico2.Size);
                                                                                             }
                                                                                         });
                                                     }

                                                 }
                                                 catch (Exception)
                                                 {
                                                 }

                                             });
        }
    }


    public static class Shell32
    {
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string path, FileAttributes fileAttributes, ref ShFileInfo psfi, int fileInfo, SHGFI flags);

        [DllImport("shell32.dll")]
        public static extern int SHGetIconOverlayIndex(string pszIconPath, int iIconIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static bool DestroyIcon(IntPtr handle);

        [DllImport("shell32.dll")]
        public static extern int SHGetImageList(int iImageList, ref Guid riid, ref IntPtr ppv);

        [DllImport("shell32.dll", EntryPoint = "#727")]
        public static extern int SHGetImageList(int iImageList, ref Guid riid, ref IImageList ppv);

        [DllImport("shell32.dll", EntryPoint = "#727")]
        public static extern int SHGetImageListHandle(int iImageList, ref Guid riid, ref IntPtr handle);
        //[DllImport("shell32.dll", EntryPoint = "#727")]
        //public static extern int SHGetImageListHandle(int iImageList, ref Guid riid, ref IntPtr handle);



    }





    public static class User32
    {
        // Sends the specified message to a window or windows
        [DllImport("user32.dll", EntryPoint = "SendMessage", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, WM wMsg, int wParam, IntPtr lParam);
    }

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

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        int x;
        int y;
    }


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

}
