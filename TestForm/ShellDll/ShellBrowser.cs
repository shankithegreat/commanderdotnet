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
            ShellApi.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.DESKTOP, out tempPidl);
            return tempPidl;
        }

        public void OnShellItemUpdate(object sender, ShellItemUpdateEventArgs e)
        {
            if (ShellItemUpdate != null)
            {
                ShellItemUpdate(sender, e);
            }
        }

        public ShellItem GetShellItem(Pidl pidlFull)
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
            
            SHFILEINFO info;

            //My Computer
            info = new SHFILEINFO();
            IntPtr pidl = IntPtr.Zero;
            ShellApi.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.DRIVES, out pidl);

            ShellApi.SHGetFileInfo(pidl, 0, ref info, ShellApi.cbFileInfo, SHGFI.PIDL | SHGFI.DISPLAYNAME | SHGFI.TYPENAME);

            SystemFolderName = info.szTypeName;
            MyComputerName = info.szDisplayName;
            Marshal.FreeCoTaskMem(pidl);
            //

            //Dekstop
            ShellApi.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.DESKTOP, out pidl);
            IntPtr desktopFolderPtr;
            ShellApi.SHGetDesktopFolder(out desktopFolderPtr);
            DesktopItem = new ShellItem(this, pidl, desktopFolderPtr);
            //


            //My Documents
            uint pchEaten = 0;
            SFGAO pdwAttributes = 0;
            DesktopItem.ShellFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, SpecialFolderPath.MyDocuments, ref pchEaten, out pidl, ref pdwAttributes);

            info = new SHFILEINFO();
            ShellApi.SHGetFileInfo(pidl, 0, ref info, ShellApi.cbFileInfo, SHGFI.PIDL | SHGFI.DISPLAYNAME);

            MyDocumentsName = info.szDisplayName;
            Marshal.FreeCoTaskMem(pidl);

            StringBuilder path = new StringBuilder(ShellApi.MAX_PATH);
            ShellApi.SHGetFolderPath(IntPtr.Zero, CSIDL.PERSONAL, IntPtr.Zero, SHGFP.TYPE_CURRENT, path);
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