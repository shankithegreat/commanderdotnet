namespace Nomad.FileSystem.Archive.Wcx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int ChangeVolProcCallback([MarshalAs(UnmanagedType.LPStr)] string ArcName, PK_VOL Mode);
}

