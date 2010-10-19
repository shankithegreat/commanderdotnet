namespace Microsoft.COM
{
    using Microsoft.Win32;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [ComImport, Guid("0000010d-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IViewObject
    {
        void Draw(DVASPECT2 dwDrawAspect, int lindex, [In] ref DVASPECTINFO pvAspect, [In] ref DVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, [In] ref Rectangle lprcBounds, [In] ref Rectangle lprcWBounds, IntPtr pfnContinue, int dwContinue);
        [PreserveSig]
        int GetColorSet(DVASPECT2 dwDrawAspect, int lindex, IntPtr pvAspect, DVTARGETDEVICE ptd, IntPtr hicTargetDev, out LOGPALETTE ppColorSet);
        [PreserveSig]
        int Freeze(DVASPECT2 dwDrawAspect, int lindex, IntPtr pvAspect, out IntPtr pdwFreeze);
        [PreserveSig]
        int Unfreeze(IntPtr dwFreeze);
        [PreserveSig]
        int SetAdvise(DVASPECT2 dwAspect, int advf, [MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink);
        void GetAdvise(out DVASPECT2 pdwAspect, out int advf, out IAdviseSink pAdvSink);
    }
}

