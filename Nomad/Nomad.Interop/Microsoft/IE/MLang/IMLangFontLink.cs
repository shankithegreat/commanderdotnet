namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComConversionLoss, Guid("359F3441-BD4A-11D0-B188-00AA0038C969")]
    public interface IMLangFontLink
    {
        void GetCharCodePages(char chSrc, out uint pdwCodePages);
        void GetStrCodePages([MarshalAs(UnmanagedType.LPWStr)] string pszSrc, int cchSrc, uint dwPriorityCodePages, out uint pdwCodePages, out int pcchCodePages);
        void CodePageToCodePages(uint uCodePage, out uint pdwCodePages);
        void CodePagesToCodePage(uint dwCodePages, uint uDefaultCodePage, out uint puCodePage);
        void GetFontCodePages(IntPtr hDC, IntPtr hFont, out uint pdwCodePages);
        void MapFont(IntPtr hDC, uint dwCodePages, IntPtr hSrcFont, out IntPtr phDestFont);
        void ReleaseFont(IntPtr hFont);
        void ResetFontMapping();
    }
}

