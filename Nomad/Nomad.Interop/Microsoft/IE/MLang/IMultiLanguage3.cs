namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;

    [ComImport, Guid("4E5868AB-B157-4623-9ACC-6A1D9CAEBE04"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMultiLanguage3
    {
        uint GetNumberOfCodePageInfo();
        [PreserveSig]
        int GetCodePageInfo(uint uiCodePage, ushort LangId, out MIMECPINFO pCodePageInfo);
        [PreserveSig]
        int GetFamilyCodePage(uint uiCodePage, out uint puiFamilyCodePage);
        void EnumCodePages(MIMECONTF grfFlags, ushort LangId, [MarshalAs(UnmanagedType.Interface)] out IEnumCodePage ppEnumCodePage);
        [PreserveSig]
        int GetCharsetInfo([MarshalAs(UnmanagedType.BStr)] string Charset, out MIMECSETINFO pCharsetInfo);
        [PreserveSig]
        int IsConvertible(uint dwSrcEncoding, uint dwDstEncoding);
        [PreserveSig]
        int ConvertString(ref uint pdwMode, uint dwSrcEncoding, uint dwDstEncoding, [MarshalAs(UnmanagedType.LPArray)] byte[] pSrcStr, ref uint pcSrcSize, [MarshalAs(UnmanagedType.LPArray)] byte[] pDstStr, ref uint pcDstSize);
        [PreserveSig]
        int ConvertStringToUnicode(ref uint pdwMode, uint dwEncoding, [MarshalAs(UnmanagedType.LPArray)] byte[] pSrcStr, ref uint pcSrcSize, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pDstStr, ref uint pcDstSize);
        [PreserveSig]
        int ConvertStringFromUnicode(ref uint pdwMode, uint dwEncoding, [MarshalAs(UnmanagedType.LPWStr)] string pSrcStr, ref uint pcSrcSize, [MarshalAs(UnmanagedType.LPArray)] byte[] pDstStr, ref uint pcDstSize);
        void ConvertStringReset();
        void GetRfc1766FromLcid(uint locale, [MarshalAs(UnmanagedType.BStr)] out string pbstrRfc1766);
        void GetLcidFromRfc1766(out uint plocale, [MarshalAs(UnmanagedType.BStr)] string bstrRfc1766);
        void EnumRfc1766(ushort LangId, [MarshalAs(UnmanagedType.Interface)] out IEnumRfc1766 ppEnumRfc1766);
        void GetRfc1766Info(uint locale, ushort LangId, out RFC1766INFO pRfc1766Info);
        void CreateConvertCharset(uint uiSrcCodePage, uint uiDstCodePage, uint dwProperty, [MarshalAs(UnmanagedType.Interface)] out IMLangConvertCharset ppMLangConvertCharset);
        [PreserveSig]
        int ConvertStringInIStream(ref uint pdwMode, uint dwFlag, [MarshalAs(UnmanagedType.LPWStr)] string lpFallBack, uint dwSrcEncoding, uint dwDstEncoding, [MarshalAs(UnmanagedType.Interface)] IStream pstmIn, [MarshalAs(UnmanagedType.Interface)] IStream pstmOut);
        [PreserveSig]
        int ConvertStringToUnicodeEx(ref uint pdwMode, uint dwEncoding, [MarshalAs(UnmanagedType.LPArray)] byte[] pSrcStr, ref uint pcSrcSize, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pDstStr, ref uint pcDstSize, uint dwFlag, [MarshalAs(UnmanagedType.LPWStr)] string lpFallBack);
        [PreserveSig]
        int ConvertStringFromUnicodeEx(ref uint pdwMode, uint dwEncoding, [MarshalAs(UnmanagedType.LPWStr)] string pSrcStr, ref uint pcSrcSize, [MarshalAs(UnmanagedType.LPArray)] byte[] pDstStr, ref uint pcDstSize, uint dwFlag, [MarshalAs(UnmanagedType.LPWStr)] string lpFallBack);
        [PreserveSig]
        int DetectCodepageInIStream([In] MLDETECTCP flags, uint dwPrefWinCodePage, [MarshalAs(UnmanagedType.Interface)] IStream pstmIn, [In, Out, MarshalAs(UnmanagedType.LPArray)] DetectEncodingInfo[] lpEncoding, ref int pnScores);
        [PreserveSig]
        int DetectInputCodepage(MLDETECTCP flags, uint dwPrefWinCodePage, [MarshalAs(UnmanagedType.LPArray)] byte[] pSrcStr, ref int pcSrcSize, [In, Out, MarshalAs(UnmanagedType.LPArray)] DetectEncodingInfo[] lpEncoding, ref int pnScores);
        void ValidateCodePage(uint uiCodePage, IntPtr hwnd);
        [PreserveSig]
        int GetCodePageDescription(uint uiCodePage, uint lcid, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpWideCharStr, int cchWideChar);
        [PreserveSig]
        int IsCodePageInstallable(uint uiCodePage);
        void SetMimeDBSource(MIMECONTF dwSource);
        uint GetNumberOfScripts();
        void EnumScripts(uint dwFlags, ushort LangId, [MarshalAs(UnmanagedType.Interface)] out IEnumScript ppEnumScript);
        void ValidateCodePageEx(uint uiCodePage, IntPtr hwnd, uint dwfIODControl);
        void DetectOutboundCodePage(MLCPF dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string lpWideCharStr, uint cchWideChar, IntPtr puiPreferredCodePages, uint nPreferredCodePages, [MarshalAs(UnmanagedType.LPArray)] int[] puiDetectedCodePages, ref uint pnDetectedCodePages, ref char lpSpecialChar);
        void DetectOutboundCodePageInIStream(MLCPF dwFlags, [MarshalAs(UnmanagedType.Interface)] IStream pStrIn, IntPtr puiPreferredCodePages, uint nPreferredCodePages, [MarshalAs(UnmanagedType.LPArray)] int[] puiDetectedCodePages, ref uint pnDetectedCodePages, ref char lpSpecialChar);
    }
}

