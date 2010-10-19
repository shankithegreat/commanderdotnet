namespace Nomad.FileSystem.Archive.Wcx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int ProcessFileHandler(IntPtr hArcData, PK_OPERATION Operation, [MarshalAs(UnmanagedType.LPStr)] string DestPath, [MarshalAs(UnmanagedType.LPStr)] string DestName);
}

