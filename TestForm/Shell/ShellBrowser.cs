using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Threading;
using System.ComponentModel;

namespace Shell
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
            Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.Desktop, out tempPidl);
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
            ShFileInfo info;

            //My Computer
            info = new ShFileInfo();
            tempPidl = IntPtr.Zero;
            Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.Drives, out tempPidl);

            Shell32.SHGetFileInfo(tempPidl, 0, ref info, Marshal.SizeOf(info), SHGFI.Pidl | SHGFI.DisplayName | SHGFI.TypeName);

            SystemFolderName = info.TypeName;
            MyComputerName = info.DisplayName;
            Marshal.FreeCoTaskMem(tempPidl);
            //

            //Dekstop
            tempPidl = IntPtr.Zero;
            Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.Desktop, out tempPidl);
            IntPtr desktopFolderPtr;
            Shell32.SHGetDesktopFolder(out desktopFolderPtr);
            DesktopItem = new ShellNode(this, tempPidl, desktopFolderPtr);
            //


            //My Documents
            uint pchEaten = 0;
            SFGAO pdwAttributes = 0;
            DesktopItem.ShellFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, SpecialFolderPath.MyDocuments, ref pchEaten, out tempPidl, ref pdwAttributes);

            info = new ShFileInfo();
            Shell32.SHGetFileInfo(tempPidl, 0, ref info, Marshal.SizeOf(info), SHGFI.Pidl | SHGFI.DisplayName);

            MyDocumentsName = info.DisplayName;
            Marshal.FreeCoTaskMem(tempPidl);

            StringBuilder path = new StringBuilder(ShellApi.MaxPath);
            Shell32.SHGetFolderPath(IntPtr.Zero, CSIDL.Personal, IntPtr.Zero, SHGFP.TypeCurrent, path);
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