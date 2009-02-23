using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Commander
{
    internal class SafeNativeMethods
    {
        internal static HandleRef NullHandleRef;

        internal static int SHGFI_ICON = 0x000000100;
        internal static int SHGFI_LARGEICON = 0x000000000;     // get large icon
        internal static int SHGFI_SMALLICON = 0x000000001;    // get small icon
        internal static int SHGFI_SYSICONINDEX = 0x000004000;
        internal static int SHGFI_TYPENAME = 0x000000400;

        [DllImport("shell32.dll", EntryPoint = "ExtractAssociatedIcon", CharSet = CharSet.Auto)]
        private static extern IntPtr IntExtractAssociatedIcon(HandleRef hInst, StringBuilder iconPath, ref int index);

        [DllImport("shfolder.dll", CharSet = CharSet.Auto)]
        internal static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public int dwAttributes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public Char[] szDisplayName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            public Char[] szTypeName;
        }


        [DllImport("shell32.dll", EntryPoint = "SHGetFileInfo", CharSet = CharSet.Auto)]
        internal static extern IntPtr SHGetFileInfo(StringBuilder path, int fileAttributes, ref SHFILEINFO psfi, int fileInfo, int flags);

        public static Icon ExtractAssociatedIcon(string path)
        {
            int i = 0;
            return ExtractAssociatedIcon(path, 0, out i);
        }

        public static Icon ExtractAssociatedIcon(string path, int index, out int i)
        {
            StringBuilder iconPath = new StringBuilder(260);
            iconPath.Append(path);
            IntPtr handle = IntExtractAssociatedIcon(NullHandleRef, iconPath, ref index);
            i = index;
            if (handle != IntPtr.Zero)
            {
                return Icon.FromHandle(handle);
            }
            return null;
        }

        public static Icon GetSmallAssociatedIcon(string path)
        {
            StringBuilder iconPath = new StringBuilder(260);
            iconPath.Append(path);
            SHFILEINFO fi = new SHFILEINFO();
            int size = Marshal.SizeOf(fi);
            SHGetFileInfo(iconPath, 0, ref fi, size, SHGFI_SMALLICON | SHGFI_ICON);
            if (fi.hIcon != IntPtr.Zero)
            {
                return Icon.FromHandle(fi.hIcon);
            }
            return null;
        }

        public static Icon GetLargeAssociatedIcon(string path)
        {
            StringBuilder iconPath = new StringBuilder(260);
            iconPath.Append(path);
            SHFILEINFO fi = new SHFILEINFO();
            int size = Marshal.SizeOf(fi);
            SHGetFileInfo(iconPath, 0, ref fi, size, SHGFI_LARGEICON | SHGFI_ICON);
            if (fi.hIcon != IntPtr.Zero)
            {
                return Icon.FromHandle(fi.hIcon);
            }
            return null;
        }


        public static int GetAssociatedIconIndex(string path)
        {
            StringBuilder iconPath = new StringBuilder(260);
            iconPath.Append(path);
            SHFILEINFO fi = new SHFILEINFO();
            int size = Marshal.SizeOf(fi);
            SHGetFileInfo(iconPath, 0, ref fi, size, SHGFI_SMALLICON | SHGFI_SYSICONINDEX);
            return fi.iIcon;
        }

    }
}
