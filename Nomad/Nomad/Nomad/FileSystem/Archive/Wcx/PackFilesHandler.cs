namespace Nomad.FileSystem.Archive.Wcx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int PackFilesHandler([MarshalAs(UnmanagedType.LPStr)] string PackedFile, [MarshalAs(UnmanagedType.LPStr)] string SubPath, [MarshalAs(UnmanagedType.LPStr)] string SrcPath, [MarshalAs(UnmanagedType.LPStr)] string AddList, PK_PACK Flags);
}

