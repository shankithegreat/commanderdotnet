
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ShellDll
{
    public class ShellFolder : ShellItem
    {
        private IShellFolder folder;
        private string name;
        private string path;
        private ShellItem[] childs;


        public ShellFolder(IShellFolder folder, IntPtr pidl)
            : base(ToIShellItem(folder, pidl), pidl)
        {
            this.folder = folder;
        }

        public ShellFolder(IShellItem item, IntPtr pidl)
            : base(item, pidl)
        {
            this.folder = GetIShellFolder();
        }


        public virtual string Name { get { return name ?? (name = (item == null ? GetName() : base.Name)); } }

        //public virtual string Path { get { return path ?? (path = GetPath()); } }

        public override bool IsFolder { get { return true; } }

        public ShellItem[] Childs { get { return childs ?? (childs = GetChilds()); } }


        public static ShellFolder GetDesktopFolder()
        {
            IShellFolder folder;
            Shell32.SHGetDesktopFolder(out folder);

            return new ShellFolder(folder, IntPtr.Zero);
        }


        private string GetName()
        {
            IntPtr p = Marshal.AllocCoTaskMem(260 * 2 + 4);
            try
            {
                StringBuilder result = new StringBuilder(260);

                if (folder.GetDisplayNameOf(pidl, SHGNO.INFOLDER, p) == 0)
                {
                    ShellApi.StrRetToBuf(p, pidl, result, 260);
                    return result.ToString();
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(p);
            }

            return string.Empty;
        }

        private string GetPath()
        {
            IntPtr p = Marshal.AllocCoTaskMem(260 * 2 + 4);
            try
            {
                StringBuilder result = new StringBuilder(260);

                if (folder.GetDisplayNameOf(pidl, SHGNO.FORADDRESSBAR | SHGNO.FORPARSING, p) == 0)
                {
                    ShellApi.StrRetToBuf(p, pidl, result, 260);
                    return result.ToString();
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(p);
            }

            return string.Empty;
        }

        private static IntPtr GetIdList(string path)
        {
            IShellFolder desktop;
            Shell32.SHGetDesktopFolder(out desktop);

            // Get PIDL       
            IntPtr pidl;
            uint pchEaten = 0;
            SFGAO pdwAttributes = 0;
            desktop.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, path, ref pchEaten, out pidl, ref pdwAttributes);

            return pidl;
        }

        private IShellFolder GetIShellFolder()
        {
            IShellFolder result;
            item.BindToHandler(IntPtr.Zero, ShellGuids.ShellFolderObject, ShellGuids.IShellFolder, out result);

            return result;
        }

        private static IShellItem ToIShellItem(IShellFolder folder, IntPtr pidl)
        {
            IntPtr p;
            IShellItem result = null;
            folder.BindToObject(pidl, IntPtr.Zero, ref ShellGuids.IShellItem, out p);
            if (p != IntPtr.Zero)
            {
                result = (IShellItem) Marshal.GetTypedObjectForIUnknown(p, typeof (IShellItem));
                Marshal.Release(p);
            }

            return result;
        }

        private ShellItem[] GetChilds()
        {
            List<ShellItem> result = new List<ShellItem>(40);
            Dictionary<IntPtr, int> list2 = new Dictionary<IntPtr, int>(40);



            IEnumIDList list;
            folder.EnumObjects(IntPtr.Zero, SHCONT.INIT_ON_FIRST_NEXT | SHCONT.NONFOLDERS | SHCONT.STORAGE | SHCONT.SHAREABLE | SHCONT.FOLDERS | SHCONT.INCLUDEHIDDEN, out list);

            IntPtr pidl;
            int numItemsReturned;
            int itemsRequested = 1;
            while (list.Next(itemsRequested, out pidl, out numItemsReturned) == 0 && numItemsReturned == itemsRequested)
            {
                IShellItem item;
                Shell32.SHCreateShellItem(IntPtr.Zero, folder, pidl, out item);

                SFGAO attr;
                item.GetAttributes(SFGAO.STREAM | SFGAO.BROWSABLE | SFGAO.STORAGE | SFGAO.FILESYSTEM | SFGAO.FOLDER, out attr);

                if ((attr & SFGAO.FOLDER) == 0 || ((attr & SFGAO.FILESYSTEM) != 0 && (attr & SFGAO.CANMONIKER) != 0))
                {
                    result.Add(new ShellFile(item, pidl));
                }
                else
                {
                    result.Add(new ShellFolder(item, pidl));
                }
                
                list2.Add(pidl, 0);
            }

            List<ShellItem> result2 = new List<ShellItem>(result.Count);
            foreach (var shellItem in result)
            {
                if (shellItem.IsFolder)
                {
                    result2.Add(shellItem);
                }
            }

            foreach (var shellItem in result)
            {
                if (!shellItem.IsFolder)
                {
                    result2.Add(shellItem);
                }
            }

            /*folder.EnumObjects(IntPtr.Zero, SHCONT.INIT_ON_FIRST_NEXT | SHCONT.NONFOLDERS | SHCONT.INCLUDEHIDDEN, out list);

            itemsRequested = 1;
            while (list.Next(itemsRequested, out pidl, out numItemsReturned) == 0 && numItemsReturned == itemsRequested)
            {
                if (!list2.ContainsKey(pidl))
                {
                    IShellItem item;
                    Shell32.SHCreateShellItem(IntPtr.Zero, folder, pidl, out item);

                    result.Add(new ShellFile(item, pidl));
                }
            }*/

            return result2.ToArray();
        }
    }
}
