namespace Nomad.FileSystem.Archive.Wcx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int DeleteFilesHandler([MarshalAs(UnmanagedType.LPStr)] string PackedFile, [MarshalAs(UnmanagedType.LPStr)] string DeleteList);
}

