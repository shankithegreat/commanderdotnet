namespace Microsoft.Shell
{
    using Microsoft;
    using Microsoft.Win32;
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class ShellItem : CriticalFinalizerObject, IDisposable, ISerializable
    {
        private const string EntryPidl = "Pidl";
        private IntPtr FAbsolutePidl;
        private IntPtr FRelativePidl;

        public ShellItem(IntPtr absolutePidl)
        {
            if (absolutePidl == IntPtr.Zero)
            {
                throw new ArgumentException();
            }
            this.FAbsolutePidl = absolutePidl;
        }

        public ShellItem(IShellFolder folder, IntPtr relativePidl)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folfer");
            }
            if (relativePidl == IntPtr.Zero)
            {
                throw new ArgumentException("relativePidl == IntPtr.Zero", "relativePidl");
            }
            this.FAbsolutePidl = GetAbsolutePidl(folder, relativePidl);
        }

        public ShellItem(IntPtr hwndOwner, string absoluteName)
        {
            if (absoluteName == null)
            {
                throw new ArgumentNullException("absoluteName");
            }
            if (absoluteName == string.Empty)
            {
                throw new ArgumentException("absoluteName is empty");
            }
            this.FAbsolutePidl = GetAbsolutePidl(hwndOwner, absoluteName);
        }

        protected ShellItem(SerializationInfo info, StreamingContext context)
        {
            byte[] source = (byte[]) info.GetValue("Pidl", typeof(byte[]));
            this.AbsolutePidl = Marshal.AllocCoTaskMem(source.Length);
            Marshal.Copy(source, 0, this.AbsolutePidl, source.Length);
        }

        public ShellItem(IShellFolder folder, IntPtr hwndOwner, string relativeName)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }
            if (relativeName == null)
            {
                throw new ArgumentNullException("relativeName");
            }
            if (relativeName == string.Empty)
            {
                throw new ArgumentException("relativeName is empty");
            }
            IntPtr relativePidl = folder.ParseDisplayName(hwndOwner, relativeName);
            try
            {
                this.FAbsolutePidl = GetAbsolutePidl(folder, relativePidl);
            }
            finally
            {
                Marshal.FreeCoTaskMem(relativePidl);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (this.FAbsolutePidl != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(this.FAbsolutePidl);
                this.FAbsolutePidl = IntPtr.Zero;
            }
            this.FRelativePidl = IntPtr.Zero;
        }

        ~ShellItem()
        {
            this.Dispose(false);
        }

        private static IntPtr GetAbsolutePidl(IShellFolder folder, IntPtr relativePidl)
        {
            IntPtr ptr;
            IShellFolder desktopFolder = GetDesktopFolder();
            try
            {
                string lpszDisplayName = folder.GetDisplayNameOf(relativePidl, SHGNO.SHGDN_FORPARSING);
                ptr = desktopFolder.ParseDisplayName(IntPtr.Zero, lpszDisplayName);
            }
            finally
            {
                Marshal.ReleaseComObject(desktopFolder);
            }
            return ptr;
        }

        private static IntPtr GetAbsolutePidl(IntPtr hwndOwner, string absolutePath)
        {
            IntPtr ptr2;
            if (absolutePath.StartsWith("::", StringComparison.Ordinal) && absolutePath.EndsWith(@"\", StringComparison.Ordinal))
            {
                absolutePath = absolutePath.Substring(0, absolutePath.Length - 1);
            }
            if (OS.IsWinXP && (hwndOwner == IntPtr.Zero))
            {
                IntPtr ptr;
                SFGAO sfgao;
                int errorCode = Shell32.SHParseDisplayName(absolutePath, IntPtr.Zero, out ptr, 0, out sfgao);
                if (HRESULT.FAILED(errorCode))
                {
                    throw Marshal.GetExceptionForHR(errorCode);
                }
                return ptr;
            }
            IShellFolder desktopFolder = GetDesktopFolder();
            try
            {
                ptr2 = desktopFolder.ParseDisplayName(hwndOwner, absolutePath);
            }
            finally
            {
                Marshal.ReleaseComObject(desktopFolder);
            }
            return ptr2;
        }

        public static IShellFolder GetDesktopFolder()
        {
            IShellFolder folder;
            int errorCode = Shell32.SHGetDesktopFolder(out folder);
            if (errorCode != 0)
            {
                throw Marshal.GetExceptionForHR(errorCode);
            }
            return folder;
        }

        public IShellFolder GetFolder()
        {
            IShellFolder folder2;
            if (this.AbsolutePidl == this.RelativePidl)
            {
                return GetDesktopFolder();
            }
            IShellFolder desktopFolder = GetDesktopFolder();
            try
            {
                short val = Marshal.ReadInt16(this.RelativePidl);
                try
                {
                    Marshal.WriteInt16(this.RelativePidl, (short) 0);
                    folder2 = desktopFolder.BindToFolder(this.AbsolutePidl);
                }
                finally
                {
                    Marshal.WriteInt16(this.RelativePidl, val);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(desktopFolder);
            }
            return folder2;
        }

        public IShellItem GetItem()
        {
            int num;
            IShellItem item2;
            if (!OS.IsWin2k)
            {
                throw new PlatformNotSupportedException();
            }
            if (OS.IsWinVista)
            {
                object obj2;
                num = Shell32.SHCreateItemFromIDList(this.AbsolutePidl, typeof(IShellItem).GUID, out obj2);
                if (HRESULT.FAILED(num))
                {
                    throw Marshal.GetExceptionForHR(num);
                }
                return (IShellItem) obj2;
            }
            IShellFolder psfParent = this.GetFolder();
            try
            {
                IShellItem item;
                num = Shell32.SHCreateShellItem(IntPtr.Zero, psfParent, this.RelativePidl, out item);
                if (HRESULT.FAILED(num))
                {
                    throw Marshal.GetExceptionForHR(num);
                }
                item2 = item;
            }
            finally
            {
                Marshal.ReleaseComObject(psfParent);
            }
            return item2;
        }

        public static IShellItem GetItem(string absolutePath)
        {
            int num;
            IShellItem item2;
            if (!OS.IsWin2k)
            {
                throw new PlatformNotSupportedException();
            }
            if (OS.IsWinVista)
            {
                object obj2;
                num = Shell32.SHCreateItemFromParsingName(absolutePath, IntPtr.Zero, typeof(IShellItem).GUID, out obj2);
                if (HRESULT.FAILED(num))
                {
                    throw Marshal.GetExceptionForHR(num);
                }
                return (IShellItem) obj2;
            }
            IntPtr absolutePidl = GetAbsolutePidl(IntPtr.Zero, absolutePath);
            try
            {
                IShellItem item;
                if (ITEMIDLIST.GetDepth(absolutePidl) == 1)
                {
                    IShellFolder desktopFolder = GetDesktopFolder();
                    try
                    {
                        num = Shell32.SHCreateShellItem(IntPtr.Zero, desktopFolder, absolutePidl, out item);
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(desktopFolder);
                    }
                }
                else
                {
                    IntPtr lastPidl = ITEMIDLIST.GetLastPidl(absolutePidl);
                    IntPtr parent = GetParent(absolutePidl);
                    try
                    {
                        num = Shell32.SHCreateShellItem(parent, null, lastPidl, out item);
                    }
                    finally
                    {
                        Marshal.FreeCoTaskMem(parent);
                    }
                }
                if (HRESULT.FAILED(num))
                {
                    throw Marshal.GetExceptionForHR(num);
                }
                item2 = item;
            }
            finally
            {
                Marshal.FreeCoTaskMem(absolutePidl);
            }
            return item2;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Pidl", ITEMIDLIST.FromPidl(this.AbsolutePidl).ToByteArray());
        }

        public ShellItem GetParent()
        {
            IntPtr parent = GetParent(this.AbsolutePidl);
            if (parent != IntPtr.Zero)
            {
                return new ShellItem(parent);
            }
            return null;
        }

        protected static IntPtr GetParent(IntPtr absolutePidl)
        {
            IntPtr ptr2;
            ITEMIDLIST itemidlist = ITEMIDLIST.FromPidl(absolutePidl);
            if (itemidlist.mkid.Length < 2)
            {
                return IntPtr.Zero;
            }
            int depth = itemidlist.mkid.Length - 1;
            IntPtr pidl = Marshal.AllocCoTaskMem(itemidlist.GetSize(depth));
            try
            {
                itemidlist.ToPidl(pidl, depth);
                ptr2 = pidl;
            }
            catch
            {
                Marshal.FreeCoTaskMem(pidl);
                throw;
            }
            return ptr2;
        }

        public IntPtr AbsolutePidl
        {
            get
            {
                return this.FAbsolutePidl;
            }
            set
            {
                if (value == IntPtr.Zero)
                {
                    throw new ArgumentException();
                }
                if (this.FAbsolutePidl != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(this.FAbsolutePidl);
                }
                this.FAbsolutePidl = value;
            }
        }

        public IntPtr RelativePidl
        {
            get
            {
                if (this.FRelativePidl == IntPtr.Zero)
                {
                    if (ITEMIDLIST.GetDepth(this.AbsolutePidl) == 1)
                    {
                        this.FRelativePidl = this.AbsolutePidl;
                    }
                    else
                    {
                        this.FRelativePidl = ITEMIDLIST.GetLastPidl(this.AbsolutePidl);
                    }
                }
                return this.FRelativePidl;
            }
        }
    }
}

