namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("F5BE2EE1-BFD7-11D0-B188-00AA0038C969"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMLangLineBreakConsole
    {
        void BreakLineML([MarshalAs(UnmanagedType.Interface)] IMLangString pSrcMLStr, int lSrcPos, int lSrcLen, int cMinColumns, int cMaxColumns, out int plLineLen, out int plSkipLen);
        void BreakLineW(uint locale, [MarshalAs(UnmanagedType.LPWStr)] string pszSrc, int cchSrc, int cMaxColumns, out int pcchLine, out int pcchSkip);
        void BreakLineA(uint locale, uint uCodePage, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] byte[] pszSrc, int cchSrc, int cMaxColumns, out int pcchLine, out int pcchSkip);
    }
}

