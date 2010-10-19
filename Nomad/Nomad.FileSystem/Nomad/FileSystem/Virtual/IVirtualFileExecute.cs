namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;
    using System.Diagnostics;
    using System.Security;
    using System.Windows.Forms;

    public interface IVirtualFileExecute : IVirtualFile, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        Process Execute(IWin32Window owner);
        Process ExecuteEx(IWin32Window owner, string arguments, ExecuteAsUser runAs, string userName, SecureString password);

        bool CanExecuteEx { get; }
    }
}

