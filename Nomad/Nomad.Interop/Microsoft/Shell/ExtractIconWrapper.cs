namespace Microsoft.Shell
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ExtractIconWrapper : IDisposable
    {
        private IExtractIconW ExtractW;
        private string FIconFile;
        private GIL_OUT FIconFlags;
        private int FIconIndex;

        public ExtractIconWrapper(IExtractIconW ExtractW)
        {
            if (ExtractW == null)
            {
                throw new ArgumentNullException("ExtractW");
            }
            this.ExtractW = ExtractW;
        }

        public void Dispose()
        {
            if (this.ExtractW != null)
            {
                Marshal.ReleaseComObject(this.ExtractW);
                this.ExtractW = null;
            }
        }

        public bool Extract(out IntPtr phiconLarge, out IntPtr phiconSmall, uint nIconSize)
        {
            if (this.FIconFile == null)
            {
                throw new ApplicationException("IconFile is empty. Call GetIconLocation first");
            }
            return (this.ExtractW.Extract(this.FIconFile, this.FIconIndex, out phiconLarge, out phiconSmall, nIconSize) == 0);
        }

        public static ExtractIconWrapper FromShellFolder(IShellFolder folder, IntPtr pidl)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }
            if (pidl == IntPtr.Zero)
            {
                throw new ArgumentException("pidl == IntPtr.Zero", "pidl");
            }
            try
            {
                object obj2;
                IntPtr[] apidl = new IntPtr[] { pidl };
                folder.GetUIObjectOf(IntPtr.Zero, 1, apidl, typeof(IExtractIconW).GUID, IntPtr.Zero, out obj2);
                if (obj2 != null)
                {
                    return new ExtractIconWrapper((IExtractIconW) obj2);
                }
            }
            catch (FileNotFoundException)
            {
            }
            return null;
        }

        public bool GetIconLocation(GIL_IN uFlags)
        {
            StringBuilder szIconFile = new StringBuilder(260);
            bool flag = this.ExtractW.GetIconLocation(uFlags, szIconFile, (uint) szIconFile.Capacity, out this.FIconIndex, out this.FIconFlags) == 0;
            if (flag)
            {
                this.FIconFile = szIconFile.ToString();
            }
            return flag;
        }

        public string IconFile
        {
            get
            {
                return this.FIconFile;
            }
        }

        public GIL_OUT IconFlags
        {
            get
            {
                return this.FIconFlags;
            }
        }

        public int IconIndex
        {
            get
            {
                return this.FIconIndex;
            }
        }
    }
}

