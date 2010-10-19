namespace Nomad.FileSystem.Archive.Wcx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate bool CanYouHandleThisFileHandler([MarshalAs(UnmanagedType.LPStr)] string FileName);
}

