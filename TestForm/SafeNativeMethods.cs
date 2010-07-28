using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TestForm
{
    public static class SafeNativeMethods
    {
        internal static HandleRef NullHandleRef;


        public static Icon ExtractAssociatedIcon(string path)
        {
            int i;
            return ExtractAssociatedIcon(path, 0, out i);
        }

        public static Icon ExtractAssociatedIcon(string path, int index, out int i)
        {
            StringBuilder iconPath = new StringBuilder(260);
            iconPath.Append(path);
            IntPtr handle = ExtractAssociatedIcon(NullHandleRef, iconPath, ref index);
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

        public static Icon GetSmallAssociatedIcon(string path)
        {
            ShFileInfo fileInfo = new ShFileInfo();
            SHGetFileInfo(path, 0, ref fileInfo, Marshal.SizeOf(fileInfo), FileInfoType.SmallIcon | FileInfoType.Icon);

            try
            {
                if (fileInfo.IconHandle != IntPtr.Zero)
                {
                    return Icon.FromHandle(fileInfo.IconHandle);
                }
            }
            finally
            {
                DestroyIcon(fileInfo.IconHandle);
            }

            return null;
        }

        public static Icon GetLargeAssociatedIcon(string path)
        {
            ShFileInfo fileInfo = new ShFileInfo();
            SHGetFileInfo(path, 0, ref fileInfo, Marshal.SizeOf(fileInfo), FileInfoType.LargeIcon | FileInfoType.Icon);

            try
            {
                if (fileInfo.IconHandle != IntPtr.Zero)
                {
                    return Icon.FromHandle(fileInfo.IconHandle);
                }
            }
            finally
            {
                DestroyIcon(fileInfo.IconHandle);
            }

            return null;
        }

        public static int GetSmallAssociatedIconIndex(string path)
        {
            ShFileInfo fileInfo = new ShFileInfo();

            SHGetFileInfo(path, 0, ref fileInfo, Marshal.SizeOf(fileInfo), FileInfoType.SmallIcon | FileInfoType.SysIconIndex | FileInfoType.LinkOverlay);
            DestroyIcon(fileInfo.IconHandle);

            return fileInfo.IconIndex;
        }

        public static int GetSmallAssociatedIconIndex(string path, FileAttributes attributes)
        {
            ShFileInfo fileInfo = new ShFileInfo();

            SHGetFileInfo(path, attributes, ref fileInfo, Marshal.SizeOf(fileInfo), FileInfoType.SmallIcon | FileInfoType.SysIconIndex | FileInfoType.UseFileAttributes | FileInfoType.LinkOverlay);
            DestroyIcon(fileInfo.IconHandle);

            return fileInfo.IconIndex;
        }

        public static int GetLargeAssociatedIconIndex(string path)
        {
            ShFileInfo fileInfo = new ShFileInfo();

            SHGetFileInfo(path, 0, ref fileInfo, Marshal.SizeOf(fileInfo), FileInfoType.LargeIcon | FileInfoType.SysIconIndex | FileInfoType.LinkOverlay | FileInfoType.OverlayIndex);
            DestroyIcon(fileInfo.IconHandle);

            return fileInfo.IconIndex;
        }

        public static int GetLargeAssociatedIconIndex(string path, FileAttributes attributes)
        {
            ShFileInfo fileInfo = new ShFileInfo();

            SHGetFileInfo(path, attributes, ref fileInfo, Marshal.SizeOf(fileInfo), FileInfoType.LargeIcon | FileInfoType.SysIconIndex | FileInfoType.LinkOverlay | FileInfoType.UseFileAttributes);
            DestroyIcon(fileInfo.IconHandle);

            return fileInfo.IconIndex;
        }


        [DllImport("shell32.dll", EntryPoint = "ExtractAssociatedIcon")]
        internal static extern IntPtr ExtractAssociatedIcon(HandleRef handle, StringBuilder iconPath, ref int index);

        [DllImport("shfolder.dll")]
        internal static extern int SHGetFolderPath(IntPtr owner, int folderIndex, IntPtr hToken, int flags, StringBuilder path);

        [DllImport("shell32.dll")]
        internal static extern IntPtr SHGetFileInfo(string path, int fileAttributes, ref ShFileInfo psfi, int fileInfo, FileInfoType flags);

        [DllImport("shell32.dll")]
        internal static extern IntPtr SHGetFileInfo(string path, FileAttributes fileAttributes, ref ShFileInfo psfi, int fileInfo, FileInfoType flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal extern static bool DestroyIcon(IntPtr handle);
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct ShFileInfo
    {
        public IntPtr IconHandle;
        public int IconIndex;
        public int Attributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string DisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string TypeName;
    }

    [Flags]
    public enum FileInfoType
    {
        /// <summary>get icon</summary>
        Icon = 0x000000100,
        /// <summary>get display name</summary>
        DisplayName = 0x000000200,
        /// <summary>get type name</summary>
        TypeName = 0x000000400,
        /// <summary>get attributes</summary>
        Attributes = 0x000000800,
        /// <summary>get icon location</summary>
        IconLocation = 0x000001000,
        /// <summary>return exe type</summary>
        ExeType = 0x000002000,
        /// <summary>get system icon index</summary>
        SysIconIndex = 0x000004000,
        /// <summary>put a link overlay on icon</summary>
        LinkOverlay = 0x000008000,
        /// <summary>show icon in selected state</summary>
        Selected = 0x000010000,
        /// <summary>get only specified attributes</summary>
        Attr_Specified = 0x000020000,
        /// <summary>get large icon</summary>
        LargeIcon = 0x000000000,
        /// <summary>get small icon</summary>
        SmallIcon = 0x000000001,
        /// <summary>get open icon</summary>
        OpenIcon = 0x000000002,
        /// <summary>get shell size icon</summary>
        ShellIconSize = 0x000000004,
        /// <summary>pszPath is a pidl</summary>
        PIDL = 0x000000008,
        /// <summary>use passed dwFileAttribute</summary>
        UseFileAttributes = 0x000000010,
        /// <summary>apply the appropriate overlays</summary>
        AddOverlays = 0x000000020,
        /// <summary>Get the index of the overlay in the upper 8 bits of the iIcon</summary>
        OverlayIndex = 0x000000040,
    }
}
