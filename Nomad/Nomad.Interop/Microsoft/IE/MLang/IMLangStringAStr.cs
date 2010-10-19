namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("C04D65D2-B70D-11D0-B188-00AA0038C969"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComConversionLoss]
    public interface IMLangStringAStr
    {
        void Sync(int fNoAccess);
        int GetLength();
        void SetMLStr(int lDestPos, int lDestLen, [MarshalAs(UnmanagedType.IUnknown)] object pSrcMLStr, int lSrcPos, int lSrcLen);
        void GetMLStr(int lSrcPos, int lSrcLen, [MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, uint dwClsContext, [In, MarshalAs(UnmanagedType.LPStruct)] Guid piid, [MarshalAs(UnmanagedType.IUnknown)] out object ppDestMLStr, out int plDestPos, out int plDestLen);
        void SetAStr(int lDestPos, int lDestLen, uint uCodePage, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] byte[] pszSrc, int cchSrc, out int pcchActual, out int plActualLen);
        void SetStrBufA(int lDestPos, int lDestLen, uint uCodePage, [MarshalAs(UnmanagedType.Interface)] IMLangStringBufA pSrcBuf, out int pcchActual, out int plActualLen);
        void GetAStr(int lSrcPos, int lSrcLen, uint uCodePageIn, out uint puCodePageOut, out sbyte pszDest, int cchDest, out int pcchActual, out int plActualLen);
        void GetStrBufA(int lSrcPos, int lSrcMaxLen, out uint puDestCodePage, [MarshalAs(UnmanagedType.Interface)] out IMLangStringBufA ppDestBuf, out int plDestLen);
        void LockAStr(int lSrcPos, int lSrcLen, int lFlags, uint uCodePageIn, int cchRequest, out uint puCodePageOut, [Out] IntPtr ppszDest, out int pcchDest, out int plDestLen);
        void UnlockAStr([MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] byte[] pszSrc, int cchSrc, out int pcchActual, out int plActualLen);
        void SetLocale(int lDestPos, int lDestLen, uint locale);
        void GetLocale(int lSrcPos, int lSrcMaxLen, out uint plocale, out int plLocalePos, out int plLocaleLen);
    }
}

