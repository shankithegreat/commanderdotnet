namespace Nomad.FileSystem.Shell
{
    using Microsoft.Shell;
    using Microsoft.Win32;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Windows.Forms;

    [Serializable]
    public class ShellFile : CustomShellItem, IVirtualFileExecute, IVirtualFile, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        protected ShellFile(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        internal ShellFile(SafeShellItem item, SFGAO attributes, IVirtualFolder parent) : base(item, attributes, parent)
        {
        }

        public Process Execute(IWin32Window owner)
        {
            SHELLEXECUTEINFO shellexecuteinfo;
            shellexecuteinfo = new SHELLEXECUTEINFO {
                cbSize = Marshal.SizeOf(shellexecuteinfo),
                fMask = 12,
                lpIDList = base.ItemInfo.AbsolutePidl,
                nShow = SW.SW_SHOW
            };
            if (owner != null)
            {
                shellexecuteinfo.hwnd = owner.Handle;
            }
            if (!Microsoft.Shell.Shell32.ShellExecuteEx(ref shellexecuteinfo))
            {
                throw new Win32Exception();
            }
            return null;
        }

        public Process ExecuteEx(IWin32Window owner, string arguments, ExecuteAsUser runAs, string userName, SecureString password)
        {
            throw new NotImplementedException();
        }

        public bool CanExecuteEx
        {
            get
            {
                return false;
            }
        }
    }
}

