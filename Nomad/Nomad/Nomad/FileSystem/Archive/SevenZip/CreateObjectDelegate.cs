namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int CreateObjectDelegate([In] ref Guid classID, [In] ref Guid interfaceID, out IntPtr outObject);
}

