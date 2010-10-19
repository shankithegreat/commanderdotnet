namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [ComImport, Guid("D66D6F98-CDAA-11D0-B822-00C04FC9B31F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMLangConvertCharset
    {
        [PreserveSig]
        int Initialize(uint uiSrcCodePage, uint uiDstCodePage, MLCONVCHAR dwProperty);
        uint GetSourceCodePage();
        uint GetDestinationCodePage();
        MLCONVCHAR GetProperty();
        [PreserveSig]
        int DoConversion([MarshalAs(UnmanagedType.LPArray)] byte[] pSrcStr, ref uint pcSrcSize, [MarshalAs(UnmanagedType.LPArray)] byte[] pDstStr, ref uint pcDstSize);
        [PreserveSig]
        int DoConversionToUnicode([MarshalAs(UnmanagedType.LPArray)] byte[] pSrcStr, ref uint pcSrcSize, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pDstStr, ref uint pcDstSize);
        [PreserveSig]
        int DoConversionFromUnicode([MarshalAs(UnmanagedType.LPWStr)] string pSrcStr, ref uint pcSrcSize, [MarshalAs(UnmanagedType.LPArray)] byte[] pDstStr, ref uint pcDstSize);
    }
}

