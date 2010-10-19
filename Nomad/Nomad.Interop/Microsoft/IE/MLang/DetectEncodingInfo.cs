namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct DetectEncodingInfo
    {
        public uint nLangID;
        public uint nCodePage;
        public int nDocPercent;
        public int nConfidence;
    }
}

