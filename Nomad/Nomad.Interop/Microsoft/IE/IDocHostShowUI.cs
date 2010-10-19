namespace Microsoft.IE
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("C4D244B0-D43E-11CF-893B-00AA00BDCE1A")]
    public interface IDocHostShowUI
    {
        void ShowMessage(int hwnd, ref int lpstrText, ref int lpstrCaption, uint dwType, ref int lpstrHelpFile, uint dwHelpContext, out int lpResult);
        void ShowHelp(uint hwnd, ref int pszHelpFile, uint uCommand, uint dwData, Point ptMouse, out object pDispatchObjectHit);
    }
}

