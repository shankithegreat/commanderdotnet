using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TestForm
{
    public static class SafeNativeMethods
    {
        internal static HandleRef NullHandleRef;

        internal static int SHGFI_ICON = 0x000000100;
        internal static int SHGFI_LARGEICON = 0x000000000;     // get large icon
        internal static int SHGFI_SMALLICON = 0x000000001;    // get small icon
        internal static int SHGFI_SYSICONINDEX = 0x000004000;
        internal static int SHGFI_TYPENAME = 0x000000400;


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


        public static Icon ExtractAssociatedIcon(string path)
        {
            int i;
            return ExtractAssociatedIcon(path, 0, out i);
        }

        public static Icon ExtractAssociatedIcon(string path, int index, out int i)
        {
            StringBuilder iconPath = new StringBuilder(260);
            iconPath.Append(path);
            IntPtr handle = IntExtractAssociatedIcon(NullHandleRef, iconPath, ref index);
            i = index;
            try
            {
                if (handle != IntPtr.Zero)
                {
                    return Icon.FromHandle(handle);
                }
            }
            finally
            {
                DestroyIcon(handle);
            }
            return null;
        }

        public static SHFILEINFO GetFileInfo(string path, int flag)
        {
            StringBuilder iconPath = new StringBuilder(260);
            iconPath.Append(path);
            SHFILEINFO fileInfo = new SHFILEINFO();
            int size = Marshal.SizeOf(fileInfo);

            SHGetFileInfo(iconPath, 0, ref fileInfo, size, flag);

            return fileInfo;
        }

        public static Icon GetSmallAssociatedIcon(string path)
        {
            SHFILEINFO fi = GetFileInfo(path, SHGFI_SMALLICON | SHGFI_ICON);

            try
            {
                if (fi.hIcon != IntPtr.Zero)
                {
                    return Icon.FromHandle(fi.hIcon);
                }
            }
            finally
            {
                DestroyIcon(fi.hIcon);
            }

            return null;
        }

        public static Icon GetLargeAssociatedIcon(string path)
        {
            SHFILEINFO fi = GetFileInfo(path, SHGFI_LARGEICON | SHGFI_ICON);

            try
            {
                if (fi.hIcon != IntPtr.Zero)
                {
                    return Icon.FromHandle(fi.hIcon);
                }
            }
            finally
            {
                DestroyIcon(fi.hIcon);
            }

            return null;
        }

        public static int GetSmallAssociatedIconIndex(string path)
        {
            SHFILEINFO fi = GetFileInfo(path, SHGFI_SMALLICON | SHGFI_SYSICONINDEX);
            DestroyIcon(fi.hIcon);
            return fi.iIcon;
        }

        public static int GetLargeAssociatedIconIndex(string path)
        {
            SHFILEINFO fi = GetFileInfo(path, SHGFI_LARGEICON | SHGFI_SYSICONINDEX);
            DestroyIcon(fi.hIcon);
            return fi.iIcon;
        }


        [DllImport("shell32.dll", EntryPoint = "ExtractAssociatedIcon")]
        internal static extern IntPtr IntExtractAssociatedIcon(HandleRef hInst, StringBuilder iconPath, ref int index);

        [DllImport("shfolder.dll")]
        internal static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

        [DllImport("shell32.dll")]
        internal static extern IntPtr SHGetFileInfo(StringBuilder path, int fileAttributes, ref SHFILEINFO psfi, int fileInfo, int flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal extern static bool DestroyIcon(IntPtr handle);
    }
}
