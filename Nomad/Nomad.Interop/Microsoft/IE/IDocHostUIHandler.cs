namespace Microsoft.IE
{
    using Microsoft.Win32;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [ComImport, Guid("bd3f23c0-d43e-11cf-893b-00aa00bdce1a"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDocHostUIHandler
    {
        [PreserveSig]
        uint ShowContextMenu(uint dwID, ref Point ppt, [MarshalAs(UnmanagedType.IUnknown)] object pcmdtReserved, [MarshalAs(UnmanagedType.IDispatch)] object pdispReserved);
        void GetHostInfo(ref DOCHOSTUIINFO pInfo);
        void ShowUI(uint dwID, ref object pActiveObject, ref object pCommandTarget, ref object pFrame, ref object pDoc);
        void HideUI();
        void UpdateUI();
        void EnableModeless(int fEnable);
        void OnDocWindowActivate(int fActivate);
        void OnFrameWindowActivate(int fActivate);
        void ResizeBorder(ref Microsoft.Win32.RECT prcBorder, int pUIWindow, int fFrameWindow);
        [PreserveSig]
        uint TranslateAccelerator(ref Microsoft.Win32.MSG lpMsg, ref Guid pguidCmdGroup, uint nCmdID);
        void GetOptionKeyPath([MarshalAs(UnmanagedType.BStr)] ref string pchKey, uint dw);
        uint GetDropTarget(int pDropTarget, ref int ppDropTarget);
        [PreserveSig]
        void GetExternal([MarshalAs(UnmanagedType.IDispatch)] out object ppDispatch);
        [PreserveSig]
        uint TranslateUrl(uint dwTranslate, [MarshalAs(UnmanagedType.BStr)] string pchURLIn, [MarshalAs(UnmanagedType.BStr)] ref string ppchURLOut);
        IDataObject FilterDataObject(IDataObject pDO);
    }
}

