namespace Microsoft.COM
{
    using Microsoft.Win32;
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("B196B288-BAB4-101A-B69C-00AA00341D07"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleControl
    {
        void GetControlInfo(ref CONTROLINFO pCI);
        void OnMnemonic([In] ref Microsoft.Win32.MSG pMsg);
        void OnAmbientPropertyChange(int dispID);
        void FreezeEvents([MarshalAs(UnmanagedType.Bool)] bool bFreeze);
    }
}

