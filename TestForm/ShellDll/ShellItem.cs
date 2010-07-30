using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ShellDll
{
    public abstract class ShellItem
    {
        protected IShellItem item;
        protected IntPtr pidl;
        private string name;
        private string path;
        private int? imageIndex;


        protected ShellItem(IShellItem item, IntPtr pidl)
        {
            this.item = item;
            this.pidl = pidl;
        }


        public virtual string Name { get { return name ?? (name = GetName()); } }

        public virtual string Path { get { return path ?? (path = GetPath()); } }

        //public int ImageIndex { get { return (imageIndex ?? (imageIndex = GetImageIndex())).Value; } }

        public abstract bool IsFolder { get; }


        private string GetName()
        {
            string result;
            this.item.GetDisplayName(SIGDN.NORMALDISPLAY, out result);
            return result;
        }

        private string GetPath()
        {            
            if (item != null)
            {
                try
                {
                    string result;
                    this.item.GetDisplayName(SIGDN.FILESYSPATH, out result);
                    return result;
                }
                catch (Exception)
                { 
                }
            }

            return string.Empty;
        }

        private IShellItem GetParent()
        {
            IShellItem result;
            this.item.GetParent(out result);
            return result;
        }

        private IShellFolder ToIShellFolder()
        {
            IShellFolder result;
            item.BindToHandler(IntPtr.Zero, ShellGuids.ShellFolderObject, ShellGuids.IShellFolder, out result);

            return result;
        }

        private SFGAO GetAttributes()
        {
            SFGAO result;
            this.item.GetAttributes(SFGAO.FILESYSTEM | SFGAO.FOLDER, out result);
            return result;
        }

        public int GetImageIndex()
        {
            SHFILEINFO info = new SHFILEINFO();
            Shell32.SHGetFileInfo(pidl, 0, ref info, Marshal.SizeOf(info), SHGFI.PIDL | SHGFI.SYSICONINDEX | SHGFI.OVERLAYINDEX | SHGFI.LARGEICON | SHGFI.ADDOVERLAYS | SHGFI.LINKOVERLAY);
            Shell32.DestroyIcon(info.hIcon);

            return info.iIcon;
        }
    }
}
