namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("C04D65CE-B70D-11D0-B188-00AA0038C969")]
    public interface IMLangString
    {
        void Sync(int fNoAccess);
        int GetLength();
        void SetMLStr(int lDestPos, int lDestLen, [MarshalAs(UnmanagedType.IUnknown)] object pSrcMLStr, int lSrcPos, int lSrcLen);
        void GetMLStr(int lSrcPos, int lSrcLen, [MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, uint dwClsContext, [In, MarshalAs(UnmanagedType.LPStruct)] Guid piid, [MarshalAs(UnmanagedType.IUnknown)] out object ppDestMLStr, out int plDestPos, out int plDestLen);
    }
}

