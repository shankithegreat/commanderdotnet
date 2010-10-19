namespace Microsoft.COM
{
    using Microsoft.Win32;
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Security;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SecurityCritical(SecurityCriticalScope.Everything), Guid("00000112-0000-0000-C000-000000000046")]
    public interface IOleObject
    {
        [PreserveSig]
        int SetClientSite([MarshalAs(UnmanagedType.Interface)] IOleClientSite pClientSite);
        void GetClientSite(out IOleClientSite ppClientSite);
        [PreserveSig]
        int SetHostNames([MarshalAs(UnmanagedType.LPWStr)] string szContainerApp, [MarshalAs(UnmanagedType.LPWStr)] string szContainerObj);
        [PreserveSig]
        int Close(int dwSaveOption);
        [PreserveSig]
        int SetMoniker([MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] IMoniker pmk);
        [PreserveSig]
        int GetMoniker([MarshalAs(UnmanagedType.U4)] int dwAssign, [MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out IMoniker moniker);
        [PreserveSig]
        int InitFromData([MarshalAs(UnmanagedType.Interface)] IDataObject pDataObject, int fCreation, [MarshalAs(UnmanagedType.U4)] int dwReserved);
        [PreserveSig]
        int GetClipboardData([MarshalAs(UnmanagedType.U4)] int dwReserved, out IDataObject data);
        [PreserveSig]
        int DoVerb(int iVerb, IntPtr lpmsg, [MarshalAs(UnmanagedType.Interface)] object pActiveSite, int lindex, IntPtr hwndParent, RECT lprcPosRect);
        [PreserveSig]
        int EnumVerbs(out object e);
        [PreserveSig]
        int OleUpdate();
        [PreserveSig]
        int IsUpToDate();
        [PreserveSig]
        int GetUserClassID(out Guid pClsid);
        [PreserveSig]
        int GetUserType([MarshalAs(UnmanagedType.U4)] int dwFormOfType, [MarshalAs(UnmanagedType.LPWStr)] out string userType);
        [PreserveSig]
        int SetExtent([MarshalAs(UnmanagedType.U4)] int dwDrawAspect, SIZE pSizel);
        [PreserveSig]
        int GetExtent([MarshalAs(UnmanagedType.U4)] int dwDrawAspect, [In, Out] ref SIZE pSizel);
        [PreserveSig]
        int Advise(IAdviseSink pAdvSink, out int cookie);
        [PreserveSig]
        int Unadvise([MarshalAs(UnmanagedType.U4)] int dwConnection);
        [PreserveSig]
        int EnumAdvise(out IEnumSTATDATA e);
        [PreserveSig]
        int GetMiscStatus([MarshalAs(UnmanagedType.U4)] int dwAspect, out int misc);
        void SetColorScheme([In] ref LOGPALETTE pLogpal);
    }
}

