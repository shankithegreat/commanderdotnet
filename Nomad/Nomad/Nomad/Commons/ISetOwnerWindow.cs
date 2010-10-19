namespace Nomad.Commons
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ComVisible(false)]
    public interface ISetOwnerWindow
    {
        IWin32Window Owner { get; set; }
    }
}

