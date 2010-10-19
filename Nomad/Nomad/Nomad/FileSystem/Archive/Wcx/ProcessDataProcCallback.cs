namespace Nomad.FileSystem.Archive.Wcx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int ProcessDataProcCallback([MarshalAs(UnmanagedType.LPStr)] string FileName, int Size);
}

