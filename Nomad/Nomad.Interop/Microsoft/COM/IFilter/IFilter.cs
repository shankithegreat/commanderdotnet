namespace Microsoft.COM.IFilter
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("89BCB740-6119-101A-BCB7-00DD010655AF")]
    public interface IFilter
    {
        [PreserveSig]
        int Init(IFILTER_INIT grfFlags, int cAttributes, IntPtr aAttributes, out IFILTER_FLAGS pdwFlags);
        [PreserveSig]
        int GetChunk(out STAT_CHUNK pStat);
        [PreserveSig]
        int GetText(ref uint pcwcBuffer, [Out, MarshalAs(UnmanagedType.LPArray)] char[] awcBuffer);
        [PreserveSig]
        int GetValue(ref IntPtr PropVal);
        [PreserveSig]
        int BindRegion(ref FILTERREGION origPos, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, ref object ppunk);
    }
}

