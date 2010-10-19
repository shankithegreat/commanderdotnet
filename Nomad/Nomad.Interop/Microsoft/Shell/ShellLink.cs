namespace Microsoft.Shell
{
    using Microsoft.COM;
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;
    using System.Windows.Forms;

    public class ShellLink : ICloneable, IDisposable
    {
        private IShellLinkA LinkA;
        private IShellLinkW LinkW;
        private Microsoft.COM.IPersistStream PersistStream;

        public ShellLink()
        {
            this.Initialize();
        }

        public ShellLink(Stream source)
        {
            this.Initialize();
            this.Load(source);
        }

        public ShellLink(string fileName)
        {
            this.Initialize();
            this.Load(fileName);
        }

        public void AddBlock(uint signature, byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.AddBlock(signature, data, 0, data.Length);
        }

        public void AddBlock(uint signature, byte[] data, int startIndex, int dataLength)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            IShellLinkDataList persistStream = this.PersistStream as IShellLinkDataList;
            if (persistStream == null)
            {
                throw new NotSupportedException();
            }
            int num = Marshal.SizeOf(typeof(DATABLOCKHEADER));
            DATABLOCKHEADER structure = new DATABLOCKHEADER {
                cbSize = num + dataLength,
                dwSignature = signature
            };
            IntPtr ptr = Windows.LocalAlloc(LMEM.NONZEROLPTR, structure.cbSize);
            try
            {
                Marshal.StructureToPtr(structure, ptr, false);
                IntPtr destination = new IntPtr(ptr.ToInt64() + num);
                Marshal.Copy(data, startIndex, destination, dataLength);
                persistStream.AddDataBlock(ptr);
            }
            catch
            {
                Windows.LocalFree(ptr);
                throw;
            }
        }

        public virtual object Clone()
        {
            ShellLink link = (ShellLink) base.MemberwiseClone();
            link.Initialize();
            using (MemoryStream stream = new MemoryStream())
            {
                this.Save(stream, false);
                stream.Seek(0L, SeekOrigin.Begin);
                link.Load(stream);
            }
            return link;
        }

        public void Dispose()
        {
            if (this.PersistStream != null)
            {
                Marshal.FinalReleaseComObject(this.PersistStream);
            }
            this.LinkA = null;
            this.LinkW = null;
            this.PersistStream = null;
        }

        public byte[] GetBlock(uint signature)
        {
            IntPtr ptr;
            IShellLinkDataList persistStream = this.PersistStream as IShellLinkDataList;
            if ((persistStream != null) && HRESULT.SUCCEEDED(persistStream.CopyDataBlock(signature, out ptr)))
            {
                try
                {
                    int num = Marshal.ReadInt32(ptr);
                    int num2 = Marshal.SizeOf(typeof(DATABLOCKHEADER));
                    byte[] destination = new byte[num - num2];
                    IntPtr source = new IntPtr(ptr.ToInt64() + num2);
                    Marshal.Copy(source, destination, 0, destination.Length);
                    return destination;
                }
                finally
                {
                    Windows.LocalFree(ptr);
                }
            }
            return null;
        }

        public void GetIconLocation(out string iconPath, out int iconIndex)
        {
            StringBuilder pszIconPath = new StringBuilder(0x400);
            if (this.LinkW != null)
            {
                this.LinkW.GetIconLocation(pszIconPath, pszIconPath.Capacity, out iconIndex);
                iconPath = pszIconPath.ToString();
            }
            else
            {
                if (this.LinkA == null)
                {
                    throw new ObjectDisposedException("ShellLink");
                }
                this.LinkA.GetIconLocation(pszIconPath, pszIconPath.Capacity, out iconIndex);
                iconPath = pszIconPath.ToString();
            }
        }

        private void Initialize()
        {
            object obj2 = new CoShellLink();
            this.LinkW = obj2 as IShellLinkW;
            if (this.LinkW == null)
            {
                this.LinkA = obj2 as IShellLinkA;
                if (this.LinkA == null)
                {
                    throw new NotSupportedException("IShellLink");
                }
            }
            this.PersistStream = obj2 as Microsoft.COM.IPersistStream;
            if (this.PersistStream == null)
            {
                throw new NotSupportedException("IPersistStream");
            }
        }

        private bool InternalSetFlags(SHELL_LINK_DATA_FLAGS flags, bool value)
        {
            SHELL_LINK_DATA_FLAGS shell_link_data_flags2;
            IShellLinkDataList persistStream = this.PersistStream as IShellLinkDataList;
            if (persistStream == null)
            {
                return false;
            }
            SHELL_LINK_DATA_FLAGS shell_link_data_flags = persistStream.GetFlags();
            if (value)
            {
                shell_link_data_flags2 = shell_link_data_flags | flags;
            }
            else
            {
                shell_link_data_flags2 = shell_link_data_flags & ~flags;
            }
            if (shell_link_data_flags2 != shell_link_data_flags)
            {
                persistStream.SetFlags(shell_link_data_flags2);
            }
            return true;
        }

        public void Load(Stream source)
        {
            if (this.PersistStream == null)
            {
                throw new ObjectDisposedException("ShellLink");
            }
            this.PersistStream.Load(new ComStream(source));
        }

        public void Load(string fileName)
        {
            if (this.PersistStream == null)
            {
                throw new ObjectDisposedException("ShellLink");
            }
            IPersistFile persistStream = this.PersistStream as IPersistFile;
            if (persistStream != null)
            {
                persistStream.Load(fileName, 0);
            }
            else
            {
                using (Stream stream = File.OpenRead(fileName))
                {
                    this.Load(stream);
                }
            }
        }

        public void RemoveDataBlock(uint signature)
        {
            IShellLinkDataList persistStream = this.PersistStream as IShellLinkDataList;
            if (persistStream != null)
            {
                persistStream.RemoveDataBlock(signature);
            }
        }

        public void Resolve(IWin32Window owner, SLR flags)
        {
            IntPtr hwnd = (owner != null) ? owner.Handle : IntPtr.Zero;
            if (this.LinkW != null)
            {
                this.LinkW.Resolve(hwnd, flags);
            }
            else
            {
                if (this.LinkA == null)
                {
                    throw new ObjectDisposedException("ShellLink");
                }
                this.LinkA.Resolve(hwnd, flags);
            }
        }

        public void Save(Stream dest)
        {
            this.Save(dest, true);
        }

        public void Save(string fileName)
        {
            if (this.PersistStream == null)
            {
                throw new ObjectDisposedException("ShellLink");
            }
            IPersistFile persistStream = this.PersistStream as IPersistFile;
            if (persistStream != null)
            {
                persistStream.Save(fileName, true);
                persistStream.SaveCompleted(fileName);
            }
            else
            {
                using (Stream stream = File.Create(fileName))
                {
                    this.Save(stream, true);
                }
            }
        }

        public void Save(Stream dest, bool resetModified)
        {
            if (this.PersistStream == null)
            {
                throw new ObjectDisposedException("ShellLink");
            }
            this.PersistStream.Save(new ComStream(dest), resetModified);
        }

        public void SetFlags(SHELL_LINK_DATA_FLAGS flags, bool value)
        {
            if (!this.InternalSetFlags(flags, value))
            {
                throw new NotSupportedException();
            }
        }

        public void SetIconLocation(string iconPath, int iconIndex)
        {
            if (this.LinkW != null)
            {
                this.LinkW.SetIconLocation(iconPath, iconIndex);
            }
            else
            {
                if (this.LinkA == null)
                {
                    throw new ObjectDisposedException("ShellLink");
                }
                this.LinkA.SetIconLocation(iconPath, iconIndex);
            }
        }

        public string Arguments
        {
            get
            {
                StringBuilder pszArgs = new StringBuilder(0x400);
                if (this.LinkW != null)
                {
                    this.LinkW.GetArguments(pszArgs, pszArgs.Capacity);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.GetArguments(pszArgs, pszArgs.Capacity);
                }
                return pszArgs.ToString();
            }
            set
            {
                if (this.LinkW != null)
                {
                    this.LinkW.SetArguments(value);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.SetArguments(value);
                }
            }
        }

        public string Description
        {
            get
            {
                StringBuilder pszName = new StringBuilder(0x400);
                if (this.LinkW != null)
                {
                    this.LinkW.GetDescription(pszName, pszName.Capacity);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.GetDescription(pszName, pszName.Capacity);
                }
                return pszName.ToString();
            }
            set
            {
                if (this.LinkW != null)
                {
                    this.LinkW.SetDescription(value);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.SetDescription(value);
                }
            }
        }

        public SHELL_LINK_DATA_FLAGS Flags
        {
            get
            {
                IShellLinkDataList persistStream = this.PersistStream as IShellLinkDataList;
                if (persistStream == null)
                {
                    return 0;
                }
                return persistStream.GetFlags();
            }
            set
            {
                IShellLinkDataList persistStream = this.PersistStream as IShellLinkDataList;
                if (persistStream == null)
                {
                    throw new NotSupportedException();
                }
                persistStream.SetFlags(value);
            }
        }

        public Keys Hotkey
        {
            get
            {
                short num;
                if (this.LinkW != null)
                {
                    this.LinkW.GetHotkey(out num);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.GetHotkey(out num);
                }
                return WindowsWrapper.HotkeyToShortcutKeys(num);
            }
            set
            {
                HOTKEYF hotkeyf = 0;
                if ((value & Keys.Shift) > Keys.None)
                {
                    hotkeyf = (HOTKEYF) ((byte) (hotkeyf | HOTKEYF.HOTKEYF_SHIFT));
                }
                if ((value & Keys.Control) > Keys.None)
                {
                    hotkeyf = (HOTKEYF) ((byte) (hotkeyf | HOTKEYF.HOTKEYF_CONTROL));
                }
                if ((value & Keys.Alt) > Keys.None)
                {
                    hotkeyf = (HOTKEYF) ((byte) (hotkeyf | HOTKEYF.HOTKEYF_ALT));
                }
                short wHotkey = (short) ((value & Keys.KeyCode) | (((byte) hotkeyf) << 8));
                if (this.LinkW != null)
                {
                    this.LinkW.SetHotkey(wHotkey);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.SetHotkey(wHotkey);
                }
            }
        }

        public IntPtr IdList
        {
            get
            {
                IntPtr ptr;
                if (this.LinkW != null)
                {
                    this.LinkW.GetIDList(out ptr);
                    return ptr;
                }
                if (this.LinkA == null)
                {
                    throw new ObjectDisposedException("ShellLink");
                }
                this.LinkA.GetIDList(out ptr);
                return ptr;
            }
            set
            {
                if (this.LinkW != null)
                {
                    this.LinkW.SetIDList(value);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.SetIDList(value);
                }
            }
        }

        public bool Modified
        {
            get
            {
                if (this.PersistStream == null)
                {
                    throw new ObjectDisposedException("ShellLink");
                }
                return (this.PersistStream.IsDirty() != 1);
            }
        }

        public object NativeObject
        {
            get
            {
                if (this.PersistStream == null)
                {
                    throw new ObjectDisposedException("ShellLink");
                }
                return this.PersistStream;
            }
        }

        public string Path
        {
            get
            {
                Microsoft.Win32.WIN32_FIND_DATA win_find_data;
                StringBuilder pszFile = new StringBuilder(260);
                if (this.LinkW != null)
                {
                    this.LinkW.GetPath(pszFile, pszFile.Capacity, out win_find_data, SLGP.SLGP_UNCPRIORITY);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.GetPath(pszFile, pszFile.Capacity, out win_find_data, SLGP.SLGP_UNCPRIORITY);
                }
                return pszFile.ToString();
            }
            set
            {
                if (this.LinkW != null)
                {
                    this.LinkW.SetPath(value);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.SetPath(value);
                }
            }
        }

        public string Title
        {
            get
            {
                string str;
                if (this.PersistStream == null)
                {
                    throw new ObjectDisposedException("ShellLink");
                }
                IPropertyStore persistStream = this.PersistStream as IPropertyStore;
                if (persistStream == null)
                {
                    throw new PlatformNotSupportedException();
                }
                PropVariant pv = new PropVariant();
                try
                {
                    PropertyKey pkey = new PropertyKey(PropertyKey.PropertySet, 2);
                    persistStream.GetValue(ref pkey, out pv);
                    str = (string) pv.Value;
                }
                finally
                {
                    pv.Clear();
                }
                return str;
            }
            set
            {
                if (this.PersistStream == null)
                {
                    throw new ObjectDisposedException("ShellLink");
                }
                IPropertyStore persistStream = this.PersistStream as IPropertyStore;
                if (persistStream == null)
                {
                    throw new PlatformNotSupportedException();
                }
                PropVariant pv = new PropVariant(value);
                try
                {
                    PropertyKey pkey = new PropertyKey(PropertyKey.PropertySet, 2);
                    persistStream.SetValue(ref pkey, ref pv);
                    persistStream.Commit();
                }
                finally
                {
                    pv.Clear();
                }
            }
        }

        public FormWindowState WindowState
        {
            get
            {
                SW sw;
                if (this.LinkW != null)
                {
                    this.LinkW.GetShowCmd(out sw);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.GetShowCmd(out sw);
                }
                switch (sw)
                {
                    case SW.SW_SHOWMINIMIZED:
                        return FormWindowState.Minimized;

                    case SW.SW_SHOWMAXIMIZED:
                        return FormWindowState.Maximized;
                }
                return FormWindowState.Normal;
            }
            set
            {
                SW sw;
                switch (value)
                {
                    case FormWindowState.Normal:
                        sw = SW.SW_SHOWNORMAL;
                        break;

                    case FormWindowState.Minimized:
                        sw = SW.SW_SHOWMINIMIZED;
                        break;

                    case FormWindowState.Maximized:
                        sw = SW.SW_SHOWMAXIMIZED;
                        break;

                    default:
                        throw new InvalidEnumArgumentException();
                }
                if (this.LinkW != null)
                {
                    this.LinkW.SetShowCmd(sw);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.SetShowCmd(sw);
                }
            }
        }

        public string WorkingDirectory
        {
            get
            {
                StringBuilder pszDir = new StringBuilder(0x400);
                if (this.LinkW != null)
                {
                    this.LinkW.GetWorkingDirectory(pszDir, pszDir.Capacity);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.GetWorkingDirectory(pszDir, pszDir.Capacity);
                }
                return pszDir.ToString();
            }
            set
            {
                if (this.LinkW != null)
                {
                    this.LinkW.SetWorkingDirectory(value);
                }
                else
                {
                    if (this.LinkA == null)
                    {
                        throw new ObjectDisposedException("ShellLink");
                    }
                    this.LinkA.SetWorkingDirectory(value);
                }
            }
        }
    }
}

