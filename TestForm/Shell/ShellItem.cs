using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TestForm.Shell
{
    public class ShellItem
    {
        private IShellItem item;
        private string name;
        private string path;


        public ShellItem(IntPtr pidl, IShellItem item)
        {
            this.Pidl = pidl;
            this.item = item;
        }


        public IntPtr Pidl { get; private set; }

        public string Name { get { return name ?? (name = GetName()); } }

        public string Path { get { return path ?? (path = GetPath()); } }


        private string GetName()
        {
            string result;
            this.item.GetDisplayName(SIGDN.NORMALDISPLAY, out result);
            return result;
        }

        private string GetPath()
        {
            string result;
            this.item.GetDisplayName(SIGDN.FILESYSPATH, out result);
            return result;
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
            this.item.GetAttributes(SFGAO.FILESYSTEM, out result);
            return result;
        }
    }
}
