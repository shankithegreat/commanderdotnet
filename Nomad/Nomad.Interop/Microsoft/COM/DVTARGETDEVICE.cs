namespace Microsoft.COM
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DVTARGETDEVICE
    {
        public int tdSize;
        public short tdDriverNameOffset;
        public short tdDeviceNameOffset;
        public short tdPortNameOffset;
        public short tdExtDevmodeOffset;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)]
        public byte[] tdData;
    }
}

