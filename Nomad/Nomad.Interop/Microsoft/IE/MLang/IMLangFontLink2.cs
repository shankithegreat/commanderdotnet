namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("DCCFC162-2B38-11D2-B7EC-00C04F8F5D9A"), ComConversionLoss, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMLangFontLink2
    {
        void GetCharCodePages(char chSrc, out uint pdwCodePages);
        void GetStrCodePages([MarshalAs(UnmanagedType.LPWStr)] string pszSrc, int cchSrc, uint dwPriorityCodePages, out uint pdwCodePages, out int pcchCodePages);
        void CodePageToCodePages(uint uCodePage, out uint pdwCodePages);
        void CodePagesToCodePage(uint dwCodePages, uint uDefaultCodePage, out uint puCodePage);
        void GetFontCodePages(IntPtr hDC, IntPtr hFont, out uint pdwCodePages);
        void ReleaseFont(IntPtr hFont);
        void ResetFontMapping();
        void MapFont(IntPtr hDC, uint dwCodePages, char chSrc, out IntPtr pFont);
        void GetFontUnicodeRanges(IntPtr hDC, ref uint puiRanges, out UNICODERANGE pUranges);
        void GetScriptFontInfo(SCRIPTCONTF sid, SCRIPTFONTCONTF dwFlags, ref uint puiFonts, out SCRIPFONTINFO pScriptFont);
        void CodePageToScriptID(uint uiCodePage, out SCRIPTCONTF pSid);
    }
}

