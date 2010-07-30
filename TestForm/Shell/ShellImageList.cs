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
            SHFILEINFO fileInfo = new SHFILEINFO();

            SmallImageList = Shell32.SHGetFileInfo(".txt", FILE_ATTRIBUTE.NORMAL, ref fileInfo, Marshal.SizeOf(fileInfo), SHGFI.USEFILEATTRIBUTES | SHGFI.SYSICONINDEX | SHGFI.SMALLICON);
            LargeImageList = Shell32.SHGetFileInfo(".txt", FILE_ATTRIBUTE.NORMAL, ref fileInfo, Marshal.SizeOf(fileInfo), SHGFI.USEFILEATTRIBUTES | SHGFI.SYSICONINDEX | SHGFI.LARGEICON);
        }


        public static IntPtr SmallImageList { get; private set; }

        public static IntPtr LargeImageList { get; private set; }


        public static Icon GetIcon(int index, bool small)
        {
            IntPtr iconPtr = ComCtl32.ImageList_GetIcon(small ? SmallImageList : LargeImageList, index, ILD.NORMAL);

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

            SHGFI flag = SHGFI.SYSICONINDEX | SHGFI.PIDL | SHGFI.ICON;
            FILE_ATTRIBUTE attribute = 0;

            // build Key into HashTable for this Item
            int Key = index * 256;

            if (item.IsLink)
            {
                Key = Key | 1;
                flag = flag | SHGFI.LINKOVERLAY;
                hasOverlay = true;
            }
            if (item.IsShared)
            {
                Key = Key | 2;
                flag = flag | SHGFI.ADDOVERLAYS;
                hasOverlay = true;
            }
            if (selectedIcon)
            {
                Key = Key | 4;
                flag = flag | SHGFI.OPENICON;
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
                    flag = flag | SHGFI.USEFILEATTRIBUTES;
                    attribute = attribute | FILE_ATTRIBUTE.NORMAL;
                }

                Pidl pidl = item.PIDLFull;

                SHFILEINFO smallFileInfo = new SHFILEINFO();
                Shell32.SHGetFileInfo(pidl.Ptr, attribute, ref smallFileInfo, Marshal.SizeOf(smallFileInfo), flag | SHGFI.SMALLICON);

                SHFILEINFO largeFileInfo = new SHFILEINFO();
                Shell32.SHGetFileInfo(pidl.Ptr, attribute, ref largeFileInfo, Marshal.SizeOf(largeFileInfo), flag | SHGFI.LARGEICON);

                Marshal.FreeCoTaskMem(pidl.Ptr);

                lock (imageTable)
                {
                    result = ComCtl32.ImageList_ReplaceIcon(SmallImageList, -1, smallFileInfo.hIcon);
                    ComCtl32.ImageList_ReplaceIcon(LargeImageList, -1, largeFileInfo.hIcon);
                }

                User32.DestroyIcon(smallFileInfo.hIcon);
                User32.DestroyIcon(largeFileInfo.hIcon);

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
            User32.SendMessage(treeView.Handle, WM.TVM_SETIMAGELIST, TVSIL_NORMAL, SmallImageList);
        }

        public static void SetSmallImageList(ListView listView)
        {
            User32.SendMessage(listView.Handle, WM.LVM_SETIMAGELIST, TVSIL_SMALL, SmallImageList);
        }

        public static void Set32SmallImageList(ListView listView)
        {
            User32.SendMessage(listView.Handle, WM.LVM_SETIMAGELIST, TVSIL_SMALL, LargeImageList);
        }

        public static void SetLargeImageList(ListView listView)
        {
            User32.SendMessage(listView.Handle, WM.LVM_SETIMAGELIST, TVSIL_NORMAL, LargeImageList);
        }

        public static void SetLargeImageList(ListView listView, IntPtr handle)
        {
            User32.SendMessage(listView.Handle, WM.LVM_SETIMAGELIST, TVSIL_NORMAL, handle);
        }
    }
}