using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Threading;
using System.ComponentModel;

namespace ShellDll
{
    public class ShellBrowser : Component
    {
        private ShellBrowserUpdater updater;


        public ShellBrowser()
        {
            Initialize();

            Browsers = new ArrayList();
            UpdateCondition = new ShellItemUpdateCondition();
            updater = new ShellBrowserUpdater(this);
        }


        public ShellItem DesktopItem { get; private set; }

        public string MyDocumentsName { get; private set; }

        public string MyComputerName { get; private set; }

        public string SystemFolderName { get; private set; }

        public string MyDocumentsPath { get; private set; }

        public ShellItemUpdateCondition UpdateCondition { get; private set; }

        public ArrayList Browsers { get; private set; }


        public event ShellItemUpdateEventHandler ShellItemUpdate;


        public static IntPtr GetDesctopPidl()
        {
            IntPtr tempPidl = IntPtr.Zero;
            ShellAPI.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.DESKTOP, out tempPidl);
            return tempPidl;
        }

        public void OnShellItemUpdate(object sender, ShellItemUpdateEventArgs e)
        {
            if (ShellItemUpdate != null)
            {
                ShellItemUpdate(sender, e);
            }
        }

        public ShellItem GetShellItem(PIDL pidlFull)
        {
            ShellItem current = DesktopItem;
            if (pidlFull.Ptr == IntPtr.Zero)
            {
                return current;
            }

            foreach (IntPtr pidlRel in pidlFull)
            {
                int index;
                if ((index = current.IndexOf(pidlRel)) > -1)
                {
                    current = current[index];
                }
                else
                {
                    current = null;
                    break;
                }
            }

            return current;
        }

        public ShellItem[] GetPath(ShellItem item)
        {
            ArrayList pathList = new ArrayList();

            ShellItem currentItem = item;
            while (currentItem.ParentItem != null)
            {
                pathList.Add(currentItem);
                currentItem = currentItem.ParentItem;
            }
            pathList.Add(currentItem);
            pathList.Reverse();

            return (ShellItem[])pathList.ToArray(typeof(ShellItem));
        }


        private void Initialize()
        {
            IntPtr tempPidl;
            SHFILEINFO info;

            //My Computer
            info = new SHFILEINFO();
            tempPidl = IntPtr.Zero;
            ShellAPI.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.DRIVES, out tempPidl);

            ShellAPI.SHGetFileInfo(tempPidl, 0, ref info, ShellAPI.cbFileInfo, SHGFI.PIDL | SHGFI.DISPLAYNAME | SHGFI.TYPENAME);

            SystemFolderName = info.szTypeName;
            MyComputerName = info.szDisplayName;
            Marshal.FreeCoTaskMem(tempPidl);
            //

            //Dekstop
            tempPidl = IntPtr.Zero;
            ShellAPI.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.DESKTOP, out tempPidl);
            IntPtr desktopFolderPtr;
            ShellAPI.SHGetDesktopFolder(out desktopFolderPtr);
            DesktopItem = new ShellItem(this, tempPidl, desktopFolderPtr);
            //


            //My Documents
            uint pchEaten = 0;
            SFGAO pdwAttributes = 0;
            DesktopItem.ShellFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, SpecialFolderPath.MyDocuments, ref pchEaten, out tempPidl, ref pdwAttributes);

            info = new SHFILEINFO();
            ShellAPI.SHGetFileInfo(tempPidl, 0, ref info, ShellAPI.cbFileInfo, SHGFI.PIDL | SHGFI.DISPLAYNAME);

            MyDocumentsName = info.szDisplayName;
            Marshal.FreeCoTaskMem(tempPidl);

            StringBuilder path = new StringBuilder(ShellAPI.MAX_PATH);
            ShellAPI.SHGetFolderPath(IntPtr.Zero, CSIDL.PERSONAL, IntPtr.Zero, SHGFP.TYPE_CURRENT, path);
            MyDocumentsPath = path.ToString();
            //
        }
    }


    public delegate void ShellItemUpdateEventHandler(object sender, ShellItemUpdateEventArgs e);

    public class ShellItemUpdateEventArgs : EventArgs
    {
        public ShellItemUpdateEventArgs(ShellItem oldItem, ShellItem newItem, ShellItemUpdateType type)
        {
            this.OldItem = oldItem;
            this.NewItem = newItem;
            this.UpdateType = type;
        }


        public ShellItem OldItem { get; private set; }

        public ShellItem NewItem { get; private set; }

        public ShellItemUpdateType UpdateType { get; private set; }
    }

    public enum ShellItemUpdateType
    {
        Created,
        IconChange,
        Updated,
        Renamed,
        Deleted,
        MediaChange
    }
}