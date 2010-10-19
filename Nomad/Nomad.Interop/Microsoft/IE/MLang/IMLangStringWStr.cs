namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("C04D65D0-B70D-11D0-B188-00AA0038C969"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComConversionLoss]
    public interface IMLangStringWStr
    {
        void Sync(int fNoAccess);
        int GetLength();
        void SetMLStr(int lDestPos, int lDestLen, [MarshalAs(UnmanagedType.IUnknown)] object pSrcMLStr, int lSrcPos, int lSrcLen);
        void GetMLStr(int lSrcPos, int lSrcLen, [MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, uint dwClsContext, [In, MarshalAs(UnmanagedType.LPStruct)] Guid piid, [MarshalAs(UnmanagedType.IUnknown)] out object ppDestMLStr, out int plDestPos, out int plDestLen);
        void SetWStr(int lDestPos, int lDestLen, [MarshalAs(UnmanagedType.LPWStr)] string pszSrc, int cchSrc, out int pcchActual, out int plActualLen);
        void SetStrBufW(int lDestPos, int lDestLen, [MarshalAs(UnmanagedType.Interface)] IMLangStringBufW pSrcBuf, out int pcchActual, out int plActualLen);
        void GetWStr(int lSrcPos, int lSrcLen, out ushort pszDest, [In] int cchDest, out int pcchActual, out int plActualLen);
        void GetStrBufW(int lSrcPos, int lSrcMaxLen, [MarshalAs(UnmanagedType.Interface)] out IMLangStringBufW ppDestBuf, out int plDestLen);
        void LockWStr(int lSrcPos, int lSrcLen, int lFlags, int cchRequest, [Out] IntPtr ppszDest, out int pcchDest, out int plDestLen);
        void UnlockWStr([In] ref ushort pszSrc, [In] int cchSrc, out int pcchActual, out int plActualLen);
        void SetLocale(int lDestPos, int lDestLen, uint locale);
        void GetLocale(int lSrcPos, int lSrcMaxLen, out uint plocale, out int plLocalePos, out int plLocaleLen);
    }
}

