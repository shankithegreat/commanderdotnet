namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("275C23E1-3747-11D0-9FEA-00AA003F8646")]
    public interface IMultiLanguage
    {
        uint GetNumberOfCodePageInfo();
        void GetCodePageInfo(uint uiCodePage, out MIMECPINFO pCodePageInfo);
        void GetFamilyCodePage(uint uiCodePage, out uint puiFamilyCodePage);
        void EnumCodePages(MIMECONTF grfFlags, [MarshalAs(UnmanagedType.Interface)] out IEnumCodePage ppEnumCodePage);
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
        void EnumRfc1766([MarshalAs(UnmanagedType.Interface)] out IEnumRfc1766 ppEnumRfc1766);
        void GetRfc1766Info(uint locale, out RFC1766INFO pRfc1766Info);
        void CreateConvertCharset(uint uiSrcCodePage, uint uiDstCodePage, uint dwProperty, [MarshalAs(UnmanagedType.Interface)] out IMLangConvertCharset ppMLangConvertCharset);
    }
}

