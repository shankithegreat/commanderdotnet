using System;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Shell
{
    public static class ShellImageList
    {
        private const int TVSIL_NORMAL = 0;
        private const int TVSIL_SMALL = 1;
        private static Hashtable imageTable = new Hashtable();


        static ShellImageList()
        {
            ShFileInfo fileInfo = new ShFileInfo();

            SmallImageList = Shell32.SHGetFileInfo(".txt", FileAttribute.Normal, ref fileInfo, Marshal.SizeOf(fileInfo), SHGFI.UseFileAttributes | SHGFI.SysIconIndex | SHGFI.SmallIcon);
            LargeImageList = Shell32.SHGetFileInfo(".txt", FileAttribute.Normal, ref fileInfo, Marshal.SizeOf(fileInfo), SHGFI.UseFileAttributes | SHGFI.SysIconIndex | SHGFI.LargeIcon);
        }


        public static IntPtr SmallImageList { get; private set; }

        public static IntPtr LargeImageList { get; private set; }


        public static Icon GetIcon(int index, bool small)
        {
            IntPtr iconPtr = ComCtl32.ImageList_GetIcon(small ? SmallImageList : LargeImageList, index, ILD.Normal);

            if (iconPtr != IntPtr.Zero)
            {
                Icon icon = Icon.FromHandle(iconPtr);
                Icon result = (Icon)icon.Clone();
                User32.DestroyIcon(iconPtr);

                return result;
            }

            return null;
        }

        public static void SetIconIndex(ShellNode item, int index, bool selectedIcon)
        {
            bool hasOverlay = false;
            int result; // The returned Index

            SHGFI flag = SHGFI.SysIconIndex | SHGFI.Pidl | SHGFI.Icon;
            FileAttribute attribute = 0;

            // build Key into HashTable for this Item
            int Key = index * 256;

            if (item.IsLink)
            {
                Key = Key | 1;
                flag = flag | SHGFI.LinkOverlay;
                hasOverlay = true;
            }
            if (item.IsShared)
            {
                Key = Key | 2;
                flag = flag | SHGFI.AddOverlays;
                hasOverlay = true;
            }
            if (selectedIcon)
            {
                Key = Key | 4;
                flag = flag | SHGFI.OpenIcon;
                hasOverlay = true; // not really an overlay, but handled the same
            }

            if (imageTable.ContainsKey(Key))
            {
                result = (int)imageTable[Key];
            }
            else if (!hasOverlay && !item.IsHidden) // for non-overlay icons, we already have
            {
                result = (int)System.Math.Floor((double)Key / 256); // the right index -- put in table
                imageTable[Key] = result;
            }
            else // don't have iconindex for an overlay, get it.
            {
                if (item.IsFileSystem & !item.IsDisk & !item.IsFolder)
                {
                    flag = flag | SHGFI.UseFileAttributes;
                    attribute = attribute | FileAttribute.Normal;
                }

                Pidl pidl = item.PIDLFull;

                ShFileInfo smallFileInfo = new ShFileInfo();
                Shell32.SHGetFileInfo(pidl.Ptr, attribute, ref smallFileInfo, Marshal.SizeOf(smallFileInfo), flag | SHGFI.SmallIcon);

                ShFileInfo largeFileInfo = new ShFileInfo();
                Shell32.SHGetFileInfo(pidl.Ptr, attribute, ref largeFileInfo, Marshal.SizeOf(largeFileInfo), flag | SHGFI.LargeIcon);

                Marshal.FreeCoTaskMem(pidl.Ptr);

                lock (imageTable)
                {
                    result = ComCtl32.ImageList_ReplaceIcon(SmallImageList, -1, smallFileInfo.IconHandle);
                    ComCtl32.ImageList_ReplaceIcon(LargeImageList, -1, largeFileInfo.IconHandle);
                }

                User32.DestroyIcon(smallFileInfo.IconHandle);
                User32.DestroyIcon(largeFileInfo.IconHandle);

                imageTable[Key] = result;
            }

            if (selectedIcon)
            {
                item.SelectedImageIndex = result;
            }
            else
            {
                item.ImageIndex = result;
            }
        }

        public static void SetSmallImageList(TreeView treeView)
        {
            User32.SendMessage(treeView.Handle, WM.TvmSetImageList, TVSIL_NORMAL, SmallImageList);
        }

        public static void SetSmallImageList(ListView listView)
        {
            User32.SendMessage(listView.Handle, WM.LvmSetImageList, TVSIL_SMALL, SmallImageList);
        }

        public static void Set32SmallImageList(ListView listView)
        {
            User32.SendMessage(listView.Handle, WM.LvmSetImageList, TVSIL_SMALL, LargeImageList);
        }

        public static void SetLargeImageList(ListView listView)
        {
            User32.SendMessage(listView.Handle, WM.LvmSetImageList, TVSIL_NORMAL, LargeImageList);
        }

        public static void SetLargeImageList(ListView listView, IntPtr handle)
        {
            User32.SendMessage(listView.Handle, WM.LvmSetImageList, TVSIL_NORMAL, handle);
        }
    }
}