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


        public ShellNode DesktopItem { get; private set; }

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

        public ShellNode GetShellItem(Pidl pidlFull)
        {
            ShellNode current = DesktopItem;
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

        public ShellNode[] GetPath(ShellNode item)
        {
            ArrayList pathList = new ArrayList();

            ShellNode currentItem = item;
            while (currentItem.ParentItem != null)
            {
                pathList.Add(currentItem);
                currentItem = currentItem.ParentItem;
            }
            pathList.Add(currentItem);
            pathList.Reverse();

            return (ShellNode[])pathList.ToArray(typeof(ShellNode));
        }


        private void Initialize()
        {
            IntPtr tempPidl;
            SHFILEINFO info;

            //My Computer
            info = new SHFILEINFO();
            tempPidl = IntPtr.Zero;
            ShellApi.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.DRIVES, out tempPidl);

            ShellApi.SHGetFileInfo(tempPidl, 0, ref info, ShellApi.cbFileInfo, SHGFI.PIDL | SHGFI.DISPLAYNAME | SHGFI.TYPENAME);

            SystemFolderName = info.szTypeName;
            MyComputerName = info.szDisplayName;
            Marshal.FreeCoTaskMem(tempPidl);
            //

            //Dekstop
            tempPidl = IntPtr.Zero;
            ShellApi.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.DESKTOP, out tempPidl);
            IntPtr desktopFolderPtr;
            ShellApi.SHGetDesktopFolder(out desktopFolderPtr);
            DesktopItem = new ShellNode(this, tempPidl, desktopFolderPtr);
            //


            //My Documents
            uint pchEaten = 0;
            SFGAO pdwAttributes = 0;
            DesktopItem.ShellFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, SpecialFolderPath.MyDocuments, ref pchEaten, out tempPidl, ref pdwAttributes);

            info = new SHFILEINFO();
            ShellApi.SHGetFileInfo(tempPidl, 0, ref info, ShellApi.cbFileInfo, SHGFI.PIDL | SHGFI.DISPLAYNAME);

            MyDocumentsName = info.szDisplayName;
            Marshal.FreeCoTaskMem(tempPidl);

            StringBuilder path = new StringBuilder(ShellApi.MAX_PATH);
            ShellApi.SHGetFolderPath(IntPtr.Zero, CSIDL.PERSONAL, IntPtr.Zero, SHGFP.TYPE_CURRENT, path);
            MyDocumentsPath = path.ToString();
            //
        }
    }


    public delegate void ShellItemUpdateEventHandler(object sender, ShellItemUpdateEventArgs e);

    public class ShellItemUpdateEventArgs : EventArgs
    {
        public ShellItemUpdateEventArgs(ShellNode oldItem, ShellNode newItem, ShellItemUpdateType type)
        {
            this.OldItem = oldItem;
            this.NewItem = newItem;
            this.UpdateType = type;
        }


        public ShellNode OldItem { get; private set; }

        public ShellNode NewItem { get; private set; }

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